' PLAY  -  https://qbasic.net/en/qb-manual/Statement%20%28Music%29/PLAY.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Speaker
Imports QB.Graphics

Namespace Examples

  Friend Module PlayExample

    Public Sub Run()
    ' Play scale in 7 different octaves
    Dim scale = "CDEFGAB"
    PLAY("L16")
    For i = 0 To 6
      PLAY("O" & Str(i))
      ' PLAY "X" + VARPTR$(scale$) becomes plain concatenation: X isn't supported.
      PLAY(scale)
    Next
    End Sub

  End Module

End Namespace
