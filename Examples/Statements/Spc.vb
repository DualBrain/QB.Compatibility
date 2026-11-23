' SPC  -  https://qbasic.net/en/qb-manual/Function/SPC.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module SpcExample

    Public Sub Run()
    PRINT("Text1" & SPC(10) & "Text2")
    End Sub

  End Module

End Namespace
