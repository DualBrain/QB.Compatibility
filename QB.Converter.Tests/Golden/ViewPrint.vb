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

Module ViewPrint

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/VIEW%20PRINT.htm
    VIEW_PRINT(10, 15)
    For i = 1 To 100  'Output will scroll.
        PRINT(Str$(i) & " ")
    Next i
  End Sub

End Module
