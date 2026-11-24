' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    CLS()
    sample((100))
    sample((1000))
    sample((10000))
    End
  End Sub

  Sub sample(ByRef n As Single)
    Dim z!
    Dim samp(CInt(n)) As Single
    For i! = 1 To n
      samp(CInt(i)) = Rnd(1)
    Next i
    ' calculate mean, standard deviation
    Dim sum = 0!
    Dim sumsq = 0!
    For i! = 1 To n
      sum = sum + samp(CInt(i))
      sumsq = CSng(sumsq + (samp(CInt(i)) ^ 2))
    Next i
    PRINT("Sample size " & Str$(n) & " ")
    Dim mean! = sum / n
    PRINT()
    PRINT("  Mean    = " & Str$(mean) & " ")
    PRINT("  Std Dev = " & Str$(CSng(((sumsq / n) - (mean ^ 2)) ^ .5!)) & " ")
    PRINT()
    '------- Show histogram
    Dim scal = 10!
    Dim bins(CInt(scal)) As Single
    For i! = 1 To n
      z = Int(scal * samp(CInt(i)))
      bins(CInt(z)) = bins(CInt(z)) + 1
    Next i
    For b! = 0 To scal - 1
      PRINT("  " & Str$(b) & " " & " : ", True)
      For j! = 1 To (Int(scal * bins(CInt(b))) / n) * 70
        PRINT("*", True)
      Next j
      PRINT()
    Next b
    PRINT()
  End Sub
  
End Module
