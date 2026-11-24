' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim K!
    Dim C!
    
    Dim S$ = "1223334444"
    Dim U$ = ""
    For I! = 1 To Len(S$)
        K = 0
        For J! = 1 To Len(U$)
            If Mid$(U$, CInt(J), 1) = Mid$(S$, CInt(I), 1) Then K = 1
        Next J
        If K = 0 Then U$ = U$ + Mid$(S$, CInt(I), 1)
    Next I
    Dim R(Len(U$) - 1) As Single
    For I! = 1 To Len(U$)
        C = 0
        For J! = 1 To Len(S$)
            If Mid$(U$, CInt(I), 1) = Mid$(S$, CInt(J), 1) Then C = C + 1
        Next J
        R(CInt(I - 1)) = (C / Len(S$)) * L(C / Len(S$))
    Next I
    Dim E = 0!
    For I! = 0 To Len(U$) - 1
        E = E - R(CInt(I))
    Next I
    PRINT(Str$(E) & " ")
    End
  End Sub

  Function L(ByRef X As Single) As Single
      L = CSng(Log(X) / Log(2))
  End Function
  
End Module
