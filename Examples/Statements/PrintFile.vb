' PRINT_FILE  -  https://qbasic.net/en/qb-manual/Statement/PRINT%20%23.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.
' QBasic's OPEN ... FOR OUTPUT AS #1 is OPEN(file, OpenMode.Output, 1); the # is dropped.

Imports QB
Imports QB.Core
Imports QB.File
Imports QB.Video

Namespace Examples

  Friend Module PrintFileExample

    Public Sub Run()
    ' PRINT # to a file.  (The LPRINT lines of the original are left out: there is no printer.)
    OPEN("TEST.DAT", OpenMode.Output, 1)
    PRINT_USING(1, "##.### ", 12.12345)
    CLOSE()
    OPEN("TEST.DAT", OpenMode.Input, 1)
    Dim a As String = ""
    INPUT(1, a)
    PRINT(a)
    CLOSE()
    KILL("TEST.DAT") ' (Clean up; not in the original.)
    End Sub

  End Module

End Namespace
