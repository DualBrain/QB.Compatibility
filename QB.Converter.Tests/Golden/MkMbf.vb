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

Module MkMbf

  Structure Buffer
    <VBFixedString(4)> Public SngNum As String
    <VBFixedString(8)> Public DblNum As String
  End Structure
  

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    Dim SNum As Single
    Dim DNum As Single
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/CVSMBF.htm
    ' TYPE Buffer (see the Structure at the top)
    Dim RecBuffer As Buffer
    OPEN("TESTDAT.DAT", OpenMode.Random, 1, 12)
    SNum = 98.9!
    DNum = 645.3235622#
    RecBuffer.SngNum = MKSMBF$(SNum)
    RecBuffer.DblNum = MKDMBF$(DNum)
    PUT(1, 1, RecBuffer)
    [GET](1, 1, RecBuffer)
    CLOSE(1)
    PRINT(Str$(CVSMBF(RecBuffer.SngNum)) & " " & vbTab & Str$(CVDMBF(RecBuffer.DblNum)) & " ")
  End Sub

End Module
