' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim KBD$
    PRINT("Press Y or N to continue.")
    Do
        KBD$ = ""
        While KBD$ = ""
            KBD$ = UCase$(INKEY$())
        End While
        If (KBD$ <> "Y") And (KBD$ <> "N") Then BEEP()
    Loop Until (KBD$ = "Y") Or (KBD$ = "N")
    PRINT("The response was " & KBD$)
  End Sub

End Module
