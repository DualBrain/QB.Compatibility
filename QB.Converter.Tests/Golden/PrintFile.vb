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
    Dim a$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/PRINT%20%23.htm
    OPEN("TEST.DAT", OpenMode.Output, 1)
    PRINT_USING(1, "##.###  ", 12.12345!)
    CLOSE()
    OPEN("TEST.DAT", OpenMode.Input, 1)
    INPUT(1, a$)
    PRINT(a$)
    LPRINT("This is a line" & Str$(1) & " ")
    LPRINT("This is a line" & vbTab, True)
    LPRINT(Str$(2) & " ")
  End Sub

End Module
