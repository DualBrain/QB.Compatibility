Option Explicit On
Option Strict On
Option Infer On

Namespace Global

  Partial Public NotInheritable Class QB

    Private Declare Function GetAsyncKeyState Lib "user32" (vkey As Integer) As Short

    Private Sub New()
    End Sub

    ' Variables and Types

#Region "SWAP"

    Public Shared Sub SWAP(ByRef value1 As Short, ByRef value2 As Short)
      Dim temp = value1
      value1 = value2
      value2 = temp
    End Sub

    Public Shared Sub SWAP(ByRef value1 As String, ByRef value2 As String)
      Dim temp = value1
      value1 = value2
      value2 = temp
    End Sub

    Public Shared Sub SWAP(ByRef value1 As Single, ByRef value2 As Single)
      Dim temp = value1
      value1 = value2
      value2 = temp
    End Sub

    Public Shared Sub SWAP(ByRef value1 As Integer, ByRef value2 As Integer)
      Dim temp = value1
      value1 = value2
      value2 = value1
    End Sub

#End Region


    ' Flow Control

    ' Decisions and Operators

    ' Procedures

    ' Strings

    Public Shared Function QBChr$(value%)

      '┘ └ ┌ ┐ ─ │ ├ ├ ╟ ╙ ╓ ╥ ╨ ┬ ┴ ║ ┤

      'Return ChrW(value%)

      Select Case value%

        Case 0 : Return " "
        Case 1 : Return "☺"
        Case 2 : Return "☻"
        Case 3 : Return "♥"
        Case 4 : Return "♦"
        Case 5 : Return "♣"
        Case 6 : Return "♠"
        Case 7 : Return "●"
        Case 8 : Return "◘"
        Case 9 : Return "○"
        Case 10 : Return "◙"
        Case 11 : Return "♂"
        Case 12 : Return "♀"
        Case 13 : Return "♪"
        Case 14 : Return "♫"
        Case 15 : Return "☼"
        Case 16 : Return "►"
        Case 17 : Return "◄"
        Case 18 : Return "↕"
        Case 19 : Return "‼"
        Case 20 : Return "¶"
        Case 21 : Return "§"
        Case 22 : Return "▬"
        Case 23 : Return "↨"
        Case 24 : Return "↑" ' ?
        Case 25 : Return "↓" ' ?

        Case 26 : Return "→"
        Case 27 : Return "←"
        Case 28 : Return "∟"
        Case 29 : Return "↔"

        Case 30 : Return "▲" '?
        Case 31 : Return "▼" '?

        Case 32 : Return " "

        Case 127 : Return "∆"
        Case 128 : Return " "
        Case 129 : Return " "
        Case 130 : Return " "
        Case 131 : Return " "
        Case 132 : Return " "
        Case 133 : Return " "
        Case 134 : Return " "
        Case 135 : Return " "
        Case 136 : Return " "
        Case 137 : Return " "
        Case 138 : Return " "
        Case 139 : Return " "
        Case 140 : Return " "
        Case 141 : Return " "
        Case 142 : Return " "
        Case 143 : Return " "
        Case 144 : Return " "
        Case 145 : Return " "
        Case 146 : Return " "
        Case 147 : Return " "
        Case 148 : Return " "
        Case 149 : Return " "
        Case 150 : Return " "
        Case 151 : Return " "
        Case 152 : Return " "
        Case 153 : Return " "
        Case 154 : Return " "
        Case 155 : Return "¢"
        Case 156 : Return "£"
        Case 157 : Return "¥"
        Case 158 : Return " "
        Case 159 : Return "ƒ"
        Case 160 : Return "á"
        Case 161 : Return "í"
        Case 162 : Return "ó"
        Case 163 : Return "ú"
        Case 164 : Return "ñ"
        Case 165 : Return "Ñ"
        Case 166 : Return " "
        Case 167 : Return " "
        Case 168 : Return "¿"
        Case 169 : Return "⌐"
        Case 170 : Return "¬"
        Case 171 : Return "½"
        Case 172 : Return "¼"
        Case 173 : Return "¡"
        Case 174 : Return "«"
        Case 175 : Return "»"

        Case 176 : Return "░"
        Case 177 : Return "▒"
        Case 178 : Return "▓"
        Case 179 : Return "│"
        Case 180 : Return "┤"
        Case 181 : Return "╡"
        Case 182 : Return "╢"
        Case 183 : Return "╖"
        Case 184 : Return "╕"
        Case 185 : Return "╣"
        Case 186 : Return "║"
        Case 187 : Return "╗"
        Case 188 : Return "╝"
        Case 189 : Return "╜"
        Case 190 : Return "╛"
        Case 191 : Return "┐"
        Case 192 : Return "└"
        Case 193 : Return "┴"
        Case 194 : Return "┬"
        Case 195 : Return "├"
        Case 196 : Return "─"
        Case 197 : Return "┼"
        Case 198 : Return "╞"
        Case 199 : Return "╟"
        Case 200 : Return "╚"
        Case 201 : Return "╔"
        Case 202 : Return "╩"
        Case 203 : Return "╦"
        Case 204 : Return "╠"
        Case 205 : Return "═"
        Case 206 : Return "╬"
        Case 207 : Return "╧"
        Case 208 : Return "╨"
        Case 209 : Return "╤"
        Case 210 : Return "╥"
        Case 211 : Return "╙"
        Case 212 : Return "╘"
        Case 213 : Return "╒"
        Case 214 : Return "╓"
        Case 215 : Return "╫"
        Case 216 : Return "╪"
        Case 217 : Return "┘"
        Case 218 : Return "┌"
        Case 219 : Return "█"
        Case 220 : Return "▄"
        Case 221 : Return "▌"
        Case 222 : Return "▐"
        Case 223 : Return "▀"
        Case 224 : Return " "
        Case 225 : Return " "
        Case 226 : Return " "
        Case 227 : Return " "
        Case 228 : Return "Ʃ"
        Case 229 : Return " "
        Case 230 : Return "µ"
        Case 231 : Return " "
        Case 232 : Return " "
        Case 233 : Return " "
        Case 234 : Return "Ω"
        Case 235 : Return " "
        Case 236 : Return " "
        Case 237 : Return " "
        Case 238 : Return " "
        Case 239 : Return " "
        Case 240 : Return " "
        Case 241 : Return "±"
        Case 242 : Return " "
        Case 243 : Return " "
        Case 244 : Return " "
        Case 245 : Return " "
        Case 246 : Return " "
        Case 247 : Return " "
        Case 248 : Return " "
        Case 249 : Return "●" ' ?
        Case 250 : Return "∙"
        Case 251 : Return "√"
        Case 252 : Return " "
        Case 253 : Return " "
        Case 254 : Return "█"
        Case 255 : Return " "

        Case Else
          Return Chr(value)
      End Select

    End Function

