Option Explicit On
Option Strict On
Option Infer On

Imports System.Diagnostics

Namespace Global.QB

  ''' <summary>
  ''' QBasic's event trapping (ON TIMER, ON KEY, ON PLAY, ...): <c>ON_TIMER(5, AddressOf Tick) : TIMER_ON()</c>.
  ''' A handler is a Sub with no arguments (QBasic's GOSUB subroutine; it returns by ending).
  ''' Events never interrupt the program from another thread: they are noted when they happen and the handler is run,
  ''' on the program's own thread, the next time the program calls into the library (any drawing, INKEY, SLEEP,
  ''' PLAY, ...) or <see cref="Poll"/>.  A loop that calls nothing from the library has to call <see cref="Poll"/>.
  ''' While a handler runs its event is stopped (as in QBasic) and fires again afterwards if it happened meanwhile.
  ''' </summary>
  Public NotInheritable Class Events

    Private Sub New()
    End Sub

    Private Const Off As Integer = 0
    Private Const [On] As Integer = 1
    Private Const Suspended As Integer = 2

    Friend NotInheritable Class Trap
      Friend Handler As Action
      Friend State As Integer            ' Off, On or Suspended (STOP).
      Friend Pending As Boolean
      Friend Interval As Long            ' Timer: ticks; Play: queue limit; Key: unused.
      Friend NextTime As Long            ' Timer.
      Friend Armed As Boolean            ' Play.
      Friend KeyFlags As Integer = -1    ' Key: keyboard flag and scan code of a user-defined key.
      Friend KeyScan As Integer
      Friend KeyText As String           ' Key: the INKEY$ text of a function or arrow key.
    End Class

    Private Shared ReadOnly m_traps As New Dictionary(Of String, Trap)
    Private Shared m_active As Integer     ' 1 when any trap is on or stopped.
    Private Shared m_keyTraps As Boolean   ' True when a key trap is on or stopped.

    <ThreadStatic>
    Private Shared t_dispatching As Boolean

    Private Shared Function GetTrap(name As String) As Trap
      Dim t As Trap = Nothing
      If Not m_traps.TryGetValue(name, t) Then
        t = New Trap
        m_traps.Add(name, t)
      End If
      Return t
    End Function

    Private Shared Sub Refresh()
      Dim any = False, keys = False
      For Each kv In m_traps
        If kv.Value.State <> Off Then
          any = True
          If kv.Key.StartsWith("KEY", StringComparison.Ordinal) Then keys = True
        End If
      Next
      m_keyTraps = keys
      Threading.Volatile.Write(m_active, If(any, 1, 0))
    End Sub

    Private Shared Sub SetState(name As String, state As Integer)
      Dim t = GetTrap(name)
      t.State = state
      If state = Off Then t.Pending = False
      Refresh()
    End Sub

    ' ---- Running the handlers --------------------------------------------------------------------

    ''' <summary>Runs the handlers of events that have happened.  Call it in a loop that calls nothing else from the library.</summary>
    Public Shared Sub Poll()
      Dispatch()
    End Sub

    Friend Shared Sub Dispatch()

      If Threading.Volatile.Read(m_active) = 0 OrElse t_dispatching Then Return
      t_dispatching = True
      Try

        If m_keyTraps Then Video.PumpKeys()
        PollSources()

        For Each name In m_traps.Keys.ToArray
          Dim t = m_traps(name)
          If t.State = [On] AndAlso t.Pending AndAlso t.Handler IsNot Nothing Then
            t.Pending = False
            t.State = Suspended                ' Stopped while its handler runs.
            Try
              t.Handler()
            Finally
              If t.State = Suspended Then t.State = [On]
              Refresh()
            End Try
          End If
        Next

      Finally
        t_dispatching = False
      End Try

    End Sub

    Private Shared Sub PollSources()
      Dim now = Stopwatch.GetTimestamp()
      Dim timer As Trap = Nothing
      If m_traps.TryGetValue("TIMER", timer) AndAlso timer.State <> Off AndAlso now >= timer.NextTime Then
        timer.Pending = True
        timer.NextTime = now + timer.Interval
      End If
      Dim play As Trap = Nothing
      If m_traps.TryGetValue("PLAY", play) AndAlso play.State <> Off Then
        Dim count = Multimedia.PendingNotes()
        If count >= play.Interval Then
          play.Armed = True
        ElseIf play.Armed Then
          play.Armed = False
          play.Pending = True
        End If
      End If
      PollDevices()
    End Sub

    ' Joystick, pen and serial port traps.
    Private Shared Sub PollDevices()
      For Each kv In m_traps
        Dim t = kv.Value
        If t.State = Off Then Continue For
        If kv.Key.StartsWith("STRIG", StringComparison.Ordinal) Then
          Dim presses = StrigPresses(Integer.Parse(kv.Key.Substring(5), Globalization.CultureInfo.InvariantCulture))
          If presses <> t.Interval Then t.Pending = True : t.Interval = presses
        ElseIf kv.Key.StartsWith("COM", StringComparison.Ordinal) Then
          Dim waiting = File.ComBytesWaiting(Integer.Parse(kv.Key.Substring(3), Globalization.CultureInfo.InvariantCulture))
          If waiting > t.Interval Then t.Pending = True
          t.Interval = waiting
        ElseIf kv.Key = "PEN" Then
          Dim input = Video.GameInput()
          Dim presses = If(input Is Nothing, 0, input.Pointer().Presses)
          If presses <> t.Interval Then t.Pending = True : t.Interval = presses
        End If
      Next
    End Sub

    ' ---- ON TIMER ---------------------------------------------------------------------------------

    ''' <summary>ON TIMER(seconds) GOSUB handler: the handler runs every <paramref name="seconds"/> (1 to 86400) once TIMER_ON is called.</summary>
    Public Shared Sub ON_TIMER(seconds As Integer, handler As Action)
      If seconds < 1 OrElse seconds > 86400 Then Throw New ArgumentException("Illegal function call")
      Dim t = GetTrap("TIMER")
      t.Handler = handler
      t.Interval = CLng(seconds) * Stopwatch.Frequency
    End Sub

    Public Shared Sub TIMER_ON()
      Dim t = GetTrap("TIMER")
      If t.State = Off Then t.NextTime = Stopwatch.GetTimestamp() + t.Interval
      SetState("TIMER", [On])
    End Sub

    Public Shared Sub TIMER_OFF()
      SetState("TIMER", Off)
    End Sub

    ''' <summary>TIMER STOP: events are remembered and handled after TIMER_ON.</summary>
    Public Shared Sub TIMER_STOP()
      SetState("TIMER", Suspended)
    End Sub

    ' ---- ON PLAY ----------------------------------------------------------------------------------

    ''' <summary>ON PLAY(limit) GOSUB handler: runs when fewer than <paramref name="queueLimit"/> (1 to 32) notes are left in the music buffer.</summary>
    Public Shared Sub ON_PLAY(queueLimit As Integer, handler As Action)
      If queueLimit < 1 OrElse queueLimit > 32 Then Throw New ArgumentException("Illegal function call")
      Dim t = GetTrap("PLAY")
      t.Handler = handler
      t.Interval = queueLimit
    End Sub

    Public Shared Sub PLAY_ON()
      Dim t = GetTrap("PLAY")
      If t.State = Off Then t.Armed = Multimedia.PendingNotes() >= t.Interval
      SetState("PLAY", [On])
    End Sub

    Public Shared Sub PLAY_OFF()
      SetState("PLAY", Off)
    End Sub

    Public Shared Sub PLAY_STOP()
      SetState("PLAY", Suspended)
    End Sub

    ' ---- ON STRIG and ON PEN ------------------------------------------------------------------------

    Private Shared Function StrigName(n As Integer) As String
      If n <> 0 AndAlso n <> 2 AndAlso n <> 4 AndAlso n <> 6 Then Throw New ArgumentException("Illegal function call")
      Return "STRIG" & n.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    Private Shared Function StrigPresses(n As Integer) As Integer
      Dim input = Video.GameInput()
      Return If(input Is Nothing, 0, input.StickButtonPresses((n \ 2) And 1, n \ 4))
    End Function

    ''' <summary>ON STRIG(n) GOSUB handler: n is 0 (lower trigger, joystick A), 2 (lower, B), 4 (upper, A) or 6 (upper, B).</summary>
    Public Shared Sub ON_STRIG(n As Integer, handler As Action)
      GetTrap(StrigName(n)).Handler = handler
    End Sub

    Public Shared Sub STRIG_ON(n As Integer)
      Dim name = StrigName(n)
      Dim t = GetTrap(name)
      If t.State = Off Then t.Interval = StrigPresses(n)
      SetState(name, [On])
    End Sub

    Public Shared Sub STRIG_OFF(n As Integer)
      SetState(StrigName(n), Off)
    End Sub

    Public Shared Sub STRIG_STOP(n As Integer)
      SetState(StrigName(n), Suspended)
    End Sub

    ''' <summary>ON PEN GOSUB handler: runs when the pen (the mouse's left button) is pressed.</summary>
    Public Shared Sub ON_PEN(handler As Action)
      GetTrap("PEN").Handler = handler
    End Sub

    Public Shared Sub PEN_ON()
      Dim t = GetTrap("PEN")
      If t.State = Off Then
        Dim input = Video.GameInput()
        t.Interval = If(input Is Nothing, 0, input.Pointer().Presses)
      End If
      SetState("PEN", [On])
    End Sub

    Public Shared Sub PEN_OFF()
      SetState("PEN", Off)
    End Sub

    Public Shared Sub PEN_STOP()
      SetState("PEN", Suspended)
    End Sub

    ' ---- ON COM ---------------------------------------------------------------------------------------

    Private Shared Function ComName(n As Integer) As String
      If n < 1 OrElse n > 4 Then Throw New ArgumentException("Illegal function call")
      Return "COM" & n.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    ''' <summary>ON COM(n) GOSUB handler: runs when characters arrive at the open serial port COMn.</summary>
    Public Shared Sub ON_COM(n As Integer, handler As Action)
      GetTrap(ComName(n)).Handler = handler
    End Sub

    Public Shared Sub COM_ON(n As Integer)
      Dim name = ComName(n)
      Dim t = GetTrap(name)
      If t.State = Off Then t.Interval = File.ComBytesWaiting(n)
      SetState(name, [On])
    End Sub

    Public Shared Sub COM_OFF(n As Integer)
      SetState(ComName(n), Off)
    End Sub

    Public Shared Sub COM_STOP(n As Integer)
      SetState(ComName(n), Suspended)
    End Sub

    ' ---- ON KEY -----------------------------------------------------------------------------------

    Private Shared Function KeyName(n As Integer) As String
      Return "KEY" & n.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    Private Shared Function KeyText(n As Integer) As String
      Select Case n
        Case 1 To 10 : Return ChrW(0) & ChrW(58 + n)
        Case 11 : Return ChrW(0) & ChrW(72)
        Case 12 : Return ChrW(0) & ChrW(75)
        Case 13 : Return ChrW(0) & ChrW(77)
        Case 14 : Return ChrW(0) & ChrW(80)
        Case 30 : Return ChrW(0) & ChrW(133)
        Case 31 : Return ChrW(0) & ChrW(134)
        Case Else : Return Nothing
      End Select
    End Function

    Private Shared Sub CheckKey(n As Integer)
      If Not ((n >= 1 AndAlso n <= 25) OrElse n = 30 OrElse n = 31) Then Throw New ArgumentException("Illegal function call")
    End Sub

    ''' <summary>ON KEY(n) GOSUB handler: n is 1 to 10 (F1 to F10), 11 to 14 (Up, Left, Right, Down), 15 to 25 (user-defined with KEY_DEFINE) or 30 and 31 (F11, F12).</summary>
    Public Shared Sub ON_KEY(n As Integer, handler As Action)
      CheckKey(n)
      Dim t = GetTrap(KeyName(n))
      t.Handler = handler
      t.KeyText = KeyText(n)
    End Sub

    ''' <summary>
    ''' KEY n, CHR$(keyboardflag) + CHR$(scancode): defines user key 15 to 25.  The flag is 0 (none), 1 to 3
    ''' (Shift), 4 (Ctrl), 8 (Alt) and so on, as in QBasic; the scan code is the key's scan code.
    ''' </summary>
    Public Shared Sub KEY_DEFINE(n As Integer, keyboardFlag As Integer, scanCode As Integer)
      If n < 15 OrElse n > 25 Then Throw New ArgumentException("Illegal function call")
      Dim t = GetTrap(KeyName(n))
      t.KeyFlags = keyboardFlag
      t.KeyScan = scanCode
    End Sub

    Private Shared Sub SetKeyState(n As Integer, state As Integer)
      If n = 0 Then
        For Each kv In m_traps
          If kv.Key.StartsWith("KEY", StringComparison.Ordinal) Then
            kv.Value.State = state
            If state = Off Then kv.Value.Pending = False
          End If
        Next
        Refresh()
      Else
        CheckKey(n)
        SetState(KeyName(n), state)
      End If
    End Sub

    ''' <summary>KEY(n) ON (n = 0 for all keys that have a handler).</summary>
    Public Shared Sub KEY_ON(n As Integer)
      SetKeyState(n, [On])
    End Sub

    Public Shared Sub KEY_OFF(n As Integer)
      SetKeyState(n, Off)
    End Sub

    Public Shared Sub KEY_STOP(n As Integer)
      SetKeyState(n, Suspended)
    End Sub

    Private Shared Function LetterScan(letter As Char) As Integer
      Dim row1 = "QWERTYUIOP".IndexOf(letter)
      If row1 >= 0 Then Return &H10 + row1
      Dim row2 = "ASDFGHJKL".IndexOf(letter)
      If row2 >= 0 Then Return &H1E + row2
      Dim row3 = "ZXCVBNM".IndexOf(letter)
      If row3 >= 0 Then Return &H2C + row3
      Return -1
    End Function

    ' The keyboard flag and scan code of a key (as INKEY$ returns it), where it has one.
    Private Shared Function Decode(key As String, ByRef flags As Integer, ByRef scan As Integer) As Boolean
      If key.Length <> 1 Then Return False
      Dim c = AscW(key(0))
      flags = 0
      Select Case c
        Case 27 : scan = 1 : Return True
        Case 13 : scan = &H1C : Return True
        Case 9 : scan = &HF : Return True
        Case 8 : scan = &HE : Return True
        Case 32 : scan = &H39 : Return True
        Case 1 To 26 : flags = 4 : scan = LetterScan(ChrW(c - 1 + AscW("A"c))) : Return True ' Ctrl+letter
        Case AscW("a"c) To AscW("z"c) : scan = LetterScan(ChrW(c - 32)) : Return True
        Case AscW("A"c) To AscW("Z"c) : flags = 1 : scan = LetterScan(ChrW(c)) : Return True
        Case AscW("1"c) To AscW("9"c) : scan = 2 + c - AscW("1"c) : Return True
        Case AscW("0"c) : scan = 11 : Return True
      End Select
      Return False
    End Function

    ' Called for every key before INKEY returns it: True when a key trap takes it (the program doesn't get the key).
    Friend Shared Function TryTrap(key As String) As Boolean
      If Not m_keyTraps Then Return False
      Dim flags, scan As Integer
      Dim decoded = Decode(key, flags, scan)
      For Each kv In m_traps
        If Not kv.Key.StartsWith("KEY", StringComparison.Ordinal) Then Continue For
        Dim t = kv.Value
        If t.State = Off Then Continue For
        Dim hit = False
        If t.KeyText IsNot Nothing Then
          hit = (key = t.KeyText)
        ElseIf t.KeyFlags >= 0 AndAlso decoded Then
          ' Shift (1 to 3) is matched by either shift flag.
          hit = (scan = t.KeyScan AndAlso ((t.KeyFlags And 3) <> 0) = ((flags And 3) <> 0) AndAlso (t.KeyFlags And 4) = (flags And 4))
        End If
        If hit Then
          t.Pending = True
          Return True
        End If
      Next
      Return False
    End Function

  End Class

End Namespace
