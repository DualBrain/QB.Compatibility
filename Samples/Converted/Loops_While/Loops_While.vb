' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i = 1024!
    While i > 0
       PRINT(Str$(i) & " ")
       i = Int(i / 2)
    End While
    PRINT()
    
    i = 1024
    Do While i > 0
       PRINT(Str$(i) & " ")
       i = CInt(i) \ 2
    Loop
    End
  End Sub

End Module
