' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim linea$ = ""
    Dim f! = FREEFILE()
    Dim filename$ = "file.txt"
    
    OPEN(filename$, OpenMode.Input, CInt(f))
    
    While Not EOF(CInt(f))
        LINE_INPUT(CInt(f), linea$)
        PRINT(linea$)
    End While
    CLOSE(CInt(f))
    End
  End Sub

End Module
