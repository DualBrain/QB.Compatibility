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

Module Screen_Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/SCREEN.htm
    'This example requires a color graphics adapter.
    SCREEN(1)  '320 x 200 graphics
    LINE(110, 70, 190, 120, -1, LineOption.B)
    LINE(0, 0, 320, 200, 3, LineOption.None, -256)
  End Sub

End Module
