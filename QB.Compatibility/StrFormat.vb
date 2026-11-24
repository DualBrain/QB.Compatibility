Option Explicit On
Option Strict On
Option Infer On

Imports System.Globalization

Namespace Global.QB

  Partial Public NotInheritable Class Core

    ' STR$ the way QBasic writes a number: a space (or the minus sign) in front, a Single with at most 7 digits and a Double with at most 16,
    ' no trailing zeros, and an exponent (E for Single, D for Double) when the number is very large or very small.

    ''' <summary>STR$(n): the number as text, with a space in front of a positive number.</summary>
    Public Shared Function Str$(value As Short)
      Return Sign(value) & Math.Abs(CInt(value)).ToString(CultureInfo.InvariantCulture)
    End Function

    Public Shared Function Str$(value As Integer)
      Return Sign(value) & Math.Abs(CLng(value)).ToString(CultureInfo.InvariantCulture)
    End Function

    Public Shared Function Str$(value As Long)
      Return If(value < 0, "-", " ") & If(value = Long.MinValue, "9223372036854775808", Math.Abs(value).ToString(CultureInfo.InvariantCulture))
    End Function

    Public Shared Function Str$(value As Single)
      Return Real(value, 7, "E"c)
    End Function

    Public Shared Function Str$(value As Double)
      Return Real(value, 16, "D"c)
    End Function

    Private Shared Function Sign(value As Long) As String
      Return If(value < 0, "-", " ")
    End Function

    Private Shared Function Real(value As Double, digits As Integer, exponentLetter As Char) As String
      If Double.IsNaN(value) OrElse Double.IsInfinity(value) Then Return ""
      Dim lead = If(value < 0, "-", " ")
      Dim x = Math.Abs(value)
      If x = 0 Then Return lead & "0"
      ' d.ddd...E+xxx rounded to the digits that QBasic keeps
      Dim sci = x.ToString("E" & (digits - 1), CultureInfo.InvariantCulture)
      Dim ePos = sci.IndexOf("E"c)
      Dim exp = Integer.Parse(sci.Substring(ePos + 1), CultureInfo.InvariantCulture)
      Dim mant = sci.Substring(0, ePos).Replace(".", "").TrimEnd("0"c)
      If mant = "" Then mant = "0"
      Dim limit = If(digits = 7, 7, 16)
      If exp >= limit OrElse exp < -3 Then
        Dim m = mant.Substring(0, 1) & If(mant.Length > 1, "." & mant.Substring(1), "")
        Return lead & m & exponentLetter & If(exp < 0, "-", "+") & Math.Abs(exp).ToString("00", CultureInfo.InvariantCulture)
      End If
      If exp >= 0 Then
        If mant.Length <= exp + 1 Then Return lead & mant & New String("0"c, exp + 1 - mant.Length)
        Return lead & mant.Substring(0, exp + 1) & "." & mant.Substring(exp + 1)
      End If
      Return lead & "." & New String("0"c, -exp - 1) & mant
    End Function

  End Class

End Namespace
