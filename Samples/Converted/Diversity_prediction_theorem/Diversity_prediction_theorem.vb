' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Vari!
    Dim Sum!
    Dim c!
    Dim AvgErr!
    Dim RefAvg!
    Dim CrowdErr!
    Dim test(1, 4) As Double
    test(0, 0) = 48!
    test(0, 1) = 47!
    test(0, 2) = 51!
    test(1, 0) = 48!
    test(1, 1) = 47!
    test(1, 2) = 51!
    test(1, 3) = 42!
    
    Dim TrueVal! = 49!
    
    For i = 0! To 1!
        Vari = 0!
        Sum = 0!
        c = 0
        While test(CInt(i), CInt(c)) <> 0
            Vari = CSng(Vari + ((test(CInt(i), CInt(c)) - TrueVal) ^ 2))
            Sum = CSng(Sum + test(CInt(i), CInt(c)))
            c = c + 1
        End While
        AvgErr = Vari / c
        RefAvg = Sum / c
        CrowdErr = CSng((TrueVal - RefAvg) ^ 2)
    
        PRINT_USING("Average error : ##.####", AvgErr)
        PRINT_USING("  Crowd error : ##.####", CrowdErr)
        PRINT_USING("    Diversity : ##.####", AvgErr - CrowdErr)
        PRINT()
    Next i
  End Sub

End Module
