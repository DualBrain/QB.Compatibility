' VIEW  -  https://qbasic.net/en/qb-manual/Statement/VIEW.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module ViewExample

    Public Sub Run()
    SCREEN(1)
    VIEW(10, 10, 300, 180, , 1)
    LOCATE(1, 11) : PRINT("A big graphics viewport", True)
    VIEW_SCREEN(80, 80, 200, 125, , 1)
    LOCATE(11, 11) : PRINT("A small graphics viewport", True)
    End Sub

  End Module

End Namespace
