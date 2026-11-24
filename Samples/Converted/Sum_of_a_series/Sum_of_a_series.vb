' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim pi# = 4 * Atan(1)
    PRINT("s(1000) = " & Str$(sumSeries#(1000)) & " ")
    PRINT("zeta(2) = " & Str$((pi# * pi#) / 6) & " ")
    End
  End Sub

  Function sumSeries#(ByRef n As Single)
    Dim sum#
      If n = 0 Then sumSeries# = 0
      For k! = 1 To n
          sum# = sum# + (1! / (k * k))
      Next
      sumSeries# = sum#
  End Function
  
End Module
