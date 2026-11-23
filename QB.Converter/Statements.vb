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
      Return result
    End Function

    Private Function Cond(t As List(Of Token), s As Integer, e As Integer) As String
      Dim ep As New ExprParser(t.GetRange(s, e - s), 0, Me)
      Dim x = ep.ParseExpr()
      If Not ep.AtEnd() Then Throw New ConvertException("unexpected text in the condition")
      Return ExprParser.AsBool(x)
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
            Return "While " & Cond(t, s + 1, e)
          Case "WEND"
            Return "End While"

          Case "DO"
            If n = 1 Then Return "Do"
            If t(s + 1).IsWord("WHILE") Then Return "Do While " & Cond(t, s + 2, e)
            If t(s + 1).IsWord("UNTIL") Then Return "Do Until " & Cond(t, s + 2, e)
          Case "LOOP"
            If n = 1 Then Return "Loop"
            If t(s + 1).IsWord("WHILE") Then Return "Loop While " & Cond(t, s + 2, e)
            If t(s + 1).IsWord("UNTIL") Then Return "Loop Until " & Cond(t, s + 2, e)

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
                Case "SUB" : Return "Exit Sub"
                Case "FUNCTION" : Return "Exit Function"
                Case "DO" : Return "Exit Do"
                Case "FOR" : Return "Exit For"
              End Select
            End If

          Case "GOTO"
            Return "GoTo " & NeedLabel(t(s + 1))

          Case "GOSUB", "RETURN"
            Throw New ConvertException(w & " isn't converted yet")

          Case "ON"
            If n >= 4 AndAlso t(s + 1).IsWord("ERROR") AndAlso t(s + 2).IsWord("GOTO") Then
              Dim lab = t(s + 3).Text
              If lab = "0" Then Return "On Error GoTo 0"
              Return "On Error GoTo " & NeedLabel(t(s + 3), True)
            End If
            If n >= 4 AndAlso t(s + 1).IsWord("ERROR") AndAlso t(s + 2).IsWord("RESUME") Then
              Return "On Error Resume Next"
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

          Case "OPEN"
            Return OpenStatement(t, s, e)

          Case "CLOSE"
            Return "CLOSE(" & ArgList(t, s + 1, e) & ")"

          Case "NAME"
            Dim asIdx = FindWord(t, s, e, "AS")
            If asIdx > 0 Then Return "NAME(" & Expr(t, s + 1, asIdx).Text & ", " & Expr(t, asIdx + 1, e).Text & ")"

          Case "READ"
            Dim items As New List(Of String)
            For Each part In SplitTopLevel(t, s + 1, e)
              items.Add("READ(" & Expr(part, 0, part.Count).Text & ")")
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

          Case "SHARED", "STATIC", "DIM", "REDIM", "COMMON"
            If w = "SHARED" Then Return ""
            If w = "STATIC" Then Return DimText(t, "Static", False)
            Return DimText(t, "Dim", False)

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

    ' A label used by GOTO and friends.  A module-level label that a procedure's ON ERROR names has its handler copied in.
    Private Function NeedLabel(tok As Token, Optional handler As Boolean = False) As String
      If handler AndAlso m_curProc IsNot Nothing AndAlso m_mainLabels.ContainsKey(tok.Text) AndAlso Not m_neededHandlers.Contains(tok.Text, StringComparer.OrdinalIgnoreCase) Then
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

      Dim lhs = Expr(t, s, eq)
      Dim target = lhs.Vb
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
        Case "VIEW", "KEY", "WIDTH", "ERROR", "ENVIRON", "DEF", "SOUND", "PLAY", "DRAW", "PSET", "PRESET", "CIRCLE", "PAINT", "GET", "PUT", "WINDOW", "PALETTE", "PCOPY", "OUT", "WAIT", "LOCK", "UNLOCK", "TIMER", "STRIG", "PEN", "COM", "LPRINT", "LOCATE", "COLOR", "CLS", "RANDOMIZE", "BEEP", "SCREEN", "KILL", "MKDIR", "CHDIR", "RMDIR", "FILES", "SHELL", "POKE", "BLOAD", "BSAVE", "SEEK", "WRITE", "FIELD", "LSET", "RSET", "RUN", "CHAIN", "CLEAR", "ERASE", "FILEATTR", "ON", "RESET", "TROFF", "TRON"
          ' fall through to the generic form below
        Case Else
      End Select
      Select Case name
        Case "LINE", "PSET", "PRESET", "CIRCLE", "PAINT", "GET", "PUT", "VIEW", "WINDOW", "DRAW", "PLAY", "WRITE", "FIELD", "LSET", "RSET", "DEF", "KEY", "ON", "ERASE", "CHAIN", "RUN", "COMMON"
          Throw New ConvertException(name & " isn't converted yet")
      End Select
      If p >= e Then Return name & "()"
      Return name & "(" & ArgList(t, p, e) & ")"
    End Function

    ' ---- PRINT / INPUT / OPEN -------------------------------------------------------------------------------

    Private Function PrintStatement(t As List(Of Token), s As Integer, e As Integer) As String
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
        Return "PRINT_USING(" & If(fileNum <> "", fileNum & ", ", "") & fmt & If(vals.Count > 0, ", " & String.Join(", ", vals), "") & ")"
      End If
      If p >= e Then Return "PRINT(" & fileNum & ")"

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
      Return "PRINT(" & If(fileNum <> "", fileNum & ", ", "") & body & If(noCr, ", True", "") & ")"
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
      For Each part In SplitTopLevel(t, p, e)
        If part.Count = 0 Then Continue For
        Dim x = Expr(part, 0, part.Count)
        vars.Add(x.Text)
      Next
      Dim args As New List(Of String)
      If fileNum <> "" Then args.Add(fileNum)
      If prompt <> "" Then args.Add(prompt)
      args.AddRange(vars)
      If prompt <> "" AndAlso Not question Then args.Add("False")
      If fileNum = "" AndAlso prompt = "" AndAlso keep Then
        args.Insert(0, """""") : args.Add("False") : args.Add("True")
      End If
      Return "INPUT(" & String.Join(", ", args) & ")"
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
      Return "LINE_INPUT(" & String.Join(", ", args) & ")"
    End Function

    Private Function OpenStatement(t As List(Of Token), s As Integer, e As Integer) As String
      Dim forIdx = FindWord(t, s, e, "FOR")
      Dim asIdx = FindWord(t, s, e, "AS")
      If forIdx < 0 OrElse asIdx < 0 Then Throw New ConvertException("this form of OPEN isn't converted yet")
      Dim fileName = Expr(t, s + 1, forIdx).Text
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
