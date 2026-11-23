' INKEY$  -  https://qbasic.net/en/qb-manual/Function/INKEY%24.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module InkeyExample

    Public Sub Run()
    PRINT("Press Esc to exit...")
    Do
    Loop Until INKEY() = Chr(27)    ' 27 is the ASCII code for Esc.
    End Sub

  End Module

End Namespace
