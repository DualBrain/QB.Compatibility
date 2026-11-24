' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim PLAYER = 1!
    Dim COMP = 0!
    RANDOMIZE(CInt(Timer))
    
    Dim sum = 0!
    Dim total = 0!
    Dim turn! = Int(Rnd() + 0.5!)
    Dim precomp(3) As Single
    precomp(0) = 1 : precomp(1) = 1
    precomp(2) = 3 : precomp(3) = 2
    
    While sum < 21
        turn = 1 - turn
        PRINT("The sum is " & Str$(sum) & " ")
        If turn = PLAYER Then
            PRINT("It is your turn.")
            While ((total < 1) Or (total > 3)) Or ((total + sum) > 21)
                INPUT("How many would you like to total? ", total, False)
            End While
        Else
            PRINT("It is the computer's turn.")
            total = precomp(CInt(sum) Mod 4)
            PRINT("The computer totals" & Str$(total) & " " & ".")
        End If
        PRINT()
        sum = sum + total
        total = 0
    End While
    
    If turn = PLAYER Then
        PRINT("Congratulations. You win.")
    Else
        PRINT("Bad luck. The computer wins.")
    End If
  End Sub

End Module
