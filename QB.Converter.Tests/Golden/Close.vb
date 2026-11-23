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

Module Close_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim n$
    Dim a$
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
