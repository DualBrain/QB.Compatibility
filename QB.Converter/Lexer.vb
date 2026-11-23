Option Explicit On
Option Strict On
Option Infer On

Imports System.Text

Namespace QB2VB

  Friend Enum TokKind
    Ident
    Number
    [String]
    [Op]
  End Enum

  Friend Class Token
    Public Kind As TokKind
    ''' <summary>The text as written (identifiers keep their suffix, strings are without the quotes).</summary>
    Public Text As String = ""
    Public Function Upper() As String
      Return Text.ToUpperInvariant()
    End Function
    Public Function Is_(op As String) As Boolean
      Return Kind = TokKind.Op AndAlso Text = op
    End Function
    Public Function IsWord(word As String) As Boolean
      Return Kind = TokKind.Ident AndAlso String.Equals(Text, word, StringComparison.OrdinalIgnoreCase)
    End Function
  End Class

  ''' <summary>One logical line of QBasic source (physical lines joined at a trailing underscore).</summary>
  Friend Class SourceLine
    Public Number As Integer              ' 1-based number of its first physical line
    Public Indent As String = ""
    Public LineNumber As String = ""      ' a leading numeric line number
    Public Label As String = ""           ' a leading "Name:" label
    Public Tokens As New List(Of Token)   ' the statement tokens, without the comment
    Public Comment As String = ""         ' the trailing comment, as written (starting with ' or REM)
    Public Raw As String = ""
    Public ReadOnly Property IsBlank As Boolean
      Get
        Return Tokens.Count = 0 AndAlso Comment = "" AndAlso LineNumber = "" AndAlso Label = ""
      End Get
    End Property
    Public ReadOnly Property IsCommentOnly As Boolean
      Get
        Return Tokens.Count = 0 AndAlso Comment <> "" AndAlso LineNumber = "" AndAlso Label = ""
      End Get
    End Property
  End Class

  Friend NotInheritable Class Lexer

    Private Sub New()
    End Sub

    Friend Shared Function Split(source As String) As List(Of SourceLine)
      Dim physical = source.Replace(vbCrLf, vbLf).Replace(vbCr, vbLf).Split(ControlChars.Lf)
      If physical.Length > 0 AndAlso physical(physical.Length - 1) = "" Then ReDim Preserve physical(physical.Length - 2)
      Dim lines As New List(Of SourceLine)
      Dim i = 0
      Do While i < physical.Length
        Dim start = i
        Dim text = physical(i)
        ' A trailing underscore (after a space) continues the statement on the next line.
        Do While i + 1 < physical.Length AndAlso text.TrimEnd().EndsWith(" _", StringComparison.Ordinal) AndAlso Not InComment(text)
          text = text.TrimEnd()
          text = text.Substring(0, text.Length - 1).TrimEnd() & " " & physical(i + 1).TrimStart()
          i += 1
        Loop
        lines.Add(LexLine(text, start + 1))
        i += 1
      Loop
      Return lines
    End Function

    Private Shared Function InComment(text As String) As Boolean
      Dim inStr = False
      For Each c In text
        If c = """"c Then inStr = Not inStr
        If c = "'"c AndAlso Not inStr Then Return True
      Next
      Return False
    End Function

    Private Shared Function IsIdentStart(c As Char) As Boolean
      Return (c >= "A"c AndAlso c <= "Z"c) OrElse (c >= "a"c AndAlso c <= "z"c)
    End Function

    Private Shared Function IsIdentChar(c As Char) As Boolean
      Return IsIdentStart(c) OrElse (c >= "0"c AndAlso c <= "9"c) OrElse c = "."c
    End Function

    Friend Shared Function LexLine(text As String, number As Integer) As SourceLine

      Dim sl As New SourceLine With {.Number = number, .Raw = text}
      Dim p = 0
      While p < text.Length AndAlso (text(p) = " "c OrElse text(p) = vbTab)
        p += 1
      End While
      sl.Indent = text.Substring(0, p)

      Dim first = True
      Do While p < text.Length

        Dim c = text(p)
        If c = " "c OrElse c = vbTab Then
          p += 1
          Continue Do
        End If

        ' Comments: ' to the end of the line, and REM at the start of a statement.
        If c = "'"c Then
          sl.Comment = text.Substring(p)
          Exit Do
        End If

        Dim atStatementStart = sl.Tokens.Count = 0 OrElse sl.Tokens(sl.Tokens.Count - 1).Is_(":")

        If IsIdentStart(c) Then
          Dim s = p
          While p < text.Length AndAlso IsIdentChar(text(p))
            p += 1
          End While
          If p < text.Length AndAlso "$%&!#".IndexOf(text(p)) >= 0 Then p += 1
          Dim word = text.Substring(s, p - s)
          If atStatementStart AndAlso String.Equals(word, "REM", StringComparison.OrdinalIgnoreCase) Then
            sl.Comment = text.Substring(s)
            Exit Do
          End If
          ' A label: a word followed by a colon, first on the line (and not a statement word).
          If first AndAlso sl.Tokens.Count = 0 AndAlso p < text.Length AndAlso text(p) = ":"c AndAlso Not Keywords.IsStatementWord(word) Then
            sl.Label = word
            p += 1
            first = False
            Continue Do
          End If
          sl.Tokens.Add(New Token With {.Kind = TokKind.Ident, .Text = word})
          first = False
          Continue Do
        End If

        If (c >= "0"c AndAlso c <= "9"c) OrElse (c = "."c AndAlso p + 1 < text.Length AndAlso Char.IsDigit(text(p + 1))) Then
          Dim s = p
          ' A line number: digits first on the line.
          If first AndAlso sl.Tokens.Count = 0 AndAlso Char.IsDigit(c) Then
            While p < text.Length AndAlso Char.IsDigit(text(p))
              p += 1
            End While
            If p >= text.Length OrElse text(p) = " "c OrElse text(p) = vbTab OrElse text(p) = ":"c Then
              sl.LineNumber = text.Substring(s, p - s)
              first = False
              If p < text.Length AndAlso text(p) = ":"c Then p += 1
              Continue Do
            End If
            p = s
          End If
          p = LexNumber(text, p)
          sl.Tokens.Add(New Token With {.Kind = TokKind.Number, .Text = text.Substring(s, p - s)})
          first = False
          Continue Do
        End If

        If c = "&"c AndAlso p + 1 < text.Length AndAlso (text(p + 1) = "H"c OrElse text(p + 1) = "h"c OrElse text(p + 1) = "O"c OrElse text(p + 1) = "o"c) Then
          Dim s = p
          p += 2
          While p < text.Length AndAlso Uri.IsHexDigit(text(p))
            p += 1
          End While
          If p < text.Length AndAlso (text(p) = "&"c) Then p += 1
          sl.Tokens.Add(New Token With {.Kind = TokKind.Number, .Text = text.Substring(s, p - s)})
          first = False
          Continue Do
        End If

        If c = """"c Then
          Dim e = text.IndexOf(""""c, p + 1)
          If e < 0 Then e = text.Length
          sl.Tokens.Add(New Token With {.Kind = TokKind.String, .Text = text.Substring(p + 1, e - p - 1)})
          p = Math.Min(text.Length, e + 1)
          first = False
          Continue Do
        End If

        ' Operators.
        Dim two = If(p + 1 < text.Length, text.Substring(p, 2), "")
        If two = "<>" OrElse two = "<=" OrElse two = ">=" OrElse two = "=<" OrElse two = "=>" OrElse two = "><" Then
          sl.Tokens.Add(New Token With {.Kind = TokKind.Op, .Text = two})
          p += 2
        Else
          sl.Tokens.Add(New Token With {.Kind = TokKind.Op, .Text = c.ToString})
          p += 1
        End If
        first = False

      Loop

      Return sl

    End Function

    ' Digits, a decimal point, an exponent and a type suffix.
    Private Shared Function LexNumber(text As String, start As Integer) As Integer
      Dim p = start
      While p < text.Length AndAlso Char.IsDigit(text(p))
        p += 1
      End While
      If p < text.Length AndAlso text(p) = "."c Then
        p += 1
        While p < text.Length AndAlso Char.IsDigit(text(p))
          p += 1
        End While
      End If
      If p < text.Length AndAlso "EeDd".IndexOf(text(p)) >= 0 Then
        Dim q = p + 1
        If q < text.Length AndAlso (text(q) = "+"c OrElse text(q) = "-"c) Then q += 1
        If q < text.Length AndAlso Char.IsDigit(text(q)) Then
          While q < text.Length AndAlso Char.IsDigit(text(q))
            q += 1
          End While
          p = q
        End If
      End If
      If p < text.Length AndAlso "%&!#".IndexOf(text(p)) >= 0 Then p += 1
      Return p
    End Function

  End Class

End Namespace
