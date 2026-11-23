' PAINT_TILE  -  (not in the documentation)
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module PaintTileExample

    Public Sub Run()
    SCREEN(12)
    CIRCLE(320, 240, 150, 15)
    ' A checkerboard: one byte per plane for each row (here planes 0 and 2, giving color 5).
    Dim tile = QBChr(&HAA) & QBChr(0) & QBChr(&HAA) & QBChr(0) &
               QBChr(&H55) & QBChr(0) & QBChr(&H55) & QBChr(0)
    PAINT(320, 240, tile, 15)
    End Sub

  End Module

End Namespace
