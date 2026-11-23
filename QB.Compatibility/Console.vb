Option Explicit On
Option Strict On
Option Infer On

Imports QB.Core

Namespace Global.QB

  Public NotInheritable Class Console

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
        Return KeyCodec.Encode(cki.Key, cki.KeyChar,
                               (cki.Modifiers And ConsoleModifiers.Shift) <> 0,
                               (cki.Modifiers And ConsoleModifiers.Control) <> 0,
                               (cki.Modifiers And ConsoleModifiers.Alt) <> 0)
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
      File.INPUT(filenumber, resultKey)
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

    Public Shared Sub PRINT_USING(template$, ParamArray values() As Object)
      System.Console.WriteLine([USING](template$, values))
    End Sub

    Public Shared Function [USING](template$, ParamArray values() As Object) As String
      Return PrintUsing.Format(template$, values)
    End Function

    Public Shared Sub PRINT(filenum%, value$)
      File.PRINT(filenum, value)
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
      File.LINE_INPUT(filenum, value)
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