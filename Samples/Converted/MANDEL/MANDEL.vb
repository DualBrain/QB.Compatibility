' Converted from QBasic by qb2vb.

Module Program

  Const [FALSE] As Short = 0
  Const [TRUE] As Short = Not [FALSE]
  Const MAXLOOP As Short = 30
  Const MAXSIZE As Integer = 1000000
  Dim PaletteArray(15) As Short
  Dim ColorRange As Short
  Dim EgaMode As Short

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim I As Short
    Dim WLeft As Short
    Dim WRight As Short
    Dim WTop As Short
    Dim WBottom As Short
    Dim VLeft As Short
    Dim VRight As Short
    Dim VTop As Short
    Dim VBottom As Short
    Dim XLength As Short
    Dim YLength As Short
    Dim ColorWidth As Short
    Dim Y As Short
    Dim LogicY As Short
    Dim OldColor As Short
    Dim X As Short
    Dim LogicX As Short
    Dim MandelX%
    Dim MandelY%
    Dim RealNum%
    Dim ImagNum%
    Dim PColor As Short
    ' DEFINT A-Z         ' Default variable type is integer
    
    ' DECLARE SUB ShiftPalette ()
    ' DECLARE SUB WindowVals (WL%, WR%, WT%, WB%)
    ' DECLARE SUB ScreenTest (EM%, CR%, VL%, VR%, VT%, VB%)
    
    ' CONST FALSE = 0, TRUE = NOT FALSE       ' Boolean constants
    
    ' Set maximum number of iterations per point:
    ' CONST MAXLOOP = 30, MAXSIZE = 1000000
    
    For I = 0 To 15 : PaletteArray(I) = I : Next I
    
    ' Call WindowVals to get coordinates of window corners:
    WindowVals(WLeft, WRight, WTop, WBottom)
    
    ' Call ScreenTest to find out if this is an EGA machine,
    ' and get coordinates of viewport corners:
    ScreenTest(EgaMode, ColorRange, VLeft, VRight, VTop, VBottom)
    
    ' Define viewport and corresponding window:
    VIEW(CInt(VLeft), CInt(VTop), CInt(VRight), CInt(VBottom), 0, ColorRange)
    WINDOW(WLeft, WTop, WRight, WBottom)
    
    LOCATE(24, 10) : PRINT("Press any key to quit.", True)
    
    XLength = VRight - VLeft
    YLength = VBottom - VTop
    ColorWidth = MAXLOOP \ ColorRange
    
    ' Loop through each pixel in viewport and calculate
    ' whether or not it is in the Mandelbrot Set:
    For Y = 0 To YLength  ' Loop through every line in
                               ' the viewport.
       LogicY = CShort(PMAP(Y, 3))  ' Get the pixel's logical y
                               ' coordinate.
       PSET(WLeft, LogicY)  ' Plot leftmost pixel in the line.
       OldColor = 0  ' Start with background color.
    
       For X = 0 To XLength  ' Loop through every pixel in
                               ' the line.
          LogicX = CShort(PMAP(X, 2))  ' Get the pixel's logical x
                               ' coordinate .
          MandelX% = LogicX
          MandelY% = LogicY
    
          ' Do the calculations to see if this point is in
          ' the Mandelbrot Set:
          For I = 1 To MAXLOOP
             RealNum% = MandelX% * MandelX%
             ImagNum% = MandelY% * MandelY%
             If (RealNum% + ImagNum%) >= MAXSIZE Then Exit For
             MandelY% = ((MandelX% * MandelY%) \ 250) + LogicY
             MandelX% = ((RealNum% - ImagNum%) \ 500) + LogicX
          Next I
    
          ' Assign a color to the point:
          PColor = I \ ColorWidth
    
          ' If color has changed, draw a line from the
          ' last point referenced to the new point,
          ' using the old color:
          If PColor <> OldColor Then
             LINE(False, LogicX, LogicY, (ColorRange - OldColor))
             OldColor = PColor
          End If
    
          If INKEY$() <> "" Then End
       Next X
    
       ' Draw the last line segment to the right edge of
       ' the viewport:
       LINE(False, LogicX, LogicY, (ColorRange - OldColor))
    
       ' If this is an EGA machine, shift the palette after
       ' drawing each line:
       If EgaMode <> 0 Then ShiftPalette()
    Next Y
    
    Do
       ' Continue shifting the palette until the user
       ' presses a key:
       If EgaMode <> 0 Then ShiftPalette()
    Loop While INKEY$() = ""
    
    SCREEN(0, 0)  ' Restore the screen to text mode,
    WIDTH(80)  ' 80 columns.
    End
    
    BadScreen:  ' Error handler that is invoked if
       EgaMode = [FALSE]  ' there is no EGA graphics card
       Resume Next
  End Sub

  '
  ' ======================= ShiftPalette =======================
  ' Rotates the palette by one each time it is called.
  ' ============================================================
  '
  Sub ShiftPalette()
    Static I As Short
     ' SHARED PaletteArray(), ColorRange   (shared variables are declared at module level)
  
     For I = 1 To ColorRange
        PaletteArray(I) = CShort((PaletteArray(I) Mod ColorRange) + 1)
     Next I
     PALETTE_USING(PaletteArray, 0)
  
  End Sub
  
  '
  ' ======================== ScreenTest ========================
  '    Tests to see if user has EGA hardware with SCREEN 8.
  '    If this causes an error, the EM flag is set to FALSE,
  '    and the screen is set with SCREEN 1.
  '
  '    Also sets values for corners of viewport (VL = left,
  '    VR = right, VT = top, VB = bottom), scaled with the
  '    correct aspect ratio so viewport is a perfect square.
  ' ============================================================
  '
  Sub ScreenTest(ByRef EM As Short, ByRef CR As Short, ByRef VL As Short, ByRef VR As Short, ByRef VT As Short, ByRef VB As Short)
     EM = [TRUE]
     On Error GoTo BadScreen
     SCREEN(8, 1)
     On Error GoTo 0
  
     If EM <> 0 Then  ' No error, so SCREEN 8 is OK
        VL = 110 : VR = 529
        VT = 5 : VB = 179
        CR = 15  ' 16 colors (0 - 15)
  
     Else  ' Error, so use SCREEN 1
        SCREEN(1, 1)
        VL = 55 : VR = 264
        VT = 5 : VB = 179
        CR = 3  ' 4 colors (0 - 3)
     End If
  
    Exit Sub
  BadScreen:  ' Error handler that is invoked if
     EgaMode = [FALSE]  ' there is no EGA graphics card
     Resume Next
  End Sub
  
  '
  ' ======================== WindowVals ========================
  '     Gets window corners as input from the user, or sets
  '     values for the corners if there is no input.
  ' ============================================================
  '
  Sub WindowVals(ByRef WL As Short, ByRef WR As Short, ByRef WT As Short, ByRef WB As Short)
    Static Resp$
     CLS()
     PRINT("This program prints the graphic representation of")
     PRINT("the complete Mandelbrot Set. The default window is")
     PRINT("from (-1000,625) to (250,-625). To zoom in on part")
     PRINT("of the figure, input coordinates inside this window.")
     PRINT()
     PRINT("Press <ENTER> to see the default window. Press any")
     PRINT("other key to input your own window coordinates: ", True)
     LOCATE(-1, -1, 1)
     Resp$ = InputStr(1)
  
     ' User didn't press ENTER, so input window corners:
     If Resp$ <> QBChr(13) Then
        PRINT()
        INPUT("X coordinate of upper left corner: ", WL, False)
        Do
           INPUT("X coordinate of lower right corner: ", WR, False)
           If WR <= WL Then
           PRINT("Right corner must be greater than left corner.")
           End If
        Loop While WR <= WL
        INPUT("Y coordinate of upper left corner: ", WT, False)
        Do
           INPUT("Y coordinate of lower right corner: ", WB, False)
           If WB >= WT Then
           PRINT("Bottom corner must be less than top corner.")
           End If
        Loop While WB >= WT
  
     Else  ' Pressed ENTER, so set default values.
        WL = -1000
        WR = 250
        WT = 625
        WB = -625
     End If
  End Sub
  
End Module
