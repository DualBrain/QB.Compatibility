' WRITE  -  https://qbasic.net/en/qb-manual/Statement/WRITE%20%23.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.
' QBasic's OPEN ... FOR OUTPUT AS #1 is OPEN(file, OpenMode.Output, 1); the # is dropped.

Imports QB
Imports QB.Core
Imports QB.File
Imports QB.Graphics

Namespace Examples

  Friend Module WriteExample

    Public Sub Run()
    CLS()
    OPEN("LIST", OpenMode.Output, 1)
    Dim name As String = "", age As String = "", r As String = ""
    Do
      INPUT("   NAME:       ", name, False)
      INPUT("   AGE:        ", age, False)
      WRITE(1, name, age)
      INPUT("Add another entry", r)
    Loop While UCase(r) = "Y"
    CLOSE(1)
    ' Print the file to the screen.
    OPEN("LIST", OpenMode.Input, 1)
    CLS()
    PRINT("Entries in file:") : PRINT()
    Do While Not EOF(1)
      Dim rec1 As String = "", rec2 As String = ""
      INPUT(1, rec1, rec2)                    ' Read entries from file.
      PRINT(rec1 & vbTab & rec2)              ' PRINT Rec1$, Rec2$: the comma is a Tab.
    Loop
    CLOSE(1)
    KILL("LIST")
    End Sub

  End Module

End Namespace
