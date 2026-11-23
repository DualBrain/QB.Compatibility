Option Explicit On
Option Strict On
Option Infer On

Imports System.Text

Namespace Global.QB

  ''' <summary>Conversion between code page 437 bytes (what QBasic files and screens use) and characters.</summary>
  Friend NotInheritable Class Cp437

    Private Sub New()
    End Sub

    Private Shared ReadOnly s_encoding As Encoding = CreateEncoding()

    ''' <summary>The characters QBChr produces (including the glyphs for codes 1 to 31) and their codes.</summary>
    Friend Shared ReadOnly Map As Dictionary(Of Char, Integer) = Build()

    Private Shared Function CreateEncoding() As Encoding
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
      Return Encoding.GetEncoding(437)
    End Function

    Private Shared Function Build() As Dictionary(Of Char, Integer)
      Dim result As New Dictionary(Of Char, Integer)
      For code = 1 To 255
        Dim s = Core.QBChr(code)
        If s.Length = 1 AndAlso Not result.ContainsKey(s(0)) Then result.Add(s(0), code)
      Next
      Return result
    End Function

    ''' <summary>The byte for a character: ASCII as is, QBChr glyphs and other CP437 characters by code, else "?".</summary>
    Friend Shared Function ToByte(c As Char) As Byte
      If AscW(c) < 128 Then Return CByte(AscW(c))
      Dim code As Integer
      If Map.TryGetValue(c, code) Then Return CByte(code)
      Dim b = s_encoding.GetBytes(c.ToString)
      Return If(b.Length = 1, b(0), CByte(63))
    End Function

    ''' <summary>The character of a byte: ASCII (and control characters) as is, 128 to 255 from code page 437.</summary>
    Friend Shared Function ToChar(b As Byte) As Char
      If b < 128 Then Return ChrW(b)
      Return s_encoding.GetChars(New Byte() {b})(0)
    End Function

  End Class

End Namespace
