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
    Dim Name$ = ""
    Dim Age$ = ""
    Dim R$ = ""
    Dim Rec1$ = ""
    Dim Rec2$ = ""
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/WRITE%20%23.htm
    CLS()
    OPEN("LIST", OpenMode.Output, 1)
    Do
        INPUT("   NAME:       ", Name$, False)
        INPUT("   AGE:        ", Age$, False)
        WRITE(1, Name$, Age$)
        INPUT("Add another entry", R$)
    Loop While UCase$(R$) = "Y"
    CLOSE(1)
    'Print the file to the screen.
    OPEN("LIST", OpenMode.Input, 1)
    CLS()
    PRINT("Entries in file:") : PRINT()
    Do While Not EOF(1)
        INPUT(1, Rec1$, Rec2$)  'Read entries from file.
        PRINT(Rec1$ & vbTab & Rec2$)  'Print the entries on the screen.
    Loop
    CLOSE(1)
    KILL("LIST")
  End Sub

End Module
