Option Explicit On
Option Strict On
Option Infer On

Imports System.Text

Namespace QB2VB

  Partial Friend Class Converter

    Private ReadOnly m_mainDecls As New List(Of String)
    Private ReadOnly m_sharedNames As New List(Of String)
    Private ReadOnly m_mainLabels As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly m_neededHandlers As New List(Of String)
    Private m_procDeclAt As Integer
    Private m_procBodyStart As Integer
    Private m_procStatic As Boolean
    Private ReadOnly m_gosubTargets As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private m_inGosub As Boolean
    Private ReadOnly m_labelOwner As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)   ' label -> the line where its GOSUB routine starts, or -1 for the program itself

    ' ---- Pass 1: what the program declares ---------------------------------------------------------------

    Private Function ParseAsType(toks As List(Of Token), ByRef p As Integer, ByRef userType As String, ByRef fixedLen As Integer) As QT
      ' At the word after AS.
      userType = "" : fixedLen = 0
      If p >= toks.Count Then Return QT.Unknown
      Dim w = toks(p).Upper()
      p += 1
      Dim t As QT
      Select Case w
        Case "INTEGER" : t = QT.Short
        Case "LONG" : t = QT.Long
        Case "SINGLE" : t = QT.Single
        Case "DOUBLE" : t = QT.Double
        Case "STRING"
          t = QT.String
          If p < toks.Count AndAlso toks(p).Is_("*") Then
            p += 1
            If p < toks.Count Then
              Dim n As Integer
              If Integer.TryParse(toks(p).Text, n) Then fixedLen = n Else fixedLen = -1
              p += 1
            End If
          End If
        Case Else
          t = QT.User
          userType = toks(p - 1).Text
      End Select
      Return t
    End Function

    Private Sub Pass1()

      Dim inProc As ProcSym = Nothing
      Dim procScope As Scope = Nothing

      For i = 0 To m_lines.Count - 1
        Dim ln = m_lines(i)
        m_currentLine = ln.Number
        If ln.Label <> "" AndAlso inProc Is Nothing Then m_mainLabels(ln.Label) = i
        If ln.LineNumber <> "" AndAlso inProc Is Nothing Then m_mainLabels(ln.LineNumber) = i
        Dim t = ln.Tokens
        If t.Count = 0 Then Continue For
        Dim first = t(0).Upper()

        ' The labels that module-level GOSUBs call (they become procedures).
        If inProc Is Nothing Then
          For k = 0 To t.Count - 1
            If t(k).IsWord("GOSUB") Then
              Dim q = k + 1
              Do While q < t.Count AndAlso (t(q).Kind = TokKind.Ident OrElse t(q).Kind = TokKind.Number)
                m_gosubTargets.Add(t(q).Text)
                q += 1
                If q < t.Count AndAlso t(q).Is_(",") Then q += 1 Else Exit Do
              Loop
            End If
          Next
        End If

        Try
          Select Case first
            Case "DEFINT", "DEFSNG", "DEFDBL", "DEFLNG", "DEFSTR"
              ParseDef(t, first)

            Case "DEF"
              If t.Count > 3 AndAlso Not t(1).IsWord("SEG") Then
                Dim eq = -1
                Dim depth = 0
                For k = 2 To t.Count - 1
                  If t(k).Is_("(") Then depth += 1
                  If t(k).Is_(")") Then depth -= 1
                  If depth = 0 AndAlso t(k).Is_("=") Then eq = k : Exit For
                Next
                If eq > 0 Then
                  Dim sym = ParseProcHeader(t.GetRange(0, eq), 1, True)
                  sym.IsDefFn = True : sym.HeaderLine = i
                  sym.DefBody = t.GetRange(eq + 1, t.Count - eq - 1)
                  m_procs(sym.QbName) = sym
                  m_procScopes(sym.QbName) = New Scope(m_module) With {.Proc = sym}
                End If
              End If

            Case "FIELD"
              RegisterField(i, t, If(inProc IsNot Nothing, procScope, m_main))

            Case "DECLARE"
              If t.Count > 2 Then
                Dim isFn = t(1).IsWord("FUNCTION")
                Dim sym = ParseProcHeader(t, 2, isFn)
                If Not m_procs.ContainsKey(sym.QbName) Then m_procs(sym.QbName) = sym
              End If

            Case "SUB", "FUNCTION"
              If t.Count > 1 Then
                Dim sym = ParseProcHeader(t, 1, first = "FUNCTION")
                sym.HeaderLine = i
                m_procs(sym.QbName) = sym
                inProc = sym
                procScope = New Scope(m_module) With {.Proc = sym}
                m_procScopes(sym.QbName) = procScope
              End If

            Case "END"
              If t.Count > 1 AndAlso (t(1).IsWord("SUB") OrElse t(1).IsWord("FUNCTION")) Then inProc = Nothing : procScope = Nothing

            Case "TYPE"
              ParseTypeBlock(i)

            Case "DATA"
              m_dataLines.Add(i)

            Case "CONST"
              m_scope = m_module
              RegisterConsts(t)

            Case "SHARED"
              If inProc IsNot Nothing Then
                For Each part In SplitTopLevel(t, 1, t.Count)
                  If part.Count > 0 Then m_sharedNames.Add(part(0).Text & If(part.Count > 1 AndAlso part(1).Is_("("), "()", ""))
                Next
              End If

            Case "DIM", "REDIM", "COMMON", "STATIC"
              Dim scope As Scope = If(inProc IsNot Nothing, procScope, m_main)
              RegisterDims(t, scope, first = "STATIC", first = "REDIM")

          End Select
        Catch ex As ConvertException
          Note(ex.Message)
        End Try
      Next

      ' A name that a procedure SHARES is module-level for the whole program.
      For Each shared_ In m_sharedNames
        Dim isArray = shared_.EndsWith("()", StringComparison.Ordinal)
        Dim name = shared_.Replace("()", "")
        Dim hasSuffix As Boolean, bare As String = ""
        Dim qty = TypeOfName(name, hasSuffix, bare)
        Dim key = KeyOf(bare, qty, isArray)
        Dim v As VarSym = Nothing
        If m_main.Vars.TryGetValue(key, v) Then
          m_main.Vars.Remove(key)
          m_main.Locals.Remove(v)
          m_module.Vars(key) = v
          m_module.Locals.Add(v)
          v.IsShared = True
        ElseIf Not m_module.Vars.ContainsKey(key) Then
          m_scope = m_module
          Dim nv = DeclareVariable(m_module, name, isArray)
          nv.IsShared = True
        End If
      Next

      ' Which GOSUB routine (if any) each label of the program's own code belongs to.
      For Each label In m_mainLabels.Keys
        m_labelOwner(label) = -1
      Next
      For Each target In m_gosubTargets
        Dim start As Integer
        If Not m_mainLabels.TryGetValue(target, start) Then Continue For
        For q = start To HandlerRegionEnd(start)
          If m_lines(q).Label <> "" Then m_labelOwner(m_lines(q).Label) = start
          If m_lines(q).LineNumber <> "" Then m_labelOwner(m_lines(q).LineNumber) = start
        Next
      Next

    End Sub

    Private Sub ParseDef(t As List(Of Token), word As String)
      Dim qty As QT
      Select Case word
        Case "DEFINT" : qty = QT.Short
        Case "DEFLNG" : qty = QT.Long
        Case "DEFSNG" : qty = QT.Single
        Case "DEFDBL" : qty = QT.Double
        Case Else : qty = QT.String
      End Select
      Dim p = 1
      Do While p < t.Count
        Dim a = t(p).Text
        p += 1
        Dim b = a
        If p + 1 < t.Count AndAlso t(p).Is_("-") Then
          b = t(p + 1).Text
          p += 2
        End If
        If a.Length > 0 AndAlso b.Length > 0 Then
          For c = AscW(Char.ToUpperInvariant(a(0))) To AscW(Char.ToUpperInvariant(b(0)))
            If c >= AscW("A"c) AndAlso c <= AscW("Z"c) Then m_defType(c - AscW("A"c)) = qty
          Next
        End If
        If p < t.Count AndAlso t(p).Is_(",") Then p += 1
      Loop
    End Sub

    ''' <summary>SUB name [(params)] [STATIC] at token index <paramref name="start"/> (the name).</summary>
    Private Function ParseProcHeader(t As List(Of Token), start As Integer, isFunction As Boolean) As ProcSym
      Dim nameTok = t(start)
      Dim hasSuffix As Boolean, bare As String = ""
      Dim qty = TypeOfName(nameTok.Text, hasSuffix, bare)
      If qty = QT.Short Then hasSuffix = False      ' VB has no suffix for a 16 bit Short
      Dim sym As New ProcSym With {.QbName = bare, .IsFunction = isFunction, .RetQt = If(isFunction, qty, QT.Unknown), .HasSuffix = hasSuffix}
      Dim p = start + 1
      If p < t.Count AndAlso t(p).Is_("(") Then
        p += 1
        Do While p < t.Count AndAlso Not t(p).Is_(")")
          Dim prm As New ParamSym
          Dim pn = t(p).Text
          p += 1
          Dim pHasSuffix As Boolean, pBare As String = ""
          prm.Qt = TypeOfName(pn, pHasSuffix, pBare)
          prm.QbName = pBare : prm.HasSuffix = pHasSuffix AndAlso prm.Qt <> QT.Short
          If p + 1 < t.Count AndAlso t(p).Is_("(") AndAlso t(p + 1).Is_(")") Then
            prm.IsArray = True
            p += 2
          End If
          If p < t.Count AndAlso t(p).IsWord("AS") Then
            p += 1
            Dim ut As String = "", fl As Integer
            prm.Qt = ParseAsType(t, p, ut, fl)
            prm.UserType = ut
          End If
          sym.Params.Add(prm)
          If p < t.Count AndAlso t(p).Is_(",") Then p += 1
        Loop
        p += 1
      End If
      If p < t.Count AndAlso t(p).IsWord("STATIC") Then sym.IsStatic = True
      Return sym
    End Function

    Private Function SplitTopLevel(t As List(Of Token), start As Integer, [end] As Integer) As List(Of List(Of Token))
      Dim parts As New List(Of List(Of Token))
      Dim cur As New List(Of Token)
      Dim depth = 0
      For i = start To [end] - 1
        Dim tk = t(i)
        If tk.Is_("(") Then depth += 1
        If tk.Is_(")") Then depth -= 1
        If depth = 0 AndAlso tk.Is_(",") Then
          parts.Add(cur) : cur = New List(Of Token)
        Else
          cur.Add(tk)
        End If
      Next
      parts.Add(cur)
      Return parts
    End Function

    Private Sub RegisterConsts(t As List(Of Token))
      For Each part In SplitTopLevel(t, 1, t.Count)
        If part.Count < 3 OrElse Not part(1).Is_("=") Then Continue For
        Dim v = DeclareVariable(m_module, part(0).Text, False)
        v.IsConst = True : v.IsShared = True
        m_module.Locals.Remove(v)
      Next
    End Sub

    ' FIELD #n, w AS a$, ...: a FieldRecord variable holds the buffer, and each name is one of its fields.
    Private ReadOnly m_fieldRecords As New Dictionary(Of Integer, String)

    Private Sub RegisterField(lineIndex As Integer, t As List(Of Token), scope As Scope)
      Dim n = 1
      Dim recName As String
      Do
        recName = "Fields" & n
        n += 1
      Loop While scope.Names.Contains(recName.ToLowerInvariant()) OrElse m_module.Names.Contains(recName.ToLowerInvariant())
      scope.Names.Add(recName.ToLowerInvariant())
      m_fieldRecords(lineIndex) = recName
      Dim rec As New VarSym With {.QbName = recName, .Qt = QT.Unknown, .UserType = "FieldRecord", .VbName = recName}
      scope.Locals.Add(rec)
      Dim parts = SplitTopLevel(t, 1, t.Count)
      Dim index = 0
      For k = 1 To parts.Count - 1
        Dim part = parts(k)
        If part.Count < 3 OrElse Not part(part.Count - 2).IsWord("AS") Then Continue For
        Dim v = DeclareVariable(scope, part(part.Count - 1).Text, False)
        scope.Locals.Remove(v)
        v.FieldVar = recName : v.FieldIndex = index
        v.VbName = recName & "(" & index & ")" : v.HasSuffix = False : v.Declared = True
        index += 1
      Next
    End Sub

    Private Sub RegisterDims(t As List(Of Token), scope As Scope, isStatic As Boolean, Optional isRedim As Boolean = False)
      Dim p = 1
      Dim isShared = False
      If p < t.Count AndAlso t(p).IsWord("SHARED") Then
        isShared = True
        p += 1
      End If
      Dim target = If(isShared AndAlso scope Is m_main, m_module, scope)
      For Each part In SplitTopLevel(t, p, t.Count)
        If part.Count = 0 Then Continue For
        Dim name = part(0).Text
        Dim q = 1
        Dim isArray = False
        If q < part.Count AndAlso part(q).Is_("(") Then
          isArray = True
          Dim depth = 0
          Do While q < part.Count
            If part(q).Is_("(") Then depth += 1
            If part(q).Is_(")") Then depth -= 1
            q += 1
            If depth = 0 Then Exit Do
          Loop
        End If
        Dim asType = QT.Unknown, userType = "", fixedLen = 0
        If q < part.Count AndAlso part(q).IsWord("AS") Then
          q += 1
          asType = ParseAsType(part, q, userType, fixedLen)
        End If
        ' REDIM of an array that is already declared (a shared one, say) resizes it.
        If isRedim AndAlso FindVariable(scope, name, True) IsNot Nothing Then Continue For
        Dim v = DeclareVariable(target, name, isArray, asType, userType)
        If isShared Then v.IsShared = True
        If fixedLen <> 0 Then v.FixedLen = fixedLen
      Next
    End Sub

    Private Sub ParseTypeBlock(startIndex As Integer)
      Dim head = m_lines(startIndex).Tokens
      If head.Count < 2 Then Return
      Dim ts As New TypeSym With {.QbName = head(1).Text}
      Dim i = startIndex + 1
      Do While i < m_lines.Count
        Dim t = m_lines(i).Tokens
        If t.Count >= 2 AndAlso t(0).IsWord("END") AndAlso t(1).IsWord("TYPE") Then Exit Do
        If t.Count >= 3 AndAlso t(1).IsWord("AS") Then
          Dim p = 2
          Dim ut As String = "", fl As Integer
          Dim qty = ParseAsType(t, p, ut, fl)
          ts.Fields.Add((t(0).Text, qty, fl, ut))
        End If
        i += 1
      Loop
      m_types(ts.QbName) = ts
    End Sub

  End Class

End Namespace
