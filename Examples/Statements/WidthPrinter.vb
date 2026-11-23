' WIDTH_FILE  -  https://qbasic.net/en/qb-manual/Statement/WIDTH.htm
' A VB translation of the QBasic example.  Printing goes to the text file LPT1.TXT unless Printer.UseSystemPrinter() is called.

Imports QB
Imports QB.File
Imports QB.Printer
Imports QB.Graphics

Namespace Examples

  Friend Module WidthPrinterExample

    Public Sub Run()
    OPEN("LPT1:", OpenMode.Output, 1)
    WIDTH_FILE(1, 132)      ' WIDTH #1, 132
    PRINT(1, "A line sent to the printer through file #1.")
    CLOSE(1)
    End Sub

  End Module

End Namespace
