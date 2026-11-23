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

Module OnStrig

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/ON%20STRIG.htm
        'This example requires a joystick.
        ON_STRIG(0, AddressOf Handler)
        STRIG_ON(0)
        PRINT("Press Esc to exit.")
        Do Until INKEY$() = QBChr(27) : Loop
        End
    
  End Sub

  Sub Handler()
      PRINT("Joystick trigger is depressed.")
      Return
  End Sub
  
End Module
