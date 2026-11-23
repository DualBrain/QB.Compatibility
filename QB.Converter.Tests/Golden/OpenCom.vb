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
Imports QB.Serial
Imports QB.Multimedia
Imports QB.Printer
Imports QB.Video

Module OpenCom

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/OPEN%20COM.htm
    'Use this example for trouble shooting serial communications problems.
    'Slow baud, hardware handshaking is ignored and buffers are enlarged.
    OPEN("COM1:300,N,8,1,CD0,CS0,DS0,OP0,RS,TB2048,RB2048", OpenMode.Random, 1)
  End Sub

End Module
