' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim f! = FREEFILE()
    OPEN("output.txt", OpenMode.Output, CInt(f))
    PRINT(CInt(f), "This string is to be written to the file")
    CLOSE(-1)
  End Sub

End Module
