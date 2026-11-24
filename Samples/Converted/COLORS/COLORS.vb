' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim X!
    Dim Y!
    Dim PaletteVal!
    Dim BackGroundVal!
    Dim Pause$ = ""
    SCREEN(1)
    
    Dim Esc$ = QBChr(27)
    
    ' Draw three boxes and paint the interior of each
    ' box with a different color:
    For ColorVal = 1! To 3!
       LINE(False, X, Y, [STEP], 60, 50, CInt(ColorVal), LineOption.BF)
       X = X + 61
       Y = Y + 51
    Next ColorVal
    
    LOCATE(21, 1)
    PRINT("Press ESC to end.")
    PRINT("Press any other key to continue.")
    
    ' Restrict additional printed output to the twenty-third line:
    VIEW_PRINT(23, 23)
    
    Do
       PaletteVal = 1
       Do
    
          ' PaletteVal is either one or zero:
          PaletteVal = 1 - PaletteVal
    
          ' Set the background color and choose the palette:
          COLOR(CInt(BackGroundVal), CInt(PaletteVal))
          PRINT("Background =" & Str$(BackGroundVal) & " " & "Palette =" & Str$(PaletteVal) & " ", True)
    
          Pause$ = InputStr(1)  ' Wait for a keystroke.
          PRINT()
    
       ' Exit the loop if both palettes have been shown,
       ' or if the user pressed the ESC key:
       Loop Until (PaletteVal = 1) Or (Pause$ = Esc$)
    
       BackGroundVal = BackGroundVal + 1
    
    ' Exit this loop if all sixteen background colors have been
    ' shown, or if the user pressed the ESC key:
    Loop Until (BackGroundVal > 15) Or (Pause$ = Esc$)
    
    SCREEN(0)  ' Restore text mode and
    WIDTH(80)  ' eighty-column screen width.
  End Sub

End Module
