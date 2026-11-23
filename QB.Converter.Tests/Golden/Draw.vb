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

Module Draw_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim Triangle$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/DRAW.htm
    SCREEN(1)
    Triangle$ = "F60 L120 E60"
    DRAW("C2 X" + VARPTR$(Triangle$))
    DRAW("BD30 P1,2 C3 M-30,-30")
  End Sub

End Module
