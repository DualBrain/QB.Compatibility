Option Explicit On
Option Strict On
Option Infer On

Imports System.IO

Namespace Global.QB

  ''' <summary>The write-only stream behind a file opened on the screen: OPEN "SCRN:" (or "CON", "CONS:") FOR OUTPUT AS #1.</summary>
  Friend NotInheritable Class ScreenStream
    Inherits Stream

    Public Overrides ReadOnly Property CanRead As Boolean = False
    Public Overrides ReadOnly Property CanSeek As Boolean = False
    Public Overrides ReadOnly Property CanWrite As Boolean = True

    Public Overrides ReadOnly Property Length As Long
      Get
        Throw New NotSupportedException
      End Get
    End Property

    Public Overrides Property Position As Long
      Get
        Throw New NotSupportedException
      End Get
      Set(value As Long)
        Throw New NotSupportedException
      End Set
    End Property

    Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
      Dim chars(count - 1) As Char
      For i = 0 To count - 1
        chars(i) = Cp437.ToChar(buffer(offset + i))
      Next
      Video.PRINT(New String(chars), True)
    End Sub

    Public Overrides Sub Flush()
    End Sub

    Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
      Throw New NotSupportedException
    End Function

    Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
      Throw New NotSupportedException
    End Function

    Public Overrides Sub SetLength(value As Long)
      Throw New NotSupportedException
    End Sub

  End Class

  ''' <summary>The read-only stream behind OPEN "KYBD:" FOR INPUT AS #1: each read waits for a key press.</summary>
  Friend NotInheritable Class KeyboardStream
    Inherits Stream

    Private m_pending As String = ""

    Public Overrides ReadOnly Property CanRead As Boolean = True
    Public Overrides ReadOnly Property CanSeek As Boolean = False
    Public Overrides ReadOnly Property CanWrite As Boolean = False

    Public Overrides ReadOnly Property Length As Long
      Get
        Throw New NotSupportedException
      End Get
    End Property

    Public Overrides Property Position As Long
      Get
        Throw New NotSupportedException
      End Get
      Set(value As Long)
        Throw New NotSupportedException
      End Set
    End Property

    Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
      If count <= 0 Then Return 0
      Do While m_pending.Length = 0
        m_pending = Video.INKEY()
        If m_pending.Length = 0 Then Threading.Thread.Sleep(5)
      Loop
      Dim n = Math.Min(count, m_pending.Length)
      For i = 0 To n - 1
        buffer(offset + i) = Cp437.ToByte(m_pending(i))
      Next
      m_pending = m_pending.Substring(n)
      Return n
    End Function

    Public Overrides Sub Flush()
    End Sub

    Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
      Throw New NotSupportedException
    End Sub

    Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
      Throw New NotSupportedException
    End Function

    Public Overrides Sub SetLength(value As Long)
      Throw New NotSupportedException
    End Sub

  End Class

End Namespace
