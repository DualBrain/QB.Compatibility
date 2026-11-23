' POINT  -  https://qbasic.net/en/qb-manual/Function/POINT.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module PointExample

    Public Sub Run()
    SCREEN(1)
    LINE(0, 0, 100, 100, 2)
    LOCATE(14, 1)
    For y = 1 To 10
      For x = 1 To 10
        PRINT(Str(POINT(x, y)) & " ", True)
      Next
      PRINT()
    Next
    End Sub

  End Module

End Namespace
