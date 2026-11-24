' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim linea$ = ""
    OPEN("test.txt", OpenMode.Input, 1)
    
    While Not EOF(1)
        INPUT(1, linea$)
        PRINT(linea$)  ' echo to the console
    End While
    
    CLOSE(1)
    End
  End Sub

End Module
