' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim n! = 7.125!
    PRINT_USING(("0000#.###"), n)  ' => 00007.125
  End Sub

End Module
