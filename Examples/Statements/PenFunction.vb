' PEN  -  https://qbasic.net/en/qb-manual/Function/PEN.htm
' A VB translation of the QBasic example.

Imports QB
Imports QB.Devices
Imports QB.Events
Imports QB.Graphics

Namespace Examples

  Friend Module PenFunctionExample

    Public Sub Run()
    ' (The mouse is the light pen, and its left button is the pen switch.)
    Do
      Dim p = PEN(3)
      LOCATE(1, 1) : PRINT("Pen is ", True)
      If p <> 0 Then PRINT("down") Else PRINT("up ")
      PRINT("X =" & Str(PEN(4)) & vbTab & " Y =" & Str(PEN(5)) & " ")
    Loop
    End Sub

  End Module

End Namespace
