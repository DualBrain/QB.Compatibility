' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    PRINT(Str$(fpow(1001)) & " ")
  End Sub

  Function fpow(ByRef n As Single) As Single
    Dim i!
      While ((i * i) - ((i - 1) * (i - 1))) < n
          i = i + 1
      End While
      fpow = i
  End Function
  
End Module
