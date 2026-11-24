' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim suma!
    
    Dim n = 0!
    Dim num = 0!
    PRINT("The first 20 pairs of numbers whose sum is prime:")
    Do
        n = n + 1
        suma = (2 * n) + 1
        If isPrime(suma) <> 0 Then
            num = num + 1
            If num < 21 Then
                PRINT_USING("## + ## = ##", n, (n + 1), suma)
            Else
                Exit Do
            End If
        End If
    Loop
    End
  End Sub

  Function isPrime(ByRef ValorEval As Single) As Single
      If ValorEval <= 1 Then isPrime = 0
      For i! = 2 To CSng(Int(Sqrt(ValorEval)))
          If (CInt(ValorEval) Mod CInt(i)) = 0 Then isPrime = 0
      Next i
      isPrime = 1
  End Function
  
End Module
