' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Do While INKEY$() <> ""
    Loop
    PRINT("Keyboard buffer flushed")
    End
  End Sub

End Module
