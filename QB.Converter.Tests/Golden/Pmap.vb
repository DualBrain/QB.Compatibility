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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/PMAP.htm
    'This example requires a graphics adapter that supports screen mode 1.
    SCREEN(1)
    WINDOW_SCREEN(0, 0, 100, 100)
    PRINT("Logical x=50, physical x=" & Str$(PMAP(50, 0)) & " ")
    PRINT("Logical y=50, physical y=" & Str$(PMAP(50, 1)) & " ")
  End Sub

End Module