#Region "STRING"

    Public Shared Function QBString(count As Double, char%) As String
      Return New String(QBChr(char%)(0), CInt(count))
    End Function

    Public Shared Function QBString(count%, char%) As String
      Return New String(QBChr(char%)(0), count%)
    End Function

    Public Shared Function QBString(count%, char$) As String
      Return New String(char$(0), count%)
    End Function

#End Region

    ' Arrays and Data

    Private Shared m_data As New List(Of String)
    Private Shared m_dataIndex As Integer = 0

    Public Shared Sub DATA(ParamArray values$())
      For Each value In values$
        m_data.Add(value)
      Next
    End Sub

    Public Shared Sub READ(ByRef value$)
      If m_dataIndex < m_data.Count - 1 Then
        value$ = m_data(m_dataIndex)
        m_dataIndex += 1
      Else
        Throw New IndexOutOfRangeException()
      End If
    End Sub

    Public Shared Sub RESTORE()
      m_dataIndex = 0
    End Sub

    ' Math

#Region "ABS"

    Public Shared Function ABS(value As Integer) As Integer
      Return Math.Abs(value)
    End Function

    Public Shared Function ABS(value As String) As Integer
      Return Math.Abs(CInt(value))
    End Function

    Public Shared Function ABS(value As Boolean) As Integer
      Return Math.Abs(CInt(value))
    End Function

    Public Shared Function ABS(value As Long) As Long
      Return Math.Abs(value)
    End Function

