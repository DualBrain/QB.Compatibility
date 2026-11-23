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

Module LsetRset

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim Fields1 As FieldRecord
    Dim Fields2 As FieldRecord
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/LSET.htm
    OPEN("FILEDAT.DAT", OpenMode.Random, 1, 10)
    Fields1 = FIELD(1, 5, 5)
    Fields1.LSET(0, "LSET")
    Fields1.RSET(1, "RSET")
    PUT_FIELD(1, 1)
    CLOSE(1)
    OPEN("FILEDAT.DAT", OpenMode.Random, 1, 10)
    Fields2 = FIELD(1, 5, 5)
    GET_FIELD(1, 1)
    PRINT((("*" + Fields2(0)) + "*") & vbTab & (("*" + Fields2(1)) + "*"))
    CLOSE(1)
  End Sub

End Module
