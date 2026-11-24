' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim sum!
    Dim prod!
    DATA(1, 2, 3, 4, 5)
    
    Dim array(5) As Single
    ' DATA 1, 2, 3, 4, 5   (DATA is listed at the start of the program)
    For index! = 1 To UBound(array)
        READ(array(CInt(index)))
    Next index
    
    sum = 0
    prod = 1
    For index! = 1 To UBound(array)
        sum = sum + array(CInt(index))
        prod = prod * array(CInt(index))
    Next index
    PRINT("The sum is " & Str$(sum) & " ")
    PRINT("and the product is " & Str$(prod) & " ")
    End
  End Sub

End Module
