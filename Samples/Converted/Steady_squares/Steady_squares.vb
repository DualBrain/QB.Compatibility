' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    For i = 1! To 9999!
      If isSteady!(i) <> 0 Then
        PRINT_USING(("####^2 = ########"), i, i * i)
      End If
    Next i
  End Sub

  Function isSteady!(ByRef n!)
    Dim mask = 1!
    Dim d! = n!
    While d <> 0
      mask = mask * 10
      d = Int(d / 10)
    End While
    isSteady! = CSng(((CInt((n! * n!)) Mod CInt(mask)) = n!))
  End Function
  
End Module
