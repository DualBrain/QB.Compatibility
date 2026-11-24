' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim s$ = "alphaBETA"
    PRINT("Original string: " & s$)
    PRINT("To Lower case:   " & LCase$(s$))
    PRINT("To Upper case:   " & UCase$(s$))
  End Sub

End Module
