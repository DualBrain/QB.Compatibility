' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim monton = 12!
    Dim llevar = 0!
    
    Do While monton > 0
        PRINT_USING("There are ## tokens remaining. How many would you like to take", monton)
        INPUT(llevar)
        Do While (llevar = 0) Or (llevar > 3)
           INPUT("You must take 1, 2, or 3 tokens. How many would you like to take", llevar)
        Loop
    
       PRINT("On my turn I will take" & Str$(4 - llevar) & " " & " token(s).")
        monton = monton - 4
    Loop
    
    PRINT()
    PRINT("I got the last token. I win! Better luck next time.")
    End
  End Sub

End Module
