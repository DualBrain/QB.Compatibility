' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim s$ = ""
    Dim letter$ = ""
    Dim char_$ = ""
    CLS()
    INPUT("Enter a string: ", s$, False)
    Dim ans$ = ""
    For a! = 1 To Len(s$)
            letter$ = Mid$(s$, CInt(a), 1)
            If (letter$ >= "A") And (letter$ <= "Z") Then
                    char_$ = QBChr(Asc(letter$) + 13)
                    If char_$ > "Z" Then char_$ = QBChr(Asc(char_$) - 26)
            ElseIf (letter$ >= "a") And (letter$ <= "z") Then
                    char_$ = QBChr(Asc(letter$) + 13)
                    If char_$ > "z" Then char_$ = QBChr(Asc(char_$) - 26)
            Else
                    char_$ = letter$
            End If
            ans$ = ans$ + char_$
    Next a
    PRINT(ans$)
  End Sub

End Module
