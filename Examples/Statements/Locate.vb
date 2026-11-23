' LOCATE  -  https://qbasic.net/en/qb-manual/Statement/LOCATE.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module LocateExample

    Public Sub Run()
    CLS()
    LOCATE(5, 5)
    Dim myRow = CSRLIN()
    Dim myCol = POS(0)
    PRINT("Position 1 (Press any key)")
    Do
    Loop While INKEY() = ""
    LOCATE(myRow + 2, myCol + 2)
    PRINT("Position 2")
    End Sub

  End Module

End Namespace
