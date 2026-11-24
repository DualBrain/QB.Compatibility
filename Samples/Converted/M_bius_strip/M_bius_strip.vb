' Converted from QBasic by qb2vb.

Module Program

  Const PI As Double = 3.141592654#

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    ' DECLARE SUB drawMoebius ()
    
    ' CONST PI = 3.141592654#
    
    SCREEN(12)  ' (640x480, 16 colores)
    CLS()
    
    drawMoebius()
    SLEEP()
  End Sub

  
  Sub drawMoebius()
      Dim cX As Short, cY As Short, u As Short
      Dim radius As Single, halfWidth As Single, k As Single, radU As Single
      Dim v1 As Single, x1 As Single, y1 As Single, z1 As Single, px1 As Single, py1 As Single
      Dim v2 As Single, x2 As Single, y2 As Single, z2 As Single, px2 As Single, py2 As Single
  
      cX = 320
      cY = 240
      radius = 150!
      halfWidth = 60!
      k = .3!
  
      For u = 0 To 360 Step 2
          radU = CSng((u * PI) / 180!)
  
          ' Inner edge point
          v1 = -halfWidth
          x1 = CSng((radius + (v1 * Cos(radU / 2))) * Cos(radU))
          y1 = CSng((radius + (v1 * Cos(radU / 2))) * Sin(radU))
          z1 = CSng(v1 * Sin(radU / 2))
          px1 = (cX + x1) + (z1 * k)
          py1 = (cY + y1) - (z1 * k)
  
          ' Outer edge point
          v2 = halfWidth
          x2 = CSng((radius + (v2 * Cos(radU / 2))) * Cos(radU))
          y2 = CSng((radius + (v2 * Cos(radU / 2))) * Sin(radU))
          z2 = CSng(v2 * Sin(radU / 2))
          px2 = (cX + x2) + (z2 * k)
          py2 = (cY + y2) - (z2 * k)
  
          ' Draw a red line between the two edges
          LINE(px1, py1, px2, py2, 4)
      Next u
  End Sub
  
End Module
