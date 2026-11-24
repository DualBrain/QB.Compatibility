' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim j!
    Dim k!
    ' Version 1: Brute force
    
    Dim T! = CSng(Timer)
    Dim n1 = 0!
    For i = 1! To 10000000!
        j = i
        Do
            k = 0
            Do
                k = CSng(Int(k + ((CInt(j) Mod 10) ^ 2)))
                j = Int(j / 10)
            Loop While j <> 0
            j = k
        Loop Until (j = 89) Or (j = 1)
        If j > 1 Then n1 = n1 + 1
    Next i
    PRINT_USING("Version 1: ####### in ##.### seconds.", n1, (Timer - T))
    End
  End Sub

End Module
