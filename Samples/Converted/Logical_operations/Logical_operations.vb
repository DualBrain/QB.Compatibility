' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim b1 = -1!
    Dim b2 = 0!
    PRINT(Str$(CInt(b1) And CInt(b2)) & " ")
    PRINT(Str$(CInt(b1) Or CInt(b2)) & " ")
    PRINT(Str$(Not CInt(b1)) & " ")
  End Sub

End Module
