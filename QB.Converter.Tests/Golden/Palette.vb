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

Module Palette_Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim i As Short
    Dim a(10) As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/PALETTE.htm
    'This example requires a color graphics adapter.
    PALETTE(0, 1)
    SCREEN(1)
    For i = 0 To 3 : a(i) = i : Next i
    LINE(138, 35, 288, 165, 3, LineOption.BF)
    LINE(20, 10, 160, 100, 2, LineOption.BF)
    Do
        For i = 0 To 3
            a(i) = CShort((a(i) + 1) Mod 16)
        Next i
        PALETTE_USING(a, 0)
    Loop While INKEY$() = ""
  End Sub

End Module
