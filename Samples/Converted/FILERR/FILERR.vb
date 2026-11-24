' Converted from QBasic by qb2vb.

Module Program

  Const [FALSE] As Short = 0
  Const [TRUE] As Short = Not [FALSE]

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Restart!
    Dim FileName$
    Dim FileNum!
    Dim LocString$
    Dim LineNum!
    Dim LineBuffer$
    Dim Char_$
    ' Declare symbolic constants used in program:
    ' CONST FALSE = 0, TRUE = NOT FALSE
    
    ' DECLARE FUNCTION GetFileName$ ()
    
    ' Set up the ERROR trap, and specify the name of the
    ' error-handling routine:
    On Error GoTo ErrorProc
    
    Do
       Restart = [FALSE]
       CLS()
    
       FileName$ = GetFileName$()  ' Input file name.
    
       If FileName$ = "" Then
          End  ' End if <ENTER> pressed.
       Else
    
          ' Otherwise, open the file, assigning it the
          ' next available file number:
          FileNum = FREEFILE()
          OPEN(FileName$, OpenMode.Input, CInt(FileNum))
       End If
    
       If (Not CInt(Restart)) <> 0 Then
    
          ' Input search string:
          LINE_INPUT("Enter string to locate: ", LocString$)
          LocString$ = UCase$(LocString$)
    
          ' Loop through the lines in the file, printing them
          ' if they contain the search string:
          LineNum = 1
          Do While Not EOF(CInt(FileNum))
    
             ' Input line from file:
             LINE_INPUT(CInt(FileNum), LineBuffer$)
    
             ' Check for string, printing the line and its
             ' number if found:
             If InStr(UCase$(LineBuffer$), LocString$) <> 0 Then
                PRINT_USING("#### &", LineNum, LineBuffer$)
             End If
    
             LineNum = LineNum + 1
          Loop
    
          CLOSE(CInt(FileNum))  ' Close the file.
    
       End If
    Loop While Restart = [TRUE]
    
    End
    
    ErrorProc:
    
       Select Case Err.Number
    
          Case 64  ' Bad File Name
             PRINT("** ERROR - Invalid file name")
    
             ' Get a new file name and try again:
             FileName$ = GetFileName$()
    
             ' Resume at the statement that caused the error:
             Resume
    
          Case 71  ' Disk not ready
             PRINT("** ERROR - Disk drive not ready")
             PRINT("Press C to continue, R to restart, Q to quit: ")
             Do
                Char_$ = UCase$(InputStr(1))
                If Char_$ = "C" Then
                   Resume  ' Resume where you left off
    
                ElseIf Char_$ = "R" Then
                   Restart = [TRUE]  ' Resume at beginning
                   Resume Next
    
                ElseIf Char_$ = "Q" Then
                   End  ' Don't resume at all
                End If
             Loop
    
          Case 53, 76  ' File or path not found
             PRINT("** ERROR - File or path not found")
             FileName$ = GetFileName$()
             Resume
    
          Case Else  ' Unforeseen error
    
             ' Disable error trapping and print standard
             ' system message:
             On Error GoTo 0
       End Select
  End Sub

  '
  ' ======================= GETFILENAME$ =======================
  '              Returns a file name from user input
  ' ============================================================
  '
  Function GetFileName$()
    Static FTemp$
     INPUT("Enter file to search (press ENTER to quit): ", FTemp$, False)
     GetFileName$ = FTemp$
  End Function
  
End Module
