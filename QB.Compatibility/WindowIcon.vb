Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>The icon of the window of a program that draws: the letters QB in white on the blue of the QBasic editor.</summary>
  Friend Module WindowIcon

    Friend Const Size As Integer = 32
    Friend Const Background As UInteger = &HFF0000AAUI
    Friend Const Letters As UInteger = &HFFFFFFFFUI

    ' 7 x 9 dots each, drawn twice as large
    Private ReadOnly Q As String() = {
      ".#####.",
      "#.....#",
      "#.....#",
      "#.....#",
      "#.....#",
      "#..#..#",
      "#...#.#",
      ".#####.",
      "......#"}

    Private ReadOnly B As String() = {
      "######.",
      "#.....#",
      "#.....#",
      "#.....#",
      "######.",
      "#.....#",
      "#.....#",
      "#.....#",
      "######."}

    ''' <summary>The icon as 32-bit ARGB pixels, <see cref="Size"/> by <see cref="Size"/>.</summary>
    Friend Function Pixels(ByRef width As Integer) As UInteger()

      width = Size
      Dim result(Size * Size - 1) As UInteger
      For i = 0 To result.Length - 1
        result(i) = Background
      Next

      Draw(result, Q, 1, 7)
      Draw(result, B, 17, 7)

      Return result

    End Function

    Private Sub Draw(pixels() As UInteger, glyph() As String, left As Integer, top As Integer)
      For row = 0 To glyph.Length - 1
        For column = 0 To glyph(row).Length - 1
          If glyph(row)(column) <> "#"c Then Continue For
          For dy = 0 To 1
            For dx = 0 To 1
              pixels((top + row * 2 + dy) * Size + left + column * 2 + dx) = Letters
            Next
          Next
        Next
      Next
    End Sub

  End Module

End Namespace
