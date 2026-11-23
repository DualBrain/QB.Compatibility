Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>
  ''' QBasic's fixed-length strings (<c>DIM name AS STRING * 20</c>): always the same length.  A VB String variable holds one if every
  ''' assignment goes through <see cref="Fit"/>; the converter does that.  Before anything is assigned a fixed-length string is filled
  ''' with CHR$(0), as in QBasic.
  ''' </summary>
  Public NotInheritable Class FixedString

    Private Sub New()
    End Sub

    ''' <summary>The initial value of a fixed-length string: <paramref name="length"/> characters CHR$(0).</summary>
    Public Shared Function Blank(length As Integer) As String
      Return New String(ChrW(0), length)
    End Function

    ''' <summary>The text as a fixed-length string: padded with spaces on the right, or cut short.</summary>
    Public Shared Function Fit(text As String, length As Integer) As String
      Dim s = If(text, "")
      If s.Length = length Then Return s
      If s.Length > length Then Return s.Substring(0, length)
      Return s.PadRight(length)
    End Function

    ''' <summary>Gives every element of an array of fixed-length strings its initial value.</summary>
    Public Shared Sub Fill(items() As String, length As Integer)
      Dim blank = FixedString.Blank(length)
      For i = 0 To items.Length - 1
        items(i) = blank
      Next
    End Sub

    ''' <summary>The same for a two-dimensional array.</summary>
    Public Shared Sub Fill(items(,) As String, length As Integer)
      Dim blank = FixedString.Blank(length)
      For i = 0 To items.GetLength(0) - 1
        For j = 0 To items.GetLength(1) - 1
          items(i, j) = blank
        Next
      Next
    End Sub

  End Class

End Namespace
