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

Module Stick_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim Temp As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/STICK.htm
    Temp = CShort(STICK(0))
    PRINT(Str$(STICK(2)) & " " & vbTab & Str$(STICK(3)) & " ")
  End Sub

End Module
