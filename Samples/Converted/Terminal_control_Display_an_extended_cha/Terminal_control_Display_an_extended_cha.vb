' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    PRINT("£")
    
    ' or
    
    PRINT(QBChr(156))
    End
  End Sub

End Module
