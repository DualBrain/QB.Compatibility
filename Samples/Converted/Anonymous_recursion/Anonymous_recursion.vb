' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION Fibonacci! (num!)
    
    PRINT(Str$(Fibonacci(20)) & " ")
    PRINT(Str$(Fibonacci(30)) & " ")
    PRINT(Str$(Fibonacci(-10)) & " ")
    PRINT(Str$(Fibonacci(10)) & " ")
    End
  End Sub

  
  Function Fibonacci(ByRef num As Single) As Single
    If num < 0 Then
      PRINT("Invalid argument: ", True)
      Fibonacci = num
    End If
  
    If num < 2 Then
      Fibonacci = num
    Else
      Fibonacci = Fibonacci(num - 1) + Fibonacci(num - 2)
    End If
  End Function
  
End Module
