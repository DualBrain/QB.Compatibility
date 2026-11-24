' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION diy% (y AS INTEGER)
    ' DECLARE FUNCTION isLeapYear% (yr AS INTEGER)
    ' DECLARE FUNCTION year% (date AS STRING)
    
    PRINT(Str$(isLeapYear(year((DateString)))) & " ")
  End Sub

  
  Function diy(ByRef y As Short) As Short
      If (y Mod 4) <> 0 Then
          diy = 365
      ElseIf (y Mod 100) <> 0 Then
          diy = 366
      ElseIf (y Mod 400) <> 0 Then
          diy = 365
      Else
          diy = 366
      End If
  End Function
  
  
  Function isLeapYear(ByRef yr As Short) As Short
      isLeapYear = CShort((366 = diy(yr)))
  End Function
  
  
  Function year(ByRef [date] As String) As Short
      year = CShort(Val(Right$([date], 4)))
  End Function
  
End Module
