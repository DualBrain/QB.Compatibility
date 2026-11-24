' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim sFlatter$ = ""
    Dim sComma$ = ""
    Dim sString$ = "[[1], 2, [[3,4], 5], [[[]]], [[[6]]], 7, 8 []]"
    
    For siCount! = 1 To Len(sString$)
        If InStr("[] ,", Mid$(sString$, CInt(siCount), 1)) = 0 Then
            sFlatter$ = (sFlatter$ + sComma$) + Mid$(sString$, CInt(siCount), 1)
            sComma$ = ", "
        End If
    Next siCount
    
    PRINT("[" & sFlatter$ & "]")
    End
  End Sub

End Module
