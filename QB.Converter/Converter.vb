Option Explicit On
Option Strict On
Option Infer On

Imports System.Text

Namespace QB2VB

  Friend Class Scope
    Public ReadOnly Parent As Scope
    Public ReadOnly Vars As New Dictionary(Of String, VarSym)
    Public ReadOnly Locals As New List(Of VarSym)          ' in the order they were met
    Public Proc As ProcSym
    Public ReadOnly Names As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)   ' the VB names in use
    Public Sub New(parent As Scope)
      Me.Parent = parent
    End Sub
  End Class

  ''' <summary>Converts a QBasic program to VB that uses the QB.Compatibility library, line by line, keeping the program as it was.</summary>
  Partial Friend Class Converter
    Implements IContext

    Private ReadOnly m_lines As List(Of SourceLine)
    Private ReadOnly m_defType(25) As QT
    Private ReadOnly m_procs As New Dictionary(Of String, ProcSym)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly m_types As New Dictionary(Of String, TypeSym)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly m_module As New Scope(Nothing)    ' shared variables and constants
    Private ReadOnly m_main As Scope                   ' the module-level code's own variables
    Private ReadOnly m_procScopes As New Dictionary(Of String, Scope)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly m_dataLines As New List(Of Integer)
    Private m_scope As Scope
    Private m_curProc As ProcSym
    Private ReadOnly m_vbNames As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly m_notes As New List(Of String)
    Private m_currentLine As Integer

    ''' <summary>Things the converter couldn't convert or wants checked (also written as TODO comments in the output).</summary>
    ''' <summary>When set, the program starts by setting QB.Core.EmptyLoopMicroseconds to this (how long an empty loop pass takes).</summary>
    Public Property EmptyLoopMicroseconds As Double?

    Public ReadOnly Property Notes As List(Of String)
      Get
        Return m_notes
      End Get
    End Property

    Public Sub New(source As String)
      m_lines = Lexer.Split(source)
      m_main = New Scope(m_module)
      For i = 0 To 25
        m_defType(i) = QT.Single
      Next
    End Sub

    ''' <summary>The program draws, reads the joystick or the pen, or uses SCREEN 1 and up: it needs the graphics display (QB.Graphics).</summary>
    Public ReadOnly Property UsesGraphics As Boolean

    ''' <summary>The program uses SOUND or PLAY (the system audio plays them: nothing extra is referenced).</summary>
    Public ReadOnly Property UsesSound As Boolean

    Friend Sub Feature(kind As String) Implements IContext.Feature
      If kind = "graphics" Then m_usesGraphics = True
      If kind = "sound" Then m_usesSound = True
      If kind = "serial" Then m_usesSerial = True
    End Sub

    ''' <summary>The program opens a serial port (OPEN "COM1:...", COM(n) events): it needs QB.Serial.</summary>
    ''' <summary>True when the namespaces the program uses are imported by its project (see Imports) instead of by the file.</summary>
    Public Property OmitImports As Boolean

    Private _Imports As String() = Array.Empty(Of String)()

    ''' <summary>The namespaces that the converted program uses (known after Convert).</summary>
    Public ReadOnly Property [Imports] As IReadOnlyList(Of String)
      Get
        Return _Imports
      End Get
    End Property

    Public ReadOnly Property UsesSerial As Boolean

    Private m_usesSerial As Boolean
    Private m_usesGraphics As Boolean
    Private m_usesSound As Boolean

    Private Sub Note(message As String) Implements IContext.Note
      m_notes.Add($"line {m_currentLine}: {message}")
    End Sub

    ' ---- Names and types --------------------------------------------------------------------------------

    Private Shared ReadOnly s_shadowRisk As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
      "Left", "Right", "Mid", "Len", "Str", "Val", "Asc", "Space", "Abs", "Int", "Fix", "Sin", "Cos", "Tan", "Log", "Exp", "Sqrt", "Sign",
      "Atan", "Hex", "Oct", "LCase", "UCase", "LTrim", "RTrim", "InStr", "Rnd", "Timer", "DateString", "TimeString", "QBChr", "QBString",
      "CShort", "CInt", "CSng", "CDbl", "CStr", "Math", "System", "Err", "Erl", "Console", "File", "Printer", "Video", "Events", "Devices",
      "Development", "Multimedia", "Core", "Main", "Program"}

    Private Function DefaultType(name As String) As QT
      Dim c = Char.ToUpperInvariant(name(0))
      If c >= "A"c AndAlso c <= "Z"c Then Return m_defType(AscW(c) - AscW("A"c))
      Return QT.Single
    End Function

    ''' <summary>The type of a name as written (suffix, else the DEF default).</summary>
    Private Function TypeOfName(name As String, ByRef hasSuffix As Boolean, ByRef bare As String) As QT
      hasSuffix = False
      bare = name
      If name.Length > 1 Then
        Dim t = Types.FromSuffix(name(name.Length - 1))
        If t <> QT.Unknown Then
          hasSuffix = True
          bare = name.Substring(0, name.Length - 1)
          Return t
        End If
      End If
      Return DefaultType(name)
    End Function

    ' An array and a simple variable of the same name are different variables in QBasic.
    Private Shared Function KeyOf(bare As String, qty As QT, Optional isArray As Boolean = False) As String
      Return bare.ToLowerInvariant() & "|" & Types.Suffix(qty) & If(isArray, "()", "")
    End Function

    Private Function ChooseVbName(bare As String, qty As QT, hasSuffix As Boolean, scope As Scope) As String
      Dim n = bare.Replace("."c, "_"c)    ' (QBasic names may have dots; VB's can't)
      If Keywords.IsVbReserved(n) OrElse s_shadowRisk.Contains(n) OrElse m_procs.ContainsKey(n) Then
        n = If(hasSuffix OrElse s_shadowRisk.Contains(n) OrElse m_procs.ContainsKey(n), n & "_", "[" & n & "]")
      End If
      ' The same name with another type is another variable in QBasic but the same in VB: tell them apart.
      Dim candidate = n
      Dim bareKey = candidate.Trim("["c, "]"c).ToLowerInvariant()
      If InUse(scope, bareKey) Then
        Dim tag = Select_(qty)
        candidate = n.TrimEnd("]"c) & "_" & tag & If(n.EndsWith("]", StringComparison.Ordinal), "]", "")
        bareKey = candidate.Trim("["c, "]"c).ToLowerInvariant()
        Dim i = 2
        Do While InUse(scope, bareKey)
          candidate = n.TrimEnd("]"c) & "_" & tag & i & If(n.EndsWith("]", StringComparison.Ordinal), "]", "")
          bareKey = candidate.Trim("["c, "]"c).ToLowerInvariant()
          i += 1
        Loop
      End If
      scope.Names.Add(bareKey)
      Return candidate
    End Function

    ' A name is taken when this scope or (for a procedure) the module already uses it.
    Private Function InUse(scope As Scope, key As String) As Boolean
      Return scope.Names.Contains(key) OrElse (scope IsNot m_module AndAlso m_module.Names.Contains(key))
    End Function

    Private Shared Function Select_(qty As QT) As String
      Select Case qty
        Case QT.Short : Return "Int"
        Case QT.Long : Return "Lng"
        Case QT.Single : Return "Sng"
        Case QT.Double : Return "Dbl"
        Case QT.String : Return "Str"
        Case Else : Return "Var"
      End Select
    End Function

    ' ---- Scopes -----------------------------------------------------------------------------------------

    Private Function Lookup(scope As Scope, key As String) As VarSym
      Dim s = scope
      Dim v As VarSym = Nothing
      Do While s IsNot Nothing
        If s.Vars.TryGetValue(key, v) Then Return v
        s = s.Parent
      Loop
      Return Nothing
    End Function

    ''' <summary>The declared type of an unsuffixed name (DIM x AS STRING), else its DEF type.</summary>
    Private Function ResolveType(name As String, ByRef hasSuffix As Boolean, ByRef bare As String) As QT
      Dim t = TypeOfName(name, hasSuffix, bare)
      If Not hasSuffix Then
        ' A name declared with AS keeps that type everywhere.
        Dim s = m_scope
        Do While s IsNot Nothing
          For Each v In s.Vars.Values
            If v.QbName.Equals(bare, StringComparison.OrdinalIgnoreCase) AndAlso Not v.HasSuffix AndAlso v.UserType <> "" AndAlso False Then Return v.Qt
          Next
          s = s.Parent
        Loop
        Dim declared = LookupDeclaredAs(bare)
        If declared IsNot Nothing Then Return declared.Qt
      End If
      Return t
    End Function

    Private Function LookupDeclaredAs(bare As String) As VarSym
      Dim s = m_scope
      Do While s IsNot Nothing
        For Each v In s.Vars.Values
          If Not v.HasSuffix AndAlso v.QbName.Equals(bare, StringComparison.OrdinalIgnoreCase) AndAlso v.DeclaredAs Then Return v
        Next
        s = s.Parent
      Loop
      Return Nothing
    End Function

    Friend Function DeclareVariable(scope As Scope, name As String, isArray As Boolean, Optional asType As QT = QT.Unknown, Optional userType As String = "", Optional isParam As Boolean = False) As VarSym
      Dim hasSuffix As Boolean, bare As String = ""
      Dim qty = TypeOfName(name, hasSuffix, bare)
      Dim declaredAs = False
      If asType <> QT.Unknown Then
        qty = asType : declaredAs = True : hasSuffix = False
      End If
      Dim key = KeyOf(bare, qty, isArray)
      Dim v As VarSym = Nothing
      If scope.Vars.TryGetValue(key, v) Then
        If isArray Then v.IsArray = True
        Return v
      End If
      If qty = QT.Short Then hasSuffix = False      ' VB has no suffix for a 16 bit Short (a LONG gets VB's %)
      v = New VarSym With {.QbName = bare, .Qt = qty, .HasSuffix = hasSuffix, .IsArray = isArray, .IsParam = isParam, .UserType = userType, .DeclaredAs = declaredAs}
      v.VbName = ChooseVbName(bare, qty, hasSuffix, scope)
      scope.Vars(key) = v
      If Not isParam Then scope.Locals.Add(v)
      Return v
    End Function

    Private Function ResolveVariableCore(name As String, isArray As Boolean) As VarSym Implements IContext.ResolveVariable
      Dim hasSuffix As Boolean, bare As String = ""
      Dim qty = ResolveType(name, hasSuffix, bare)
      Dim key = KeyOf(bare, qty, isArray)
      Dim v = Lookup(m_scope, key)
      If v IsNot Nothing Then
        If isArray Then v.IsArray = True
        Return v
      End If
      ' A record member: rec.field.
      Dim dot = name.IndexOf("."c)
      If dot > 0 Then
        Dim parts = name.Split("."c)
        Dim rv = FindRecord(parts(0))
        If rv IsNot Nothing Then
          ' rec.field, and a field of a field (rec.inner.field).
          Dim userType = rv.UserType, vbName = rv.VbName, fieldType = QT.Unknown, found = True
          For k = 1 To parts.Length - 1
            Dim fq As QT, fut As String = ""
            If Not FieldOfCore(userType, parts(k), fq, fut) Then found = False : Exit For
            vbName &= "." & ExprParser.FieldName(parts(k))
            fieldType = fq : userType = fut
          Next
          If found Then Return New VarSym With {.QbName = name, .Qt = fieldType, .VbName = vbName, .HasSuffix = False, .UserType = userType}
          fieldType = FieldTypeOf(rv, name.Substring(dot + 1))
          Return New VarSym With {.QbName = name, .Qt = fieldType, .VbName = rv.VbName & "." & ExprParser.FieldName(name.Substring(dot + 1)), .HasSuffix = False}
        End If
      End If
      ' New implicit variable: local to where it is used.
      Dim target = If(m_scope Is Nothing, m_main, m_scope)
      v = DeclareVariable(target, name, isArray)
      If isArray AndAlso v.Dims = "" Then v.Dims = "10"   ' QBasic's implicit arrays run from 0 to 10
      Return v
    End Function

    ''' <summary>An existing variable of this name that the scope can see (null when there isn't one).</summary>
    Private Function FindVariable(scope As Scope, name As String, Optional isArray As Boolean = False) As VarSym
      Dim hasSuffix As Boolean, bare As String = ""
      Dim saved = m_scope
      m_scope = scope
      Try
        Dim qty = ResolveType(name, hasSuffix, bare)
        Return Lookup(scope, KeyOf(bare, qty, isArray))
      Finally
        m_scope = saved
      End Try
    End Function

    Private Function FindRecord(recName As String) As VarSym
      Dim hasSuffix As Boolean, bare As String = ""
      Dim t = TypeOfName(recName, hasSuffix, bare)
      Dim s = m_scope
      Do While s IsNot Nothing
        For Each v In s.Vars.Values
          If v.QbName.Equals(bare, StringComparison.OrdinalIgnoreCase) AndAlso v.UserType <> "" Then Return v
        Next
        s = s.Parent
      Loop
      Return Nothing
    End Function

    Private Function FieldTypeOf(rec As VarSym, field As String) As QT
      Dim ts As TypeSym = Nothing
      If m_types.TryGetValue(rec.UserType, ts) Then
        For Each f In ts.Fields
          If f.Name.Equals(field, StringComparison.OrdinalIgnoreCase) Then Return f.Qt
        Next
      End If
      Dim hasSuffix As Boolean, bare As String = ""
      Return TypeOfName(field, hasSuffix, bare)
    End Function

    Private Function IsArrayCore(name As String) As Boolean Implements IContext.IsArray
      Dim hasSuffix As Boolean, bare As String = ""
      Dim qty = ResolveType(name, hasSuffix, bare)
      Dim v = Lookup(m_scope, KeyOf(bare, qty, True))
      If v IsNot Nothing Then Return v.IsArray
      ' rec.field where the field is an array of the record's TYPE
      Dim dot = name.LastIndexOf("."c)
      If dot > 0 Then
        Dim rv = FindRecord(name.Substring(0, name.IndexOf("."c)))
        If rv IsNot Nothing Then
          Dim ut = rv.UserType
          Dim parts = name.Split("."c)
          For k = 1 To parts.Length - 1
            Dim ts As TypeSym = Nothing
            If ut = "" OrElse Not m_types.TryGetValue(ut, ts) Then Return False
            If k = parts.Length - 1 Then
              Dim fh As Boolean, fb As String = ""
              TypeOfName(parts(k), fh, fb)
              Return ts.ArrayHi.ContainsKey(fb)
            End If
            Dim fq As QT, fut As String = ""
            If Not FieldOfCore(ut, parts(k), fq, fut) Then Return False
            ut = fut
          Next
        End If
      End If
      Return False
    End Function

    Private Function FindProcCore(name As String) As ProcSym Implements IContext.FindProc
      Dim hasSuffix As Boolean, bare As String = ""
      TypeOfName(name, hasSuffix, bare)
      Dim p As ProcSym = Nothing
      m_procs.TryGetValue(bare, p)
      Return p
    End Function

    Private Function FieldOfCore(userType As String, field As String, ByRef fieldType As QT, ByRef fieldUserType As String) As Boolean Implements IContext.FieldOf
      Dim ts As TypeSym = Nothing
      If Not m_types.TryGetValue(userType, ts) Then Return False
      Dim hasSuffix As Boolean, bare As String = ""
      TypeOfName(field, hasSuffix, bare)
      For Each f In ts.Fields
        Dim fh As Boolean, fb As String = ""
        TypeOfName(f.Name, fh, fb)
        If fb.Equals(bare, StringComparison.OrdinalIgnoreCase) Then
          fieldType = f.Qt : fieldUserType = f.UserType
          Return True
        End If
      Next
      Return False
    End Function

    Private Function FindTypeCore(name As String) As TypeSym Implements IContext.FindType
      Dim t As TypeSym = Nothing
      m_types.TryGetValue(name, t)
      Return t
    End Function

    ' ---- Output -----------------------------------------------------------------------------------------

    Private ReadOnly m_hoisted As New List(Of String)
    Private ReadOnly m_mainPrologue As New List(Of String)   ' statements at the start of Main (arrays of fixed-length strings are filled there)
    Private ReadOnly m_mainLines As New List(Of String)
    Private ReadOnly m_procLines As New List(Of String)
    Private m_target As List(Of String)
    Private ReadOnly m_pending As New List(Of String)

    Private Sub Flush(target As List(Of String))
      target.AddRange(m_pending)
      m_pending.Clear()
    End Sub

    ''' <summary>The VB module for the program.</summary>
    Public Function Convert(moduleName As String) As String

      Pass1()

      m_scope = m_main
      m_curProc = Nothing
      m_target = m_mainLines

      Pass2()
      _UsesGraphics = m_usesGraphics
      _UsesSound = m_usesSound
      _UsesSerial = m_usesSerial

      Dim sb As New StringBuilder
      sb.AppendLine("' Converted from QBasic by qb2vb.TODO_HEADER")
      sb.AppendLine()
      _Imports = {"System.Math", "QB", "QB.Core", "QB.Development", "QB.Devices", "QB.Events", If(m_usesSerial, "QB.Serial", "QB.File"), "QB.Multimedia", "QB.Printer", If(m_usesGraphics, "QB.Graphics", "QB.Video")}
      If Not OmitImports Then
        For Each name In _Imports
          sb.AppendLine("Imports " & name)
        Next
        sb.AppendLine()
      End If
      sb.AppendLine("Module " & moduleName)
      sb.AppendLine()
      For Each l In m_hoisted
        sb.AppendLine("  " & l)
      Next
      If m_hoisted.Count > 0 Then sb.AppendLine()
      sb.AppendLine("  Public Sub Main()")
      sb.AppendLine(If(m_usesGraphics, "    QB.Graphics.WaitForKeyAtExit = True", "    QB.Video.WaitForKeyAtExit = True"))
      If Not m_usesGraphics Then
        ' A text program built as WinExe has no console: its project defines QB_WINDOW (and references the SDL assembly), and the text screen is shown in a window.
        sb.AppendLine("#If QB_WINDOW Then")
        sb.AppendLine("    QB.SdlBackend.Use()")
        sb.AppendLine("#End If")
      End If
      For Each l In m_mainPrologue
        sb.AppendLine("    " & l)
      Next
      If EmptyLoopMicroseconds.HasValue Then sb.AppendLine("    EmptyLoopMicroseconds = " & EmptyLoopMicroseconds.Value.ToString(Globalization.CultureInfo.InvariantCulture))
      For Each l In m_mainDecls
        sb.AppendLine("    " & l)
      Next
      For Each l In m_mainLines
        sb.AppendLine("    " & l)
      Next
      sb.AppendLine("  End Sub")
      sb.AppendLine()
      For Each l In m_procLines
        sb.AppendLine("  " & l)
      Next
      sb.AppendLine("End Module")
      Dim text = sb.ToString()
      Return text.Replace("TODO_HEADER", If(text.Contains("TODO(QB2VB)"), "  Lines marked TODO(QB2VB) need attention.", ""))

    End Function

  End Class

End Namespace
