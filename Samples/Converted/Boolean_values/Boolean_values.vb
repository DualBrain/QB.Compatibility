' Converted from QBasic by qb2vb.

Module Program

  Const [FALSE] As Short = 0
  Const [TRUE] As Short = Not [FALSE]

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' CONST FALSE=0
    ' CONST TRUE = Not FALSE
    PRINT(Str$([FALSE]) & " ")
    PRINT(Str$([TRUE]) & " ")
  End Sub

End Module
