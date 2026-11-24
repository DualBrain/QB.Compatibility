' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim DOG$ = "Benjamin"
    DOG$ = "Samba"
    DOG$ = "Bernie"
    PRINT("There is just one dog, named " & DOG$)
  End Sub

End Module