#End Region

    ' Simple I/O

    Public Shared Sub CLS()
      Console.Clear()
    End Sub

    Public Shared Sub CLS(viewport As Integer)
      Select Case viewport
        Case 0, 1, 2
          CLS() 'TODO: For now, just call the base.
        Case Else
          Throw New ArgumentException
      End Select
    End Sub

    Public Shared Function CSRLIN() As Integer
      Return Console.CursorTop + 1
    End Function

    Public Shared Function INKEY$()
      If Console.KeyAvailable Then
        Dim cki = Console.ReadKey(True)

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
            Return Chr(0) & Chr(83)
          Case ConsoleKey.Home
            Return Chr(0) & Chr(71)
          Case ConsoleKey.UpArrow
            Return Chr(0) & Chr(72)
          Case ConsoleKey.PageUp
            Return Chr(0) & Chr(73)
          Case ConsoleKey.End
            Return Chr(0) & Chr(79)
          Case ConsoleKey.DownArrow
            Return Chr(0) & Chr(80)
          Case ConsoleKey.PageDown
            Return Chr(0) & Chr(81)

          Case ConsoleKey.LeftArrow
            Return Chr(0) & Chr(75)
          Case ConsoleKey.RightArrow
            Return Chr(0) & Chr(77)

          Case ConsoleKey.F1
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(84 + adder), Chr(59))
          Case ConsoleKey.F2
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(85 + adder), Chr(60))
          Case ConsoleKey.F3
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(86 + adder), Chr(61))
          Case ConsoleKey.F4
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(87 + adder), Chr(62))
          Case ConsoleKey.F5
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(88 + adder), Chr(63))
          Case ConsoleKey.F6
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(89 + adder), Chr(64))
          Case ConsoleKey.F7
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(80 + adder), Chr(65))
          Case ConsoleKey.F8
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(91 + adder), Chr(66))
          Case ConsoleKey.F9
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(92 + adder), Chr(67))
          Case ConsoleKey.F10
            Return Chr(0) & If(shift OrElse ctrl OrElse alt, Chr(93 + adder), Chr(68))
          Case ConsoleKey.F11
            If shift Then
              Return Chr(0) & Chr(135)
            ElseIf ctrl Then
              Return Chr(0) & Chr(137)
            ElseIf alt Then
              Return Chr(0) & Chr(139)
            Else
              Return Chr(0) & Chr(133)
            End If
          Case ConsoleKey.F12
            If shift Then
              Return Chr(0) & Chr(136)
            ElseIf ctrl Then
              Return Chr(0) & Chr(138)
            ElseIf alt Then
              Return Chr(0) & Chr(140)
            Else
              Return Chr(0) & Chr(134)
            End If
          Case Else
            Return cki.KeyChar
        End Select
      Else
        Return ""
      End If
    End Function

    Public Shared Sub INPUT(ByRef result%)
      result% = CInt(Console.ReadLine())
    End Sub

    Public Shared Sub INPUT(ByRef result&)
      result& = CLng(Console.ReadLine())
    End Sub

    Public Shared Sub INPUT(ByRef result!)
      result! = CSng(Console.ReadLine())
    End Sub

    Public Shared Sub INPUT(ByRef result#)
      result# = CDbl(Console.ReadLine())
    End Sub

    Public Shared Sub INPUT(ByRef result$)
      result$ = Console.ReadLine()
    End Sub

    Public Shared Sub INPUT(prompt$, ByRef result$)
      Console.Write(prompt$ & " ")
      result$ = Console.ReadLine()
    End Sub

    Public Shared Function INPUT$(n%, Optional filenumber% = -1)
      If filenumber% = -1 Then
        Dim result$ = ""
        Do
          Dim A$ = INKEY()
          If A$ <> "" Then
            result$ &= A$ : A$ = ""
            If Len(result$) >= n% Then
              Return result$
            End If
          End If
        Loop
      Else
        Throw New NotImplementedException()
      End If
    End Function

    Public Shared Sub LOCATE(Optional row% = -1, Optional column% = -1, Optional a% = -1, Optional b% = -1, Optional c% = -1)
      If row% = -1 AndAlso column% > 0 Then
        Console.CursorLeft = column% - 1
      ElseIf column% = -1 AndAlso row% > 0 Then
        Console.CursorTop = row% - 1
      ElseIf row% > 0 AndAlso column% > 0 Then
        Console.SetCursorPosition(column% - 1, row% - 1)
      End If
    End Sub

    Public Shared Function POS(value%) As Integer
      Return Console.CursorLeft + 1
    End Function

#Region "PRINT, PRINT USING"

    Public Shared Sub PRINT()
      Console.WriteLine()
    End Sub

    Public Shared Sub PRINT(text$, Optional noCr As Boolean = False)
      'TODO: (Possibly) Need to take into account the usage of TAB
      '      Tab should return a token that can then be used
      '      within the PRINT statement in order to do 
      '      specific formatting.  This is because the TAB
      '      could be called after other formatting (USING) takes
      '      place.  So should determine the total output and then
      '      adjust for TAB(s).
      If noCr Then
        Console.Write(text)
      Else
        Console.WriteLine(text)
      End If
    End Sub

    Public Shared Sub PRINT_USING(template$, ParamArray values$())
      Console.Write([USING](template$, values$))
    End Sub

    Public Shared Function [USING](template$, ParamArray values$()) As String
      'TODO: Need to actually implement...
      Dim result$ = ""
      For Each value$ In values$
        result$ &= value$
      Next
      Return result$
    End Function

#End Region

    Public Shared Function TAB(column%) As String
      If column% <= 32767 Then
        If column% < 0 Then
          column% = 1
        Else
          column% = column% Mod 80
        End If
        Dim current% = Console.CursorLeft + 1
        Dim count% = column% - current%
        Dim result$ = ""
        If count% < 0 Then
          result$ = vbCrLf : count% = column%
        End If
        result$ &= Space(count%)
        Return result$
      Else
        Throw New ArgumentException
      End If
    End Function


    Public Shared Sub WIDTH(Optional a% = -1, Optional b% = -1)
      Console.ForegroundColor = ConsoleColor.Gray
      Console.BackgroundColor = ConsoleColor.Black
      Console.Clear()
    End Sub

    ' Error Trapping

    ' Timing, Date, and Time

    Public Shared Function QBDate() As String
      Return Today.ToString("MM-dd-yyyy")
    End Function

#Region "SLEEP"

    Public Shared Sub SLEEP()
      Do
        If INKEY() <> "" Then
          Exit Do
        End If
        Threading.Thread.Sleep(100)
      Loop
    End Sub

    Public Shared Sub SLEEP(seconds As Integer)
      Dim till = DateAdd(DateInterval.Second, seconds, Now)
      Do
        If INKEY() <> "" OrElse
           Now >= till Then
          Exit Do
        End If
        Threading.Thread.Sleep(100)
      Loop
    End Sub

#End Region

    Public Shared Function QBTime() As String
      Return Now.ToString("HH:mm:ss")
    End Function

    Public Shared Sub QBTime(value As String)
      'NOTE: Set the time???? (Is this even possible?)
    End Sub

    Public Shared Function QBTimer() As Long
      Dim midnight = New Date(Now.Year, Now.Month, Now.Day)
      Return DateDiff(DateInterval.Second, midnight, Now)
    End Function

  End Class

End Namespace