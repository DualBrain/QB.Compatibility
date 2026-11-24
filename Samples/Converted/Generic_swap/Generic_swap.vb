' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim a = 1!
    Dim b = 2!
    
    PRINT(Str$(a) & " " & vbTab & Str$(b) & " ")
    nswap(a, b)
    PRINT(Str$(a) & " " & vbTab & Str$(b) & " ")
  End Sub

  Sub nswap(ByRef a As Single, ByRef b As Single)
    Dim temp!
      temp = a : a = b : b = temp
  End Sub
  
End Module
