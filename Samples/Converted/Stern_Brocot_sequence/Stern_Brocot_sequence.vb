' Converted from QBasic by qb2vb.

Module Program

  Const max As Short = 2000
  Dim stern(max + 2) As Single

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i!
    ' CONST max = 2000
    ' DIM SHARED stern(max + 2)
    
    SternBrocot()
    
    PRINT("The first 15 are: ", True)
    For i = 1 To 15
        PRINT(Str$(stern(CInt(i))) & " " & " ", True)
    Next i
    
    PRINT() : PRINT()
    PRINT("  Index   First nr.")
    Dim d = 1!
    For i = 1 To max
        If stern(CInt(i)) = d Then
            PRINT_USING(" ######", i, stern(CInt(i)))
            d = d + 1
            If d = 11 Then d = 100
            If d = 101 Then Exit For
            i = 0
        End If
    Next i
    
    PRINT() : PRINT()
    d = 0
    For i = 1 To 1000
        If gcd(stern(CInt(i)), stern(CInt(i + 1))) <> 1 Then
            d = gcd(stern(CInt(i)), stern(CInt(i + 1)))
            Exit For
        End If
    Next i
    
    If d <> 0 Then
        PRINT("GCD of two consecutive members of the series up to the 1000th member is 1")
    Else
        PRINT("The GCD for index " & Str$(i) & " " & " and " & Str$(i + 1) & " " & " = " & Str$(d) & " ")
    End If
  End Sub

  
  Function gcd(ByRef x As Single, ByRef y As Single) As Single
    Dim t!
      While y <> 0
          t = y
          y = CInt(x) Mod CInt(y)
          x = t
      End While
      gcd = x
  End Function
  
  
  Sub SternBrocot()
    Dim i!
    Dim n!
    Dim ub!
      stern(1) = 1
      stern(2) = 1
  
      i = 2 : n = 2 : ub = UBound(stern)
  
      Do While i < ub
          i = i + 1
          stern(CInt(i)) = stern(CInt(n)) + stern(CInt(n - 1))
          i = i + 1
          stern(CInt(i)) = stern(CInt(n))
          n = n + 1
      Loop
  End Sub
  
End Module
