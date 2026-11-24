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
    Dim x1 As Short
    Dim x2 As Short
    Dim y1 As Short
    Dim y2 As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement%20%28Graphics%29/PUT.htm
    'This example requires a color graphics adapter.
    SCREEN(1)
    Dim Box(200) As Short
    x1 = 0 : x2 = 10 : y1 = 0 : y2 = 10
    LINE(x1, y1, x2, y2, 2, LineOption.BF)
    [GET](x1, y1, x2, y2, Box)
    Do
        PUT(x1, y1, Box, PutOption.PUT_XOR)
        x1 = CShort(Rnd() * 300)
        y1 = CShort(Rnd() * 180)
        PUT(x1, y1, Box)
    Loop While INKEY$() = ""
  End Sub

End Module
