Option Explicit On
Option Strict On
Option Infer On

Imports System.Runtime.InteropServices
Imports SDL2

Namespace Global.QB

  ''' <summary>Where SOUND, PLAY and BEEP are heard.</summary>
  Public Interface IAudioDevice
    ''' <summary>Queues a square-wave tone (a frequency of 0 or less is a rest) after the sound already queued.</summary>
    Sub Tone(frequency As Double, seconds As Double)
    ''' <summary>How many seconds of queued sound haven't been played yet.</summary>
    ReadOnly Property QueuedSeconds As Double
    ''' <summary>Throws away the sound that hasn't been played.</summary>
    Sub Clear()
    Sub Close()
  End Interface

  ''' <summary>
  ''' An audio device for tests: it records the tones and never makes anyone wait
  ''' (the sound counts as played at once).
  ''' </summary>
  Public NotInheritable Class HeadlessAudio
    Implements IAudioDevice

    Public ReadOnly Tones As New List(Of (Frequency As Double, Seconds As Double))

    Public Sub Tone(frequency As Double, seconds As Double) Implements IAudioDevice.Tone
      Tones.Add((frequency, seconds))
    End Sub

    Public ReadOnly Property QueuedSeconds As Double Implements IAudioDevice.QueuedSeconds
      Get
        Return 0
      End Get
    End Property

    Public Sub Clear() Implements IAudioDevice.Clear
      Tones.Clear()
    End Sub

    Public Sub Close() Implements IAudioDevice.Close
    End Sub

    ''' <summary>The total length of the recorded tones and rests.</summary>
    Public ReadOnly Property TotalSeconds As Double
      Get
        Return Tones.Sum(Function(t) t.Seconds)
      End Get
    End Property

  End Class

  ''' <summary>Used when there is no sound device: nothing is heard, but the timing is kept.</summary>
  Friend NotInheritable Class SilentAudio
    Implements IAudioDevice

    Private m_end As DateTime = DateTime.UtcNow

    Public Sub Tone(frequency As Double, seconds As Double) Implements IAudioDevice.Tone
      Dim start = If(m_end > DateTime.UtcNow, m_end, DateTime.UtcNow)
      m_end = start.AddSeconds(seconds)
    End Sub

    Public ReadOnly Property QueuedSeconds As Double Implements IAudioDevice.QueuedSeconds
      Get
        Return Math.Max(0, (m_end - DateTime.UtcNow).TotalSeconds)
      End Get
    End Property

    Public Sub Clear() Implements IAudioDevice.Clear
      m_end = DateTime.UtcNow
    End Sub

    Public Sub Close() Implements IAudioDevice.Close
    End Sub

  End Class

  ''' <summary>The default audio device: SDL2 audio, which works on Windows, Linux and macOS.</summary>
  Friend NotInheritable Class SdlAudio
    Implements IAudioDevice

    Private Const Rate As Integer = 44100
    Private Const Amplitude As Integer = 7000   ' Out of 32767; a square wave is loud.
    Private m_device As UInteger

    Public Sub New()
      If SDL.SDL_InitSubSystem(SDL.SDL_INIT_AUDIO) <> 0 Then
        Throw New InvalidOperationException(SDL.SDL_GetError())
      End If
      Dim want As New SDL.SDL_AudioSpec With {.freq = Rate, .format = SDL.AUDIO_S16, .channels = 1, .samples = 1024}
      Dim have As SDL.SDL_AudioSpec
      ' (SDL2-CS can't be given a null device name, so the first playback device is used.)
      Dim name = SDL.SDL_GetAudioDeviceName(0, 0)
      If String.IsNullOrEmpty(name) Then Throw New InvalidOperationException("No audio device")
      m_device = SDL.SDL_OpenAudioDevice(name, 0, want, have, 0)
      If m_device = 0 Then Throw New InvalidOperationException(SDL.SDL_GetError())
      SDL.SDL_PauseAudioDevice(m_device, 0)
    End Sub

    Public Sub Tone(frequency As Double, seconds As Double) Implements IAudioDevice.Tone
      Dim count = CInt(Math.Round(seconds * Rate))
      If count <= 0 Then Return
      Dim samples(count - 1) As Short
      If frequency > 0 Then
        Dim phase = 0.0
        Dim step_ = frequency / Rate
        Dim ramp = Math.Min(44, count \ 2) ' About a millisecond in and out, so tones don't click.
        For i = 0 To count - 1
          phase += step_
          If phase >= 1 Then phase -= 1
          Dim a = Amplitude
          If i < ramp Then a = Amplitude * i \ ramp
          If count - 1 - i < ramp Then a = Amplitude * (count - 1 - i) \ ramp
          samples(i) = CShort(If(phase < 0.5, a, -a))
        Next
      End If
      Dim handle = GCHandle.Alloc(samples, GCHandleType.Pinned)
      Try
        SDL.SDL_QueueAudio(m_device, handle.AddrOfPinnedObject(), CUInt(count * 2))
      Finally
        handle.Free()
      End Try
    End Sub

    Public ReadOnly Property QueuedSeconds As Double Implements IAudioDevice.QueuedSeconds
      Get
        Return SDL.SDL_GetQueuedAudioSize(m_device) / (Rate * 2.0)
      End Get
    End Property

    Public Sub Clear() Implements IAudioDevice.Clear
      SDL.SDL_ClearQueuedAudio(m_device)
    End Sub

    Public Sub Close() Implements IAudioDevice.Close
      If m_device <> 0 Then SDL.SDL_CloseAudioDevice(m_device)
      m_device = 0
    End Sub

  End Class

End Namespace
