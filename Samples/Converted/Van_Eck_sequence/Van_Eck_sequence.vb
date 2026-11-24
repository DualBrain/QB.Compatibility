' Converted from QBasic by qb2vb.

Module Program

  Const limite As Short = 1000

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' CONST limite = 1000
    Dim a(limite) As Single
    
    For n! = 0 To limite - 1
        For m! = n - 1 To 0 Step -1
            If a(CInt(m)) = a(CInt(n)) Then
                a(CInt(n + 1)) = n - m
                Exit For
            End If
        Next m
    Next n
    
    PRINT("Secuencia de Van Eck:" & QBChr(10))
    PRINT("Primeros 10 terminos: ", True)
    For i = 0! To 9!
        PRINT(Str$(a(CInt(i))) & " ", True)
    Next i
    PRINT(QBChr(10) & "Terminos 991 al 1000: ", True)
    For i = 990! To 999!
        PRINT(Str$(a(CInt(i))) & " ", True)
    Next i
    End
  End Sub

End Module
