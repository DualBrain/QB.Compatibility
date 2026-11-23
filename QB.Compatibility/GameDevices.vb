Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ' Joysticks (STICK, STRIG) and the light pen (PEN, played by the mouse).
  Partial Public NotInheritable Class Devices

    Private Shared ReadOnly m_stickSnapshot(3) As Integer
    Private Shared ReadOnly m_lastPresses(1, 1) As Integer
    Private Shared m_lastPenPresses As Integer

    ' -32768..32767 to QBasic's 0..200 (centered at 100).
    Private Shared Function Scale(raw As Integer) As Integer
      Return CInt((CLng(raw) + 32768L) * 200L \ 65535L)
    End Function

    ''' <summary>
    ''' The position of a joystick, 0 to 200 (100 is the center): 0 = x of joystick A, 1 = y of A, 2 = x of B,
    ''' 3 = y of B.  As in QBasic, STICK(0) reads all four, and the others return what STICK(0) read.
    ''' Joysticks are the first two that SDL finds; without one the value is 0.
    ''' </summary>
    Friend Shared Function STICK(n As Integer) As Integer
      If n < 0 OrElse n > 3 Then Throw New ArgumentException("Illegal function call")
      If n = 0 Then
        Dim input = Video.GameInput()
        For i = 0 To 3
          m_stickSnapshot(i) = If(input Is Nothing, 0, Scale(input.StickAxis(i \ 2, i Mod 2)))
        Next
      End If
      Return m_stickSnapshot(n)
    End Function

    ''' <summary>
    ''' A joystick button, as -1 (true) or 0: 0 = lower trigger of A pressed since the last STRIG(0), 1 = pressed now,
    ''' 2 and 3 the same for the lower trigger of B, 4 and 5 for the upper trigger of A, 6 and 7 for the upper trigger of B.
    ''' </summary>
    Friend Shared Function STRIG(n As Integer) As Integer
      If n < 0 OrElse n > 7 Then Throw New ArgumentException("Illegal function call")
      Dim input = Video.GameInput()
      If input Is Nothing Then Return 0
      Dim stick = (n \ 2) And 1
      Dim button = n \ 4
      If (n And 1) = 1 Then Return If(input.StickButtonDown(stick, button), -1, 0)
      Dim presses = input.StickButtonPresses(stick, button)
      Dim pressed = presses <> m_lastPresses(stick, button)
      m_lastPresses(stick, button) = presses
      Return If(pressed, -1, 0)
    End Function

    ''' <summary>
    ''' The light pen (the mouse, with its left button as the pen switch): 0 = used since the last call (-1 or 0),
    ''' 1 and 2 = x and y of the last press, 3 = switch down (-1) or up (0), 4 and 5 = x and y it was last at,
    ''' 6 and 7 = character row and column of the last press, 8 and 9 = row and column it was last at.
    ''' Coordinates are screen pixels of the current mode.
    ''' </summary>
    Friend Shared Function PEN(n As Integer) As Integer
      If n < 0 OrElse n > 9 Then Throw New ArgumentException("Illegal function call")
      Dim input = Video.GameInput()
      If input Is Nothing Then Return 0
      Dim p = input.Pointer()
      Dim row, col As Integer
      Select Case n
        Case 0
          Dim used = p.Presses <> m_lastPenPresses
          m_lastPenPresses = p.Presses
          Return If(used, -1, 0)
        Case 1 : Return p.PressX
        Case 2 : Return p.PressY
        Case 3 : Return If(p.Down, -1, 0)
        Case 4 : Return p.X
        Case 5 : Return p.Y
        Case 6, 7
          Video.PixelToCell(p.PressX, p.PressY, row, col)
          Return If(n = 6, row, col)
        Case Else
          Video.PixelToCell(p.X, p.Y, row, col)
          Return If(n = 8, row, col)
      End Select
    End Function

  End Class

End Namespace
