' PSET  -  https://qbasic.net/en/qb-manual/Statement/PSET.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module PsetExample

    Public Sub Run()
    SCREEN(1)
    For i = 0 To 320
      PSET(i, 100)
      For delay = 1 To 100 : Next
      PRESET(i, 100)
    Next
    End Sub

  End Module

End Namespace
