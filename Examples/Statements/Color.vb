' COLOR  -  https://qbasic.net/en/qb-manual/Statement/COLOR.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module ColorExample

    Public Sub Run()
    SCREEN(7)
    For i = 0 To 15
      COLOR(i)
      PRINT(Str(i))
    Next
    End Sub

  End Module

End Namespace
