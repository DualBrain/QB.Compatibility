Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>
  ''' OPEN for serial ports (<c>OPEN("COM1:9600,N,8,1", OpenMode.Random, 1)</c>), together with everything in <see cref="File"/>:
  ''' <c>Imports QB.Serial</c> instead of <c>Imports QB.File</c>.  Using OPEN through it makes System.IO.Ports the serial port
  ''' provider, so only programs that use a serial port need the QB.Compatibility.Ports assembly.
  ''' </summary>
  Public NotInheritable Class Serial
    Inherits File

    Private Sub New()
    End Sub

    Shared Sub New()
      File.SerialPortFactory = Function(settings) New SerialConnection(settings)
    End Sub

    ''' <summary>OPEN file FOR mode AS #n (a name like "COM1:9600,N,8,1" is a serial port).</summary>
    Public Shared Shadows Sub OPEN(fileName$, accessMode As OpenMode, filenum%)
      File.OPEN(fileName, accessMode, filenum)
    End Sub

    ''' <summary>OPEN file FOR mode AS #n LEN = recordLength.</summary>
    Public Shared Shadows Sub OPEN(fileName$, accessMode As OpenMode, filenum%, recordLength%)
      File.OPEN(fileName, accessMode, filenum, recordLength)
    End Sub

  End Class

  ' A System.IO.Ports serial port, opened with the settings of an OPEN statement.
  Friend NotInheritable Class SerialConnection
    Implements ISerialPort

    Private ReadOnly m_port As IO.Ports.SerialPort

    Friend Sub New(settings As SerialSettings)
      Dim parity As IO.Ports.Parity
      Select Case settings.Parity
        Case "N"c : parity = IO.Ports.Parity.None
        Case "O"c : parity = IO.Ports.Parity.Odd
        Case "S"c : parity = IO.Ports.Parity.Space
        Case "M"c : parity = IO.Ports.Parity.Mark
        Case Else : parity = IO.Ports.Parity.Even
      End Select
      Dim stopBits As IO.Ports.StopBits
      Select Case settings.StopBits
        Case "1.5" : stopBits = IO.Ports.StopBits.OnePointFive
        Case "2" : stopBits = IO.Ports.StopBits.Two
        Case Else : stopBits = IO.Ports.StopBits.One
      End Select
      m_port = New IO.Ports.SerialPort(settings.PortName, settings.BaudRate, parity, settings.DataBits, stopBits) With {
        .ReadBufferSize = settings.ReadBufferSize, .WriteBufferSize = settings.WriteBufferSize,
        .ReadTimeout = IO.Ports.SerialPort.InfiniteTimeout, .WriteTimeout = IO.Ports.SerialPort.InfiniteTimeout}
      m_port.Open()
    End Sub

    Public ReadOnly Property BaseStream As IO.Stream Implements ISerialPort.BaseStream
      Get
        Return m_port.BaseStream
      End Get
    End Property

    Public ReadOnly Property BytesToRead As Integer Implements ISerialPort.BytesToRead
      Get
        Return m_port.BytesToRead
      End Get
    End Property

    Public ReadOnly Property BytesToWrite As Integer Implements ISerialPort.BytesToWrite
      Get
        Return m_port.BytesToWrite
      End Get
    End Property

    Public ReadOnly Property WriteBufferSize As Integer Implements ISerialPort.WriteBufferSize
      Get
        Return m_port.WriteBufferSize
      End Get
    End Property

    Public Sub Close() Implements ISerialPort.Close
      m_port.Dispose()
    End Sub

  End Class

End Namespace
