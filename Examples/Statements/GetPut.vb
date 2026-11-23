' GET  -  https://qbasic.net/en/qb-manual/Statement%20%28Graphics%29/PUT.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module GetPutExample

    Public Sub Run()
    SCREEN(1)
    ' DIM Box%(1 TO 200): a QBasic INTEGER is a VB Short, and VB arrays start at 0.
    Dim box(200) As Short
    Dim x1 = 0, x2 = 10, y1 = 0, y2 = 10
    LINE(x1, y1, x2, y2, 2, LineOption.BF)
    [GET](x1, y1, x2, y2, box)
    Do
      PUT(x1, y1, box, PutOption.PUT_XOR)
      x1 = CInt(Rnd() * 300)
      y1 = CInt(Rnd() * 180)
      PUT(x1, y1, box) ' XOR is the default.
      Threading.Thread.Sleep(20)
    Loop While INKEY() = ""
    End Sub

  End Module

End Namespace
