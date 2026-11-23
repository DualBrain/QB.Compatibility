' STICK  -  https://qbasic.net/en/qb-manual/Function/STICK.htm
' A VB translation of the QBasic example.

Imports QB
Imports QB.Devices
Imports QB.Events
Imports QB.Graphics

Namespace Examples

  Friend Module StickExample

    Public Sub Run()
    Dim temp = STICK(0)
    PRINT(Str(STICK(2)) & vbTab & Str(STICK(3)))
    End Sub

  End Module

End Namespace
