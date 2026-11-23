' LOF  -  https://qbasic.net/en/qb-manual/Function/LOF.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.
' QBasic's OPEN ... FOR OUTPUT AS #1 is OPEN(file, OpenMode.Output, 1); the # is dropped.

Imports QB
Imports QB.Core
Imports QB.File
Imports QB.Video

Namespace Examples

  Friend Module LofExample

    Public Sub Run()
    ' The documentation opens a file the user names; one is made here first.
    OPEN("TEST.DAT", OpenMode.Output, 1)
    PRINT(1, "Some text in a file")
    CLOSE(1)
    Dim f As String = ""
    INPUT("Enter filename: ", f)
    OPEN(f, OpenMode.Binary, 1)
    PRINT("File length = " & Str(LOF(1)))
    CLOSE()
    KILL("TEST.DAT") ' (Clean up; not in the original.)
    End Sub

  End Module

End Namespace
