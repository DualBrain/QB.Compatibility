Option Explicit On
Option Strict On
Option Infer On

Imports System.Runtime.InteropServices

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

End Namespace
