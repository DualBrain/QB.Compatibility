' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    iterate()
    End
  End Sub

  Sub iterate()
    Dim phi1!
    Dim diff!
      Dim iter = 0!
      Dim phi0! = 1!
      Do
          phi1 = 1! + (1! / phi0)
          diff = Abs(phi1 - phi0)
          phi0 = phi1
          iter = iter + 1
      Loop Until (.00001! > diff)
  
      PRINT("Result: " & Str$(phi1) & " " & " after " & Str$(iter) & " " & " iterations")
      PRINT("The error is approximately " & Str$(CSng(phi1 - (.5! * (1! + Sqrt(5!))))) & " ")
  End Sub
  
End Module
