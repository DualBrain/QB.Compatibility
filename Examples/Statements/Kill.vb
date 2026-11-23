' KILL  -  https://qbasic.net/en/qb-manual/Statement/KILL.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.
' QBasic's OPEN ... FOR OUTPUT AS #1 is OPEN(file, OpenMode.Output, 1); the # is dropped.

Imports QB
Imports QB.Core
Imports QB.File
Imports QB.Graphics

Namespace Examples

  Friend Module KillExample

    Public Sub Run()
    ' The documentation shows only the last two lines; the file is created here first.
    OPEN("DELETEME.TMP", OpenMode.Output, 1) : CLOSE(1)
    Dim f As String = ""
    INPUT("File to delete: ", f)
    KILL(f)
    PRINT("Deleted " & f)
    End Sub

  End Module

End Namespace
