' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim y!
    Dim m!
    Dim d_Sng!
    Dim J!
    Dim K!
    DATA("Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday")
    DATA(2021, 12, 25)
    DATA(2022, 1, 1)
    
    Dim d$(6)
    For i = 0! To 6! : READ(d$(CInt(i))) : Next i
    For i = 1! To 2!
        READ(y) : READ(m) : READ(d_Sng)
        PRINT_USING("##/##/#### is a ", m, d_Sng, y)
        If m <= 2 Then m = m + 12 : y = y - 1
        J = CInt(y) \ 100
        K = CInt(y) Mod 100
        d_Sng = CInt((((((d_Sng + (CInt(((m + 1) * 26)) \ 10)) + K) + (CInt(K) \ 4)) + (CInt(J) \ 4)) + (5 * J))) Mod 7
        PRINT(d$(CInt(d_Sng)))
    Next i
    ' DATA Saturday, Sunday, Monday, Tuesday, Wednesday, Thursday, Friday   (DATA is listed at the start of the program)
    ' DATA 2021, 12, 25   (DATA is listed at the start of the program)
    ' DATA 2022, 1, 1   (DATA is listed at the start of the program)
    End
  End Sub

End Module
