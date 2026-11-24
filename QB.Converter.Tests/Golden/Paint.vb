' Converted from QBasic by qb2vb.

Imports System.Math
Imports QB
Imports QB.Core
Imports QB.Development
Imports QB.Devices
Imports QB.Events
Imports QB.File
Imports QB.Multimedia
Imports QB.Printer
Imports QB.Graphics

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/PAINT.htm
    'This example requires a color graphics adapter.
    SCREEN(1)
    CIRCLE(106, 100, 75, 1)
    LINE(138, 35, 288, 165, 1, LineOption.B)
    PAINT(160, 100, 2, 1)
  End Sub

End Module
