Option Explicit On
Option Strict On
Option Infer On

Imports System.Collections.Concurrent
Imports System.Runtime.InteropServices

Namespace Global.QB

  ''' <summary>Supplies the pixels of the QB screen to a display.</summary>
  Public Interface IVideoSource
    ''' <summary>
    ''' Copies the screen as 32-bit ARGB pixels when it has changed since the last call.
    ''' Returns False (leaving the arguments alone) when there is nothing new to show.
    ''' </summary>
    Function TryCopyFrame(ByRef argb() As UInteger, ByRef width As Integer, ByRef height As Integer) As Boolean
  End Interface

  ''' <summary>Marks a display that is a window the user closes (a program that ends waits for a key in it).</summary>
  Public Interface IWindowDisplay
  End Interface

  ''' <summary>A display with a title bar the program can name (see <see cref="Video.WindowTitle"/>).</summary>
  Public Interface IWindowTitle
    ''' <summary>Puts <paramref name="title"/> on the window.</summary>
    Sub SetTitle(title As String)
  End Interface

  ''' <summary>A place where the QB screen is shown and keys are read from.</summary>
  Public Interface IDisplay
    ''' <summary>Starts showing the source.  Returns once the display is ready.</summary>
    Sub Open(source As IVideoSource)
    ''' <summary>Returns the next key (encoded like INKEY$), or an empty string.</summary>
    Function ReadKey() As String
    Sub Close()
  End Interface

  ''' <summary>The state of the pointer (the mouse stands in for QBasic's light pen), in screen pixels.</summary>
  Public Structure PointerState
    ''' <summary>The last known position.</summary>
    Public X As Integer
    Public Y As Integer
    ''' <summary>True while the button is down.</summary>
    Public Down As Boolean
    ''' <summary>How many times the button has been pressed.</summary>
    Public Presses As Integer
    ''' <summary>Where it was last pressed.</summary>
    Public PressX As Integer
    Public PressY As Integer
    ''' <summary>Where it was when it last left the screen.</summary>
    Public LeaveX As Integer
    Public LeaveY As Integer
  End Structure

  ''' <summary>
  ''' Joysticks and the pointer, for STICK, STRIG, PEN and their events.  A display can offer this as well as
  ''' <see cref="IDisplay"/>; the SDL window does (first two joysticks plus the mouse), HeadlessDisplay simulates it.
  ''' </summary>
  Public Interface IGameInput
    ''' <summary>An axis of joystick A (0) or B (1): axis 0 is x, 1 is y.  -32768 to 32767.</summary>
    Function StickAxis(stick As Integer, axis As Integer) As Integer
    ''' <summary>Whether the lower (0) or upper (1) button of joystick A (0) or B (1) is down.</summary>
    Function StickButtonDown(stick As Integer, button As Integer) As Boolean
    ''' <summary>How many times that button has been pressed.</summary>
    Function StickButtonPresses(stick As Integer, button As Integer) As Integer
    Function Pointer() As PointerState
  End Interface

  ''' <summary>
  ''' A display with no window, for tests: keys are supplied with <see cref="EnqueueKey"/> and the
  ''' screen is read with <see cref="Capture"/> or <see cref="SavePpm"/>.
  ''' </summary>
  Public NotInheritable Class HeadlessDisplay
    Implements IDisplay, IGameInput, IGraphicsDisplay, IWindowTitle

    Private m_source As IVideoSource
    Private ReadOnly m_keys As New ConcurrentQueue(Of String)
    Private m_argb() As UInteger
    Private m_width As Integer
    Private m_height As Integer

    ''' <summary>The title the program last gave the window (what a window would show).</summary>
    Public Property Title As String = ""

    Private Sub SetTitle(title As String) Implements IWindowTitle.SetTitle
      Me.Title = title
    End Sub

    Public Sub Open(source As IVideoSource) Implements IDisplay.Open
      m_source = source
    End Sub

    Public Function ReadKey() As String Implements IDisplay.ReadKey
      Dim k As String = Nothing
      Return If(m_keys.TryDequeue(k), k, "")
    End Function

    Public Sub Close() Implements IDisplay.Close
      m_source = Nothing
    End Sub

    Public Sub EnqueueKey(key As String)
      m_keys.Enqueue(key)
    End Sub

    ''' <summary>The most recent screen as ARGB pixels (null before anything was drawn).</summary>
    Public Function Capture(ByRef width As Integer, ByRef height As Integer) As UInteger()
      m_source?.TryCopyFrame(m_argb, m_width, m_height)
      width = m_width : height = m_height
      Return m_argb
    End Function

    ' ---- Simulated joysticks and pointer --------------------------------------------------------

    Private ReadOnly m_axes(1, 1) As Integer
    Private ReadOnly m_buttons(1, 1) As Boolean
    Private ReadOnly m_presses(1, 1) As Integer
    Private m_pointer As PointerState

    Public Sub SetStick(stick As Integer, axis As Integer, value As Integer)
      m_axes(stick, axis) = value
    End Sub

    Public Sub SetStickButton(stick As Integer, button As Integer, down As Boolean)
      If down AndAlso Not m_buttons(stick, button) Then m_presses(stick, button) += 1
      m_buttons(stick, button) = down
    End Sub

    Public Sub MovePointer(x As Integer, y As Integer)
      m_pointer.X = x : m_pointer.Y = y
    End Sub

    Public Sub PressPointer(x As Integer, y As Integer)
      m_pointer.X = x : m_pointer.Y = y
      m_pointer.Down = True
      m_pointer.Presses += 1
      m_pointer.PressX = x : m_pointer.PressY = y
    End Sub

    Public Sub ReleasePointer()
      m_pointer.Down = False
    End Sub

    Public Sub LeavePointer()
      m_pointer.LeaveX = m_pointer.X : m_pointer.LeaveY = m_pointer.Y
    End Sub

    Private Function StickAxis(stick As Integer, axis As Integer) As Integer Implements IGameInput.StickAxis
      Return m_axes(stick, axis)
    End Function

    Private Function StickButtonDown(stick As Integer, button As Integer) As Boolean Implements IGameInput.StickButtonDown
      Return m_buttons(stick, button)
    End Function

    Private Function StickButtonPresses(stick As Integer, button As Integer) As Integer Implements IGameInput.StickButtonPresses
      Return m_presses(stick, button)
    End Function

    Private Function Pointer() As PointerState Implements IGameInput.Pointer
      Return m_pointer
    End Function

    Public Sub SavePpm(path As String)
      Dim w, h As Integer
      Dim px = Capture(w, h)
      Using fs = IO.File.Create(path)
        Dim header = Text.Encoding.ASCII.GetBytes($"P6{vbLf}{w} {h}{vbLf}255{vbLf}")
        fs.Write(header, 0, header.Length)
        Dim rgb(px.Length * 3 - 1) As Byte
        For i = 0 To px.Length - 1
          rgb(i * 3) = CByte((px(i) >> 16) And 255UI)
          rgb(i * 3 + 1) = CByte((px(i) >> 8) And 255UI)
          rgb(i * 3 + 2) = CByte(px(i) And 255UI)
        Next
        fs.Write(rgb, 0, rgb.Length)
      End Using
    End Sub

  End Class

End Namespace
