' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim bs!
    Dim c1!
    Dim c2!
    CLS()
    bs = 10 : c1 = 0 : c2 = 0
    For k! = 1 To CSng((bs ^ 2) - 1)
      c1 = c1 + 1
      If (CInt(k) Mod CInt((bs - 1))) = (CInt((k * k)) Mod CInt((bs - 1))) Then c2 = c2 + 1 : PRINT(Str$(k) & " ", True)
    Next k
    PRINT()
    PRINT_USING("Trying ## numbers instead of ### numbers saves ##.##%", c2, c1, 100 - ((100 * c2) / c1))
  End Sub

End Module
