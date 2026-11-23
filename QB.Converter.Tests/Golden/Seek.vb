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

Module Seek_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/SEEK.htm
    OPEN("TEST.DAT", OpenMode.Random, 1)
    For i = 1 To 10
        PUT(1, i)
    Next i
    SEEK(1, 2)
    [GET](1, i)
    PRINT("Data: " & Str$(i) & " " & " Current record: " & Str$(LOC(1)) & " " & " Next: " & Str$(SEEK(1)) & " ")
  End Sub

End Module
