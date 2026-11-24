' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB fib (a() AS INTEGER)
    
    CLS()
    PRINT("  fibonacci =>", True)
    Dim a(2) As Short
    a(1) = 1 : a(2) = 1
    fib(a)
    PRINT(" tribonacci =>", True)
    Dim b(3) As Short
    b(1) = 1 : b(2) = 1 : b(3) = 2
    fib(b)
    PRINT(" tetranacci =>", True)
    Dim c(4) As Short
    c(1) = 1 : c(2) = 1 : c(3) = 2 : c(4) = 4
    fib(c)
    PRINT("      lucas =>", True)
    Dim d(2) As Short
    d(1) = 2 : d(2) = 1
    fib(d)
    End
  End Sub

  
  Sub fib(ByRef a() As Short)
      Dim f(24) As Single
      Dim b = 0!
      For x! = 1 To UBound(a)
          b = b + 1
          f(CInt(x)) = a(CInt(x))
      Next x
      For i! = b To 12 + b
          PRINT_USING_NOCR("#### ", f(CInt((i - b) + 1)))
          For j! = ((i - b) + 1) To i
              f(CInt(i + 1)) = f(CInt(i + 1)) + f(CInt(j))
          Next j
      Next i
  End Sub
  
End Module
