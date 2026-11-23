' PMAP  -  https://qbasic.net/en/qb-manual/Function/PMAP.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module PmapExample

    Public Sub Run()
    SCREEN(1)
    WINDOW_SCREEN(0, 0, 100, 100)
    PRINT("Logical x=50, physical x=" & Str(PMAP(50, 0)))
    PRINT("Logical y=50, physical y=" & Str(PMAP(50, 1)))
    End Sub

  End Module

End Namespace
