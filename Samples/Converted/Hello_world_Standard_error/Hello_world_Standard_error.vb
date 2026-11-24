' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    On Error GoTo ManejoErrores
    ERROR(99)
    End
    
    ManejoErrores:
      PRINT("Googbye World!")
      Resume
  End Sub

End Module
