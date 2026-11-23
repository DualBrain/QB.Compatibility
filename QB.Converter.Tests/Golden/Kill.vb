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

Module Kill_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim f$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/KILL.htm
    INPUT("File to delete: ", f$)
    KILL(f$)
  End Sub

End Module
