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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement%20%28Music%29/PLAY.htm
    'Play scale in 7 different octaves
    Dim scale$ = "CDEFGAB"
    PLAY("L16")
    For i = 0S To 6S
        PLAY("O" + Str$(i))
        PLAY("X" + VARPTR$(scale$))
    Next i
  End Sub

End Module
