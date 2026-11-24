' Converted from QBasic by qb2vb.

Module Program

  Structure TitleType
    Implements IFileRecord
    Private m_MainTitle As String
    Public Property MainTitle As String
      Get
        Return If(m_MainTitle, FixedString.Blank(40))
      End Get
      Set(value As String)
        m_MainTitle = FixedString.Fit(value, 40)
      End Set
    End Property
    Private m_XTitle As String
    Public Property XTitle As String
      Get
        Return If(m_XTitle, FixedString.Blank(40))
      End Get
      Set(value As String)
        m_XTitle = FixedString.Fit(value, 40)
      End Set
    End Property
    Private m_YTitle As String
    Public Property YTitle As String
      Get
        Return If(m_YTitle, FixedString.Blank(18))
      End Get
      Set(value As String)
        m_YTitle = FixedString.Fit(value, 18)
      End Set
    End Property
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(MainTitle, 40)
      writer.WriteString(XTitle, 40)
      writer.WriteString(YTitle, 18)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      MainTitle = reader.ReadString(40)
      XTitle = reader.ReadString(40)
      YTitle = reader.ReadString(18)
    End Sub
  End Structure
  
  Const [FALSE] As Short = 0
  Const [TRUE] As Short = Not [FALSE]
  Const GRAPHTOP As Short = 24
  Const GRAPHBOTTOM As Short = 171
  Const GRAPHLEFT As Short = 48
  Const GRAPHRIGHT As Short = 624
  Const YLENGTH As Short = GRAPHBOTTOM - GRAPHTOP

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim N As Short
    Dim NewGraph$
    ' Define type for the titles:
    ' TYPE TitleType (see the Structure at the top)
    
    ' DECLARE SUB InputTitles (T AS TitleType)
    ' DECLARE FUNCTION DrawGraph$ (T AS TitleType, Label$(), Value!(), N%)
    ' DECLARE FUNCTION InputData% (Label$(), Value!())
    
    ' Variable declarations for titles and bar data:
    Dim Titles As TitleType : Dim Label$(5) : Dim Value(5) As Single
    
    ' CONST FALSE = 0, TRUE = NOT FALSE
    
    Do
       InputTitles(Titles)
       N = InputData(Label$, Value)
       If N <> [FALSE] Then
          NewGraph$ = DrawGraph$(Titles, Label$, Value, N)
       End If
    Loop While NewGraph$ = "Y"
    
    End
  End Sub

  ' $STATIC
  '
  ' ========================== DRAWGRAPH =========================
  '   Draws a bar graph from the data entered in the INPUTTITLES
  '   and INPUTDATA procedures.
  ' ==============================================================
  '
  Function DrawGraph$(ByRef T As TitleType, ByRef Label$(), ByRef Value() As Single, ByRef N As Short)
    Static YMax!
    Static YMin!
    Static I As Short
    Static BarWidth!
    Static BarSpace!
    Static Start As Short
    Static Power!
    Static ScaleFactor!
    Static YRange!
    Static Tile$
    Static Bottom!
    Static BarMid!
    Static CharMid!
    Static BarHeight!
  
     ' Set size of graph:
     ' CONST GRAPHTOP = 24, GRAPHBOTTOM = 171
     ' CONST GRAPHLEFT = 48, GRAPHRIGHT = 624
     ' CONST YLENGTH = GRAPHBOTTOM - GRAPHTOP
  
     ' Calculate max/min values:
     YMax = 0
     YMin = 0
     For I = 1 To N
        If Value(I) < YMin Then YMin = Value(I)
        If Value(I) > YMax Then YMax = Value(I)
     Next I
  
     ' Calculate width of bars and space between them:
     BarWidth = CSng((GRAPHRIGHT - GRAPHLEFT) / N)
     BarSpace = .2! * BarWidth
     BarWidth = BarWidth - BarSpace
  
     SCREEN(2)
     CLS()
  
     ' Draw y axis:
     LINE(GRAPHLEFT, GRAPHTOP, GRAPHLEFT, GRAPHBOTTOM, 1)
  
     ' Draw main graph title:
     Start = CShort(44 - (Len(RTrim$(T.MainTitle)) / 2))
     LOCATE(2, Start)
     PRINT(RTrim$(T.MainTitle), True)
  
     ' Annotate Y axis:
     Start = CShort(13 - (Len(RTrim$(T.YTitle)) / 2))
     For I = 1 To CShort(Len(RTrim$(T.YTitle)))
        LOCATE((Start + I) - 1, 1)
        PRINT(Mid$(T.YTitle, I, 1), True)
     Next I
  
     ' Calculate scale factor so labels aren't bigger than 4 digits:
     If Abs(YMax) > Abs(YMin) Then
        Power = YMax
     Else
        Power = YMin
     End If
     Power = CShort(Log(Abs(Power) / 100) / Log(10))
     If Power < 0 Then Power = 0
  
     ' Scale min and max down:
     ScaleFactor = CSng(10 ^ Power)
     YMax = CShort(YMax / ScaleFactor)
     YMin = CShort(YMin / ScaleFactor)
  
     ' If power isn't zero then put scale factor on chart:
     If Power <> 0 Then
        LOCATE(3, 2)
        PRINT("x 10^" & LTrim$(Str$(Power)))
     End If
  
     ' Put tic mark and number for Max point on Y axis:
     LINE(False, GRAPHLEFT - 3, GRAPHTOP, [STEP], 3, 0)
     LOCATE(4, 2)
     PRINT_USING("####", YMax)
  
     ' Put tic mark and number for Min point on Y axis:
     LINE(False, GRAPHLEFT - 3, GRAPHBOTTOM, [STEP], 3, 0)
     LOCATE(22, 2)
     PRINT_USING("####", YMin)
  
     ' Scale min and max back up for charting calculations:
     YMax = YMax * ScaleFactor
     YMin = YMin * ScaleFactor
  
     ' Annotate X axis:
     Start = CShort(44 - (Len(RTrim$(T.XTitle)) / 2))
     LOCATE(25, Start)
     PRINT(RTrim$(T.XTitle), True)
  
     ' Calculate the pixel range for the Y axis:
     YRange = YMax - YMin
  
     ' Define a diagonally striped pattern:
     Tile$ = ((((((QBChr(1) + QBChr(2)) + QBChr(4)) + QBChr(8)) + QBChr(16)) + QBChr(32)) + QBChr(64)) + QBChr(128)
  
     ' Draw a zero line if appropriate:
     If YMin < 0 Then
        Bottom = GRAPHBOTTOM - (((-YMin) / YRange) * YLENGTH)
        LOCATE(CInt(Int((Bottom - 1) / 8) + 1), 5)
        PRINT("0", True)
     Else
        Bottom = GRAPHBOTTOM
     End If
  
     ' Draw x axis:
     LINE(GRAPHLEFT - 3, Bottom, GRAPHRIGHT, Bottom)
  
     ' Draw bars and labels:
     Start = CShort(GRAPHLEFT + (BarSpace / 2))
     For I = 1 To N
  
        ' Draw a bar label:
        BarMid = Start + (BarWidth / 2)
        CharMid = Int((BarMid - 1) / 8) + 1
        LOCATE(23, CInt(CharMid - Int(Len(RTrim$(Label$(I))) / 2)))
        PRINT(Label$(I), True)
  
        ' Draw the bar and fill it with the striped pattern:
        BarHeight = (Value(I) / YRange) * YLENGTH
        LINE(False, Start, Bottom, [STEP], BarWidth, -BarHeight, -1, LineOption.B)
        PAINT(BarMid, Bottom - (BarHeight / 2), Tile$, 1)
  
        Start = CShort((Start + BarWidth) + BarSpace)
     Next I
  
     LOCATE(1, 1, 1)
     PRINT("New graph? ", True)
     DrawGraph$ = UCase$(InputStr(1))
  
  End Function
  
  '
  ' ========================= INPUTDATA ========================
  '         Gets input for the bar labels and their values
  ' ============================================================
  '
  Function InputData(ByRef Label$(), ByRef Value() As Single) As Short
    Static NumData As Short
    Static Done As Short
  
     ' Initialize the number of data values:
     NumData = 0
  
     ' Print data-entry instructions:
     CLS()
     PRINT("Enter data for up to 5 bars:")
     PRINT("   * Enter the label and value for each bar.")
     PRINT("   * Values can be negative.")
     PRINT("   * Enter a blank label to stop.")
     PRINT()
     PRINT("After viewing the graph, press any key ", True)
     PRINT("to end the program.")
  
     ' Accept data until blank label or 5 entries:
     Done = [FALSE]
     Do
        NumData = CShort(NumData + 1)
        PRINT()
        PRINT("Bar(" & LTrim$(Str$(NumData)) & "):")
        INPUT("        Label? ", Label$(NumData), False)
  
        ' Only input value if label isn't blank:
        If Label$(NumData) <> "" Then
           LOCATE(-1, 35)
           INPUT("Value? ", Value(NumData), False)
  
        ' If label was blank, decrement data counter and
        ' set Done flag equal to TRUE:
        Else
           NumData = CShort(NumData - 1)
           Done = [TRUE]
        End If
     Loop Until (CShort((NumData = 5)) Or Done) <> 0
  
     ' Return the number of data values input:
     InputData = NumData
  
  End Function
  
  '
  ' ======================= INPUTTITLES ========================
  '       Accepts input for the three different graph titles
  ' ============================================================
  '
  Sub InputTitles(ByRef T As TitleType)
    Static OK$
  
     ' Set text screen:
     SCREEN(0, 0)
  
     ' Input Titles
     Do
        CLS()
        INPUT("Enter main graph title: ", T.MainTitle, False)
        INPUT("Enter X-Axis title    : ", T.XTitle, False)
        INPUT("Enter Y-Axis title    : ", T.YTitle, False)
  
        ' Check to see if titles are OK:
        LOCATE(7, 1)
        PRINT("OK (Y to continue, N to change)? ", True)
        LOCATE(-1, -1, 1)
        OK$ = UCase$(InputStr(1))
     Loop Until OK$ = "Y"
  End Sub
  
End Module
