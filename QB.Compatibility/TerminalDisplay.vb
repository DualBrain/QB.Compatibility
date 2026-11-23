Option Explicit On
Option Strict On
Option Infer On

Imports System.Collections.Concurrent
Imports System.Runtime.InteropServices
Imports System.Text

Namespace Global.QB

  ''' <summary>
  ''' A display that follows text output as it happens (what a terminal can show): characters, line breaks, colors and clearing the
  ''' screen.  <see cref="Positional"/> is called when the program starts to use the screen by position (LOCATE and friends).
  ''' </summary>
  Public Interface ITextOutput
    Sub WriteChar(code As Integer)
    Sub NewLine()
    Sub CarriageReturn()
    ''' <summary>Erases the character before the cursor (editing a line of INPUT).</summary>
    Sub EraseBack()
    Sub SetColors(foreground As Integer, background As Integer, blink As Boolean)
    Sub ClearScreen()
    Sub Bell()
    Sub Positional()
  End Interface

  ''' <summary>A copy of the text of the screen, for a display that shows text only.</summary>
  Public Interface ITextSource
    ''' <summary>Copies the characters (CP437 codes) and attributes (foreground | background * 256 | blink * 65536) of the visible page.</summary>
    Sub CopyText(ByRef version As Long, ByRef columns As Integer, ByRef rows As Integer, ByRef chars(,) As Byte, ByRef attrs(,) As Integer,
                 ByRef cursorRow As Integer, ByRef cursorColumn As Integer, ByRef cursorVisible As Boolean)
  End Interface

  ''' <summary>Marks a display that can show graphics (SCREEN 1 and up).</summary>
  Public Interface IGraphicsDisplay
  End Interface

  ''' <summary>Tells the program that the keyboard has nothing more to give (input from a file or pipe ran out).</summary>
  Public Interface IInputEnd
    ReadOnly Property InputEnded As Boolean
  End Interface

  ''' <summary>
  ''' The default display for text programs: a terminal.  Output that is redirected stays plain text.  On a terminal, programs that
  ''' print and read lines just scroll (stream mode); the first use of the screen by position (LOCATE, VIEW PRINT, WIDTH...) switches
  ''' to a full-screen view of the whole text screen (the alternate screen of the terminal), which is left again when the program ends.
  ''' </summary>
  Public NotInheritable Class TerminalDisplay
    Implements IDisplay, ITextOutput, IInputEnd

    Private Enum Mode
      Piped        ' output isn't a terminal: plain text
      Stream       ' scrolling text
      Full         ' the whole text screen, drawn by position
    End Enum

    Private ReadOnly m_out As IO.TextWriter
    Private ReadOnly m_columns As Func(Of Integer)
    Private ReadOnly m_rows As Func(Of Integer)
    Private ReadOnly m_readsConsole As Boolean
    Private ReadOnly m_sync As New Object
    Private m_mode As Mode
    Private m_source As IVideoSource
    Private m_text As ITextSource
    Private ReadOnly m_keys As New ConcurrentQueue(Of String)
    Private m_inputEnded As Boolean
    Private m_renderThread As Threading.Thread
    Private m_running As Boolean
    Private m_opened As Boolean
    Private m_closed As Boolean
    Private m_lastFlush As Integer

    ' What the full-screen view last drew: character, attribute, per cell.
    Private m_shadowChars(,) As Byte
    Private m_shadowAttrs(,) As Integer
    Private m_shadowValid As Boolean
    Private m_lastVersion As Long = -1
    Private m_lastTermColumns, m_lastTermRows As Integer

    Private Shared ReadOnly s_ansi As Integer() = {0, 4, 2, 6, 1, 5, 3, 7}   ' QBasic color order to ANSI

    ''' <summary>A display on the console of the program.</summary>
    Public Sub New()
      Me.New(New IO.StreamWriter(Console.OpenStandardOutput(), New UTF8Encoding(False), 4096) With {.AutoFlush = False}, Not Console.IsOutputRedirected,
             Function() Console.WindowWidth, Function() Console.WindowHeight, True)
    End Sub

    ''' <summary>A display on a writer (for tests); <paramref name="terminal"/> says whether escape sequences may be written.</summary>
    Friend Sub New(output As IO.TextWriter, terminal As Boolean, columns As Func(Of Integer), rows As Func(Of Integer), readsConsole As Boolean)
      m_out = output
      m_mode = If(terminal, Mode.Stream, Mode.Piped)
      m_columns = columns
      m_rows = rows
      m_readsConsole = readsConsole
    End Sub

    ''' <summary>True when the output is a terminal (escape sequences are used).</summary>
    Friend ReadOnly Property IsTerminal As Boolean
      Get
        Return m_mode <> Mode.Piped
      End Get
    End Property

    ''' <summary>True once the view shows the whole text screen.</summary>
    Friend ReadOnly Property IsFullScreen As Boolean
      Get
        Return m_mode = Mode.Full
      End Get
    End Property

    Public ReadOnly Property InputEnded As Boolean Implements IInputEnd.InputEnded
      Get
        Return m_inputEnded AndAlso m_keys.IsEmpty
      End Get
    End Property

    ' ---- Opening and closing -----------------------------------------------------------------------------------------

    Public Sub Open(source As IVideoSource) Implements IDisplay.Open
      m_source = source
      m_text = TryCast(source, ITextSource)
      m_opened = True
      m_running = True
      If m_mode <> Mode.Piped AndAlso m_readsConsole Then EnableVirtualTerminal()
      If m_readsConsole Then
        Dim reader As New Threading.Thread(AddressOf ReadKeys) With {.IsBackground = True, .Name = "QB terminal keyboard"}
        reader.Start()
      End If
      AddHandler AppDomain.CurrentDomain.ProcessExit, AddressOf OnExit
      If m_readsConsole Then
        ' The terminal is put back as it was even when the program stops with an error or at Ctrl+C.
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnUnhandled
        AddHandler Console.CancelKeyPress, AddressOf OnCancel
      End If
    End Sub

    Private Sub OnUnhandled(sender As Object, e As UnhandledExceptionEventArgs)
      Shutdown()
    End Sub

    Private Sub OnCancel(sender As Object, e As ConsoleCancelEventArgs)
      Shutdown()
    End Sub

    Public Sub Close() Implements IDisplay.Close
      Shutdown()
    End Sub

    Private Sub OnExit(sender As Object, e As EventArgs)
      Shutdown()
    End Sub

    Private Sub Shutdown()
      SyncLock m_sync
        If m_closed Then Return
        m_closed = True
        m_running = False
        If m_mode = Mode.Full Then LeaveFullScreen()
        m_out.Flush()
      End SyncLock
      RemoveHandler AppDomain.CurrentDomain.ProcessExit, AddressOf OnExit
    End Sub

    ' ---- Keyboard ----------------------------------------------------------------------------------------------------------

    ''' <summary>Adds a key as if it had been typed (encoded like INKEY$).</summary>
    Friend Sub EnqueueKey(key As String)
      m_keys.Enqueue(key)
    End Sub

    Public Function ReadKey() As String Implements IDisplay.ReadKey
      Dim k As String = Nothing
      If m_keys.TryDequeue(k) Then Return k
      Return ""
    End Function

    Private Sub ReadKeys()
      Try
        If Console.IsInputRedirected Then
          ' Input from a file or a pipe: characters; a line feed is Enter.
          Do
            Dim c = Console.In.Read()
            If c < 0 Then Exit Do
            If c = 13 Then Continue Do
            m_keys.Enqueue(If(c = 10, ChrW(13), ChrW(c)))
          Loop
          m_inputEnded = True
        Else
          Do While m_running
            Dim info = Console.ReadKey(True)
            Dim s = KeyCodec.Encode(info.Key, info.KeyChar, (info.Modifiers And ConsoleModifiers.Shift) <> 0,
                                    (info.Modifiers And ConsoleModifiers.Control) <> 0, (info.Modifiers And ConsoleModifiers.Alt) <> 0)
            If s <> "" Then m_keys.Enqueue(s)
          Loop
        End If
      Catch ex As Exception
        m_inputEnded = True
      End Try
    End Sub

    ' ---- Stream mode: output as it happens ----------------------------------------------------------------------------------

    Private Sub Write(text As String)
      SyncLock m_sync
        If m_closed Then Return
        m_out.Write(text)
        FlushSoon()
      End SyncLock
    End Sub

    ' The writer is flushed at the end of a line and at most every 20 ms otherwise (and when the program waits for a key).
    Private Sub FlushSoon()
      Dim now = Environment.TickCount
      If now - m_lastFlush >= 20 Then
        m_out.Flush()
        m_lastFlush = now
      End If
    End Sub

    Friend Sub FlushOutput()
      SyncLock m_sync
        If Not m_closed Then m_out.Flush()
      End SyncLock
    End Sub

    Public Sub WriteChar(code As Integer) Implements ITextOutput.WriteChar
      If m_mode = Mode.Full Then Return
      Write(Cp437.ToChar(CByte(code And 255)).ToString)
    End Sub

    Public Sub NewLine() Implements ITextOutput.NewLine
      If m_mode = Mode.Full Then Return
      SyncLock m_sync
        If m_closed Then Return
        m_out.Write(vbLf)
        m_out.Flush()
        m_lastFlush = Environment.TickCount
      End SyncLock
    End Sub

    Public Sub CarriageReturn() Implements ITextOutput.CarriageReturn
      If m_mode = Mode.Full Then Return
      Write(vbCr)
    End Sub

    Public Sub EraseBack() Implements ITextOutput.EraseBack
      If m_mode = Mode.Full Then Return
      Write(ChrW(8) & " " & ChrW(8))
    End Sub

    Public Sub SetColors(foreground As Integer, background As Integer, blink As Boolean) Implements ITextOutput.SetColors
      If m_mode <> Mode.Stream Then Return
      Write(Sgr(foreground, background, blink))
    End Sub

    Public Sub ClearScreen() Implements ITextOutput.ClearScreen
      If m_mode <> Mode.Stream Then Return
      Write(ChrW(27) & "[2J" & ChrW(27) & "[H")
    End Sub

    Public Sub Bell() Implements ITextOutput.Bell
      If m_mode = Mode.Piped Then Return
      Write(ChrW(7))
    End Sub

    ''' <summary>The SGR escape sequence for QBasic colors (0 to 15 for the foreground, 0 to 7 for the background).</summary>
    Friend Shared Function Sgr(foreground As Integer, background As Integer, blink As Boolean) As String
      Dim fg = foreground And 15
      Dim bg = background And 15
      Dim fgCode = If(fg < 8, 30 + s_ansi(fg), 90 + s_ansi(fg - 8))
      Dim bgCode = If(bg < 8, 40 + s_ansi(bg), 100 + s_ansi(bg - 8))
      Return ChrW(27) & "[0;" & If(blink, "5;", "") & fgCode & ";" & bgCode & "m"
    End Function

    ' ---- Full-screen mode ------------------------------------------------------------------------------------------------------

    Public Sub Positional() Implements ITextOutput.Positional
      If m_mode <> Mode.Stream OrElse m_text Is Nothing OrElse Not m_opened Then Return
      SyncLock m_sync
        If m_mode <> Mode.Stream OrElse m_closed Then Return
        EnterFullScreen()
      End SyncLock
    End Sub

    Private Sub EnterFullScreen()
      Dim cols As Integer, rows As Integer
      Dim version As Long, chars(,) As Byte = Nothing, attrs(,) As Integer = Nothing
      Dim cr, cc As Integer, cv As Boolean
      m_text.CopyText(version, cols, rows, chars, attrs, cr, cc, cv)

      ' Ask the terminal for the room the screen needs (many terminals honor this), then see what it gave.
      If TerminalColumns() < cols OrElse TerminalRows() < rows Then
        m_out.Write(ChrW(27) & "[8;" & rows & ";" & cols & "t")
        m_out.Flush()
        Threading.Thread.Sleep(120)
      End If

      m_mode = Mode.Full
      m_out.Write(ChrW(27) & "[?1049h" & ChrW(27) & "[0m" & ChrW(27) & "[2J")
      m_shadowValid = False
      m_lastVersion = -1
      m_renderThread = New Threading.Thread(AddressOf RenderLoop) With {.IsBackground = True, .Name = "QB terminal screen"}
      Render()   ' the first picture, before the program goes on
      m_renderThread.Start()
    End Sub

    Private Sub LeaveFullScreen()
      m_mode = Mode.Stream
      Dim cols As Integer, rows As Integer
      Dim version As Long, chars(,) As Byte = Nothing, attrs(,) As Integer = Nothing
      Dim cr, cc As Integer, cv As Boolean
      m_text.CopyText(version, cols, rows, chars, attrs, cr, cc, cv)
      m_out.Write(ChrW(27) & "[0m" & ChrW(27) & "[?25h" & ChrW(27) & "[?1049l")
      ' The last picture of the screen stays in the terminal's history.
      Dim last = rows - 1
      Do While last >= 0 AndAlso RowIsBlank(chars, last, cols)
        last -= 1
      Loop
      For r = 0 To last
        Dim sb As New StringBuilder
        For c = 0 To cols - 1
          Dim code = chars(r, c)
          sb.Append(If(code = 0, " "c, Cp437.ToChar(code)))
        Next
        m_out.Write(sb.ToString.TrimEnd() & vbLf)
      Next
    End Sub

    Private Shared Function RowIsBlank(chars(,) As Byte, row As Integer, cols As Integer) As Boolean
      For c = 0 To cols - 1
        If chars(row, c) > 32 Then Return False
      Next
      Return True
    End Function

    Private Function TerminalColumns() As Integer
      Try
        Return Math.Max(1, m_columns())
      Catch ex As Exception
        Return 80
      End Try
    End Function

    Private Function TerminalRows() As Integer
      Try
        Return Math.Max(1, m_rows())
      Catch ex As Exception
        Return 25
      End Try
    End Function

    Private Sub RenderLoop()
      Do While m_running
        Threading.Thread.Sleep(25)
        SyncLock m_sync
          If m_closed OrElse m_mode <> Mode.Full Then Return
          Render()
        End SyncLock
      Loop
    End Sub

    ''' <summary>Draws what changed since the last picture (all of it after a change of size).</summary>
    Friend Sub Render()
      Dim cols As Integer, rows As Integer
      Dim version As Long, chars(,) As Byte = Nothing, attrs(,) As Integer = Nothing
      Dim cr, cc As Integer, cv As Boolean
      m_text.CopyText(version, cols, rows, chars, attrs, cr, cc, cv)

      Dim tc = TerminalColumns(), tr = TerminalRows()
      If tc <> m_lastTermColumns OrElse tr <> m_lastTermRows Then
        m_shadowValid = False
        m_lastTermColumns = tc : m_lastTermRows = tr
        m_out.Write(ChrW(27) & "[2J")
      ElseIf version = m_lastVersion AndAlso m_shadowValid Then
        Return
      End If
      m_lastVersion = version

      If m_shadowChars Is Nothing OrElse m_shadowChars.GetLength(0) <> rows OrElse m_shadowChars.GetLength(1) <> cols Then
        m_shadowChars = New Byte(rows - 1, cols - 1) {}
        m_shadowAttrs = New Integer(rows - 1, cols - 1) {}
        m_shadowValid = False
      End If

      ' A terminal with fewer rows than the screen shows the rows around the cursor.
      Dim top = 0
      If tr < rows Then top = Math.Max(0, Math.Min(rows - tr, cr - tr))
      Dim visibleRows = Math.Min(rows, tr)
      Dim visibleCols = Math.Min(cols, tc)

      Dim sb As New StringBuilder
      sb.Append(ChrW(27) & "[?25l")
      Dim lastAttr = -1
      For r = 0 To visibleRows - 1
        Dim sr = r + top
        Dim c = 0
        Do While c < visibleCols
          If m_shadowValid AndAlso m_shadowChars(sr, c) = chars(sr, c) AndAlso m_shadowAttrs(sr, c) = attrs(sr, c) Then
            c += 1
            Continue Do
          End If
          sb.Append(ChrW(27) & "[" & (r + 1) & ";" & (c + 1) & "H")
          ' A run of cells that changed.
          Do While c < visibleCols AndAlso Not (m_shadowValid AndAlso m_shadowChars(sr, c) = chars(sr, c) AndAlso m_shadowAttrs(sr, c) = attrs(sr, c))
            Dim a = attrs(sr, c)
            If a <> lastAttr Then
              sb.Append(Sgr(a And 15, (a >> 8) And 15, (a And &H10000) <> 0))
              lastAttr = a
            End If
            Dim code = chars(sr, c)
            sb.Append(If(code = 0, " "c, Cp437.ToChar(code)))
            m_shadowChars(sr, c) = chars(sr, c)
            m_shadowAttrs(sr, c) = attrs(sr, c)
            c += 1
          Loop
        Loop
      Next
      m_shadowValid = True

      ' The cursor: where the program left it, if it is visible.
      Dim cursorRow = cr - top
      If cv AndAlso cursorRow >= 1 AndAlso cursorRow <= visibleRows AndAlso cc <= visibleCols Then
        sb.Append(ChrW(27) & "[" & cursorRow & ";" & cc & "H" & ChrW(27) & "[?25h")
      End If
      m_out.Write(sb.ToString)
      m_out.Flush()
    End Sub

    ' ---- Windows consoles need virtual terminal processing switched on -------------------------------------------------------------

    <DllImport("kernel32.dll")>
    Private Shared Function GetStdHandle(nStdHandle As Integer) As IntPtr
    End Function

    <DllImport("kernel32.dll")>
    Private Shared Function GetConsoleMode(handle As IntPtr, ByRef mode As UInteger) As Boolean
    End Function

    <DllImport("kernel32.dll")>
    Private Shared Function SetConsoleMode(handle As IntPtr, mode As UInteger) As Boolean
    End Function

    Private Shared Sub EnableVirtualTerminal()
      If Not OperatingSystem.IsWindows() Then Return
      Try
        Dim h = GetStdHandle(-11)   ' STD_OUTPUT_HANDLE
        Dim mode As UInteger
        If GetConsoleMode(h, mode) Then SetConsoleMode(h, mode Or &H4UI Or &H1UI)   ' ENABLE_VIRTUAL_TERMINAL_PROCESSING | ENABLE_PROCESSED_OUTPUT
      Catch ex As Exception
      End Try
    End Sub

  End Class

End Namespace
