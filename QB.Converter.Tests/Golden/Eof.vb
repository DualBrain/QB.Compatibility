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

Module Eof_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim i As Short
    Dim a$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/EOF.htm
    CLS()
    OPEN("TEST.DAT", OpenMode.Output, 1)
    For i = 1 To 10
        WRITE(1, i, 2 * i, 5 * i)
    Next i
    CLOSE(1)
    OPEN("TEST.DAT", OpenMode.Input, 1)
    Do
        LINE_INPUT(1, a$)
        PRINT(a$)
    Loop Until (EOF(1))
  End Sub

End Module
