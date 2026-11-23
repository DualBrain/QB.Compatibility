' CIRCLE  -  https://qbasic.net/en/qb-manual/Statement/CIRCLE.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module CircleExample

    Public Sub Run()
    SCREEN(2)
    CIRCLE(320, 100, 200)
    CIRCLE([STEP], 0, 0, 100)
    End Sub

  End Module

End Namespace
