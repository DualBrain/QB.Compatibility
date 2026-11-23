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

Module PenFunction

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim P As Single
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/PEN.htm
    Do
      P = PEN(3)
      LOCATE(1, 1) : PRINT("Pen is ", True)
      If P <> 0 Then PRINT("down") Else PRINT("up  ")
      PRINT("X =" & Str$(PEN(4)) & " " & vbTab & " Y =" & Str$(PEN(5)) & " " & "  ")
    Loop
  End Sub

End Module
