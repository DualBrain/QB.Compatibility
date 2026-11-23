' ON_STRIG  -  https://qbasic.net/en/qb-manual/Statement/ON%20STRIG.htm
' A VB translation of the QBasic example.

Imports QB
Imports QB.Devices
Imports QB.Events
Imports QB.Graphics

Namespace Examples

  Friend Module OnStrigExample

    Private Sub Handler()
      PRINT("Joystick trigger is depressed.")
    End Sub

    Public Sub Run()
    ' This example requires a joystick.
    ON_STRIG(0, AddressOf Handler)
    STRIG_ON(0)
    PRINT("Press Esc to exit.")
    Do Until INKEY() = Chr(27) : Loop
    End Sub

  End Module

End Namespace
