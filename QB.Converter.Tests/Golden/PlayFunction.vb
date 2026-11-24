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
Imports QB.Video

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/PLAY.htm
    Dim Music$ = "MBT180o2P2P8L8GGGL2E-P24P8L8FFFL2D"
    PLAY(Music$)
    While PLAY(0) > 5 : End While
    PRINT("Just about done!")
  End Sub

End Module
