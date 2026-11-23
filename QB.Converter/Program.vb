Option Explicit On
Option Strict On
Option Infer On

Imports System.IO
Imports System.Text

Namespace QB2VB

  Friend Module Program

    ''' <summary>A project file for a converted program: text programs reference only QB.Compatibility; the SDL assembly is added for graphics and sound.</summary>
    Friend Function ProjectText(moduleName As String, sourceFile As String, projectDir As String, libDir As String, needsSdl As Boolean, needsPorts As Boolean, aot As Boolean) As String
      Dim core = Path.GetRelativePath(projectDir, Path.Combine(Path.GetFullPath(libDir), "QB.Compatibility", "QB.Compatibility.vbproj"))
      Dim sdl = Path.GetRelativePath(projectDir, Path.Combine(Path.GetFullPath(libDir), "QB.Compatibility.Sdl", "QB.Compatibility.Sdl.vbproj"))
      Dim ports = Path.GetRelativePath(projectDir, Path.Combine(Path.GetFullPath(libDir), "QB.Compatibility.Ports", "QB.Compatibility.Ports.vbproj"))
      Dim q = ChrW(34)
      Dim sb As New StringBuilder
      sb.AppendLine("<Project Sdk=" & q & "Microsoft.NET.Sdk" & q & ">")
      sb.AppendLine("  <PropertyGroup>")
      sb.AppendLine("    <OutputType>Exe</OutputType>")
      sb.AppendLine("    <TargetFramework>net10.0</TargetFramework>")
      sb.AppendLine("    <RootNamespace></RootNamespace>")
      sb.AppendLine("    <StartupObject>" & moduleName & "</StartupObject>")
      sb.AppendLine("    <OptionExplicit>On</OptionExplicit>")
      sb.AppendLine("    <OptionInfer>On</OptionInfer>")
      sb.AppendLine("    <OptionStrict>On</OptionStrict>")
      sb.AppendLine("    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>")
      If aot Then
        sb.AppendLine("    <PublishAot>true</PublishAot>")
        sb.AppendLine("    <InvariantGlobalization>true</InvariantGlobalization>")
      End If
      sb.AppendLine("  </PropertyGroup>")
      ' Changing OutputType to WinExe (no console) is all it takes to show a text program's screen in a window.
      sb.AppendLine("  <PropertyGroup Condition=" & q & "'$(OutputType)' == 'WinExe'" & q & ">")
      sb.AppendLine("    <DefineConstants>QB_WINDOW</DefineConstants>")
      sb.AppendLine("  </PropertyGroup>")
      sb.AppendLine("  <ItemGroup>")
      sb.AppendLine("    <Compile Include=" & q & sourceFile & q & " />")
      sb.AppendLine("    <ProjectReference Include=" & q & core & q & " />")
      If needsSdl Then
        sb.AppendLine("    <ProjectReference Include=" & q & sdl & q & " />")
      Else
        sb.AppendLine("    <ProjectReference Include=" & q & sdl & q & " Condition=" & q & "'$(OutputType)' == 'WinExe'" & q & " />")
      End If
      If needsPorts Then sb.AppendLine("    <ProjectReference Include=" & q & ports & q & " />")
      sb.AppendLine("  </ItemGroup>")
      sb.AppendLine("</Project>")
      Return sb.ToString
    End Function

    ''' <summary>The VB module name for a source file: its name, made into an identifier that doesn't clash with a statement or function.</summary>
    Friend Function ModuleNameFor(path As String) As String
      Dim moduleName = IO.Path.GetFileNameWithoutExtension(path)
      Dim sb As New StringBuilder
      For Each c In moduleName
        sb.Append(If(Char.IsLetterOrDigit(c), c, "_"c))
      Next
      moduleName = sb.ToString
      If moduleName = "" OrElse Char.IsDigit(moduleName(0)) Then moduleName = "Program_" & moduleName
      ' (A module can't share its name with a statement or function it calls.)
      If Keywords.IsStatementWord(moduleName) OrElse Keywords.IsVbReserved(moduleName) OrElse Builtins.Table.ContainsKey(moduleName) OrElse Builtins.Table.ContainsKey(moduleName & "$") OrElse Builtins.LibraryNames.Contains(moduleName) Then moduleName &= "_Program"
      Return moduleName
    End Function

    Public Function Main(args As String()) As Integer

      Dim input = ""
      Dim output = ""
      Dim emptyLoop As Double? = Nothing
      Dim libDir = ""
      Dim aot = False
      Dim i = 0
      Do While i < args.Length
        Dim a = args(i)
        If a = "-o" AndAlso i + 1 < args.Length Then
          output = args(i + 1) : i += 2
        ElseIf a = "--empty-loop-us" AndAlso i + 1 < args.Length Then
          emptyLoop = Double.Parse(args(i + 1), Globalization.CultureInfo.InvariantCulture) : i += 2
        ElseIf a = "--project" AndAlso i + 1 < args.Length Then
          libDir = args(i + 1) : i += 2
        ElseIf a = "--aot" Then
          aot = True : i += 1
        ElseIf a = "-h" OrElse a = "--help" Then
          Console.WriteLine("qb2vb input.bas [-o output.vb]    converts a QBasic program to VB that uses QB.Compatibility" & vbLf & "  --project DIR       also write a .vbproj that references the QB.Compatibility projects under DIR (QB.Compatibility.Sdl only if the program draws, plays sound or reads the joystick; QB.Compatibility.Ports only if it opens a serial port)" & vbLf & "  --aot               with --project: publish as a Native AOT program" & vbLf & "  --empty-loop-us N   start the program with QB.Core.EmptyLoopMicroseconds = N (the time of one pass of a delay loop; 400 by default)")
          Return 0
        Else
          input = a : i += 1
        End If
      Loop
      If input = "" Then
        Console.Error.WriteLine("usage: qb2vb input.bas [-o output.vb]")
        Return 2
      End If

      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
      Dim source = File.ReadAllText(input, Encoding.GetEncoding(437))
      Dim moduleName = ModuleNameFor(input)

      Dim conv As New Converter(source) With {.EmptyLoopMicroseconds = emptyLoop}
      Dim vb = conv.Convert(moduleName)
      If output = "" Then output = Path.ChangeExtension(input, ".vb")
      File.WriteAllText(output, vb, New UTF8Encoding(False))

      Console.WriteLine($"{input} -> {output}")
      If libDir <> "" Then
        Dim projectPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(output)), moduleName & ".vbproj")
        Dim needsSdl = conv.UsesGraphics
        File.WriteAllText(projectPath, ProjectText(moduleName, Path.GetFileName(output), Path.GetDirectoryName(projectPath), libDir, needsSdl, conv.UsesSerial, aot), New UTF8Encoding(False))
        Console.WriteLine($"{projectPath}  (references QB.Compatibility{If(needsSdl, " and QB.Compatibility.Sdl", "")}{If(conv.UsesSerial, " and QB.Compatibility.Ports", "")})")
      End If
      For Each n In conv.Notes
        Console.WriteLine("  " & n)
      Next
      Return If(conv.Notes.Count > 0, 1, 0)

    End Function

  End Module

End Namespace
