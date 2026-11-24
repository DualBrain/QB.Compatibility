' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DEF FNmultiply# (a AS DOUBLE, b AS DOUBLE) = a * b
    
    PRINT(Str$(multiply#(3, 1.23456!)) & " ")
    PRINT(Str$(FNmultiply#(3, 1.23456!)) & " ")
  End Sub

  'This function could either be used for all numeric types 
  '(as they are implicitly convertible to Double)
  Function multiply#(ByRef a As Double, ByRef b As Double)
      multiply# = a * b
  End Function
  
  '
  ' Alternatively, it can be expressed in abbreviated form :
  '
  Function FNmultiply#(ByVal a As Double, ByVal b As Double)
    Return a * b
  End Function
  
End Module
