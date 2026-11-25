Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>
  ''' The graphics statements (SCREEN 1 and up, PSET, LINE, CIRCLE, PAINT, GET, PUT, DRAW, PALETTE, VIEW, WINDOW...) and the joystick and
  ''' pen functions, together with everything in <see cref="Video"/>: <c>Imports QB.Graphics</c> instead of <c>Imports QB.Video</c>.
  ''' Using any of them makes the SDL2 window the display (this is what needs the QB.Compatibility.Sdl assembly and SDL2).
  ''' </summary>
  Public NotInheritable Class Graphics
    Inherits Video

    Private Sub New()
    End Sub

    Shared Sub New()
      Video.GraphicsDisplayFactory = Function() New SdlDisplay
      Multimedia.AudioFactory = Function() New SdlAudio
    End Sub

    ''' <summary>
    ''' The same as <see cref="Video.WaitForKeyAtExit"/>; setting it first thing in a program (as the converter does) makes the window available
    ''' from the start, for programs that print before they draw and have no console.
    ''' </summary>
    Public Shared Shadows Property WaitForKeyAtExit As Boolean
      Get
        Return Video.WaitForKeyAtExit
      End Get
      Set(value As Boolean)
        Video.WaitForKeyAtExit = value
      End Set
    End Property

    ''' <summary>The same as <see cref="Video.WindowTitle"/>: the title of the window (the name of the program until it is set).</summary>
    Public Shared Shadows Property WindowTitle As String
      Get
        Return Video.WindowTitle
      End Get
      Set(value As String)
        Video.WindowTitle = value
      End Set
    End Property

    ' The joystick and the pen come from the window.
    Private Shared Sub EnsureWindow()
      If Not (TypeOf Video.Display Is IGraphicsDisplay) Then Video.Display = New SdlDisplay
    End Sub

  Public Shared Shadows Sub VIEW()
    Video.VIEW()
    End Sub
  Public Shared Shadows Sub VIEW(x1%, y1%, x2%, y2%, Optional color% = -1, Optional border% = -1)
    Video.VIEW(x1, y1, x2, y2, color, border)
    End Sub
  Public Shared Shadows Sub VIEW_SCREEN(x1%, y1%, x2%, y2%, Optional color% = -1, Optional border% = -1)
    Video.VIEW_SCREEN(x1, y1, x2, y2, color, border)
    End Sub
  Public Shared Shadows Sub WINDOW()
    Video.WINDOW()
    End Sub
  Public Shared Shadows Sub WINDOW(x1#, y1#, x2#, y2#)
    Video.WINDOW(x1, y1, x2, y2)
    End Sub
  Public Shared Shadows Sub WINDOW_SCREEN(x1#, y1#, x2#, y2#)
    Video.WINDOW_SCREEN(x1, y1, x2, y2)
    End Sub
  Public Shared Shadows Function PMAP(expression#, func%) As Double
    Return Video.PMAP(expression, func)
    End Function
  Public Shared Shadows Sub PSET(x#, y#)
    Video.PSET(x, y)
    End Sub
  Public Shared Shadows Sub PSET(x#, y#, color%)
    Video.PSET(x, y, color)
    End Sub
  Public Shared Shadows Sub PSET([step] As Boolean, x#, y#, Optional color% = -1)
    Video.PSET([step], x, y, color)
    End Sub
  Public Shared Shadows Sub PRESET(x#, y#)
    Video.PRESET(x, y)
    End Sub
  Public Shared Shadows Sub PRESET(x#, y#, color%)
    Video.PRESET(x, y, color)
    End Sub
  Public Shared Shadows Sub PRESET([step] As Boolean, x#, y#, Optional color% = -1)
    Video.PRESET([step], x, y, color)
    End Sub
  Public Shared Shadows Sub CIRCLE(x#, y#, radius#, Optional attribute% = -1, Optional start As Double? = Nothing, Optional [end] As Double? = Nothing, Optional aspect As Double? = Nothing)
    Video.CIRCLE(x, y, radius, attribute, start, [end], aspect)
    End Sub
  Public Shared Shadows Sub CIRCLE([step] As Boolean, x#, y#, radius#, Optional attribute% = -1, Optional start As Double? = Nothing, Optional [end] As Double? = Nothing, Optional aspect As Double? = Nothing)
    Video.CIRCLE([step], x, y, radius, attribute, start, [end], aspect)
    End Sub
  Public Shared Shadows Sub PAINT(x#, y#)
    Video.PAINT(x, y)
    End Sub
  Public Shared Shadows Sub PAINT(x#, y#, color%)
    Video.PAINT(x, y, color)
    End Sub
  Public Shared Shadows Sub PAINT(x#, y#, color%, border%)
    Video.PAINT(x, y, color, border)
    End Sub
  Public Shared Shadows Sub PAINT(x#, y#, color%, border%, background$)
    Video.PAINT(x, y, color, border, background)
    End Sub
  Public Shared Shadows Sub PAINT([step] As Boolean, x#, y#)
    Video.PAINT([step], x, y)
    End Sub
  Public Shared Shadows Sub PAINT([step] As Boolean, x#, y#, color%)
    Video.PAINT([step], x, y, color)
    End Sub
  Public Shared Shadows Sub PAINT([step] As Boolean, x#, y#, color%, border%)
    Video.PAINT([step], x, y, color, border)
    End Sub
  Public Shared Shadows Sub PAINT(x#, y#, tile$)
    Video.PAINT(x, y, tile)
    End Sub
  Public Shared Shadows Sub PAINT(x#, y#, tile$, border%)
    Video.PAINT(x, y, tile, border)
    End Sub
  Public Shared Shadows Sub PAINT(x#, y#, tile$, border%, background$)
    Video.PAINT(x, y, tile, border, background)
    End Sub
  Public Shared Shadows Sub PAINT([step] As Boolean, x#, y#, tile$)
    Video.PAINT([step], x, y, tile)
    End Sub
  Public Shared Shadows Sub PAINT([step] As Boolean, x#, y#, tile$, border%)
    Video.PAINT([step], x, y, tile, border)
    End Sub
  Public Shared Shadows Sub PAINT([step] As Boolean, x#, y#, tile$, border%, background$)
    Video.PAINT([step], x, y, tile, border, background)
    End Sub
  Public Shared Shadows Sub PAINT([step] As Boolean, x#, y#, color%, border%, background$)
    Video.PAINT([step], x, y, color, border, background)
    End Sub
  Public Shared Shadows Sub DRAW(commands As String)
    Video.DRAW(commands)
    End Sub
  Public Shared Shadows Sub LINE(x2#, y2#)
    Video.LINE(x2, y2)
    End Sub
  Public Shared Shadows Sub LINE([step2] As Boolean, x2#, y2#, Optional attr% = -1, Optional lo As LineOption = LineOption.None, Optional style% = &HFFFF)
    Video.LINE([step2], x2, y2, attr, lo, style)
    End Sub
  Public Shared Shadows Sub LINE(x1#, y1#, x2#, y2#, Optional attr% = -1, Optional lo As LineOption = LineOption.None, Optional style% = &HFFFF)
    Video.LINE(x1, y1, x2, y2, attr, lo, style)
    End Sub
  Public Shared Shadows Sub LINE([step1] As Boolean, x1#, y1#, [step2] As Boolean, x2#, y2#, Optional attr% = -1, Optional lo As LineOption = LineOption.None, Optional style% = &HFFFF)
    Video.LINE([step1], x1, y1, [step2], x2, y2, attr, lo, style)
    End Sub
  Public Shared Shadows Sub PUT(x#, y#, image As Array, Optional action As PutOption = PutOption.PUT_XOR, Optional index% = 0)
    Video.PUT(x, y, image, action, index)
    End Sub
  Public Shared Shadows Sub PUT(x As Integer, y As Integer, image As Array)
    Video.PUT(x, y, image)
    End Sub
  Public Shared Shadows Sub PUT(x As Integer, y As Integer, image As Array, action As PutOption)
    Video.PUT(x, y, image, action)
    End Sub
  Public Shared Shadows Sub PUT(x As Integer, y As Integer, image As Array, action As PutOption, index As Integer)
    Video.PUT(x, y, image, action, index)
    End Sub
  Public Shared Shadows Sub PUT([step] As Boolean, x#, y#, image As Array, Optional action As PutOption = PutOption.PUT_XOR, Optional index% = 0)
    Video.PUT([step], x, y, image, action, index)
    End Sub
  Public Shared Shadows Sub [GET](x1#, y1#, x2#, y2#, image As Array, Optional index% = 0)
    Video.[GET](x1, y1, x2, y2, image, index)
    End Sub
  Public Shared Shadows Sub [GET]([step1] As Boolean, x1#, y1#, [step2] As Boolean, x2#, y2#, image As Array, Optional index% = 0)
    Video.[GET]([step1], x1, y1, [step2], x2, y2, image, index)
    End Sub
  Public Shared Shadows Function SCREEN%(row%, column%, Optional colr% = 0)
    Return Video.SCREEN(row, column, colr)
    End Function
  Public Shared Shadows Sub SCREEN(mode%, colorswitch%, apage%, vpage%)
    Video.SCREEN(mode, colorswitch, apage, vpage)
    End Sub
  Public Shared Shadows Sub PCOPY(source%, destination%)
    Video.PCOPY(source, destination)
    End Sub
  Public Shared Shadows Sub SCREEN(mode%)
    Video.SCREEN(mode)
    End Sub
  Public Shared Shadows Sub PALETTE()
    Video.PALETTE()
    End Sub
  Public Shared Shadows Sub PALETTE(color%, display%)
    Video.PALETTE(color, display)
    End Sub
  Public Shared Shadows Sub PALETTE_USING(values As Array, Optional index% = 0)
    Video.PALETTE_USING(values, index)
    End Sub
  Public Shared Shadows Function POINT%(x#, y#)
    Return Video.POINT(x, y)
    End Function
  Public Shared Shadows Function POINT(func%) As Double
    Return Video.POINT(func)
    End Function
  Public Shared Shadows Sub [GET](Of T)(filenum%, position As Long, ByRef value As T)
    Video.[GET](filenum, position, value)
    End Sub
  Public Shared Shadows Sub [GET](Of T)(filenum%, ByRef value As T)
    Video.[GET](filenum, value)
    End Sub
  Public Shared Shadows Sub PUT(filenum%, position As Long, value As Object)
    Video.PUT(filenum, position, value)
    End Sub
  Public Shared Shadows Sub PUT(filenum%, value As Object)
    Video.PUT(filenum, value)
    End Sub

  Public Shared Shadows Function STICK(n As Integer) As Integer
    EnsureWindow()
    Return Devices.STICK(n)
    End Function
  Public Shared Shadows Function STRIG(n As Integer) As Integer
    EnsureWindow()
    Return Devices.STRIG(n)
    End Function
  Public Shared Shadows Function PEN(n As Integer) As Integer
    EnsureWindow()
    Return Devices.PEN(n)
    End Function

  End Class

End Namespace
