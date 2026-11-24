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
    Dim P!
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/PEN.htm
    Do
      P = PEN(3)
      LOCATE(1, 1) : PRINT("Pen is ", True)
      If P <> 0 Then PRINT("down") Else PRINT("up  ")
      PRINT("X =" & Str$(PEN(4)) & " " & vbTab & " Y =" & Str$(PEN(5)) & " " & "  ")
    Loop
  End Sub

End Module
