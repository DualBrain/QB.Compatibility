' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    For i = 1! To 21! Step 2!
      PRINT(Str$(i) & " ", True)
    Next i
  End Sub

End Module
