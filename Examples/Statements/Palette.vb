' PALETTE  -  https://qbasic.net/en/qb-manual/Statement/PALETTE.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Graphics

Namespace Examples

  Friend Module PaletteExample

    Public Sub Run()
    PALETTE(0, 1)
    SCREEN(1)
    Dim a(3) As Short
    For i = 0 To 3 : a(i) = CShort(i) : Next
    LINE(138, 35, 288, 165, 3, LineOption.BF)
    LINE(20, 10, 160, 100, 2, LineOption.BF)
    Do
      For i = 0 To 3
        a(i) = CShort((a(i) + 1) Mod 16)
      Next
      PALETTE_USING(a)
      Threading.Thread.Sleep(100)
    Loop While INKEY() = ""
    End Sub

  End Module

End Namespace
