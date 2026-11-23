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

Module OnKey

  Const ESC As Short = 27

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/ON%20KEY.htm
    'This example requires Caps Lock and Num Lock to be off.
    ' CONST ESC = 27
    KEY_DEFINE(15, Asc(Left$(QBChr(4) + QBChr(31), 1)), Asc(Mid$(QBChr(4) + QBChr(31), 2, 1)))  'Set up Ctrl+S as KEY 15.
    ON_KEY(15, AddressOf PauseHandler)
    KEY_ON(15)
    While INKEY$() <> QBChr(ESC)
        PRINT("Press Esc to stop, Ctrl+S to pause.")
        PRINT()
    End While
    End
    
  End Sub

  Sub PauseHandler()
      SLEEP(1)
      Return
  End Sub
  
End Module
