Option Explicit On
Option Strict On
Option Infer On

Imports System.Text

Namespace QB2VB

  Partial Friend Class Converter

    ' ---- Pass 2: the lines ---------------------------------------------------------------------------------

    Private Function VbComment(comment As String) As String
      If comment.StartsWith("'", StringComparison.Ordinal) Then Return comment
      If comment.StartsWith("REM", StringComparison.OrdinalIgnoreCase) Then Return "'" & comment.Substring(3)
      Return "' " & comment
    End Function

    Private Sub Pass2()

      EmitData()
      PlanInlineDeclarations()

      Dim i = 0
      Do While i < m_lines.Count
        m_currentLine = m_lines(i).Number
        Try
          i = EmitLine(i)
        Catch ex As ConvertException
          Todo(m_lines(i), ex.Message)
          i += 1
        End Try
      Loop

      FinishModule()

    End Sub

    Private Sub Todo(ln As SourceLine, message As String)
      Note(message)
      Flush(m_target)
      m_target.Add(ln.Indent & "' TODO(QB2VB): " & message & ":")
      m_target.Add(ln.Indent & "'   " & ln.Raw.Trim())
    End Sub

    ' The DATA statements, at the start of the program, in order (QBasic's DATA is static; a label becomes DATA_LABEL).
    Private Sub EmitData()
      For Each idx In m_dataLines
        Dim ln = m_lines(idx)
        Dim back = idx - 1
        Dim labels As New List(Of String)
        Do While back >= 0 AndAlso (m_lines(back).IsBlank OrElse m_lines(back).IsCommentOnly OrElse (m_lines(back).Tokens.Count = 0 AndAlso (m_lines(back).Label <> "" OrElse m_lines(back).LineNumber <> "")))
          If m_lines(back).Label <> "" Then labels.Insert(0, m_lines(back).Label)
          If m_lines(back).LineNumber <> "" Then labels.Insert(0, m_lines(back).LineNumber)
          back -= 1
        Loop
        For Each l In labels
          m_mainLines.Add("DATA_LABEL(" & ExprParser.Quote(l) & ")")
        Next
        If ln.Label <> "" Then m_mainLines.Add("DATA_LABEL(" & ExprParser.Quote(ln.Label) & ")")
        If ln.LineNumber <> "" Then m_mainLines.Add("DATA_LABEL(" & ExprParser.Quote(ln.LineNumber) & ")")
        Dim raw = ln.Raw
        Dim p = raw.IndexOf("DATA", StringComparison.OrdinalIgnoreCase)
        Dim body = raw.Substring(p + 4)
        ' Items: separated by commas outside quotes; a ' outside quotes starts a comment.
        Dim items As New List(Of String)
        Dim sb As New StringBuilder
        Dim inQ = False
        Dim quotedItem = False
        For Each c In body
          If c = """"c Then
            inQ = Not inQ : quotedItem = True
          ElseIf c = "'"c AndAlso Not inQ Then
            Exit For
          ElseIf c = ","c AndAlso Not inQ Then
            items.Add(DataItem(sb.ToString, quotedItem)) : sb.Clear() : quotedItem = False
          Else
            sb.Append(c)
          End If
        Next
        items.Add(DataItem(sb.ToString, quotedItem))
        m_mainLines.Add("DATA(" & String.Join(", ", items) & ")")
      Next
      If m_dataLines.Count > 0 Then m_mainLines.Add("")
    End Sub

    Private Shared Function DataItem(text As String, quoted As Boolean) As String
      Dim t = text.Trim()
      If quoted Then
        If t.Length >= 2 AndAlso t.StartsWith(""", StringComparison.Ordinal) AndAlso t.EndsWith(""", StringComparison.Ordinal) Then t = t.Substring(1, t.Length - 2)
        Return ExprParser.Quote(t)
      End If
      ' &H and &O numbers (16 bit unless they have more digits, as in QBasic).
      If t.Length > 2 AndAlso t(0) = "&"c AndAlso "HhOo".IndexOf(t(1)) >= 0 Then
        Try
          Dim isHex = Char.ToUpperInvariant(t(1)) = "H"c
          Dim digits = t.Substring(2).TrimEnd("&"c)
          Dim value = System.Convert.ToInt64(digits, If(isHex, 16, 8))
          If digits.Length <= If(isHex, 4, 6) AndAlso value > 32767 Then value -= 65536
          If digits.Length > If(isHex, 4, 6) AndAlso value > Integer.MaxValue Then value -= 4294967296L
          Return value.ToString(Globalization.CultureInfo.InvariantCulture)
        Catch ex As FormatException
        End Try
      End If
      Dim d As Double
      If t <> "" AndAlso Double.TryParse(t, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, d) Then
        ' (A number as VB writes it: no "1." or ".5".)
        If t.IndexOfAny({"."c, "e"c, "E"c, "d"c, "D"c}) < 0 Then Return t
        Return d.ToString("R", Globalization.CultureInfo.InvariantCulture)
      End If
      Return ExprParser.Quote(t)
    End Function

    ''' <summary>Converts the line at <paramref name="i"/>; returns the index of the next line to convert.</summary>
    Private Function EmitLine(i As Integer) As Integer

      Dim ln = m_lines(i)
      m_emitIndex = i

      If ln.IsBlank Then
        m_pending.Add("")
        Return i + 1
      End If
      If ln.IsCommentOnly Then
        m_pending.Add(ln.Indent & VbComment(ln.Comment))
        Return i + 1
      End If

      Dim t = ln.Tokens
      Dim first = If(t.Count > 0, t(0).Upper(), "")
      Dim labelPrefix = If(ln.Label <> "", ln.Label & ":", If(ln.LineNumber <> "", ln.LineNumber & ":", ""))

      ' A label that a GOSUB in a SUB or FUNCTION calls: its routine becomes a lambda in the procedure.
      If m_curProc IsNot Nothing AndAlso Not m_inGosub AndAlso m_inlineEnd < 0 AndAlso ((ln.Label <> "" AndAlso m_procGosubTargets.Contains(ln.Label)) OrElse (ln.LineNumber <> "" AndAlso m_procGosubTargets.Contains(ln.LineNumber))) Then
        Dim target = If(ln.Label <> "", ln.Label, ln.LineNumber)
        If m_inlineRoutines.Contains(target) Then
          Flush(m_target)
          m_target.Add(ln.Indent & "' " & target & ": (this routine is expanded where it is called, because it uses ON ERROR)")
          Return HandlerRegionEnd(i) + 1
        End If
        Return EmitProcRoutine(i)
      End If

      ' A label that a GOSUB calls: its routine becomes a procedure.
      If m_curProc Is Nothing AndAlso Not m_inGosub AndAlso ((ln.Label <> "" AndAlso m_gosubTargets.Contains(ln.Label)) OrElse (ln.LineNumber <> "" AndAlso m_gosubTargets.Contains(ln.LineNumber))) Then
        Return EmitGosubRoutine(i)
      End If
      If m_inGosub Then labelPrefix = If(i = m_gosubStart, "", labelPrefix)

      ' A label alone on its line.
      If t.Count = 0 Then
        Flush(m_target)
        m_target.Add(labelPrefix & If(ln.Comment <> "", "  " & VbComment(ln.Comment), ""))
        Return i + 1
      End If

      Select Case first

        Case "SUB", "FUNCTION"
          BeginProc(i)
          Return i + 1

        Case "END"
          If t.Count > 1 AndAlso (t(1).IsWord("SUB") OrElse t(1).IsWord("FUNCTION")) Then
            EndProc(i)
            Return i + 1
          End If

        Case "TYPE"
          Return EmitTypeBlock(i)

        Case "DECLARE", "DEFINT", "DEFSNG", "DEFDBL", "DEFLNG", "DEFSTR"
          Flush(m_target)
          m_target.Add(ln.Indent & "' " & ln.Raw.Trim())
          Return i + 1

        Case "OPTION"
          If t.Count > 2 AndAlso t(1).IsWord("BASE") AndAlso t(2).Text <> "0" Then Note("OPTION BASE " & t(2).Text & ": VB arrays start at 0")
          Flush(m_target)
          m_target.Add(ln.Indent & "' " & ln.Raw.Trim())
          Return i + 1

        Case "CONST"
          ' (constants are program-wide here, so one declared inside a procedure is hoisted too)
          ' (CONST A = 1: B = 2 is a constant and then an assignment)
          Dim colon = -1
          Dim depth = 0
          For k = 0 To t.Count - 1
            If t(k).Is_("(") Then depth += 1
            If t(k).Is_(")") Then depth -= 1
            If depth = 0 AndAlso t(k).Is_(":") Then colon = k : Exit For
          Next
          For Each c In HoistedConst(If(colon > 0, t.GetRange(0, colon), t))
            If Not m_hoisted.Contains(c) Then m_hoisted.Add(c)
          Next
          Flush(m_target)
          m_target.Add(ln.Indent & "' " & ln.Raw.Trim())
          If colon < 0 OrElse colon = t.Count - 1 Then Return i + 1
          t = t.GetRange(colon + 1, t.Count - colon - 1)

        Case "DATA"
          Flush(m_target)
          m_target.Add(ln.Indent & "' " & ln.Raw.Trim() & "   (DATA is listed at the start of the program)")
          Return i + 1

        Case "DIM", "COMMON"
          If m_curProc Is Nothing AndAlso t.Count > 1 AndAlso t(1).IsWord("SHARED") Then
            m_hoisted.Add(DimText(t, "Dim", True))
            If first = "COMMON" Then m_mainPrologue.AddRange(CommonLoads(t))
            Flush(m_target)
            m_target.Add(ln.Indent & "' " & ln.Raw.Trim())
            Return i + 1
          End If

        Case "SHARED"
          If m_curProc IsNot Nothing Then
            Flush(m_target)
            m_target.Add(ln.Indent & "' " & ln.Raw.Trim() & "   (shared variables are declared at module level)")
            Return i + 1
          End If

      End Select

      Dim statements = ParseStatementList(t, 0, t.Count)
      If statements.Count = 0 Then
        If ln.Comment <> "" Then m_pending.Add(ln.Indent & VbComment(ln.Comment))
        Return i + 1
      End If

      ' A FOR whose NEXT is on the next line with nothing between: an empty (delay) loop.
      If statements(statements.Count - 1).StartsWith("For ", StringComparison.Ordinal) Then
        Dim nx = i + 1
        Do While nx < m_lines.Count AndAlso (m_lines(nx).IsBlank OrElse m_lines(nx).IsCommentOnly)
          nx += 1
        Loop
        If nx < m_lines.Count AndAlso m_lines(nx).Tokens.Count > 0 AndAlso m_lines(nx).Tokens(0).IsWord("NEXT") AndAlso m_lines(nx).Label = "" Then statements.Add("EMPTYLOOP()")
      End If

      If statements.Count = 1 AndAlso statements(0).StartsWith(InlineMarker, StringComparison.Ordinal) Then
        Return ExpandInline(i, statements(0).Substring(InlineMarker.Length), labelPrefix)
      End If
      If statements.Any(Function(x) x.Contains(InlineMarker)) Then Throw New ConvertException("this GOSUB needs to be expanded inline, which only works for a GOSUB on a line of its own")

      Flush(m_target)
      Dim text = ln.Indent & labelPrefix & If(labelPrefix <> "", " ", "") & String.Join(" : ", statements)
      If ln.Comment <> "" Then text &= "  " & VbComment(ln.Comment)
      m_target.Add(text)
      Return i + 1

    End Function

    Private m_gosubStart As Integer
    Private m_emitIndex As Integer

    ' The routine that starts at a label that a GOSUB calls: lines up to its RETURN become a Sub (module-level variables are shared).
    Private Function EmitGosubRoutine(i As Integer) As Integer
      Dim ln = m_lines(i)
      Dim name = GosubName(If(ln.Label <> "", ln.Label, ln.LineNumber))
      Dim [end] = HandlerRegionEnd(i)
      Flush(m_target)
      Dim saved = m_target
      m_target = m_procLines
      m_procLines.Add("Sub " & name & "()" & If(ln.Comment <> "", "  " & VbComment(ln.Comment), ""))
      m_inGosub = True
      m_gosubStart = i
      Dim k = i
      Do While k <= [end]
        m_currentLine = m_lines(k).Number
        Try
          If k = i AndAlso m_lines(k).Tokens.Count = 0 Then
            k += 1
            Continue Do
          End If
          EmitLine(k)
        Catch ex As ConvertException
          Todo(m_lines(k), ex.Message)
        End Try
        k += 1
      Loop
      Flush(m_target)
      EmitNeededHandlers("Exit Sub")
      m_procLines.Add("End Sub")
      m_procLines.Add("")
      m_inGosub = False
      m_target = saved
      Return [end] + 1
    End Function

    ' ---- Procedures ---------------------------------------------------------------------------------------------

    Private Function ParamText(v As VarSym, prm As ParamSym) As String
      Dim name = v.VbName & If(prm.CopyIn, "_Arg", "") & If(v.HasSuffix, Types.VbSuffix(v.Qt), "")
      Dim sb As New StringBuilder("ByRef " & name)
      If prm.IsArray Then sb.Append("(" & New String(","c, prm.Rank - 1) & ")")
      If Not v.HasSuffix Then
        If v.UserType <> "" Then
          sb.Append(" As " & v.UserType)
        Else
          sb.Append(" As " & Types.VbName(v.Qt))
        End If
      End If
      Return sb.ToString
    End Function

    Private Sub BeginProc(i As Integer)
      Dim ln = m_lines(i)
      Dim t = ln.Tokens
      Dim sym = FindProcCore(t(1).Text)
      Dim scope = m_procScopes(sym.QbName)
      m_scope = scope
      m_curProc = sym
      m_procStatic = sym.IsStatic
      m_neededHandlers.Clear()

      Flush(m_procLines)
      m_target = m_procLines

      Dim parms As New List(Of String)
      SetArrayRanks(i, sym)
      PrepareProcRoutines(i, sym)
      For Each prm In sym.Params
        Dim v = DeclareVariable(scope, prm.QbName & If(prm.HasSuffix, Types.Suffix(prm.Qt), ""), prm.IsArray, If(prm.HasSuffix, QT.Unknown, If(prm.UserType <> "" OrElse prm.Qt <> QT.Unknown, prm.Qt, QT.Unknown)), prm.UserType, True)
        parms.Add(If(sym.IsDefBlock, ParamText(v, prm).Replace("ByRef ", "ByVal "), ParamText(v, prm)))
        If prm.CopyIn Then
          Dim local = v.VbName & If(v.HasSuffix, Types.VbSuffix(v.Qt), "")
          Dim arg = v.VbName & "_Arg" & If(v.HasSuffix, Types.VbSuffix(v.Qt), "")
          Dim dims = If(prm.IsArray, "(" & New String(","c, prm.Rank - 1) & ")", "")
          Dim asType = If(v.HasSuffix, "", " As " & If(v.UserType <> "", v.UserType, Types.VbName(v.Qt)))
          m_prologue.Add("  Dim " & local & dims & asType & " = " & arg)
          m_copyBack.Add(arg & " = " & local)
        End If
      Next
      Dim head = If(sym.IsFunction, "Function ", "Sub ") & ExprParser.ProcName(sym) & "(" & String.Join(", ", parms) & ")"
      If sym.IsFunction AndAlso Not sym.HasSuffix Then head &= " As " & Types.VbName(sym.RetQt)
      m_procLines.Add(ln.Indent & head & If(ln.Comment <> "", "  " & VbComment(ln.Comment), ""))
      m_procDeclAt = m_procLines.Count
    End Sub

    ' The number of indexes an array parameter is used with in the procedure's body.
    Private Sub SetArrayRanks(headerLine As Integer, sym As ProcSym)
      For Each prm In sym.Params
        If Not prm.IsArray Then Continue For
        Dim rank = 1
        Dim k = headerLine + 1
        Do While k < m_lines.Count
          Dim t = m_lines(k).Tokens
          If t.Count >= 2 AndAlso t(0).IsWord("END") AndAlso (t(1).IsWord("SUB") OrElse t(1).IsWord("FUNCTION")) Then Exit Do
          For q = 0 To t.Count - 2
            Dim hs As Boolean, bare As String = ""
            If t(q).Kind = TokKind.Ident AndAlso t(q + 1).Is_("(") Then
              TypeOfName(t(q).Text, hs, bare)
              If bare.Equals(prm.QbName, StringComparison.OrdinalIgnoreCase) Then
                Dim depth = 0, commas = 0, r = q + 1
                Do While r < t.Count
                  If t(r).Is_("(") Then depth += 1
                  If t(r).Is_(")") Then depth -= 1
                  If depth = 1 AndAlso t(r).Is_(",") Then commas += 1
                  If depth = 0 Then Exit Do
                  r += 1
                Loop
                If r > q + 2 Then rank = Math.Max(rank, commas + 1)
              End If
            End If
          Next
          k += 1
        Loop
        prm.Rank = rank
      Next
    End Sub

    Private Function DeclKeyword() As String
      Return If(m_procStatic, "Static", "Dim")
    End Function

    ' The same, and a string variable starts out empty, as in QBasic (a VB string is Nothing until it is set).
    Private Shared Function DeclItemInit(v As VarSym) As String
      Dim item = DeclItem(v)
      If v.Qt = QT.String AndAlso Not v.IsArray AndAlso v.UserType = "" AndAlso v.FixedLen = 0 AndAlso Not item.Contains(" = ") Then Return item & " = """""
      Return item
    End Function

    ' A declaration (without the keyword) of a variable found by use.
    Private Shared Function DeclItem(v As VarSym) As String
      Dim name = v.VbName & If(v.HasSuffix, Types.VbSuffix(v.Qt), "")
      Dim dims = If(v.IsArray, "(" & If(v.DeferRank > 0, New String(","c, v.DeferRank - 1), If(v.Dims <> "", v.Dims, "10")) & ")", "")
      If v.UserType <> "" Then Return name & dims & " As " & v.UserType
      If v.HasSuffix Then Return name & dims
      ' (a variable that the program never declared gets its type character, where VB has one)
      If Not v.DeclaredAs AndAlso Not name.EndsWith("]", StringComparison.Ordinal) AndAlso Not (v.Qt = QT.String AndAlso v.FixedLen > 0) AndAlso Types.VbSuffix(v.Qt) <> "" Then Return name & Types.VbSuffix(v.Qt) & dims
      If v.FixedLen > 0 AndAlso Not v.IsArray AndAlso v.Qt = QT.String Then Return name & " As String = FixedString.Blank(" & v.FixedLen & ")"
      Return name & dims & " As " & Types.VbName(v.Qt)
    End Function

    Private Sub EndProc(i As Integer)
      Dim ln = m_lines(i)
      Flush(m_target)

      ' Variables used without being declared.
      Dim decls As New List(Of String)
      For Each v In m_scope.Locals
        If Not v.Declared AndAlso Not v.IsParam AndAlso Not v.IsShared AndAlso Not v.IsConst Then
          decls.Add(ln.Indent & "  " & DeclKeyword() & " " & DeclItemInit(v))
          v.Declared = True
        End If
      Next
      ' The copies of ByRef parameters that GOSUB routines use, then the routines themselves (lambdas, so they share the locals).
      Dim after As New List(Of String)
      after.AddRange(m_prologue)
      For Each name In m_routineNames.Values
        after.Add("  Dim " & name & " As Action")
      Next
      For Each r In m_routines
        after.AddRange(r)
      Next
      decls.AddRange(after)
      m_procLines.InsertRange(m_procDeclAt, decls)
      m_procLines.AddRange(m_copyBack.Select(Function(c) "  " & c))

      EmitNeededHandlers(If(m_curProc.IsFunction, "Exit Function", "Exit Sub"))

      m_procLines.Add(ln.Indent & If(m_curProc.IsFunction, "End Function", "End Sub"))
      m_procLines.Add("")
      m_target = m_mainLines   ' (the program's own code can come after a procedure)
      m_scope = m_main
      m_curProc = Nothing
      m_procStatic = False
      m_procGosubTargets.Clear() : m_routines.Clear() : m_routineNames.Clear() : m_copyBack.Clear() : m_prologue.Clear() : m_inlineRoutines.Clear()
    End Sub

    ' GOSUB X where X uses ON ERROR: the routine's lines are written here instead of the call.
    Private Function ExpandInline(callLine As Integer, label As String, labelPrefix As String) As Integer
      Dim ln = m_lines(callLine)
      Dim start As Integer
      If Not m_procLabelLine.TryGetValue(label, start) Then Throw New ConvertException("GOSUB to an unknown label " & label)
      Dim [end] = HandlerRegionEnd(start)
      Flush(m_target)
      m_target.Add(ln.Indent & labelPrefix & If(labelPrefix <> "", " ", "") & "' GOSUB " & label & ": the routine, expanded")
      Dim savedEnd = m_inlineEnd, savedLabel = m_inlineLabel, savedUsed = m_inlineUsedLabel, savedIndex = m_emitIndex
      m_inlineCount += 1
      m_inlineEnd = [end] : m_inlineLabel = label & "_Return" & m_inlineCount : m_inlineUsedLabel = False
      For k = start + 1 To [end]
        m_currentLine = m_lines(k).Number
        Try
          EmitLine(k)
        Catch ex As ConvertException
          Todo(m_lines(k), ex.Message)
        End Try
      Next
      Flush(m_target)
      If m_inlineUsedLabel Then m_target.Add(m_inlineLabel & ":")
      m_inlineEnd = savedEnd : m_inlineLabel = savedLabel : m_inlineUsedLabel = savedUsed : m_emitIndex = savedIndex
      Return callLine + 1
    End Function

    ' ---- GOSUB routines inside a SUB or FUNCTION -------------------------------------------------------

    Private ReadOnly m_procGosubTargets As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly m_inlineRoutines As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private m_inlineEnd As Integer = -1          ' while a routine is expanded: the line of its last statement
    Private m_inlineLabel As String = ""
    Private m_inlineUsedLabel As Boolean
    Private m_inlineCount As Integer
    Private ReadOnly m_procLabelLine As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly m_routines As New List(Of List(Of String))
    Private ReadOnly m_routineNames As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly m_copyBack As New List(Of String)
    Private ReadOnly m_prologue As New List(Of String)

    ' Finds the labels that GOSUBs in the procedure call and the parameters those routines use.
    Private Sub PrepareProcRoutines(headerLine As Integer, sym As ProcSym)
      m_procGosubTargets.Clear() : m_routines.Clear() : m_routineNames.Clear() : m_copyBack.Clear() : m_prologue.Clear() : m_inlineRoutines.Clear()
      m_procLabelLine.Clear()
      Dim labelLine As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
      Dim k = headerLine + 1
      Do While k < m_lines.Count
        Dim ln = m_lines(k)
        Dim t = ln.Tokens
        If t.Count >= 2 AndAlso t(0).IsWord("END") AndAlso (t(1).IsWord("SUB") OrElse t(1).IsWord("FUNCTION")) Then Exit Do
        If ln.Label <> "" Then labelLine(ln.Label) = k : m_procLabelLine(ln.Label) = k
        If ln.LineNumber <> "" Then labelLine(ln.LineNumber) = k : m_procLabelLine(ln.LineNumber) = k
        For q = 0 To t.Count - 1
          If t(q).IsWord("GOSUB") Then
            Dim r = q + 1
            Do While r < t.Count AndAlso (t(r).Kind = TokKind.Ident OrElse t(r).Kind = TokKind.Number)
              m_procGosubTargets.Add(t(r).Text)
              r += 1
              If r < t.Count AndAlso t(r).Is_(",") Then r += 1 Else Exit Do
            Loop
          End If
        Next
        k += 1
      Loop
      If m_procGosubTargets.Count = 0 Then Return

      ' A lambda can't contain ON ERROR or RESUME: such a routine is expanded where it is called instead.
      For Each target In m_procGosubTargets
        Dim st As Integer
        If Not labelLine.TryGetValue(target, st) Then Continue For
        For q = st To HandlerRegionEnd(st)
          For Each tok In m_lines(q).Tokens
            If tok.IsWord("RESUME") OrElse (tok.IsWord("ERROR") AndAlso q > 0) Then m_inlineRoutines.Add(target)
          Next
        Next
      Next

      Dim used As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
      For Each target In m_procGosubTargets
        Dim start As Integer
        If Not labelLine.TryGetValue(target, start) Then Continue For
        For q = start To HandlerRegionEnd(start)
          For Each tok In m_lines(q).Tokens
            If tok.Kind = TokKind.Ident Then
              Dim name = tok.Text
              Dim dot = name.IndexOf("."c)
              If dot > 0 Then name = name.Substring(0, dot)
              Dim hs As Boolean, bare As String = ""
              TypeOfName(name, hs, bare)
              used.Add(bare)
            End If
          Next
        Next
      Next
      For Each prm In sym.Params
        prm.CopyIn = used.Contains(prm.QbName)
      Next
    End Sub

    ' The routine that starts at a label a GOSUB in this procedure calls: a lambda over the procedure's locals.
    Private Function EmitProcRoutine(i As Integer) As Integer
      Dim ln = m_lines(i)
      Dim label = If(ln.Label <> "", ln.Label, ln.LineNumber)
      Dim name = GosubName(label)
      Dim [end] = HandlerRegionEnd(i)
      Flush(m_target)
      Dim saved = m_target
      Dim buf As New List(Of String)
      m_target = buf
      buf.Add("  " & name & " = Sub()" & If(ln.Comment <> "", "  " & VbComment(ln.Comment), ""))
      m_inGosub = True
      m_gosubStart = i
      Dim k = i
      Do While k <= [end]
        m_currentLine = m_lines(k).Number
        Try
          If k = i AndAlso m_lines(k).Tokens.Count = 0 Then
            k += 1
            Continue Do
          End If
          EmitLine(k)
        Catch ex As ConvertException
          Todo(m_lines(k), ex.Message)
        End Try
        k += 1
      Loop
      Flush(m_target)
      buf.Add("  End Sub")
      m_routines.Add(buf)
      m_inGosub = False
      m_target = saved
      Return [end] + 1
    End Function

    ' Error handlers that are module-level in QBasic are copied into the procedure (or GOSUB routine) that names them, so Resume works there.
    Private Sub EmitNeededHandlers(exitText As String)
      If m_neededHandlers.Count = 0 Then Return
      m_procLines.Add("  " & exitText)
      Dim done As New HashSet(Of Integer)
      For Each label In m_neededHandlers.ToList()
        Dim start As Integer
        If Not m_mainLabels.TryGetValue(label, start) Then Continue For
        Dim [end] = HandlerRegionEnd(start)
        If done.Contains(start) Then Continue For
        For k = start To [end]
          done.Add(k)
          Dim hl = m_lines(k)
          m_currentLine = hl.Number
          Try
            EmitLine(k)
          Catch ex As ConvertException
            Todo(hl, ex.Message)
          End Try
        Next
      Next
      Flush(m_target)
      m_neededHandlers.Clear()
    End Sub

    ' ---- Error handler regions ----------------------------------------------------------------------------------------

    ' The last line of the handler that starts at a label in the module-level code: up to the point where flow can't continue.
    Private Function HandlerRegionEnd(start As Integer) As Integer
      Dim blocks As New Stack(Of (HasElse As Boolean, AllTerm As Boolean, CurTerm As Boolean))
      Dim open = 0     ' open SELECT, DO, FOR and WHILE blocks
      Dim last = start
      For k = start To m_lines.Count - 1
        Dim ln = m_lines(k)
        Dim t = ln.Tokens
        If t.Count > 0 AndAlso (t(0).IsWord("SUB") OrElse t(0).IsWord("FUNCTION")) Then Return last
        last = k
        If t.Count = 0 Then Continue For
        Dim first = t(0).Upper()
        Dim terminal = first = "RESUME" OrElse first = "RETURN" OrElse first = "GOTO" OrElse (first = "END" AndAlso t.Count = 1) OrElse first = "SYSTEM"
        Dim isBlockIf = first = "IF" AndAlso t(t.Count - 1).IsWord("THEN")

        open += BlockDelta(t)
        If open < 0 Then open = 0

        If isBlockIf Then
          blocks.Push((False, True, False))
        ElseIf first = "ELSEIF" AndAlso blocks.Count > 0 Then
          Dim b = blocks.Pop()
          blocks.Push((b.HasElse, b.AllTerm AndAlso b.CurTerm, False))
        ElseIf first = "ELSE" AndAlso blocks.Count > 0 Then
          Dim b = blocks.Pop()
          blocks.Push((True, b.AllTerm AndAlso b.CurTerm, False))
        ElseIf first = "END" AndAlso t.Count > 1 AndAlso t(1).IsWord("IF") AndAlso blocks.Count > 0 Then
          Dim b = blocks.Pop()
          Dim allTerm = b.AllTerm AndAlso b.CurTerm
          Dim blockTerminates = b.HasElse AndAlso allTerm
          If blocks.Count = 0 Then
            If blockTerminates AndAlso open = 0 Then Return k
          ElseIf blockTerminates Then
            Dim parent = blocks.Pop()
            blocks.Push((parent.HasElse, parent.AllTerm, True))
          End If
        ElseIf terminal Then
          If blocks.Count = 0 Then
            If open = 0 Then Return k
          Else
            Dim b = blocks.Pop()
            blocks.Push((b.HasElse, b.AllTerm, True))
          End If
        End If
      Next
      Return last
    End Function

    ' How many SELECT, DO, FOR and WHILE blocks a line opens (minus those it closes).
    Private Shared Function BlockDelta(t As List(Of Token)) As Integer
      Dim delta = 0
      For q = 0 To t.Count - 1
        If q > 0 AndAlso Not (t(q - 1).Is_(":") OrElse t(q - 1).IsWord("THEN") OrElse t(q - 1).IsWord("ELSE")) Then Continue For
        Select Case t(q).Upper()
          Case "FOR", "DO", "WHILE" : delta += 1
          Case "SELECT" : If q + 1 < t.Count AndAlso t(q + 1).IsWord("CASE") Then delta += 1
          Case "LOOP", "WEND" : delta -= 1
          Case "END" : If q + 1 < t.Count AndAlso t(q + 1).IsWord("SELECT") Then delta -= 1
          Case "NEXT"
            Dim names = 1
            For r = q + 1 To t.Count - 1
              If t(r).Is_(":") Then Exit For
              If t(r).Is_(",") Then names += 1
            Next
            delta -= names
        End Select
      Next
      Return delta
    End Function

    ' ---- Declarations -------------------------------------------------------------------------------------------------

    Private Function HoistedConst(t As List(Of Token)) As List(Of String)
      Dim result As New List(Of String)
      Dim savedScope = m_scope
      m_scope = m_module
      For Each part In SplitTopLevel(t, 1, t.Count)
        If part.Count < 3 OrElse Not part(1).Is_("=") Then Continue For
        Dim v = ResolveVariableCore(part(0).Text, False)
        Dim ep As New ExprParser(part, 2, Me)
        Dim e = ep.ParseExpr()
        v.IsConst = True : v.IsShared = True
        If e.Qt = QT.String Then
          v.Qt = QT.String
          result.Add("Const " & ExprParser.VarText(v) & If(v.HasSuffix, "", " As String") & " = " & e.Text)
        Else
          Dim qty = e.Qt
          If Types.Rank(qty) < 0 Then qty = QT.Short
          v.Qt = If(v.HasSuffix, v.Qt, qty)
          result.Add("Const " & v.VbName & If(v.HasSuffix, Types.VbSuffix(v.Qt), " As " & Types.VbName(v.Qt)) & " = " & ExprParser.Coerce(e, v.Qt))
        End If
      Next
      m_scope = If(m_curProc Is Nothing, m_main, savedScope)
      Return result
    End Function

    ''' <summary>The VB for a DIM/REDIM/STATIC/COMMON statement.</summary>
    Private Function DimText(t As List(Of Token), keyword As String, hoist As Boolean) As String
      Dim p = 1
      If p < t.Count AndAlso t(p).IsWord("SHARED") Then p += 1
      Dim items As New List(Of String)
      Dim redims As New List(Of String)      ' REDIM of an array that already exists: resized, not declared again
      Dim fills As New List(Of String)       ' arrays of fixed-length strings are given their initial value
      Dim savedScope = m_scope
      If hoist Then m_scope = m_module
      For Each part In SplitTopLevel(t, p, t.Count)
        If part.Count = 0 Then Continue For
        Dim name = part(0).Text
        If part(0).Kind <> TokKind.Ident Then Throw New ConvertException("this isn't a QBasic DIM: " & ChrW(34) & part(0).Text & ChrW(34) & " isn't a variable name")
        Dim q = 1
        Dim dims As New List(Of String)
        Dim lowers As New List(Of Integer)
        Dim isArray = False
        If q < part.Count AndAlso part(q).Is_("(") Then
          isArray = True
          Dim depth = 0
          Dim startDims = q + 1
          Do While q < part.Count
            If part(q).Is_("(") Then depth += 1
            If part(q).Is_(")") Then depth -= 1
            q += 1
            If depth = 0 Then Exit Do
          Loop
          For Each dimPart In SplitTopLevel(part, startDims, q - 1)
            dims.Add(DimBound(dimPart))
            lowers.Add(LowerBoundOf(dimPart))
          Next
        End If
        Dim asType = QT.Unknown, userType = "", fixedLen = 0
        If q < part.Count AndAlso part(q).IsWord("AS") Then
          q += 1
          asType = ParseAsType(part, q, userType, fixedLen)
        End If
        If keyword = "ReDim" Then
          Dim existing = FindVariable(m_scope, name, True)
          If existing IsNot Nothing AndAlso (existing.IsShared OrElse existing.Declared OrElse Not m_scope.Locals.Contains(existing)) Then
            redims.Add(ExprParser.VarText(existing) & "(" & String.Join(", ", dims) & ")")
            If existing.FixedLen > 0 Then fills.Add("FixedString.Fill(" & ExprParser.VarText(existing) & ", " & existing.FixedLen & ")")
            Continue For
          End If
        End If
        ' A variable that a procedure SHAREs lives at module level: this DIM sizes it there instead of declaring a second one.
        If Not hoist AndAlso m_curProc Is Nothing AndAlso keyword <> "ReDim" Then
          Dim hasSfx As Boolean, bareName As String = ""
          Dim mq = TypeOfName(name, hasSfx, bareName)
          If asType <> QT.Unknown Then mq = asType
          Dim mv As VarSym = Nothing
          If m_module.Vars.TryGetValue(KeyOf(bareName, mq, isArray), mv) AndAlso mv.IsShared AndAlso Not mv.IsConst Then
            If userType <> "" Then mv.UserType = userType
            If fixedLen > 0 Then mv.FixedLen = fixedLen
            If isArray Then
              mv.Dims = String.Join(", ", dims) : mv.LowerBounds = lowers
              If dims.Any(Function(d) Not Integer.TryParse(d, Nothing)) Then
                mv.DeferRank = dims.Count
                redims.Add(ExprParser.VarText(mv) & "(" & mv.Dims & ")")
              End If
              If mv.FixedLen > 0 Then fills.Add("FixedString.Fill(" & ExprParser.VarText(mv) & ", " & mv.FixedLen & ")")
            End If
            Continue For
          End If
        End If
        Dim v = DeclareVariable(m_scope, name, isArray, asType, userType)
        v.Declared = True
        If fixedLen > 0 Then v.FixedLen = fixedLen
        If isArray Then v.Dims = String.Join(", ", dims) : v.LowerBounds = lowers
        ' GOSUB routines are lambdas at the top of the procedure: its locals must be declared there too, and sized where the DIM was.
        ' (The same for the program's own variables when GOSUB routines are procedures: they need module-level variables.)
        If ((m_curProc IsNot Nothing AndAlso m_procGosubTargets.Count > 0 AndAlso Not m_procStatic) OrElse (m_curProc Is Nothing AndAlso m_gosubTargets.Count > 0)) AndAlso Not hoist Then
          v.Declared = False
          If isArray Then
            v.DeferRank = dims.Count
            redims.Add(ExprParser.VarText(v) & "(" & String.Join(", ", dims) & ")")
            If v.FixedLen > 0 Then fills.Add("FixedString.Fill(" & ExprParser.VarText(v) & ", " & v.FixedLen & ")")
          End If
          Continue For
        End If
        Dim vbName = v.VbName & If(v.HasSuffix, Types.VbSuffix(v.Qt), "")
        Dim dimSuffix = If(isArray, "(" & String.Join(", ", dims) & ")", "")
        If v.UserType <> "" Then
          items.Add(vbName & dimSuffix & " As " & v.UserType)
        ElseIf v.HasSuffix Then
          items.Add(vbName & dimSuffix)
        ElseIf v.Qt = QT.String AndAlso fixedLen > 0 Then
          If isArray Then
            items.Add(vbName & dimSuffix & " As String")
            fills.Add("FixedString.Fill(" & vbName & ", " & fixedLen & ")")
          Else
            items.Add(vbName & " As String = FixedString.Blank(" & fixedLen & ")")
          End If
        Else
          items.Add(vbName & dimSuffix & " As " & Types.VbName(v.Qt))
        End If
      Next
      m_scope = savedScope
      Dim result As New List(Of String)
      If redims.Count > 0 Then result.Add("ReDim " & String.Join(", ", redims))
      If items.Count > 1 AndAlso items.Any(Function(x) Not x.Contains(" As ")) AndAlso items.Any(Function(x) x.Contains(" As ")) Then
        ' (a name with a type character takes the type of the declarator after it in VB, so each is declared on its own)
        For Each item In items
          result.Add(If(keyword = "ReDim", "Dim", keyword) & " " & item)
        Next
      ElseIf items.Count > 0 Then
        result.Add(If(keyword = "ReDim", "Dim", keyword) & " " & String.Join(", ", items))
      End If
      If hoist Then m_mainPrologue.AddRange(fills) Else result.AddRange(fills)
      Return String.Join(" : ", result)
    End Function

    ' The lower bound that a dimension was declared with (0 when it is only n, or isn't a number that is known).
    Private Function LowerBoundOf(toks As List(Of Token)) As Integer
      Dim toIdx = -1
      For k = 0 To toks.Count - 1
        If toks(k).IsWord("TO") Then toIdx = k : Exit For
      Next
      If toIdx < 0 Then Return 0
      Dim lo = New ExprParser(toks.GetRange(0, toIdx), 0, Me).ParseExpr()
      Return If(lo.IsLiteral, CInt(lo.LiteralValue), 0)
    End Function

    ' One dimension: "n" or "lo TO hi" (the array keeps a spare element when the lower bound is above 0).
    Private Function DimBound(toks As List(Of Token)) As String
      Dim toIdx = -1
      For k = 0 To toks.Count - 1
        If toks(k).IsWord("TO") Then toIdx = k : Exit For
      Next
      If toIdx < 0 Then
        Dim e = New ExprParser(toks, 0, Me).ParseExpr()
        Return ExprParser.Coerce(e, QT.Long)
      End If
      Dim lo = New ExprParser(toks.GetRange(0, toIdx), 0, Me).ParseExpr()
      Dim hi = New ExprParser(toks.GetRange(toIdx + 1, toks.Count - toIdx - 1), 0, Me).ParseExpr()
      If lo.IsLiteral AndAlso lo.LiteralValue < 0 Then
        ' (indexes are moved up by the lower bound where the array is used)
        Return If(hi.IsLiteral, CStr(CLng(hi.LiteralValue) - CLng(lo.LiteralValue)), ExprParser.Coerce(hi, QT.Long) & " + " & (-CLng(lo.LiteralValue)))
      ElseIf Not lo.IsLiteral Then
        Note("an array whose lower bound isn't a number the converter knows: check the indexes")
      End If
      Return ExprParser.Coerce(hi, QT.Long)
    End Function

    Private Shared Function ReaderCall(qty As QT) As String
      Select Case qty
        Case QT.Long : Return "ReadInt32()"
        Case QT.Single : Return "ReadSingle()"
        Case QT.Double : Return "ReadDouble()"
        Case Else : Return "ReadInt16()"
      End Select
    End Function

    Private Function EmitTypeBlock(i As Integer) As Integer
      Dim head = m_lines(i).Tokens
      Dim ts = m_types(head(1).Text)
      Dim lines As New List(Of String)
      Dim writes As New List(Of String)
      Dim reads As New List(Of String)
      lines.Add("Structure " & ts.QbName)
      lines.Add("  Implements IFileRecord")
      For Each f In ts.Fields
        Dim ftype = If(f.UserType <> "", f.UserType, Types.VbName(f.Qt))
        Dim hasSuffix As Boolean, bare As String = ""
        TypeOfName(f.Name, hasSuffix, bare)
        Dim fname = If(Keywords.IsVbReserved(bare), "[" & bare & "]", bare)
        Dim arrayHi As Integer
        If ts.ArrayHi.TryGetValue(f.Name, arrayHi) Then
          ' An array field: made when first used (a Structure has no constructor to do it), one spare element so that 1 TO n works.
          Dim backing = "m_" & bare
          Dim elem = If(f.Qt = QT.String, "String", ftype)
          lines.Add("  Private " & backing & "() As " & elem)
          lines.Add("  Public ReadOnly Property " & fname & " As " & elem & "()")
          lines.Add("    Get")
          lines.Add("      If " & backing & " Is Nothing Then")
          lines.Add("        " & backing & " = New " & elem & "(" & arrayHi & ") {}")
          If f.Qt = QT.String Then lines.Add("        FixedString.Fill(" & backing & ", " & If(f.Length > 0, f.Length, 0) & ")")
          lines.Add("      End If")
          lines.Add("      Return " & backing)
          lines.Add("    End Get")
          lines.Add("  End Property")
          Dim itemRef = fname & "(i)"
          writes.Add("    For i = " & ts.ArrayLo(f.Name) & " To " & arrayHi)
          reads.Add("    For i = " & ts.ArrayLo(f.Name) & " To " & arrayHi)
          If f.UserType <> "" Then
            writes.Add("      writer.Write(" & itemRef & ")") : reads.Add("      reader.Read(Of " & f.UserType & ")(" & fname & "(i))")
          ElseIf f.Qt = QT.String Then
            writes.Add("      writer.WriteString(" & itemRef & ", " & f.Length & ")") : reads.Add("      " & itemRef & " = reader.ReadString(" & f.Length & ")")
          Else
            writes.Add("      writer.Write(" & itemRef & ")")
            reads.Add("      " & itemRef & " = reader." & ReaderCall(f.Qt))
          End If
          writes.Add("    Next") : reads.Add("    Next")
          Note("a TYPE with an array field (" & ts.QbName & "." & bare & "): assigning one record to another shares the array")
          Continue For
        End If
        If f.Qt = QT.String AndAlso f.Length > 0 Then
          ' A fixed-length string: the property keeps it STRING * n (padded or cut when assigned; CHR$(0)s until then).
          Dim backing = "m_" & bare
          lines.Add("  Private " & backing & " As String")
          lines.Add("  Public Property " & fname & " As String")
          lines.Add("    Get")
          lines.Add("      Return If(" & backing & ", FixedString.Blank(" & f.Length & "))")
          lines.Add("    End Get")
          lines.Add("    Set(value As String)")
          lines.Add("      " & backing & " = FixedString.Fit(value, " & f.Length & ")")
          lines.Add("    End Set")
          lines.Add("  End Property")
          writes.Add("    writer.WriteString(" & fname & ", " & f.Length & ")")
          reads.Add("    " & fname & " = reader.ReadString(" & f.Length & ")")
        Else
          lines.Add("  Public " & fname & " As " & ftype)
          If f.UserType <> "" Then
            writes.Add("    writer.Write(" & fname & ")")
            reads.Add("    reader.Read(Of " & f.UserType & ")(" & fname & ")")
          ElseIf f.Qt = QT.Short Then
            writes.Add("    writer.Write(" & fname & ")")
            reads.Add("    " & fname & " = reader.ReadInt16()")
          ElseIf f.Qt = QT.Long Then
            writes.Add("    writer.Write(" & fname & ")")
            reads.Add("    " & fname & " = reader.ReadInt32()")
          ElseIf f.Qt = QT.Single Then
            writes.Add("    writer.Write(" & fname & ")")
            reads.Add("    " & fname & " = reader.ReadSingle()")
          ElseIf f.Qt = QT.Double Then
            writes.Add("    writer.Write(" & fname & ")")
            reads.Add("    " & fname & " = reader.ReadDouble()")
          End If
        End If
      Next
      lines.Add("  Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo")
      lines.AddRange(writes)
      lines.Add("  End Sub")
      lines.Add("  Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom")
      lines.AddRange(reads)
      lines.Add("  End Sub")
      lines.Add("End Structure")
      m_hoisted.AddRange(lines)
      m_hoisted.Add("")
      Dim k = i + 1
      Do While k < m_lines.Count
        Dim t = m_lines(k).Tokens
        If t.Count >= 2 AndAlso t(0).IsWord("END") AndAlso t(1).IsWord("TYPE") Then Exit Do
        k += 1
      Loop
      Flush(m_target)
      m_target.Add("' TYPE " & ts.QbName & " (see the Structure at the top)")
      Return k + 1
    End Function

    ' ---- The end ------------------------------------------------------------------------------------------------------

    Private Sub FinishModule()
      Flush(If(m_curProc Is Nothing, m_mainLines, m_procLines))
      Dim mainDecls = If(m_gosubTargets.Count > 0, m_hoisted, m_mainDecls)   ' GOSUB routines are procedures that share the main variables
      ' Module-level variables that are shared only because a procedure says so, and were never DIMmed.
      For Each v In m_module.Locals
        If Not v.Declared AndAlso Not v.IsConst Then
          m_hoisted.Add("Dim " & DeclItemInit(v))
          v.Declared = True
        End If
      Next
      For Each v In m_main.Locals
        If Not v.Declared AndAlso Not v.IsConst AndAlso Not v.IsParam Then
          mainDecls.Add("Dim " & DeclItemInit(v))
          v.Declared = True
        End If
      Next
    End Sub

  End Class

End Namespace
