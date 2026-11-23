Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>The settings of OPEN "COM1:9600,N,8,1": what a serial port has to be opened with.</summary>
  Public NotInheritable Class SerialSettings
    ''' <summary>The operating system's name for the port (COM3, /dev/ttyS0, /dev/ttyUSB0...).</summary>
    Public Property PortName As String = ""
    Public Property BaudRate As Integer = 300
    ''' <summary>N (none), E (even), O (odd), S (space) or M (mark).</summary>
    Public Property Parity As Char = "E"c
    Public Property DataBits As Integer = 7
    ''' <summary>"1", "1.5" or "2".</summary>
    Public Property StopBits As String = "1"
    Public Property ReadBufferSize As Integer = 4096
    Public Property WriteBufferSize As Integer = 2048
  End Class

  ''' <summary>An open serial port, as OPEN, EOF, LOF and LOC use it (provided by QB.Compatibility.Ports).</summary>
  Public Interface ISerialPort
    ReadOnly Property BaseStream As IO.Stream
    ReadOnly Property BytesToRead As Integer
    ReadOnly Property BytesToWrite As Integer
    ReadOnly Property WriteBufferSize As Integer
    Sub Close()
  End Interface

End Namespace
