' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Ar$ = ""
    Dim I!
    Dim P$ = ""
    Dim Q!
    Dim Q_Sng!(10)
    Dim K!
    Dim K_Sng!(10)
    Dim B!
    Dim B_Sng!(10)
    Dim N!
    Dim N_Sng!(10)
    Dim R!
    Dim R_Sng!(10)
    Dim N0!
    Dim N1!
    Dim L!
    Dim D!
    CLS()
    PRINT("Enter start array as seen by white.")
    120: PRINT()
    PRINT("Starting array", True)
    INPUT(Ar$)
    PRINT()
    If Len(Ar$) = 0 Then End
    If Len(Ar$) = 8 Then GoTo 170
    PRINT("Array must be 8 pieces.") : GoTo 120
    
    170: For I = 1 To 8
        P$ = Mid$(Ar$, CInt(I), 1)
        If (P$ = "Q") Or (P$ = "q") Then Q_Sng(CInt(Q)) = I : Q = Q + 1 : GoTo 250
        If (P$ = "K") Or (P$ = "k") Then K_Sng(CInt(K)) = I : K = K + 1 : GoTo 250
        If (P$ = "B") Or (P$ = "b") Then B_Sng(CInt(B)) = I : B = B + 1 : GoTo 250
        If (P$ = "N") Or (P$ = "n") Then N_Sng(CInt(N)) = I : N = N + 1 : GoTo 250
        If (P$ = "R") Or (P$ = "r") Then R_Sng(CInt(R)) = I : R = R + 1 : GoTo 250
        PRINT("Illegal piece '" & P$ & "'.") : GoTo 120
    250: Next I
    
    If K <> 1 Then PRINT("There must be exactly one King.") : GoTo 120
    If Q <> 1 Then PRINT("There must be exactly one Queen.") : GoTo 120
    If B <> 2 Then PRINT("There must be exactly two Bishops.") : GoTo 120
    If N <> 2 Then PRINT("There must be exactly two Knights.") : GoTo 120
    If R <> 2 Then PRINT("There must be exactly two Rooks.") : GoTo 120
    If (K_Sng(0) > R_Sng(0)) And (K_Sng(0) < R_Sng(1)) Then GoTo 330
    PRINT("King must be between the Rooks.") : GoTo 120
    
    330: If (CInt(B_Sng(0)) And 1) <> (CInt(B_Sng(1)) And 1) Then GoTo 350
    PRINT("Bishops must be on opposite colors.") : GoTo 120
    
    350: For I = 0 To 1
        N = N_Sng(CInt(I))
        If N_Sng(CInt(I)) > Q_Sng(0) Then N = N - 1
        For J = 0! To 1!
            If N_Sng(CInt(I)) > B_Sng(CInt(J)) Then N = N - 1
        Next J
        N_Sng(CInt(I)) = N
    Next I
    N0 = 1 : N1 = 2
    
    For N = 0 To 9
        If (N0 = N_Sng(0)) And (N1 = N_Sng(1)) Then GoTo 490
        N1 = N1 + 1
        If N1 > 5 Then N0 = N0 + 1 : N1 = N0 + 1
    Next N
    490: Q = Q_Sng(0) - 1
    
    For I = 0 To 1
        If Q_Sng(0) > B_Sng(CInt(I)) Then Q = Q - 1
    Next I
    
    For I = 0 To 1
        B = B_Sng(CInt(I)) - 1
        If (CInt(B) And 1) <> 0 Then L = Int(B / 2)
        If (CInt(B) And 1) = 0 Then D = B / 2
    Next I
    PRINT("SP-ID =" & Str$((((96 * N) + (16 * Q)) + (4 * D)) + L) & " ")
    End
  End Sub

End Module
