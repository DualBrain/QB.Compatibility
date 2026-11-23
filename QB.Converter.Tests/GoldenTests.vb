Option Explicit On
Option Strict On
Option Infer On

Imports Xunit

Namespace QB2VB.Tests

  ''' <summary>
  ''' Every sample (the programs in Samples and the manual examples in Samples/Manual) is converted and the result is compared with
  ''' the reviewed output kept in the Golden folder.  When the converter changes on purpose, run the tests with the environment
  ''' variable UPDATE_GOLDEN=1 to rewrite the files, and look at the difference before committing it.
  ''' </summary>
  Public Class GoldenTests

    Private Shared Function Root() As String
      Dim dir = AppContext.BaseDirectory
      Do While dir IsNot Nothing
        If IO.Directory.Exists(IO.Path.Combine(dir, "Samples")) AndAlso IO.Directory.Exists(IO.Path.Combine(dir, "QB.Converter.Tests")) Then Return dir
        dir = IO.Path.GetDirectoryName(dir)
      Loop
      Throw New IO.DirectoryNotFoundException("The repository folder (with Samples) wasn't found.")
    End Function

    Public Shared ReadOnly Property Cases As IEnumerable(Of Object())
      Get
        Dim r = Root()
        Dim files = IO.Directory.GetFiles(IO.Path.Combine(r, "Samples"), "*.BAS").Concat(IO.Directory.GetFiles(IO.Path.Combine(r, "Samples", "Manual"), "*.BAS"))
        Return files.OrderBy(Function(f) f, StringComparer.Ordinal).Select(Function(f) New Object() {IO.Path.GetRelativePath(r, f).Replace("\"c, "/"c)}).ToList()
      End Get
    End Property

    Private Shared Function Converted(relative As String, ByRef notes As List(Of String)) As String
      Text.Encoding.RegisterProvider(Text.CodePagesEncodingProvider.Instance)
      Dim path = IO.Path.Combine(Root(), relative)
      Dim c As New Converter(IO.File.ReadAllText(path, Text.Encoding.GetEncoding(437)))
      Dim vb = c.Convert(Program.ModuleNameFor(path)).Replace(vbCrLf, vbLf)
      notes = c.Notes
      Return vb
    End Function

    Private Shared Function GoldenPath(relative As String) As String
      Return IO.Path.Combine(Root(), "QB.Converter.Tests", "Golden", IO.Path.GetFileNameWithoutExtension(relative) & ".vb")
    End Function

    <Theory>
    <MemberData(NameOf(Cases))>
    Public Sub ConvertsToTheReviewedOutput(relative As String)
      Dim notes As List(Of String) = Nothing
      Dim vb = Converted(relative, notes)
      Dim golden = GoldenPath(relative)
      If Environment.GetEnvironmentVariable("UPDATE_GOLDEN") = "1" Then
        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(golden))
        IO.File.WriteAllText(golden, vb, New Text.UTF8Encoding(False))
        Return
      End If
      Assert.True(IO.File.Exists(golden), "No reviewed output for " & relative & ": run the tests with UPDATE_GOLDEN=1.")
      Dim expected = IO.File.ReadAllText(golden).Replace(vbCrLf, vbLf)
      If expected <> vb Then
        Dim a = expected.Split(vbLf(0)), b = vb.Split(vbLf(0))
        Dim line = 0
        Do While line < a.Length AndAlso line < b.Length AndAlso a(line) = b(line)
          line += 1
        Loop
        Assert.Fail($"{relative} converts differently from {IO.Path.GetFileName(golden)} at line {line + 1}:" & vbLf &
                    "  expected: " & If(line < a.Length, a(line), "(end)") & vbLf & "  actual:   " & If(line < b.Length, b(line), "(end)") & vbLf &
                    "Run the tests with UPDATE_GOLDEN=1 if the change is intended.")
      End If
    End Sub

    <Fact>
    Public Sub ManualExamplesConvertWithoutNotes()
      Dim problems As New List(Of String)
      For Each c In Cases.Where(Function(x) CStr(x(0)).Contains("/Manual/"))
        Dim notes As List(Of String) = Nothing
        Converted(CStr(c(0)), notes)
        problems.AddRange(notes.Select(Function(n) CStr(c(0)) & ": " & n))
      Next
      Assert.Empty(problems)
    End Sub

    <Fact>
    Public Sub TheReviewedOutputCompilesTogether()
      Dim sources = IO.Directory.GetFiles(IO.Path.Combine(Root(), "QB.Converter.Tests", "Golden"), "*.vb").OrderBy(Function(f) f, StringComparer.Ordinal).Select(Function(f) IO.File.ReadAllText(f)).ToList()
      Assert.True(sources.Count > 50)
      Dim errors = VbCompiler.Compile(sources, "Beep_Program")
      Assert.True(errors = "", errors)
    End Sub

  End Class

End Namespace
