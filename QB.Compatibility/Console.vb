Option Explicit On
Option Strict On
Option Infer On

Imports QB.Core

Namespace Global.QB

  Public NotInheritable Class Console

    Private Declare Function GetAsyncKeyState Lib "user32" (vkey As Integer) As Short

    Private Sub New()
    End Sub

    Public Shared Sub COLOR(fg%)
      If fg% > 15 Then fg% -= 16
      System.Console.ForegroundColor = CType(fg, ConsoleColor)
    End Sub

    Public Shared Sub COLOR(fg%, bg%)
      System.Console.ForegroundColor = CType(fg, ConsoleColor)
      System.Console.BackgroundColor = CType(bg, ConsoleColor)
    End Sub

#Region "SCREEN (Function)"

    'Public Shared Function SCREEN(r%, c%) As Integer
    '  Return 0
    'End Function

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
              'Case "√" : Return 251
            Case Else
              Return QBAsc(CChar(ch))
          End Select
        Else
          Return 0
        End If
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
        Throw New NotImplementedException
      End If
    End Sub

#End Region

    Public Shared Sub CLS()
      System.Console.Clear()
    End Sub

    Public Shared Sub CLS(viewport As Integer)
      Select Case viewport
        Case 0, 1, 2
          CLS() 'TODO: For now, just call the base.
        Case Else
          Throw New ArgumentException("viewport valid values 0, 1, or 2.", NameOf(viewport))
      End Select
    End Sub

    Public Shared Function CSRLIN() As Integer
      Return System.Console.CursorTop + 1
    End Function

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

    Public Shared Sub INPUT(ByRef result%)
      result% = CInt(System.Console.ReadLine())
    End Sub

    Public Shared Sub INPUT(ByRef result&)
      result& = CLng(System.Console.ReadLine())
    End Sub

    Public Shared Sub INPUT(ByRef result!)
      result! = CSng(System.Console.ReadLine())
    End Sub

    Public Shared Sub INPUT(ByRef result#)
      result# = CDbl(System.Console.ReadLine())
    End Sub

    Public Shared Sub INPUT(ByRef resuLtKey$)
      resuLtKey$ = System.Console.ReadLine()
    End Sub

    Public Shared Sub INPUT(prompt$, ByRef resuLtKey$)
      System.Console.Write(prompt$ & " ")
      resuLtKey$ = System.Console.ReadLine()
    End Sub

    Public Shared Sub INPUT(filenumber%, ByRef resultKey$)
      If filenumber <> 0 OrElse String.IsNullOrEmpty(resultKey) Then
      End If
      ' Read from the file a "word".
      ' Each word is separated by either a space or a comma.
      Throw New NotImplementedException
    End Sub

    Public Shared Function QBInput$(n As Long)
      Dim resuLtKey$ = ""
      Do
        Dim A$ = INKEY()
        If Not String.IsNullOrEmpty(A$) Then
          resuLtKey$ &= A$ ': A$ = ""
          If resuLtKey$?.Length >= n Then
            Return resuLtKey$
          End If
        End If
      Loop
    End Function

    Public Shared Sub LOCATE(Optional row% = -1, Optional column% = -1, Optional a% = -1, Optional b% = -1, Optional c% = -1)
      If a <> 0 OrElse b <> 0 OrElse c <> 0 Then
      End If
      If row% = -1 AndAlso column% > 0 Then
        System.Console.CursorLeft = column% - 1
      ElseIf column% = -1 AndAlso row% > 0 Then
        System.Console.CursorTop = row% - 1
      ElseIf row% > 0 AndAlso column% > 0 Then
        System.Console.SetCursorPosition(column% - 1, row% - 1)
      End If
    End Sub

    Public Shared Function POS(value%) As Integer
      If value <> 0 Then ' Value is a "dummy" value.
      End If
      Return System.Console.CursorLeft + 1
    End Function

#Region "SLEEP"

    Public Shared Sub SLEEP()
      Do
        If Not String.IsNullOrEmpty(INKEY) Then
          Exit Do
        End If
        Threading.Thread.Sleep(100)
      Loop
    End Sub

    Public Shared Sub SLEEP(seconds As Integer)
      Dim till = DateAdd(DateInterval.Second, seconds, Now)
      Do
        If Not String.IsNullOrEmpty(INKEY) OrElse
           Microsoft.VisualBasic.Now >= till Then
          Exit Do
        End If
        Threading.Thread.Sleep(100)
      Loop
    End Sub

#End Region

#Region "PRINT, PRINT USING"

    Public Shared Sub PRINT()
      System.Console.WriteLine()
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
        System.Console.Write(text)
      Else
        System.Console.WriteLine(text)
      End If
    End Sub

    Public Shared Sub PRINT_USING(template$, ParamArray values$())
      System.Console.Write([USING](template$, values$))
    End Sub

    Public Shared Function [USING](template$, ParamArray values$()) As String
      'TODO: Need to actually implement...
      If String.IsNullOrEmpty(template$) Then
      End If
      Dim resuLtKey$ = ""
      For Each value$ In values$
        resuLtKey$ &= value$
      Next
      Throw New NotImplementedException
      Return resuLtKey$
    End Function

    Public Shared Sub PRINT(filenum%, value$)
      If File.m_files.ContainsKey(filenum%) Then
        Dim b = Text.ASCIIEncoding.ASCII.GetBytes(value$ & vbCrLf)
        File.m_files(filenum%).Stream.Write(b, 0, b.Length)
      End If
    End Sub

#End Region

    Public Shared Sub LINE_INPUT(prompt$, ByRef value$)
      System.Console.Write(prompt$)
      value$ = System.Console.ReadLine()
      'value$ = ""
      'Dim b(0) As Byte
      'Do
      '  Dim result = m_files(filenum%).Read(b, 0, 1)
      '  If result = 0 Then
      '    Return
      '  End If
      '  value$ &= QBChr(b(0))
      '  If b(0) = 13 Then
      '    ' nearing end of line, assume next character is a LF...
      '    result = m_files(filenum%).Read(b, 0, 1)
      '    value$ &= QBChr(b(0))
      '    value$ = value$.Substring(0, value$.Length - 2) ' Trim of the CRLF
      '    Return
      '  End If
      'Loop
    End Sub

    Public Shared Sub LINE_INPUT(ByRef value$)
      value$ = System.Console.ReadLine()
      'value$ = ""
      'Dim b(0) As Byte
      'Do
      '  Dim result = m_files(filenum%).Read(b, 0, 1)
      '  If result = 0 Then
      '    Return
      '  End If
      '  value$ &= QBChr(b(0))
      '  If b(0) = 13 Then
      '    ' nearing end of line, assume next character is a LF...
      '    result = m_files(filenum%).Read(b, 0, 1)
      '    value$ &= QBChr(b(0))
      '    value$ = value$.Substring(0, value$.Length - 2) ' Trim of the CRLF
      '    Return
      '  End If
      'Loop
    End Sub

    Public Shared Sub LINE_INPUT(filenum%, ByRef value$)
      value$ = ""
      Dim b(0) As Byte
      Do
        Dim result = File.m_files(filenum%).Stream.Read(b, 0, 1)
        If result = 0 Then
          Return
        End If
        value$ &= QBChr(b(0))
        If b(0) = 13 Then
          ' nearing end of line, assume next character is a LF...
          Dim size = File.m_files(filenum%).Stream.Read(b, 0, 1)
          value$ &= QBChr(b(0))
          value$ = value$.Substring(0, value$.Length - 2) ' Trim of the CRLF
          Return
        End If
      Loop
    End Sub

    Public Shared Sub WIDTH(Optional a% = -1, Optional b% = -1)
      If a <> 0 OrElse b <> 0 Then
      End If
      System.Console.ForegroundColor = ConsoleColor.Gray
      System.Console.BackgroundColor = ConsoleColor.Black
      System.Console.Clear()
    End Sub

  End Class

End Namespace