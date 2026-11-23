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

Module LprintUsing

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim a As Single
    Dim a_Str$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/LPRINT%20USING.htm
    a = 123.4567!
    PRINT_USING("###.##", a)
    LPRINT_USING("+###.####", a)
    a_Str$ = "ABCDEFG"
    PRINT_USING("!", a_Str$)
    LPRINT_USING("\ \", a_Str$)
  End Sub

End Module
