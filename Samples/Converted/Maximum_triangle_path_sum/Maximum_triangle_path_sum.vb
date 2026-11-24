' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim trim$(10)
    Dim ln2$
    Dim s1!
    Dim s2!
    Dim ln$(19)
    ln$(1) = "                   55"
    ln$(2) = "                  94 48"
    ln$(3) = "                95 30 96"
    ln$(4) = "               77 71 26 67"
    ln$(5) = "              97 13 76 38 45"
    ln$(6) = "             07 36 79 16 37 68"
    ln$(7) = "            48 07 09 18 70 26 06"
    ln$(8) = "           18 72 79 46 59 79 29 90"
    ln$(9) = "          20 76 87 11 32 07 07 49 18"
    ln$(10) = "         27 83 58 35 71 11 25 57 29 85"
    ln$(11) = "        14 64 36 96 27 11 58 56 92 18 55"
    ln$(12) = "       02 90 03 60 48 49 41 46 33 36 47 23"
    ln$(13) = "      92 50 48 02 36 59 42 79 72 20 82 77 42"
    ln$(14) = "     56 78 38 80 39 75 02 71 66 66 01 03 55 72"
    ln$(15) = "    44 25 67 84 71 67 11 61 40 57 58 89 40 56 36"
    ln$(16) = "   85 32 25 85 57 48 84 35 47 62 17 01 01 99 89 52"
    ln$(17) = "  06 71 28 75 94 48 37 10 23 51 06 48 53 18 74 98 15"
    ln$(18) = " 27 02 92 23 08 71 76 84 15 52 92 63 81 10 44 10 69 93"
    ln$(19) = "end"
    
    Dim matrix(20, 20) As Single
    Dim x = 1!
    Dim tam = 0!
    
    For n = 1! To 19!  'ubound(ln$) - 1
        ln2$ = trim$(CInt(ln$(CInt(n))))
        For y! = 1 To x
            matrix(CInt(x), CInt(y)) = CSng(Val(Left$(ln2$, 2)))
            If Len(ln2$) > 4 Then ln2$ = Mid$(ln2$, 4, Len(ln2$) - 4)
        Next y
        x = x + 1
        tam = tam + 1
    Next n
    
    For z! = tam - 1 To 1 Step -1
        For y! = 1 To z
            s1 = matrix(CInt(z + 1), CInt(y))
            s2 = matrix(CInt(z + 1), CInt(y + 1))
            If s1 > s2 Then
                matrix(CInt(z), CInt(y)) = matrix(CInt(z), CInt(y)) + s1
            Else
                matrix(CInt(z), CInt(y)) = matrix(CInt(z), CInt(y)) + s2
            End If
        Next y
    Next z
    
    PRINT("  maximum triangle path sum = " & Str$(matrix(1, 1)) & " ")
  End Sub

End Module
