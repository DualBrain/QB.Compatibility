' SEEK  -  https://qbasic.net/en/qb-manual/Function/SEEK.htm
' A VB translation of the QBasic example (it also shows LOC for a random file).

Imports QB
Imports QB.File
Imports QB.Graphics

Namespace Examples

  Friend Module SeekExample

    Public Sub Run()
      OPEN("TEST.DAT", OpenMode.Random, 1)
      ' A QBasic INTEGER is a VB Short.
      For i As Short = 1 To 10
        PUT(1, i)               ' PUT #1, , i%
      Next
      SEEK(1, 2)
      Dim n As Short
      [GET](1, n)               ' GET #1, , i%
      PRINT("Data: " & Str(n) & " Current record: " & Str(LOC(1)) & " Next: " & Str(SEEK(1)))
      CLOSE(1)
      KILL("TEST.DAT") ' (Clean up; not in the original.)
    End Sub

  End Module

End Namespace
