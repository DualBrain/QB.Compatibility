' POS  -  https://qbasic.net/en/qb-manual/Function/POS.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module PosExample

    Public Sub Run()
    CLS()                       ' Clear screen
    PRINT("This program starts a new line after every 40")
    PRINT("characters you type. Press <CTRL-C> to end.")
    PRINT()
    Do
      Do While POS(0) < 41      ' Stay on same line until 40 characters printed.
        Dim ch As String
        Do                      ' Wait for a key.
          ch = INKEY()
        Loop While ch = ""
        ' If input is key combination CTRL-C then end; otherwise, print the character.
        If QBAsc(ch) = 3 Then Return
        PRINT(ch, True)
      Loop
      PRINT()                   ' Print a new line.
    Loop
    End Sub

  End Module

End Namespace
