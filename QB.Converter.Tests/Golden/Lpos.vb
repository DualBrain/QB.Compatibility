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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/LPOS.htm
    LPRINT()
    For i = 1S To 20S
        LPRINT(Str$(i) & " ", True)
        If LPOS(1) >= 10 Then LPRINT()  'Begin a new line.
    Next i
  End Sub

End Module
