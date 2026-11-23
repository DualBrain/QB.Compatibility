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
Imports QB.Speaker
Imports QB.Printer
Imports QB.Video

Module OnPlay

  Dim Music$
  Dim i As Short

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/ON%20PLAY.htm
    ON_PLAY(3, AddressOf Background)
    PLAY_ON()
    Music$ = "MBo3L8ED+ED+Eo2Bo3DCL2o2A"
    PLAY(Music$)
    LOCATE(2, 1) : PRINT("Press any key to stop.", True)
    Do While INKEY$() = "" : Loop
    End
    
  End Sub

  Sub Background()
      i = CShort(i + 1)
      LOCATE(1, 1) : PRINT("Background called " & Str$(i) & " " & "time(s)", True)
      PLAY(Music$)
      Return
  End Sub
  
End Module
