' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim x!
    Dim y!
    INPUT("Please enter two integers, separated by a comma: ", x, y, False)
    
    If x < y Then PRINT(Str$(x) & " " & " is less than " & Str$(y) & " ")
    If x = y Then PRINT(Str$(x) & " " & " is equal to " & Str$(y) & " ")
    If x > y Then PRINT(Str$(x) & " " & " is greater than " & Str$(y) & " ")
  End Sub

End Module
