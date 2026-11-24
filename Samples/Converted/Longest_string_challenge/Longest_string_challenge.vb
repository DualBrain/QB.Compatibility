' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim test$ = ""
    Dim test1$ = ""
    Dim test2$ = ""
    DATA("a", "bb", "ccc", "ddd", "ee", "f", "ggg", "~ :")
    
    Do
       READ(test$)
       If test$ = "~" Then Exit Do
       If Len(test$) > Len(test1$) Then
          test1$ = test$
          test2$ = test1$ + QBChr(10)
       ElseIf Len(test$) = Len(test1$) Then
          test2$ = (test2$ + test$) + QBChr(10)
       End If
    Loop
    
    PRINT((test2$))
    
    ' DATA "a", "bb", "ccc", "ddd", "ee", "f", "ggg", "~" : ' la tilde es solo para mantener el código compacto   (DATA is listed at the start of the program)
  End Sub

End Module
