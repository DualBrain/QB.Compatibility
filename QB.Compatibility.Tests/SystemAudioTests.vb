Option Explicit On
Option Strict On
Option Infer On

Imports Xunit

Namespace QB.Tests

  ' The system audio hands raw 16-bit mono PCM to a player's input; a "player" that writes it to a file shows what was sent.
  <Collection("Audio")>
  Public Class SystemAudioTests

    Private Shared Function PlayThroughFile(play As Action(Of SystemAudio)) As Byte()
      If OperatingSystem.IsWindows() Then Return Nothing
      Dim file = IO.Path.Combine(IO.Path.GetTempPath(), "qbaudio-" & Guid.NewGuid().ToString("N") & ".raw")
      SystemAudio.PlayerCommand = "/bin/sh"
      SystemAudio.PlayerArguments = "-c ""cat > " & file & """"
      Try
        Dim device As New SystemAudio
        play(device)
        device.Close()                               ' (closes the player's input: it ends, with everything written)
        For i = 1 To 100
          If IO.File.Exists(file) AndAlso IO.File.ReadAllBytes(file).Length > 0 Then Exit For
          Threading.Thread.Sleep(50)
        Next
        Threading.Thread.Sleep(100)
        Return If(IO.File.Exists(file), IO.File.ReadAllBytes(file), Array.Empty(Of Byte)())
      Finally
        SystemAudio.PlayerCommand = "" : SystemAudio.PlayerArguments = ""
        If IO.File.Exists(file) Then IO.File.Delete(file)
      End Try
    End Function

    <Fact>
    Public Sub ToneIsASquareWaveOfTheRightLength()
      Dim samples = SystemAudio.Render(440, 0.1)
      Assert.Equal(2205, samples.Length)
      Assert.Contains(samples, Function(s) s > 1000)
      Assert.Contains(samples, Function(s) s < -1000)
      Assert.Equal(0, SystemAudio.Render(0, 0.1).Max())    ' a rest is silence
    End Sub

    <Fact>
    Public Sub QueuedSecondsCountsDownLikeAClock()
      If OperatingSystem.IsWindows() Then Return
      SystemAudio.PlayerCommand = "/bin/sh"
      SystemAudio.PlayerArguments = "-c ""cat > /dev/null"""
      Try
        Dim device As New SystemAudio
        device.Tone(440, 0.5)
        device.Tone(0, 0.5)
        Dim queued = device.QueuedSeconds
        Assert.InRange(queued, 0.9, 1.0)
        device.Clear()
        Assert.Equal(0, device.QueuedSeconds, 2)
        device.Close()
      Finally
        SystemAudio.PlayerCommand = "" : SystemAudio.PlayerArguments = ""
      End Try
    End Sub

    <Fact>
    Public Sub TonesReachThePlayerAsPcm()
      Dim bytes = PlayThroughFile(Sub(d)
                                    d.Tone(440, 0.1)
                                    d.Tone(0, 0.05)
                                  End Sub)
      If bytes Is Nothing Then Return
      Assert.Equal((2205 + 1102) * 2, bytes.Length)
    End Sub

  End Class

End Namespace
