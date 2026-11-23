' LPOS  -  https://qbasic.net/en/qb-manual/Function/LPOS.htm
' A VB translation of the QBasic example.  Printing goes to the text file LPT1.TXT unless Printer.UseSystemPrinter() is called.

Imports QB
Imports QB.File
Imports QB.Printer
Imports QB.Video

Namespace Examples

  Friend Module LposExample

    Public Sub Run()
    LPRINT()
    For i = 1 To 20
      LPRINT(Str(i) & " ", True)       ' LPRINT i%;
      If LPOS(1) >= 10 Then LPRINT()    ' Begin a new line.
    Next
    End Sub

  End Module

End Namespace
