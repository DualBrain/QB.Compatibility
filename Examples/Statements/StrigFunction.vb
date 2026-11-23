' STRIG  -  https://qbasic.net/en/qb-manual/Function/STRIG.htm
' A VB translation of the QBasic example.

Imports QB
Imports QB.Devices
Imports QB.Events
Imports QB.Multimedia
Imports QB.Video

Namespace Examples

  Friend Module StrigFunctionExample

    Public Sub Run()
    PRINT("Press Esc to exit.")
    Do
      If STRIG(0) <> 0 OrElse INKEY() = Chr(27) Then Exit Do
    Loop
    Do
      BEEP() ' BEEP while trigger A is pressed.
    Loop While STRIG(1) <> 0
    End Sub

  End Module

End Namespace
