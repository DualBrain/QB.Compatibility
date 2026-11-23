' INPUT$  -  https://qbasic.net/en/qb-manual/Function/INPUT%24.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.
' QBasic's OPEN ... FOR OUTPUT AS #1 is OPEN(file, OpenMode.Output, 1); the # is dropped.

Imports QB
Imports QB.Core
Imports QB.File
Imports QB.Video

Namespace Examples

  Friend Module InputFunctionExample

    Public Sub Run()
    OPEN("TEST.DAT", OpenMode.Output, 1)
    PRINT(1, "The text")
    CLOSE()
    OPEN("TEST.DAT", OpenMode.Input, 1)
    PRINT(INPUT$(3, 1)) ' Print first 3 characters.
    CLOSE()
    KILL("TEST.DAT") ' (Clean up; not in the original.)
    End Sub

  End Module

End Namespace
