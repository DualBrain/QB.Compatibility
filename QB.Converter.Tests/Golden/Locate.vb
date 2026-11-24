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
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/LOCATE.htm
    CLS()
    LOCATE(5, 5)
    Dim MyRow As Short = CShort(CSRLIN())
    Dim MyCol As Short = CShort(POS(0))
    PRINT("Position 1 (Press any key)")
    Do
    Loop While INKEY$() = ""
    LOCATE((MyRow + 2), (MyCol + 2))
    PRINT("Position 2")
  End Sub

End Module
