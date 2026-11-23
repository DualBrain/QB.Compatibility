Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ' MKI$ / MKL$ / MKS$ / MKD$ and CVI / CVL / CVS / CVD, the Microsoft Binary format ones (MKSMBF$ ...), LSET and RSET.
  ' The strings hold one character per byte (code page 437), so they can be written to files and fields as they are.
  Partial Public NotInheritable Class Devices

    Private Shared Function BytesToString(bytes() As Byte) As String
      Dim chars(bytes.Length - 1) As Char
      For i = 0 To bytes.Length - 1
        chars(i) = Cp437.ToChar(bytes(i))
      Next
      Return New String(chars)
    End Function

    Private Shared Function StringToBytes(text As String, count As Integer) As Byte()
      If text Is Nothing OrElse text.Length < count Then Throw New ArgumentException("Illegal function call")
      Dim bytes(count - 1) As Byte
      For i = 0 To count - 1
        bytes(i) = Cp437.ToByte(text(i))
      Next
      Return bytes
    End Function

#Region "MKI$, MKL$, MKS$, MKD$ and CVI, CVL, CVS, CVD (IEEE)"

    ''' <summary>MKI$: a 2-byte string holding an integer (a QBasic INTEGER is a VB Short).</summary>
    Public Shared Function MKI(value As Short) As String
      Return BytesToString(BitConverter.GetBytes(value))
    End Function

    ''' <summary>MKL$: a 4-byte string holding a long integer (a QBasic LONG is a VB Integer).</summary>
    Public Shared Function MKL(value As Integer) As String
      Return BytesToString(BitConverter.GetBytes(value))
    End Function

    ''' <summary>MKS$: a 4-byte string holding a single-precision number.</summary>
    Public Shared Function MKS(value As Single) As String
      Return BytesToString(BitConverter.GetBytes(value))
    End Function

    ''' <summary>MKD$: an 8-byte string holding a double-precision number.</summary>
    Public Shared Function MKD(value As Double) As String
      Return BytesToString(BitConverter.GetBytes(value))
    End Function

    ''' <summary>CVI: the integer in the first 2 bytes of a string.</summary>
    Public Shared Function CVI(text As String) As Short
      Return BitConverter.ToInt16(StringToBytes(text, 2), 0)
    End Function

    ''' <summary>CVL: the long integer in the first 4 bytes of a string.</summary>
    Public Shared Function CVL(text As String) As Integer
      Return BitConverter.ToInt32(StringToBytes(text, 4), 0)
    End Function

    ''' <summary>CVS: the single-precision number in the first 4 bytes of a string.</summary>
    Public Shared Function CVS(text As String) As Single
      Return BitConverter.ToSingle(StringToBytes(text, 4), 0)
    End Function

    ''' <summary>CVD: the double-precision number in the first 8 bytes of a string.</summary>
    Public Shared Function CVD(text As String) As Double
      Return BitConverter.ToDouble(StringToBytes(text, 8), 0)
    End Function

#End Region

#Region "MKSMBF$, MKDMBF$ and CVSMBF, CVDMBF (Microsoft Binary format, as older versions of BASIC stored numbers)"

    ''' <summary>MKSMBF$: an IEEE single as a 4-byte Microsoft Binary format string.</summary>
    Public Shared Function MKSMBF(value As Single) As String
      Dim bits = BitConverter.SingleToInt32Bits(value)
      Dim exponent = (bits >> 23) And &HFF
      Dim bytes(3) As Byte
      If exponent = 0 Then Return BytesToString(bytes) ' Zero (and denormals).
      If exponent = &HFF Then Throw New OverflowException
      Dim mbfExponent = exponent + 2                    ' MBF has the binary point before the leading 1.
      If mbfExponent > 255 Then Throw New OverflowException
      Dim fraction = bits And &H7FFFFF
      bytes(0) = CByte(fraction And &HFF)
      bytes(1) = CByte((fraction >> 8) And &HFF)
      bytes(2) = CByte(((fraction >> 16) And &H7F) Or (If(bits < 0, &H80, 0)))
      bytes(3) = CByte(mbfExponent)
      Return BytesToString(bytes)
    End Function

    ''' <summary>CVSMBF: the IEEE single in a 4-byte Microsoft Binary format string.</summary>
    Public Shared Function CVSMBF(text As String) As Single
      Dim b = StringToBytes(text, 4)
      If b(3) = 0 Then Return 0
      Dim exponent = b(3) - 2
      If exponent <= 0 Then Return 0
      Dim fraction = CInt(b(0)) Or (CInt(b(1)) << 8) Or (CInt(b(2) And &H7F) << 16)
      Dim bits = (exponent << 23) Or fraction
      If (b(2) And &H80) <> 0 Then bits = bits Or Integer.MinValue
      Return BitConverter.Int32BitsToSingle(bits)
    End Function

    ''' <summary>MKDMBF$: an IEEE double as an 8-byte Microsoft Binary format string.</summary>
    Public Shared Function MKDMBF(value As Double) As String
      Dim bits = BitConverter.DoubleToInt64Bits(value)
      Dim exponent = CInt((bits >> 52) And &H7FFL)
      Dim bytes(7) As Byte
      If exponent = 0 Then Return BytesToString(bytes)
      If exponent = &H7FF Then Throw New OverflowException
      Dim mbfExponent = exponent - 1023 + 129
      If mbfExponent < 1 Then Return BytesToString(bytes)
      If mbfExponent > 255 Then Throw New OverflowException
      Dim mantissa = (bits And &HFFFFFFFFFFFFFL) << 3          ' 52 bits become the top of 55.
      Dim sign56 = mantissa Or (If(bits < 0, 1L << 55, 0L))
      For i = 0 To 6
        bytes(i) = CByte((sign56 >> (8 * i)) And &HFFL)
      Next
      bytes(7) = CByte(mbfExponent)
      Return BytesToString(bytes)
    End Function

    ''' <summary>CVDMBF: the IEEE double in an 8-byte Microsoft Binary format string.</summary>
    Public Shared Function CVDMBF(text As String) As Double
      Dim b = StringToBytes(text, 8)
      If b(7) = 0 Then Return 0
      Dim v56 = 0L
      For i = 0 To 6
        v56 = v56 Or (CLng(b(i)) << (8 * i))
      Next
      Dim negative = (v56 And (1L << 55)) <> 0
      Dim mantissa = (v56 And ((1L << 55) - 1)) >> 3
      Dim exponent = b(7) - 129 + 1023
      If exponent <= 0 Then Return 0
      Dim bits = (CLng(exponent) << 52) Or mantissa
      If negative Then bits = bits Or Long.MinValue
      Return BitConverter.Int64BitsToDouble(bits)
    End Function

#End Region

    ''' <summary>
    ''' LSET variable = text: puts the text at the left of the variable, keeping the variable's length (padding with
    ''' spaces, or cutting the text short).
    ''' </summary>
    Public Shared Sub LSET(ByRef variable As String, text As String)
      Dim width = If(variable, "").Length
      variable = If(text, "").PadRight(width).Substring(0, width)
    End Sub

    ''' <summary>RSET variable = text: puts the text at the right of the variable, keeping the variable's length.</summary>
    Public Shared Sub RSET(ByRef variable As String, text As String)
      Dim width = If(variable, "").Length
      Dim t = If(text, "")
      variable = If(t.Length >= width, t.Substring(0, width), t.PadLeft(width))
    End Sub

  End Class

End Namespace
