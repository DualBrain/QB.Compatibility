' Converted from QBasic by qb2vb.

Module Program

  Const EOFTYPE As Short = 0
  Const FILETYPE As Short = 1
  Const DIRTYPE As Short = 2
  Const ROOT As String = "TWH"

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim FileSpec$
    Dim PathSpec$
    ' DEFINT A-Z
    
    ' Declare symbolic constants used in program:
    ' CONST EOFTYPE = 0, FILETYPE = 1, DIRTYPE = 2, ROOT = "TWH"
    
    ' DECLARE SUB ScanDir (PathSpec$, Level, FileSpec$, Row)
    
    ' DECLARE FUNCTION MakeFileName$ (Num)
    ' DECLARE FUNCTION GetEntry$ (FileNum, EntryType)
    
    CLS()
    INPUT("File to look for", FileSpec$)
    PRINT()
    PRINT("Enter the directory where the search should start")
    PRINT("(optional drive + directories). Press <ENTER> to begin")
    PRINT("the search in the root directory of the current drive.")
    PRINT()
    INPUT("Starting directory", PathSpec$)
    CLS()
    
    Dim RightCh$ = Right$(PathSpec$, 1)
    
    If ((PathSpec$ = "") Or (RightCh$ = ":")) Or (RightCh$ <> "\") Then
       PathSpec$ = PathSpec$ + "\"
    End If
    
    FileSpec$ = UCase$(FileSpec$)
    PathSpec$ = UCase$(PathSpec$)
    Dim Level = 1S
    Dim Row = 3S
    
    ' Make the top level call (level 1) to begin the search:
    ScanDir(PathSpec$, Level, FileSpec$, Row)
    
    KILL(ROOT + ".*")  ' Delete all temporary files created
                            ' by the program.
    
    LOCATE(Row + 1, 1) : PRINT("Search complete.")
    End
  End Sub

  '
  ' ======================= GETENTRY ==========================
  '    This procedure processes entry lines in a DIR listing
  '    saved to a file.
  ' ===========================================================
  '
  Function GetEntry$(ByRef FileNum As Short, ByRef EntryType As Short)
    Static EntryLine$
    Static TestCh$
    Static EntryName$
    Static EntryExt$
  
     ' Loop until a valid entry or end-of-file (EOF) is read:
     Do Until EOF(FileNum)
        LINE_INPUT(FileNum, EntryLine$)
        If EntryLine$ <> "" Then
  
           ' Get first character from the line for test:
           TestCh$ = Left$(EntryLine$, 1)
           If (TestCh$ <> " ") And (TestCh$ <> ".") Then Exit Do
        End If
     Loop
  
     ' Entry or EOF found, decide which:
     If EOF(FileNum) Then
        EntryType = EOFTYPE
        GetEntry$ = ""
  
     Else  ' Not EOF, either a file or a directory.
  
        ' Build and return the entry name:
        EntryName$ = RTrim$(Left$(EntryLine$, 8))
  
        ' Test for extension and add to name if there is one:
        EntryExt$ = RTrim$(Mid$(EntryLine$, 10, 3))
        If EntryExt$ <> "" Then
           GetEntry$ = (EntryName$ + ".") + EntryExt$
        Else
           GetEntry$ = EntryName$
        End If
  
        ' Determine the entry type, and return that
        ' value to the point where GetEntry$ was called:
        If Mid$(EntryLine$, 15, 3) = "DIR" Then
           EntryType = DIRTYPE  ' Directory
        Else
           EntryType = FILETYPE  ' File
        End If
  
     End If
  
  End Function
  
  '
  ' ===================== MAKEFILENAME$ =======================
  '    This procedure makes a file name from a root string
  '    ("TWH" - defined as a symbolic constant at the module
  '    level) and a number passed to it as an argument (Num).
  ' ===========================================================
  '
  Function MakeFileName$(ByRef Num As Short)
  
     MakeFileName$ = (ROOT + ".") + LTrim$(Str$(Num))
  
  End Function
  
  '
  ' ======================= SCANDIR ===========================
  '   This procedure recursively scans a directory for the
  '   file name entered by the user.
  '
  '   NOTE: The SUB header doesn't use the STATIC keyword
  '         since this procedure needs a new set of variables
  '         each time it is invoked.
  ' ===========================================================
  '
  Sub ScanDir(ByRef PathSpec$, ByRef Level As Short, ByRef FileSpec$, ByRef Row As Short)
    Dim EntryType As Short
    Dim DirEntry$
    Dim NewPath$
  
     LOCATE(1, 1) : PRINT("Now searching" & Space$(50), True)
     LOCATE(1, 15) : PRINT(PathSpec$, True)
  
     ' Make a file specification for the temporary file:
     Dim TempSpec$ = MakeFileName$(Level)
  
     ' Get a directory listing of the current directory, and
     ' save it in the temporary file:
     SHELL((("DIR " + PathSpec$) + " > ") + TempSpec$)
  
     ' Get the next available file number:
     Dim FileNum As Short = CShort(FREEFILE())
  
     ' Open the DIR listing file and scan it:
     OPEN(TempSpec$, OpenMode.Input, FileNum)
  
     ' Process the file, one line at a time:
     Do
  
        ' Get an entry from the DIR listing:
        DirEntry$ = GetEntry$(FileNum, EntryType)
  
        ' If entry is a file:
        If EntryType = FILETYPE Then
  
           ' If the FileSpec$ string matches, print entry and
           ' exit this loop:
           If DirEntry$ = FileSpec$ Then
              LOCATE(Row, 1) : PRINT(PathSpec$ & DirEntry$, True)
              Row = CShort(Row + 1)
              EntryType = EOFTYPE
           End If
  
        ' If the entry is a directory, then make a recursive
        ' call to ScanDir with the new directory:
        ElseIf EntryType = DIRTYPE Then
           NewPath$ = (PathSpec$ + DirEntry$) + "\"
           ScanDir(NewPath$, CShort(Level + 1), FileSpec$, Row)
           LOCATE(1, 1) : PRINT("Now searching" & Space$(50), True)
           LOCATE(1, 15) : PRINT(PathSpec$, True)
        End If
  
     Loop Until EntryType = EOFTYPE
  
     ' Scan on this DIR listing file is finished, so close it:
     CLOSE(FileNum)
  End Sub
  
End Module
