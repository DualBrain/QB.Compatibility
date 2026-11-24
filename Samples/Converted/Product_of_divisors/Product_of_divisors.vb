' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim p!
    For n = 1! To 50!
        p = n
        For i! = 2 To n / 2
            If (CInt(n) Mod CInt(i)) = 0 Then p = p * i
        Next i
        If (CInt((n - 1)) Mod 5) = 0 Then PRINT()
        PRINT_USING_NOCR("###########", p)
    Next n
    End
  End Sub

End Module
