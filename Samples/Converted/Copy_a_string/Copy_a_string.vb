' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim src$ = "Hello"  ' is the original string
    Dim dst$ = src$  ' is the copy
    src$ = " world..."
    PRINT(dst$ & src$)
  End Sub

End Module
