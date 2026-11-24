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
    Dim n$ = ""
    Dim a$ = ""
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/CLOSE.htm
    CLS()
    INPUT("Enter filename: ", n$, False)
    OPEN(n$, OpenMode.Output, 1)
    PRINT(1, "This is saved to the file.")
    CLOSE()
    OPEN(n$, OpenMode.Input, 1)
    INPUT(1, a$)
    PRINT("Read from file: " & a$)
    CLOSE()
  End Sub

End Module
