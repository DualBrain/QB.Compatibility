' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim num!
    Dim row = 0!
    
    PRINT("Numbers which count of divisors is prime are:")
    
    For n = 1! To 1000!
        num = 0
        For m! = 1 To n
            If (CInt(n) Mod CInt(m)) = 0 Then num = num + 1
        Next m
        If (CInt(isPrime(num)) And CShort(num <> 2)) <> 0 Then
            PRINT_USING(" ##### ", n)
            row = row + 1
            If (CInt(row) Mod 5) = 0 Then PRINT()
        End If
    Next n
    
    PRINT() : PRINT("Found" & Str$(row) & " " & " numbers")
    SLEEP()
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
  
End Module
