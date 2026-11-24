' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(-19, 7, -4, 6)
    
    
    Dim x = 3!
    Dim coeffs(3) As Single
    ' DATA -19, 7, -4, 6   (DATA is listed at the start of the program)
    For a! = 0 To UBound(coeffs)
    READ(coeffs(CInt(a)))
    Next a
    
    PRINT("Horner's algorithm for the polynomial 6*x^3 - 4*x^2 + 7*x - 19 when x = 3 is: ", True)
    PRINT(Str$(Horner(coeffs, x)) & " ")
    End
  End Sub

  Function Horner(ByRef coeffs() As Single, ByRef x As Single) As Single
      Dim acumulador = 0!
      For i! = UBound(coeffs) To LBound(coeffs) Step -1
          acumulador = (acumulador * x) + coeffs(CInt(i))
      Next i
      Horner = acumulador
  End Function
  
End Module
