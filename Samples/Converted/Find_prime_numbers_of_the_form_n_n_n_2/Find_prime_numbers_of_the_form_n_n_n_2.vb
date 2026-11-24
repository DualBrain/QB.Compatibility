' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION isPrime% (ValorEval!)
    
    For n = 1! To 200!
        If isPrime(CSng((n ^ 3) + 2)) <> 0 Then PRINT(Str$(n) & " " & vbTab & Str$(CSng((n ^ 3) + 2)) & " ")
    Next n
  End Sub

  
  Function isPrime(ByRef ValorEval!) As Short
      Static i As Short
      If ValorEval! = 2 Then
          isPrime = 1
      ElseIf (ValorEval! <= 1) Or ((CInt(ValorEval!) Mod 2) = 0) Then
          isPrime = 0
      Else
          isPrime = 1
          For i = 3 To CShort(Int(Sqrt(ValorEval!))) Step 2
              If (CInt(ValorEval!) Mod i) = 0 Then
                  isPrime = 0
                  Exit Function
              End If
          Next i
      End If
  End Function
  
End Module
