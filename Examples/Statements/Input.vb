' INPUT  -  https://qbasic.net/en/qb-manual/Statement/INPUT.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.
' QBasic's OPEN ... FOR OUTPUT AS #1 is OPEN(file, OpenMode.Output, 1); the # is dropped.

Imports QB
Imports QB.Core
Imports QB.File
Imports QB.Graphics

Namespace Examples

  Friend Module InputExample

    Public Sub Run()
    CLS()
    OPEN("LIST", OpenMode.Output, 1)
    Dim name As String = "", age As String = "", r As String = ""
    Do
      ' INPUT "   NAME:       ", Name$ -- a comma after the prompt shows no question mark.
      INPUT("   NAME:       ", name, False)
      INPUT("   AGE:        ", age, False)
      WRITE(1, name, age)
      ' INPUT "Add another entry"; R$ -- a semicolon adds "? " to the prompt.
      INPUT("Add another entry", r)
    Loop While UCase(r) = "Y"
    CLOSE(1)
    ' Echo the file back.
    OPEN("LIST", OpenMode.Input, 1)
    CLS()
    PRINT("Entries in file:") : PRINT()
    Do While Not EOF(1)
      Dim rec As String = ""
      LINE_INPUT(1, rec)     ' Read entries from the file.
      PRINT(rec)             ' Print the entries on the screen.
    Loop
    CLOSE(1)
    KILL("LIST")
    End Sub

  End Module

End Namespace
