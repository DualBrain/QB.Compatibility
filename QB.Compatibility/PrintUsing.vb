Option Explicit On
Option Strict On
Option Infer On

Imports System.Globalization
Imports System.Text

Namespace Global.QB

  ''' <summary>The formatting engine behind PRINT USING.</summary>
  Friend NotInheritable Class PrintUsing

    Private Sub New()
    End Sub

    Private Structure NumericField
      Friend Length As Integer     ' Characters of the field in the template.
      Friend LeadPlus As Boolean
      Friend TrailSign As Char     ' "+" or "-" (or NUL).
      Friend Star As Boolean       ' ** fill
      Friend Dollar As Boolean     ' $$ floating dollar sign
      Friend Comma As Boolean
      Friend HasPoint As Boolean
      Friend Digits As Integer     ' # before the decimal point.
      Friend Decimals As Integer   ' # after the decimal point.
      Friend Exponent As Integer   ' 4 or 5 for ^^^^ / ^^^^^, else 0.
    End Structure

    Friend Shared Function Format(template As String, values() As Object) As String

      If String.IsNullOrEmpty(template) OrElse values Is Nothing OrElse values.Length = 0 Then
        Throw New ArgumentException("Illegal function call")
      End If

      Dim sb As New StringBuilder
      Dim vi = 0

      Do

        Dim fields = 0
        Dim i = 0

        Do While i < template.Length

          Dim ch = template(i)

          If ch = "_"c AndAlso i + 1 < template.Length Then ' Next character is a literal.
            sb.Append(template(i + 1))
            i += 2
            Continue Do
          End If

          If ch = "!"c OrElse ch = "&"c Then

            If vi >= values.Length Then Return sb.ToString
            Dim s = AsText(values(vi))
            vi += 1 : fields += 1 : i += 1
            If ch = "!"c Then sb.Append(If(s.Length > 0, s.Substring(0, 1), " ")) Else sb.Append(s)
            Continue Do

          ElseIf ch = "\"c Then

            Dim j = template.IndexOf("\"c, i + 1)
            If j > 0 AndAlso template.Substring(i + 1, j - i - 1).Trim(" "c).Length = 0 Then
              If vi >= values.Length Then Return sb.ToString
              Dim width = j - i + 1
              Dim s = AsText(values(vi))
              vi += 1 : fields += 1 : i = j + 1
              sb.Append(If(s.Length > width, s.Substring(0, width), s.PadRight(width)))
              Continue Do
            End If

          Else

            Dim f As NumericField
            If TryParseNumeric(template, i, f) Then
              If vi >= values.Length Then Return sb.ToString
              sb.Append(FormatNumber(values(vi), f))
              vi += 1 : fields += 1 : i += f.Length
              Continue Do
            End If

          End If

          sb.Append(ch)
          i += 1

        Loop

        If fields = 0 Then Throw New ArgumentException("Illegal function call")

      Loop While vi < values.Length

      Return sb.ToString

    End Function

    Private Shared Function AsText(value As Object) As String
      If TypeOf value Is String Then Return DirectCast(value, String)
      If TypeOf value Is Char Then Return value.ToString
      Throw New ArgumentException("Type mismatch")
    End Function

    Private Shared Function TryParseNumeric(t As String, start As Integer, ByRef f As NumericField) As Boolean

      f = New NumericField
      Dim j = start

      If t(j) = "+"c Then f.LeadPlus = True : j += 1

      If String.CompareOrdinal(t, j, "**$", 0, 3) = 0 Then
        f.Star = True : f.Dollar = True : j += 3
      ElseIf String.CompareOrdinal(t, j, "**", 0, 2) = 0 Then
        f.Star = True : j += 2
      ElseIf String.CompareOrdinal(t, j, "$$", 0, 2) = 0 Then
        f.Dollar = True : j += 2
      End If

      Do While j < t.Length AndAlso (t(j) = "#"c OrElse t(j) = ","c)
        If t(j) = ","c Then f.Comma = True Else f.Digits += 1
        j += 1
      Loop

      If j < t.Length AndAlso t(j) = "."c Then
        Dim k = j + 1
        Dim d = 0
        Do While k < t.Length AndAlso t(k) = "#"c
          d += 1 : k += 1
        Loop
        If f.Digits > 0 OrElse d > 0 Then
          f.HasPoint = True : f.Decimals = d : j = k
        End If
      End If

      If f.Digits = 0 AndAlso f.Decimals = 0 Then Return False

      If String.CompareOrdinal(t, j, "^^^^^", 0, 5) = 0 Then
        f.Exponent = 5 : j += 5
      ElseIf String.CompareOrdinal(t, j, "^^^^", 0, 4) = 0 Then
        f.Exponent = 4 : j += 4
      End If

      If Not f.LeadPlus AndAlso j < t.Length AndAlso (t(j) = "+"c OrElse t(j) = "-"c) Then
        f.TrailSign = t(j) : j += 1
      End If

      f.Length = j - start
      Return True

    End Function

    Private Shared Function FormatNumber(value As Object, f As NumericField) As String

      If TypeOf value Is String OrElse TypeOf value Is Char OrElse value Is Nothing Then
        Throw New ArgumentException("Type mismatch")
      End If

      Dim d = Convert.ToDouble(value, CultureInfo.InvariantCulture)
      Dim magnitude = Math.Abs(d)
      Dim digits As String
      Dim suffix = ""

      Dim dec = Math.Min(f.Decimals, 15)

      If f.Exponent > 0 Then

        Dim exp10 = 0
        Dim mant = 0.0
        If magnitude <> 0 Then
          exp10 = CInt(Math.Floor(Math.Log10(magnitude))) - (Math.Max(f.Digits, 1) - 1)
          mant = Math.Round(magnitude / Math.Pow(10, exp10), dec, MidpointRounding.AwayFromZero)
          If mant >= Math.Pow(10, Math.Max(f.Digits, 1)) Then mant /= 10 : exp10 += 1
        End If
        digits = mant.ToString("F" & dec, CultureInfo.InvariantCulture)
        suffix = "E" & If(exp10 < 0, "-", "+") & Math.Abs(exp10).ToString(If(f.Exponent = 5, "000", "00"), CultureInfo.InvariantCulture)
        magnitude = mant

      Else

        magnitude = Math.Round(magnitude, dec, MidpointRounding.AwayFromZero)
        digits = magnitude.ToString(If(f.Comma, "N", "F") & dec, CultureInfo.InvariantCulture)

      End If

      If f.Digits = 0 AndAlso magnitude < 1 AndAlso digits.StartsWith("0") Then digits = digits.Substring(1)
      If f.HasPoint AndAlso f.Decimals = 0 Then digits &= "."

      Dim negative = d < 0 AndAlso magnitude <> 0

      Dim body = ""
      If f.LeadPlus Then
        body = If(negative, "-", "+")
      ElseIf f.TrailSign = ChrW(0) AndAlso negative Then
        body = "-"
      End If
      If f.Dollar Then body &= "$"
      body &= digits & suffix
      If f.TrailSign = "+"c Then
        body &= If(negative, "-", "+")
      ElseIf f.TrailSign = "-"c Then
        body &= If(negative, "-", " ")
      End If

      If body.Length > f.Length Then Return "%" & body
      Return body.PadLeft(f.Length, If(f.Star, "*"c, " "c))

    End Function

  End Class

End Namespace
