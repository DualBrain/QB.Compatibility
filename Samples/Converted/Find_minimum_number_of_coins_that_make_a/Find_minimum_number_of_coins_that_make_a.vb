' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim a!
    Dim t!
    Dim tmp!
    DATA(1, 2, 5, 10, 20, 50, 100, 200)
    
    CLS()
    Dim amount = 988!
    Dim sumcoins = 0!
    Dim coins(7) As Single
    ' DATA 1,2,5,10,20,50,100,200   (DATA is listed at the start of the program)
    For i = 0! To 7!
        READ(a)
        coins(CInt(i)) = a
    Next i
    PRINT("Make a value of" & Str$(amount) & " " & "using the coins 1, 2, 5, 10, 20, 50, 100 and 200:")
    For n! = UBound(coins) To 0 Step -1
        'tmp = floor(amount / coins(n))
        t = (amount / coins(CInt(n)))
        tmp = Int(t) - CShort(((Sign(t) < 0) And (t <> Int(t))))
        If tmp >= 0 Then
            PRINT(Str$(tmp) & " " & "* " & Str$(coins(CInt(n))) & " ")
            sumcoins = sumcoins + tmp
            amount = CInt(amount) Mod CInt(coins(CInt(n)))
        End If
    Next n
    End
  End Sub

End Module
