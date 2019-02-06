Option Explicit On
Option Strict On
Option Infer On

Namespace QB

  Public NotInheritable Class Multimedia

    Private Sub New()
    End Sub

    ' Graphics

    Public Shared Sub COLOR(fg%, bg%)
      Console.ForegroundColor = CType(fg, ConsoleColor)
      Console.BackgroundColor = CType(bg, ConsoleColor)
    End Sub

#Region "SCREEN (Function)"

    Public Shared Function SCREEN%(row%, column%, Optional colr% = 0)
      If colr% = 0 Then
        Dim ch = ConsoleEx.ReadChar(column - 1, row - 1)
        If ch IsNot Nothing Then
          Select Case CStr(ch)
            Case "█" : Return 219
            Case "▄" : Return 220
            Case "▌" : Return 221
            Case "▐" : Return 222
            Case "▀" : Return 223
            Case Else
              Return Asc(CChar(ch))
          End Select
        Else
          Return 0
        End If
      Else
        Return 0
      End If
    End Function

#End Region

#Region "SCREEN (Statement)"

    Public Shared Sub SCREEN(mode%, colr%, active%, visible%)

    End Sub

    Public Shared Sub SCREEN(mode%)

    End Sub

#End Region

    ' Sound

    ' Light Pen and Joystick

  End Class

End Namespace