Option Explicit On
Option Strict On
Option Infer On

Imports System.IO
Imports System.Text

Namespace QB2VB

  Friend Module Program

    ''' <summary>The version of the QB.Compatibility packages that go with this tool (they are released together).</summary>
    Friend Function PackageVersion() As String
      Dim v = Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(GetType(Reflection.AssemblyInformationalVersionAttribute), False).OfType(Of Reflection.AssemblyInformationalVersionAttribute)().FirstOrDefault()?.InformationalVersion
      If String.IsNullOrEmpty(v) Then Return "*"
      Dim plus = v.IndexOf("+"c)
      Return If(plus >= 0, v.Substring(0, plus), v)
    End Function

    ''' <summary>
    ''' A project file for a converted program: text programs reference only QB.Compatibility; the SDL assembly is added for graphics and sound.
    ''' With a libDir (the repository folder) the references are projects; without one they are the NuGet packages, at packageVersion.
    ''' </summary>
    Friend Function ProjectText(moduleName As String, sourceFile As String, projectDir As String, libDir As String, needsSdl As Boolean, needsPorts As Boolean, aot As Boolean, Optional namespaces As IEnumerable(Of String) = Nothing, Optional packageVersion As String = "*") As String
      Dim usePackages = libDir = ""
      Dim core = "", sdl = "", ports = ""
      If Not usePackages Then
        core = Path.GetRelativePath(projectDir, Path.Combine(Path.GetFullPath(libDir), "QB.Compatibility", "QB.Compatibility.vbproj"))
        sdl = Path.GetRelativePath(projectDir, Path.Combine(Path.GetFullPath(libDir), "QB.Compatibility.Sdl", "QB.Compatibility.Sdl.vbproj"))
        ports = Path.GetRelativePath(projectDir, Path.Combine(Path.GetFullPath(libDir), "QB.Compatibility.Ports", "QB.Compatibility.Ports.vbproj"))
      End If
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
      If namespaces IsNot Nothing Then
        sb.AppendLine("  <ItemGroup>")
        For Each name In namespaces
          sb.AppendLine("    <Import Include=" & q & name & q & " />")
        Next
        sb.AppendLine("  </ItemGroup>")
      End If
      sb.AppendLine("  <ItemGroup>")
      sb.AppendLine("    <Compile Include=" & q & sourceFile & q & " />")
      Dim winExe = " Condition=" & q & "'$(OutputType)' == 'WinExe'" & q
      If usePackages Then
        Dim version = " Version=" & q & packageVersion & q
        sb.AppendLine("    <PackageReference Include=" & q & "QB.Compatibility" & q & version & " />")
        sb.AppendLine("    <PackageReference Include=" & q & "QB.Compatibility.Sdl" & q & version & If(needsSdl, "", winExe) & " />")
        If needsPorts Then sb.AppendLine("    <PackageReference Include=" & q & "QB.Compatibility.Ports" & q & version & " />")
      Else
        sb.AppendLine("    <ProjectReference Include=" & q & core & q & " />")
        sb.AppendLine("    <ProjectReference Include=" & q & sdl & q & If(needsSdl, "", winExe) & " />")
        If needsPorts Then sb.AppendLine("    <ProjectReference Include=" & q & ports & q & " />")
      End If
      sb.AppendLine("  </ItemGroup>")
      sb.AppendLine("</Project>")
      Return sb.ToString
    End Function

    ''' <summary>The VB module name of every converted program.</summary>
    Friend Function ModuleNameFor(path As String) As String
      Return "Program"
    End Function

    Public Function Main(args As String()) As Integer

      Dim input = ""
      Dim output = ""
      Dim emptyLoop As Double? = Nothing
      Dim libDir = ""
      Dim project = False
      Dim aot = False
      Dim optionsDone = False
      Dim i = 0
      Do While i < args.Length
        Dim a = args(i)
        i += 1
        If optionsDone OrElse a.Length < 2 OrElse a(0) <> "-"c Then
          input = a
          Continue Do
        End If
        If a = "--" Then ' Everything after this is a file name, even if it starts with a dash.
          optionsDone = True
          Continue Do
        End If
        ' --name=value is the same as --name value.
        Dim name = a
        Dim value As String = Nothing
        If a.StartsWith("--", StringComparison.Ordinal) Then
          Dim eq = a.IndexOf("="c)
          If eq > 0 Then
            name = a.Substring(0, eq)
            value = a.Substring(eq + 1)
          End If
        End If
        Select Case name
          Case "-o", "--output", "--repo", "--empty-loop-us"
            If value Is Nothing Then
              If i >= args.Length Then
                Console.Error.WriteLine($"qb2vb: option '{name}' needs a value")
                Return 2
              End If
              value = args(i)
              i += 1
            End If
            If name = "-o" OrElse name = "--output" Then
              output = value
            ElseIf name = "--repo" Then
              project = True : libDir = value
            Else
              Dim us As Double
              If Not Double.TryParse(value, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, us) Then
                Console.Error.WriteLine($"qb2vb: '{value}' is not a number for '{name}'")
                Return 2
              End If
              emptyLoop = us
            End If
          Case "--project"
            project = True
          Case "--aot"
            aot = True
          Case "-V", "--version"
            Dim v = PackageVersion()
            Console.WriteLine("qb2vb " & If(v = "*", "(unknown version)", v))
            Return 0
          Case "-h", "--help"
            Console.WriteLine("usage: qb2vb [options] input.bas" & vbLf & "converts a QBasic program to VB that uses QB.Compatibility" & vbLf & vbLf & "options:" & vbLf & "  -o, --output FILE   the VB file to write (default: the input name with .vb)" & vbLf & "      --project       also write a .vbproj that references the QB.Compatibility NuGet packages (QB.Compatibility.Sdl only if the program draws or reads the joystick; QB.Compatibility.Ports only if it opens a serial port)" & vbLf & "      --repo DIR      like --project, but references the projects of a QB.Compatibility checkout in DIR instead of the packages" & vbLf & "      --aot           with --project or --repo: publish as a Native AOT program" & vbLf & "      --empty-loop-us N   start the program with QB.Core.EmptyLoopMicroseconds = N (the time of one pass of a delay loop; 400 by default)" & vbLf & "  -V, --version       show the version" & vbLf & "  -h, --help          show this help" & vbLf & "  --                  end of options (the next argument is the input file)" & vbLf & vbLf & "Options that take a value accept --name value or --name=value.")
            Return 0
          Case Else
            Console.Error.WriteLine($"qb2vb: unknown option '{name}' (try --help)")
            Return 2
        End Select
      Loop
      If input = "" Then
        Console.Error.WriteLine("usage: qb2vb [options] input.bas (try --help)")
        Return 2
      End If

      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
      Dim source = File.ReadAllText(input, Encoding.GetEncoding(437))
      Dim moduleName = ModuleNameFor(input)

      Dim conv As New Converter(source) With {.EmptyLoopMicroseconds = emptyLoop, .OmitImports = project}
      Dim vb = conv.Convert(moduleName)
      If output = "" Then output = Path.ChangeExtension(input, ".vb")
      File.WriteAllText(output, vb, New UTF8Encoding(False))

      Console.WriteLine($"{input} -> {output}")
      If project Then
        Dim projectPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(output)), Path.GetFileNameWithoutExtension(input) & ".vbproj")
        Dim needsSdl = conv.UsesGraphics
        File.WriteAllText(projectPath, ProjectText(moduleName, Path.GetFileName(output), Path.GetDirectoryName(projectPath), libDir, needsSdl, conv.UsesSerial, aot, conv.Imports, PackageVersion()), New UTF8Encoding(False))
        Console.WriteLine($"{projectPath}  (references QB.Compatibility{If(needsSdl, " and QB.Compatibility.Sdl", "")}{If(conv.UsesSerial, " and QB.Compatibility.Ports", "")})")
      End If
      For Each n In conv.Notes
        Console.WriteLine("  " & n)
      Next
      Return If(conv.Notes.Count > 0, 1, 0)

    End Function

  End Module

End Namespace
