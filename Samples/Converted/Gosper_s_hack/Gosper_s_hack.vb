' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION gospersHack! (x AS LONG)
    
    Dim starts(3) As Short
    starts(0) = 1
    starts(1) = 3
    starts(2) = 7
    starts(3) = 15
    
    Dim i As Short, j As Short
    Dim x As Integer
    Dim results(9) As String
    
    For i = 0 To CShort(UBound(starts))
        x = starts(i)
        For j = 0 To CShort(UBound(results))
            x = CInt(gospersHack(x))
            results(j) = Str$(x)
        Next j
        PRINT(Str$(starts(i)) & " " & ": ", True)
        For j = 0 To CShort(UBound(results))
            PRINT(results(j), True)
            If j < 9 Then PRINT(" ", True)
        Next j
        PRINT()
    Next i
  End Sub

  
  Function gospersHack(ByRef x As Integer) As Single
      Dim c As Integer, r As Integer
      c = x And (-x)
      r = x + c
      gospersHack = (((r Xor x) \ 4) \ c) Or r
  End Function
  
End Module
