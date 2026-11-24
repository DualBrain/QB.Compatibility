' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    PRINT(" n:      Wilson primes")
    PRINT("----------------------")
    For n = 1! To 11!
        PRINT_USING_NOCR("##:      ", n)
        For p = 3! To 10099! Step 2!
            If (CInt(isPrime(p)) And CInt(isWilson(n, p))) <> 0 Then PRINT(Str$(p) & " " & " ", True)
        Next p
        PRINT()
    Next n
    End
  End Sub

  Function isPrime(ByRef ValorEval As Single) As Single
    Dim [False] As Single
    Dim [True] As Single
      If ValorEval < 2 Then isPrime = [False]
      If (CInt(ValorEval) Mod 2) = 0 Then isPrime = 2
      If (CInt(ValorEval) Mod 3) = 0 Then isPrime = 3
      Dim d = 5!
      While (d * d) <= ValorEval
          If (CInt(ValorEval) Mod CInt(d)) = 0 Then isPrime = [False] Else d = d + 2
      End While
      isPrime = [True]
  End Function
  
  
  Function isWilson(ByRef n As Single, ByRef p As Single) As Single
    Dim [False] As Single
    Dim [True] As Single
      If p < n Then isWilson = [False]
      Dim prod = 1!
      Dim p2! = CSng(p ^ 2)
      For i! = 1 To n - 1
          prod = CInt((prod * i)) Mod CInt(p2)
      Next i
      For i! = 1 To p - n
          prod = CInt((prod * i)) Mod CInt(p2)
      Next i
      prod = CInt(((p2 + prod) - ((-1) ^ n))) Mod CInt(p2)
      If prod = 0 Then isWilson = [True] Else isWilson = [False]
  End Function
  
End Module
