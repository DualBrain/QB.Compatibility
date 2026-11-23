' WINDOW  -  https://qbasic.net/en/qb-manual/Statement/WINDOW.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module WindowExample

    Public Sub Run()
    SCREEN(1)
    For i = 1 To 10 Step 2
      WINDOW(-160 / i, -100 / i, 160 / i, 100 / i)
      CIRCLE(0, 0, 10)
    Next
    End Sub

  End Module

End Namespace
