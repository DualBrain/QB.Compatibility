' Converted from QBasic by qb2vb.  Lines marked TODO(QB2VB) need attention.
Option Explicit On
Option Strict On
Option Infer On

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

Module Point_Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim y As Short
    Dim x As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/POINT.htm
    SCREEN(1)
    LINE(0, 0, 100, 100, 2)
    LOCATE(14, 1)
    For y = 1 To 10
        For x = 1 To 10
            PRINT(Str$(POINT(x, y)) & " ", True)
        Next x
        PRINT()
    Next y
  End Sub

End Module
