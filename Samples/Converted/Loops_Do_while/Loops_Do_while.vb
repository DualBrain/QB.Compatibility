' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim a = 0!
    Do
      a = a + 1
      PRINT(Str$(a) & " ", True)
    Loop While (CInt(a) Mod 6) <> 0
  End Sub

End Module
