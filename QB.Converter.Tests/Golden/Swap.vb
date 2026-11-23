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

Module Swap_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim a As Short
    Dim b As Short
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/SWAP.htm
    a = 1 : b = 2
    PRINT("Before: " & Str$(a) & " " & vbTab & Str$(b) & " ")
    SWAP(a, b)
    PRINT("After: " & Str$(a) & " " & vbTab & Str$(b) & " ")
  End Sub

End Module
