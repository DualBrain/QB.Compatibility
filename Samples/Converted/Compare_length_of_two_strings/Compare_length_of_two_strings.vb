' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    comp("abcd", "123456789")
    End
  End Sub

  Sub comp(ByRef A$, ByRef B$)
      If Len(A$) >= Len(B$) Then
         PRINT(A$ & vbTab & Str$(Len(A$)) & " ")
         PRINT(B$ & vbTab & Str$(Len(B$)) & " ")
      Else
         PRINT(B$ & vbTab & Str$(Len(B$)) & " ")
         PRINT(A$ & vbTab & Str$(Len(A$)) & " ")
      End If
  End Sub
  
End Module
