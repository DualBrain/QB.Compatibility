Option Explicit On
Option Strict On
Option Infer On

Imports System.Drawing
Imports System.Windows.Forms
Imports QB.Core

Imports Extensions
Imports System.CodeDom.Compiler

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

    Private Shared m_parent As Form
    Private Shared m_pictureBox As System.Windows.Forms.PictureBox

    'Public Shared WriteOnly Property PictureBox As System.Windows.Forms.PictureBox
    '  Set(value As System.Windows.Forms.PictureBox)
    '    m_pictureBox = value
    '    m_pictureBox.Image = m_display
    '  End Set
    'End Property

    Private Sub New()
    End Sub

    Public Shared Sub Init(parent As Form, pic As PictureBox)
      m_parent = parent
      m_parent.BackColor = Drawing.Color.Black
      m_pictureBox = pic
      m_pictureBox.SizeMode = PictureBoxSizeMode.StretchImage
      AddHandler parent.KeyDown, AddressOf Me_KeyDown
      AddHandler parent.Resize, AddressOf Me_Resize
    End Sub

    Private Shared Sub Me_KeyDown(sender As Object, e As KeyEventArgs)
      m_keys.Push(e.KeyCode) : e.Handled = True : e.SuppressKeyPress = True
    End Sub

    Private Shared Sub Me_Resize(sender As Object, e As EventArgs)

      ' Resizes the picture box preserving the aspect ratio of 4x3.

      Dim clientW = m_parent.ClientSize.Width
      Dim clientH = m_parent.ClientSize.Height

      If clientH > clientW * 0.75 Then
        m_pictureBox.Width = clientW
        m_pictureBox.Height = CInt(Fix(clientW * 0.75))
        m_pictureBox.Left = 0
        m_pictureBox.Top = (clientH - m_pictureBox.Height) \ 2
      Else
        m_pictureBox.Height = clientH
        m_pictureBox.Width = CInt(Fix(clientH * 1.333))
        m_pictureBox.Top = 0
        m_pictureBox.Left = (clientW - m_pictureBox.Width) \ 2
      End If

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
      If noCr Then
        m_cursorCol += If(text?.Length, 0)
      Else
        m_cursorCol = 1
        m_cursorRow += 1
      End If
      Refresh()
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

    Public Shared Sub PSET(x%, y%, color%)
      CType(m_display, Bitmap).SetPixel(x, y, m_palette(color))
      Refresh()
    End Sub

    Public Shared Sub CIRCLE(x#, y#, color#)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub CIRCLE(x%, y%, radius#, attribute%)
      Dim start = 0
      Dim [end] = Math.PI * 2
      CIRCLE(x, y, radius, attribute, start, [end])
    End Sub

    Public Shared Sub CIRCLE(x%, y%, radius#, attribute%, start#, end#)
      Dim aspect = 4.0 * (350.0 / 640.0) / 3.0
      CIRCLE(x, y, radius, attribute, start, [end], aspect)
    End Sub

    Public Shared Sub CIRCLE(ByVal xcenter As Integer, ByVal ycenter As Integer, ByVal radius As Double, ByVal attribute As Integer, ByVal start As Double, ByVal [end] As Double, ByVal aspect As Double)

      'Dim start As Double
      'Dim [end] As Double

      'If startIn Is Nothing Then
      '  start = 0
      'End If

      'If endIn Is Nothing Then
      '  [end] = 0
      'End If

      Select Case 9 'Me.m_screenMode
        'Case 0
        '????
        Case 1
          If Not attribute.Between(0, 3) Then attribute = 3
        Case 2
          If Not attribute.Between(0, 1) Then attribute = 1
        Case 7
          If Not attribute.Between(0, 15) Then attribute = 15
        Case 8
          If Not attribute.Between(0, 15) Then attribute = 15
        Case 9
          If Not attribute.Between(0, 15) Then attribute = 15
        Case 10
          If Not attribute.Between(0, 3) Then attribute = 3
        Case Else
          Stop
      End Select

      Static pi As Double = 3.1415926535897931
      Static pi2 As Double = 6.2831853071795862
      Static line_to_start, line_from_end As Integer
      Static ix, iy As Integer ' integer screen co-ordinates of circle's centre
      Static xspan, yspan As Double
      Static c As Double ' circumference
      Static px, py As Double
      Static sinb, cosb As Double ' second angle used in double-angle-formula
      Static pixels As Integer
      Static tmp As Double
      Static tmpi As Integer
      Static i As Integer
      Static exclusive As Integer
      Static arc1, arc2, arc3, arc4, arcinc As Double
      Static px2 As Double ', py2 As Double
      Static x2, y2 As Integer
      Static lastplotted_x2, lastplotted_y2 As Integer
      Static lastchecked_x2, lastchecked_y2 As Integer

      'If m_writePage.Text Then Throw New InvalidOperationException

      ' lines to & from centre
      'If (Not ((passed And 4) = 4)) Then start = 0
      'If (Not ((passed And 8) = 8)) Then [end] = pi2
      line_to_start = 0 : If (start < 0) Then line_to_start = 1 : start = -start
      line_from_end = 0 : If ([end] < 0) Then line_from_end = 1 : [end] = -[end]

      ' error checking
      If (start > pi2) Then Throw New InvalidOperationException
      If ([end] > pi2) Then Throw New InvalidOperationException

      ' when end<start, the arc of the circle that wouldn't have been drawn if start & end 
      ' were swapped is drawn
      exclusive = 0
      If [end] < start Then
        tmp = start : start = [end] : [end] = tmp
        tmpi = line_to_start : line_to_start = line_from_end : line_from_end = tmpi
        exclusive = 1
      End If

      ' calc. centre
      'If (passed And 1) = 1 Then x = m_writePage.X + x : y = m_writePage.Y + y
      'm_writePage.X = x : m_writePage.Y = y ' set graphics cursor position to circle's centre

      Dim r As Double = radius
      Dim x As Integer = xcenter
      Dim y As Integer = ycenter

      r = x + r ' the differece between x & x+r in pixels will be the radius in pixels
      ' resolve coordinates (but keep as floats)
      'If m_writePage.ClippingOrScaling <> 0 Then
      '  If m_writePage.ClippingOrScaling = 2 Then
      '    x = x * m_writePage.ScalingX + m_writePage.ScalingOffsetX + m_writePage.ViewOffsetX
      '    y = y * m_writePage.ScalingY + m_writePage.ScalingOffsetY + m_writePage.ViewOffsetY
      '    r = r * m_writePage.ScalingX + m_writePage.ScalingOffsetX + m_writePage.ViewOffsetX
      '  Else
      '    x = x + m_writePage.ViewOffsetX
      '    y = y + m_writePage.ViewOffsetY
      '    r = r + m_writePage.ViewOffsetX
      '  End If
      'End If
      'If x < 0 Then ix = CInt(x - 0.05) Else ix = CInt(x + 0.5)
      'If y < 0 Then iy = CInt(y - 0.05) Else iy = CInt(y + 0.05)
      If x < 0 Then ix = CInt(x) Else ix = CInt(x)
      If y < 0 Then iy = CInt(y) Else iy = CInt(y)
      r = Math.Abs(r - x) ' r is now a radius in pixels

      ' adjust vertical and horizontal span of the circle based on aspect ratio
      xspan = r : yspan = r
      'If Not ((passed And 16) = 16) Then
      '  aspect = 1 ' Note: default aspect ratio is 1:1 for QB64 specific modes (256/32)
      '  If (m_writePage.CompatibleMode = 1) Then aspect = 4.0 * (200.0 / 320.0) / 3.0
      '  If (m_writePage.CompatibleMode = 2) Then aspect = 4.0 * (200.0 / 640.0) / 3.0
      '  If (m_writePage.CompatibleMode = 7) Then aspect = 4.0 * (200.0 / 320.0) / 3.0
      '  If (m_writePage.CompatibleMode = 8) Then aspect = 4.0 * (200.0 / 640.0) / 3.0
      '  If (m_writePage.CompatibleMode = 9) Then aspect = 4.0 * (350.0 / 640.0) / 3.0
      '  If (m_writePage.CompatibleMode = 10) Then aspect = 4.0 * (350.0 / 640.0) / 3.0
      '  If (m_writePage.CompatibleMode = 11) Then aspect = 4.0 * (480.0 / 640.0) / 3.0
      '  If (m_writePage.CompatibleMode = 12) Then aspect = 4.0 * (480.0 / 640.0) / 3.0
      '  If (m_writePage.CompatibleMode = 13) Then aspect = 4.0 * (200.0 / 320.0) / 3.0
      '  ' Old method: aspect = 4.0 * (m_writePage.Height / m_writePage.width) / 3.0
      'End If
      If aspect >= 0 Then
        If aspect < 1 Then
          ' aspect: 0 to 1
          yspan *= aspect
        End If
        If aspect > 1 Then
          ' aspect: 1 to infinity
          xspan /= aspect
        End If
      Else
        If (aspect > -1) Then
          ' aspect: -1 to 0
          yspan *= (1 + aspect)
        End If
        ' if aspect<-1 no change is required
      End If

      ' skip everything if none of the circle is inside current viwport
      'If ((x + xspan + 0.5) < m_writePage.ViewX1) Then Return
      'If ((y + yspan + 0.5) < m_writePage.ViewY1) Then Return
      'If ((x - xspan - 0.5) > m_writePage.ViewX2) Then Return
      'If ((y - yspan - 0.5) > m_writePage.ViewY2) Then Return

      'If Not ((passed And 2) = 2) Then col = m_writePage.Color
      'm_writePage.DrawColor = col

      ' pre-set/pre-calcualate values
      c = pi2 * r
      pixels = CInt(c / 4.0) ' + 0.5)
      arc1 = 0
      arc2 = pi
      arc3 = pi
      arc4 = pi2
      arcinc = (pi / 2) / CDbl(pixels)
      sinb = Math.Sin(arcinc)
      cosb = Math.Cos(arcinc)
      lastplotted_x2 = -1
      lastchecked_x2 = -1
      i = 0

      If CBool(line_to_start) Then
        px = Math.Cos(start) : py = Math.Sin(start)
        x2 = CInt(px * xspan + 0.5) : y2 = CInt(py * yspan - 0.5)
        'FastLine(ix, iy, ix + x2, iy - y2, col)
        LINE(ix, iy, ix + x2, iy - y2, attribute)
      End If

      px = 1
      py = 0

drawcircle:
      x2 = CInt(px * xspan) ' + 0.5)
      y2 = CInt(py * yspan) ' - 0.5)

      If (i = 0) Then lastchecked_x2 = x2 : lastchecked_y2 = y2 : GoTo plot

      If ((Math.Abs(x2 - lastplotted_x2) >= 2) OrElse (Math.Abs(y2 - lastplotted_y2) >= 2)) Then
plot:
        If CBool(exclusive) Then
          If ((arc1 <= start) OrElse (arc1 >= [end])) Then PSET(ix + lastchecked_x2, iy + lastchecked_y2, attribute)
          If ((arc2 <= start) OrElse (arc2 >= [end])) Then PSET(ix - lastchecked_x2, iy + lastchecked_y2, attribute)
          If ((arc3 <= start) OrElse (arc3 >= [end])) Then PSET(ix - lastchecked_x2, iy - lastchecked_y2, attribute)
          If ((arc4 <= start) OrElse (arc4 >= [end])) Then PSET(ix + lastchecked_x2, iy - lastchecked_y2, attribute)
        Else ' inclusive
          If ((arc1 >= start) AndAlso (arc1 <= [end])) Then PSET(ix + lastchecked_x2, iy + lastchecked_y2, attribute)
          If ((arc2 >= start) AndAlso (arc2 <= [end])) Then PSET(ix - lastchecked_x2, iy + lastchecked_y2, attribute)
          If ((arc3 >= start) AndAlso (arc3 <= [end])) Then PSET(ix - lastchecked_x2, iy - lastchecked_y2, attribute)
          If ((arc4 >= start) AndAlso (arc4 <= [end])) Then PSET(ix + lastchecked_x2, iy - lastchecked_y2, attribute)
        End If
        If (i > pixels) Then GoTo allplotted
        lastplotted_x2 = lastchecked_x2 : lastplotted_y2 = lastchecked_y2
      End If
      lastchecked_x2 = x2 : lastchecked_y2 = y2

      If (i <= pixels) Then
        i += 1
        If (i > pixels) Then GoTo plot
        px2 = px * cosb + py * sinb
        py = py * cosb - px * sinb
        px = px2
        If CBool(i) Then arc1 += arcinc : arc2 -= arcinc : arc3 += arcinc : arc4 -= arcinc
        GoTo drawcircle
      End If

allplotted:

      If CBool(line_from_end) Then
        px = Math.Cos([end]) : py = Math.Sin([end])
        x2 = CInt(px * xspan + 0.5) : y2 = CInt(py * yspan - 0.5)
        'FastLine(ix, iy, ix + x2, iy - y2, col)
        LINE(ix, iy, ix + x2, iy - y2, attribute)
      End If

      Refresh()

    End Sub

    Public Shared Sub PAINT(x%, y%)
      PAINT(False, x, y, m_fgColor, m_fgColor, Nothing)
    End Sub

    Public Shared Sub PAINT(x%, y%, color%)
      PAINT(False, x, y, color, color, Nothing)
    End Sub

    Public Shared Sub PAINT(x%, y%, color%, border%)
      PAINT(False, x, y, color, border, Nothing)
    End Sub

    Public Shared Sub PAINT(x%, y%, color%, border%, background$)
      PAINT(False, x, y, color, border, background)
    End Sub

    Public Shared Sub PAINT([step] As Boolean, x%, y%)
      PAINT([step], x, y, m_fgColor, m_fgColor, Nothing)
    End Sub

    Public Shared Sub PAINT([step] As Boolean, x%, y%, color%)
      PAINT([step], x, y, color, color, Nothing)
    End Sub

    Public Shared Sub PAINT([step] As Boolean, x%, y%, color%, border%)
      PAINT([step], x, y, color, border, Nothing)
    End Sub

    Public Shared Sub PAINT([step] As Boolean, x%, y%, color%, border%, background$)

      Dim p = New Drawing.Point(x, y)
      Dim stk As New Stack()
      stk.Push(p)
      Dim b = CType(m_display, Bitmap)
      Dim replacementColor = b.GetPixel(x, y)
      Do While stk.Count <> 0
        p = CType(stk.Pop(), Point)
        Dim testColor = b.GetPixel(p.X, p.Y)
        If SameColor(testColor, replacementColor) AndAlso Not SameColor(testColor, m_palette(color)) Then
          CType(m_display, Bitmap).SetPixel(p.X, p.Y, m_palette(color))
          If p.X - 1 > -1 Then stk.Push(New Point(p.X - 1, p.Y))
          If p.X + 1 < m_display.Width Then stk.Push(New Point(p.X + 1, p.Y))
          If p.Y - 1 > -1 Then stk.Push(New Point(p.X, p.Y - 1))
          If p.Y + 1 < m_display.Height Then stk.Push(New Point(p.X, p.Y + 1))
        End If
      Loop

      Refresh()

    End Sub

    Private Shared Function SameColor(c1 As Color, c2 As Color) As Boolean
      Return ((c1.A = c2.A) AndAlso (c1.B = c2.B) AndAlso (c1.G = c2.G) AndAlso (c1.R = c2.R))
    End Function

    Public Shared Sub LINE(x1!, y1!, x2!, y2!, attr%, Optional lo As LineOption = LineOption.None)

      If x2 < x1 Then SWAP(x1, x2)
      If y2 < y1 Then SWAP(y1, y2)

      Select Case lo
        Case LineOption.None
          Using g = Graphics.FromImage(m_display)
            Using p = New Pen(m_palette(attr))
              g.DrawLine(p, x1, y1, x2, y2)
            End Using
          End Using
        Case LineOption.B
          Using g = Graphics.FromImage(m_display)
            Using p = New Pen(m_palette(attr))
              g.DrawRectangle(p, x1, y1, x2 - x1, y2 - y1)
            End Using
          End Using
        Case LineOption.BF
          Using g = Graphics.FromImage(m_display)
            Using b = New SolidBrush(m_palette(attr))
              g.FillRectangle(b, x1, y1, x2 - x1, y2 - y1)
            End Using
          End Using
        Case Else
      End Select
      Refresh()
    End Sub

    Private Shared Sub Refresh()
      If m_pictureBox IsNot Nothing Then
        m_pictureBox.Image = m_display
      End If
    End Sub

    Public Shared Sub PUT(x%, y%, img As Image, po As PutOption)
      Using g = Graphics.FromImage(m_display)
        Dim src = New Rectangle(0, 0, img.Width, img.Height)
        Dim dest = New Rectangle(x, y, img.Width, img.Height)
        g.DrawImage(img, dest, src, GraphicsUnit.Pixel)
      End Using
      Refresh()
    End Sub

    Private Shared m_address As New List(Of Image)

    Public Shared Sub [GET](x1%, y1%, x2%, y2%, ByRef img As Image)
      img = New Bitmap(x2 - x1, y2 - y1)
      Using g = Graphics.FromImage(img)
        Dim src = New Rectangle(x1, y1, x2 - x1, y2 - y1)
        Dim dest = New Rectangle(0, 0, x2 - x1, y2 - y1)
        g.DrawImage(m_display, dest, src, GraphicsUnit.Pixel)
      End Using
    End Sub

    Public Shared Async Function LineInputAsync(prompt$) As Task(Of String)
      PRINT(prompt$)
      Dim result$ = ""
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
      Refresh()
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
      Dim result$ = ""
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
      If display < 0 OrElse display > 63 Then
        Throw New ArgumentOutOfRangeException(NameOf(display))
      End If
      Dim clr = System.Drawing.Color.FromArgb(255, 0, 0, 0)
      Select Case display
        Case 0 : clr = System.Drawing.Color.FromArgb(255, 0, 0, 0)
        Case 1 : clr = System.Drawing.Color.FromArgb(255, &H0, &H0, &HAA)
        Case 2 : clr = System.Drawing.Color.FromArgb(255, &H0, &HAA, &H0)
        Case 3 : clr = System.Drawing.Color.FromArgb(255, &H0, &HAA, &HAA)
        Case 4 : clr = System.Drawing.Color.FromArgb(255, &HAA, &H0, &H0)
        Case 5 : clr = System.Drawing.Color.FromArgb(255, &HAA, &H0, &HAA)
        Case 6 : clr = System.Drawing.Color.FromArgb(255, &HAA, &HAA, &H0)
        Case 7 : clr = System.Drawing.Color.FromArgb(255, &HAA, &HAA, &HAA)
        Case 8 : clr = System.Drawing.Color.FromArgb(255, &H0, &H0, &H55)
        Case 9 : clr = System.Drawing.Color.FromArgb(255, &H0, &H0, &HFF)
        Case 10 : clr = System.Drawing.Color.FromArgb(255, &H0, &HAA, &H55)
        Case 11 : clr = System.Drawing.Color.FromArgb(255, &H0, &HAA, &H55)
        Case 12 : clr = System.Drawing.Color.FromArgb(255, &HAA, &H0, &H55)
        Case 13 : clr = System.Drawing.Color.FromArgb(255, &HAA, &H0, &HFF)
        Case 14 : clr = System.Drawing.Color.FromArgb(255, &HAA, &HAA, &H55)
        Case 15 : clr = System.Drawing.Color.FromArgb(255, &HAA, &HAA, &HFF)
        Case 16 : clr = System.Drawing.Color.FromArgb(255, &H0, &H55, &H0)
        Case 17 : clr = System.Drawing.Color.FromArgb(255, &H0, &H55, &HAA)
        Case 18 : clr = System.Drawing.Color.FromArgb(255, &H0, &HFF, &H0)
        Case 19 : clr = System.Drawing.Color.FromArgb(255, &H0, &HFF, &HAA)
        Case 20 : clr = System.Drawing.Color.FromArgb(255, &HAA, &H55, &H0)
        Case 21 : clr = System.Drawing.Color.FromArgb(255, &HAA, &H55, &HAA)
        Case 22 : clr = System.Drawing.Color.FromArgb(255, &HAA, &HFF, &H0)
        Case 23 : clr = System.Drawing.Color.FromArgb(255, &HAA, &HFF, &HAA)
        Case 24 : clr = System.Drawing.Color.FromArgb(255, &H0, &H55, &H55)
        Case 25 : clr = System.Drawing.Color.FromArgb(255, &H0, &H55, &HFF)
        Case 26 : clr = System.Drawing.Color.FromArgb(255, &H0, &HFF, &H55)
        Case 27 : clr = System.Drawing.Color.FromArgb(255, &H0, &HFF, &HFF)
        Case 28 : clr = System.Drawing.Color.FromArgb(255, &HAA, &H55, &H55)
        Case 29 : clr = System.Drawing.Color.FromArgb(255, &HAA, &H55, &HFF)
        Case 30 : clr = System.Drawing.Color.FromArgb(255, &HAA, &HFF, &H55)
        Case 31 : clr = System.Drawing.Color.FromArgb(255, &HAA, &HFF, &HFF)
        Case 32 : clr = System.Drawing.Color.FromArgb(255, &H55, &H0, &H0)
        Case 33 : clr = System.Drawing.Color.FromArgb(255, &H55, &H0, &HAA)
        Case 34 : clr = System.Drawing.Color.FromArgb(255, &H55, &HAA, &H0)
        Case 35 : clr = System.Drawing.Color.FromArgb(255, &H55, &HAA, &HAA)
        Case 36 : clr = System.Drawing.Color.FromArgb(255, &HFF, &H0, &H0)
        Case 37 : clr = System.Drawing.Color.FromArgb(255, &HFF, &H0, &HAA)
        Case 38 : clr = System.Drawing.Color.FromArgb(255, &HFF, &HAA, &H0)
        Case 39 : clr = System.Drawing.Color.FromArgb(255, &HFF, &HAA, &HAA)
        Case 40 : clr = System.Drawing.Color.FromArgb(255, &H55, &H0, &H55)
        Case 41 : clr = System.Drawing.Color.FromArgb(255, &H55, &H0, &HFF)
        Case 42 : clr = System.Drawing.Color.FromArgb(255, &H55, &HAA, &H55)
        Case 23 : clr = System.Drawing.Color.FromArgb(255, &H55, &HAA, &HFF)
        Case 44 : clr = System.Drawing.Color.FromArgb(255, &HFF, &H0, &H55)
        Case 45 : clr = System.Drawing.Color.FromArgb(255, &HFF, &H0, &HFF)
        Case 46 : clr = System.Drawing.Color.FromArgb(255, &HFF, &HAA, &H55)
        Case 47 : clr = System.Drawing.Color.FromArgb(255, &HFF, &HAA, &HFF)
        Case 48 : clr = System.Drawing.Color.FromArgb(255, &H55, &H55, &H0)
        Case 49 : clr = System.Drawing.Color.FromArgb(255, &H55, &H55, &HAA)
        Case 50 : clr = System.Drawing.Color.FromArgb(255, &H55, &HFF, &H0)
        Case 51 : clr = System.Drawing.Color.FromArgb(255, &H55, &HFF, &HAA)
        Case 52 : clr = System.Drawing.Color.FromArgb(255, &HFF, &H55, &H0)
        Case 53 : clr = System.Drawing.Color.FromArgb(255, &HFF, &H55, &HAA)
        Case 54 : clr = System.Drawing.Color.FromArgb(255, &HFF, &HFF, &H0)
        Case 55 : clr = System.Drawing.Color.FromArgb(255, &HFF, &HFF, &HAA)
        Case 56 : clr = System.Drawing.Color.FromArgb(255, &H55, &H55, &H55)
        Case 57 : clr = System.Drawing.Color.FromArgb(255, &H55, &H55, &HFF)
        Case 58 : clr = System.Drawing.Color.FromArgb(255, &H55, &HFF, &H55)
        Case 59 : clr = System.Drawing.Color.FromArgb(255, &H55, &HFF, &HFF)
        Case 60 : clr = System.Drawing.Color.FromArgb(255, &HFF, &H55, &H55)
        Case 61 : clr = System.Drawing.Color.FromArgb(255, &HFF, &H55, &HFF)
        Case 62 : clr = System.Drawing.Color.FromArgb(255, &HFF, &HFF, &H55)
        Case 63 : clr = System.Drawing.Color.FromArgb(255, &HFF, &HFF, &HFF)
        Case Else
      End Select
      m_palette(color) = clr
    End Sub

    Public Shared Sub QbWidth(Optional a% = -1, Optional b% = -1)
      If a <> 80 OrElse b <> 25 Then
        Throw New NotImplementedException
      End If
    End Sub

    Private Shared m_keys As New Stack(Of Keys)

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

    Public Shared Async Function SleepAsync(seconds As Integer) As Task
      Dim till = DateAdd(DateInterval.Second, seconds, Now)
      Do
        If Not String.IsNullOrEmpty(INKEY) OrElse
           Now >= till Then
          Exit Do
        End If
        'Threading.Thread.Sleep(100)
        Await Task.Delay(100)
      Loop
    End Function

#End Region

    Public Shared Function POINT%(x%, y%)
      Dim b = CType(m_display, Bitmap)
      Dim c = b.GetPixel(x, y)
      For entry = 0 To m_palette.Length - 1
        If c = m_palette(entry) Then
          Return entry
        End If
      Next
      Return 0
    End Function

    Public Shared Function TAB(column%) As String
      If column% <= 32767 Then
        If column% < 0 Then
          column% = 1
        Else
          column% = column% Mod 80
        End If
        Dim current% = m_cursorCol
        Dim count% = column% - current%
        Dim resuLtKey$ = ""
        If count% < 0 Then
          resuLtKey$ = vbCrLf : count% = column%
        End If
        resuLtKey$ &= Space(count%)
        Return resuLtKey$
      Else
        Throw New ArgumentException
      End If
    End Function

  End Class

End Namespace