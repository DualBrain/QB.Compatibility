' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim zero3d(2) As Single  'some example vectors
    zero3d(0) = 0! : zero3d(1) = 0! : zero3d(2) = 0!
    Dim zero5d(4) As Single
    zero5d(0) = 0! : zero5d(1) = 0! : zero5d(2) = 0! : zero5d(3) = 0! : zero5d(4) = 0!
    Dim x(2) As Single : x(0) = 1! : x(1) = 0! : x(2) = 0!
    Dim y(2) As Single : y(0) = 0! : y(1) = 1! : y(2) = 0!
    Dim z(2) As Single : z(0) = 0! : z(1) = 0! : z(2) = 1!
    Dim q(2) As Single : q(0) = 1! : q(1) = 1! : q(2) = 3.14159!
    Dim r(2) As Single : r(0) = -1! : r(1) = 2.618033989# : r(2) = 3!
    
    PRINT(" q dot r           = " & Str$(dot(q, r)) & " ")
    PRINT(" zero3d dot zero5d = " & Str$(dot(zero3d, zero5d)) & " ")
    PRINT(" zero3d dot x      = " & Str$(dot(zero3d, x)) & " ")
    PRINT(" z dot z           = " & Str$(dot(z, z)) & " ")
    PRINT(" y dot z           = " & Str$(dot(y, z)) & " ")
  End Sub

  
  Function dot(ByRef a() As Single, ByRef b() As Single) As Single
      If UBound(a) <> UBound(b) Then dot = 0
  
      Dim dp! = 0!
      For i! = 0 To UBound(a)
          dp = dp + (a(CInt(i)) * b(CInt(i)))
      Next i
      dot = dp
  End Function
  
End Module
