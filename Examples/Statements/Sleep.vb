' SLEEP  -  https://qbasic.net/en/qb-manual/Statement/SLEEP.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module SleepExample

    Public Sub Run()
    PRINT("Taking a 10-second nap...")
    SLEEP(10)
    PRINT("Wake up!")
    End Sub

  End Module

End Namespace
