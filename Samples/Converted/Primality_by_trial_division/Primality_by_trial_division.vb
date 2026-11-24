' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' Primality by trial division
    
    ' Test and display primes 1 .. 50
    ' DECLARE FUNCTION prime% (n!)
    For n = 1! To 50!
      If prime(n) = 1 Then PRINT(Str$(n) & " ", True)
    Next n
  End Sub

  
  Function prime(ByRef n!) As Short
    Static i As Short
    If n! = 2 Then
      prime = 1
    ElseIf (n! <= 1) Or ((CInt(n!) Mod 2) = 0) Then
      prime = 0
    Else
      prime = 1
      For i = 3 To CShort(Int(Sqrt(n!))) Step 2
        If (CInt(n!) Mod i) = 0 Then
          prime = 0
          Exit Function
        End If
      Next i
    End If
  End Function
  
End Module
