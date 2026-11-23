' TREE  -  (sample from the project)
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports System.Math
Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module TreeExample

    Public Sub Run()
    SCREEN(13)
    COLOR(2)
    PSET(140 + Cos(0), 20)
    For i = 0 To 70 Step 0.1
      LINE(i * Cos(i) + 140, 20 - i * (Sin(i) * 0.2 - 2))
    Next
    COLOR(14)
    LINE(136, 20, 140, 12)
    LINE(144, 20) : LINE(134, 16)
    LINE(146, 16) : LINE(136, 20)
    End Sub

  End Module

End Namespace
