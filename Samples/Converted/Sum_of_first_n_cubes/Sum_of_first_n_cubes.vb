' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim sumCubos%
    ' DEFLNG A-Z
    
    Dim fila = 0%
    Dim lenCubos = 49%
    
    CLS()
    PRINT(("Suma de N cubos para n = [0..49]" + QBChr(10)))
    
    For n% = 0 To lenCubos
        sumCubos = 0
        For m% = 1 To n
            sumCubos = CInt(sumCubos + (m ^ 3))
        Next m
        fila = fila + 1
        PRINT_USING(" ####### ", sumCubos)
        If (fila Mod 5) = 0 Then PRINT()
    Next n
    
    PRINT((QBChr(13) + "Encontrados") & Str$(fila) & " " & "cubos.")
    End
  End Sub

End Module
