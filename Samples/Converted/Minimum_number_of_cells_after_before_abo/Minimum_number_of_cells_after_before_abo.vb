' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION min! (a!, b!)
    ' DECLARE SUB minab (n!)
    
    CLS()
    minab((10))
    End
  End Sub

  
  Function min(ByRef a As Single, ByRef b As Single) As Single
  	If a <= b Then min = a Else min = b
  End Function
  
  
  Sub minab(ByRef n As Single)
  	For i! = 1 To n
  		For j! = 1 To n
  			PRINT_USING("## ", min(min(i - 1, n - i), min(j - 1, n - j)))
  		Next j
  		PRINT()
  	Next i
  End Sub
  
End Module
