Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>The icon of the window of a program that draws: a bold white Q on the blue of the QBasic editor.</summary>
  Friend Module WindowIcon

    Friend Const Size As Integer = 32
    Friend Const Background As UInteger = &HFF0000AAUI
    Friend Const Letters As UInteger = &HFFFFFFFFUI

    ' 8 x 9 dots, drawn three times as large and centered
    Private ReadOnly Q As String() = {
      "..####..",
      ".######.",
      "##....##",
      "##....##",
      "##....##",
      "##..#.##",
      ".######.",
      "..####.#",
      "......##"}

    Private Const Scale As Integer = 3

    ''' <summary>The icon as 32-bit ARGB pixels, <see cref="Size"/> by <see cref="Size"/>.</summary>
    Friend Function Pixels(ByRef width As Integer) As UInteger()

      width = Size
      Dim result(Size * Size - 1) As UInteger
      For i = 0 To result.Length - 1
        result(i) = Background
      Next

      Dim glyphWidth = Q(0).Length * Scale
      Dim glyphHeight = Q.Length * Scale
      Draw(result, (Size - glyphWidth) \ 2, (Size - glyphHeight) \ 2)

      Return result

    End Function

    Private Sub Draw(pixels() As UInteger, left As Integer, top As Integer)
      For row = 0 To Q.Length - 1
        For column = 0 To Q(row).Length - 1
          If Q(row)(column) <> "#"c Then Continue For
          For dy = 0 To Scale - 1
            For dx = 0 To Scale - 1
              pixels((top + row * Scale + dy) * Size + left + column * Scale + dx) = Letters
            Next
          Next
        Next
      Next
    End Sub

  End Module

End Namespace
