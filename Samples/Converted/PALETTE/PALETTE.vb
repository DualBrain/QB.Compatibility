' Converted from QBasic by qb2vb.

Module Program

  Dim PaletteArray(15) As Short
  Const ASPECT As Single = CSng(1 / 3)

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    ' DECLARE SUB InitPalette ()
    ' DECLARE SUB ChangePalette ()
    ' DECLARE SUB DrawEllipses ()
    ' DEFINT A-Z
    
    ' DIM SHARED PaletteArray(15)
    
    SCREEN(8)  ' 640 x 200 resolution; 16 colors
    
    InitPalette()
    DrawEllipses()
    
    Do
       ChangePalette()
    Loop While INKEY$() = ""  ' Shift palette until key pressed
    
    End
  End Sub

  
  '
  ' ======================= InitPalette ========================
  '    This procedure initializes the integer array used to
  '    change the palette.
  ' ============================================================
  '
  Sub InitPalette()
    Static I As Short
     For I = 0 To 15
        PaletteArray(I) = I
     Next I
  End Sub
  
  
  '
  ' ====================== DrawEllipses ========================
  '    This procedure draws fifteen concentric ellipses, and
  '    paints the interior of each with a different color.
  ' ============================================================
  '
  Sub DrawEllipses()
    Static ColorVal As Short
    Static Radius As Short
     ' CONST ASPECT = 1 / 3
     For ColorVal = 15 To 1 Step -1
        Radius = CShort(20 * ColorVal)
        CIRCLE(320, 100, Radius, ColorVal, Nothing, Nothing, ASPECT)
        PAINT(320, 100, ColorVal)
     Next
  End Sub
  
  
  '
  ' ====================== ChangePalette =======================
  '    This procedure rotates the palette by one each time it
  '    is called.  For example, after the first call to
  '    ChangePalette, PaletteArray(1) = 2, PaletteArray(2) = 3,
  '    . . . , PaletteArray(14) = 15, and PaletteArray(15) = 1
  ' ============================================================
  '
  Sub ChangePalette()
    Static I As Short
     For I = 1 To 15
        PaletteArray(I) = CShort((PaletteArray(I) Mod 15) + 1)
     Next I
  
     ' Shift the color displayed by each of the attributes from
     ' one to fifteen:
     PALETTE_USING(PaletteArray, 0)
  End Sub
  
End Module
