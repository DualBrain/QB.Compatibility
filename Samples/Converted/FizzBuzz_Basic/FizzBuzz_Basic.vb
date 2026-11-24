' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    For n = 1! To 100!
        If (CInt(n) Mod 15) = 0 Then PRINT("FizzBuzz") Else If (CInt(n) Mod 3) = 0 Then PRINT("Fizz") Else If (CInt(n) Mod 5) = 0 Then PRINT("Buzz") Else PRINT(Str$(n) & " ")
    Next n
  End Sub

End Module
