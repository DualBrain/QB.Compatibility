Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>
  ''' SOUND and PLAY, together with everything in <see cref="Multimedia"/> (<c>Imports QB.Speaker</c> instead of <c>Imports QB.Multimedia</c>).
  ''' Using either of them makes SDL2 audio the sound device (BEEP alone doesn't need it).
  ''' </summary>
  Public NotInheritable Class Speaker
    Inherits Multimedia

    Private Sub New()
    End Sub

    Shared Sub New()
      Multimedia.AudioFactory = Function() New SdlAudio
    End Sub

  Public Shared Shadows Sub SOUND(frequency As Double, duration As Double)
    Multimedia.SOUND(frequency, duration)
    End Sub
  Public Shared Shadows Function PLAY(n As Integer) As Integer
    Return Multimedia.PLAY(n)
    End Function
  Public Shared Shadows Sub PLAY(commands As String)
    Multimedia.PLAY(commands)
    End Sub

  End Class

End Namespace
