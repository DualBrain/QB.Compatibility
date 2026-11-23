Option Explicit On
Option Strict On
Option Infer On

Imports System.Text

Namespace QB2VB

  Partial Friend Class Converter

    ' ---- Statement lists ------------------------------------------------------------------------------------

    Private Function ParseStatementList(t As List(Of Token), start As Integer, [end] As Integer) As List(Of String)
      Dim result As New List(Of String)
      Dim s = start
      Do While s < [end]
        ' find the end of this statement (a top-level colon), unless it's an IF, which takes the rest of the line
        Dim e = s
        Dim isIf = t(s).IsWord("IF")
        If Not isIf Then
          Dim depth = 0
          Do While e < [end]
            If t(e).Is_("(") Then depth += 1
            If t(e).Is_(")") Then depth -= 1
            If depth = 0 AndAlso t(e).Is_(":") Then Exit Do
            e += 1
          Loop
        Else
          e = [end]
        End If
        If e > s Then
          Dim text = Statement(t, s, e)
          If text <> "" Then result.Add(text)
        End If
        s = e + 1
      Loop
      ' An empty loop on one line (FOR ...: NEXT) is a delay: EMPTYLOOP gives each pass some time.
      For k = result.Count - 2 To 0 Step -1
        If result(k).StartsWith("For ", StringComparison.Ordinal) AndAlso result(k + 1).StartsWith("Next", StringComparison.Ordinal) Then result.Insert(k + 1, "EMPTYLOOP()")
      Next
      Return result
    End Function

    Private Function Cond(t As List(Of Token), s As Integer, e As Integer) As String
      Dim ep As New ExprParser(t.GetRange(s, e - s), 0, Me)
      Dim x = ep.ParseExpr()
      If Not ep.AtEnd() Then Throw New ConvertException("unexpected text in the condition")
      Return ExprParser.AsBool(x)
    End Function

    ' A loop whose condition reads TIMER waits for the clock (or counts while it does): each pass gets EMPTYLOOP's time, so counts are measurable.
    Private Shared Function PollsTimer(t As List(Of Token), s As Integer, e As Integer) As Boolean
      For k = s To e - 1
        If t(k).IsWord("TIMER") Then Return True
      Next
      Return False
    End Function

    Private Function Expr(t As List(Of Token), s As Integer, e As Integer) As E
      Dim ep As New ExprParser(t.GetRange(s, e - s), 0, Me)
      Dim x = ep.ParseExpr()
      If Not ep.AtEnd() Then Throw New ConvertException("unexpected text in the expression")
      Return x
    End Function

    Private Function LabelText(tok As Token) As String
      Return tok.Text
    End Function

    ' ---- One statement -----------------------------------------------------------------------------------

    Private Function Statement(t As List(Of Token), s As Integer, e As Integer) As String

      Dim w = t(s).Upper()
      Dim n = e - s

      If t(s).Kind = TokKind.Ident Then
        Select Case w

          Case "LET"
            Return Statement(t, s + 1, e)

          Case "REM"
            Return ""

          Case "IF"
            Return IfStatement(t, s, e)

          Case "ELSEIF"
            Dim th = FindWord(t, s, e, "THEN")
            If th < 0 Then Throw New ConvertException("ELSEIF without THEN")
            Return "ElseIf " & Cond(t, s + 1, th) & " Then"

          Case "ELSE"
            Return "Else"

          Case "FOR"
            Return ForStatement(t, s, e)

          Case "NEXT"
            Dim names As New List(Of String)
            For Each part In SplitTopLevel(t, s + 1, e)
              If part.Count > 0 Then names.Add(ExprParser.VarText(ResolveVariableCore(part(0).Text, False)))
            Next
            Return "Next" & If(names.Count > 0, " " & String.Join(", ", names), "")

          Case "WHILE"
            Return "While " & Cond(t, s + 1, e) & If(PollsTimer(t, s + 1, e), " : EMPTYLOOP()", "")
          Case "WEND"
            Return "End While"

          Case "DO"
            If n = 1 Then Return "Do"
            If t(s + 1).IsWord("WHILE") Then Return "Do While " & Cond(t, s + 2, e) & If(PollsTimer(t, s + 2, e), " : EMPTYLOOP()", "")
            If t(s + 1).IsWord("UNTIL") Then Return "Do Until " & Cond(t, s + 2, e) & If(PollsTimer(t, s + 2, e), " : EMPTYLOOP()", "")
          Case "LOOP"
            If n = 1 Then Return "Loop"
            If t(s + 1).IsWord("WHILE") Then Return If(PollsTimer(t, s + 2, e), "EMPTYLOOP() : ", "") & "Loop While " & Cond(t, s + 2, e)
            If t(s + 1).IsWord("UNTIL") Then Return If(PollsTimer(t, s + 2, e), "EMPTYLOOP() : ", "") & "Loop Until " & Cond(t, s + 2, e)

          Case "SELECT"
            If n > 1 AndAlso t(s + 1).IsWord("CASE") Then
              Dim x = Expr(t, s + 2, e)
              m_selectType = x.Vb
              m_selectStack.Push(x.Vb)
              Return "Select Case " & x.Text
            End If
          Case "CASE"
            Return CaseStatement(t, s, e)

          Case "END"
            If n = 1 Then Return "End"
            If t(s + 1).IsWord("IF") Then Return "End If"
            If t(s + 1).IsWord("SELECT") Then
              If m_selectStack.Count > 0 Then m_selectStack.Pop()
              Return "End Select"
            End If
          Case "SYSTEM", "STOP"
            Return "End"

          Case "EXIT"
            If n = 2 Then
              Select Case t(s + 1).Upper()
                Case "SUB" : Return CopyBackText() & "Exit Sub"
                Case "FUNCTION" : Return CopyBackText() & "Exit Function"
                Case "DO" : Return "Exit Do"
                Case "FOR" : Return "Exit For"
              End Select
            End If

          Case "GOTO"
            CheckJump(t(s + 1).Text)
            Return "GoTo " & NeedLabel(t(s + 1))

          Case "GOSUB"
            If m_curProc IsNot Nothing AndAlso Not m_procGosubTargets.Contains(t(s + 1).Text) Then Throw New ConvertException("GOSUB to a label outside the procedure")
            If m_curProc IsNot Nothing AndAlso m_inlineRoutines.Contains(t(s + 1).Text) Then Return InlineMarker & t(s + 1).Text
            Return GosubName(t(s + 1).Text) & "()"
          Case "RETURN"
            If m_inlineEnd >= 0 AndAlso n = 1 Then
              If m_emitIndex = m_inlineEnd Then Return ""
              m_inlineUsedLabel = True
              Return "GoTo " & m_inlineLabel
            End If
            If Not m_inGosub OrElse n <> 1 Then Throw New ConvertException("RETURN outside a converted GOSUB routine")
            Return "Return"

          Case "DEF"
            If n >= 2 AndAlso t(s + 1).IsWord("SEG") Then
              If n = 2 Then Return "DEF_SEG()"
              If n > 3 AndAlso t(s + 2).Is_("=") Then Return "DEF_SEG(" & ExprParser.Coerce(Expr(t, s + 3, e), QT.Long) & ")"
            End If
            If FindProcCore(t(s + 1).Text) IsNot Nothing AndAlso FindProcCore(t(s + 1).Text).IsDefFn Then Return EmitDefFn(FindProcCore(t(s + 1).Text))
            Throw New ConvertException("this form of DEF isn't converted yet")

          Case "VIEW"
            If n >= 2 AndAlso t(s + 1).IsWord("SCREEN") Then Return ViewWindowStatement("VIEW_SCREEN", t, s, e, 2)
            If n >= 2 AndAlso Not t(s + 1).IsWord("PRINT") Then Return ViewWindowStatement("VIEW", t, s, e)
            If n >= 2 AndAlso t(s + 1).IsWord("PRINT") Then
              If n = 2 Then Return "VIEW_PRINT()"
              Dim toIdx = FindWord(t, s, e, "TO")
              If toIdx > 0 Then Return "VIEW_PRINT(" & ExprParser.Coerce(Expr(t, s + 2, toIdx), QT.Long) & ", " & ExprParser.Coerce(Expr(t, toIdx + 1, e), QT.Long) & ")"
            End If

          Case "ON"
            If n >= 3 AndAlso {"TIMER", "PLAY", "KEY", "STRIG", "PEN", "COM"}.Contains(t(s + 1).Upper()) Then Return OnEventStatement(t, s, e)
            If n >= 4 AndAlso t(s + 1).IsWord("ERROR") AndAlso t(s + 2).IsWord("GOTO") Then
              Dim lab = t(s + 3).Text
              If lab = "0" Then Return "On Error GoTo 0"
              Return "On Error GoTo " & NeedLabel(t(s + 3), True)
            End If
            If n >= 4 AndAlso t(s + 1).IsWord("ERROR") AndAlso t(s + 2).IsWord("RESUME") Then
              Return "On Error Resume Next"
            End If
            Dim jump = FindWord(t, s, e, "GOTO")
            Dim isGosub = False
            If jump < 0 Then jump = FindWord(t, s, e, "GOSUB") : isGosub = True
            If jump > 0 Then
              If isGosub AndAlso m_curProc IsNot Nothing AndAlso Not m_procGosubTargets.Contains(t(s + 3).Text) AndAlso False Then Throw New ConvertException("GOSUB to a label outside the procedure")
              Dim sel = ExprParser.Coerce(Expr(t, s + 1, jump), QT.Long)
              Dim sb = New StringBuilder("Select Case " & sel)
              Dim idx = 1
              For Each part In SplitTopLevel(t, jump + 1, e)
                If part.Count = 0 Then Continue For
                sb.Append(" : Case " & idx & " : " & If(isGosub, GosubName(part(0).Text) & "()", "GoTo " & part(0).Text))
                idx += 1
              Next
              sb.Append(" : End Select")
              Return sb.ToString
            End If
            Throw New ConvertException("ON " & If(n > 1, t(s + 1).Text, "") & " isn't converted yet")

          Case "RESUME"
            If n = 1 Then Return "Resume"
            If t(s + 1).IsWord("NEXT") Then Return "Resume Next"
            If t(s + 1).Text = "0" Then Return "Resume"
            Return "Resume " & NeedLabel(t(s + 1))

          Case "CALL"
            Return CallStatement(t, s + 1, e)

          Case "PRINT"
            Return PrintStatement(t, s, e)

          Case "INPUT"
            Return InputStatement(t, s, e)

          Case "LINE"
            If n > 1 AndAlso t(s + 1).IsWord("INPUT") Then Return LineInput(t, s, e)
            Return LineStatement(t, s, e)
          Case "PSET", "PRESET"
            Return PointStatement(t, s, e)
          Case "CIRCLE"
            Return CircleStatement(t, s, e)
          Case "PAINT"
            Return PaintStatement(t, s, e)
          Case "GET", "PUT"
            Return GetPutStatement(t, s, e)
          Case "PALETTE"
            m_usesGraphics = True
            If n > 1 AndAlso t(s + 1).IsWord("USING") Then Return "PALETTE_USING(" & ArrayArgs(t.GetRange(s + 2, n - 2)) & ")"
            Return LibraryStatement(t, s, e)

          Case "OPEN"
            Return OpenStatement(t, s, e)

          Case "FIELD"
            Dim recName As String = Nothing
            If Not m_fieldRecords.TryGetValue(m_emitIndex, recName) Then Throw New ConvertException("FIELD in an unexpected place")
            Dim fparts = SplitTopLevel(t, s + 1, e)
            Dim fnum = fparts(0)
            If fnum.Count > 0 AndAlso fnum(0).Is_("#") Then fnum = fnum.GetRange(1, fnum.Count - 1)
            Dim args As New List(Of String) From {ExprParser.Coerce(Expr(fnum, 0, fnum.Count), QT.Long)}
            For fk = 1 To fparts.Count - 1
              Dim fp = fparts(fk)
              Dim asAt = FindWord(fp, 0, fp.Count, "AS")
              If asAt < 0 Then Throw New ConvertException("FIELD needs width AS name")
              args.Add(ExprParser.Coerce(Expr(fp, 0, asAt), QT.Long))
            Next
            Return recName & " = FIELD(" & String.Join(", ", args) & ")"

          Case "LSET", "RSET"
            Dim eqAt = -1
            For k = s + 1 To e - 1
              If t(k).Is_("=") Then eqAt = k : Exit For
            Next
            If eqAt < 0 Then Throw New ConvertException(w & " needs =")
            Dim target = ResolveVariableCore(t(s + 1).Text, False)
            Dim value = Expr(t, eqAt + 1, e).Text
            If target.FieldVar <> "" Then Return target.FieldVar & "." & w & "(" & target.FieldIndex & ", " & value & ")"
            Return w & "(" & Expr(t, s + 1, eqAt).Text & ", " & value & ")"

          Case "LPRINT"
            Return PrintStatement(t, s, e, True)

          Case "WIDTH"
            If n >= 4 AndAlso t(s + 1).Is_("#") Then
              Dim parts = SplitTopLevel(t, s + 2, e)
              If parts.Count = 2 Then Return "WIDTH_FILE(" & ExprParser.Coerce(Expr(parts(0), 0, parts(0).Count), QT.Long) & ", " & ExprParser.Coerce(Expr(parts(1), 0, parts(1).Count), QT.Long) & ")"
            End If
            If n >= 3 AndAlso t(s + 1).IsWord("LPRINT") Then Return "WIDTH_LPRINT(" & ExprParser.Coerce(Expr(t, s + 2, e), QT.Long) & ")"
            Return LibraryStatement(t, s, e)

          Case "WRITE"
            Dim wp = s + 1
            Dim wfile = ""
            If wp < e AndAlso t(wp).Is_("#") Then
              Dim wcomma = -1
              For k = wp To e - 1
                If t(k).Is_(",") Then wcomma = k : Exit For
              Next
              If wcomma < 0 Then Throw New ConvertException("malformed WRITE #")
              wfile = ExprParser.Coerce(Expr(t, wp + 1, wcomma), QT.Long)
              wp = wcomma + 1
            End If
            Dim wvals As New List(Of String)
            For Each part In SplitTopLevel(t, wp, e)
              If part.Count > 0 Then wvals.Add(Expr(part, 0, part.Count).Text)
            Next
            Return "WRITE(" & String.Join(", ", If(wfile <> "", New List(Of String) From {wfile}, New List(Of String)).Concat(wvals)) & ")"

          Case "TIMER", "PEN"
            If w = "PEN" Then m_usesGraphics = True
            If n = 2 AndAlso {"ON", "OFF", "STOP"}.Contains(t(s + 1).Upper()) Then Return w & "_" & t(s + 1).Upper() & "()"
            Return LibraryStatement(t, s, e)
          Case "PLAY"
            m_usesSound = True
            If n = 2 AndAlso {"ON", "OFF", "STOP"}.Contains(t(s + 1).Upper()) Then Return "PLAY_" & t(s + 1).Upper() & "()"
            Return LibraryStatement(t, s, e)
          Case "STRIG", "COM"
            If w = "STRIG" Then m_usesGraphics = True
            If w = "COM" Then m_usesSerial = True
            If n >= 4 AndAlso t(s + 1).Is_("(") AndAlso {"ON", "OFF", "STOP"}.Contains(t(e - 1).Upper()) Then Return w & "_" & t(e - 1).Upper() & "(" & ExprParser.Coerce(Expr(t, s + 2, e - 2), QT.Long) & ")"
            Return LibraryStatement(t, s, e)
          Case "KEY"
            If n = 2 AndAlso {"ON", "OFF", "LIST"}.Contains(t(s + 1).Upper()) Then Return "KEY_" & t(s + 1).Upper() & "()"
            If n >= 4 AndAlso t(s + 1).Is_("(") AndAlso {"ON", "OFF", "STOP"}.Contains(t(e - 1).Upper()) Then Return "KEY_" & t(e - 1).Upper() & "(" & ExprParser.Coerce(Expr(t, s + 2, e - 2), QT.Long) & ")"
            Dim kparts = SplitTopLevel(t, s + 1, e)
            If kparts.Count = 2 AndAlso kparts(0).Count > 0 AndAlso kparts(1).Count > 0 Then
              Dim kn = Expr(kparts(0), 0, kparts(0).Count)
              Dim ktext = Expr(kparts(1), 0, kparts(1).Count)
              ' KEY 15 to 25 define a key from two characters: the keyboard flag and the scan code.
              If kn.IsLiteral AndAlso kn.LiteralValue >= 15 Then Return "KEY_DEFINE(" & ExprParser.Coerce(kn, QT.Long) & ", Asc(Left$(" & ktext.Text & ", 1)), Asc(Mid$(" & ktext.Text & ", 2, 1)))"
              Return "KEY_SET(" & ExprParser.Coerce(kn, QT.Long) & ", " & ktext.Text & ")"
            End If
            Throw New ConvertException("this form of KEY isn't converted yet")

          Case "CLOSE"
            If n = 1 Then Return "CLOSE()"
            Return "CLOSE(" & ArgList(t, s + 1, e) & ")"

          Case "NAME"
            Dim asIdx = FindWord(t, s, e, "AS")
            If asIdx > 0 Then Return "NAME(" & Expr(t, s + 1, asIdx).Text & ", " & Expr(t, asIdx + 1, e).Text & ")"

          Case "READ"
            Dim items As New List(Of String)
            For Each part In SplitTopLevel(t, s + 1, e)
              Dim target = Expr(part, 0, part.Count)
              items.Add("READ(" & target.Text & ")")
              If target.FixedLen > 0 Then items.Add(FitStatement(target))
            Next
            Return String.Join(" : ", items)

          Case "RESTORE"
            If n = 1 Then Return "RESTORE()"
            Return "RESTORE(" & ExprParser.Quote(t(s + 1).Text) & ")"

          Case "SWAP"
            Dim parts = SplitTopLevel(t, s + 1, e)
            If parts.Count = 2 Then Return "SWAP(" & Expr(parts(0), 0, parts(0).Count).Text & ", " & Expr(parts(1), 0, parts(1).Count).Text & ")"

          Case "MID"
            ' MID$(s$, p[, n]) = x$
            If n > 1 AndAlso t(s + 1).Is_("(") Then Return Assignment(t, s, e)

          Case "WINDOW"
            If n >= 2 AndAlso Not t(s + 1).IsWord("SCREEN") Then Return ViewWindowStatement("WINDOW", t, s, e)
            If n >= 2 Then Return ViewWindowStatement("WINDOW_SCREEN", t, s, e, 2)
            Return "WINDOW()"

          Case "SHARED", "STATIC", "DIM", "REDIM", "COMMON"
            If w = "SHARED" Then Return ""
            ' (The statement is the tokens s to e: several DIMs can be on one line.)
            Dim dimTokens = t.GetRange(s, e - s)
            If w = "STATIC" Then Return DimText(dimTokens, "Static", False)
            Return DimText(dimTokens, If(w = "REDIM", "ReDim", "Dim"), False)

          Case Else
            ' A call to a procedure, a library statement, or an assignment.
            If Keywords.IsStatementWord(w) AndAlso FindProcCore(t(s).Text) Is Nothing Then
              Return LibraryStatement(t, s, e)
            End If
        End Select
      End If

      ' name = expr, name(i) = expr, rec.f = expr, MID$(..) = expr
      For k = s To e - 1
        If t(k).Is_("=") Then Return Assignment(t, s, e)
        If t(k).Is_("(") Then
          Dim depth = 0
          Do While k < e
            If t(k).Is_("(") Then depth += 1
            If t(k).Is_(")") Then depth -= 1
            If depth = 0 Then Exit Do
            k += 1
          Loop
        End If
      Next

      ' a procedure call without CALL
      If FindProcCore(t(s).Text) IsNot Nothing Then Return CallStatement(t, s, e, True)

      Throw New ConvertException("unrecognized statement """ & t(s).Text & """")

    End Function

    ' The VB name of the procedure that a GOSUB label becomes.
    Private Function GosubName(label As String) As String
      If m_curProc IsNot Nothing Then
        Dim existing As String = Nothing
        If m_routineNames.TryGetValue(label, existing) Then Return existing
        Dim rn = If(Char.IsDigit(label(0)), "L" & label, label)
        If Keywords.IsVbReserved(rn) Then rn = "[" & rn & "]"
        Do While m_scope.Names.Contains(rn.ToLowerInvariant()) OrElse m_module.Names.Contains(rn.ToLowerInvariant()) OrElse m_procs.ContainsKey(rn)
          rn &= "_Sub"
        Loop
        m_scope.Names.Add(rn.ToLowerInvariant())
        m_routineNames(label) = rn
        Return rn
      End If
      Dim n = If(Char.IsDigit(label(0)), "L" & label, label)
      If Keywords.IsVbReserved(n) Then n = "[" & n & "]"
      If m_procs.ContainsKey(n) OrElse m_module.Names.Contains(n.ToLowerInvariant()) OrElse m_main.Names.Contains(n.ToLowerInvariant()) Then n &= "_Sub"
      Return n
    End Function

    Friend Const InlineMarker As String = ChrW(1) & "INLINE:"

    ' Before leaving a procedure whose GOSUB routines use copies of its parameters, the copies go back.
    Private Function CopyBackText() As String
      If m_copyBack.Count = 0 OrElse m_inGosub Then Return ""
      Return String.Join(" : ", m_copyBack) & " : "
    End Function

    Private m_selectType As QT
    Private ReadOnly m_selectStack As New Stack(Of QT)

    Private Function FindWord(t As List(Of Token), s As Integer, e As Integer, word As String) As Integer
      Dim depth = 0
      For k = s To e - 1
        If t(k).Is_("(") Then depth += 1
        If t(k).Is_(")") Then depth -= 1
        If depth = 0 AndAlso t(k).IsWord(word) Then Return k
      Next
      Return -1
    End Function

    ' A GOTO has to stay in the routine it is in: a GOSUB routine is a procedure of its own in VB, and so is what is outside it.
    Private Sub CheckJump(label As String)
      If m_curProc IsNot Nothing Then Return
      Dim owner As Integer
      If Not m_labelOwner.TryGetValue(label, owner) Then Return   ' (a label that isn't in the program: the compiler will say)
      Dim here = If(m_inGosub, m_gosubStart, -1)
      If owner <> here Then Throw New ConvertException("GOTO " & label & " jumps " & If(m_inGosub, "out of", "into") & " a GOSUB routine, which VB can't do")
    End Sub

    ' A label used by GOTO and friends.  A module-level label that a procedure's ON ERROR names has its handler copied in.
    Private Function NeedLabel(tok As Token, Optional handler As Boolean = False) As String
      If handler AndAlso (m_curProc IsNot Nothing OrElse m_inGosub) AndAlso m_mainLabels.ContainsKey(tok.Text) AndAlso Not m_neededHandlers.Contains(tok.Text, StringComparer.OrdinalIgnoreCase) Then
        m_neededHandlers.Add(tok.Text)
      End If
      Return tok.Text
    End Function

    ' ---- IF / FOR / SELECT -------------------------------------------------------------------------------------

    Private Function IfStatement(t As List(Of Token), s As Integer, e As Integer) As String
      Dim th = FindWord(t, s, e, "THEN")
      If th < 0 Then
        ' IF cond GOTO label
        Dim g = FindWord(t, s, e, "GOTO")
        If g > 0 Then Return "If " & Cond(t, s + 1, g) & " Then GoTo " & t(g + 1).Text
        Throw New ConvertException("IF without THEN")
      End If
      Dim c = Cond(t, s + 1, th)
      If th = e - 1 Then Return "If " & c & " Then"

      ' inline: IF c THEN a : b ELSE c : d
      Dim elseIdx = -1
      Dim depth = 0
      Dim nested = 0
      For k = th + 1 To e - 1
        If t(k).IsWord("IF") Then nested += 1
        If t(k).IsWord("ELSE") Then
          If nested > 0 Then nested -= 1 Else elseIdx = k : Exit For
        End If
      Next
      Dim thenEnd = If(elseIdx >= 0, elseIdx, e)
      Dim thenPart As List(Of String)
      If t(th + 1).Kind = TokKind.Number Then
        thenPart = New List(Of String) From {"GoTo " & t(th + 1).Text}
      Else
        thenPart = ParseStatementList(t, th + 1, thenEnd)
      End If
      Dim text = "If " & c & " Then " & String.Join(" : ", thenPart)
      If elseIdx >= 0 Then
        Dim elsePart As List(Of String)
        If elseIdx + 1 < e AndAlso t(elseIdx + 1).Kind = TokKind.Number Then
          elsePart = New List(Of String) From {"GoTo " & t(elseIdx + 1).Text}
        Else
          elsePart = ParseStatementList(t, elseIdx + 1, e)
        End If
        text &= " Else " & String.Join(" : ", elsePart)
      End If
      Return text
    End Function

    Private Function ForStatement(t As List(Of Token), s As Integer, e As Integer) As String
      Dim eq = FindWord(t, s, e, "=")
      For k = s To e - 1
        If t(k).Is_("=") Then eq = k : Exit For
      Next
      Dim toIdx = FindWord(t, s, e, "TO")
      If eq < 0 OrElse toIdx < 0 Then Throw New ConvertException("malformed FOR")
      Dim stepIdx = FindWord(t, s, e, "STEP")
      Dim v = ResolveVariableCore(t(s + 1).Text, False)
      Dim a = Expr(t, eq + 1, toIdx)
      Dim b = Expr(t, toIdx + 1, If(stepIdx > 0, stepIdx, e))
      Dim text = "For " & ExprParser.VarText(v) & " = " & ExprParser.Coerce(a, v.Qt) & " To " & ExprParser.Coerce(b, v.Qt)
      If stepIdx > 0 Then text &= " Step " & ExprParser.Coerce(Expr(t, stepIdx + 1, e), v.Qt)
      Return text
    End Function

    Private Function CaseStatement(t As List(Of Token), s As Integer, e As Integer) As String
      If e - s = 2 AndAlso t(s + 1).IsWord("ELSE") Then Return "Case Else"
      Dim sel = If(m_selectStack.Count > 0, m_selectStack.Peek(), QT.Unknown)
      Dim items As New List(Of String)
      For Each part In SplitTopLevel(t, s + 1, e)
        If part.Count = 0 Then Continue For
        If part(0).IsWord("IS") Then
          Dim op = part(1).Text
          Dim x = Expr(part, 2, part.Count)
          items.Add("Is " & op & " " & CaseValue(x, sel))
        Else
          Dim toIdx = FindWord(part, 0, part.Count, "TO")
          If toIdx > 0 Then
            items.Add(CaseValue(Expr(part, 0, toIdx), sel) & " To " & CaseValue(Expr(part, toIdx + 1, part.Count), sel))
          Else
            items.Add(CaseValue(Expr(part, 0, part.Count), sel))
          End If
        End If
      Next
      Return "Case " & String.Join(", ", items)
    End Function

    Private Shared Function CaseValue(x As E, sel As QT) As String
      If sel = QT.String OrElse sel = QT.Unknown OrElse x.Qt = QT.String Then Return x.Text
      Return ExprParser.Coerce(x, sel)
    End Function

    ' ---- Assignment ------------------------------------------------------------------------------------

    Private Function Assignment(t As List(Of Token), s As Integer, e As Integer) As String
      Dim eq = -1
      Dim depth = 0
      For k = s To e - 1
        If t(k).Is_("(") Then depth += 1
        If t(k).Is_(")") Then depth -= 1
        If depth = 0 AndAlso t(k).Is_("=") Then eq = k : Exit For
      Next
      If eq < 0 Then Throw New ConvertException("assignment without =")

      Dim rhs = Expr(t, eq + 1, e)

      ' MID$(s$, p[, n]) = x$
      If t(s).Upper() = "MID$" OrElse t(s).Upper() = "MID" Then
        Dim lhsParts = SplitTopLevel(t, s + 2, eq - 1)
        Dim texts As New List(Of String)
        Dim idx = 0
        For Each part In lhsParts
          Dim x = Expr(part, 0, part.Count)
          texts.Add(If(idx = 0, x.Text, ExprParser.Coerce(x, QT.Long)))
          idx += 1
        Next
        Return "Mid(" & String.Join(", ", texts) & ") = " & rhs.Text
      End If

      ' the function's own name: the return value
      If m_curProc IsNot Nothing AndAlso m_curProc.IsFunction AndAlso eq = s + 1 Then
        Dim hs As Boolean, bare As String = ""
        TypeOfName(t(s).Text, hs, bare)
        If bare.Equals(m_curProc.QbName, StringComparison.OrdinalIgnoreCase) Then
          Return ExprParser.ProcName(m_curProc) & " = " & ExprParser.Coerce(rhs, m_curProc.RetQt)
        End If
      End If

      If eq = s + 1 AndAlso t(s).Kind = TokKind.Ident AndAlso ResolveVariableCore(t(s).Text, False).FieldVar <> "" Then
        Throw New ConvertException("assigning to a FIELD variable unbinds it in QBasic; use LSET or RSET")
      End If
      Dim lhs = Expr(t, s, eq)
      Dim target = lhs.Vb
      ' STRING * n: what is assigned is padded or cut to n characters.
      If lhs.FixedLen > 0 Then Return lhs.Text & " = FixedString.Fit(" & ExprParser.Coerce(rhs, target) & ", " & lhs.FixedLen & ")"
      Return lhs.Text & " = " & ExprParser.Coerce(rhs, target)
    End Function

    ' ---- Calls ----------------------------------------------------------------------------------------

    Private Function CallStatement(t As List(Of Token), s As Integer, e As Integer, Optional noParens As Boolean = False) As String
      Dim sym = FindProcCore(t(s).Text)
      If sym Is Nothing Then Throw New ConvertException("unknown procedure " & t(s).Text)
      Dim argStart = s + 1
      Dim argEnd = e
      If argStart < e AndAlso t(argStart).Is_("(") AndAlso t(e - 1).Is_(")") AndAlso Not noParens Then
        argStart += 1 : argEnd -= 1
      End If
      Dim texts As New List(Of String)
      Dim idx = 0
      For Each part In SplitTopLevel(t, argStart, argEnd)
        If part.Count = 0 Then Continue For
        Dim x = Expr(part, 0, part.Count)
        texts.Add(ArgFor(x, sym, idx))
        idx += 1
      Next
      Return ExprParser.ProcName(sym) & "(" & String.Join(", ", texts) & ")"
    End Function

    Private Function ArgFor(x As E, sym As ProcSym, idx As Integer) As String
      If idx >= sym.Params.Count Then Return x.Text
      Dim prm = sym.Params(idx)
      If prm.IsArray Then Return x.Text
      Dim want = If(prm.HasSuffix OrElse prm.Qt <> QT.Unknown, prm.Qt, QT.Single)
      If x.Vb = want Then Return x.Text
      If x.IsLValue Then Note("argument " & x.Text & " is passed by value because its type differs from the parameter")
      Return ExprParser.Coerce(x, want)
    End Function

    ' Arguments of a library statement: numbers go as whole numbers, strings as strings.
    Private Function ArgList(t As List(Of Token), s As Integer, e As Integer, Optional numeric As QT = QT.Long) As String
      Dim texts As New List(Of String)
      For Each part In SplitTopLevel(t, s, e)
        If part.Count > 0 AndAlso part(0).Is_("#") Then part = part.GetRange(1, part.Count - 1)
        If part.Count = 0 Then
          texts.Add("-1")
        Else
          Dim x = Expr(part, 0, part.Count)
          If x.Qt = QT.String OrElse x.Vb = QT.Short OrElse x.Vb = QT.Long Then texts.Add(x.Text) Else texts.Add(ExprParser.Coerce(x, numeric))
        End If
      Next
      Return String.Join(", ", texts)
    End Function

    Private Function LibraryStatement(t As List(Of Token), s As Integer, e As Integer) As String
      Dim name = t(s).Upper()
      Dim p = s + 1
      Select Case name
        Case "DRAW", "PALETTE", "PCOPY", "STRIG", "PEN" : m_usesGraphics = True
        Case "SOUND", "PLAY" : m_usesSound = True
        Case "SCREEN"
          ' SCREEN 0 is the text screen; any other mode (or one that isn't known) needs graphics.
          If Not (p < e AndAlso t(p).Kind = TokKind.Number AndAlso t(p).Text = "0") Then m_usesGraphics = True
      End Select
      Select Case name
        Case "VIEW", "KEY", "WIDTH", "ERROR", "ENVIRON", "DEF", "SOUND", "PLAY", "DRAW", "PSET", "PRESET", "CIRCLE", "PAINT", "GET", "PUT", "WINDOW", "PALETTE", "PCOPY", "OUT", "WAIT", "LOCK", "UNLOCK", "TIMER", "STRIG", "PEN", "COM", "LPRINT", "LOCATE", "COLOR", "CLS", "RANDOMIZE", "BEEP", "SCREEN", "KILL", "MKDIR", "CHDIR", "RMDIR", "FILES", "SHELL", "POKE", "BLOAD", "BSAVE", "SEEK", "WRITE", "FIELD", "LSET", "RSET", "RUN", "CHAIN", "CLEAR", "ERASE", "FILEATTR", "ON", "RESET", "TROFF", "TRON"
          ' fall through to the generic form below
        Case Else
      End Select
      Select Case name
        Case "WRITE", "DEF", "KEY", "ON", "ERASE", "CHAIN", "RUN", "COMMON"
          Throw New ConvertException(name & " isn't converted yet")
      End Select
      If p >= e Then Return name & "()"
      Return name & "(" & ArgList(t, p, e) & ")"
    End Function

    ' ---- Graphics ---------------------------------------------------------------------------------------

    ''' <summary>[STEP] (x, y) starting at token <paramref name="p"/>; leaves p after the closing parenthesis.</summary>
    Private Function ParseCoord(t As List(Of Token), ByRef p As Integer, e As Integer, ByRef isStep As Boolean, ByRef x As String, ByRef y As String) As Boolean
      isStep = False
      If p < e AndAlso t(p).IsWord("STEP") Then isStep = True : p += 1
      If p >= e OrElse Not t(p).Is_("(") Then Return False
      Dim depth = 0
      Dim q = p
      Do While q < e
        If t(q).Is_("(") Then depth += 1
        If t(q).Is_(")") Then depth -= 1
        If depth = 0 Then Exit Do
        q += 1
      Loop
      Dim parts = SplitTopLevel(t, p + 1, q)
      If parts.Count <> 2 Then Throw New ConvertException("a coordinate needs (x, y)")
      x = Expr(parts(0), 0, parts(0).Count).Text
      y = Expr(parts(1), 0, parts(1).Count).Text
      p = q + 1
      Return True
    End Function

    ' The arguments after the coordinates, split at top-level commas, keeping empty ones.
    Private Function TailParts(t As List(Of Token), p As Integer, e As Integer) As List(Of List(Of Token))
      If p < e AndAlso t(p).Is_(",") Then p += 1 Else If p >= e Then Return New List(Of List(Of Token))
      Return SplitTopLevel(t, p, e)
    End Function

    Private Function TailExpr(parts As List(Of List(Of Token)), index As Integer) As E
      If index >= parts.Count OrElse parts(index).Count = 0 Then Return Nothing
      Return Expr(parts(index), 0, parts(index).Count)
    End Function

    Private Function IntText(x As E, Optional empty As String = "-1") As String
      If x Is Nothing Then Return empty
      Return ExprParser.Coerce(x, QT.Long)
    End Function

    Private Shared Function Trimmed(args As List(Of String), emptyValue As String) As List(Of String)
      Dim n = args.Count
      Do While n > 0 AndAlso args(n - 1) = emptyValue
        n -= 1
      Loop
      Return args.GetRange(0, n)
    End Function

    Private Function PointStatement(t As List(Of Token), s As Integer, e As Integer) As String
      m_usesGraphics = True
      Dim p = s + 1
      Dim st As Boolean, x As String = "", y As String = ""
      If Not ParseCoord(t, p, e, st, x, y) Then Throw New ConvertException(t(s).Text & " needs (x, y)")
      Dim parts = TailParts(t, p, e)
      Dim args As New List(Of String)
      If st Then args.Add("[STEP]")
      args.Add(x) : args.Add(y)
      If parts.Count > 0 AndAlso parts(0).Count > 0 Then args.Add(IntText(TailExpr(parts, 0)))
      Return t(s).Upper() & "(" & String.Join(", ", args) & ")"
    End Function

    Private Function LineStatement(t As List(Of Token), s As Integer, e As Integer) As String
      m_usesGraphics = True
      Dim p = s + 1
      Dim st1 As Boolean, x1 As String = "", y1 As String = ""
      Dim hasFirst = ParseCoord(t, p, e, st1, x1, y1)
      If p >= e OrElse Not t(p).Is_("-") Then Throw New ConvertException("LINE needs -(x, y)")
      p += 1
      Dim st2 As Boolean, x2 As String = "", y2 As String = ""
      If Not ParseCoord(t, p, e, st2, x2, y2) Then Throw New ConvertException("LINE needs -(x, y)")
      Dim parts = TailParts(t, p, e)
      Dim args As New List(Of String)
      If hasFirst Then
        If st1 OrElse st2 Then args.Add(If(st1, "[STEP]", "False"))
        args.Add(x1) : args.Add(y1)
      End If
      If st2 Then args.Add("[STEP]") Else If hasFirst AndAlso st1 Then args.Add("False")
      args.Add(x2) : args.Add(y2)
      Dim opt = "LineOption.None"
      If parts.Count > 1 AndAlso parts(1).Count = 1 Then
        Select Case parts(1)(0).Upper()
          Case "B" : opt = "LineOption.B"
          Case "BF" : opt = "LineOption.BF"
          Case Else : Throw New ConvertException("unknown LINE option " & parts(1)(0).Text)
        End Select
      End If
      Dim tail As New List(Of String) From {IntText(TailExpr(parts, 0)), opt, If(parts.Count > 2 AndAlso parts(2).Count > 0, IntText(TailExpr(parts, 2)), "&HFFFF")}
      If tail(2) = "&HFFFF" Then tail.RemoveAt(2)
      If tail.Count = 1 AndAlso tail(0) = "-1" Then tail.Clear()
      If tail.Count = 2 AndAlso tail(1) = "LineOption.None" Then tail.RemoveAt(1) : If tail(0) = "-1" Then tail.Clear()
      args.AddRange(tail)
      Return "LINE(" & String.Join(", ", args) & ")"
    End Function

    Private Function CircleStatement(t As List(Of Token), s As Integer, e As Integer) As String
      m_usesGraphics = True
      Dim p = s + 1
      Dim st As Boolean, x As String = "", y As String = ""
      If Not ParseCoord(t, p, e, st, x, y) Then Throw New ConvertException("CIRCLE needs (x, y)")
      Dim parts = TailParts(t, p, e)
      If parts.Count = 0 OrElse parts(0).Count = 0 Then Throw New ConvertException("CIRCLE needs a radius")
      Dim args As New List(Of String)
      If st Then args.Add("[STEP]")
      args.Add(x) : args.Add(y) : args.Add(TailExpr(parts, 0).Text)
      args.Add(IntText(TailExpr(parts, 1)))
      For i = 2 To 4
        Dim v = TailExpr(parts, i)
        args.Add(If(v Is Nothing, "Nothing", v.Text))
      Next
      Dim n = args.Count
      Do While n > 0 AndAlso (args(n - 1) = "Nothing" OrElse (n = args.Count AndAlso False))
        n -= 1
      Loop
      args = args.GetRange(0, n)
      If args.Count > 0 AndAlso args(args.Count - 1) = "-1" AndAlso args.Count = If(st, 5, 4) Then args.RemoveAt(args.Count - 1)
      Return "CIRCLE(" & String.Join(", ", args) & ")"
    End Function

    Private Function PaintStatement(t As List(Of Token), s As Integer, e As Integer) As String
      m_usesGraphics = True
      Dim p = s + 1
      Dim st As Boolean, x As String = "", y As String = ""
      If Not ParseCoord(t, p, e, st, x, y) Then Throw New ConvertException("PAINT needs (x, y)")
      Dim parts = TailParts(t, p, e)
      Dim args As New List(Of String)
      If st Then args.Add("[STEP]")
      args.Add(x) : args.Add(y)
      Dim paintE = TailExpr(parts, 0)
      If paintE IsNot Nothing Then
        args.Add(If(paintE.Qt = QT.String, paintE.Text, IntText(paintE)))
        Dim border = TailExpr(parts, 1)
        Dim back = TailExpr(parts, 2)
        If border IsNot Nothing OrElse back IsNot Nothing Then args.Add(IntText(border))
        If back IsNot Nothing Then args.Add(back.Text)
      End If
      Return "PAINT(" & String.Join(", ", args) & ")"
    End Function

    ''' <summary>An array argument of GET, PUT and PALETTE USING: the array name (QBasic names the array without parentheses), or an element, which becomes the start index.</summary>
    Private Function ArrayArgs(part As List(Of Token)) As String
      If part.Count = 0 Then Throw New ConvertException("an array is needed")
      If part(0).Kind = TokKind.Ident Then
        Dim v = ResolveVariableCore(part(0).Text, True)
        If part.Count = 1 Then Return ExprParser.VarText(v)
        If part(1).Is_("(") AndAlso part(part.Count - 1).Is_(")") Then
          Dim idx = Expr(part, 2, part.Count - 1)
          Return ExprParser.VarText(v) & ", " & ExprParser.Coerce(idx, QT.Long)
        End If
      End If
      Throw New ConvertException("unrecognized array argument")
    End Function

    Private Function GetPutStatement(t As List(Of Token), s As Integer, e As Integer) As String
      Dim isGet = t(s).IsWord("GET")
      Dim p = s + 1
      Dim isGraphic = p < e AndAlso (t(p).Is_("(") OrElse t(p).IsWord("STEP"))
      If Not isGraphic Then
        ' GET/PUT [#]filenum, [record], variable
        If p < e AndAlso t(p).Is_("#") Then p += 1
        Dim parts = SplitTopLevel(t, p, e)
        Dim num = ExprParser.Coerce(Expr(parts(0), 0, parts(0).Count), QT.Long)
        Dim rec = If(parts.Count > 1 AndAlso parts(1).Count > 0, ExprParser.Coerce(Expr(parts(1), 0, parts(1).Count), QT.Long), "")
        If parts.Count < 3 Then
          ' GET #n [, record]: the FIELD buffer.
          Return If(isGet, "GET_FIELD", "PUT_FIELD") & "(" & num & If(rec <> "", ", " & rec, "") & ")"
        End If
        Dim target = parts(2)
        Dim v = Expr(target, 0, target.Count)
        Return If(isGet, "[GET]", "PUT") & "(" & num & If(rec <> "", ", " & rec, "") & ", " & v.Text & ")"
      End If

      m_usesGraphics = True
      Dim st1 As Boolean, x1 As String = "", y1 As String = ""
      ParseCoord(t, p, e, st1, x1, y1)
      Dim st2 As Boolean, x2 As String = "", y2 As String = ""
      Dim args As New List(Of String)
      If isGet Then
        If p >= e OrElse Not t(p).Is_("-") Then Throw New ConvertException("GET needs (x1, y1)-(x2, y2)")
        p += 1
        If Not ParseCoord(t, p, e, st2, x2, y2) Then Throw New ConvertException("GET needs (x1, y1)-(x2, y2)")
        If st1 OrElse st2 Then args.Add(If(st1, "[STEP]", "False")) : args.Add(x1) : args.Add(y1) : args.Add(If(st2, "[STEP]", "False")) : args.Add(x2) : args.Add(y2) Else args.AddRange({x1, y1, x2, y2})
        Dim parts = TailParts(t, p, e)
        If parts.Count = 0 Then Throw New ConvertException("GET needs an array")
        args.Add(ArrayArgs(parts(0)))
        Return "[GET](" & String.Join(", ", args) & ")"
      End If

      If st1 Then args.Add("[STEP]")
      args.Add(x1) : args.Add(y1)
      Dim tail = TailParts(t, p, e)
      If tail.Count = 0 Then Throw New ConvertException("PUT needs an array")
      Dim arrayParts = ArrayArgs(tail(0)).Split({", "}, 2, StringSplitOptions.None)
      args.Add(arrayParts(0))
      Dim action = ""
      If tail.Count > 1 AndAlso tail(1).Count = 1 Then
        Select Case tail(1)(0).Upper()
          Case "PSET" : action = "PutOption.PUT_PSET"
          Case "PRESET" : action = "PutOption.PUT_PRESET"
          Case "AND" : action = "PutOption.PUT_AND"
          Case "OR" : action = "PutOption.PUT_OR"
          Case "XOR" : action = "PutOption.PUT_XOR"
          Case Else : Throw New ConvertException("unknown PUT action " & tail(1)(0).Text)
        End Select
      End If
      If action <> "" OrElse arrayParts.Length > 1 Then args.Add(If(action <> "", action, "PutOption.PUT_XOR"))
      If arrayParts.Length > 1 Then args.Add(arrayParts(1))
      Return "PUT(" & String.Join(", ", args) & ")"
    End Function

    ' VIEW (x1, y1)-(x2, y2)[, color[, border]]  and  WINDOW (x1, y1)-(x2, y2)
    Private Function ViewWindowStatement(name As String, t As List(Of Token), s As Integer, e As Integer, Optional skip As Integer = 1) As String
      m_usesGraphics = True
      Dim p = s + skip
      Dim st As Boolean, x1 As String = "", y1 As String = "", x2 As String = "", y2 As String = ""
      If Not ParseCoord(t, p, e, st, x1, y1) OrElse p >= e OrElse Not t(p).Is_("-") Then Throw New ConvertException(name & " needs (x1, y1)-(x2, y2)")
      p += 1
      If Not ParseCoord(t, p, e, st, x2, y2) Then Throw New ConvertException(name & " needs (x1, y1)-(x2, y2)")
      Dim args As New List(Of String) From {x1, y1, x2, y2}
      If name.StartsWith("VIEW", StringComparison.Ordinal) Then
        Dim parts = TailParts(t, p, e)
        If parts.Count > 0 Then args.Add(IntText(TailExpr(parts, 0)))
        If parts.Count > 1 Then args.Add(IntText(TailExpr(parts, 1)))
      End If
      Return name & "(" & String.Join(", ", args) & ")"
    End Function

    ' ON TIMER(n) GOSUB label, ON PLAY(n), ON KEY(n), ON STRIG(n), ON PEN, ON COM(n): the library takes the routine as a delegate.
    Private Function OnEventStatement(t As List(Of Token), s As Integer, e As Integer) As String
      Dim kind = t(s + 1).Upper()
      If kind = "PEN" OrElse kind = "STRIG" Then m_usesGraphics = True
      If kind = "COM" Then m_usesSerial = True
      If kind = "PLAY" Then m_usesSound = True
      Dim gosubAt = FindWord(t, s, e, "GOSUB")
      If gosubAt < 0 Then Throw New ConvertException("ON " & kind & " needs GOSUB")
      Dim arg = ""
      If gosubAt > s + 2 Then
        If Not t(s + 2).Is_("(") OrElse Not t(gosubAt - 1).Is_(")") Then Throw New ConvertException("malformed ON " & kind)
        arg = ExprParser.Coerce(Expr(t, s + 3, gosubAt - 1), QT.Long)
      End If
      Dim label = t(gosubAt + 1).Text
      If m_curProc IsNot Nothing AndAlso Not m_procGosubTargets.Contains(label) Then Throw New ConvertException("GOSUB to a label outside the procedure")
      Dim routine = If(m_curProc IsNot Nothing, GosubName(label), "AddressOf " & GosubName(label))
      Return "ON_" & kind & "(" & If(arg <> "", arg & ", ", "") & routine & ")"
    End Function

    ' DEF FNname (params) = expression: a function, with its parameters by value.
    Private Function EmitDefFn(sym As ProcSym) As String
      Dim scope = m_procScopes(sym.QbName)
      Dim savedScope = m_scope, savedProc = m_curProc
      m_scope = scope : m_curProc = sym
      Try
        Dim parms As New List(Of String)
        For Each prm In sym.Params
          Dim v = DeclareVariable(scope, prm.QbName & If(prm.HasSuffix, Types.Suffix(prm.Qt), ""), prm.IsArray, If(prm.HasSuffix, QT.Unknown, If(prm.Qt <> QT.Unknown, prm.Qt, QT.Unknown)), prm.UserType, True)
          parms.Add(ParamText(v, prm).Replace("ByRef ", "ByVal "))
        Next
        Dim body = Expr(sym.DefBody, 0, sym.DefBody.Count)
        Dim head = "Function " & ExprParser.ProcName(sym) & "(" & String.Join(", ", parms) & ")" & If(sym.HasSuffix, "", " As " & Types.VbName(sym.RetQt))
        Flush(m_procLines)
        m_procLines.Add(head)
        m_procLines.Add("  Return " & ExprParser.Coerce(body, sym.RetQt))
        m_procLines.Add("End Function")
        m_procLines.Add("")
      Finally
        m_scope = savedScope : m_curProc = savedProc
      End Try
      Return "' " & m_lines(sym.HeaderLine).Raw.Trim()
    End Function

    ' ---- PRINT / INPUT / OPEN -------------------------------------------------------------------------------

    Private Function PrintStatement(t As List(Of Token), s As Integer, e As Integer, Optional toPrinter As Boolean = False) As String
      Dim stmt = If(toPrinter, "LPRINT", "PRINT")
      Dim p = s + 1
      Dim fileNum = ""
      If p < e AndAlso t(p).Is_("#") Then
        Dim comma = -1
        For k = p To e - 1
          If t(k).Is_(",") Then comma = k : Exit For
        Next
        If comma < 0 Then Throw New ConvertException("malformed PRINT #")
        fileNum = ExprParser.Coerce(Expr(t, p + 1, comma), QT.Long)
        p = comma + 1
      End If
      If p < e AndAlso t(p).IsWord("USING") Then
        Dim semi = -1
        For k = p To e - 1
          If t(k).Is_(";") Then semi = k : Exit For
        Next
        Dim fmt = Expr(t, p + 1, If(semi > 0, semi, e)).Text
        Dim vals As New List(Of String)
        If semi > 0 Then
          For Each part In SplitTopLevelBy(t, semi + 1, e, ";")
            If part.Count > 0 Then vals.Add(Expr(part, 0, part.Count).Text)
          Next
        End If
        Dim trailing = t(e - 1).Is_(";")
        Return stmt & "_USING(" & If(fileNum <> "", fileNum & ", ", "") & fmt & If(vals.Count > 0, ", " & String.Join(", ", vals), "") & ")"
      End If
      If p >= e Then Return stmt & "(" & fileNum & ")"

      ' items separated by ; or ,
      Dim sb As New StringBuilder
      Dim noCr = False
      Dim itemStart = p
      Dim depth = 0
      Dim k2 = p
      Dim first = True
      Do While k2 <= e
        Dim atEnd = k2 = e
        Dim sep As String = ""
        If Not atEnd Then
          If t(k2).Is_("(") Then depth += 1
          If t(k2).Is_(")") Then depth -= 1
          If depth = 0 AndAlso (t(k2).Is_(";") OrElse t(k2).Is_(",")) Then sep = t(k2).Text
        End If
        If atEnd OrElse sep <> "" Then
          If k2 > itemStart Then
            Dim x = Expr(t, itemStart, k2)
            If Not first Then sb.Append(" & ")
            first = False
            If x.Qt = QT.String Then
              sb.Append(If(x.Atom, x.Text, "(" & x.Text & ")"))
            Else
              sb.Append("Str$(" & x.Text & ") & "" """)
            End If
          End If
          If sep = "," Then
            If sb.Length > 0 Then sb.Append(" & vbTab") Else sb.Append("vbTab")
            first = False
          End If
          If atEnd Then
            noCr = (k2 > p AndAlso (t(k2 - 1).Is_(";") OrElse t(k2 - 1).Is_(",")))
          End If
          itemStart = k2 + 1
        End If
        k2 += 1
      Loop
      Dim body = sb.ToString
      If body = "" Then body = """"""
      Return stmt & "(" & If(fileNum <> "", fileNum & ", ", "") & body & If(noCr, ", True", "") & ")"
    End Function

    Private Shared Function SplitTopLevelBy(t As List(Of Token), s As Integer, e As Integer, sep As String) As List(Of List(Of Token))
      Dim result As New List(Of List(Of Token))
      Dim cur As New List(Of Token)
      Dim depth = 0
      For k = s To e - 1
        If t(k).Is_("(") Then depth += 1
        If t(k).Is_(")") Then depth -= 1
        If depth = 0 AndAlso (t(k).Is_(sep) OrElse (sep = ";" AndAlso t(k).Is_(","))) Then
          result.Add(cur) : cur = New List(Of Token)
        Else
          cur.Add(t(k))
        End If
      Next
      result.Add(cur)
      Return result
    End Function

    Private Function InputStatement(t As List(Of Token), s As Integer, e As Integer) As String
      Dim p = s + 1
      Dim keep = False
      If p < e AndAlso t(p).Is_(";") Then keep = True : p += 1

      Dim fileNum = ""
      If p < e AndAlso t(p).Is_("#") Then
        Dim comma = -1
        For k = p To e - 1
          If t(k).Is_(",") Then comma = k : Exit For
        Next
        fileNum = ExprParser.Coerce(Expr(t, p + 1, comma), QT.Long)
        p = comma + 1
      End If

      Dim prompt = ""
      Dim question = True
      If fileNum = "" AndAlso p < e AndAlso t(p).Kind = TokKind.String Then
        prompt = ExprParser.Quote(t(p).Text)
        p += 1
        If p < e AndAlso t(p).Is_(";") Then
          p += 1
        ElseIf p < e AndAlso t(p).Is_(",") Then
          question = False : p += 1
        End If
      End If

      Dim vars As New List(Of String)
      Dim fits As New List(Of String)
      For Each part In SplitTopLevel(t, p, e)
        If part.Count = 0 Then Continue For
        Dim x = Expr(part, 0, part.Count)
        vars.Add(x.Text)
        If x.FixedLen > 0 Then fits.Add(FitStatement(x))
      Next
      Dim args As New List(Of String)
      If fileNum <> "" Then args.Add(fileNum)
      If prompt <> "" Then args.Add(prompt)
      ' INPUT a, b without a prompt: the prompt is empty (it still shows the question mark).
      If fileNum = "" AndAlso prompt = "" AndAlso vars.Count > 1 Then args.Add(ChrW(34) & ChrW(34))
      args.AddRange(vars)
      If prompt <> "" AndAlso Not question Then args.Add("False")
      If fileNum = "" AndAlso prompt = "" AndAlso keep Then
        args.Insert(0, """""") : args.Add("False") : args.Add("True")
      End If
      Return "INPUT(" & String.Join(", ", args) & ")" & String.Concat(fits.Select(Function(f) " : " & f))
    End Function

    ' After a statement that reads into a STRING * n variable: the text is padded or cut to n characters.
    Private Shared Function FitStatement(target As E) As String
      Return target.Text & " = FixedString.Fit(" & target.Text & ", " & target.FixedLen & ")"
    End Function

    Private Function LineInput(t As List(Of Token), s As Integer, e As Integer) As String
      Dim p = s + 2
      Dim fileNum = ""
      If p < e AndAlso t(p).Is_("#") Then
        Dim comma = -1
        For k = p To e - 1
          If t(k).Is_(",") Then comma = k : Exit For
        Next
        fileNum = ExprParser.Coerce(Expr(t, p + 1, comma), QT.Long)
        p = comma + 1
      End If
      Dim prompt = ""
      If fileNum = "" AndAlso p < e AndAlso t(p).Kind = TokKind.String Then
        prompt = ExprParser.Quote(t(p).Text)
        p += 1
        If p < e AndAlso (t(p).Is_(";") OrElse t(p).Is_(",")) Then p += 1
      End If
      Dim v = Expr(t, p, e)
      Dim args As New List(Of String)
      If fileNum <> "" Then args.Add(fileNum)
      If prompt <> "" Then args.Add(prompt)
      args.Add(v.Text)
      Return "LINE_INPUT(" & String.Join(", ", args) & ")" & If(v.FixedLen > 0, " : " & FitStatement(v), "")
    End Function

    Private Function OpenStatement(t As List(Of Token), s As Integer, e As Integer) As String
      Dim forIdx = FindWord(t, s, e, "FOR")
      Dim asIdx = FindWord(t, s, e, "AS")
      If forIdx < 0 OrElse asIdx < 0 Then Throw New ConvertException("this form of OPEN isn't converted yet")
      Dim fileName = Expr(t, s + 1, forIdx).Text
      If System.Text.RegularExpressions.Regex.IsMatch(fileName, "^\s*""(COM[0-9]|/dev/)", System.Text.RegularExpressions.RegexOptions.IgnoreCase) Then m_usesSerial = True
      Dim mode = t(forIdx + 1).Upper()
      Dim vbMode As String
      Select Case mode
        Case "INPUT" : vbMode = "OpenMode.Input"
        Case "OUTPUT" : vbMode = "OpenMode.Output"
        Case "APPEND" : vbMode = "OpenMode.Append"
        Case "RANDOM" : vbMode = "OpenMode.Random"
        Case "BINARY" : vbMode = "OpenMode.Binary"
        Case Else : Throw New ConvertException("OPEN mode " & mode)
      End Select
      Dim numStart = asIdx + 1
      If numStart < e AndAlso t(numStart).Is_("#") Then numStart += 1
      Dim lenIdx = FindWord(t, s, e, "LEN")
      Dim numEnd = If(lenIdx > 0, lenIdx, e)
      Dim num = ExprParser.Coerce(Expr(t, numStart, numEnd), QT.Long)
      Dim extra = ""
      If lenIdx > 0 Then extra = ", " & ExprParser.Coerce(Expr(t, lenIdx + 2, e), QT.Long)
      Return "OPEN(" & fileName & ", " & vbMode & ", " & num & extra & ")"
    End Function

  End Class

End Namespace
