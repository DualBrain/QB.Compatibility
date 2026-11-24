' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    For i = 1! To 10!
    	PRINT(Str$(i), True)
    	If (CInt(i) Mod 5) <> 0 Then PRINT(", ", True) Else PRINT()
    Next i
    PRINT()
    End
  End Sub

End Module
