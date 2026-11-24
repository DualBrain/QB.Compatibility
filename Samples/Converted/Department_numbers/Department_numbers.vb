' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim sanitation!
    PRINT("--police--  --sanitation--  --fire--")
    
    For police = 2! To 7! Step 2!
        For fire = 1! To 7!
            If fire = police Then GoTo cont
        sanitation = (12 - police) - fire
        If (sanitation = fire) Or (sanitation = police) Then GoTo cont
        If (sanitation >= 1) And (sanitation <= 7) Then
            PRINT_USING("    #             #            #", police, fire, sanitation)
        End If
    cont:
        Next fire
    Next police
  End Sub

End Module
