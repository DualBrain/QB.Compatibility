' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i!
    Dim n = 0!
    Dim sum = 0!
    
    Do
        INPUT("Enter integral rainfall (99999 to quit): ", i, False)
        If i = 99999 Then
            Exit Do
        ElseIf (i < 0) Or (i <> Int(i)) Then
            PRINT("Must be an integer no less than 0, try again.")
        Else
            n = n + 1
            sum = sum + i
            PRINT("  The current average rainfall is" & Str$(sum / n) & " ")
        End If
    Loop
  End Sub

End Module
