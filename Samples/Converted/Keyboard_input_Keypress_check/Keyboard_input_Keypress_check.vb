' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim k$ = ""
    Do : k$ = INKEY$() : Loop Until k$ <> ""
    PRINT(k$)
  End Sub

End Module
