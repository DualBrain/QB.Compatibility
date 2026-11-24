' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim x$ = "big"
    PRINT("Mary had a " & x$ & " lamb")
    x$ = "little"
    PRINT_USING("Mary also had a & lamb", x$)
    ' this code above doesn't modify the first string subsustituting a piece of it with another string
    'surely it gives the right output on the screen
  End Sub

End Module
