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
Imports QB.Speaker
Imports QB.Printer
Imports QB.Video

Module Play_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim scale$
    Dim i As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement%20%28Music%29/PLAY.htm
    'Play scale in 7 different octaves
    scale$ = "CDEFGAB"
    PLAY("L16")
    For i = 0 To 6
        PLAY("O" + Str$(i))
        PLAY("X" + VARPTR$(scale$))
    Next i
  End Sub

End Module
