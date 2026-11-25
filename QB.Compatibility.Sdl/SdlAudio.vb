Option Explicit On
Option Strict On
Option Infer On

Imports System.Runtime.InteropServices
Imports SDL2

Namespace Global.QB

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
      Dim have As New SDL.SDL_AudioSpec()
      ' No device name: SDL opens the system's default playback device (the first device it lists isn't necessarily the default).
      m_device = OpenDefaultDevice(want, have)
      If m_device = 0 Then Throw New InvalidOperationException(SDL.SDL_GetError())
      SDL.SDL_PauseAudioDevice(m_device, 0)
    End Sub

    ' (SDL2-CS only takes a device name as a string and can't pass the null that selects the default device.)
    <DllImport("SDL2", EntryPoint:="SDL_OpenAudioDevice", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function SDL_OpenAudioDevice(device As IntPtr, iscapture As Integer, ByRef desired As SDL.SDL_AudioSpec, ByRef obtained As SDL.SDL_AudioSpec, allowedChanges As Integer) As UInteger
    End Function

    Private Shared Function OpenDefaultDevice(want As SDL.SDL_AudioSpec, ByRef have As SDL.SDL_AudioSpec) As UInteger
      Return SDL_OpenAudioDevice(IntPtr.Zero, 0, want, have, 0)
    End Function

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
