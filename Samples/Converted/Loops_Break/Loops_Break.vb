' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim a!
    Dim b!
    RANDOMIZE(CInt(Timer))
    Do
        a = Int(Rnd() * 20)
        PRINT(Str$(a) & " ")
        If a = 10 Then Exit Do
        b = Int(Rnd() * 20)
        PRINT(Str$(b) & " ")
    Loop
  End Sub

End Module
