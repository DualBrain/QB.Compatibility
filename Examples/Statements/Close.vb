' CLOSE  -  https://qbasic.net/en/qb-manual/Statement/CLOSE.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.
' QBasic's OPEN ... FOR OUTPUT AS #1 is OPEN(file, OpenMode.Output, 1); the # is dropped.

Imports QB
Imports QB.Core
Imports QB.File
Imports QB.Graphics

Namespace Examples

  Friend Module CloseExample

    Public Sub Run()
    CLS()
    Dim n As String = ""
    INPUT("Enter filename: ", n, False)
    OPEN(n, OpenMode.Output, 1)
    PRINT(1, "This is saved to the file.")
    CLOSE()
    OPEN(n, OpenMode.Input, 1)
    Dim a As String = ""
    INPUT(1, a)
    PRINT("Read from file: " & a)
    CLOSE()
    KILL(n) ' (Clean up; not in the original.)
    End Sub

  End Module

End Namespace
