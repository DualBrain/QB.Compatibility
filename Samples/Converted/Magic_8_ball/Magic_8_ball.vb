' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim question$
    DATA("It is certain.", "It is decidedly so.")
    DATA("Without a doubt.", "Yes ? definitely.")
    DATA("You may rely on it.", "As I see it, yes.")
    DATA("Most likely.", "Outlook good.", "Yes.")
    DATA("Signs point to yes.", "Reply hazy, try again.")
    DATA("Ask again later.", "Better not tell you now.")
    DATA("Cannot predict now.", "Concentrate and ask again.")
    DATA("Don't count on it.", "My reply is no.")
    DATA("My sources say no.", "Outlook not so good.")
    DATA("Very doubtful.")
    
    Dim answer$(19)
    For i! = 0 To UBound(answer$) : READ(answer$(CInt(i))) : Next i
    RANDOMIZE(CInt(Timer))
    
    PRINT("Q to quit.")
    Do
        INPUT("What would you like to know? ", question$, False)
        If UCase$(question$) = "Q" Then Exit Do
        PRINT(answer$(CInt(Int(Rnd() * UBound(answer$)))))
        PRINT()
    Loop
    End
    
    ' DATA "It is certain.","It is decidedly so."   (DATA is listed at the start of the program)
    ' DATA "Without a doubt.","Yes ? definitely."   (DATA is listed at the start of the program)
    ' DATA "You may rely on it.","As I see it, yes."   (DATA is listed at the start of the program)
    ' DATA "Most likely.","Outlook good.","Yes."   (DATA is listed at the start of the program)
    ' DATA "Signs point to yes.","Reply hazy, try again."   (DATA is listed at the start of the program)
    ' DATA "Ask again later.","Better not tell you now."   (DATA is listed at the start of the program)
    ' DATA "Cannot predict now.","Concentrate and ask again."   (DATA is listed at the start of the program)
    ' DATA "Don't count on it.","My reply is no."   (DATA is listed at the start of the program)
    ' DATA "My sources say no.","Outlook not so good."   (DATA is listed at the start of the program)
    ' DATA "Very doubtful."   (DATA is listed at the start of the program)
  End Sub

End Module
