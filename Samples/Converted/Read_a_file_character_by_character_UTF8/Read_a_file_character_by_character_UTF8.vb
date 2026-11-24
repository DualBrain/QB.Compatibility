' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim char_$ = ""
    Dim f! = FREEFILE()
    Dim filename$ = "file.txt"
    
    OPEN(filename$, OpenMode.Binary, CInt(f))
    While Not EOF(CInt(f))
        char_$ = Str$(LOF(CInt(f)))
        [GET](CInt(f), char_$)
        PRINT(char_$, True)
    End While
    CLOSE(CInt(f))
  End Sub

End Module
