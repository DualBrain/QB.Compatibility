' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Data$
    ' File input/output
    OPEN("INPUT.TXT", OpenMode.Input, 1)
    OPEN("OUTPUT.TXT", OpenMode.Output, 2)
    Do Until EOF(1)
      LINE_INPUT(1, Data$)
      PRINT(2, Data$)
    Loop
    CLOSE(1)
    CLOSE(2)
    End
  End Sub

End Module
