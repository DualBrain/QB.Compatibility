' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim tmp!
    RANDOMIZE(CInt(Timer))
    Dim number(999) As Single
    Dim highest = 0!
    Dim lowest = 100000!
    For i = 0! To 999!
        number(CInt(i)) = Int(Rnd() * 10)
        If i >= 4 Then
            tmp = (((number(CInt(i)) + (10 * number(CInt(i - 1)))) + (100 * number(CInt(i - 2)))) + (1000 * number(CInt(i - 3)))) + (10000 * number(CInt(i - 4)))
            If tmp < lowest Then lowest = tmp
            If tmp > highest Then highest = tmp
        End If
    Next i
    PRINT(Str$(highest) & " " & vbTab & Str$(lowest) & " ")
    End
  End Sub

End Module
