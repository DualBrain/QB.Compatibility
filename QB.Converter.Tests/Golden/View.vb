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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/VIEW.htm
    'This example requires a color graphics adapter.
    SCREEN(1)
    VIEW(10, 10, 300, 180, -1, 1)
    LOCATE(1, 11) : PRINT("A big graphics viewport", True)
    VIEW_SCREEN(80, 80, 200, 125, -1, 1)
    LOCATE(11, 11) : PRINT("A small graphics viewport", True)
  End Sub

End Module
