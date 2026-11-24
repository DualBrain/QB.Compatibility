' Converted from QBasic by qb2vb.

Module Program

  Const limite As Short = 10000

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' CONST limite = 10000
    Dim Q(limite) As Single
    Q(1) = 1
    Q(2) = 1
    
    Dim cont = 0!
    For i! = 3 To limite
        Q(CInt(i)) = Q(CInt(i - Q(CInt(i - 1)))) + Q(CInt(i - Q(CInt(i - 2))))
        If Q(CInt(i)) < Q(CInt(i - 1)) Then cont = cont + 1
    Next i
    
    PRINT("First 10 terms: ", True)
    For i = 1! To 10!
        PRINT(Str$(Q(CInt(i))) & " " & " ", True)
    Next i
    
    PRINT()
    PRINT("Term 1000: " & Str$(Q(1000)) & " ")
    PRINT("Terms less than preceding in first 100k: " & Str$(cont) & " ")
  End Sub

End Module
