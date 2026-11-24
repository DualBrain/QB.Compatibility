' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' No primality testing is even required.
    Dim a As Double
    a = 600851475143#
    Dim i = 3!
    Do
        If Int(a / i) = (a / i) Then a = a / i
        i = i + 2
    Loop Until a = i  ' o WHILE a <> i
    PRINT(Str$(a) & " ")
  End Sub

End Module
