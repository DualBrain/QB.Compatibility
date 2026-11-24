' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim ext! = Recursion(0)
  End Sub

  Function Recursion(ByRef i As Single) As Single
  PRINT(Str$(i) & " ")
  Dim ext! = Recursion(i + 1)
  Recursion = 0
  End Function
  
End Module
