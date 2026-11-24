' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim n!
    Dim m!
    
    INPUT("Enter n comb m.  ", n, m, False)
    
    Dim outstr$ = ""
    iterate(outstr$, 0, m - 1, n - 1)
    End
  End Sub

  Sub iterate(ByRef curr$, ByRef start As Single, ByRef stp As Single, ByRef depth As Single)
    For i! = start To stp
      If depth = 0 Then PRINT(((curr$ + " ") + Str$(i)))
      iterate((curr$ + " ") + Str$(i), i + 1, stp, depth - 1)
    Next i
  End Sub
  
End Module
