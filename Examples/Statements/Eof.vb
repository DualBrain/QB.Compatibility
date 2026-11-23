' EOF  -  https://qbasic.net/en/qb-manual/Function/EOF.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.
' QBasic's OPEN ... FOR OUTPUT AS #1 is OPEN(file, OpenMode.Output, 1); the # is dropped.

Imports QB
Imports QB.Core
Imports QB.File
Imports QB.Video

Namespace Examples

  Friend Module EofExample

    Public Sub Run()
    CLS()
    OPEN("TEST.DAT", OpenMode.Output, 1)
    For i = 1 To 10
      WRITE(1, i, 2 * i, 5 * i)
    Next
    CLOSE(1)
    OPEN("TEST.DAT", OpenMode.Input, 1)
    Do
      Dim a As String = ""
      LINE_INPUT(1, a)
      PRINT(a)
    Loop Until EOF(1)
    CLOSE(1)
    KILL("TEST.DAT") ' (Clean up; not in the original.)
    End Sub

  End Module

End Namespace
