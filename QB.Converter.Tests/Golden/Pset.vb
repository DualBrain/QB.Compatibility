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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/PSET.htm
    SCREEN(1)
    For i = 0S To 320S
        PSET(i, 100)
        For delay = 1S To 100S : EMPTYLOOP() : Next delay
        PRESET(i, 100)
    Next i
  End Sub

End Module
