' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim a!
    Dim b!
    Dim n!
    Dim w!
    Dim pi! = CSng(4 * Atan(1))
    a = 0 : b = 1 : n = 10
    Dim cheby!(CInt(n))
    Dim coef!(CInt(n))
    
    For i! = 0 To n - 1
        coef!(CInt(i)) = CSng(Cos(((Cos((pi / n) * (i + (1 / 2))) * (b - a)) / 2) + ((b + a) / 2)))
    Next i
    
    For i! = 0 To n - 1
        w = 0
        For j! = 0 To n - 1
            w = CSng(w + (coef!(CInt(j)) * Cos(((pi / n) * i) * (j + (1 / 2)))))
        Next j
        cheby!(CInt(i)) = (w * 2) / n
        PRINT_USING(" # : ##.#####################", i, cheby!(CInt(i)))
    Next i
    End
  End Sub

End Module
