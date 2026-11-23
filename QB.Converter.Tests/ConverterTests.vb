Option Explicit On
Option Strict On
Option Infer On

Imports Xunit

Namespace QB2VB.Tests

  Public Class ConverterTests

    ' The converted statements of the program's module-level code, trimmed.
    Private Shared Function Main(source As String, Optional ByRef notes As List(Of String) = Nothing) As List(Of String)
      Dim c As New Converter(source)
      Dim vb = c.Convert("T")
      notes = c.Notes
      Dim lines = vb.Replace(vbCrLf, vbLf).Split(vbLf(0)).Select(Function(l) l.Trim()).ToList()
      Dim s = lines.IndexOf("Public Sub Main()")
      Dim e = lines.IndexOf("End Sub", s)
      Return lines.GetRange(s + 1, e - s - 1).Where(Function(l) l <> "" AndAlso Not l.StartsWith("QB.Video.WaitForKey", StringComparison.Ordinal) AndAlso Not l.StartsWith("Dim ", StringComparison.Ordinal)).ToList()
    End Function

    Private Shared Function Convert(source As String) As String
      Return New Converter(source).Convert(If(source.Contains("DECLARE SUB GetFileNames"), "REMLINE", "T"))
    End Function

    Private Shared Sub AssertMain(source As String, ParamArray expected() As String)
      Assert.Equal(expected.ToList(), Main(source))
    End Sub

    <Fact>
    Public Sub PrintListsKeepTheirMeaning()
      AssertMain("PRINT ""Hi""", "PRINT(""Hi"")")
      AssertMain("PRINT", "PRINT()")
      AssertMain("PRINT ""a""; ""b""", "PRINT(""a"" & ""b"")")
      AssertMain("PRINT ""a"";", "PRINT(""a"", True)")
      AssertMain("PRINT 5", "PRINT(Str$(5) & "" "")")
    End Sub

    <Fact>
    Public Sub FileNumbersAreKept()
      AssertMain("PRINT #2, ""x""", "PRINT(2, ""x"")")
      AssertMain("OPEN ""a.txt"" FOR INPUT AS #1", "OPEN(""a.txt"", OpenMode.Input, 1)")
      AssertMain("OPEN ""a.txt"" FOR OUTPUT AS 3", "OPEN(""a.txt"", OpenMode.Output, 3)")
      AssertMain("CLOSE #1, #2", "CLOSE(1, 2)")
      AssertMain("LINE INPUT #1, a$", "LINE_INPUT(1, a$)")
    End Sub

    <Fact>
    Public Sub InputForms()
      AssertMain("INPUT ""Name""; n$", "INPUT(""Name"", n$)")
      AssertMain("INPUT ""Name"", n$", "INPUT(""Name"", n$, False)")
      AssertMain("INPUT #1, a$", "INPUT(1, a$)")
    End Sub

    <Fact>
    Public Sub ControlFlowKeepsItsShape()
      AssertMain("FOR i = 1 TO 10 STEP 2" & vbLf & "NEXT i", "For i = 1 To 10 Step 2 : EMPTYLOOP()", "Next i")
      AssertMain("WHILE a$ <> """"" & vbLf & "WEND", "While a$ <> """"", "End While")
      AssertMain("DO UNTIL x = 3" & vbLf & "LOOP", "Do Until x = 3", "Loop")
      AssertMain("IF x = 1 THEN y = 2 ELSE y = 3", "If x = 1 Then y = 2 Else y = 3")
      AssertMain("SELECT CASE a$" & vbLf & "CASE ""x"", ""y""" & vbLf & "CASE ELSE" & vbLf & "END SELECT",
                 "Select Case a$", "Case ""x"", ""y""", "Case Else", "End Select")
    End Sub

    <Fact>
    Public Sub LabelsAndGotoStay()
      Dim m = Main("Top:" & vbLf & "GOTO Top")
      Assert.Equal({"Top:", "GoTo Top"}, m)
    End Sub

    <Fact>
    Public Sub ErrorHandling()
      AssertMain("ON ERROR GOTO Oops" & vbLf & "ON ERROR GOTO 0" & vbLf & "END" & vbLf & "Oops:" & vbLf & "RESUME NEXT",
                 "On Error GoTo Oops", "On Error GoTo 0", "End", "Oops:", "Resume Next")
    End Sub

    <Fact>
    Public Sub IntegerArithmeticIsNarrowed()
      Dim m = Main("DEFINT A-Z" & vbLf & "a = b + 1")
      Assert.Contains("a = CShort(b + 1)", m)
    End Sub

    <Fact>
    Public Sub LongToIntegerIsConverted()
      Dim m = Main("a% = 1" & vbLf & "b& = 2" & vbLf & "a% = b&")
      Assert.Contains("a = CShort(b%)", m)
    End Sub

    <Fact>
    Public Sub DoubleSuffixesAndSingleDefaultAreKept()
      Dim m = Main("x! = 1.5" & vbLf & "y# = 2")
      Assert.Contains("x! = 1.5!", m)
    End Sub

    <Fact>
    Public Sub IntegerIsShortAndLongUsesVbsPercent()
      Dim vb = Convert("a% = 1" & vbLf & "b& = 2")
      Assert.Contains("Dim a As Short", vb)
      Assert.Contains("Dim b%", vb)
      Assert.Contains("b% = 2", vb)
    End Sub

    <Fact>
    Public Sub TypeFieldsKeepTheirSize()
      Dim vb = Convert("TYPE P" & vbLf & "x AS INTEGER" & vbLf & "y AS LONG" & vbLf & "END TYPE" & vbLf & "DIM r AS P" & vbLf & "r.x = 1")
      Assert.Contains("Public x As Short", vb)
      Assert.Contains("Public y As Integer", vb)
    End Sub

    <Fact>
    Public Sub ReservedWordsAreRenamed()
      Dim vb = Convert("DIM Next AS INTEGER" & vbLf & "Next = 1")
      Assert.Contains("[Next]", vb)
    End Sub

    <Fact>
    Public Sub SharedAndConstGoToTheTopOfTheModule()
      Dim vb = Convert("CONST Max = 10" & vbLf & "DIM SHARED n AS INTEGER" & vbLf & "n = Max")
      Dim main = vb.IndexOf("Public Sub Main()", StringComparison.Ordinal)
      Assert.True(vb.IndexOf("Const Max As Short = 10", StringComparison.Ordinal) < main)
      Assert.True(vb.IndexOf("Dim n As Short", StringComparison.Ordinal) < main)
    End Sub

    <Fact>
    Public Sub DataGoesToTheStartWithItsLabel()
      Dim m = Main("PRINT 1" & vbLf & "Items:" & vbLf & "DATA 1, ""two"", three")
      Assert.Equal("DATA_LABEL(""Items"")", m(0))
      Assert.Equal("DATA(1, ""two"", ""three"")", m(1))
    End Sub

    <Fact>
    Public Sub SubWithStaticGetsStaticLocals()
      Dim vb = Convert("DECLARE SUB Foo ()" & vbLf & "Foo" & vbLf & "SUB Foo STATIC" & vbLf & "n = 1" & vbLf & "END SUB")
      Assert.Contains("Static n As Single", vb)
      Assert.Contains("Foo()", vb)
    End Sub

    <Fact>
    Public Sub FunctionNameAssignmentIsTheReturnValue()
      Dim vb = Convert("DECLARE FUNCTION Two% ()" & vbLf & "PRINT Two" & vbLf & "FUNCTION Two%" & vbLf & "Two = 2" & vbLf & "END FUNCTION")
      Assert.Contains("Function Two() As Short", vb)
      Assert.Contains("Two = 2", vb)
    End Sub

    <Fact>
    Public Sub ModuleHandlerIsCopiedIntoTheProcedureThatUsesIt()
      Dim src = "DECLARE SUB Foo ()" & vbLf & "Foo" & vbLf & "END" & vbLf &
                "Bad:" & vbLf & "RESUME NEXT" & vbLf &
                "SUB Foo" & vbLf & "ON ERROR GOTO Bad" & vbLf & "END SUB"
      Dim vb = Convert(src)
      Dim p = vb.IndexOf("Sub Foo()", StringComparison.Ordinal)
      Assert.True(vb.IndexOf("Bad:", p, StringComparison.Ordinal) > p)
    End Sub

    <Fact>
    Public Sub UnsupportedStatementsAreFlaggedNotDropped()
      Dim notes As New List(Of String)
      Dim vb As String
      Dim c As New Converter("DEF FNA(X) = X")
      vb = c.Convert("T")
      Assert.Contains("TODO(QB2VB)", vb)
      Assert.Contains("DEF FNA(X) = X", vb)
      Assert.NotEmpty(c.Notes)
    End Sub

    <Fact>
    Public Sub CommentsAreKept()
      Dim m = Main("' note" & vbLf & "PRINT ""x""  ' tail")
      Assert.Contains("' note", m)
      Assert.Contains("PRINT(""x"")  ' tail", m)
    End Sub

    ' Small programs whose conversion has to compile.
    Public Shared ReadOnly Property Programs As IEnumerable(Of Object())
      Get
        Return New List(Of Object()) From {
          New Object() {"print", "PRINT ""Hi"" : PRINT 5; ""x""; : PRINT"},
          New Object() {"loops", "FOR i = 1 TO 10 STEP 2" & vbLf & "PRINT i" & vbLf & "NEXT i" & vbLf & "n% = 3" & vbLf & "DO WHILE n% > 0" & vbLf & "n% = n% - 1" & vbLf & "LOOP"},
          New Object() {"select", "a$ = ""x""" & vbLf & "SELECT CASE a$" & vbLf & "CASE ""x"", ""y""" & vbLf & "PRINT 1" & vbLf & "CASE ELSE" & vbLf & "PRINT 2" & vbLf & "END SELECT"},
          New Object() {"types", "TYPE P" & vbLf & "x AS INTEGER" & vbLf & "n AS STRING * 8" & vbLf & "END TYPE" & vbLf & "DIM r AS P" & vbLf & "r.x = 4" & vbLf & "PRINT r.x"},
          New Object() {"procs", "DECLARE FUNCTION Twice% (n%)" & vbLf & "DECLARE SUB Show (s$)" & vbLf & "Show ""a""" & vbLf & "PRINT Twice%(4)" & vbLf & "END" & vbLf &
                                "FUNCTION Twice% (n%)" & vbLf & "Twice% = n% * 2" & vbLf & "END FUNCTION" & vbLf & "SUB Show (s$)" & vbLf & "PRINT s$" & vbLf & "END SUB"},
          New Object() {"data", "FOR i = 1 TO 2" & vbLf & "READ a$, b" & vbLf & "PRINT a$; b" & vbLf & "NEXT" & vbLf & "DATA x, 1, y, 2"},
          New Object() {"gosub", "GOSUB Show" & vbLf & "ON n GOSUB Show, Show" & vbLf & "END" & vbLf & "Show:" & vbLf & "PRINT ""x""" & vbLf & "IF n = 1 THEN RETURN" & vbLf & "PRINT ""y""" & vbLf & "RETURN"},
          New Object() {"arrays of types", "TYPE P" & vbLf & "x AS INTEGER" & vbLf & "n AS STRING * 4" & vbLf & "END TYPE" & vbLf & "DIM ps(1 TO 3) AS P" & vbLf & "ps(2).x = ps(1).x + 1" & vbLf & "DECLARE SUB Fill (a() AS P, g() AS INTEGER)" & vbLf & "DIM g(1 TO 2, 1 TO 2) AS INTEGER" & vbLf & "Fill ps(), g()" & vbLf &
                                          "SUB Fill (a() AS P, g() AS INTEGER)" & vbLf & "a(1).x = g(1, 2)" & vbLf & "END SUB"},
          New Object() {"memory and view", "DEF SEG = 0" & vbLf & "POKE 1047, 0" & vbLf & "DEF SEG" & vbLf & "VIEW PRINT 3 TO 10" & vbLf & "VIEW PRINT" & vbLf & "PLAY ""O2CDE"""},
          New Object() {"files", "OPEN ""t.txt"" FOR OUTPUT AS #1" & vbLf & "PRINT #1, ""hi""" & vbLf & "CLOSE #1" & vbLf & "OPEN ""t.txt"" FOR INPUT AS #1" & vbLf & "LINE INPUT #1, a$" & vbLf & "CLOSE" & vbLf & "PRINT a$"}
        }
      End Get
    End Property

    <Theory>
    <MemberData(NameOf(Programs))>
    Public Sub ConvertedProgramsCompile(name As String, source As String)
      Dim vb = Convert(source)
      Dim errors = VbCompiler.Compile(vb, "T")
      Assert.True(errors = "", name & ":" & vbLf & errors & vbLf & vb)
    End Sub

    <Fact>
    Public Sub NibblesConvertsWithoutNotesAndCompiles()
      Dim c As New Converter(IO.File.ReadAllText(FindSample("NIBBLES.BAS"), Text.Encoding.Latin1))
      Dim vb = c.Convert("NIBBLES")
      Assert.Empty(c.Notes)
      Dim errors = VbCompiler.Compile(vb, "NIBBLES")
      Assert.True(errors = "", errors)
    End Sub

    <Fact>
    Public Sub EmptyLoopsGetADelay()
      AssertMain("FOR i = 1 TO 1000: NEXT i", "For i = 1 To 1000 : EMPTYLOOP() : Next i")
      Dim m = Main("FOR i = 1 TO 5" & vbLf & "' wait" & vbLf & "NEXT")
      Assert.Contains("For i = 1 To 5 : EMPTYLOOP()", m)
      Assert.DoesNotContain("EMPTYLOOP()", String.Join(vbLf, Main("FOR i = 1 TO 5" & vbLf & "PRINT i" & vbLf & "NEXT")))
    End Sub

    <Fact>
    Public Sub GosubRoutineBecomesAProcedure()
      Dim vb = Convert("GOSUB Show" & vbLf & "END" & vbLf & "Show:" & vbLf & "PRINT ""x""" & vbLf & "RETURN")
      Assert.Contains("Show()", vb)
      Assert.Contains("Sub Show()", vb)
      Assert.DoesNotContain("TODO(QB2VB):", vb)
    End Sub

    <Fact>
    Public Sub ArrayParametersGetTheirRankFromTheirUse()
      Dim vb = Convert("DECLARE SUB F (g() AS INTEGER)" & vbLf & "DIM g(1 TO 2, 1 TO 2)" & vbLf & "F g()" & vbLf & "SUB F (g() AS INTEGER)" & vbLf & "g(1, 1) = 3" & vbLf & "END SUB")
      Assert.Contains("ByRef g(,) As Short", vb)
    End Sub

    <Fact>
    Public Sub ControlZEndsTheSource()
      Dim m = Main("PRINT 1" & vbLf & ChrW(26) & "garbage ((")
      Assert.Equal({"PRINT(Str$(1) & "" "")"}, m)
    End Sub

    <Fact>
    Public Sub RemlineCompiles()
      Dim vb = Convert(IO.File.ReadAllText(FindSample("REMLINE.BAS"), Text.Encoding.Latin1))
      Dim errors = VbCompiler.Compile(vb.Replace("Module T", "Module T"), "REMLINE")
      Assert.True(errors = "", errors)
    End Sub

    Private Shared Function FindSample(name As String) As String
      Dim dir = AppContext.BaseDirectory
      Do While dir IsNot Nothing
        Dim candidate = IO.Path.Combine(dir, "Samples", name)
        If IO.File.Exists(candidate) Then Return candidate
        dir = IO.Path.GetDirectoryName(dir)
      Loop
      Throw New IO.FileNotFoundException(name)
    End Function

    <Fact>
    Public Sub RemlineConvertsWithoutNotes()
      Dim dir = AppContext.BaseDirectory
      Dim path As String = Nothing
      Do While dir IsNot Nothing
        Dim candidate = IO.Path.Combine(dir, "Samples", "REMLINE.BAS")
        If IO.File.Exists(candidate) Then path = candidate : Exit Do
        dir = IO.Path.GetDirectoryName(dir)
      Loop
      Assert.NotNull(path)
      Text.Encoding.RegisterProvider(Text.CodePagesEncodingProvider.Instance)
      Dim c As New Converter(IO.File.ReadAllText(path, Text.Encoding.GetEncoding(437)))
      Dim vb = c.Convert("REMLINE")
      Assert.Empty(c.Notes)
      Assert.Contains("Function StrSpn(ByRef InString$, ByRef Separator$) As Short", vb)
      Assert.Contains("DATA_LABEL(""KeyData"")", vb)
    End Sub

  End Class

End Namespace
