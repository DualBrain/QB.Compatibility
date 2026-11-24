' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    For i = 1! To 10!
      If (CInt(i) And 1) <> 0 Then PRINT(Str$(i) & " " & " is odd") Else PRINT(Str$(i) & " " & " is even")
    Next i
  End Sub

End Module
