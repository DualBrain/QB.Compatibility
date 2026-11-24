' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim j!
    Dim limite = 50000!
    Dim DSum(CInt(limite + 1)) As Single, DCount(CInt(limite + 1)) As Single
    
    For i! = 0 To limite
        DSum(CInt(i)) = 1
        DCount(CInt(i)) = 1
    Next i
    
    For i! = 2 To limite
        j = i + i
        While j <= limite
            If DSum(CInt(j)) = j Then
                PRINT_USING("######## equals the sum of its first ### divisors", j, DCount(CInt(j)))
            End If
            DSum(CInt(j)) = DSum(CInt(j)) + i
            DCount(CInt(j)) = DCount(CInt(j)) + 1
            j = j + i
        End While
    Next i
  End Sub

End Module
