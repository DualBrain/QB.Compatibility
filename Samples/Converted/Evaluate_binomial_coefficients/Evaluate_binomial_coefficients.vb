' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION factorial! (n AS INTEGER)
    ' DECLARE FUNCTION binomial! (n AS INTEGER, k AS INTEGER)
    
    Dim n As Short, k As Short
    
    For n = 0 To 14
        For k = 0 To n
            PRINT_USING(" ####", binomial!(n, k))
        Next k
        PRINT()
    Next n
    End
  End Sub

  
  Function binomial!(ByRef n As Short, ByRef k As Short)
      ' Special cases
      If (k = 0) Or (k = n) Then binomial! = 1
      If (k < 0) Or (k > n) Then binomial! = 0
  
      ' For efficiency, we use the smallest value of k or (n-k)
      Dim tmp As Short
      tmp = k
      If tmp > (n - tmp) Then tmp = n - tmp
  
      Dim product As Double  ' Double to avoid overflow
      product = 1
  
      For i! = 1 To tmp
          product = (product * ((n - tmp) + i)) / i
      Next i
  
      binomial! = CSng(product)
  End Function
  
  
  Function factorial(ByRef n As Short) As Single
      If n <= 1 Then factorial = 1
  
      Dim product As Double  ' Double to avoid overflow
      product = 1
  
      For i! = 2 To n
          product = product * i
      Next i
  
      factorial = CSng(product)
  End Function
  
End Module
