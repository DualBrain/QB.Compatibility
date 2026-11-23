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

Module Cls_Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim Count As Single
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/CLS.htm
    RANDOMIZE(CInt(Timer))
    SCREEN(1)
    ' Set up a graphics viewport with a border.
    VIEW(5, 5, 100, 80, 3, 1)
    ' Set up a text viewport.
    VIEW_PRINT(12, 24)
    ' Print a message on the screen outside the text viewport.
    LOCATE(25, 1) : PRINT("Press any key to stop.")
    Count = 0
    Do
       ' Draw a circle with a random radius.
       CIRCLE(50, 40, Int(((35 - 4) * Rnd()) + 5), (CInt(Count) Mod 4))
       ' Clear the graphics viewport every 30 times.
       If (CInt(Count) Mod 30) = 0 Then CLS(1)
       PRINT("Hello. ", True)
       ' Clear the text viewport every 45 times.
       If (CInt(Count) Mod 45) = 0 Then CLS(2)
       Count = Count + 1
    Loop Until INKEY$() <> ""
  End Sub

End Module
