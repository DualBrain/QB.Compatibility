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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/WINDOW.htm
    'This example requires a color graphics adapter.
    SCREEN(1)
    For i = 1S To 10S Step 2S
        WINDOW((-160) / i, (-100) / i, 160 / i, 100 / i)
        CIRCLE(0, 0, 10)
    Next i
  End Sub

End Module
