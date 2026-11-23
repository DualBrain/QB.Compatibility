' ON_PEN  -  https://qbasic.net/en/qb-manual/Statement/ON%20PEN.htm
' A VB translation of the QBasic example.

Imports QB
Imports QB.Devices
Imports QB.Events
Imports QB.Graphics

Namespace Examples

  Friend Module OnPenExample

    Private Sub Handler()
      PRINT("Pen is at row" & Str(PEN(6)) & ", column" & Str(PEN(7)))
    End Sub

    Public Sub Run()
    ' (The mouse is the light pen: click in the window.)
    ON_PEN(AddressOf Handler)
    PEN_ON()
    PRINT("Press Esc to exit.")
    Do Until INKEY() = Chr(27) : Loop
    End Sub

  End Module

End Namespace
