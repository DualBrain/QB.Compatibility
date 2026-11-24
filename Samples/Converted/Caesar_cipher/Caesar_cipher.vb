' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim dec$ = ""
    Dim tipo$ = ""
    Dim llave!
    Dim cadena$ = ""
    Dim longitud!
    Dim iTemp!
    dec$ = ""
    tipo$ = "cleartext "
    
    PRINT("If decrypting enter 'd' -- else press enter ", True)
    INPUT(dec$)
    PRINT("Enter offset ", True)
    INPUT(llave)
    
    If dec$ = "d" Then
        llave = 26 - llave
        tipo$ = "ciphertext "
    End If
    
    PRINT("Enter " & tipo$, True)
    INPUT(cadena$)
    
    cadena$ = UCase$(cadena$)
    longitud = Len(cadena$)
    
    For i! = 1 To longitud
        iTemp = Asc(Mid$(cadena$, CInt(i), 1))  'QBasic
        'LET itemp = ORD((cadena$)[i:i+1-1][1:1]) 'True BASIC
    
        If (iTemp > 64) And (iTemp < 91) Then
            iTemp = CInt(((iTemp - 65) + llave)) Mod 26  'QBasic
            'LET iTemp = MOD(((iTemp - 65) + llave), 26) 'True BASIC
            PRINT(QBChr(CInt(iTemp + 65)), True)
        Else
            PRINT(QBChr(CInt(iTemp)), True)
        End If
    Next i
    End
  End Sub

End Module
