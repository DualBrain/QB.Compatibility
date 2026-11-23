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

Module Beep_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim Response$
    Dim R$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/BEEP.htm
    CLS()  ' Clear the screen
    Do
       INPUT("Hear a beep (Y or N)", Response$)
       R$ = UCase$(Mid$(Response$, 1, 1))
       If (R$ <> "Y") Or (R$ = "N") Then Exit Do
       BEEP()
    Loop
  End Sub

End Module
