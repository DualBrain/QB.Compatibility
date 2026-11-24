' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim c$ = ""
    Dim mensaje$ = "If Peter Piper picked a pack of pickled peppers" + ", how many pickled peppers did Peter Piper pick?"
    Dim textofinal$ = ""
    
    For i! = 1 To Len(mensaje$)
        c$ = Mid$(mensaje$, CInt(i), 1)
        Select Case c$
        Case "a", "e", "i", "o", "u", "A", "E", "I", "O", "U"
            ' Continue For
        Case Else
            textofinal$ = textofinal$ + c$
        End Select
    Next i
    
    PRINT(textofinal$)
    End
  End Sub

End Module
