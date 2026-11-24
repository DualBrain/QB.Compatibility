' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim p!
    PRINT(Str$(1) & " " & vbTab, True)
    For n = 2! To 100!
        p = 1 + n
        For i! = 2 To n / 2
            If (CInt(n) Mod CInt(i)) = 0 Then p = p + i
        Next i
        PRINT(Str$(p) & " " & vbTab, True)
    Next n
    End
  End Sub

End Module
