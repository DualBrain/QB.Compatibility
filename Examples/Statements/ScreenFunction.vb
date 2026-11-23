' SCREEN_FUNCTION  -  https://qbasic.net/en/qb-manual/Function/SCREEN.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module ScreenFunctionExample

    Public Sub Run()
    CLS()
    PRINT("Hello")
    PRINT("The ASCII value of character at 1,1 is" & Str(SCREEN(1, 1)))
    End Sub

  End Module

End Namespace
