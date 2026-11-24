' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    ' The macro string to draw the cube and paint its sides:
    Dim Plot$ = "BR30 BU25 C1 R54 U45 L54 D45 BE20 P1,1 G20 C2 G20" + "R54 E20 L54 BD5 P2,2 U5 C4 G20 U45 E20 D45 BL5 P4,4"
    
    Dim APage = 1S  ' Initialize values for the active and visual
    Dim VPage = 0S  ' pages, as well as the angle of rotation.
    Dim Angle = 0S
    
    Do
    
       ' Draw to the active page while showing
       ' the visual page:
       SCREEN(7, -1, APage, VPage)
       CLS(1)
    
       ' Rotate the cube "Angle%" degrees:
       DRAW(("TA" + Str$(Angle)) + Plot$)
    
       ' Angle% is some multiple of 15 degrees:
       Angle = CShort((Angle + 15) Mod 360)
    
       ' Switch the active and visual pages:
       SWAP(APage, VPage)
    
    Loop While INKEY$() = ""  ' A key press ends the program.
    
    End
  End Sub

End Module
