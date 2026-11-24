' Converted from QBasic by qb2vb.

Module Program

  Const PI# = 3.141592653589793#

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION Atan2# (y AS DOUBLE, x AS DOUBLE)
    ' DECLARE FUNCTION MeanAngle# (angles#())
    ' CONST PI# = 3.141592653589793#
    
    Dim angles1#(2)
    angles1#(1) = 350
    angles1#(2) = 10
    
    Dim angles2#(4)
    angles2#(1) = 90
    angles2#(2) = 180
    angles2#(3) = 270
    angles2#(4) = 360
    
    Dim angles3#(3)
    angles3#(1) = 10
    angles3#(2) = 20
    angles3#(3) = 30
    
    PRINT_USING("Mean for angles 1 is : ####.## degrees", MeanAngle#(angles1#))
    PRINT_USING("Mean for angles 2 is : ####.## degrees", MeanAngle#(angles2#))
    PRINT_USING("Mean for angles 3 is : ####.## degrees", MeanAngle#(angles3#))
  End Sub

  
  Function Atan2#(ByRef y As Double, ByRef x As Double)
      If x > 0 Then
          Atan2# = Atan(y / x)
      ElseIf x < 0 Then
          If y >= 0 Then
              Atan2# = Atan(y / x) + PI#
          Else
              Atan2# = Atan(y / x) - PI#
          End If
      Else  ' x = 0
          If y > 0 Then
              Atan2# = PI# / 2
          ElseIf y < 0 Then
              Atan2# = (-PI#) / 2
          Else  ' y = 0
              Atan2# = 0
          End If
      End If
  End Function
  
  
  Function MeanAngle#(ByRef angles#())
      Dim length# = (UBound(angles#) - LBound(angles#)) + 1
      Dim sinSum# = 0!
      Dim cosSum# = 0!
  
      For i As Short = CShort(LBound(angles#)) To CShort(UBound(angles#))
          sinSum# = sinSum# + Sin((angles#(i) * PI#) / 180!)
          cosSum# = cosSum# + Cos((angles#(i) * PI#) / 180!)
      Next i
  
      MeanAngle# = (Atan2#(sinSum# / length#, cosSum# / length#) * 180!) / PI#
  End Function
  
End Module
