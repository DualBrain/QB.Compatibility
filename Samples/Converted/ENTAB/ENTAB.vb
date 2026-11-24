' Converted from QBasic by qb2vb.

Module Program

  Const MAXLINE As Short = 255
  Const TABSPACE As Short = 8
  Const NO As Short = 0
  Const YES As Short = Not NO
  Dim TabStops(MAXLINE) As Short

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim COMMAND$ = ""
    Dim CurrentColumn As Short
    Dim C$ = ""
    ' ENTAB.BAS
    '
    ' Replace runs of spaces in a file with tabs.
    '
    ' DECLARE SUB SetTabPos ()
    ' DECLARE SUB StripCommand (CLine$)
    
    
    ' DEFINT A-Z
    ' DECLARE FUNCTION ThisIsATab (Column AS INTEGER)
    
    ' CONST MAXLINE = 255
    ' CONST TABSPACE = 8
    ' CONST NO = 0, YES = NOT NO
    
    ' DIM SHARED TabStops(MAXLINE) AS INTEGER
    
    StripCommand((COMMAND$))
    
    ' Set the tab positions (uses the global array TabStops).
    SetTabPos()
    
    Dim LastColumn = 1S
    
    Do
    
       CurrentColumn = LastColumn
    
    ' Replace a run of blanks with a tab when you reach a tab
    ' column. CurrentColumn is the current column read.
    ' LastColumn is the last column that was printed.
       Do
          C$ = InputStr(1, 1)
          If (C$ <> " ") And (C$ <> QBChr(9)) Then Exit Do
          CurrentColumn = CShort(CurrentColumn + 1)
          If (CShort(C$ = QBChr(9)) Or ThisIsATab(CurrentColumn)) <> 0 Then
             ' Go to a tab column if we have a tab and this
             ' is not a tab column.
             Do While (Not ThisIsATab(CurrentColumn)) <> 0
                CurrentColumn = CShort(CurrentColumn + 1)
             Loop
             PRINT(2, QBChr(9), True)
             LastColumn = CurrentColumn
          End If
       Loop
    
    ' Print out any blanks left over.
       Do While LastColumn < CurrentColumn
          PRINT(2, " ", True)
          LastColumn = CShort(LastColumn + 1)
       Loop
    
    ' Print the non-blank character.
       PRINT(2, C$, True)
    
    ' Reset the column position if this is the end of a line.
       If C$ = QBChr(10) Then
          LastColumn = 1
       Else
          LastColumn = CShort(LastColumn + 1)
       End If
    
    Loop Until EOF(1)
    CLOSE(1, 2)
    End
  End Sub

  
  '------------------SUB SetTabPos-------------------------
  ' Set the tab positions in the array TabStops.
  '
  Sub SetTabPos()
    Static I As Short
     For I = 1 To 255
        TabStops(I) = CShort(((I Mod TABSPACE) = 1))
     Next I
  End Sub
  
  '
  '------------------SUB StripCommand----------------------
  '
  Sub StripCommand(ByRef CommandLine$)
    Static InFileName$ = ""
    Static OutFileName$ = ""
    Static SpacePos As Short
     If CommandLine$ = "" Then
        INPUT("File to entab:   ", InFileName$, False)
        INPUT("Store entabbed file in:   ", OutFileName$, False)
     Else
        SpacePos = CShort(InStr(CommandLine$, " "))
        If SpacePos > 0 Then
           InFileName$ = Left$(CommandLine$, SpacePos - 1)
           OutFileName$ = LTrim$(Mid$(CommandLine$, SpacePos))
        Else
           InFileName$ = CommandLine$
           INPUT("Store entabbed file in:   ", OutFileName$, False)
        End If
     End If
     OPEN(InFileName$, OpenMode.Input, 1)
     OPEN(OutFileName$, OpenMode.Output, 2)
  End Sub
  
  '---------------FUNCTION ThisIsATab----------------------
  ' Answer the question, "Is this a tab position?"
  '
  Function ThisIsATab(ByRef LastColumn As Short) As Short
     If LastColumn > MAXLINE Then
        ThisIsATab = YES
     Else
        ThisIsATab = TabStops(LastColumn)
     End If
  End Function
  
End Module
