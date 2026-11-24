' Converted from QBasic by qb2vb.

Module Program

  Structure MonthType
    Implements IFileRecord
    Public Number As Short
    Private m_MName As String
    Public Property MName As String
      Get
        Return If(m_MName, FixedString.Blank(9))
      End Get
      Set(value As String)
        m_MName = FixedString.Fit(value, 9)
      End Set
    End Property
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.Write(Number)
      writer.WriteString(MName, 9)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      Number = reader.ReadInt16()
      MName = reader.ReadString(9)
    End Sub
  End Structure
  
  Const LEAP As Short = CShort(366 Mod 7)
  Const NORMAL As Short = CShort(365 Mod 7)
  Dim MonthData(12) As MonthType

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Year As Short
    Dim Month As Short
    Dim Resp$ = ""
    DATA("January", 31, "February", 28, "March", 31)
    DATA("April", 30, "May", 31, "June", 30, "July", 31, "August", 31)
    DATA("September", 30, "October", 31, "November", 30, "December", 31)
    
    ' DEFINT A-Z               ' Default variable type is integer
    
    ' Define a data type for the names of the months and the
    ' number of days in each:
    ' TYPE MonthType (see the Structure at the top)
    
    ' Declare procedures used:
    ' DECLARE FUNCTION IsLeapYear% (N%)
    ' DECLARE FUNCTION GetInput% (Prompt$, Row%, LowVal%, HighVal%)
    
    ' DECLARE SUB PrintCalendar (Year%, Month%)
    ' DECLARE SUB ComputeMonth (Year%, Month%, StartDay%, TotalDays%)
    
    
    ' Initialize month definitions from DATA statements below:
    For I = 1S To 12S
       READ(MonthData(I).MName) : READ(MonthData(I).Number)
    Next
    
    ' Main loop, repeat for as many months as desired:
    Do
    
       CLS()
    
       ' Get year and month as input:
       Year = GetInput("Year (1899 to 2099): ", 1, 1899, 2099)
       Month = GetInput("Month (1 to 12): ", 2, 1, 12)
    
       ' Print the calendar:
       PrintCalendar(Year, Month)
    
       ' Another Date?
       LOCATE(13, 1)  ' Locate in 13th row, 1st column
       PRINT("New Date? ", True)  ' Keep cursor on same line
       LOCATE(-1, -1, 1, 0, 13)  ' Turn cursor on and make it one
                            ' character high
       Resp$ = InputStr(1)  ' Wait for a key press
       PRINT(Resp$)  ' Print the key pressed
    
    Loop While UCase$(Resp$) = "Y"
    End
    
    ' Data for the months of a year:
    ' DATA January, 31, February, 28, March, 31   (DATA is listed at the start of the program)
    ' DATA April, 30, May, 31, June, 30, July, 31, August, 31   (DATA is listed at the start of the program)
    ' DATA September, 30, October, 31, November, 30, December, 31   (DATA is listed at the start of the program)
  End Sub

  '
  ' ====================== COMPUTEMONTH ========================
  '     Computes the first day and the total days in a month.
  ' ============================================================
  '
  Sub ComputeMonth(ByRef Year As Short, ByRef Month As Short, ByRef StartDay As Short, ByRef TotalDays As Short)
    Static NumDays As Short
    Static I As Short
     ' SHARED MonthData() AS MonthType   (shared variables are declared at module level)
     ' CONST LEAP = 366 MOD 7
     ' CONST NORMAL = 365 MOD 7
  
     ' Calculate total number of days (NumDays) since 1/1/1899.
  
     ' Start with whole years:
     NumDays = 0
     For I = 1899 To CShort(Year - 1)
        If IsLeapYear(I) <> 0 Then  ' If year is leap, add
           NumDays = NumDays + LEAP  ' 366 MOD 7.
        Else  ' If normal year, add
           NumDays = NumDays + NORMAL  ' 365 MOD 7.
        End If
     Next
  
     ' Next, add in days from whole months:
     For I = 1 To CShort(Month - 1)
        NumDays = NumDays + MonthData(I).Number
     Next
  
     ' Set the number of days in the requested month:
     TotalDays = MonthData(Month).Number
  
     ' Compensate if requested year is a leap year:
     If IsLeapYear(Year) <> 0 Then
  
        ' If after February, add one to total days:
        If Month > 2 Then
           NumDays = CShort(NumDays + 1)
  
        ' If February, add one to the month's days:
        ElseIf Month = 2 Then
           TotalDays = CShort(TotalDays + 1)
  
        End If
     End If
  
     ' 1/1/1899 was a Sunday, so calculating "NumDays MOD 7"
     ' gives the day of week (Sunday = 0, Monday = 1, Tuesday = 2,
     ' and so on) for the first day of the input month:
     StartDay = CShort(NumDays Mod 7)
  End Sub
  
  '
  ' ======================== GETINPUT ==========================
  '       Prompts for input, then tests for a valid range.
  ' ============================================================
  '
  Function GetInput(ByRef Prompt$, ByRef Row As Short, ByRef LowVal As Short, ByRef HighVal As Short) As Short
    Static Column As Short
    Static Value As Short
  
     ' Locate prompt at specified row, turn cursor on and
     ' make it one character high:
     LOCATE(Row, 1, 1, 0, 13)
     PRINT(Prompt$, True)
  
     ' Save column position:
     Column = CShort(POS(0))
  
     ' Input value until it's within range:
     Do
        LOCATE(Row, Column)  ' Locate cursor at end of prompt
        PRINT(Space$(10))  ' Erase anything already there
        LOCATE(Row, Column)  ' Relocate cursor at end of prompt
        INPUT("", Value, False)  ' Input value with no prompt
     Loop While ((Value < LowVal) Or (Value > HighVal))
  
     ' Return valid input as value of function:
     GetInput = Value
  
  End Function
  
  '
  ' ====================== ISLEAPYEAR ==========================
  '         Determines if a year is a leap year or not.
  ' ============================================================
  '
  Function IsLeapYear(ByRef N As Short) As Short
  
     ' If the year is evenly divisible by 4 and not divisible
     ' by 100, or if the year is evenly divisible by 400, then
     ' it's a leap year:
     IsLeapYear = CShort((((N Mod 4) = 0) And ((N Mod 100) <> 0)) Or ((N Mod 400) = 0))
  End Function
  
  '
  ' ===================== PRINTCALENDAR ========================
  '     Prints a formatted calendar given the year and month.
  ' ============================================================
  '
  Sub PrintCalendar(ByRef Year As Short, ByRef Month As Short)
    Static StartDay As Short
    Static TotalDays As Short
    Static Header$ = ""
    Static LeftMargin As Short
    Static I As Short
  ' SHARED MonthData() AS MonthType   (shared variables are declared at module level)
  
     ' Compute starting day (Su M Tu ...) and total days
     ' for the month:
     ComputeMonth(Year, Month, StartDay, TotalDays)
     CLS()
     Header$ = (RTrim$(MonthData(Month).MName) + ",") + Str$(Year)
  
     ' Calculates location for centering month and year:
     LeftMargin = CShort((35 - Len(Header$)) \ 2)
  
     ' Print header:
     PRINT(TAB(LeftMargin) & Header$)
     PRINT()
     PRINT("Su    M   Tu    W   Th    F   Sa")
     PRINT()
  
     ' Recalculate and print tab to the first day
     ' of the month (Su M Tu ...):
     LeftMargin = CShort((5 * StartDay) + 1)
     PRINT(TAB(LeftMargin), True)
  
     ' Print out the days of the month:
     For I = 1 To TotalDays
        PRINT_USING("##   ", I)
  
        ' Advance to the next line when the cursor
        ' is past column 32:
        If POS(0) > 32 Then PRINT()
     Next
  
  End Sub
  
End Module
