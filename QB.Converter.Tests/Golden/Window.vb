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

Module Window_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim i As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/WINDOW.htm
    'This example requires a color graphics adapter.
    SCREEN(1)
    For i = 1 To 10 Step 2
        WINDOW((-160) / i, (-100) / i, 160 / i, 100 / i)
        CIRCLE(0, 0, 10)
    Next i
  End Sub

End Module
