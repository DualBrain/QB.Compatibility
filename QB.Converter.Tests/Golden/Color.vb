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

Module Color_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim i As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/COLOR.htm
    SCREEN(7)
    For i = 0 To 15
        COLOR(i)
        PRINT(Str$(i) & " ")
    Next i
  End Sub

End Module
