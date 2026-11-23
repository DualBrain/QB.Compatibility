' BEEP  -  https://qbasic.net/en/qb-manual/Statement/BEEP.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Speaker
Imports QB.Graphics

Namespace Examples

  Friend Module BeepExample

    Public Sub Run()
    CLS()
    Dim response As String = ""
    Do
      INPUT("Hear a beep (Y or N)", response)
      Dim r = UCase(Mid(response, 1, 1))
      If r <> "Y" OrElse r = "N" Then Exit Do
      BEEP()
    Loop
    End Sub

  End Module

End Namespace
