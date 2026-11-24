' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION PadLeft$ (s AS STRING, length AS INTEGER)
    ' DECLARE FUNCTION LunarAdd$ (a AS STRING, b AS STRING)
    ' DECLARE FUNCTION LunarMultiply$ (a AS STRING, b AS STRING)
    ' DECLARE FUNCTION isUnique% (value AS STRING, arr$(), cnt AS INTEGER)
    
    PRINT("     Lunar add: 976 + 348 = " & LunarAdd$("976", "348"))
    PRINT("Lunar multiply: 976 x 348 = " & LunarMultiply$("976", "348"))
    PRINT()
    
    PRINT("     Lunar add: 23 + 321 = " & LunarAdd$("23", "321"))
    PRINT("Lunar multiply: 23 x 321 = " & LunarMultiply$("23", "321"))
    PRINT()
    
    PRINT("     Lunar add: 232 + 35 = " & LunarAdd$("232", "35"))
    PRINT("Lunar multiply: 232 x 35 = " & LunarMultiply$("232", "35"))
    PRINT()
    
    PRINT("     Lunar add: 123 + 32192 + 415 + 8 = " & LunarAdd$(LunarAdd$(LunarAdd$("123", "32192"), "415"), "8"))
    PRINT("Lunar multiply: 123 x 32192 x 415 x 8 = " & LunarMultiply$(LunarMultiply$(LunarMultiply$("123", "32192"), "415"), "8"))
    PRINT()
    
    PRINT("First 20 distinct lunar even numbers:")
    Dim evenNums$(1000)
    Dim cnt As Short, n As Short, i As Short
    Do While cnt < 20
        Dim result As String
        result = LunarMultiply$(Str$(n), "2")
        If isUnique(result, evenNums$, cnt) <> 0 Then
            evenNums$(cnt) = result
            PRINT(result & " ", True)
            cnt = CShort(cnt + 1)
        End If
        n = CShort(n + 1)
    Loop
    
    PRINT()
    PRINT()
    PRINT("First 20 lunar square numbers:")
    For i = 0 To 19
        PRINT(LunarMultiply$(Str$(i), Str$(i)) & " ", True)
    Next i
    
    PRINT()
    PRINT()
    PRINT("First 20 lunar factorial numbers:")
    Dim factorial As String
    factorial = "1"
    PRINT(factorial, True)
    
    For i = 2 To 20
        factorial = LunarMultiply$(factorial, Str$(i))
        PRINT(" " & factorial, True)
    Next i
    
    PRINT()
    PRINT()
    
    Dim prevSquare As String, currSquare As String
    Dim j As Integer
    j = 1
    
    Do
        currSquare = LunarMultiply$(Str$(j), Str$(j))
    
        If (prevSquare <> "") And (Val(currSquare) < Val(prevSquare)) Then
            PRINT("First number whose lunar square is smaller than the previous:" & Str$(j) & " ")
            Exit Do
        End If
    
        prevSquare = currSquare
        j = j + 1
    Loop
    End
  End Sub

  
  
  Function isUnique(ByRef value As String, ByRef arr$(), ByRef cnt As Short) As Short
      Dim i As Short
      For i = 0 To CShort(cnt - 1)
          If arr$(i) = value Then
              isUnique = 0
              Exit Function
          End If
      Next i
      isUnique = -1
  End Function
  
  
  Function LunarAdd$(ByRef a As String, ByRef b As String)
      Dim result As String
      Dim maxLen As Short, i As Short
      Dim digitA As Short, digitB As Short, digit As Short
      If Len(a) > Len(b) Then maxLen = CShort(Len(a)) Else maxLen = CShort(Len(b))
  
      a = PadLeft$(a, maxLen)
      b = PadLeft$(b, maxLen)
  
      For i = 1 To maxLen
          digitA = CShort(Val(Mid$(a, i, 1)))
          digitB = CShort(Val(Mid$(b, i, 1)))
          If digitA > digitB Then digit = digitA Else digit = digitB
          result = result + QBChr(Asc("0") + digit)
      Next i
  
      While (Left$(result, 1) = "0") And (Len(result) > 1)
          result = Right$(result, Len(result) - 1)
      End While
  
      LunarAdd$ = result
  End Function
  
  
  Function LunarMultiply$(ByRef a As String, ByRef b As String)
      If (a = "0") Or (b = "0") Then
          LunarMultiply$ = "0"
          Exit Function
      End If
  
      Dim result As String, tmp As String
      result = "0"
      Dim i As Short, j As Short
      Dim bDigit As Short, aDigit As Short
      For i = CShort(Len(b)) To 1 Step -1
          tmp = ""
          bDigit = CShort(Val(Mid$(b, i, 1)))
  
          For j = 1 To CShort(Len(a))
              aDigit = CShort(Val(Mid$(a, j, 1)))
              If aDigit < bDigit Then
                  tmp = tmp + QBChr(Asc("0") + aDigit)
              Else
                  tmp = tmp + QBChr(Asc("0") + bDigit)
              End If
          Next j
  
          tmp = tmp + QBString(Len(b) - i, "0")
          result = LunarAdd$(result, tmp)
      Next i
  
      While (Left$(result, 1) = "0") And (Len(result) > 1)
          result = Right$(result, Len(result) - 1)
      End While
  
      LunarMultiply$ = result
  End Function
  
  
  Function PadLeft$(ByRef s As String, ByRef length As Short)
      If Len(s) >= length Then
          PadLeft$ = s
      Else
          PadLeft$ = QBString(length - Len(s), "0") + s
      End If
  End Function
  
End Module
