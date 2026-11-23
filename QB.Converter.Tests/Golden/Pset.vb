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

Module Pset_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim i As Short
    Dim delay As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/PSET.htm
    SCREEN(1)
    For i = 0 To 320
        PSET(i, 100)
        For delay = 1 To 100 : EMPTYLOOP() : Next delay
        PRESET(i, 100)
    Next i
  End Sub

End Module
