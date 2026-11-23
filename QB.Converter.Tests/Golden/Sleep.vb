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
Imports QB.Video

Module Sleep_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/SLEEP.htm
    PRINT("Taking a 10-second nap...")
    SLEEP(10)
    PRINT("Wake up!")
  End Sub

End Module
