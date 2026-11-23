Option Explicit On
Option Strict On
Option Infer On

Imports Xunit

Namespace QB.Tests

  Public Class TerminalDisplayTests

    ' A text screen of 80 x 25 that the tests fill in.
    Private NotInheritable Class FakeScreen
      Implements IVideoSource, ITextSource

      Public ReadOnly Chars(24, 79) As Byte
      Public ReadOnly Attrs(24, 79) As Integer
      Public Version As Long = 1
      Public CursorRow As Integer = 1
      Public CursorColumn As Integer = 1
      Public CursorVisible As Boolean

      Public Sub New()
        For r = 0 To 24
          For c = 0 To 79
            Chars(r, c) = 32
            Attrs(r, c) = 7
          Next
        Next
      End Sub

      Public Sub Put(row As Integer, column As Integer, text As String, Optional attr As Integer = 7)
        For i = 0 To text.Length - 1
          Chars(row - 1, column - 1 + i) = CByte(AscW(text(i)))
          Attrs(row - 1, column - 1 + i) = attr
        Next
        Version += 1
      End Sub

      Public Function TryCopyFrame(ByRef argb() As UInteger, ByRef width As Integer, ByRef height As Integer) As Boolean Implements IVideoSource.TryCopyFrame
        Return False
      End Function

      Public Sub CopyText(ByRef version As Long, ByRef columns As Integer, ByRef rows As Integer, ByRef chars(,) As Byte, ByRef attrs(,) As Integer,
                          ByRef cursorRow As Integer, ByRef cursorColumn As Integer, ByRef cursorVisible As Boolean) Implements ITextSource.CopyText
        version = Me.Version
        columns = 80 : rows = 25
        chars = CType(Me.Chars.Clone(), Byte(,))
        attrs = CType(Me.Attrs.Clone(), Integer(,))
        cursorRow = Me.CursorRow : cursorColumn = Me.CursorColumn : cursorVisible = Me.CursorVisible
      End Sub

    End Class

    Private Shared Function Open(terminal As Boolean, output As IO.StringWriter, Optional width As Integer = 100, Optional height As Integer = 30) As (TerminalDisplay, FakeScreen)
      Dim d As New TerminalDisplay(output, terminal, Function() width, Function() height, False)
      Dim screen As New FakeScreen
      d.Open(screen)
      Return (d, screen)
    End Function

    Private Const Esc As String = ChrW(27)

    <Fact>
    Public Sub RedirectedOutputStaysPlainText()
      Dim w As New IO.StringWriter
      Dim d = Open(False, w).Item1
      d.SetColors(14, 1, False)
      d.ClearScreen()
      d.WriteChar(AscW("H"c)) : d.WriteChar(AscW("i"c))
      d.NewLine()
      d.Positional()
      d.Bell()
      d.FlushOutput()
      Assert.Equal("Hi" & vbLf, w.ToString)
      Assert.False(d.IsTerminal)
      Assert.False(d.IsFullScreen)
    End Sub

    <Fact>
    Public Sub ATerminalGetsColorsAndAClearScreenButScrolls()
      Dim w As New IO.StringWriter
      Dim d = Open(True, w).Item1
      d.SetColors(14, 1, False)
      d.WriteChar(AscW("A"c))
      d.NewLine()
      d.ClearScreen()
      d.FlushOutput()
      Dim text = w.ToString
      Assert.Contains(Esc & "[0;93;44mA" & vbLf, text)
      Assert.Contains(Esc & "[2J" & Esc & "[H", text)
      Assert.False(d.IsFullScreen)
    End Sub

    <Fact>
    Public Sub ColorsUseQBasicsOrderAsAnsi()
      Assert.Equal(Esc & "[0;30;40m", TerminalDisplay.Sgr(0, 0, False))
      Assert.Equal(Esc & "[0;34;41m", TerminalDisplay.Sgr(1, 4, False))   ' blue on red
      Assert.Equal(Esc & "[0;33;46m", TerminalDisplay.Sgr(6, 3, False))   ' brown is ANSI yellow, cyan
      Assert.Equal(Esc & "[0;97;40m", TerminalDisplay.Sgr(15, 0, False))
      Assert.Equal(Esc & "[0;5;37;40m", TerminalDisplay.Sgr(7, 0, True))
    End Sub

    <Fact>
    Public Sub UsingTheScreenByPositionSwitchesToTheWholeScreen()
      Dim w As New IO.StringWriter
      Dim pair = Open(True, w)
      Dim d = pair.Item1, screen = pair.Item2
      d.WriteChar(AscW("x"c))                 ' stream mode first
      screen.Put(5, 10, "Positioned", 12)
      screen.CursorRow = 5 : screen.CursorColumn = 20 : screen.CursorVisible = True
      d.Positional()
      Dim text = w.ToString
      Assert.True(d.IsFullScreen)
      Assert.Contains(Esc & "[?1049h", text)
      Assert.Contains(Esc & "[5;1H", text)
      Assert.Contains(Esc & "[0;91;40mPositioned", text)
      Assert.EndsWith(Esc & "[5;20H" & Esc & "[?25h", text)
    End Sub

    <Fact>
    Public Sub OnlyWhatChangedIsDrawnAgain()
      Dim w As New IO.StringWriter
      Dim pair = Open(True, w)
      Dim d = pair.Item1, screen = pair.Item2
      d.Positional()
      Dim before = w.ToString.Length
      screen.Put(2, 3, "ab")
      d.Render()
      Dim added = w.ToString.Substring(before)
      Assert.Contains(Esc & "[2;3H", added)
      Assert.Contains("ab", added)
      Assert.DoesNotContain(Esc & "[1;1H", added)
      Dim after = w.ToString.Length
      d.Render()   ' nothing changed
      Assert.Equal(after, w.ToString.Length)
    End Sub

    <Fact>
    Public Sub ATerminalThatIsTooSmallIsAskedToGrowAndShowsTheRowsAroundTheCursor()
      Dim w As New IO.StringWriter
      Dim pair = Open(True, w, 80, 24)
      Dim d = pair.Item1, screen = pair.Item2
      screen.Put(25, 1, "last row")
      screen.CursorRow = 25 : screen.CursorColumn = 9 : screen.CursorVisible = True
      d.Positional()
      Dim text = w.ToString
      Assert.Contains(Esc & "[8;25;80t", text)           ' the resize request
      Assert.Contains(Esc & "[24;1Hlast row", text)   ' row 25 is shown on the last line
    End Sub

    <Fact>
    Public Sub LeavingTheWholeScreenKeepsTheLastPictureInTheTerminal()
      Dim w As New IO.StringWriter
      Dim pair = Open(True, w)
      Dim d = pair.Item1, screen = pair.Item2
      screen.Put(2, 1, "final words")
      d.Positional()
      d.Close()
      Dim text = w.ToString
      Dim leave = text.IndexOf(Esc & "[?1049l", StringComparison.Ordinal)
      Assert.True(leave > 0)
      Assert.Contains(vbLf & "final words" & vbLf, text.Substring(leave))
    End Sub

    <Fact>
    Public Sub KeysCanBeQueued()
      Dim d = Open(False, New IO.StringWriter).Item1
      d.EnqueueKey("a")
      Assert.Equal("a", d.ReadKey())
      Assert.Equal("", d.ReadKey())
    End Sub

  End Class

End Namespace
