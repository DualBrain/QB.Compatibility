' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Divisors!
    Dim MaxAntiPrime = 20!
    Dim n = 0!
    Dim MaxDivisors = 0!
    Dim AntiPrimeCount = 0!
    PRINT("The first 20 anti-primes are: ")
    While AntiPrimeCount < MaxAntiPrime
        n = n + 1
        Divisors = DivisorCount(n)
        If Divisors > MaxDivisors Then
            PRINT(Str$(n) & " ", True)
            MaxDivisors = Divisors
            AntiPrimeCount = AntiPrimeCount + 1
        End If
    End While
    End
  End Sub

  
  Function DivisorCount(ByRef v As Single) As Single
    Dim count!
      Dim total = 1!
      Dim n! = v
      While (CInt(n) Mod 2) = 0
          total = total + 1
          n = CInt(n) \ 2
      End While
      Dim p = 3!
      While (p * p) <= n
          count = 1
          While (CInt(n) Mod CInt(p)) = 0
              count = count + 1
              n = CInt(n) \ CInt(p)
          End While
          p = p + 2
          total = total * count
      End While
      If n > 1 Then total = total * 2
      DivisorCount = total
  End Function
  
End Module
