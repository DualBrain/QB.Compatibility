' LPRINT  -  https://qbasic.net/en/qb-manual/Statement/LPRINT.htm
' A VB translation of the QBasic example.  Printing goes to the text file LPT1.TXT unless Printer.UseSystemPrinter() is called.

Imports QB
Imports QB.File
Imports QB.Printer
Imports QB.Video

Namespace Examples

  Friend Module LprintExample

    Public Sub Run()
    ' LPRINT "This is a line"; 1  -- a number prints with a space in front, a trailing ; stays on the line.
    LPRINT("This is a line" & Str(1))
    LPRINT("This is a line" & vbTab, True)  ' a trailing comma moves to the next print zone
    LPRINT(Str(2))
    End Sub

  End Module

End Namespace
