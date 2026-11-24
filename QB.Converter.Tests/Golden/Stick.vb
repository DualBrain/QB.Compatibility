' Converted from QBasic by qb2vb.

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

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/STICK.htm
    Dim Temp As Short = CShort(STICK(0))
    PRINT(Str$(STICK(2)) & " " & vbTab & Str$(STICK(3)) & " ")
  End Sub

End Module
