' Converted from QBasic by qb2vb.

Module Program

  Const [FALSE] As Short = 0
  Const [TRUE] As Short = Not [FALSE]
  Const NOFILE As Short = 53
  Const FILEEXISTS As Short = 58

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim CarReturn$ = ""
    Dim LineFeed$ = ""
    Dim OutFile$ = ""
    Dim InFile$ = ""
    Dim PrevCarReturn As Short
    Dim FileChar$ = ""
    Dim More$ = ""
    ' DEFINT A-Z              ' Default variable type is integer
    
    ' The Backup$ FUNCTION makes a backup file with
    ' the same base as FileName$, plus a .BAK extension:
    ' DECLARE FUNCTION Backup$ (FileName$)
    
    ' Initialize symbolic constants and variables:
    ' CONST FALSE = 0, TRUE = NOT FALSE
    
    CarReturn$ = QBChr(13)
    LineFeed$ = QBChr(10)
    
    Do
       CLS()
    
       ' Get the name of the file to change:
       INPUT("Which file do you want to convert", OutFile$)
    
       InFile$ = Backup$(OutFile$)  ' Get the backup file's name.
    
       On Error GoTo ErrorHandler  ' Turn on error trapping.
    
       NAME(OutFile$, InFile$)  ' Copy the input file to the
                                    ' backup file.
    
       On Error GoTo 0  ' Turn off error trapping.
    
       ' Open the backup file for input and the old file
       ' for output:
       OPEN(InFile$, OpenMode.Input, 1)
       OPEN(OutFile$, OpenMode.Output, 2)
    
       ' The PrevCarReturn variable is a flag that is set to TRUE
       ' whenever the program reads a carriage-return character:
       PrevCarReturn = [FALSE]
    
       ' Read from the input file until reaching
       ' the end of the file:
       Do Until EOF(1)
    
          ' Not the end of the file, so read a character:
          FileChar$ = InputStr(1, 1)
    
          Select Case FileChar$
    
             Case CarReturn$  ' The character is a CR.
    
                ' If the previous character was also a
                ' CR, put a LF before the character:
                If PrevCarReturn <> 0 Then
                    FileChar$ = LineFeed$ + FileChar$
                End If
    
                ' In any case, set the PrevCarReturn
                ' variable to TRUE:
                PrevCarReturn = [TRUE]
    
             Case LineFeed$  ' The character is a LF.
    
                ' If the previous character was not a
                ' CR, put a CR before the character:
                If (Not PrevCarReturn) <> 0 Then
                    FileChar$ = CarReturn$ + FileChar$
                End If
    
                ' In any case, set the PrevCarReturn
                ' variable to FALSE:
                PrevCarReturn = [FALSE]
    
             Case Else  ' Neither a CR nor a LF.
    
                ' If the previous character was a CR,
                ' set the PrevCarReturn variable to FALSE
                ' and put a LF before the current character:
                If PrevCarReturn <> 0 Then
                    PrevCarReturn = [FALSE]
                    FileChar$ = LineFeed$ + FileChar$
                End If
    
          End Select
    
          ' Write the character(s) to the new file:
          PRINT(2, FileChar$, True)
       Loop
    
       ' Write a LF if the last character in the file was a CR:
       If PrevCarReturn <> 0 Then PRINT(2, LineFeed$, True)
    
       CLOSE()  ' Close both files.
       PRINT("Another file (Y/N)?")  ' Prompt to continue.
    
       ' Change the input to uppercase (capital letter):
       More$ = UCase$(InputStr(1))
    
    ' Continue the program if the user entered a "y" or a "Y":
    Loop While More$ = "Y"
    End
    
    ErrorHandler:  ' Error-handling routine
       ' CONST NOFILE = 53, FILEEXISTS = 58
    
       ' The ERR function returns the error code for last error:
       Select Case Err.Number
          Case NOFILE  ' Program couldn't find file with
                            ' input name.
             PRINT("No such file in current directory.")
             INPUT("Enter new name: ", OutFile$, False)
             InFile$ = Backup$(OutFile$)
             Resume
          Case FILEEXISTS  ' There is already a file named
                            ' <filename>.BAK in this directory:
                            ' remove it, then continue.
             KILL(InFile$)
             Resume
          Case Else  ' An unanticipated error occurred:
                            ' stop the program.
             On Error GoTo 0
       End Select
  End Sub

  '
  ' ========================= BACKUP$ ==========================
  '   This procedure returns a file name that consists of the
  '   base name of the input file (everything before the ".")
  '   plus the extension ".BAK"
  ' ============================================================
  '
  Function Backup$(ByRef FileName$)
    Static Extension As Short
  
     ' Look for a period:
     Extension = CShort(InStr(FileName$, "."))
  
     ' If there is a period, add .BAK to the base:
     If Extension > 0 Then
        Backup$ = Left$(FileName$, Extension - 1) + ".BAK"
  
     ' Otherwise, add .BAK to the whole name:
     Else
        Backup$ = FileName$ + ".BAK"
     End If
  End Function
  
End Module
