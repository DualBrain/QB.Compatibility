' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim num As Integer  ''comment this line for True BASIC 
    PRINT("First 20 Cullen numbers:")
    
    For n = 1! To 20!
        num = CInt((n * (2 ^ n)) + 1)
        PRINT(Str$(num) & " ", True)
    Next n
    
    PRINT()
    PRINT()
    PRINT("First 20 Woodall numbers:")
    
    For n = 1! To 20!
        num = CInt((n * (2 ^ n)) - 1)
        PRINT(Str$(num) & " ", True)
    Next n
    End
  End Sub

End Module
