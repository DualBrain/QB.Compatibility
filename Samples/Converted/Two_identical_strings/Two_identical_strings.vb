' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim k!
    Dim n!
    Dim p!
    
    k = 0 : n = 1 : p = 2
    Do
       If n >= p Then p = p + p
       k = n + (n * p)
       If k < 1000 Then
          PRINT(Str$(k) & " " & " = " & tobin$(k))
       Else
          Exit Do
       End If
       n = n + 1
    Loop
  End Sub

  Function tobin$(ByRef d As Single)
    Dim r!
      Dim s$ = ""
      Do While d <> 0
         r = CInt(d) Mod 2
         s$ = Str$(r) + s$
         d = CInt(d) \ 2
      Loop
      tobin$ = s$
  End Function
  
End Module
