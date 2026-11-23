Option Explicit On
Option Strict On
Option Infer On

Imports System.Text

Namespace Global.QB

  ''' <summary>The layout PRINT-style statements share (files and the printer): print zones, TAB, line ends and wrapping.</summary>
  Friend NotInheritable Class PrintFormat

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Lays out text for a device whose print position is <paramref name="column"/> (1-based, updated).  A Tab
    ''' character moves to the next 14-column zone, TAB(n) (see Video.TAB) to column n, and with a
    ''' <paramref name="width"/> (1 to 254) a longer line wraps.  A CR LF is added unless <paramref name="noCr"/>.
    ''' </summary>
    Friend Shared Function Format(text As String, ByRef column As Integer, width As Integer, noCr As Boolean) As String
      Dim sb As New StringBuilder
      For Each c In If(text, "")
        Dim code = AscW(c)
        Select Case code
          Case 13, 10
            sb.Append(c)
            column = 1
          Case 9 ' Next 14-column print zone.
            Dim target = ((column - 1) \ 14 + 1) * 14 + 1
            sb.Append(" "c, target - column)
            column = target
          Case &HE001 To &HE0FF ' TAB(n)
            Dim n = code - &HE000
            If column > n Then sb.Append(vbCrLf) : column = 1
            sb.Append(" "c, n - column)
            column = n
          Case Else
            If width > 0 AndAlso width < 255 AndAlso column > width Then
              sb.Append(vbCrLf)
              column = 1
            End If
            sb.Append(c)
            column += 1
        End Select
      Next
      If Not noCr Then
        sb.Append(vbCrLf)
        column = 1
      End If
      Return sb.ToString
    End Function

  End Class

End Namespace
