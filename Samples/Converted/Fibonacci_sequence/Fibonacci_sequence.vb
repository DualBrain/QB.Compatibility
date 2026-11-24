' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
  End Sub

  Function itFib(ByRef n As Single) As Single
    Dim sum!
      Dim n1 = 0!
      Dim n2 = 1!
      For k! = 1 To Abs(n)
          sum = n1 + n2
          n1 = n2
          n2 = sum
      Next k
      If n < 0 Then
          itFib = CSng(n1 * ((-1) ^ ((-n) + 1)))
      Else
          itFib = n1
      End If
  End Function
  
End Module
