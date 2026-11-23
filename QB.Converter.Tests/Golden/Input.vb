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

Module Input_Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Name$
    Dim Age$
    Dim R$
    Dim REC$
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/INPUT.htm
    CLS()
    OPEN("LIST", OpenMode.Output, 1)
    Do
        INPUT("   NAME:       ", Name$, False)  'Read entries from the keyboard.
        INPUT("   AGE:        ", Age$, False)
        WRITE(1, Name$, Age$)
        INPUT("Add another entry", R$)
    Loop While UCase$(R$) = "Y"
    CLOSE(1)
    'Echo the file back.
    OPEN("LIST", OpenMode.Input, 1)
    CLS()
    PRINT("Entries in file:") : PRINT()
    Do While Not EOF(1)
        LINE_INPUT(1, REC$)  'Read entries from the file.
        PRINT(REC$)  'Print the entries on the screen.
    Loop
    CLOSE(1)
    KILL("LIST")
  End Sub

End Module
