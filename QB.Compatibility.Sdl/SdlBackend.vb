Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>
  ''' The SDL2 window, audio and game input for QB.Compatibility.  A text-only program doesn't need this assembly (nor SDL2).
  ''' </summary>
  Public NotInheritable Class SdlBackend

    Private Sub New()
    End Sub

    ''' <summary>Makes the SDL2 window, audio and game input what the program uses.</summary>
    Public Shared Sub Use()
      Video.DisplayFactory = Function() New SdlDisplay
      Multimedia.AudioFactory = Function() New SdlAudio
    End Sub

  End Class

End Namespace
