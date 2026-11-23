' VIEW_PRINT  -  https://qbasic.net/en/qb-manual/Statement/VIEW%20PRINT.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module ViewPrintExample

    Public Sub Run()
    VIEW_PRINT(10, 15)
    For i = 1 To 100      ' Output will scroll.
      PRINT(Str(i))
    Next
    End Sub

  End Module

End Namespace
