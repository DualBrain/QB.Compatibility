' Converted from QBasic by qb2vb.

Module Program

  Dim arabic(12) As Single
  Dim roman$(12)

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(1000, "M", 900, "CM", 500, "D", 400, "CD", 100, "C", 90, "XC")
    DATA(50, "L", 40, "XL", 10, "X", 9, "IX", 5, "V", 4, "IV", 1, "I")
    
    ' DIM SHARED arabic(0 TO 12)
    ' DIM SHARED roman$(0 TO 12)
    
    For i = 0! To 12!
        READ(arabic(CInt(i))) : READ(roman$(CInt(i)))
    Next i
    
    ' DATA 1000, "M", 900, "CM", 500, "D", 400, "CD", 100, "C", 90, "XC"   (DATA is listed at the start of the program)
    ' DATA 50, "L", 40, "XL", 10, "X", 9, "IX", 5, "V",  4, "IV", 1, "I"   (DATA is listed at the start of the program)
    
    'Testing
    PRINT("2009 = " & toRoman$(2009))
    PRINT("1666 = " & toRoman$(1666))
    PRINT("3888 = " & toRoman$(3888))
  End Sub

  
  Function toRoman$(ByRef value As Single)
    Dim result$
      result$ = ""
      For i = 0! To 12!
          Do While value >= arabic(CInt(i))
             result$ = result$ + roman$(CInt(i))
             value = value - arabic(CInt(i))
          Loop
      Next i
      toRoman$ = result$
  End Function
  
End Module
