' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    PRINT("Primes below 100" & QBChr(10))
    For i = 2! To 100!
        If wilsonprime(i) <> 0 Then PRINT(Str$(i) & " " & "   ", True)
    Next i
  End Sub

  Function wilsonprime(ByRef n As Single) As Single
      Dim fct = 1!
      For i! = 2 To n - 1
          fct = CInt((fct * i)) Mod CInt(n)
      Next i
      If fct = (n - 1) Then wilsonprime = 1 Else wilsonprime = 0
  End Function
  
End Module
