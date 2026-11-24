' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(1, 3, 7, 8, 10)
    DATA(2, 4, 16, 14, 4)
    DATA(3, 1, 9, 18, 11)
    DATA(12, 14, 17, 18, 20)
    DATA(7, 1, 3, 9, 5)
    
    ' DEFINT A-Z
    
    Dim diag(5, 5) As Short
    Dim lenDiag As Short = CShort(UBound(diag))
    Dim ind As Short = lenDiag
    Dim sumDiag = 0S
    
    For x As Short = 1 To lenDiag
        For y As Short = 1 To lenDiag
            READ(diag(x, y))
        Next y
    Next x
    
    For x As Short = 1 To lenDiag
        For y As Short = 1 To lenDiag - ind
            sumDiag = sumDiag + diag(x, y)
        Next y
        ind = CShort(ind - 1)
    Next x
    
    PRINT("Sum of elements below main diagonal of matrix is" & Str$(sumDiag) & " ")
    End
    
    ' DATA 1, 3, 7, 8,10   (DATA is listed at the start of the program)
    ' DATA 2, 4,16,14, 4   (DATA is listed at the start of the program)
    ' DATA 3, 1, 9,18,11   (DATA is listed at the start of the program)
    ' DATA 12,14,17,18,20   (DATA is listed at the start of the program)
    ' DATA 7, 1, 3, 9, 5   (DATA is listed at the start of the program)
  End Sub

End Module
