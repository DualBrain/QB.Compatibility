' SOUND  -  https://qbasic.net/en/qb-manual/Statement/SOUND.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Multimedia
Imports QB.Video

Namespace Examples

  Friend Module SoundExample

    Public Sub Run()
    For i = 440 To 1000 Step 5
      SOUND(i, i / 1000)
    Next
    End Sub

  End Module

End Namespace
