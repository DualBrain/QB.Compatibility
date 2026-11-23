' CLS  -  https://qbasic.net/en/qb-manual/Statement/CLS.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module ClsExample

    Public Sub Run()
    Randomize(Timer)
    SCREEN(1)
    ' Set up a graphics viewport with a border.
    VIEW(5, 5, 100, 80, 3, 1)
    ' Set up a text viewport.
    VIEW_PRINT(12, 24)
    ' Print a message on the screen outside the text viewport.
    LOCATE(25, 1) : PRINT("Press any key to stop.")
    Dim count = 0
    Do
      ' Draw a circle with a random radius.
      CIRCLE(50, 40, CInt(Int((35 - 4) * Rnd() + 5)), count Mod 4)
      ' Clear the graphics viewport every 30 times.
      If (count Mod 30) = 0 Then CLS(1)
      PRINT("Hello. ", True)
      ' Clear the text viewport every 45 times.
      If (count Mod 45) = 0 Then CLS(2)
      count += 1
      Threading.Thread.Sleep(20) ' (QBasic is slow enough already.)
    Loop Until INKEY() <> ""
    End Sub

  End Module

End Namespace
