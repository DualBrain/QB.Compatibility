' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Pal!
    Dim dateTest$ = ""
    Dim total = 0!
    
    PRINT("Siguientes 15 fechas palindr¢micas al 2020-02-02:")
    For anno = 2021! To 9999!
        dateTest$ = LTrim$(Str$(anno))
        For mes = 1! To 12!
            If mes < 10 Then dateTest$ = dateTest$ + "0"
            dateTest$ = dateTest$ + LTrim$(Str$(mes))
            For dia = 1! To 31!
                If (mes = 2) And (dia > 28) Then Exit For
                If ((((mes = 4) Or (mes = 6)) Or (mes = 9)) Or (mes = 11)) And (dia > 30) Then Exit For
                If dia < 10 Then dateTest$ = dateTest$ + "0"
                dateTest$ = dateTest$ + LTrim$(Str$(dia))
                For Pal = 1 To 4
                    If Mid$(dateTest$, CInt(Pal), 1) <> Mid$(dateTest$, CInt(9 - Pal), 1) Then Exit For
                Next Pal
                If Pal = 5 Then
                   total = total + 1
                   If total <= 15 Then PRINT(Left$(dateTest$, 4) & "-" & Mid$(dateTest$, 5, 2) & "-" & Right$(dateTest$, 2))
                End If
                If total > 15 Then
                   Exit For : Exit For : Exit For
                End If
                dateTest$ = Left$(dateTest$, 6)
            Next dia
            dateTest$ = Left$(dateTest$, 4)
        Next mes
        dateTest$ = ""
    Next anno
    End
  End Sub

End Module
