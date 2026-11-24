' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim J!
    10: CLS()  '  10 HOME for Applesoft BASIC
    20: For J = Asc("a") To Asc("z")
    30: PRINT(QBChr(CInt(J)), True)
    40: Next J
    50: PRINT()
    60: For J = Asc("A") To Asc("Z")
    70: PRINT(QBChr(CInt(J)), True)
    80: Next J
    90: End
  End Sub

End Module
