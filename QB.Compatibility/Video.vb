Option Explicit On
Option Strict On
Option Infer On

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

    Private Sub New()
    End Sub


    Public Shared Sub PRINT()
      Throw New NotImplementedException
    End Sub

    Public Shared Sub PRINT(text$, Optional noCr As Boolean = False)
      'TODO: (Possibly) Need to take into account the usage of TAB
      '      Tab should return a token that can then be used
      '      within the PRINT statement in order to do 
      '      specific formatting.  This is because the TAB
      '      could be called after other formatting (USING) takes
      '      place.  So should determine the total output and then
      '      adjust for TAB(s).
      'If noCr Then
      '  System.Console.Write(text)
      'Else
      '  System.Console.WriteLine(text)
      'End If
      Throw New NotImplementedException
    End Sub

    Public Shared Sub LOCATE(Optional row% = -1, Optional column% = -1, Optional a% = -1, Optional b% = -1, Optional c% = -1)
      If a <> 0 OrElse b <> 0 OrElse c <> 0 Then
      End If
      Throw New NotImplementedException
      If row% = -1 AndAlso column% > 0 Then
        System.Console.CursorLeft = column% - 1
      ElseIf column% = -1 AndAlso row% > 0 Then
        System.Console.CursorTop = row% - 1
      ElseIf row% > 0 AndAlso column% > 0 Then
        System.Console.SetCursorPosition(column% - 1, row% - 1)
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

    Public Shared Sub LINE_INPUT(prompt$, ByRef value$)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub COLOR(fg%)
      If fg% > 15 Then fg% -= 16
      Throw New NotImplementedException
    End Sub

    Public Shared Sub COLOR(fg%, bg%)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub CLS()
      Throw New NotImplementedException
    End Sub

    Public Shared Sub CLS(viewport As Integer)
      Select Case viewport
        Case 0, 1, 2
          CLS() 'TODO: For now, just call the base.
        Case Else
          Throw New ArgumentException
      End Select
    End Sub

    Public Shared Sub INPUT(prompt$, ByRef resuLtKey$)
      Throw New NotImplementedException
    End Sub

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
      If mode = 0 Then
      Else
      End If
      Throw New NotImplementedException
    End Sub

#End Region

    Public Shared Sub VIEW_PRINT(row1%, row2%)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub PALETTE(OBJECTCOLOR%, BackColor%)
      Throw New NotImplementedException
    End Sub

    Public Shared Sub QbWidth(Optional a% = -1, Optional b% = -1)
      If a <> 0 OrElse b <> 0 Then
      End If
      Throw New NotImplementedException
    End Sub

    Public Shared Function INKEY$()
      If System.Console.KeyAvailable Then
        Dim cki = System.Console.ReadKey(True)

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

        Select Case cki.Key
          Case ConsoleKey.Delete
            Return QBChr(0) & QBChr(83)
          Case ConsoleKey.Home
            Return QBChr(0) & QBChr(71)
          Case ConsoleKey.UpArrow
            Return QBChr(0) & QBChr(72)
          Case ConsoleKey.PageUp
            Return QBChr(0) & QBChr(73)
          Case ConsoleKey.End
            Return QBChr(0) & QBChr(79)
          Case ConsoleKey.DownArrow
            Return QBChr(0) & QBChr(80)
          Case ConsoleKey.PageDown
            Return QBChr(0) & QBChr(81)

          Case ConsoleKey.LeftArrow
            Return QBChr(0) & QBChr(75)
          Case ConsoleKey.RightArrow
            Return QBChr(0) & QBChr(77)

          Case ConsoleKey.F1
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(84 + adder), QBChr(59))
          Case ConsoleKey.F2
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(85 + adder), QBChr(60))
          Case ConsoleKey.F3
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(86 + adder), QBChr(61))
          Case ConsoleKey.F4
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(87 + adder), QBChr(62))
          Case ConsoleKey.F5
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(88 + adder), QBChr(63))
          Case ConsoleKey.F6
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(89 + adder), QBChr(64))
          Case ConsoleKey.F7
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(80 + adder), QBChr(65))
          Case ConsoleKey.F8
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(91 + adder), QBChr(66))
          Case ConsoleKey.F9
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(92 + adder), QBChr(67))
          Case ConsoleKey.F10
            Return QBChr(0) & If(shift OrElse ctrl OrElse alt, QBChr(93 + adder), QBChr(68))
          Case ConsoleKey.F11
            If shift Then
              Return QBChr(0) & QBChr(135)
            ElseIf ctrl Then
              Return QBChr(0) & QBChr(137)
            ElseIf alt Then
              Return QBChr(0) & QBChr(139)
            Else
              Return QBChr(0) & QBChr(133)
            End If
          Case ConsoleKey.F12
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
            Return cki.KeyChar
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