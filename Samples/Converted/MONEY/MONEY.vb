' Converted from QBasic by qb2vb.  Lines marked TODO(QB2VB) need attention.

Module Program

  Const [TRUE] As Short = -1
  Const [FALSE] As Short = Not [TRUE]
  Structure AccountType
    Implements IFileRecord
    Private m_Title As String
    Public Property Title As String
      Get
        Return If(m_Title, FixedString.Blank(20))
      End Get
      Set(value As String)
        m_Title = FixedString.Fit(value, 20)
      End Set
    End Property
    Private m_AType As String
    Public Property AType As String
      Get
        Return If(m_AType, FixedString.Blank(1))
      End Get
      Set(value As String)
        m_AType = FixedString.Fit(value, 1)
      End Set
    End Property
    Private m_Desc As String
    Public Property Desc As String
      Get
        Return If(m_Desc, FixedString.Blank(50))
      End Get
      Set(value As String)
        m_Desc = FixedString.Fit(value, 50)
      End Set
    End Property
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(Title, 20)
      writer.WriteString(AType, 1)
      writer.WriteString(Desc, 50)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      Title = reader.ReadString(20)
      AType = reader.ReadString(1)
      Desc = reader.ReadString(50)
    End Sub
  End Structure
  
  Structure Recordtype
    Implements IFileRecord
    Private m_Date As String
    Public Property [Date] As String
      Get
        Return If(m_Date, FixedString.Blank(8))
      End Get
      Set(value As String)
        m_Date = FixedString.Fit(value, 8)
      End Set
    End Property
    Private m_Ref As String
    Public Property Ref As String
      Get
        Return If(m_Ref, FixedString.Blank(10))
      End Get
      Set(value As String)
        m_Ref = FixedString.Fit(value, 10)
      End Set
    End Property
    Private m_Desc As String
    Public Property Desc As String
      Get
        Return If(m_Desc, FixedString.Blank(50))
      End Get
      Set(value As String)
        m_Desc = FixedString.Fit(value, 50)
      End Set
    End Property
    Public Fig1 As Double
    Public Fig2 As Double
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString([Date], 8)
      writer.WriteString(Ref, 10)
      writer.WriteString(Desc, 50)
      writer.Write(Fig1)
      writer.Write(Fig2)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      [Date] = reader.ReadString(8)
      Ref = reader.ReadString(10)
      Desc = reader.ReadString(50)
      Fig1 = reader.ReadDouble()
      Fig2 = reader.ReadDouble()
    End Sub
  End Structure
  
  Dim account(19) As AccountType
  Dim ColorPref As Short
  Dim colors(20, 4) As Short
  Dim ScrollUpAsm(7) As Short
  Dim ScrollDownAsm(7) As Short
  Dim PrintErr As Short
  Dim a As Short

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim KeyFlags As Short
    DATA(0, 7, 15, 7, 0, 7, 0, 15, 0, 0)
    DATA(1, 9, 12, 3, 0, 1, 15, 0, 7, 0)
    DATA(3, 15, 13, 1, 14, 3, 15, 0, 7, 0)
    DATA(7, 12, 15, 4, 14, 0, 15, 15, 1, 0)
    DATA(184, 1, 6, 185, 1, 4, 186, 78, 22, 183, 0, 205, 16, 203)
    DATA(184, 1, 7, 185, 1, 4, 186, 78, 22, 183, 0, 205, 16, 203)
    
    '
    '                    Q B a s i c   M O N E Y   M A N A G E R
    '
    '                   Copyright (C) Microsoft Corporation 1990
    '
    ' The Money Manager is a personal finance manager that allows you
    ' to enter account transactions while tracking your account balances
    ' and net worth.
    '
    ' To run this program, press Shift+F5.
    '
    ' To exit QBasic, press Alt, F, X.
    '
    ' To get help on a BASIC keyword, move the cursor to the keyword and press
    ' F1 or click the right mouse button.
    '
    
    
    'Set default data type to integer for faster operation
    ' DEFINT A-Z
    
    'Sub and function declarations
    ' DECLARE SUB TransactionSummary (item%)
    ' DECLARE SUB LCenter (text$)
    ' DECLARE SUB ScrollUp ()
    ' DECLARE SUB ScrollDown ()
    ' DECLARE SUB Initialize ()
    ' DECLARE SUB Intro ()
    ' DECLARE SUB SparklePause ()
    ' DECLARE SUB Center (row%, text$)
    ' DECLARE SUB FancyCls (dots%, Background%)
    ' DECLARE SUB LoadState ()
    ' DECLARE SUB SaveState ()
    ' DECLARE SUB MenuSystem ()
    ' DECLARE SUB MakeBackup ()
    ' DECLARE SUB RestoreBackup ()
    ' DECLARE SUB Box (Row1%, Col1%, Row2%, Col2%)
    ' DECLARE SUB NetWorthReport ()
    ' DECLARE SUB EditAccounts ()
    ' DECLARE SUB PrintHelpLine (help$)
    ' DECLARE SUB EditTrans (item%)
    ' DECLARE FUNCTION Cvdt$ (X#)
    ' DECLARE FUNCTION Cvst$ (X!)
    ' DECLARE FUNCTION Cvit$ (X%)
    ' DECLARE FUNCTION Menu% (CurrChoiceX%, MaxChoice%, choice$(), ItemRow%(), ItemCol%(), help$(), BarMode%)
    ' DECLARE FUNCTION GetString$ (row%, col%, start$, end$, Vis%, Max%)
    ' DECLARE FUNCTION Trim$ (X$)
    
    'Constants
    ' CONST TRUE = -1
    ' CONST FALSE = NOT TRUE
    
    'User-defined types
    ' TYPE AccountType (see the Structure at the top)
    
    ' TYPE Recordtype (see the Structure at the top)
    
    'Global variables
    ' DIM SHARED account(1 TO 19)  AS AccountType     'Stores the 19 account titles
    ' DIM SHARED ColorPref                            'Color Preference
    ' DIM SHARED colors(0 TO 20, 1 TO 4)              'Different Colors
    ' DIM SHARED ScrollUpAsm(1 TO 7)                  'Assembly Language Routines
    ' DIM SHARED ScrollDownAsm(1 TO 7)
    ' DIM SHARED PrintErr AS INTEGER                  'Printer error flag
    
        DEF_SEG(0)  ' Turn off CapLock, NumLock and ScrollLock
        KeyFlags = CShort(PEEK(1047))
        POKE(1047, 0)
        DEF_SEG()
    
        'Open money manager data file.  If it does not exist in current directory,
        '  goto error handler to create and initialize it.
        On Error GoTo ErrorTrap
        OPEN("money.dat", OpenMode.Input, 1)
        CLOSE()
        On Error GoTo 0  'Reset error handler
    
        Initialize()  'Initialize program
        Intro()  'Display introduction screen
        MenuSystem()  'This is the main program
        COLOR(7, 0)  'Clear screen and end
        CLS()
    
        DEF_SEG(0)  ' Restore CapLock, NumLock and ScrollLock states
        POKE(1047, KeyFlags)
        DEF_SEG()
    
        End
    
    ' Error handler for program
    ' If data file not found, create and initialize a new one.
    ErrorTrap:
        Select Case Err.Number
            ' If data file not found, create and initialize a new one.
            Case 53
                CLOSE()
                ColorPref = 1
                For a = 1 To 19
                    account(a).Title = ""
                    account(a).AType = ""
                    account(a).Desc = ""
                Next a
                SaveState()
                Resume
            Case 24, 25
                PrintErr = [TRUE]
                Box(8, 13, 14, 69)
                Center(11, "Printer not responding ... Press Space to continue")
                While INKEY$() <> "" : End While
                While INKEY$() <> " " : End While
                Resume Next
            Case Else
        End Select
        Resume Next
    
    
    'The following data defines the color schemes available via the main menu.
    '
    '    scrn  dots  bar  back   title  shdow  choice  curs   cursbk  shdow
    ' DATA 0,    7,    15,  7,     0,     7,     0,      15,    0,      0   (DATA is listed at the start of the program)
    ' DATA 1,    9,    12,  3,     0,     1,     15,     0,     7,      0   (DATA is listed at the start of the program)
    ' DATA 3,    15,   13,  1,     14,    3,     15,     0,     7,      0   (DATA is listed at the start of the program)
    ' DATA 7,    12,   15,  4,     14,    0,     15,     15,    1,      0   (DATA is listed at the start of the program)
    
    'The following data is actually a machine language program to
    'scroll the screen up or down very fast using a BIOS call.
    ' DATA &HB8,&H01,&H06,&HB9,&H01,&H04,&HBA,&H4E,&H16,&HB7,&H00,&HCD,&H10,&HCB   (DATA is listed at the start of the program)
    ' DATA &HB8,&H01,&H07,&HB9,&H01,&H04,&HBA,&H4E,&H16,&HB7,&H00,&HCD,&H10,&HCB   (DATA is listed at the start of the program)
    
  End Sub

  
  'Box:
  '  Draw a box on the screen between the given coordinates.
  Sub Box(ByRef Row1 As Short, ByRef Col1 As Short, ByRef Row2 As Short, ByRef Col2 As Short)
    Static BoxWidth As Short
  
      BoxWidth = CShort((Col2 - Col1) + 1)
  
      LOCATE(Row1, Col1)
      PRINT("┌" & QBString(BoxWidth - 2, "─") & "┐", True)
  
      For a = CShort(Row1 + 1) To CShort(Row2 - 1)
          LOCATE(a, Col1)
          PRINT("│" & Space$(BoxWidth - 2) & "│", True)
      Next a
  
      LOCATE(Row2, Col1)
      PRINT("└" & QBString(BoxWidth - 2, "─") & "┘", True)
  
  End Sub
  
  
  'Center:
  '  Center text on the given row.
  Sub Center(ByRef row As Short, ByRef text$)
      LOCATE(row, CInt(41 - (Len(text$) / 2)))
      PRINT(text$, True)
  End Sub
  
  
  'Cvdt$:
  '  Convert a double precision number to a string WITHOUT a leading space.
  Function Cvdt$(ByRef X#)
  
      Cvdt$ = Right$(Str$(X#), Len(Str$(X#)) - 1)
  
  End Function
  
  
  'Cvit$:
  '  Convert an integer to a string WITHOUT a leading space.
  Function Cvit$(ByRef X As Short)
      Cvit$ = Right$(Str$(X), Len(Str$(X)) - 1)
  End Function
  
  
  'Cvst$:
  '  Convert a single precision number to a string WITHOUT a leading space
  Function Cvst$(ByRef X!)
      Cvst$ = Right$(Str$(X!), Len(Str$(X!)) - 1)
  End Function
  
  
  'EditAccounts:
  '  This is the full-screen editor which allows you to change your account
  '  titles and descriptions
  Sub EditAccounts()
    Dim help$()
    Dim col() As Short
    Dim Vis() As Short
    Dim Max() As Short
    Dim edit$(,)
    Dim u$
    Dim X As Short
    Dim finished As Short
    Dim CurrRow As Short
    Dim CurrCol As Short
    Dim Kbd$
    Dim Save As Short
    Dim ok As Short
    Dim start$
    Dim end_$
    Dim X_Str$
    Dim EditAccountsShowCursor As Action
    Dim EditAccountsEditItem As Action
    Dim EditAccountsHideCursor As Action
    Dim EditAccountsSaveData As Action
    EditAccountsShowCursor = Sub()
      COLOR(colors(8, ColorPref), colors(9, ColorPref))
      LOCATE(CurrRow + 4, col(CurrCol))
      PRINT(Left$(edit$(CurrRow, CurrCol), Vis(CurrCol)), True)
      Return
    End Sub
    EditAccountsEditItem = Sub()
      COLOR(colors(8, ColorPref), colors(9, ColorPref))
      ok = [FALSE]
      start$ = Kbd$
      Do
          Kbd$ = GetString$(CShort(CurrRow + 4), col(CurrCol), start$, end_$, Vis(CurrCol), Max(CurrCol))
          edit$(CurrRow, CurrCol) = Left$(end_$ + Space$(Max(CurrCol)), Max(CurrCol))
          start$ = ""
  
          If CurrCol = 3 Then
              X_Str$ = UCase$(end_$)
              If (((X_Str$ = "A") Or (X_Str$ = "L")) Or (X_Str$ = "")) Or (X_Str$ = " ") Then
                  ok = [TRUE]
                  If X_Str$ = "" Then X_Str$ = " "
                  edit$(CurrRow, CurrCol) = X_Str$
              Else
                  BEEP()
              End If
          Else
              ok = [TRUE]
          End If
  
      Loop Until ok <> 0
      Return
    End Sub
    EditAccountsHideCursor = Sub()
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      LOCATE(CurrRow + 4, col(CurrCol))
      PRINT(Left$(edit$(CurrRow, CurrCol), Vis(CurrCol)), True)
      Return
    End Sub
    EditAccountsSaveData = Sub()
      For a = 1 To 19
          account(a).Title = edit$(a, 1)
          account(a).Desc = edit$(a, 2)
          account(a).AType = edit$(a, 3)
      Next a
      SaveState()
      Return
    End Sub
  
      'Information about each column
      ReDim help$(4), col(4), Vis(4), Max(4), edit$(19, 3)
  
      'Draw the screen
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      Box(2, 1, 24, 80)
  
      COLOR(colors(5, ColorPref), colors(4, ColorPref))
      LOCATE(1, 1) : PRINT(Space$(80))
      LOCATE(1, 4) : PRINT("Account Editor", True)
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
  
      LOCATE(3, 2) : PRINT("No│ Account Title      │ Description                                      │A/L")
      LOCATE(4, 2) : PRINT("──┼────────────────────┼──────────────────────────────────────────────────┼───")
                    u$ = "##│\                  \│\                                                \│ ! "
      For a = 5 To 23
          LOCATE(a, 2)
          X = CShort(a - 4)
          PRINT_USING(u$, X, account(X).Title, account(X).Desc, account(X).AType)
      Next a
  
      'Initialize variables
      help$(1) = "  Account name                             | <F2=Save and Exit> <Escape=Abort>"
      help$(2) = "  Account description                      | <F2=Save and Exit> <Escape=Abort>"
      help$(3) = "  Account type (A = Asset, L = Liability)  | <F2=Save and Exit> <Escape=Abort>"
  
      col(1) = 5 : col(2) = 26 : col(3) = 78
      Vis(1) = 20 : Vis(2) = 50 : Vis(3) = 1
      Max(1) = 20 : Max(2) = 50 : Max(3) = 1
  
      For a = 1 To 19
          edit$(a, 1) = account(a).Title
          edit$(a, 2) = account(a).Desc
          edit$(a, 3) = account(a).AType
      Next a
  
      finished = [FALSE]
  
      CurrRow = 1
      CurrCol = 1
      PrintHelpLine(help$(CurrCol))
  
      'Loop until F2 or <ESC> is pressed
      Do
          EditAccountsShowCursor()  'Show Cursor
          Do  'Wait for key
              Kbd$ = INKEY$()
          Loop Until Kbd$ <> ""
  
          If (Kbd$ >= " ") And (Kbd$ < "~") Then  'If legal, edit item
              EditAccountsEditItem()
          End If
          EditAccountsHideCursor()  'Hide Cursor so it can move
                                                  'If it needs to
          Select Case Kbd$
              Case QBChr(0) + "H"  'Up Arrow
                  CurrRow = CShort(((CurrRow + 17) Mod 19) + 1)
              Case QBChr(0) + "P"  'Down Arrow
                  CurrRow = CShort(((CurrRow) Mod 19) + 1)
              Case QBChr(0) + "K", QBChr(0) + QBChr(15)  'Left or Shift+Tab
                  CurrCol = CShort(((CurrCol + 1) Mod 3) + 1)
                  PrintHelpLine(help$(CurrCol))
              Case QBChr(0) + "M", QBChr(9)  'Right or Tab
                  CurrCol = CShort(((CurrCol) Mod 3) + 1)
                  PrintHelpLine(help$(CurrCol))
              Case QBChr(0) + "<"  'F2
                  finished = [TRUE]
                  Save = [TRUE]
              Case QBChr(27)  'Esc
                  finished = [TRUE]
                  Save = [FALSE]
              Case QBChr(13)  'Return
              Case Else
                  BEEP()
          End Select
      Loop Until finished <> 0
  
      If Save <> 0 Then
          EditAccountsSaveData()
      End If
  
      Exit Sub
  
  
  
  
  
  
  End Sub
  
  
  'EditTrans:
  '  This is the full-screen editor which allows you to enter and change
  '  transactions
  Sub EditTrans(ByRef item As Short)
    Dim help$()
    Dim col() As Short
    Dim Vis() As Short
    Dim Max() As Short
    Dim CurrString$()
    Dim CurrFig#()
    Dim Balance#()
    Dim Fields1 As FieldRecord
    Dim Fields2 As FieldRecord
    Dim file_$
    Dim MaxRecord As Short
    Dim u$
    Dim u1$
    Dim u1x$
    Dim u2$
    Dim u3$
    Dim u4$
    Dim CurrTopline As Short
    Dim CurrRow As Short
    Dim CurrCol As Short
    Dim finished As Short
    Dim Kbd$
    Dim CurrRecord As Short
    Dim new_$
    Dim start$
    Dim new4#
    Dim new5#
    Dim temp As Short
    Dim EditTransWriteBalance As Action
    Dim EditTransPrintWholeScreen As Action
    Dim EditTransGetLine As Action
    Dim EditTransShowCursor As Action
    Dim EditTransHideCursor As Action
    Dim EditTransEditItem As Action
    Dim EditTransMoveUp As Action
    Dim EditTransMoveDown As Action
    Dim EditTransAddRecord As Action
    Dim EditTransDeleteRecord As Action
    Dim EditTransPutLine As Action
    Dim EditTransPrintBalances As Action
    Dim EditTransPrintLine As Action
    EditTransShowCursor = Sub()
      COLOR(colors(8, ColorPref), colors(9, ColorPref))
      LOCATE(CurrRow + 4, col(CurrCol))
      Select Case CurrCol
          Case 1, 2, 3
              PRINT(Left$(CurrString$(CurrCol), Vis(CurrCol)), True)
          Case 4
              If CurrFig#(4) <> 0 Then
                  PRINT_USING(u2$, CurrFig#(4))
              Else
                  PRINT(Space$(Vis(CurrCol)), True)
              End If
          Case 5
              If CurrFig#(5) <> 0 Then
                  PRINT_USING(u2$, CurrFig#(5))
              Else
                  PRINT(Space$(Vis(CurrCol)), True)
              End If
      End Select
      Return
    End Sub
    EditTransHideCursor = Sub()
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      LOCATE(CurrRow + 4, col(CurrCol))
      Select Case CurrCol
          Case 1, 2, 3
              PRINT(Left$(CurrString$(CurrCol), Vis(CurrCol)), True)
          Case 4
              If CurrFig#(4) <> 0 Then
                  PRINT_USING(u2$, CurrFig#(4))
              Else
                  PRINT(Space$(Vis(CurrCol)), True)
              End If
          Case 5
              If CurrFig#(5) <> 0 Then
                  PRINT_USING(u2$, CurrFig#(5))
              Else
                  PRINT(Space$(Vis(CurrCol)), True)
              End If
      End Select
      Return
    End Sub
    EditTransEditItem = Sub()
  
      CurrRecord = CShort((CurrTopline + CurrRow) - 1)
      COLOR(colors(8, ColorPref), colors(9, ColorPref))
  
      Select Case CurrCol
          Case 1, 2, 3
              Kbd$ = GetString$(CShort(CurrRow + 4), col(CurrCol), Kbd$, new_$, Vis(CurrCol), Max(CurrCol))
              CurrString$(CurrCol) = new_$
              EditTransPutLine()
              EditTransGetLine()
          Case 4
              start$ = Kbd$
              Do
                  Kbd$ = GetString$(CShort(CurrRow + 4), col(4), start$, new_$, Vis(4), Max(4))
                  new4# = Val(new_$)
                  start$ = ""
              Loop While (new4# >= 999999.99#) Or (new4# < 0)
  
              a = CurrRecord
              While a <= MaxRecord
                  Balance#(a) = ((Balance#(a) + new4#) - CurrFig#(4)) + CurrFig#(5)
                  a = CShort(a + 1)
              End While
              CurrFig#(4) = new4#
              CurrFig#(5) = 0
              EditTransPutLine()
              EditTransGetLine()
              EditTransPrintBalances()
              EditTransWriteBalance()
          Case 5
              start$ = Kbd$
              Do
                  Kbd$ = GetString$(CShort(CurrRow + 4), col(5), start$, new_$, Vis(5), Max(5))
                  new5# = Val(new_$)
                  start$ = ""
              Loop While (new5# >= 999999.99#) Or (new5# < 0)
  
              a = CurrRecord
              While a <= MaxRecord
                  Balance#(a) = ((Balance#(a) - new5#) + CurrFig#(5)) - CurrFig#(4)
                  a = CShort(a + 1)
              End While
              CurrFig#(4) = 0
              CurrFig#(5) = new5#
              EditTransPutLine()
              EditTransGetLine()
              EditTransPrintBalances()
              EditTransWriteBalance()
          Case Else
      End Select
      EditTransPrintLine()
      Return
    End Sub
    EditTransMoveUp = Sub()
      If CurrRow = 1 Then
          If CurrTopline = 1 Then
              BEEP()
          Else
              ScrollDown()
              CurrTopline = CShort(CurrTopline - 1)
              EditTransGetLine()
              EditTransPrintLine()
          End If
      Else
          CurrRow = CShort(CurrRow - 1)
          EditTransGetLine()
      End If
      Return
    End Sub
    EditTransMoveDown = Sub()
      If ((CurrRow + CurrTopline) - 1) >= MaxRecord Then
          BEEP()
      Else
          If CurrRow = 19 Then
              ScrollUp()
              CurrTopline = CShort(CurrTopline + 1)
              EditTransGetLine()
              EditTransPrintLine()
          Else
              CurrRow = CShort(CurrRow + 1)
              EditTransGetLine()
          End If
      End If
      Return
    End Sub
    EditTransPrintLine = Sub()
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      CurrRecord = CShort((CurrTopline + CurrRow) - 1)
      LOCATE(CurrRow + 4, 2)
      If CurrRecord = (MaxRecord + 1) Then
          PRINT(u1x$, True)
      ElseIf CurrRecord > MaxRecord Then
          PRINT(u1$, True)
      Else
          PRINT_USING(u$, CurrString$(1), CurrString$(2), CurrString$(3))
          If (CurrFig#(4) = 0) And (CurrFig#(5) = 0) Then
              PRINT_USING((((u4$ + "│") + u4$) + "│") + u3$, Balance#(CurrRecord))
          ElseIf CurrFig#(5) = 0 Then
              PRINT_USING((((u2$ + "│") + u4$) + "│") + u3$, CurrFig#(4), Balance#(CurrRecord))
          Else
              PRINT_USING((((u4$ + "│") + u2$) + "│") + u3$, CurrFig#(5), Balance#(CurrRecord))
          End If
      End If
      Return
    End Sub
    EditTransPrintBalances = Sub()
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      For a = 1 To 19
          CurrRecord = CShort((CurrTopline + a) - 1)
          If CurrRecord <= MaxRecord Then
              LOCATE(4 + a, 66)
              PRINT_USING(u3$, Balance#((CurrTopline + a) - 1))
          End If
      Next a
      Return
    End Sub
    EditTransDeleteRecord = Sub()
      If MaxRecord = 1 Then
          BEEP()
      Else
          CurrRecord = CShort((CurrTopline + CurrRow) - 1)
          MaxRecord = CShort(MaxRecord - 1)
          a = CurrRecord
          While a <= MaxRecord
              GET_FIELD(1, a + 2)
              PUT_FIELD(1, a + 1)
              Balance#(a) = (Balance#(a + 1) - CurrFig#(4)) + CurrFig#(5)
              a = CShort(a + 1)
          End While
  
          Fields2.LSET(0, "THISISVALID")
          Fields2.LSET(1, Cvit$(MaxRecord))
          PUT_FIELD(1, 1)
          EditTransPrintWholeScreen()
          CurrRecord = CShort((CurrTopline + CurrRow) - 1)
          If CurrRecord > MaxRecord Then
              EditTransMoveUp()
          End If
          EditTransGetLine()
          EditTransWriteBalance()
      End If
      Return
    End Sub
    EditTransAddRecord = Sub()
      CurrRecord = CShort((CurrTopline + CurrRow) - 1)
      a = MaxRecord
      While a > CurrRecord
          GET_FIELD(1, a + 1)
          PUT_FIELD(1, a + 2)
          Balance#(a + 1) = Balance#(a)
          a = CShort(a - 1)
      End While
      Balance#(CurrRecord + 1) = Balance#(CurrRecord)
      MaxRecord = CShort(MaxRecord + 1)
      Fields1.LSET(0, "")
      Fields1.LSET(1, "")
      Fields1.LSET(2, "")
      Fields1.LSET(3, MKD$(0))
      Fields1.LSET(4, MKD$(0))
      PUT_FIELD(1, CurrRecord + 2)
  
      Fields2.LSET(0, "THISISVALID")
      Fields2.LSET(1, Cvit$(MaxRecord))
      PUT_FIELD(1, 1)
      EditTransPrintWholeScreen()
      EditTransGetLine()
      Return
    End Sub
    EditTransPrintWholeScreen = Sub()
      temp = CurrRow
      For CurrRow = 1 To 19
          CurrRecord = CShort((CurrTopline + CurrRow) - 1)
          If CurrRecord <= MaxRecord Then
              EditTransGetLine()
          End If
          EditTransPrintLine()
      Next CurrRow
      CurrRow = temp
      Return
    End Sub
    EditTransWriteBalance = Sub()
      GET_FIELD(1, 1)
      Fields2.LSET(2, MKD$(Balance#(MaxRecord)))
      PUT_FIELD(1, 1)
      Return
    End Sub
    EditTransPutLine = Sub()
      CurrRecord = CShort((CurrTopline + CurrRow) - 1)
      Fields1.LSET(0, CurrString$(1))
      Fields1.LSET(1, CurrString$(2))
      Fields1.LSET(2, CurrString$(3))
      Fields1.LSET(3, MKD$(CurrFig#(4)))
      Fields1.LSET(4, MKD$(CurrFig#(5)))
      PUT_FIELD(1, CurrRecord + 1)
      Return
    End Sub
    EditTransGetLine = Sub()
      CurrRecord = CShort((CurrTopline + CurrRow) - 1)
      GET_FIELD(1, CurrRecord + 1)
      CurrString$(1) = Fields1(0)
      CurrString$(2) = Fields1(1)
      CurrString$(3) = Fields1(2)
      CurrFig#(4) = CVD(Fields1(3))
      CurrFig#(5) = CVD(Fields1(4))
      Return
    End Sub
  
      'Stores info about each column
      ReDim help$(6), col(6), Vis(6), Max(6), CurrString$(3), CurrFig#(5)
      'Array to keep the current balance at all the transactions
      ReDim Balance#(1000)
  
      'Open random access file
      file_$ = "money." + Cvit$(item)
      OPEN(file_$, OpenMode.Random, 1, 84)
      Fields1 = FIELD(1, 8, 10, 50, 8, 8)
      Fields2 = FIELD(1, 11, 5, 8)
  
      'Initialize variables
      CurrString$(1) = ""
      CurrString$(2) = ""
      CurrString$(3) = ""
      CurrFig#(4) = 0
      CurrFig#(5) = 0
  
      GET_FIELD(1, 1)
      If Fields2(0) <> "THISISVALID" Then
          Fields1.LSET(0, "")
          Fields1.LSET(1, "")
          Fields1.LSET(2, "")
          Fields1.LSET(3, MKD$(0))
          Fields1.LSET(4, MKD$(0))
          PUT_FIELD(1, 2)
          Fields2.LSET(0, "THISISVALID")
          Fields2.LSET(1, "1")
          Fields2.LSET(2, MKD$(0))
          PUT_FIELD(1, 1)
      End If
  
      MaxRecord = CShort(Val(Fields2(1)))
  
      Balance#(0) = 0
      a = 1
      While a <= MaxRecord
          GET_FIELD(1, a + 1)
          Balance#(a) = (Balance#(a - 1) + CVD(Fields1(3))) - CVD(Fields1(4))
          a = CShort(a + 1)
      End While
      EditTransWriteBalance()
  
      help$(1) = "Date of transaction (mm/dd/yy) "
      help$(2) = "Transaction reference number   "
      help$(3) = "Transaction description        "
      help$(4) = "Increase asset or debt value   "
      help$(5) = "Decrease asset or debt value   "
  
      col(1) = 2
      col(2) = 11
      col(3) = 18
      col(4) = 44
      col(5) = 55
  
      Vis(1) = 8
      Vis(2) = 6
      Vis(3) = 25
      Vis(4) = 10
      Vis(5) = 10
  
      Max(1) = 8
      Max(2) = 6
      Max(3) = 25
      Max(4) = 10
      Max(5) = 10
  
  
      'Draw Screen
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      Box(2, 1, 24, 80)
  
      COLOR(colors(5, ColorPref), colors(4, ColorPref))
      LOCATE(1, 1) : PRINT(Space$(80), True)
      LOCATE(1, 4) : PRINT(("Transaction Editor: " + Trim$(account(item).Title)), True)
  
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      LOCATE(3, 2) : PRINT("  Date  │ Ref# │ Description             │ Increase │ Decrease │   Balance    ")
      LOCATE(4, 2) : PRINT("────────┼──────┼─────────────────────────┼──────────┼──────────┼──────────────")
  
       u$ = "\      \│\    \│\                       \│"
      u1$ = "        │      │                         │          │          │              "
      u1x$ = "▀▀▀▀▀▀▀▀│▀▀▀▀▀▀│▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀│▀▀▀▀▀▀▀▀▀▀│▀▀▀▀▀▀▀▀▀▀│▀▀▀▀▀▀▀▀▀▀▀▀▀▀"
      u2$ = "###,###.##"
      u3$ = "###,###,###.##"
      u4$ = "          "
  
      CurrTopline = 1
      EditTransPrintWholeScreen()
  
      CurrRow = 1
      CurrCol = 1
      PrintHelpLine(help$(CurrCol) + "|  <F2=Save and Exit> <F9=Insert> <F10=Delete>")
  
      EditTransGetLine()
  
      finished = [FALSE]
  
  
      'Loop until <F2> is pressed
      Do
          EditTransShowCursor()  'Show Cursor, Wait for key
          Do : Kbd$ = INKEY$() : Loop Until Kbd$ <> ""
          EditTransHideCursor()
  
          If ((Kbd$ >= " ") And (Kbd$ < "~")) Or (Kbd$ = QBChr(8)) Then  'If legal key, edit item
              EditTransEditItem()
          End If
  
          Select Case Kbd$  'Handle Special keys
              Case QBChr(0) + "H"  'up arrow
                  EditTransMoveUp()
              Case QBChr(0) + "P"  'Down arrow
                  EditTransMoveDown()
              Case QBChr(0) + "K", QBChr(0) + QBChr(15)  'Left Arrow,BackTab
                  CurrCol = CShort(((CurrCol + 3) Mod 5) + 1)
                  PrintHelpLine(help$(CurrCol) + "|  <F2=Save and Exit> <F9=Insert> <F10=Delete>")
              Case QBChr(0) + "M", QBChr(9)  'Right Arrow,Tab
                  CurrCol = CShort(((CurrCol) Mod 5) + 1)
                  PrintHelpLine(help$(CurrCol) + "|  <F2=Save and Exit> <F9=Insert> <F10=Delete>")
              Case QBChr(0) + "G"  'Home
                  CurrCol = 1
              Case QBChr(0) + "O"  'End
                  CurrCol = 5
              Case QBChr(0) + "I"  'Page Up
                  CurrRow = 1
                  CurrTopline = CShort(CurrTopline - 19)
                  If CurrTopline < 1 Then
                      CurrTopline = 1
                  End If
                  EditTransPrintWholeScreen()
                  EditTransGetLine()
              Case QBChr(0) + "Q"  'Page Down
                  CurrRow = 1
                  CurrTopline = CShort(CurrTopline + 19)
                  If CurrTopline > MaxRecord Then
                      CurrTopline = MaxRecord
                  End If
                  EditTransPrintWholeScreen()
                  EditTransGetLine()
              Case QBChr(0) + "<"  'F2
                  finished = [TRUE]
              Case QBChr(0) + "C"  'F9
                  EditTransAddRecord()
              Case QBChr(0) + "D"  'F10
                  EditTransDeleteRecord()
              Case QBChr(13)  'Enter
              Case Else
                 BEEP()
          End Select
      Loop Until finished <> 0
  
      CLOSE()
  
      Exit Sub
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  End Sub
  
  
  'FancyCls:
  '  Clears screen in the right color, and draws nice dots.
  Sub FancyCls(ByRef dots As Short, ByRef Background As Short)
    Dim row As Short
    Dim col As Short
  
      VIEW_PRINT(2, 24)
      COLOR(dots, Background)
      CLS(2)
  
      For a = 95 To 1820 Step 45
          row = CShort((a / 80) + 1)
          col = CShort((a Mod 80) + 1)
          LOCATE(row, col)
          PRINT(QBChr(250), True)
      Next a
  
      VIEW_PRINT()
  
  End Sub
  
  
  'GetString$:
  '  Given a row and col, and an initial string, edit a string
  '  VIS is the length of the visible field of entry
  '  MAX is the maximum number of characters allowed in the string
  Function GetString$(ByRef row_Arg As Short, ByRef col_Arg As Short, ByRef start$, ByRef end__Arg$, ByRef Vis_Arg As Short, ByRef Max As Short)
    Dim curr$
    Dim finished As Short
    Dim Kbd$
    Dim row As Short = row_Arg
    Dim col As Short = col_Arg
    Dim end_$ = end__Arg$
    Dim Vis As Short = Vis_Arg
    Dim GetStringShowText As Action
    Dim GetStringGetKey As Action
    GetStringShowText = Sub()
      LOCATE(row, col)
      If Len(curr$) > Vis Then
          PRINT(Right$(curr$, Vis), True)
      Else
          PRINT(curr$ & Space$(Vis - Len(curr$)), True)
          LOCATE(row, col + Len(curr$))
      End If
      Return
    End Sub
    GetStringGetKey = Sub()
      Kbd$ = ""
      While Kbd$ = ""
          Kbd$ = INKEY$()
      End While
      Return
    End Sub
      curr$ = Trim$(Left$(start$, Max))
      If curr$ = QBChr(8) Then curr$ = ""
  
      LOCATE(-1, -1, 1)
  
      finished = [FALSE]
      Do
          GetStringShowText()
          GetStringGetKey()
  
          If Len(Kbd$) > 1 Then
              finished = [TRUE]
              GetString$ = Kbd$
          Else
              Select Case Kbd$
                  Case QBChr(13), QBChr(27), QBChr(9)
                      finished = [TRUE]
                      GetString$ = Kbd$
  
                  Case QBChr(8)
                      If curr$ <> "" Then
                          curr$ = Left$(curr$, Len(curr$) - 1)
                      End If
  
                  Case " " To "}"
                      If Len(curr$) < Max Then
                          curr$ = curr$ + Kbd$
                      Else
                          BEEP()
                      End If
  
                  Case Else
                      BEEP()
              End Select
          End If
  
      Loop Until finished <> 0
  
      end_$ = curr$
      LOCATE(-1, -1, 0)
      row_Arg = row : col_Arg = col : end__Arg$ = end_$ : Vis_Arg = Vis : Exit Function
  
  
  
    row_Arg = row
    col_Arg = col
    end__Arg$ = end_$
    Vis_Arg = Vis
  End Function
  
  
  'Initialize:
  '  Read colors in and set up assembly routines
  Sub Initialize()
    Dim J As Short
    Dim P As Short
  
      WIDTH(-1, 25)
      VIEW_PRINT()
  
      For ColorSet = 1S To 4S
          For X = 1S To 10S
              READ(colors(X, ColorSet))
          Next X
      Next ColorSet
  
      LoadState()
  
      ' TODO(QB2VB): VARPTR (the address of a variable) isn't supported:
      '   P = VARPTR(ScrollUpAsm(1))
      ' TODO(QB2VB): VARSEG (the address of a variable) isn't supported:
      '   DEF SEG = VARSEG(ScrollUpAsm(1))
      For I = 0S To 13S
         READ(J)
         POKE((P + I), J)
      Next I
  
      ' TODO(QB2VB): VARPTR (the address of a variable) isn't supported:
      '   P = VARPTR(ScrollDownAsm(1))
      ' TODO(QB2VB): VARSEG (the address of a variable) isn't supported:
      '   DEF SEG = VARSEG(ScrollDownAsm(1))
      For I = 0S To 13S
         READ(J)
         POKE((P + I), J)
      Next I
  
      DEF_SEG()
  
  End Sub
  
  
  'Intro:
  '  Display introduction screen.
  Sub Intro()
      SCREEN(0)
      WIDTH(80, 25)
      COLOR(7, 0)
      CLS()
  
      Center(4, "Q B a s i c")
      COLOR(15)
      Center(5, "▄     ▄ ▄▄▄▄ ▄   ▄ ▄▄▄▄ ▄   ▄      ▄     ▄ ▄▄▄▄ ▄   ▄ ▄▄▄▄ ▄▄▄▄▄ ▄▄▄▄ ▄▄▄▄▄")
      Center(6, "█▀▄ ▄▀█ █  █ █▄  █ █    █▄▄▄█      █▀▄ ▄▀█ █  █ █▄  █ █  █ █     █    █   █")
      Center(7, "█  ▀  █ █  █ █ ▀▄█ █▀▀▀   █        █  ▀  █ █▀▀█ █ ▀▄█ █▀▀█ █ ▀▀█ █▀▀▀ █▀█▀▀")
      Center(8, "█     █ █▄▄█ █   █ █▄▄▄   █        █     █ █  █ █   █ █  █ █▄▄▄█ █▄▄▄ █  ▀▄")
      COLOR(7)
      Center(11, "A Personal Finance Manager written in")
      Center(12, "MS-DOS QBasic")
      Center(24, "Press any key to continue")
  
      SparklePause()
  End Sub
  
  
  'LCenter:
  '  Center TEXT$ on the line printer
  Sub LCenter(ByRef text$)
      LPRINT(TAB(CInt(41 - (Len(text$) / 2))) & text$)
  End Sub
  
  
  'LoadState:
  '  Load color preferences and account info from MONEY.DAT
  Sub LoadState()
  
      OPEN("money.dat", OpenMode.Input, 1)
      INPUT(1, ColorPref)
  
      For a = 1 To 19
          LINE_INPUT(1, account(a).Title)
          LINE_INPUT(1, account(a).AType)
          LINE_INPUT(1, account(a).Desc)
      Next a
  
      CLOSE()
  
  End Sub
  
  
  'Menu:
  '  Handles Menu Selection for a single menu (either sub menu, or menu bar)
  '  currChoiceX  :  Number of current choice
  '  maxChoice    :  Number of choices in the list
  '  choice$()    :  Array with the text of the choices
  '  itemRow()    :  Array with the row of the choices
  '  itemCol()    :  Array with the col of the choices
  '  help$()      :  Array with the help text for each choice
  '  barMode      :  Boolean:  TRUE = menu bar style, FALSE = drop down style
  '
  '  Returns the number of the choice that was made by changing currChoiceX
  '  and returns the scan code of the key that was pressed to exit
  '
  Function Menu(ByRef CurrChoiceX As Short, ByRef MaxChoice_Arg As Short, ByRef choice_Arg$(), ByRef ItemRow_Arg() As Short, ByRef ItemCol_Arg() As Short, ByRef help_Arg$(), ByRef BarMode_Arg As Short) As Short
    Dim currChoice As Short
    Dim finished As Short
    Dim Kbd$
    Dim MaxChoice As Short = MaxChoice_Arg
    Dim choice$() = choice_Arg$
    Dim ItemRow() As Short = ItemRow_Arg
    Dim ItemCol() As Short = ItemCol_Arg
    Dim help$() = help_Arg$
    Dim BarMode As Short = BarMode_Arg
    Dim MenuShowCursor As Action
    Dim MenuGetKey As Action
    Dim MenuHideCursor As Action
    Dim MenuUp As Action
    Dim MenuDown As Action
    Dim MenuLeft As Action
    Dim MenuRight As Action
    Dim MenuEnter As Action
    Dim MenuEscape As Action
    MenuEnter = Sub()
      finished = [TRUE]
      Return
    End Sub
    MenuEscape = Sub()
      currChoice = 0
      finished = [TRUE]
      Return
    End Sub
    MenuUp = Sub()
      If BarMode <> 0 Then
          BEEP()
      Else
          currChoice = CShort((((currChoice + MaxChoice) - 2) Mod MaxChoice) + 1)
      End If
      Return
    End Sub
    MenuLeft = Sub()
      If BarMode <> 0 Then
          currChoice = CShort((((currChoice + MaxChoice) - 2) Mod MaxChoice) + 1)
      Else
          currChoice = -2
          finished = [TRUE]
      End If
      Return
    End Sub
    MenuRight = Sub()
      If BarMode <> 0 Then
          currChoice = CShort(((currChoice) Mod MaxChoice) + 1)
      Else
          currChoice = -3
          finished = [TRUE]
      End If
      Return
    End Sub
    MenuDown = Sub()
      If BarMode <> 0 Then
          finished = [TRUE]
      Else
          currChoice = CShort(((currChoice) Mod MaxChoice) + 1)
      End If
      Return
    End Sub
    MenuShowCursor = Sub()
      COLOR(colors(8, ColorPref), colors(9, ColorPref))
      LOCATE(ItemRow(currChoice), ItemCol(currChoice))
      PRINT(choice$(currChoice), True)
      PrintHelpLine(help$(currChoice))
      Return
    End Sub
    MenuGetKey = Sub()
      Kbd$ = ""
      While Kbd$ = ""
          Kbd$ = INKEY$()
      End While
      Return
    End Sub
    MenuHideCursor = Sub()
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      LOCATE(ItemRow(currChoice), ItemCol(currChoice))
      PRINT(choice$(currChoice), True)
      Return
    End Sub
  
      currChoice = CurrChoiceX
  
      'if in bar mode, color in menu bar, else color box/shadow
      'bar mode means you are currently in the menu bar, not a sub menu
      If BarMode <> 0 Then
          COLOR(colors(7, ColorPref), colors(4, ColorPref))
          LOCATE(1, 1)
          PRINT(Space$(80), True)
      Else
          FancyCls(colors(2, ColorPref), colors(1, ColorPref))
          COLOR(colors(7, ColorPref), colors(4, ColorPref))
          Box(CShort(ItemRow(1) - 1), CShort(ItemCol(1) - 1), CShort(ItemRow(MaxChoice) + 1), CShort((ItemCol(1) + Len(choice$(1))) + 1))
  
          COLOR(colors(10, ColorPref), colors(6, ColorPref))
          For a = 1 To CShort(MaxChoice + 1)
              LOCATE((ItemRow(1) + a) - 1, (ItemCol(1) + Len(choice$(1))) + 2)
              PRINT(QBChr(178) & QBChr(178), True)
          Next a
          LOCATE(ItemRow(MaxChoice) + 2, ItemCol(MaxChoice) + 2)
          PRINT(QBString(Len(choice$(MaxChoice)) + 2, 178), True)
      End If
  
      'print the choices
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      For a = 1 To MaxChoice
          LOCATE(ItemRow(a), ItemCol(a))
          PRINT(choice$(a), True)
      Next a
  
      finished = [FALSE]
  
      While (Not finished) <> 0
  
          MenuShowCursor()
          MenuGetKey()
          MenuHideCursor()
  
          Select Case Kbd$
              Case QBChr(0) + "H" : MenuUp()
              Case QBChr(0) + "P" : MenuDown()
              Case QBChr(0) + "K" : MenuLeft()
              Case QBChr(0) + "M" : MenuRight()
              Case QBChr(13) : MenuEnter()
              Case QBChr(27) : MenuEscape()
              Case Else : BEEP()
          End Select
      End While
  
      Menu = currChoice
  
      MaxChoice_Arg = MaxChoice : choice_Arg$ = choice$ : ItemRow_Arg = ItemRow : ItemCol_Arg = ItemCol : help_Arg$ = help$ : BarMode_Arg = BarMode : Exit Function
  
  
  
  
  
  
  
  
  
  
  
  
    MaxChoice_Arg = MaxChoice
    choice_Arg$ = choice$
    ItemRow_Arg = ItemRow
    ItemCol_Arg = ItemCol
    help_Arg$ = help$
    BarMode_Arg = BarMode
  End Function
  
  
  'MenuSystem:
  '  Main routine that controls the program.  Uses the MENU function
  '  to implement menu system and calls the appropriate function to handle
  '  the user's selection
  Sub MenuSystem()
    Dim choice$()
    Dim menuRow() As Short
    Dim menuCol() As Short
    Dim help$()
    Dim choice_Int As Short
    Dim finished As Short
    Dim subchoice As Short
    Dim NewChoice As Short
    Dim MenuSystemMain As Action
    Dim MenuSystemFile As Action
    Dim MenuSystemEdit As Action
    Dim MenuSystemAccount As Action
    Dim MenuSystemReport As Action
    Dim MenuSystemColors As Action
    MenuSystemMain = Sub()
      FancyCls(colors(2, ColorPref), colors(1, ColorPref))
      COLOR(colors(7, ColorPref), colors(4, ColorPref))
      Box(9, 19, 14, 61)
      Center(11, "Use arrow keys to navigate menu system")
      Center(12, "Press Enter to select a menu item")
  
      choice$(1) = " File "
      choice$(2) = " Accounts "
      choice$(3) = " Transactions "
      choice$(4) = " Reports "
      choice$(5) = " Colors "
  
      menuRow(1) = 1 : menuCol(1) = 2
      menuRow(2) = 1 : menuCol(2) = 8
      menuRow(3) = 1 : menuCol(3) = 18
      menuRow(4) = 1 : menuCol(4) = 32
      menuRow(5) = 1 : menuCol(5) = 41
  
      help$(1) = "Exit the Money Manager"
      help$(2) = "Add/edit/delete accounts"
      help$(3) = "Add/edit/delete account transactions"
      help$(4) = "View and print reports"
      help$(5) = "Set screen colors"
  
      Do
          NewChoice = Menu((choice_Int), 5, choice$, menuRow, menuCol, help$, [TRUE])
      Loop While NewChoice = 0
      choice_Int = NewChoice
      Return
    End Sub
    MenuSystemFile = Sub()
      choice$(1) = " Exit           "
  
      menuRow(1) = 3 : menuCol(1) = 2
  
      help$(1) = "Exit the Money Manager"
  
      subchoice = Menu(1, 1, choice$, menuRow, menuCol, help$, [FALSE])
  
      Select Case subchoice
          Case 1 : finished = [TRUE]
          Case Else
      End Select
      Return
    End Sub
    MenuSystemEdit = Sub()
      choice$(1) = " Edit Account Titles "
  
      menuRow(1) = 3 : menuCol(1) = 8
  
      help$(1) = "Add/edit/delete accounts"
  
      subchoice = Menu(1, 1, choice$, menuRow, menuCol, help$, [FALSE])
  
      Select Case subchoice
          Case 1 : EditAccounts()
          Case Else
      End Select
      Return
    End Sub
    MenuSystemAccount = Sub()
  
      For a = 1 To 19
          If Trim$(account(a).Title) = "" Then
              choice$(a) = Right$(Str$(a), 2) + ". ------------------- "
          Else
              choice$(a) = (Right$(Str$(a), 2) + ". ") + account(a).Title
          End If
          menuRow(a) = CShort(a + 2)
          menuCol(a) = 19
          help$(a) = RTrim$(account(a).Desc)
      Next a
  
      subchoice = Menu(1, 19, choice$, menuRow, menuCol, help$, [FALSE])
  
      If subchoice > 0 Then
          EditTrans((subchoice))
      End If
      Return
    End Sub
    MenuSystemReport = Sub()
      choice$(1) = " Net Worth Report       "
      menuRow(1) = 3 : menuCol(1) = 32
      help$(1) = "View and print net worth report"
  
      For a = 1 To 19
          If Trim$(account(a).Title) = "" Then
              choice$(a + 1) = Right$(Str$(a), 2) + ". ------------------- "
          Else
              choice$(a + 1) = (Right$(Str$(a), 2) + ". ") + account(a).Title
          End If
          menuRow(a + 1) = CShort(a + 3)
          menuCol(a + 1) = 32
          help$(a + 1) = ("Print " + RTrim$(account(a).Title)) + " transaction summary"
      Next a
  
      subchoice = Menu(1, 20, choice$, menuRow, menuCol, help$, [FALSE])
  
      Select Case subchoice
          Case 1
              NetWorthReport()
          Case 2 To 20
              TransactionSummary(CShort((subchoice - 1)))
          Case Else
      End Select
      Return
    End Sub
    MenuSystemColors = Sub()
      choice$(1) = " Monochrome Scheme "
      choice$(2) = " Cyan/Blue Scheme  "
      choice$(3) = " Blue/Cyan Scheme  "
      choice$(4) = " Red/Grey Scheme   "
  
      menuRow(1) = 3 : menuCol(1) = 41
      menuRow(2) = 4 : menuCol(2) = 41
      menuRow(3) = 5 : menuCol(3) = 41
      menuRow(4) = 6 : menuCol(4) = 41
  
      help$(1) = "Color scheme for monochrome and LCD displays"
      help$(2) = "Color scheme featuring cyan"
      help$(3) = "Color scheme featuring blue"
      help$(4) = "Color scheme featuring red"
  
      subchoice = Menu(1, 4, choice$, menuRow, menuCol, help$, [FALSE])
  
      Select Case subchoice
          Case 1 To 4
              ColorPref = subchoice
              SaveState()
          Case Else
      End Select
      Return
    End Sub
  
      ReDim choice$(20), menuRow(20), menuCol(20), help$(20)
      LOCATE(-1, -1, 0)
      choice_Int = 1
      finished = [FALSE]
  
      While (Not finished) <> 0
          MenuSystemMain()
  
          subchoice = -1
          While subchoice < 0
              Select Case choice_Int
                  Case 1 : MenuSystemFile()
                  Case 2 : MenuSystemEdit()
                  Case 3 : MenuSystemAccount()
                  Case 4 : MenuSystemReport()
                  Case 5 : MenuSystemColors()
              End Select
              FancyCls(colors(2, ColorPref), colors(1, ColorPref))
  
              Select Case subchoice
                  Case -2 : choice_Int = CShort(((choice_Int + 3) Mod 5) + 1)
                  Case -3 : choice_Int = CShort(((choice_Int) Mod 5) + 1)
              End Select
          End While
      End While
      Exit Sub
  
  
  
  
  
  
  
  
  
  
  
  
  End Sub
  
  
  'NetWorthReport:
  '  Prints net worth report to screen and printer
  Sub NetWorthReport()
    Dim assetIndex() As Short
    Dim liabilityIndex() As Short
    Dim Fields1 As FieldRecord
    Dim Fields2 As FieldRecord
    Dim Fields3 As FieldRecord
    Dim Fields4 As FieldRecord
    Dim maxAsset As Short
    Dim maxLiability As Short
    Dim finished As Short
    Dim u1$
    Dim u2$
    Dim assetTotal#
    Dim count1 As Short
    Dim file_$
    Dim liabilityTotal#
    Dim count2 As Short
    Dim Kbd$
    Dim u0$
    Dim u3$
    Dim u4$
      ReDim assetIndex(19), liabilityIndex(19)
  
      maxAsset = 0
      maxLiability = 0
  
      For a = 1 To 19
          If account(a).AType = "A" Then
              maxAsset = CShort(maxAsset + 1)
              assetIndex(maxAsset) = a
          ElseIf account(a).AType = "L" Then
              maxLiability = CShort(maxLiability + 1)
              liabilityIndex(maxLiability) = a
          End If
      Next a
  
      'Loop until <F2> is pressed
      finished = [FALSE]
      Do
          u1$ = "\                  \$$###,###,###.##"
          u2$ = "\               \+$$#,###,###,###.##"
  
          COLOR(colors(5, ColorPref), colors(4, ColorPref))
          LOCATE(1, 1) : PRINT(Space$(80), True)
          LOCATE(1, 4) : PRINT(("Net Worth Report: " + DateString()), True)
          PrintHelpLine("<F2=Exit>    <F3=Print Report>")
  
          COLOR(colors(7, ColorPref), colors(4, ColorPref))
          Box(2, 1, 24, 40)
          Box(2, 41, 24, 80)
  
          LOCATE(2, 16) : PRINT(" ASSETS ")
          assetTotal# = 0
          a = 1
          count1 = 1
          While a <= maxAsset
              file_$ = "money." + Cvit$(assetIndex(a))
              OPEN(file_$, OpenMode.Random, 1, 84)
              Fields1 = FIELD(1, 11, 5, 8)
              GET_FIELD(1, 1)
              If Fields4(0) = "THISISVALID" Then
                  LOCATE(2 + count1, 3) : PRINT_USING(u1$, account(assetIndex(a)).Title, CVD(Fields4(2)))
                  assetTotal# = assetTotal# + CVD(Fields4(2))
                  count1 = CShort(count1 + 1)
              End If
              CLOSE()
              a = CShort(a + 1)
          End While
  
          LOCATE(2, 55) : PRINT(" LIABILITIES ")
          liabilityTotal# = 0
          a = 1
          count2 = 1
          While a <= maxLiability
              file_$ = "money." + Cvit$(liabilityIndex(a))
              OPEN(file_$, OpenMode.Random, 1, 84)
              Fields2 = FIELD(1, 11, 5, 8)
              GET_FIELD(1, 1)
              If Fields4(0) = "THISISVALID" Then
                  LOCATE(2 + count2, 43) : PRINT_USING(u1$, account(liabilityIndex(a)).Title, CVD(Fields4(2)))
                  liabilityTotal# = liabilityTotal# + CVD(Fields4(2))
                  count2 = CShort(count2 + 1)
              End If
              CLOSE()
              a = CShort(a + 1)
          End While
          If count2 > count1 Then count1 = count2
          LOCATE(2 + count1, 25) : PRINT("--------------")
          LOCATE(2 + count1, 65) : PRINT("--------------")
          LOCATE(3 + count1, 3) : PRINT_USING(u2$, "Total assets", assetTotal#)
          LOCATE(3 + count1, 43) : PRINT_USING(u2$, "Total liabilities", liabilityTotal#)
  
          COLOR(colors(5, ColorPref), colors(4, ColorPref))
          LOCATE(1, 43) : PRINT_USING(u2$, "    NET WORTH:", assetTotal# - liabilityTotal#)
  
          Do : Kbd$ = INKEY$() : Loop Until Kbd$ <> ""
  
          Select Case Kbd$  'Handle Special keys
              Case QBChr(0) + "<"  'F2
                  finished = [TRUE]
              Case QBChr(0) + "="  'F3
                 ' GOSUB NetWorthReportPrint: the routine, expanded
      PrintHelpLine("")
  
      Box(8, 20, 14, 62)
      Center(10, "Prepare printer on LPT1 for report")
      Center(12, "Hit <Enter> to print, or <Esc> to abort")
  
      Do : Kbd$ = INKEY$() : Loop While (Kbd$ <> QBChr(13)) And (Kbd$ <> QBChr(27))
  
      If Kbd$ = QBChr(13) Then
          Box(8, 20, 14, 62)
          Center(11, "Printing report...")
          u0$ = "                     \                  \ "
          u1$ = "                        \                 \ $$###,###,###.##"
          u2$ = "                                              --------------"
          u3$ = "                                               ============="
          u4$ = "                        \               \+$$#,###,###,###.##"
          PrintErr = [FALSE]
          On Error GoTo ErrorTrap  ' test if printer is connected
          LPRINT()
          If PrintErr = [FALSE] Then
              LPRINT() : LPRINT() : LPRINT() : LPRINT() : LPRINT()
              LCenter("Q B a s i c")
              LCenter("M O N E Y   M A N A G E R")
              LPRINT() : LPRINT()
              LCenter("NET WORTH REPORT:  " + DateString())
              LCenter("-------------------------------------------")
              LPRINT_USING(u0$, "ASSETS:")
              assetTotal# = 0
              a = 1
              While a <= maxAsset
                  file_$ = "money." + Cvit$(assetIndex(a))
                  OPEN(file_$, OpenMode.Random, 1, 84)
                  Fields3 = FIELD(1, 11, 5, 8)
                  GET_FIELD(1, 1)
                  If Fields4(0) = "THISISVALID" Then
                      LPRINT_USING(u1$, account(assetIndex(a)).Title, CVD(Fields4(2)))
                      assetTotal# = assetTotal# + CVD(Fields4(2))
                  End If
                  CLOSE(1)
                  a = CShort(a + 1)
              End While
              LPRINT(u2$)
              LPRINT_USING(u4$, "Total assets", assetTotal#)
              LPRINT()
              LPRINT()
              LPRINT_USING(u0$, "LIABILITIES:")
              liabilityTotal# = 0
              a = 1
              While a <= maxLiability
                  file_$ = "money." + Cvit$(liabilityIndex(a))
                  OPEN(file_$, OpenMode.Random, 1, 84)
                  Fields4 = FIELD(1, 11, 5, 8)
                  GET_FIELD(1, 1)
                  If Fields4(0) = "THISISVALID" Then
                      LPRINT_USING(u1$, account(liabilityIndex(a)).Title, CVD(Fields4(2)))
                      liabilityTotal# = liabilityTotal# + CVD(Fields4(2))
                  End If
                  CLOSE(1)
                  a = CShort(a + 1)
              End While
              LPRINT(u2$)
              LPRINT_USING(u4$, "Total liabilities", liabilityTotal#)
              LPRINT()
  
              LPRINT()
              LPRINT(u3$)
              LPRINT_USING(u4$, "NET WORTH", assetTotal# - liabilityTotal#)
              LCenter("-------------------------------------------")
              LPRINT() : LPRINT() : LPRINT()
          End If
          On Error GoTo 0
      End If
              Case Else
                 BEEP()
          End Select
      Loop Until finished <> 0
      Exit Sub
  
  ' NetWorthReportPrint: (this routine is expanded where it is called, because it uses ON ERROR)
    Exit Sub
  ErrorTrap:
      Select Case Err.Number
          ' If data file not found, create and initialize a new one.
          Case 53
              CLOSE()
              ColorPref = 1
              For a = 1 To 19
                  account(a).Title = ""
                  account(a).AType = ""
                  account(a).Desc = ""
              Next a
              SaveState()
              Resume
          Case 24, 25
              PrintErr = [TRUE]
              Box(8, 13, 14, 69)
              Center(11, "Printer not responding ... Press Space to continue")
              While INKEY$() <> "" : End While
              While INKEY$() <> " " : End While
              Resume Next
          Case Else
      End Select
      Resume Next
  End Sub
  
  
  'PrintHelpLine:
  '  Prints help text on the bottom row in the proper color
  Sub PrintHelpLine(ByRef help$)
      COLOR(colors(5, ColorPref), colors(4, ColorPref))
      LOCATE(25, 1)
      PRINT(Space$(80), True)
      Center(25, help$)
  End Sub
  
  
  'SaveState:
  '  Save color preference and account information to "MONEY.DAT" data file.
  Sub SaveState()
      OPEN("money.dat", OpenMode.Output, 2)
      PRINT(2, Str$(ColorPref) & " ")
  
      For a = 1 To 19
          PRINT(2, account(a).Title)
          PRINT(2, account(a).AType)
          PRINT(2, account(a).Desc)
      Next a
  
      CLOSE(2)
  End Sub
  
  
  'ScrollDown:
  '  Call the assembly program to scroll the screen down
  Sub ScrollDown()
      ' TODO(QB2VB): VARSEG (the address of a variable) isn't supported:
      '   DEF SEG = VARSEG(ScrollDownAsm(1))
      ' TODO(QB2VB): unknown procedure Absolute:
      '   CALL Absolute(VARPTR(ScrollDownAsm(1)))
      DEF_SEG()
  End Sub
  
  
  'ScrollUp:
  '  Calls the assembly program to scroll the screen up
  Sub ScrollUp()
      ' TODO(QB2VB): VARSEG (the address of a variable) isn't supported:
      '   DEF SEG = VARSEG(ScrollUpAsm(1))
      ' TODO(QB2VB): unknown procedure Absolute:
      '   CALL Absolute(VARPTR(ScrollUpAsm(1)))
      DEF_SEG()
  End Sub
  
  
  'SparklePause:
  '  Creates flashing border for intro screen
  Sub SparklePause()
    Dim c As Short
  
      COLOR(4, 0)
      Dim a_Str$ = "*    *    *    *    *    *    *    *    *    *    *    *    *    *    *    *    *    "
      While INKEY$() <> "" : End While  'Clear keyboard buffer
  
      While INKEY$() = ""
          For a = 1 To 5
              LOCATE(1, 1)  'print horizontal sparkles
              PRINT(Mid$(a_Str$, a, 80), True)
              LOCATE(22, 1)
              PRINT(Mid$(a_Str$, 6 - a, 80), True)
  
              For b = 2S To 21S  'Print Vertical sparkles
                  c = CShort((a + b) Mod 5)
                  If c = 1 Then
                      LOCATE(b, 80)
                      PRINT("*", True)
                      LOCATE(23 - b, 1)
                      PRINT("*", True)
                  Else
                      LOCATE(b, 80)
                      PRINT(" ", True)
                      LOCATE(23 - b, 1)
                      PRINT(" ", True)
                  End If
              Next b
          Next a
      End While
  End Sub
  
  
  'TransactionSummary:
  '  Print transaction summary to line printer
  Sub TransactionSummary(ByRef item As Short)
    Dim Fields1 As FieldRecord
    Dim Fields2 As FieldRecord
    Dim Kbd$
    Dim u5$
    Dim u0$
    Dim u2$
    Dim u3$
    Dim u4$
    Dim file_$
    Dim Balance#
    Dim MaxRecord As Short
    Dim CurrRecord As Short
    Dim Fig1#
    Dim Fig2#
      FancyCls(colors(2, ColorPref), colors(1, ColorPref))
      PrintHelpLine("")
      Box(8, 20, 14, 62)
      Center(10, "Prepare printer on LPT1 for report")
      Center(12, "Hit <Enter> to print, or <Esc> to abort")
  
      Do : Kbd$ = INKEY$() : Loop While (Kbd$ <> QBChr(13)) And (Kbd$ <> QBChr(27))
  
      If Kbd$ = QBChr(13) Then
          Box(8, 20, 14, 62)
          Center(11, "Printing report...")
          PrintErr = [FALSE]
          On Error GoTo ErrorTrap  ' test if printer is connected
          LPRINT()
          If PrintErr = [FALSE] Then
              PRINT()
              LPRINT() : LPRINT() : LPRINT() : LPRINT() : LPRINT()
              LCenter("Q B a s i c")
              LCenter("M O N E Y   M A N A G E R")
              LPRINT() : LPRINT()
              LCenter("Transaction summary: " + Trim$(account(item).Title))
              LCenter(DateString())
              LPRINT()
              u5$ = "--------|------|------------------------|----------|----------|--------------"
              LPRINT(u5$)
              LPRINT("  Date  | Ref# | Description            | Increase | Decrease |  Balance   ")
              LPRINT(u5$)
               u0$ = "\      \|\    \|\                      \|"
              u2$ = "###,###.##"
              u3$ = "###,###,###.##"
              u4$ = "          "
  
              file_$ = "money." + Cvit$(item)
              OPEN(file_$, OpenMode.Random, 1, 84)
              Fields1 = FIELD(1, 8, 10, 50, 8, 8)
              Fields2 = FIELD(1, 11, 5, 8)
              GET_FIELD(1, 1)
              If Fields2(0) = "THISISVALID" Then
                  Balance# = 0
                  MaxRecord = CShort(Val(Fields2(1)))
                  CurrRecord = 1
                  While CurrRecord <= MaxRecord
  
                      GET_FIELD(1, CurrRecord + 1)
                      Fig1# = CVD(Fields1(3))
                      Fig2# = CVD(Fields1(4))
  
                      LPRINT_USING(u0$, Fields1(0), Fields1(1), Fields1(2))
                      If (Fig2# = 0) And (Fig1# = 0) Then
                          LPRINT_USING((((u4$ + "|") + u4$) + "|") + u3$, Balance#)
                      ElseIf Fig2# = 0 Then
                          Balance# = Balance# + Fig1#
                          LPRINT_USING((((u2$ + "|") + u4$) + "|") + u3$, Fig1#, Balance#)
                      Else
                          Balance# = Balance# - Fig2#
                          LPRINT_USING((((u4$ + "|") + u2$) + "|") + u3$, Fig2#, Balance#)
                      End If
                      CurrRecord = CShort(CurrRecord + 1)
                  End While
                  LPRINT(u5$)
                  LPRINT() : LPRINT()
              End If
              On Error GoTo 0
          End If
          CLOSE()
      End If
    Exit Sub
  ErrorTrap:
      Select Case Err.Number
          ' If data file not found, create and initialize a new one.
          Case 53
              CLOSE()
              ColorPref = 1
              For a = 1 To 19
                  account(a).Title = ""
                  account(a).AType = ""
                  account(a).Desc = ""
              Next a
              SaveState()
              Resume
          Case 24, 25
              PrintErr = [TRUE]
              Box(8, 13, 14, 69)
              Center(11, "Printer not responding ... Press Space to continue")
              While INKEY$() <> "" : End While
              While INKEY$() <> " " : End While
              Resume Next
          Case Else
      End Select
      Resume Next
  End Sub
  
  
  'Trin$:
  '  Remove null and spaces from the end of a string.
  Function Trim$(ByRef X$)
    Dim lastChar As Short
    Dim y$
  
      If X$ = "" Then
          Trim$ = ""
      Else
          lastChar = 0
          For a = 1 To CShort(Len(X$))
              y$ = Mid$(X$, a, 1)
              If (y$ <> QBChr(0)) And (y$ <> " ") Then
                  lastChar = a
              End If
          Next a
          Trim$ = Left$(X$, lastChar)
      End If
  
  End Function
  
End Module
