' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim firstEquation(3) As Single
    firstEquation(1) = 3
    firstEquation(2) = 1
    firstEquation(3) = -1
    Dim secondEquation(3) As Single
    secondEquation(1) = 2
    secondEquation(2) = -3
    secondEquation(3) = -19
    
    getCrossingPoint(firstEquation, secondEquation)
    End
  End Sub

  
  Sub getCrossingPoint(ByRef firstEquation() As Single, ByRef secondEquation() As Single)
      Dim x1! = firstEquation(1)
      Dim y1! = firstEquation(2)
      Dim r1! = firstEquation(3)
      Dim x2! = secondEquation(1)
      Dim y2! = secondEquation(2)
      Dim r2! = secondEquation(3)
      Dim temp(3) As Single
      temp(1) = x1
      temp(2) = -y1
      temp(3) = r1
      Dim resultY! = ((temp(1) * r2) - (x2 * temp(3))) / ((x2 * temp(2)) + (temp(1) * y2))
      Dim resultX! = (r1 - (y1 * resultY)) / x1
      PRINT("x = " & Str$(resultX) & " ")
      PRINT("y = " & Str$(resultY) & " ")
  End Sub
  
End Module
