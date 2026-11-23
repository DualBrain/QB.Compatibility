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

Module PlayFunction

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim Music$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/PLAY.htm
    Music$ = "MBT180o2P2P8L8GGGL2E-P24P8L8FFFL2D"
    PLAY(Music$)
    While PLAY(0) > 5 : End While
    PRINT("Just about done!")
  End Sub

End Module
