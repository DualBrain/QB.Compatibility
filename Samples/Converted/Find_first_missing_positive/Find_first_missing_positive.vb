' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(1, 2, 0)
    DATA(3, 4, -1, 1)
    DATA(7, 8, 9, 11, 12)
    
    ' DECLARE FUNCTION isin (n(), k)
    ' DECLARE FUNCTION fmp (n())
    
    Dim a(3) As Single
    For x! = 1 To UBound(a) : READ(a(CInt(x))) : Next x
    Dim b(4) As Single
    For x! = 1 To UBound(b) : READ(b(CInt(x))) : Next x
    Dim c(5) As Single
    For x! = 1 To UBound(c) : READ(c(CInt(x))) : Next x
    
    PRINT(Str$(fmp(a)) & " ")
    PRINT(Str$(fmp(b)) & " ")
    PRINT(Str$(fmp(c)) & " ")
    SLEEP()
    End
    
    ' DATA 1,2,0   (DATA is listed at the start of the program)
    ' DATA 3,4,-1,1   (DATA is listed at the start of the program)
    ' DATA 7,8,9,11,12   (DATA is listed at the start of the program)
  End Sub

  
  Function fmp(ByRef n() As Single) As Single
      Dim j = 1!
      While isin(n, j) <> 0
          j = j + 1
      End While
      fmp = j
  End Function
  
  
  Function isin(ByRef n() As Single, ByRef k As Single) As Single
      For i! = LBound(n) To UBound(n)
          If n(CInt(i)) = k Then isin = 1
      Next i
  End Function
  
End Module
