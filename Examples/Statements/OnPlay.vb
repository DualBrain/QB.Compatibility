' ON PLAY  -  https://qbasic.net/en/qb-manual/Statement/ON%20PLAY.htm
' A VB translation of the QBasic example: the handler plays the music again whenever fewer than 3 notes are left.

Imports QB
Imports QB.Events
Imports QB.Multimedia
Imports QB.Video

Namespace Examples

  Friend Module OnPlayExample

    Private music As String = ""
    Private i As Integer

    Private Sub Background()
      i += 1
      LOCATE(1, 1) : PRINT("Background called " & Str(i) & " time(s)", True)
      PLAY(music)
    End Sub

    Public Sub Run()
      ON_PLAY(3, AddressOf Background)
      PLAY_ON()
      music = "MBo3L8ED+ED+Eo2Bo3DCL2o2A"
      PLAY(music)
      LOCATE(2, 1) : PRINT("Press any key to stop.", True)
      Do While INKEY() = "" : Loop
      PLAY_OFF()
    End Sub

  End Module

End Namespace
