' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    PRINT("0 ^ 0 =" & Str$(CSng(0 ^ 0)) & " ")
  End Sub

End Module
