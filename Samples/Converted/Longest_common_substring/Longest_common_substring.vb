' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    LCS("thisisatest", "testing123testing")
    End
  End Sub

  
  Sub LCS(ByRef a$, ByRef b$)
      If (Len(a$) = 0) Or (Len(b$) = 0) Then PRINT("") : Exit Sub
      While Len(b$) <> 0
          For j! = Len(b$) To 1 Step -1
              If InStr(a$, Left$(b$, CInt(j))) <> 0 Then PRINT(Left$(b$, CInt(j))) : Exit Sub
          Next j
          b$ = Mid$(b$, 2)
      End While
  End Sub
  
End Module
