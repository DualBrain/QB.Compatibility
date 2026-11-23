' PAINT  -  https://qbasic.net/en/qb-manual/Statement/PAINT.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module PaintExample

    Public Sub Run()
    SCREEN(1)
    CIRCLE(106, 100, 75, 1)
    LINE(138, 35, 288, 165, 1, LineOption.B)
    PAINT(160, 100, 2, 1)
    End Sub

  End Module

End Namespace
