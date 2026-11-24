' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim ub!
    Dim n!
    
    Dim b$ = "010000000000100000000010000000010000000100000010000010000100010010"
    Dim tests$(8, 2)
    tests$(1, 1) = "1000" : tests$(1, 2) = "2"
    tests$(2, 1) = "0100" : tests$(2, 2) = "2"
    tests$(3, 1) = "0010" : tests$(3, 2) = "2"
    tests$(4, 1) = "0000" : tests$(4, 2) = "2"
    tests$(5, 1) = b$ : tests$(5, 2) = "0"
    tests$(6, 1) = b$ : tests$(6, 2) = "1"
    tests$(7, 1) = b$ : tests$(7, 2) = "2"
    tests$(8, 1) = b$ : tests$(8, 2) = "3"
    For k = 1! To 8!
        Dim bits(Len(tests$(CInt(k), 1))) As Single
        For i! = 1 To Len(tests$(CInt(k), 1))
            bits(CInt(i)) = CSng(Val(Mid$(tests$(CInt(k), 1), CInt(i), 1)))
        Next i
        ub = UBound(bits)
        n = CSng(Val(tests$(CInt(k), 2)))
        PRINT_USING("n = #; Width e = ##:", n, ub)
        PRINT("    Input b: " & tests$(CInt(k), 1))
        setRightBits(bits, ub, n)
        PRINT("     Result:", True)
        For i! = 1 To ub
            PRINT(Str$(bits(CInt(i))) & " ", True)
        Next i
        PRINT(QBChr(10))
    Next k
    End
  End Sub

  Sub setRightBits(ByRef bits() As Single, ByRef e As Single, ByRef n As Single)
    Dim j!
      If (e = 0) Or (n <= 0) Then Exit Sub
      Dim bits2(CInt(e)) As Single
      For i! = 1 To e : bits2(CInt(i)) = bits(CInt(i)) : Next
      For i! = 1 To e - 1
          If bits(CInt(i)) = 1 Then
              j = i + 1
              While (j <= (i + n)) And (j <= e)
                  bits2(CInt(j)) = 1
                  j = j + 1
              End While
          End If
      Next i
      For i! = 1 To e : bits(CInt(i)) = bits2(CInt(i)) : Next
  End Sub
  
End Module
