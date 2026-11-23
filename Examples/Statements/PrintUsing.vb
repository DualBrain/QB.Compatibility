' PRINT_USING  -  https://qbasic.net/en/qb-manual/Statement/PRINT%20USING.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module PrintUsingExample

    Public Sub Run()
    ' LPRINT USING becomes PRINT_USING here (there is no printer).
    Dim a = 123.4567
    PRINT_USING("###.##", a)
    PRINT_USING("+###.####", a)
    Dim s = "ABCDEFG"
    PRINT_USING("!", s)
    PRINT_USING("\ \", s)
    End Sub

  End Module

End Namespace
