' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    PRINT("The first 20 Hamming numbers are :")
    For i = 1! To 20!
        PRINT(Str$(Hamming(i)) & " " & " ", True)
    Next i
    
    PRINT()
    PRINT("H( 1691) = " & Str$(Hamming(1691)) & " ")
  End Sub

  Function min(ByRef a As Single, ByRef b As Single) As Single
      If a < b Then min = a Else min = b
  End Function
  
  
  Function Hamming(ByRef limit As Single) As Single
      Dim h(CInt(limit)) As Single
  
      h(0) = 1
      Dim x2 = 2!
      Dim x3 = 3!
      Dim x5 = 5!
      Dim i = 0!
      Dim j = 0!
      Dim k = 0!
      For n! = 1 To limit
          h(CInt(n)) = min(x2, min(x3, x5))
          If x2 = h(CInt(n)) Then
              i = i + 1
              x2 = 2 * h(CInt(i))
          End If
          If x3 = h(CInt(n)) Then
              j = j + 1
              x3 = 3 * h(CInt(j))
          End If
          If x5 = h(CInt(n)) Then
              k = k + 1
              x5 = 5 * h(CInt(k))
          End If
      Next n
      Hamming = h(CInt(limit - 1))
  End Function
  
End Module
