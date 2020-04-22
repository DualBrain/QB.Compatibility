Option Explicit On
Option Strict On
Option Infer On

Imports System.Drawing
Imports System.Windows.Forms
Imports QB.Core

Namespace Global.QB

  Public Enum LineOption
    None
    B
    BF
  End Enum

  Public Enum PutOption
    PUT_PSET
    PUT_XOR
  End Enum

  Public NotInheritable Class Video

    Private Declare Function GetAsyncKeyState Lib "user32" (vkey As Integer) As Short

    Private Shared m_display As Image = New Bitmap(640, 480)

    Private Shared m_pictureBox As System.Windows.Forms.PictureBox

    Public Shared WriteOnly Property PictureBox As System.Windows.Forms.PictureBox
      Set(value As System.Windows.Forms.PictureBox)
        m_pictureBox = value
        m_pictureBox.Image = m_display
      End Set
    End Property

    Private Sub New()
    End Sub

    Private Shared m_cursorRow As Integer
    Private Shared m_cursorCol As Integer
    Private Shared m_textH As Integer = 14
    Private Shared m_textW As Integer = 8
    Private Shared m_palette() As Color = {System.Drawing.Color.Black,
                                           System.Drawing.Color.DarkBlue,
                                           System.Drawing.Color.DarkGreen,
                                           System.Drawing.Color.DarkCyan,
                                           System.Drawing.Color.DarkRed,
                                           System.Drawing.Color.DarkMagenta,
                                           System.Drawing.Color.Brown,
                                           System.Drawing.Color.LightGray,
                                           System.Drawing.Color.Gray,
                                           System.Drawing.Color.Blue,
                                           System.Drawing.Color.LightGreen,
                                           System.Drawing.Color.Cyan,
                                           System.Drawing.Color.Red,
                                           System.Drawing.Color.Magenta,
                                           System.Drawing.Color.Yellow,
                                           System.Drawing.Color.White}

    Public Shared Sub PRINT()
      m_cursorRow += 1
      m_cursorCol = 1
    End Sub

    Public Shared Sub PRINT(text$, Optional noCr As Boolean = False)
      'TODO: (Possibly) Need to take into account the usage of TAB
      '      Tab should return a token that can then be used
      '      within the PRINT statement in order to do 
      '      specific formatting.  This is because the TAB
      '      could be called after other formatting (USING) takes
      '      place.  So should determine the total output and then
      '      adjust for TAB(s).
      Using g = Graphics.FromImage(m_display)
        Dim x = (m_cursorCol - 1) * m_textW
        Dim y = (m_cursorRow - 1) * m_textH
        Using b = New SolidBrush(m_palette(m_bgColor))
          g.FillRectangle(b, x, y, m_textW * text$.Length, m_textH)
        End Using
        Using f = New Font("Consolas", 9, FontStyle.Regular)
          Using b = New SolidBrush(m_palette(m_fgColor))
            g.DrawString(text$, f, b, x, y)
          End Using
        End Using
      End Using
      m_pictureBox.Image = m_display ': m_pictureBox.Refresh()
      If noCr Then
        m_cursorCol += If(text?.Length, 0)
      Else
        m_cursorCol = 1
        m_cursorRow += 1
      End If
    End Sub

    Public Shared Sub LOCATE(Optional row% = -1, Optional column% = -1, Optional a% = -1, Optional b% = -1, Optional c% = -1)
      If a <> 0 OrElse b <> 0 OrElse c <> 0 Then
      End If
      If row% = -1 AndAlso column% > 0 Then
        m_cursorCol = column%
      ElseIf column% = -1 AndAlso row% > 0 Then
        System.Console.CursorTop = row% - 1
        m_cursorRow = row%
      ElseIf row% > 0 AndAlso column% > 0 Then
        m_cursorCol = column%
        m_cursorRow = row%
      End If
    End Sub

    Public Shared Sub PSET(x#, y#, color#)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub CIRCLE(x#, y#, color#)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub CIRCLE(x#, y#, color1#, color2#)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub CIRCLE(x#, y#, color1#, color2#, value1#, value2#)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub CIRCLE(x#, y#, color1#, color2#, value1#, value2#, value3#)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub QbPaint(x#, y#, color#)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub QbPaint(x#, y#, color1#, color2#)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub LINE(x1#, y1#, x2#, y2#, attr%, Optional lo As LineOption = LineOption.None)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub PUT(x#, y#, value&(), po As PutOption)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub PUT(x#, y#, value&, po As PutOption)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub [GET](x1#, y1#, x2#, y2#, address&)
      Throw New NotImplementedException
    End Sub

    Public Shared Async Function LineInputAsync(prompt$) As Task(Of String)
      PRINT(prompt$)
      Dim result$
      Do
        Await Task.Delay(1)
        Dim a$ = INKEY()
        If a$?.Length > 0 Then
          If a$ = Chr(13) Then
            Return result$
          Else
            result$ &= a$
            PRINT(a$, True)
          End If
        End If
      Loop
    End Function

    'Private Shared m_bgBrush As Brush = Brushes.Black
    'Private Shared m_fgBrush As Brush = Brushes.White

    Private Shared m_bgColor As Integer = 0
    Private Shared m_fgColor As Integer = 7

    Public Shared Sub COLOR(fg%)
      'If fg% > 15 Then fg% -= 16
      'Throw New NotImplementedException
      m_fgColor = fg
    End Sub

    Public Shared Sub COLOR(fg%, bg%)
      'Throw New NotImplementedException
      m_fgColor = fg
      m_bgColor = bg
    End Sub

    Public Shared Sub CLS()
      Using g = Graphics.FromImage(m_display)
        Using b = New SolidBrush(m_palette(m_bgColor))
          g.FillRectangle(b, 0, 0, m_display.Width, m_display.Height)
        End Using
      End Using
      m_cursorCol = 1
      m_cursorRow = 1
    End Sub

    Public Shared Sub CLS(viewport As Integer)
      Select Case viewport
        Case 0, 1, 2
          CLS() 'TODO: For now, just call the base.
        Case Else
          Throw New ArgumentException
      End Select
    End Sub

    Public Shared Async Function InputAsync(prompt$) As Task(Of String)
      PRINT(prompt$)
      Dim result$
      Do
        Await Task.Delay(1)
        Dim a$ = INKEY()
        If a$?.Length > 0 Then
          If a$ = Chr(13) Then
            Return result$
          Else
            result$ &= a$
            PRINT(a$, True)
          End If
        End If
      Loop
    End Function

#Region "SCREEN (Function)"

    'Public Shared Function SCREEN(r%, c%) As Integer
    '  Return 0
    'End Function

    Public Shared Function SCREEN%(row%, column%, Optional colr% = 0)
      Throw New NotImplementedException
      If colr% = 0 Then
        'Dim ch = ConsoleEx.ReadChar(column - 1, row - 1)
        'If ch IsNot Nothing Then
        '  Select Case CStr(ch)
        '    Case "█" : Return 219
        '    Case "▄" : Return 220
        '    Case "▌" : Return 221
        '    Case "▐" : Return 222
        '    Case "▀" : Return 223
        '      'Case "√" : Return 251
        '    Case Else
        '      Return QBAsc(CChar(ch))
        '  End Select
        'Else
        '  Return 0
        'End If
      Else
        Return 0
      End If
      Return 0
    End Function

#End Region

#Region "SCREEN (Statement)"

    Public Shared Sub SCREEN(mode%, colr%, active%, visible%)
      If mode <> 0 OrElse colr <> 0 OrElse active <> 0 OrElse visible <> 0 Then
      End If
      Throw New NotImplementedException
    End Sub

    Public Shared Sub SCREEN(mode%)
      If m_pictureBox IsNot Nothing AndAlso
         m_pictureBox.Image IsNot Nothing Then
        m_pictureBox.Image = Nothing
      End If
      Select Case mode
        Case 0 ' Text Mode
          m_display = New Bitmap(640, 480) ' 80 x 25 text
          m_textW = 8 : m_textH = 19
          If m_pictureBox IsNot Nothing Then m_pictureBox.Image = m_display
        Case 9
          m_display = New Bitmap(640, 350) ' 80 x 25 text
          m_textW = 8 : m_textH = 14
          If m_pictureBox IsNot Nothing Then m_pictureBox.Image = m_display
        Case Else
          Throw New NotImplementedException
      End Select
    End Sub

#End Region

    Public Shared Sub VIEW_PRINT(row1%, row2%)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub PALETTE(color%, display%)
      If color < 0 OrElse color > 16 Then
        Throw New ArgumentOutOfRangeException(NameOf(color))
      End If
      If display < 0 OrElse display > 16 Then
        Throw New ArgumentOutOfRangeException(NameOf(display))
      End If
      m_palette(color) = System.Drawing.Color.FromArgb(QBColor(display))
    End Sub

    Public Shared Sub QbWidth(Optional a% = -1, Optional b% = -1)
      If a <> 80 OrElse b <> 25 Then
        Throw New NotImplementedException
      End If
    End Sub

    Public Shared m_keys As New Stack(Of Keys)

    Public Shared Function INKEY$()
      If m_keys.Count > 0 Then
        Dim key = m_keys.Pop

        Dim shift = GetAsyncKeyState(&H10) <> 0
        Dim ctrl = GetAsyncKeyState(&H11) <> 0
        Dim alt = GetAsyncKeyState(&H12) <> 0

        Dim adder = 0
        If shift Then
          adder = 0
        ElseIf ctrl Then
          adder = 10
        ElseIf alt Then
          adder = 20
        End If

        Select Case key
          Case Keys.Delete
            Return QBChr(0) & QBChr(83)
          Case Keys.Home
            Return QBChr(0) & QBChr(71)
          Case Keys.Up
            Return QBChr(0) & QBChr(72)
          Case Keys.PageUp
            Return QBChr(0) & QBChr(73)
          Case Keys.End
            Return QBChr(0) & QBChr(79)
          Case Keys.Down
            Return QBChr(0) & QBChr(80)
          Case Keys.PageDown
            Return QBChr(0) & QBChr(81)

          Case Keys.Left
            Return QBChr(0) & QBChr(75)
          Case Keys.Right
            Return QBChr(0) & QBChr(77)

          Case Keys.F1
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(84 + adder), QBChr(59))
          Case Keys.F2
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(85 + adder), QBChr(60))
          Case Keys.F3
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(86 + adder), QBChr(61))
          Case Keys.F4
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(87 + adder), QBChr(62))
          Case Keys.F5
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(88 + adder), QBChr(63))
          Case Keys.F6
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(89 + adder), QBChr(64))
          Case Keys.F7
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(80 + adder), QBChr(65))
          Case Keys.F8
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(91 + adder), QBChr(66))
          Case Keys.F9
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(92 + adder), QBChr(67))
          Case Keys.F10
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(93 + adder), QBChr(68))
          Case Keys.F11
            If shift Then
              Return QBChr(0) & QBChr(135)
            ElseIf ctrl Then
              Return QBChr(0) & QBChr(137)
            ElseIf alt Then
              Return QBChr(0) & QBChr(139)
            Else
              Return QBChr(0) & QBChr(133)
            End If
          Case Keys.F12
            If shift Then
              Return QBChr(0) & QBChr(136)
            ElseIf ctrl Then
              Return QBChr(0) & QBChr(138)
            ElseIf alt Then
              Return QBChr(0) & QBChr(140)
            Else
              Return QBChr(0) & QBChr(134)
            End If
          Case Else
            Return ChrW(key)
        End Select
      Else
        Return ""
      End If
    End Function

#Region "SLEEP"

    Public Shared Sub SLEEP()
      Throw New NotImplementedException
      Do
        If Not String.IsNullOrEmpty(INKEY) Then
          Exit Do
        End If
        Threading.Thread.Sleep(100)
      Loop
    End Sub

    Public Shared Sub SLEEP(seconds As Integer)
      Throw New NotImplementedException
      Dim till = DateAdd(DateInterval.Second, seconds, Now)
      Do
        If Not String.IsNullOrEmpty(INKEY) OrElse
           Now >= till Then
          Exit Do
        End If
        Threading.Thread.Sleep(100)
      Loop
    End Sub

#End Region

    Public Shared Function POINT%(x#, y#)
      Throw New NotImplementedException
    End Function

  End Class

End Namespace