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

Module Sound_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim i As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/SOUND.htm
    For i = 440 To 1000 Step 5
    SOUND(i, CInt(i / 1000))
    Next i
  End Sub

End Module
