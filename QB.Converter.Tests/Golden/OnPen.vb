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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/ON%20PEN.htm
        'This example requires a light pen.
        ON_PEN(AddressOf Handler)
        PEN_ON()
        PRINT("Press Esc to exit.")
        Do Until INKEY$() = QBChr(27) : Loop
        End
    
  End Sub

  Sub Handler()
      PRINT("Pen is at row" & Str$(PEN(6)) & " " & ", column" & Str$(PEN(7)) & " ")
      Return
  End Sub
  
End Module
