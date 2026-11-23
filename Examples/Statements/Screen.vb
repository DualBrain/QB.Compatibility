' SCREEN  -  https://qbasic.net/en/qb-manual/Statement/SCREEN.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module ScreenExample

    Public Sub Run()
    SCREEN(1)        ' 320 x 200 graphics
    LINE(110, 70, 190, 120, , LineOption.B)
    LINE(0, 0, 320, 200, 3, , &HFF00)
    End Sub

  End Module

End Namespace
