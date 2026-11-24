' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim T!
    PRINT("The tau functions for the first 100 positive integers are:") : PRINT()
    
    For N = 1! To 100!
        If N < 3 Then
            T = N
        Else
            T = 2
            For A! = 2 To CInt((N + 1)) \ 2
                If (CInt(N) Mod CInt(A)) = 0 Then T = T + 1
            Next A
        End If
        PRINT_USING("###", T)
        If (CInt(N) Mod 10) = 0 Then PRINT()
    Next N
    End
  End Sub

End Module
