' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim guess!
    Dim n!
    CLS()
    RANDOMIZE(CInt(Timer))  ' set a seed based on system time
    Dim nmax = 20!
    Dim chosen! = Int(Rnd() * nmax) + 1
    
    PRINT("Guess a whole number between 1 a" & Str$(nmax) & " " & QBChr(10))
    Do
        INPUT("Enter your number", guess)
        If (guess < n) Or (guess > nmax) Then
            PRINT("That was an invalid number")
            Exit Do
        Else
            If guess < chosen Then PRINT("Sorry, your number was too low")
            If guess > chosen Then PRINT("Sorry, your number was too high")
            If guess = chosen Then PRINT("Well guessed!")
        End If
    Loop Until guess = chosen
    End
  End Sub

End Module
