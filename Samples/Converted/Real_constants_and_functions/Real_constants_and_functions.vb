' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim x!
    Dim y!
    
    PRINT("e     = " & Str$(CSng(Exp(1))) & " ")  ' e  not available
    PRINT("PI    = " & Str$(CSng(4 * Atan(1))) & " ")  ' pi not available
    
    x = 12.345! : y = 1.23!
    
    PRINT("sqrt  = " & Str$(CSng(Sqrt(x))) & " " & vbTab & Str$(CSng(x ^ .5!)) & " ")  ' square root- NB the unusual name
    PRINT("ln    = " & Str$(CSng(Log(x))) & " ")  ' natural logarithm base e
    PRINT("log10 = " & Str$(CSng(Log(x) / 2.303!)) & " ")  ' base 10 logarithm
    PRINT("log   = " & Str$(CSng(Log(x) / Log(y))) & " ")  ' arbitrary base logarithm
    PRINT("exp   = " & Str$(CSng(Exp(x))) & " ")  ' exponential
    PRINT("abs   = " & Str$(Abs(-x)) & " ")  ' absolute value
    PRINT("floor = " & Str$(floor(x)) & " ")  ' floor
    PRINT("ceil  = " & Str$(ceiling(x)) & " ")  ' ceiling
    PRINT("power = " & Str$(CSng(x ^ y)) & " ")  ' power
    End
  End Sub

  'ceiling and floor easily implemented as functions.
  'e & pi not available- calculate as shown.
  
  Function ceiling(ByRef x As Single) As Single
      If x < 0 Then ceiling = Int(x) Else ceiling = Int(x) + 1
  End Function
  
  
  Function floor(ByRef x As Single) As Single
      If x > 0 Then
          floor = Int(x)
      Else
          If x <> Int(x) Then floor = Int(x) - 1 Else floor = Int(x)
      End If
  End Function
  
End Module
