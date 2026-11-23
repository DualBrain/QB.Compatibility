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

Module StrigFunction

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/STRIG.htm
    PRINT("Press Esc to exit.")
    Do
        If (STRIG(0) Or CShort(INKEY$() = QBChr(27))) <> 0 Then Exit Do
    Loop
    Do
        BEEP()  'BEEP while trigger A is pressed.
    Loop While STRIG(1) <> 0
  End Sub

End Module
