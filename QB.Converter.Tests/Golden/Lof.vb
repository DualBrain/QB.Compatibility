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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/LOF.htm
    INPUT("Enter filename: ", f$)
    OPEN(f$, OpenMode.Binary, 1)
    PRINT("File length = " & Str$(LOF(1)) & " ")
    CLOSE()
  End Sub

End Module
