' ON KEY  -  https://qbasic.net/en/qb-manual/Statement/ON%20KEY.htm
' A VB translation of the QBasic example.  KEY 15, CHR$(&H4) + CHR$(&H1F) becomes KEY_DEFINE(15, &H4, &H1F) (Ctrl+S).

Imports QB
Imports QB.Events
Imports QB.Graphics

Namespace Examples

  Friend Module OnKeyExample

    Private Sub PauseHandler()
      SLEEP(1)
    End Sub

    Public Sub Run()
      Const ESC As Integer = 27
      KEY_DEFINE(15, &H4, &H1F)        ' Set up Ctrl+S as KEY 15.
      ON_KEY(15, AddressOf PauseHandler)
      KEY_ON(15)
      Do While INKEY() <> Chr(ESC)
        PRINT("Press Esc to stop, Ctrl+S to pause.")
        PRINT()
        Threading.Thread.Sleep(20) ' (Slows the scrolling down.)
      Loop
    End Sub

  End Module

End Namespace
