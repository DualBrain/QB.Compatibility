' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    PRINT("5^3^2   =" & Str$((5 ^ 3) ^ 2) & " ")
    PRINT("(5^3)^2 =" & Str$((5 ^ 3) ^ 2) & " ")
    PRINT("5^(3^2) =" & Str$(5 ^ (3 ^ 2)) & " ")
    End
  End Sub

End Module
