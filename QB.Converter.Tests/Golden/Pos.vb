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

Module Pos_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim MyRow As Short
    Dim MyCol As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/POS.htm
    CLS()
    LOCATE(5, 5)
    MyRow = CShort(CSRLIN())
    MyCol = CShort(POS(0))
    PRINT("Position 1 (Press any key)")
    Do
    Loop While INKEY$() = ""
    LOCATE((MyRow + 2), (MyCol + 2))
    PRINT("Position 2")
  End Sub

End Module
