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
    Dim a_Str$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/PRINT%20USING.htm
    Dim a! = 123.4567!
    PRINT_USING("###.##", a)
    LPRINT_USING("+###.####", a)
    a_Str$ = "ABCDEFG"
    PRINT_USING("!", a_Str$)
    LPRINT_USING("\ \", a_Str$)
  End Sub

End Module
