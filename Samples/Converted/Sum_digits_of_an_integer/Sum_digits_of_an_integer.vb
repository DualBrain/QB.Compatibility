' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    PRINT("The sums of the digits are:")
    PRINT()
    PRINT("1    base 10 :" & Str$(SumDigits(1, 10)) & " ")
    PRINT("1234 base 10 :" & Str$(SumDigits(1234, 10)) & " ")
    PRINT("fe   base 16 :" & Str$(SumDigits(254, 16)) & " ")
    PRINT("f0e  base 16 :" & Str$(SumDigits(3854, 16)) & " ")
    End
  End Sub

  Function SumDigits(ByRef number As Single, ByRef nBase As Single) As Single
      If number < 0 Then number = -number
      If nBase < 2 Then nBase = 2
      Dim sum = 0!
  
      Do While number > 0
         sum = sum + (CInt(number) Mod CInt(nBase))
         number = CInt(number) \ CInt(nBase)
      Loop
  
      SumDigits = sum
  End Function
  
End Module
