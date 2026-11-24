' Converted from QBasic by qb2vb.

Module Program

  Const [FALSE] As Short = 0
  Const [TRUE] As Short = Not [FALSE]
  Structure StockItem
    Implements IFileRecord
    Private m_PartNumber As String
    Public Property PartNumber As String
      Get
        Return If(m_PartNumber, FixedString.Blank(6))
      End Get
      Set(value As String)
        m_PartNumber = FixedString.Fit(value, 6)
      End Set
    End Property
    Private m_Description As String
    Public Property Description As String
      Get
        Return If(m_Description, FixedString.Blank(20))
      End Get
      Set(value As String)
        m_Description = FixedString.Fit(value, 20)
      End Set
    End Property
    Public UnitPrice As Single
    Public Quantity As Short
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(PartNumber, 6)
      writer.WriteString(Description, 20)
      writer.Write(UnitPrice)
      writer.Write(Quantity)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      PartNumber = reader.ReadString(6)
      Description = reader.ReadString(20)
      UnitPrice = reader.ReadSingle()
      Quantity = reader.ReadInt16()
    End Sub
  End Structure
  
  Structure IndexType
    Implements IFileRecord
    Public RecordNumber As Short
    Private m_PartNumber As String
    Public Property PartNumber As String
      Get
        Return If(m_PartNumber, FixedString.Blank(6))
      End Get
      Set(value As String)
        m_PartNumber = FixedString.Fit(value, 6)
      End Set
    End Property
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.Write(RecordNumber)
      writer.WriteString(PartNumber, 6)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      RecordNumber = reader.ReadInt16()
      PartNumber = reader.ReadString(6)
    End Sub
  End Structure
  
  Const NUMCOLS As Short = 8
  Const COLWIDTH As Short = CShort(80 \ NUMCOLS)
  Dim Index(100) As IndexType
  Dim NumberOfRecords As Short

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim RecordNumber As Short
    Dim Choice$ = ""
    Dim Pause$ = ""
    ' DEFINT A-Z
    
    ' Define the symbolic constants used globally in the program:
    ' CONST FALSE = 0, TRUE = NOT FALSE
    
    ' Define a record structure for random-file records:
    ' TYPE StockItem (see the Structure at the top)
    
    ' Define a record structure for each element of the index:
    ' TYPE IndexType (see the Structure at the top)
    
    ' Declare procedures that will be called:
    ' DECLARE FUNCTION Filter$ (Prompt$)
    ' DECLARE FUNCTION FindRecord% (PartNumber$, RecordVar AS StockItem)
    
    ' DECLARE SUB AddRecord (RecordVar AS StockItem)
    ' DECLARE SUB InputRecord (RecordVar AS StockItem)
    ' DECLARE SUB PrintRecord (RecordVar AS StockItem)
    ' DECLARE SUB SortIndex ()
    ' DECLARE SUB ShowPartNumbers ()
    
    ' Define a buffer (using the StockItem type) and
    ' define & dimension the index array:
    Dim StockRecord As StockItem
    
    ' Open the random-access file:
    OPEN("STOCK.DAT", OpenMode.Random, 1, Len(StockRecord))
    
    ' Calculate number of records in the file:
    NumberOfRecords = CShort(CInt(LOF(1)) \ Len(StockRecord))
    
    ' If there are records, read them and build the index:
    If NumberOfRecords <> 0 Then
       For RecordNumber = 1 To NumberOfRecords
    
          ' Read the data from a new record in the file:
          [GET](1, RecordNumber, StockRecord)
    
          ' Place part number and record number in index:
          Index(RecordNumber).RecordNumber = RecordNumber
          Index(RecordNumber).PartNumber = StockRecord.PartNumber
       Next
    
       SortIndex()  ' Sort index in part-number order.
    End If
    
    Do  ' Main-menu loop.
       CLS()
       PRINT("(A)dd records.")
       PRINT("(L)ook up records.")
       PRINT("(Q)uit program.")
       PRINT()
       LOCATE(-1, -1, 1)
       PRINT("Type your choice (A, L, or Q) here: ", True)
    
       ' Loop until user presses, A, L, or Q:
       Do
          Choice$ = UCase$(InputStr(1))
       Loop While InStr("ALQ", Choice$) = 0
    
       ' Branch according to choice:
       Select Case Choice$
          Case "A"
             AddRecord(StockRecord)
          Case "L"
             If NumberOfRecords = 0 Then
                PRINT() : PRINT("No records in file yet. ", True)
                PRINT("Press any key to continue.", True)
                Pause$ = InputStr(1)
             Else
                InputRecord(StockRecord)
             End If
          Case "Q"  ' End program
       End Select
    Loop Until Choice$ = "Q"
    
    CLOSE(1)  ' All done, close file and end.
    End
  End Sub

  '
  ' ======================== ADDRECORD =========================
  '  Adds records to the file from input typed at the keyboard.
  ' ============================================================
  '
  Sub AddRecord(ByRef RecordVar As StockItem)
    Static OK$ = ""
     ' SHARED Index() AS IndexType, NumberOfRecords   (shared variables are declared at module level)
     Do
        CLS()
        INPUT("Part Number: ", RecordVar.PartNumber, False)
        INPUT("Description: ", RecordVar.Description, False)
        RecordVar.UnitPrice = CSng(Val(Filter$("Unit Price : ")))
        RecordVar.Quantity = CShort(Val(Filter$("Quantity   : ")))
  
        NumberOfRecords = CShort(NumberOfRecords + 1)
  
        PUT(1, NumberOfRecords, RecordVar)
  
        Index(NumberOfRecords).RecordNumber = NumberOfRecords
        Index(NumberOfRecords).PartNumber = RecordVar.PartNumber
        PRINT() : PRINT("Add another? ", True)
        OK$ = UCase$(InputStr(1))
     Loop While OK$ = "Y"
  
     SortIndex()  ' Re-sort index file.
  End Sub
  
  '
  ' ========================= FILTER ===========================
  '       Filters all non-numeric characters from a string
  '       and returns the filtered string.
  ' ============================================================
  '
  Function Filter$(ByRef Prompt$)
    Static ValTemp2$ = ""
    Static ValTemp1$ = ""
    Static StringLength As Short
    Static I As Short
    Static Char_$ = ""
     ValTemp2$ = ""
     PRINT(Prompt$, True)  ' Print the prompt passed.
     INPUT("", ValTemp1$, False)  ' Input a number as
                                       ' a string.
     StringLength = CShort(Len(ValTemp1$))  ' Get the string's length.
     For I = 1 To StringLength  ' Go through the string,
        Char_$ = Mid$(ValTemp1$, I, 1)  ' one character at a time.
  
        ' Is the character a valid part of a number (i.e.,
        ' a digit or a decimal point)?  If yes, add it to
        ' the end of a new string:
        If InStr(".0123456789", Char_$) > 0 Then
           ValTemp2$ = ValTemp2$ + Char_$
  
        ' Otherwise, check to see if it's a lowercase "l",
        ' since users used to typewriters may enter a one
        ' value that way:
        ElseIf Char_$ = "l" Then
           ValTemp2$ = ValTemp2$ + "1"  ' Change the "l" to a "1".
        End If
     Next I
  
     Filter$ = ValTemp2$  ' Return filtered string.
  
  End Function
  
  '
  ' ======================= FINDRECORD =========================
  '     Uses a binary search to locate a record in the index.
  ' ============================================================
  '
  Function FindRecord(ByRef Part$, ByRef RecordVar As StockItem) As Short
    Static TopRecord As Short
    Static BottomRecord As Short
    Static Midpoint As Short
    Static Test$ = ""
     ' SHARED Index() AS IndexType, NumberOfRecords   (shared variables are declared at module level)
  
     ' Set top and bottom bounds of search:
     TopRecord = NumberOfRecords
     BottomRecord = 1
  
     ' Search until top of range is less than bottom:
     Do Until (TopRecord < BottomRecord)
  
        ' Choose midpoint:
        Midpoint = CShort((TopRecord + BottomRecord) \ 2)
  
        ' Test to see if it's the one wanted (RTRIM$() trims
        ' trailing blanks from a fixed string):
        Test$ = RTrim$(Index(Midpoint).PartNumber)
  
        ' If it is, exit loop:
        If Test$ = Part$ Then
           Exit Do
  
        ' Otherwise, if what we're looking for is greater,
        ' move bottom up:
        ElseIf Part$ > Test$ Then
           BottomRecord = CShort(Midpoint + 1)
  
        ' Otherwise, move the top down:
        Else
           TopRecord = CShort(Midpoint - 1)
        End If
     Loop
  
     ' If part was found, get record from file using
     ' pointer in index and set FindRecord% to TRUE:
     If Test$ = Part$ Then
        [GET](1, Index(Midpoint).RecordNumber, RecordVar)
        FindRecord = [TRUE]
  
     ' Otherwise, if part was not found, set FindRecord%
     ' to FALSE:
     Else
        FindRecord = [FALSE]
     End If
  End Function
  
  '
  ' ======================= INPUTRECORD ========================
  '    First, INPUTRECORD calls SHOWPARTNUMBERS, which
  '    prints a menu of part numbers on the top of the screen.
  '    Next, INPUTRECORD prompts the user to enter a part
  '    number. Finally, it calls the FINDRECORD and PRINTRECORD
  '    procedures to find and print the given record.
  ' ============================================================
  '
  Sub InputRecord(ByRef RecordVar As StockItem)
    Static Part$ = ""
     CLS()
     ShowPartNumbers()  ' Call the ShowPartNumbers SUB.
  
     ' Print data from specified records on the bottom
     ' part of the screen:
     Do
        PRINT("Type a part number listed above ", True)
        INPUT("(or Q to quit) and press <ENTER>: ", Part$, False)
        If UCase$(Part$) <> "Q" Then
           If FindRecord(Part$, RecordVar) <> 0 Then
              PrintRecord(RecordVar)
           Else
              PRINT("Part not found.")
           End If
        End If
        PRINT(QBString(40, "_"))
     Loop While UCase$(Part$) <> "Q"
  
     VIEW_PRINT()  ' Restore the text viewport to entire screen.
  End Sub
  
  '
  ' ======================= PRINTRECORD ========================
  '                Prints a record on the screen
  ' ============================================================
  '
  Sub PrintRecord(ByRef RecordVar As StockItem)
     PRINT("Part Number: " & RecordVar.PartNumber)
     PRINT("Description: " & RecordVar.Description)
     PRINT_USING("Unit Price :$$###.##", RecordVar.UnitPrice)
     PRINT("Quantity   :" & Str$(RecordVar.Quantity) & " ")
  End Sub
  
  '
  ' ===================== SHOWPARTNUMBERS ======================
  '  Prints an index of all the part numbers in the upper part
  '  of the screen.
  ' ============================================================
  '
  Sub ShowPartNumbers()
    Static ColumnLength As Short
    Static Column As Short
    Static RecordNumber As Short
    Static Row As Short
     ' SHARED Index() AS IndexType, NumberOfRecords   (shared variables are declared at module level)
  
     ' CONST NUMCOLS = 8, COLWIDTH = 80 \ NUMCOLS
  
     ' At the top of the screen, print a menu indexing all
     ' the part numbers for records in the file.  This menu is
     ' printed in columns of equal length (except possibly the
     ' last column, which may be shorter than the others):
     ColumnLength = NumberOfRecords
     Do While (ColumnLength Mod NUMCOLS) <> 0
        ColumnLength = CShort(ColumnLength + 1)
     Loop
     ColumnLength = ColumnLength \ NUMCOLS
     Column = 1
     RecordNumber = 1
     Do Until RecordNumber > NumberOfRecords
        For Row = 1 To ColumnLength
           LOCATE(Row, Column)
           PRINT(Index(RecordNumber).PartNumber)
           RecordNumber = CShort(RecordNumber + 1)
           If RecordNumber > NumberOfRecords Then Exit For
        Next Row
        Column = Column + COLWIDTH
     Loop
  
     LOCATE(ColumnLength + 1, 1)
     PRINT(QBString(80, "_"))  ' Print separator line.
  
     ' Scroll information about records below the part-number
     ' menu (this way, the part numbers are not erased):
     VIEW_PRINT(ColumnLength + 2, 24)
  End Sub
  
  '
  ' ========================= SORTINDEX ========================
  '                Sorts the index by part number
  ' ============================================================
  '
  Sub SortIndex()
    Static Offset As Short
    Static Limit As Short
    Static Switch As Short
    Static I As Short
     ' SHARED Index() AS IndexType, NumberOfRecords   (shared variables are declared at module level)
  
     ' Set comparison offset to half the number of records
     ' in index:
     Offset = CShort(NumberOfRecords \ 2)
  
     ' Loop until offset gets to zero:
     Do While Offset > 0
        Limit = NumberOfRecords - Offset
        Do
  
           ' Assume no switches at this offset:
           Switch = [FALSE]
  
           ' Compare elements and switch ones out of order:
           For I = 1 To Limit
              If Index(I).PartNumber > Index(I + Offset).PartNumber Then
                 SWAP(Index(I), Index(I + Offset))
                 Switch = I
              End If
           Next I
  
           ' Sort on next pass only to where last
           ' switch was made:
           Limit = Switch
        Loop While Switch <> 0
  
        ' No switches at last offset, try one half as big:
        Offset = CShort(Offset \ 2)
     Loop
  End Sub
  
End Module
