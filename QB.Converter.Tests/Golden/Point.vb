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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/POINT.htm
    SCREEN(1)
    LINE(0, 0, 100, 100, 2)
    LOCATE(14, 1)
    For y = 1S To 10S
        For x = 1S To 10S
            PRINT(Str$(POINT(x, y)) & " ", True)
        Next x
        PRINT()
    Next y
  End Sub

End Module
