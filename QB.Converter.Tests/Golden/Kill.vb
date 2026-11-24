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
    Dim f$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/KILL.htm
    INPUT("File to delete: ", f$)
    KILL(f$)
  End Sub

End Module
