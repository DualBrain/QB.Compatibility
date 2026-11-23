' DRAW  -  https://qbasic.net/en/qb-manual/Statement/DRAW.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module DrawExample

    Public Sub Run()
    SCREEN(1)
    Dim triangle = "F60 L120 E60"
    ' DRAW "C2 X" + VARPTR$(Triangle$) becomes plain concatenation: X isn't supported.
    DRAW("C2 " & triangle)
    DRAW("BD30 P1,2 C3 M-30,-30")
    End Sub

  End Module

End Namespace
