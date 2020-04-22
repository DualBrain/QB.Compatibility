Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  Public NotInheritable Class Multimedia

    ' Sound

    Public Shared Sub PLAY(instructions As String)
      Return
      'TODO: Need to implement play... for now we have silence.
      If String.IsNullOrEmpty(instructions) Then
      End If
      Throw New NotImplementedException
    End Sub

    ' Light Pen and Joystick

  End Class

End Namespace