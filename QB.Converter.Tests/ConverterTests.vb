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
      Dim c As New Converter("CHAIN ""x""")
      vb = c.Convert("T")
      Assert.Contains("TODO(QB2VB)", vb)
      Assert.Contains("CHAIN ""x""", vb)
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
          New Object() {"graphics", "SCREEN 9" & vbLf & "DIM a%(200)" & vbLf & "PSET (10, 10), 4" & vbLf & "PSET STEP(1, 1)" & vbLf & "LINE (1, 1)-(50, 50), 3, BF" & vbLf & "LINE -(60, 60)" & vbLf & "LINE (1, 1)-STEP(5, 5), 2, B" & vbLf &
                                  "CIRCLE (100, 100), 20, 5, , , .5" & vbLf & "CIRCLE (100, 100), 20, 5, 0, 3.14" & vbLf & "PAINT (100, 100), 5, 3" & vbLf & "GET (1, 1)-(30, 30), a%" & vbLf & "PUT (40, 40), a%, PSET" & vbLf & "PUT (40, 40), a%(5), XOR" & vbLf & "VIEW (1, 1)-(100, 100), 1, 2" & vbLf & "WINDOW (0, 0)-(10, 10)"},
          New Object() {"def fn", "DEF FnTwice (x) = x * 2" & vbLf & "DEF FnHalf! (x!) = x! / 2" & vbLf & "PRINT FnTwice(4); FnHalf!(3)"},
          New Object() {"error handler in a gosub routine", "GOSUB Init" & vbLf & "END" & vbLf & "Init:" & vbLf & "ON ERROR GOTO Bad" & vbLf & "SCREEN 9" & vbLf & "ON ERROR GOTO 0" & vbLf & "RETURN" & vbLf & "Bad:" & vbLf & "RESUME NEXT"},
          New Object() {"gosub in a sub", "DECLARE SUB Run (n%)" & vbLf & "Run 3" & vbLf & "SUB Run (n%)" & vbLf & "DIM a$(5)" & vbLf & "GOSUB Show" & vbLf & "n% = n% + 1" & vbLf & "GOSUB Show" & vbLf & "EXIT SUB" & vbLf & "Show:" & vbLf & "a$(1) = STR$(n%)" & vbLf & "PRINT a$(1)" & vbLf & "RETURN" & vbLf & "END SUB"},
          New Object() {"gosub with on error in a sub", "DECLARE SUB Run ()" & vbLf & "Run" & vbLf & "END" & vbLf & "Trap:" & vbLf & "RESUME NEXT" & vbLf & "SUB Run" & vbLf & "GOSUB P" & vbLf & "GOSUB P" & vbLf & "EXIT SUB" & vbLf & "P:" & vbLf & "ON ERROR GOTO Trap" & vbLf & "LPRINT" & vbLf & "ON ERROR GOTO 0" & vbLf & "RETURN" & vbLf & "END SUB"},
          New Object() {"field", "OPEN ""f.dat"" FOR RANDOM AS #1 LEN = 20" & vbLf & "FIELD #1, 8 AS d$, 12 AS e$" & vbLf & "LSET d$ = ""x""" & vbLf & "RSET e$ = MKD$(1)" & vbLf & "PUT #1, 1" & vbLf & "GET #1, 1" & vbLf & "PRINT d$" & vbLf & "CLOSE" & vbLf & "end$ = ""e""" & vbLf & "PRINT end$"},
          New Object() {"lprint", "u$ = ""\ \""" & vbLf & "LPRINT USING u$; ""ab""; 1" & vbLf & "LPRINT TAB(5); ""x"";" & vbLf & "LPRINT"},
          New Object() {"data in hex", "READ a, b" & vbLf & "DATA &HFF, &H10"},
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
    Public Sub GraphicsStatementsKeepTheirShape()
      AssertMain("LINE (1, 2)-(3, 4), 5, BF", "LINE(1, 2, 3, 4, 5, LineOption.BF)")
      AssertMain("LINE (1, 2)-STEP(3, 4)", "LINE(False, 1, 2, [STEP], 3, 4)")
      AssertMain("PSET (1, 2), 3", "PSET(1, 2, 3)")
      AssertMain("CIRCLE (5, 6), 7, 1, , , .5", "CIRCLE(5, 6, 7, 1, Nothing, Nothing, .5!)")
      AssertMain("DIM g&(9)" & vbLf & "GET (1, 1)-(2, 2), g&" & vbLf & "PUT (3, 3), g&, PSET", "[GET](1, 1, 2, 2, g%)", "PUT(3, 3, g%, PutOption.PUT_PSET)")
    End Sub

    <Fact>
    Public Sub RedimOfASharedArrayResizesIt()
      Dim vb = Convert("DIM SHARED a%(3)" & vbLf & "SUB Grow" & vbLf & "REDIM a%(9)" & vbLf & "END SUB")
      Assert.Contains("ReDim a(9)", vb)
      Assert.DoesNotContain("Dim a(9) As", vb)
    End Sub

    <Fact>
    Public Sub ModRoundsItsOperandsLikeQBasic()
      Dim m = Main("DEFINT A-Z" & vbLf & "t# = 1.5" & vbLf & "r = (t# * 10) MOD 4")
      Assert.Contains("r = CShort(CInt((t# * 10)) Mod 4)", m)
    End Sub

    <Fact>
    Public Sub LoopsThatPollTimerGetADelay()
      Dim m = Main("s! = TIMER" & vbLf & "DO" & vbLf & "i! = i! + 1" & vbLf & "LOOP UNTIL TIMER - s! >= .5" & vbLf & "WHILE TIMER < 5" & vbLf & "WEND")
      Assert.Contains("EMPTYLOOP() : Loop Until (Timer - s!) >= .5!", m)
      Assert.Contains("While Timer < 5 : EMPTYLOOP()", m)
    End Sub

    <Fact>
    Public Sub EmptyLoopTimeCanBeSet()
      Dim c As New Converter("PRINT 1") With {.EmptyLoopMicroseconds = 400}
      Assert.Contains("EmptyLoopMicroseconds = 400", c.Convert("T"))
    End Sub

    <Fact>
    Public Sub FieldVariablesAreFieldsOfARecord()
      Dim vb = Convert("OPEN ""f"" FOR RANDOM AS #1 LEN = 10" & vbLf & "FIELD #1, 4 AS a$, 6 AS b$" & vbLf & "LSET b$ = ""x""" & vbLf & "GET #1, 2" & vbLf & "PRINT a$")
      Assert.Contains("Fields1 = FIELD(1, 4, 6)", vb)
      Assert.Contains("Fields1.LSET(1, ""x"")", vb)
      Assert.Contains("GET_FIELD(1, 2)", vb)
      Assert.Contains("PRINT(Fields1(0))", vb)
    End Sub

    <Fact>
    Public Sub NamesWithASuffixAreNeverStatements()
      Dim m = Main("end$ = ""a""" & vbLf & "PRINT end$")
      Assert.Equal({"end_$ = ""a""", "PRINT(end_$)"}, m.Where(Function(l) l.Contains("end")).ToList())
    End Sub

    <Fact>
    Public Sub CloseWithoutNumbersClosesAll()
      AssertMain("CLOSE", "CLOSE()")
    End Sub

    <Fact>
    Public Sub MoneyConvertsAndCompiles()
      Dim c As New Converter(IO.File.ReadAllText(FindSample("MONEY.BAS"), Text.Encoding.Latin1))
      Dim vb = c.Convert("MONEY")
      ' Only the machine code (CALL ABSOLUTE and the addresses it needs) is left.
      Assert.NotEmpty(c.Notes)
      Assert.All(c.Notes, Sub(n) Assert.True(n.Contains("Absolute") OrElse n.Contains("VARPTR") OrElse n.Contains("VARSEG"), n))
      Dim errors = VbCompiler.Compile(vb, "MONEY")
      Assert.True(errors = "", errors)
    End Sub

    <Theory>
    <InlineData("PRINT ""hi""" & vbLf & "SCREEN 0" & vbLf & "LOCATE 2, 2", False, False)>
    <InlineData("BEEP", False, False)>
    <InlineData("SCREEN 9", True, False)>
    <InlineData("m% = 9" & vbLf & "SCREEN m%", True, False)>
    <InlineData("PSET (1, 1)", True, False)>
    <InlineData("x = POINT(1, 1)", True, False)>
    <InlineData("a% = STICK(0)", True, False)>
    <InlineData("PLAY ""C""", False, True)>
    <InlineData("SOUND 440, 1", False, True)>
    <InlineData("SCREEN 12" & vbLf & "PLAY ""C""", True, True)>
    Public Sub TheImportsFollowWhatTheProgramUses(source As String, graphics As Boolean, sound As Boolean)
      Dim c As New Converter(source)
      Dim vb = c.Convert("T")
      Assert.Equal(graphics, c.UsesGraphics)
      Assert.Equal(sound, c.UsesSound)
      Assert.Contains(If(graphics, "Imports QB.Graphics", "Imports QB.Video"), vb)
      Assert.Contains(If(sound, "Imports QB.Speaker", "Imports QB.Multimedia"), vb)
      Assert.DoesNotContain(If(graphics, "Imports QB.Video", "Imports QB.Graphics"), vb)
    End Sub

    <Theory>
    <InlineData("OPEN ""COM1:9600,N,8,1"" FOR RANDOM AS #1", True)>
    <InlineData("OPEN ""com2:"" FOR INPUT AS #1", True)>
    <InlineData("OPEN ""/dev/ttyUSB0:9600"" FOR RANDOM AS #1", True)>
    <InlineData("COM(1) ON", True)>
    <InlineData("ON COM(1) GOSUB H" & vbLf & "END" & vbLf & "H:" & vbLf & "RETURN", True)>
    <InlineData("OPEN ""data.txt"" FOR OUTPUT AS #1", False)>
    <InlineData("OPEN ""LPT1:"" FOR OUTPUT AS #1", False)>
    Public Sub SerialPortsNeedTheSerialImport(source As String, serial As Boolean)
      Dim c As New Converter(source)
      Dim vb = c.Convert("T")
      Assert.Equal(serial, c.UsesSerial)
      Assert.Contains(If(serial, "Imports QB.Serial", "Imports QB.File"), vb)
      Assert.DoesNotContain(If(serial, "Imports QB.File", "Imports QB.Serial"), vb)
    End Sub

    <Fact>
    Public Sub TheProjectReferencesTheSdlAssemblyOnlyWhenNeeded()
      Dim text = Program.ProjectText("T", "T.vb", "/work/T", "/lib", False, False, False)
      Assert.Contains("QB.Compatibility.vbproj", text)
      Assert.DoesNotContain("QB.Compatibility.Sdl", text)
      Assert.DoesNotContain("PublishAot", text)
      Dim withSdl = Program.ProjectText("T", "T.vb", "/work/T", "/lib", True, True, True)
      Assert.Contains("QB.Compatibility.Sdl.vbproj", withSdl)
      Assert.Contains("<PublishAot>true</PublishAot>", withSdl)
      Assert.Contains("QB.Compatibility.Ports.vbproj", withSdl)
      Assert.DoesNotContain("QB.Compatibility.Ports", text)
    End Sub

    <Fact>
    Public Sub DefFnBecomesAFunction()
      Dim vb = Convert("DEFINT A-Z" & vbLf & "DEF FnTwice (x) = x * 2" & vbLf & "PRINT FnTwice(4)")
      Assert.Contains("Function FnTwice(ByVal x As Short) As Short", vb)
      Assert.Contains("FnTwice(4)", vb)
    End Sub

    <Fact>
    Public Sub GorillaConvertsWithoutNotesAndCompiles()
      Dim c As New Converter(IO.File.ReadAllText(FindSample("GORILLA.BAS"), Text.Encoding.Latin1))
      Dim vb = c.Convert("GORILLA")
      Assert.Empty(c.Notes)
      Dim errors = VbCompiler.Compile(vb, "GORILLA")
      Assert.True(errors = "", errors)
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
