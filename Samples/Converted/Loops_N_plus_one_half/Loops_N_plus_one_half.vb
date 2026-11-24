' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    For i = 1! To 10!
        PRINT_USING("##", i)
        If i = 10 Then Exit For
        PRINT(", ", True)
    Next i
  End Sub

End Module
