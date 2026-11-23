Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>
  ''' The record buffer of a random-access file split into fields, as FIELD does: <c>Dim f = FIELD(1, 20, 4)</c> (FIELD #1, 20 AS
  ''' name$, 4 AS score$).  Put text into a field with <see cref="LSET"/> or <see cref="RSET"/>, read it with <c>f(0)</c>; write the
  ''' buffer with PUT_FIELD and fill it with GET_FIELD.  (Variables can't be bound to a file buffer in VB, so the fields are
  ''' numbered instead of named.)
  ''' </summary>
  Public NotInheritable Class FieldRecord

    Friend ReadOnly Buffer() As Byte
    Private ReadOnly m_widths() As Integer
    Private ReadOnly m_offsets() As Integer

    Friend Sub New(recordLength As Integer, widths() As Integer)
      Buffer = New Byte(recordLength - 1) {}
      Array.Fill(Buffer, CByte(32))
      m_widths = widths
      ReDim m_offsets(widths.Length - 1)
      Dim offset = 0
      For i = 0 To widths.Length - 1
        m_offsets(i) = offset
        offset += widths(i)
      Next
    End Sub

    ''' <summary>The number of fields.</summary>
    Public ReadOnly Property Count As Integer
      Get
        Return m_widths.Length
      End Get
    End Property

    ''' <summary>The contents of a field (as many characters as its width).</summary>
    Default Public ReadOnly Property Item(index As Integer) As String
      Get
        Check(index)
        Dim chars(m_widths(index) - 1) As Char
        For i = 0 To chars.Length - 1
          chars(i) = Cp437.ToChar(Buffer(m_offsets(index) + i))
        Next
        Return New String(chars)
      End Get
    End Property

    Private Sub Check(index As Integer)
      If index < 0 OrElse index >= m_widths.Length Then Throw New ArgumentOutOfRangeException(NameOf(index))
    End Sub

    Private Sub Store(index As Integer, text As String)
      For i = 0 To m_widths(index) - 1
        Buffer(m_offsets(index) + i) = If(i < text.Length, Cp437.ToByte(text(i)), CByte(32))
      Next
    End Sub

    ''' <summary>LSET: the text at the left of the field (padded with spaces or cut short).</summary>
    Public Sub LSET(index As Integer, text As String)
      Check(index)
      Store(index, If(text, ""))
    End Sub

    ''' <summary>RSET: the text at the right of the field.</summary>
    Public Sub RSET(index As Integer, text As String)
      Check(index)
      Dim t = If(text, "")
      Dim width = m_widths(index)
      Store(index, If(t.Length >= width, t.Substring(0, width), t.PadLeft(width)))
    End Sub

  End Class

End Namespace
