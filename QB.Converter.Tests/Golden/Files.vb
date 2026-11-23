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

Module Files_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/FILES.htm
    MKDIR("C:\TEMP\TEST")
    CHDIR("C:\TEMP")
    FILES()
    RMDIR("TEST")
  End Sub

End Module
