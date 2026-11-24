' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    TRON()  ' activate system trace pragma
    TROFF()  ' deactivate system trace pragma
    ' QBasic debugging features make these instructions unnecessary
    End
  End Sub

End Module
