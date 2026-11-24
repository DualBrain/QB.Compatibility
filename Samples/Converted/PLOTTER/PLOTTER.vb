' Converted from QBasic by qb2vb.

Module Program

  Const UP As Short = 72
  Const DOWN As Short = 80
  Const LFT As Short = 75
  Const RGHT As Short = 77
  Const UPLFT As Short = 71
  Const UPRGHT As Short = 73
  Const DOWNLFT As Short = 79
  Const DOWNRGHT As Short = 81
  Const SPACEBAR As String = " "

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim KeyVal$
    ' Values for keys on the numeric keypad and the spacebar:
    ' CONST UP = 72, DOWN = 80, LFT = 75, RGHT = 77
    ' CONST UPLFT = 71, UPRGHT = 73, DOWNLFT = 79, DOWNRGHT = 81
    ' CONST SPACEBAR = " "
    
    ' Null$ is the first character of the two-character INKEY$
    ' value returned for direction keys such as UP and DOWN:
    Dim Null$ = QBChr(0)
    
    ' Plot$ = "" means draw lines; Plot$ = "B" means move
    ' graphics cursor, but don't draw lines:
    Dim Plot$ = ""
    
    PRINT("Use the cursor movement keys to draw lines.")
    PRINT("Press the spacebar to toggle line drawing on and off.")
    PRINT("Press <ENTER> to begin. Press q to end the program.")
    Do : Loop While INKEY$() = ""
    
    SCREEN(1)
    CLS()
    
    Do
       Select Case KeyVal$
          Case Null$ + QBChr(UP)
             DRAW(Plot$ + "C1 U2")
          Case Null$ + QBChr(DOWN)
             DRAW(Plot$ + "C1 D2")
          Case Null$ + QBChr(LFT)
             DRAW(Plot$ + "C2 L2")
          Case Null$ + QBChr(RGHT)
             DRAW(Plot$ + "C2 R2")
          Case Null$ + QBChr(UPLFT)
             DRAW(Plot$ + "C3 H2")
          Case Null$ + QBChr(UPRGHT)
             DRAW(Plot$ + "C3 E2")
          Case Null$ + QBChr(DOWNLFT)
             DRAW(Plot$ + "C3 G2")
          Case Null$ + QBChr(DOWNRGHT)
             DRAW(Plot$ + "C3 F2")
          Case SPACEBAR
             If Plot$ = "" Then Plot$ = "B " Else Plot$ = ""
          Case Else
             ' The user pressed some key other than one of the
             ' direction keys, the  spacebar, or "q", so
             ' don't do anything.
       End Select
    
       KeyVal$ = INKEY$()
    
    Loop Until KeyVal$ = "q"
    
    SCREEN(0, 0)
    WIDTH(80)
    End
  End Sub

End Module
