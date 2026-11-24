' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    ' DECLARE SUB ThickLine (x1!, y1!, x2!, y2!, fg!, w!)
    
    SCREEN(12)  ' (640x480, 16 colores)
    CLS()
    
    Dim bg = 15!  ' white
    Dim fg = 9!  ' lightblue
    Dim thk = 2!  ' thickness
    
    LINE(0, 0, 639, 479, CInt(bg), LineOption.BF)  ' Fondo
    
    ' Mathematical center (600 / 2 = 300)
    Dim cx = 320!  ' center
    Dim y1 = 100!  ' top part
    Dim y2 = 180!  ' wide part
    Dim y3 = 360!  ' bottom tip
    
    ' --- DIAMOND DRAWING ---
    ' Symmetrical ing around the cx axis
    ThickLine(cx - 80, y1, cx + 80, y1, fg, thk)  ' Top
    ThickLine(cx + 80, y1, cx + 160, y2, fg, thk)  ' Upper right
    ThickLine(cx + 160, y2, cx, y3, fg, thk)  ' Lower right
    ThickLine(cx, y3, cx - 160, y2, fg, thk)  ' Lower left
    ThickLine(cx - 160, y2, cx - 80, y1, fg, thk)  ' Upper left
    
    ' Inner edges (facets) aligned to the center line
    ThickLine(cx - 160, y2, cx + 160, y2, fg, thk)  ' Girdle line
    ThickLine(cx - 80, y1, cx, y3, fg, thk)  ' Inner left diagonal
    ThickLine(cx + 80, y1, cx, y3, fg, thk)  ' Inner right diagonal
    End
  End Sub

  
  Sub ThickLine(ByRef x1!, ByRef y1!, ByRef x2!, ByRef y2!, ByRef fg As Single, ByRef w!)
    Dim yMin_Int As Short
    Dim yMax_Int As Short
    Dim n As Short
    Dim j As Short
    Dim x1s!
    Dim x2s!
      Dim dx! = x2! - x1!
      Dim dy! = y2! - y1!
      Dim L! = CSng(Sqrt((dx! * dx!) + (dy! * dy!)))
      If L! = 0 Then Exit Sub
  
      dx! = dx! / L!
      dy! = dy! / L!
  
      ' Perpendicular vector
      Dim px! = (-dy!) * w!
      Dim py! = dx! * w!
  
      ' Four vertices of the rectangle
      Dim xA! = x1! + px!
      Dim yA! = y1! + py!
      Dim xB! = x1! - px!
      Dim yB! = y1! - py!
      Dim xC! = x2! - px!
      Dim yC! = y2! - py!
      Dim xD! = x2! + px!
      Dim yD! = y2! + py!
  
      ' Fill the polygon manually (scanline)
      Dim yMin! = yA!
      If yB! < yMin! Then yMin! = yB!
      If yC! < yMin! Then yMin! = yC!
      If yD! < yMin! Then yMin! = yD!
  
      Dim yMax! = yA!
      If yB! > yMax! Then yMax! = yB!
      If yC! > yMax! Then yMax! = yC!
      If yD! > yMax! Then yMax! = yD!
  
      yMin_Int = CShort(Int(yMin!))
      yMax_Int = CShort(Int(yMax!))
  
      Dim xs(4) As Single, ys(4) As Single, inter(4) As Single
      For y As Short = yMin_Int To yMax_Int
          xs(1) = xA! : ys(1) = yA!
          xs(2) = xB! : ys(2) = yB!
          xs(3) = xC! : ys(3) = yC!
          xs(4) = xD! : ys(4) = yD!
  
          n = 0
          For i = 1S To 4S
              If i = 4 Then j = 1 Else j = CShort(i + 1)
              If ((ys(i) <= y) And (ys(j) >= y)) Or ((ys(j) <= y) And (ys(i) >= y)) Then
                  If ys(i) <> ys(j) Then
                      n = CShort(n + 1)
                      inter(n) = xs(i) + (((y - ys(i)) * (xs(j) - xs(i))) / (ys(j) - ys(i)))
                  End If
              End If
          Next
  
          If n >= 2 Then
              x1s! = inter(1)
              x2s! = inter(2)
              If x1s! > x2s! Then SWAP(x1s!, x2s!)
              LINE(x1s!, y, x2s!, y, CInt(fg))
          End If
      Next
  End Sub
  
End Module
