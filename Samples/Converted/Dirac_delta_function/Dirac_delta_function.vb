' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION diracDelta$ (x AS DOUBLE)
    
    PRINT(diracDelta$(0))
    PRINT(diracDelta$(1))
    End
  End Sub

  
  Function diracDelta$(ByRef x As Double)
      diracDelta$ = "0"
      If x = 0 Then diracDelta$ = "Infinity"
  End Function
  
End Module
