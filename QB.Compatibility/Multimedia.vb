Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  Public MustInherit Class Multimedia

    Protected Sub New()
    End Sub

    Private Shared ReadOnly m_sync As New Object
    Private Shared m_device As IAudioDevice

    ' PLAY's settings last from one PLAY to the next.
    Private Shared m_octave As Integer = 4
    Private Shared m_length As Integer = 4
    Private Shared m_tempo As Integer = 120
    Private Shared m_style As Integer = 0          ' 0 = normal, 1 = legato, 2 = staccato
    Private Shared m_background As Boolean

    ' Notes queued by PLAY, as the time (in seconds of sound queued so far) at which each ends.
    Private Shared ReadOnly m_noteEnds As New List(Of Double)
    Private Shared m_queuedTotal As Double

    ''' <summary>
    ''' Where sound is heard.  By default an SDL2 audio device is opened the first time something is
    ''' played (or a silent one, if there is no sound device); assign another device (for example a
    ''' HeadlessAudio) to use it instead.
    ''' </summary>
    Public Shared Property Audio As IAudioDevice
      Get
        Return m_device
      End Get
      Set(value As IAudioDevice)
        SyncLock m_sync
          If m_device IsNot Nothing Then m_device.Close()
          m_device = value
          m_noteEnds.Clear()
          m_queuedTotal = 0
        End SyncLock
      End Set
    End Property

    ''' <summary>Creates the audio device when sound is first played (SDL2 audio, with QB.Compatibility.Sdl); without one the sound is silent.</summary>
    Public Shared Property AudioFactory As Func(Of IAudioDevice)

    Private Shared Function Device() As IAudioDevice
      If m_device Is Nothing Then
        SyncLock m_sync
          If m_device Is Nothing Then
            Try
              Dim factory = AudioFactory
              m_device = If(factory Is Nothing, New SilentAudio, factory())
            Catch ex As Exception
              m_device = New SilentAudio
            End Try
          End If
        End SyncLock
      End If
      Return m_device
    End Function

    Private Shared Sub Queue(frequency As Double, seconds As Double)
      If seconds <= 0 Then Return
      Device().Tone(frequency, seconds)
      m_queuedTotal += seconds
    End Sub

    Private Shared Sub WaitUntilFinished()
      Dim d = Device()
      Do While d.QueuedSeconds > 0.005
        Events.Dispatch()
        Threading.Thread.Sleep(5)
      Loop
    End Sub

    ''' <summary>Plays a tone: <paramref name="frequency"/> 37 to 32767 Hz for <paramref name="duration"/> clock ticks (18.2 per second).</summary>
    Friend Shared Sub SOUND(frequency As Double, duration As Double)
      Dim f = Math.Round(frequency)
      Dim ticks = Math.Round(duration)
      If f < 37 OrElse f > 32767 OrElse ticks < 0 OrElse ticks > 65535 Then
        Throw New ArgumentException("Illegal function call")
      End If
      If ticks = 0 Then Return
      Queue(f, ticks / 18.2)
      WaitUntilFinished()
    End Sub

    ''' <summary>Sounds the speaker (800 Hz for a quarter of a second); PRINT CHR$(7) does the same.</summary>
    Public Shared Sub BEEP()
      If TypeOf Device() Is SilentAudio Then Video.RingBell()   ' no sound device: the terminal's bell
      Queue(800, 0.25)
      WaitUntilFinished()
    End Sub

    ''' <summary>
    ''' The number of notes still waiting to be played in the background by PLAY (MB).
    ''' </summary>
    Friend Shared Function PLAY(n As Integer) As Integer
      Events.Dispatch()
      Return PendingNotes()
    End Function

    Friend Shared Function PendingNotes() As Integer
      If m_device Is Nothing Then Return 0
      Dim played = m_queuedTotal - m_device.QueuedSeconds
      SyncLock m_sync
        m_noteEnds.RemoveAll(Function(t) t <= played + 0.0001)
        Return m_noteEnds.Count
      End SyncLock
    End Function

    Private Shared Function ReadNumber(s As String, ByRef i As Integer, defaultValue As Integer) As Integer
      Do While i < s.Length AndAlso s(i) = " "c ' (STR$ puts a space in front of positive numbers.)
        i += 1
      Loop
      If i < s.Length AndAlso s(i) = "="c Then
        Throw New ArgumentException("Illegal function call: PLAY reads a variable with = only through VARPTR$ (L=" & "VARPTR$(n))")
      End If
      Dim start = i
      Do While i < s.Length AndAlso Char.IsDigit(s(i))
        i += 1
      Loop
      If i = start Then Return defaultValue
      Return Integer.Parse(s.Substring(start, i - start), Globalization.CultureInfo.InvariantCulture)
    End Function

    Private Shared Function ReadDots(s As String, ByRef i As Integer) As Integer
      Dim dots = 0
      Do While i < s.Length AndAlso s(i) = "."c
        dots += 1 : i += 1
      Loop
      Return dots
    End Function

    ' The length in seconds of a note of 1/length (a whole note is 4 quarter notes), with dots.
    Private Shared Function NoteSeconds(length As Integer, dots As Integer) As Double
      If length < 1 OrElse length > 64 Then Throw New ArgumentException("Illegal function call")
      Dim seconds = 60.0 / m_tempo * 4 / length
      Dim extra = seconds / 2
      For d = 1 To dots
        seconds += extra
        extra /= 2
      Next
      Return seconds
    End Function

    ' A note: sounds for most of its length (the music style decides how much) and rests for the remainder.
    Private Shared Sub PlayNote(frequency As Double, seconds As Double)
      Dim sounding = seconds
      If frequency > 0 Then
        Select Case m_style
          Case 0 : sounding = seconds * 7 / 8
          Case 2 : sounding = seconds * 3 / 4
        End Select
      End If
      Queue(frequency, sounding)
      If seconds - sounding > 0.0001 Then Queue(0, seconds - sounding)
      SyncLock m_sync
        m_noteEnds.Add(m_queuedTotal)
      End SyncLock
    End Sub

    Private Shared Function Frequency(index As Integer) As Double
      ' Octave 0 starts at C = 32.7 Hz, so octave 3 holds middle C (261.6 Hz) and A of octave 3 (index 45) is 440 Hz.
      Return 440.0 * Math.Pow(2.0, (index - 45) / 12.0)
    End Function

    ''' <summary>
    ''' Plays music written in QBasic's music macro language: A to G (with # + - and a length and dots),
    ''' O n (octave 0 to 6), &lt; and &gt; (octave down and up), N n (note 0 to 84), L n (length), P n (pause),
    ''' T n (tempo 32 to 255), MF and MB (foreground and background), MN, ML and MS (normal, legato,
    ''' staccato).  Settings last until the next PLAY.  X and = work with VARPTR$ (X plays that string, = reads a variable).
    ''' </summary>
    Friend Shared Sub PLAY(commands As String)

      Dim s = If(Core.ExpandExecute(commands), "").ToUpperInvariant()
      Dim i = 0

      Do While i < s.Length

        Dim c = s(i)
        i += 1

        Select Case c

          Case " "c, ";"c, ControlChars.Tab, ControlChars.Cr, ControlChars.Lf

          Case "A"c To "G"c
            Dim semitone = "C D EF G A B".IndexOf(c)
            Do While i < s.Length AndAlso (s(i) = "#"c OrElse s(i) = "+"c OrElse s(i) = "-"c)
              semitone += If(s(i) = "-"c, -1, 1)
              i += 1
            Loop
            Dim length = ReadNumber(s, i, m_length)
            Dim dots = ReadDots(s, i)
            Dim index = m_octave * 12 + semitone
            PlayNote(If(index < 0 OrElse index > 83, 0, Frequency(index)), NoteSeconds(length, dots))

          Case "N"c
            Dim n = ReadNumber(s, i, 0)
            If n < 0 OrElse n > 84 Then Throw New ArgumentException("Illegal function call")
            Dim dots = ReadDots(s, i)
            PlayNote(If(n = 0, 0, Frequency(n - 1)), NoteSeconds(m_length, dots))

          Case "P"c
            Dim length = ReadNumber(s, i, 0)
            Dim dots = ReadDots(s, i)
            Dim seconds = NoteSeconds(length, dots)
            Queue(0, seconds)
            SyncLock m_sync
              m_noteEnds.Add(m_queuedTotal)
            End SyncLock

          Case "O"c
            Dim n = ReadNumber(s, i, -1)
            If n < 0 OrElse n > 6 Then Throw New ArgumentException("Illegal function call")
            m_octave = n

          Case ">"c
            m_octave = Math.Min(6, m_octave + 1)

          Case "<"c
            m_octave = Math.Max(0, m_octave - 1)

          Case "L"c
            Dim n = ReadNumber(s, i, 0)
            If n < 1 OrElse n > 64 Then Throw New ArgumentException("Illegal function call")
            m_length = n

          Case "T"c
            Dim n = ReadNumber(s, i, 0)
            If n < 32 OrElse n > 255 Then Throw New ArgumentException("Illegal function call")
            m_tempo = n

          Case "M"c
            If i >= s.Length Then Throw New ArgumentException("Illegal function call")
            Select Case s(i)
              Case "F"c : m_background = False
              Case "B"c : m_background = True
              Case "N"c : m_style = 0
              Case "L"c : m_style = 1
              Case "S"c : m_style = 2
              Case Else : Throw New ArgumentException("Illegal function call")
            End Select
            i += 1

          Case "X"c
            Throw New NotSupportedException("PLAY's X (execute substring) command isn't supported.")

          Case Else
            Throw New ArgumentException("Illegal function call")

        End Select

      Loop

      If Not m_background Then WaitUntilFinished()

    End Sub

  End Class

End Namespace
