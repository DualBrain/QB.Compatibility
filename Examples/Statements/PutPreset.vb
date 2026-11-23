' PUT_PRESET  -  (not in the documentation)
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module PutPresetExample

    Public Sub Run()
    SCREEN(1)
    Dim box(200) As Short
    LINE(0, 0, 30, 30, 1, LineOption.BF)
    LINE(10, 10, 20, 20, 3, LineOption.BF)
    [GET](0, 0, 30, 30, box)
    PUT(100, 50, box, PutOption.PUT_PSET)
    PUT(160, 50, box, PutOption.PUT_PRESET) ' The same image in reverse colors.
    End Sub

  End Module

End Namespace
