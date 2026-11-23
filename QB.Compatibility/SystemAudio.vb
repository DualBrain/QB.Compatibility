Option Explicit On
Option Strict On
Option Infer On

Imports System.Diagnostics
Imports System.Runtime.InteropServices

Namespace Global.QB

  ''' <summary>
  ''' The default audio device: it needs no library.  The tones are rendered as square waves (16-bit mono PCM) and handed to what the
  ''' operating system has: on Windows the waveform API (winmm), on macOS <c>afplay</c>, and on Linux a player that reads raw audio from
  ''' its input (<c>paplay</c>, <c>pw-cat</c> or <c>aplay</c>).  When there is none, creating the device fails and the sound is silent.
  ''' </summary>
  Friend NotInheritable Class SystemAudio
    Implements IAudioDevice

    Friend Const Rate As Integer = 22050
    Private Const Amplitude As Integer = 7000   ' Out of 32767; a square wave is loud.

    ''' <summary>A player that reads raw 16-bit little-endian mono PCM at <see cref="Rate"/> from its input (set by tests, or to choose one).</summary>
    Friend Shared PlayerCommand As String = ""
    Friend Shared PlayerArguments As String = ""

    Private ReadOnly m_lock As New Object
    Private ReadOnly m_pending As New Queue(Of Short())
    Private m_end As DateTime = DateTime.UtcNow         ' when everything that was queued has been played (a clock, not the device)
    Private m_thread As Threading.Thread
    Private m_closed As Boolean
    Private ReadOnly m_command As String                ' the player of the stream form (empty when the sound goes in whole pieces)
    Private ReadOnly m_arguments As String
    Private m_player As Process

    Public Sub New()
      If PlayerCommand <> "" Then
        m_command = PlayerCommand : m_arguments = PlayerArguments
      ElseIf OperatingSystem.IsWindows() OrElse OperatingSystem.IsMacOS() Then
        m_command = ""
        If OperatingSystem.IsMacOS() AndAlso FindOnPath("afplay") Is Nothing Then Throw New InvalidOperationException("No audio player")
      Else
        If FindOnPath("paplay") IsNot Nothing Then
          m_command = "paplay" : m_arguments = "--raw --rate=" & Rate & " --format=s16le --channels=1"
        ElseIf FindOnPath("pw-cat") IsNot Nothing Then
          m_command = "pw-cat" : m_arguments = "--playback --rate " & Rate & " --format s16 --channels 1 -"
        ElseIf FindOnPath("aplay") IsNot Nothing Then
          m_command = "aplay" : m_arguments = "-q -t raw -f S16_LE -r " & Rate & " -c 1 -"
        Else
          Throw New InvalidOperationException("No audio player")
        End If
      End If
      m_thread = New Threading.Thread(AddressOf Run) With {.IsBackground = True, .Name = "QB audio"}
      m_thread.Start()
    End Sub

    Private Shared Function FindOnPath(name As String) As String
      Dim path = Environment.GetEnvironmentVariable("PATH")
      If String.IsNullOrEmpty(path) Then Return Nothing
      For Each folder In path.Split(IO.Path.PathSeparator)
        If folder = "" Then Continue For
        Dim candidate = IO.Path.Combine(folder, name)
        If IO.File.Exists(candidate) Then Return candidate
      Next
      Return Nothing
    End Function

    ''' <summary>The samples of a tone: a square wave with a short ramp at each end, so that it doesn't click.</summary>
    Friend Shared Function Render(frequency As Double, seconds As Double) As Short()
      Dim count = CInt(Math.Round(seconds * Rate))
      If count <= 0 Then Return Array.Empty(Of Short)()
      Dim samples(count - 1) As Short
      If frequency > 0 Then
        Dim phase = 0.0
        Dim step_ = frequency / Rate
        Dim ramp = Math.Min(22, count \ 2)
        For i = 0 To count - 1
          phase += step_
          If phase >= 1 Then phase -= 1
          Dim a = Amplitude
          If i < ramp Then a = Amplitude * i \ ramp
          If count - 1 - i < ramp Then a = Amplitude * (count - 1 - i) \ ramp
          samples(i) = CShort(If(phase < 0.5, a, -a))
        Next
      End If
      Return samples
    End Function

    Public Sub Tone(frequency As Double, seconds As Double) Implements IAudioDevice.Tone
      Dim samples = Render(frequency, seconds)
      If samples.Length = 0 Then Return
      SyncLock m_lock
        If m_closed Then Return
        Dim start = If(m_end > DateTime.UtcNow, m_end, DateTime.UtcNow)
        m_end = start.AddSeconds(samples.Length / CDbl(Rate))
        m_pending.Enqueue(samples)
        Threading.Monitor.PulseAll(m_lock)
      End SyncLock
    End Sub

    Public ReadOnly Property QueuedSeconds As Double Implements IAudioDevice.QueuedSeconds
      Get
        SyncLock m_lock
          Return Math.Max(0, (m_end - DateTime.UtcNow).TotalSeconds)
        End SyncLock
      End Get
    End Property

    Public Sub Clear() Implements IAudioDevice.Clear
      SyncLock m_lock
        m_pending.Clear()
        m_end = DateTime.UtcNow
        StopPlayer(True)
      End SyncLock
      If OperatingSystem.IsWindows() Then Windows.Stop_()
    End Sub

    Public Sub Close() Implements IAudioDevice.Close
      SyncLock m_lock
        m_closed = True
        Threading.Monitor.PulseAll(m_lock)
      End SyncLock
      m_thread.Join(2000)      ' (what is waiting is handed over first)
      SyncLock m_lock
        StopPlayer(False)
      End SyncLock
    End Sub

    ' ---- the thread that hands the sound to the system ----------------------------------------------------------------------

    Private Sub Run()
      Do
        Dim chunks As New List(Of Short())
        SyncLock m_lock
          Do While m_pending.Count = 0 AndAlso Not m_closed
            Threading.Monitor.Wait(m_lock)
          Loop
          If m_pending.Count = 0 Then Return
          If m_command <> "" Then
            chunks.Add(m_pending.Dequeue())
          Else
            Do While m_pending.Count > 0     ' (a player that takes whole pieces gets what is waiting in one, so there is no gap)
              chunks.Add(m_pending.Dequeue())
            Loop
          End If
        End SyncLock
        Try
          If m_command <> "" Then WriteToPlayer(chunks(0)) Else PlayPiece(chunks)
        Catch ex As Exception
          ' (a player that has gone away: the sound is lost, and the timing goes on)
          SyncLock m_lock
            StopPlayer(True)
          End SyncLock
        End Try
      Loop
    End Sub

    Private Shared Function ToBytes(chunks As IEnumerable(Of Short())) As Byte()
      Dim total = chunks.Sum(Function(c) c.Length)
      Dim bytes(total * 2 - 1) As Byte
      Dim at = 0
      For Each c In chunks
        Buffer.BlockCopy(c, 0, bytes, at, c.Length * 2)
        at += c.Length * 2
      Next
      Return bytes
    End Function

    Private Sub WriteToPlayer(samples As Short())
      Dim player As Process
      SyncLock m_lock
        If m_player Is Nothing OrElse m_player.HasExited Then
          Dim info As New ProcessStartInfo(m_command, m_arguments) With {.UseShellExecute = False, .RedirectStandardInput = True, .RedirectStandardError = True, .RedirectStandardOutput = True}
          m_player = Process.Start(info)
          m_player.BeginErrorReadLine()
          m_player.BeginOutputReadLine()
        End If
        player = m_player
      End SyncLock
      Dim bytes = ToBytes({samples})
      player.StandardInput.BaseStream.Write(bytes, 0, bytes.Length)
      player.StandardInput.BaseStream.Flush()
    End Sub

    Private Sub StopPlayer(kill As Boolean)
      If m_player Is Nothing Then Return
      Try
        If kill Then m_player.Kill() Else m_player.StandardInput.Close()
      Catch ex As Exception
      End Try
      m_player = Nothing
    End Sub

    ' A WAV file (or the same in memory) of the pieces, for the systems that play whole pieces of sound.
    Private Shared Function WaveFile(chunks As IEnumerable(Of Short())) As Byte()
      Dim data = ToBytes(chunks)
      Using ms As New IO.MemoryStream
        Using w As New IO.BinaryWriter(ms)
          w.Write(Text.Encoding.ASCII.GetBytes("RIFF")) : w.Write(36 + data.Length)
          w.Write(Text.Encoding.ASCII.GetBytes("WAVEfmt ")) : w.Write(16)
          w.Write(CShort(1)) : w.Write(CShort(1)) : w.Write(Rate) : w.Write(Rate * 2) : w.Write(CShort(2)) : w.Write(CShort(16))
          w.Write(Text.Encoding.ASCII.GetBytes("data")) : w.Write(data.Length)
          w.Write(data)
        End Using
        Return ms.ToArray()
      End Using
    End Function

    Private Shared Sub PlayPiece(chunks As List(Of Short()))
      Dim wav = WaveFile(chunks)
      If OperatingSystem.IsWindows() Then
        Windows.Play(wav)
      Else
        Dim file = IO.Path.Combine(IO.Path.GetTempPath(), "qb" & Environment.ProcessId & "-" & Guid.NewGuid().ToString("N") & ".wav")
        IO.File.WriteAllBytes(file, wav)
        Try
          Dim info As New ProcessStartInfo("afplay") With {.UseShellExecute = False}
          info.ArgumentList.Add(file)
          Using p = Process.Start(info)
            p.WaitForExit()
          End Using
        Finally
          Try
            IO.File.Delete(file)
          Catch ex As Exception
          End Try
        End Try
      End If
    End Sub

    ' winmm.dll: PlaySound plays a WAV held in memory.
    Private NotInheritable Class Windows
      <DllImport("winmm.dll", EntryPoint:="PlaySoundW", ExactSpelling:=True)>
      Private Shared Function PlaySound(sound As IntPtr, module_ As IntPtr, flags As UInteger) As Boolean
      End Function

      Friend Shared Sub Play(wav As Byte())
        Dim handle = GCHandle.Alloc(wav, GCHandleType.Pinned)
        Try
          PlaySound(handle.AddrOfPinnedObject(), IntPtr.Zero, &H4UI Or &H2UI)    ' SND_MEMORY, SND_NODEFAULT: waits until it is played
        Finally
          handle.Free()
        End Try
      End Sub

      Friend Shared Sub Stop_()
        PlaySound(IntPtr.Zero, IntPtr.Zero, 0UI)
      End Sub
    End Class

  End Class

End Namespace
