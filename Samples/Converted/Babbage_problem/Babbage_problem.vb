' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim number = 524!
    Do
        number = number + 2
    Loop Until (CInt((number ^ 2)) Mod 1000000) = 269696
    PRINT("The smallest number whose square ends in 269696 is: " & Str$(number) & " ")
    PRINT("It's square is " & Str$(number ^ 2) & " ")
    End
  End Sub

End Module
