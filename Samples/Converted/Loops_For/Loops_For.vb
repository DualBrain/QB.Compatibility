' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    For i = 1! To 5!
       For j! = 1 To i
          PRINT("*", True)
       Next j
       PRINT()
    Next i
  End Sub

End Module
