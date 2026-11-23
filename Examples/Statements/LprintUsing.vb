' LPRINT_USING  -  https://qbasic.net/en/qb-manual/Statement/LPRINT%20USING.htm
' A VB translation of the QBasic example.  Printing goes to the text file LPT1.TXT unless Printer.UseSystemPrinter() is called.

Imports QB
Imports QB.File
Imports QB.Printer
Imports QB.Graphics

Namespace Examples

  Friend Module LprintUsingExample

    Public Sub Run()
    Dim a = 123.4567
    PRINT_USING("###.##", a)
    LPRINT_USING("+###.####", a)
    Dim s = "ABCDEFG"
    PRINT_USING("!", s)
    LPRINT_USING("\ \", s)
    End Sub

  End Module

End Namespace
