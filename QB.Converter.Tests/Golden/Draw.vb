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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/DRAW.htm
    SCREEN(1)
    Dim Triangle$ = "F60 L120 E60"
    DRAW("C2 X" + VARPTR$(Triangle$))
    DRAW("BD30 P1,2 C3 M-30,-30")
  End Sub

End Module
