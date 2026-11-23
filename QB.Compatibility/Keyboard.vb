Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>Translates keys into the strings returned by QBasic's INKEY$.</summary>
  Friend NotInheritable Class KeyCodec

    Private Sub New()
    End Sub

    ' Extended keys return Chr(0) followed by a scan code.
    Friend Shared Function Encode(key As ConsoleKey, ch As Char, shift As Boolean, ctrl As Boolean, alt As Boolean) As String

      Select Case key
        Case ConsoleKey.Delete : Return Extended(83)
        Case ConsoleKey.Insert : Return Extended(82)
        Case ConsoleKey.Home : Return Extended(71)
        Case ConsoleKey.UpArrow : Return Extended(72)
        Case ConsoleKey.PageUp : Return Extended(73)
        Case ConsoleKey.End : Return Extended(79)
        Case ConsoleKey.DownArrow : Return Extended(80)
        Case ConsoleKey.PageDown : Return Extended(81)
        Case ConsoleKey.LeftArrow : Return Extended(75)
        Case ConsoleKey.RightArrow : Return Extended(77)
        Case ConsoleKey.F1 To ConsoleKey.F10
          Dim n = key - ConsoleKey.F1
          If shift Then
            Return Extended(84 + n)
          ElseIf ctrl Then
            Return Extended(94 + n)
          ElseIf alt Then
            Return Extended(104 + n)
          Else
            Return Extended(59 + n)
          End If
        Case ConsoleKey.F11
          Return Extended(If(shift, 135, If(ctrl, 137, If(alt, 139, 133))))
        Case ConsoleKey.F12
          Return Extended(If(shift, 136, If(ctrl, 138, If(alt, 140, 134))))
        Case Else
          If ch <> ChrW(0) Then Return ch.ToString
          Return ""
      End Select

    End Function

    Private Shared Function Extended(scan As Integer) As String
      Return ChrW(0) & ChrW(scan)
    End Function

  End Class

End Namespace
