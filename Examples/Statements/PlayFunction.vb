' PLAY_FUNCTION  -  https://qbasic.net/en/qb-manual/Function/PLAY.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Multimedia
Imports QB.Video

Namespace Examples

  Friend Module PlayFunctionExample

    Public Sub Run()
    Dim music = "MBT180o2P2P8L8GGGL2E-P24P8L8FFFL2D"
    PLAY(music)
    Do While PLAY(0) > 5 : Loop
    PRINT("Just about done!")
    End Sub

  End Module

End Namespace
