Option Explicit On
Option Strict On
Option Infer On

Imports System.Diagnostics
Imports System.IO
Imports System.Text

Namespace Global.QB

  ''' <summary>Where LPRINT (and files opened on LPT1: to LPT3:) send their output: bytes in code page 437.</summary>
  Public Interface IPrinterDevice
    Sub Write(data() As Byte)
    ''' <summary>Sends anything held back (to the printer, for devices that collect a job).</summary>
    Sub Flush()
  End Interface

  ''' <summary>A printer for tests: it keeps the text printed.</summary>
  Public NotInheritable Class HeadlessPrinter
    Implements IPrinterDevice

    Private ReadOnly m_text As New StringBuilder

    Public Sub Write(data() As Byte) Implements IPrinterDevice.Write
      For Each b In data
        m_text.Append(Cp437.ToChar(b))
      Next
    End Sub

    Public Sub Flush() Implements IPrinterDevice.Flush
    End Sub

    ''' <summary>Everything printed so far.</summary>
    Public ReadOnly Property Text As String
      Get
        Return m_text.ToString
      End Get
    End Property

    Public Sub Clear()
      m_text.Clear()
    End Sub

  End Class

  ''' <summary>Appends everything printed to a text file (the default: LPT1.TXT in the current directory).</summary>
  Friend NotInheritable Class FilePrinter
    Implements IPrinterDevice

    Private ReadOnly m_path As String

    Public Sub New(path As String)
      m_path = path
    End Sub

    Public Sub Write(data() As Byte) Implements IPrinterDevice.Write
      Using fs As New FileStream(m_path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
        fs.Write(data, 0, data.Length)
      End Using
    End Sub

    Public Sub Flush() Implements IPrinterDevice.Flush
    End Sub

  End Class

  ''' <summary>
  ''' Collects what is printed and sends it as one job to the operating system's printer when flushed (when a printer
  ''' file is closed, at <see cref="Printer.Flush"/> and when the program ends): <c>lp</c> on Linux and macOS, PowerShell's
  ''' Out-Printer on Windows, or another command that reads the text from its standard input.
  ''' </summary>
  Friend NotInheritable Class SystemPrinter
    Implements IPrinterDevice

    Private ReadOnly m_command As String
    Private ReadOnly m_arguments As String
    Private ReadOnly m_job As New MemoryStream

    Public Sub New(command As String, arguments As String)
      m_command = command
      m_arguments = arguments
    End Sub

    Public Sub Write(data() As Byte) Implements IPrinterDevice.Write
      m_job.Write(data, 0, data.Length)
    End Sub

    Public Sub Flush() Implements IPrinterDevice.Flush
      If m_job.Length = 0 Then Return
      Dim text As New StringBuilder
      For Each b In m_job.ToArray
        text.Append(Cp437.ToChar(b))
      Next
      m_job.SetLength(0)

      Dim info As New ProcessStartInfo(m_command, m_arguments) With {
        .RedirectStandardInput = True, .UseShellExecute = False, .CreateNoWindow = True}
      Try
        Using p = Process.Start(info)
          p.StandardInput.Write(text.ToString)
          p.StandardInput.Close()
          If Not p.WaitForExit(30000) Then QBError.Raise(24, "Device timeout")
          If p.ExitCode <> 0 Then QBError.Raise(25, "Device fault")
        End Using
      Catch ex As System.ComponentModel.Win32Exception
        QBError.Raise(25, "Device fault")
      End Try
    End Sub

  End Class

  ''' <summary>The write-only stream behind a file opened on LPT1: to LPT3:.</summary>
  Friend NotInheritable Class PrinterStream
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
      Dim data(count - 1) As Byte
      Array.Copy(buffer, offset, data, 0, count)
      Printer.Send(data)
    End Sub

    Public Overrides Sub Flush()
      Printer.Flush()
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

  ''' <summary>
  ''' LPRINT, LPRINT USING, LPOS and WIDTH LPRINT.  Output goes to <see cref="Device"/>: by default the text file
  ''' LPT1.TXT in the current directory, so nothing is printed by accident.  <see cref="UseSystemPrinter"/> sends it
  ''' to the real printer.
  ''' </summary>
  Public NotInheritable Class Printer

    Private Sub New()
    End Sub

    Private Shared m_device As IPrinterDevice
    Private Shared m_column As Integer = 1
    Private Shared m_width As Integer = 80
    Private Shared m_exitHooked As Boolean

    ''' <summary>Where printed output goes.</summary>
    Public Shared Property Device As IPrinterDevice
      Get
        If m_device Is Nothing Then m_device = New FilePrinter("LPT1.TXT")
        Return m_device
      End Get
      Set(value As IPrinterDevice)
        m_device = value
        m_column = 1
      End Set
    End Property

    ''' <summary>
    ''' Sends printing to the system printer (collected as one job, sent when it is flushed or the program ends).  The
    ''' command receives the text on its standard input; by default <c>lp</c> (or PowerShell's Out-Printer on Windows).
    ''' </summary>
    Public Shared Sub UseSystemPrinter(Optional command As String = Nothing, Optional arguments As String = Nothing)
      If String.IsNullOrEmpty(command) Then
        If OperatingSystem.IsWindows() Then
          command = "powershell" : arguments = "-NoProfile -Command ""$input | Out-Printer"""
        Else
          command = "lp" : arguments = If(arguments, "")
        End If
      End If
      Device = New SystemPrinter(command, If(arguments, ""))
      If Not m_exitHooked Then
        m_exitHooked = True
        AddHandler AppDomain.CurrentDomain.ProcessExit, Sub(sender, e) Flush()
      End If
    End Sub

    ''' <summary>Sends a collected print job to the printer.</summary>
    Public Shared Sub Flush()
      If m_device IsNot Nothing Then m_device.Flush()
    End Sub

    Friend Shared Sub Send(data() As Byte)
      Device.Write(data)
    End Sub

    Private Shared Sub Emit(text As String)
      Dim bytes(text.Length - 1) As Byte
      For i = 0 To text.Length - 1
        bytes(i) = Cp437.ToByte(text(i))
      Next
      Send(bytes)
    End Sub

    ''' <summary>
    ''' LPRINT text: writes it and a CR LF (unless <paramref name="noCr"/>, for a trailing ; ).  TAB(n), Tab characters
    ''' (the comma's print zones) and the WIDTH LPRINT line width are honored.
    ''' </summary>
    Public Shared Sub LPRINT(Optional text As String = "", Optional noCr As Boolean = False)
      Emit(PrintFormat.Format(text, m_column, m_width, noCr))
    End Sub

    Public Shared Sub LPRINT_USING(template$, ParamArray values() As Object)
      LPRINT(PrintUsing.Format(template, values))
    End Sub

    ''' <summary>The number of characters sent to the printer since the last line end (LPT1 to LPT3 share one position).</summary>
    Public Shared Function LPOS(n%) As Integer
      If n < 0 OrElse n > 3 Then Throw New ArgumentException("Illegal function call")
      Return m_column - 1
    End Function

    ''' <summary>WIDTH LPRINT columns: the line width (1 to 255; 255 means never wrap).</summary>
    Public Shared Sub WIDTH_LPRINT(columns%)
      If columns < 1 OrElse columns > 255 Then Throw New ArgumentException("Illegal function call")
      m_width = columns
    End Sub

    ' For a file opened on LPT1: (the file layer lays the text out with the printer's own position and width).
    Friend Shared Sub Print(text As String, noCr As Boolean)
      LPRINT(text, noCr)
    End Sub

  End Class

End Namespace
