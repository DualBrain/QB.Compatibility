' PCOPY  -  https://qbasic.net/en/qb-manual/Statement/PCOPY.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module PcopyExample

    Public Sub Run()
    ' The documentation shows only the statement; this adds some setup around it.
    SCREEN(7)
    SCREEN(-1, -1, 1, 0)            ' Draw on page 1 while page 0 is shown.
    CIRCLE(160, 100, 60, 14)
    PCOPY(1, 3)
    SCREEN(-1, -1, 0, 3)            ' Now show page 3: a copy of page 1.
    LOCATE(1, 1) : PRINT("Page 3 is a copy of page 1")
    End Sub

  End Module

End Namespace
