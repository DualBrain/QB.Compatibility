Option Explicit On
Option Strict On
Option Infer On

Imports System.Globalization
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic

Namespace Global.QB

  ''' <summary>
  ''' Sequential file I/O: OPEN, CLOSE, EOF, LOF, LOC, SEEK, FREEFILE, KILL, NAME, MKDIR, RMDIR and CHDIR.
  ''' (PRINT #, WRITE #, INPUT #, LINE INPUT # and INPUT$ are overloads of PRINT, WRITE, INPUT, LINE_INPUT
  ''' and INPUT$ in Video and Console, because VB can't import one name from two classes.)
  ''' Files hold code page 437 text with CR LF line ends, as QBasic wrote them.  Random files (OPEN ... LEN = n) and
  ''' binary files are read and written with GET and PUT, which are overloads in Video.)
  ''' </summary>
  Public NotInheritable Class File

    Private Sub New()
    End Sub

    Friend NotInheritable Class FileEntry

      Friend ReadOnly Mode As OpenMode
      Friend ReadOnly Stream As Stream
      Friend ReadOnly Port As IO.Ports.SerialPort      ' Serial ports (OPEN "COM1:..."); Nothing for files.
      Friend ReadOnly ComNumber As Integer
      Friend Column As Integer = 1       ' Print column, for TAB and print zones.
      Friend Width As Integer = 0        ' Line width for WIDTH #n (0 = no wrapping).
      Friend IsPrinter As Boolean        ' Opened on LPT1: to LPT3:.
      Friend ReadOnly Property IsKeyboard As Boolean  ' Opened on KYBD: (reading ahead past a CR would wait for another key).
        Get
          Return TypeOf Stream Is KeyboardStream
        End Get
      End Property
      Friend RecordBuffer() As Byte      ' The FIELD buffer of a random file (shared by all its FIELD statements).
      Friend RecordLength As Integer = 128
      Friend NextRecord As Long = 1      ' Random files: the record the next GET or PUT uses.
      Friend LastRecord As Long = 0      ' Random files: the record last read or written.
      Private m_peek As Integer = -2     ' -2 = nothing read ahead, -1 = end of file.

      Friend Sub New(stream As Stream, mode As OpenMode)
        Me.Stream = stream
        Me.Mode = mode
      End Sub

      Friend Sub New(port As IO.Ports.SerialPort, number As Integer, mode As OpenMode)
        Me.Port = port
        Me.Stream = port.BaseStream
        Me.ComNumber = number
        Me.Mode = mode
      End Sub

      Friend Function ReadByte() As Integer
        If m_peek <> -2 Then
          Dim b = m_peek
          If b <> -1 Then m_peek = -2
          Return b
        End If
        Return Stream.ReadByte()
      End Function

      Friend Function PeekByte() As Integer
        If m_peek = -2 Then m_peek = Stream.ReadByte()
        Return m_peek
      End Function

      Friend Property Position As Long
        Get
          If Port IsNot Nothing Then Return 0
          Return Stream.Position - If(m_peek >= 0, 1, 0)
        End Get
        Set(value As Long)
          If Port IsNot Nothing Then QBError.Raise(54, "Bad file mode")
          Stream.Position = value
          m_peek = -2
        End Set
      End Property

      Friend Sub Write(text As String)
        Dim bytes(text.Length - 1) As Byte
        For i = 0 To text.Length - 1
          bytes(i) = Cp437.ToByte(text(i))
        Next
        Stream.Write(bytes, 0, bytes.Length)
      End Sub

    End Class

    Friend Shared ReadOnly m_files As New Dictionary(Of Integer, FileEntry)

    Private Shared Function GetEntry(filenum As Integer) As FileEntry
      Dim e As FileEntry = Nothing
      If Not m_files.TryGetValue(filenum, e) Then QBError.Raise(52, "Bad file name or number")
      Return e
    End Function

    Private Shared Function ReadableEntry(filenum As Integer) As FileEntry
      Dim e = GetEntry(filenum)
      If e.Mode = OpenMode.Output OrElse e.Mode = OpenMode.Append Then QBError.Raise(54, "Bad file mode")
      Return e
    End Function

    Private Shared Function WritableEntry(filenum As Integer) As FileEntry
      Dim e = GetEntry(filenum)
      If e.Mode = OpenMode.Input Then QBError.Raise(54, "Bad file mode")
      Return e
    End Function

#Region "OPEN, CLOSE and file information"

    Public Shared Sub OPEN(file$, accessMode As OpenMode, filenum%)

      If filenum < 1 OrElse filenum > 255 Then QBError.Raise(52, "Bad file name or number")
      If m_files.ContainsKey(filenum) Then QBError.Raise(55, "File already open")

      ' The screen, keyboard and null devices (REMLINE.BAS opens "CON" for output).
      Select Case file.ToUpperInvariant()
        Case "CON", "CON:", "SCRN:", "CONS:"
          If accessMode = OpenMode.Input Then QBError.Raise(54, "Bad file mode")
          m_files.Add(filenum, New FileEntry(New ScreenStream, accessMode))
          Return
        Case "KYBD:"
          If accessMode = OpenMode.Output OrElse accessMode = OpenMode.Append Then QBError.Raise(54, "Bad file mode")
          m_files.Add(filenum, New FileEntry(New KeyboardStream, OpenMode.Input))
          Return
        Case "NUL", "NUL:"
          m_files.Add(filenum, New FileEntry(Stream.Null, accessMode))
          Return
      End Select

      If IsLptSpec(file) Then
        If accessMode = OpenMode.Input Then QBError.Raise(54, "Bad file mode")
        Dim printerEntry As New FileEntry(New PrinterStream, accessMode) With {.IsPrinter = True}
        m_files.Add(filenum, printerEntry)
        Return
      End If

      If IsComSpec(file) Then
        Dim number As Integer
        Dim port = OpenPort(file, number)
        If number = 0 Then ' A device path such as /dev/ttyUSB0 counts as the lowest COM number not in use (for ON COM).
          number = 1
          Do While m_files.Values.Any(Function(f) f.ComNumber = number)
            number += 1
          Loop
        End If
        m_files.Add(filenum, New FileEntry(port, number, accessMode))
        Return
      End If

      Dim fileStream As FileStream
      Select Case accessMode
        Case OpenMode.Input
          fileStream = New FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Case OpenMode.Output
          fileStream = New FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read)
        Case OpenMode.Append
          fileStream = New FileStream(file, FileMode.Append, FileAccess.Write, FileShare.Read)
        Case Else ' Binary, Random
          fileStream = New FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
      End Select

      m_files.Add(filenum, New FileEntry(fileStream, accessMode))

    End Sub

    ''' <summary>OPEN with a record length (used for random files).</summary>
    Public Shared Sub OPEN(file$, accessMode As OpenMode, filenum%, recordLength%)
      If recordLength < 1 Then QBError.Raise(59, "Bad record length")
      OPEN(file, accessMode, filenum)
      m_files(filenum).RecordLength = recordLength
    End Sub

    ''' <summary>Closes the given files, or all open files when none are given.</summary>
    Public Shared Sub CLOSE(ParamArray filenums%())
      Dim numbers = If(filenums.Length = 0, m_files.Keys.ToArray, filenums)
      For Each n In numbers
        Dim e As FileEntry = Nothing
        If m_files.TryGetValue(n, e) Then
          If e.Port IsNot Nothing Then e.Port.Dispose() Else e.Stream.Dispose()
          If e.IsPrinter Then Printer.Flush()
          m_files.Remove(n)
        End If
      Next
    End Sub

    ''' <summary>True when the next read would be past the end of the file.</summary>
    Public Shared Function EOF(filenum%) As Boolean
      Dim e = ReadableEntry(filenum)
      If e.Port IsNot Nothing Then Return e.Port.BytesToRead = 0 ' Nothing waiting in the receive buffer.
      If e.Mode = OpenMode.Input Then
        Dim b = e.PeekByte()
        Return b = -1 OrElse b = 26 ' Ctrl+Z also ends a text file.
      End If
      Return e.Position >= e.Stream.Length
    End Function

    ''' <summary>The size of a file in bytes; for a serial port, the free space in its transmit buffer.</summary>
    Public Shared Function LOF(filenum%) As Long
      Dim e = GetEntry(filenum)
      If e.Port IsNot Nothing Then Return e.Port.WriteBufferSize - e.Port.BytesToWrite
      Return e.Stream.Length
    End Function

    ''' <summary>Random files: the number of the last record read or written; binary files: the position of the last byte; sequential files: the position divided by 128.</summary>
    Public Shared Function LOC(filenum%) As Long
      Dim e = GetEntry(filenum)
      If e.Port IsNot Nothing Then Return e.Port.BytesToRead ' Serial ports: the bytes waiting to be read.
      Select Case e.Mode
        Case OpenMode.Random : Return e.LastRecord
        Case OpenMode.Binary : Return e.Position
        Case Else : Return e.Position \ 128
      End Select
    End Function

    ''' <summary>The position of the next read or write: a record number for random files, else a byte position (the first byte is 1).</summary>
    Public Shared Function SEEK(filenum%) As Long
      Dim e = GetEntry(filenum)
      Return If(e.Mode = OpenMode.Random, e.NextRecord, e.Position + 1)
    End Function

    ''' <summary>Sets the position of the next read or write: a record number for random files, else a byte position.</summary>
    Public Shared Sub SEEK(filenum%, position As Long)
      If position < 1 Then Throw New ArgumentOutOfRangeException(NameOf(position))
      Dim e = GetEntry(filenum)
      If e.Mode = OpenMode.Random Then e.NextRecord = position Else e.Position = position - 1
    End Sub

    ''' <summary>
    ''' The length of a value in bytes, as stored in a record file: a String's length, or the size of a
    ''' record structure (for <c>OPEN ... FOR RANDOM ... LEN = LEN(record)</c>).
    ''' </summary>
    Public Shared Function LEN(Of T)(value As T) As Integer
      Return Records.SizeOf(value)
    End Function

    ''' <summary>LOCK #n [, record]: locks part of a file (nothing else shares the file here, so nothing happens).</summary>
    Public Shared Sub LOCK(filenum%, Optional first As Long = -1, Optional last As Long = -1)
      GetEntry(filenum)
    End Sub

    Public Shared Sub UNLOCK(filenum%, Optional first As Long = -1, Optional last As Long = -1)
      GetEntry(filenum)
    End Sub

    ''' <summary>FILEATTR(n, 1): the mode a file was opened in (1 input, 2 output, 4 random, 8 append, 32 binary); FILEATTR(n, 2): a handle number.</summary>
    Public Shared Function FILEATTR(filenum%, attribute%) As Integer
      Dim e = GetEntry(filenum)
      Select Case attribute
        Case 1
          Select Case e.Mode
            Case OpenMode.Input : Return 1
            Case OpenMode.Output : Return 2
            Case OpenMode.Random : Return 4
            Case OpenMode.Append : Return 8
            Case Else : Return 32
          End Select
        Case 2 : Return filenum + 4
        Case Else : Throw New ArgumentException("Illegal function call")
      End Select
    End Function

    Public Shared Function FREEFILE() As Integer
      For index = 1 To 255
        If Not m_files.ContainsKey(index) Then Return index
      Next
      QBError.Raise(67, "Too many files")
    End Function

#End Region

    ''' <summary>True for the printer devices LPT1: to LPT3: (optionally followed by printer options, which are ignored).</summary>
    Private Shared Function IsLptSpec(file As String) As Boolean
      Return file.Length >= 5 AndAlso file.StartsWith("LPT", StringComparison.OrdinalIgnoreCase) AndAlso Char.IsDigit(file(3)) AndAlso file(4) = ":"c
    End Function

    ''' <summary>WIDTH #n, columns: the line width of an open file or device (1 to 255; 255 means never wrap).</summary>
    Friend Shared Sub SetWidth(filenum%, columns%)
      If columns < 1 OrElse columns > 255 Then Throw New ArgumentException("Illegal function call")
      Dim e = GetEntry(filenum)
      If e.IsPrinter Then Printer.WIDTH_LPRINT(columns) Else e.Width = If(columns = 255, 0, columns)
    End Sub

#Region "Serial ports: OPEN COM"

    ''' <summary>True for an OPEN file name like "COM1:9600,N,8,1" (or a device path such as "/dev/ttyUSB0:9600,N,8,1").</summary>
    Private Shared Function IsComSpec(file As String) As Boolean
      If file.StartsWith("COM", StringComparison.OrdinalIgnoreCase) AndAlso file.Length > 3 AndAlso Char.IsDigit(file(3)) Then Return True
      Return file.StartsWith("/dev/", StringComparison.Ordinal) AndAlso file.Contains(":"c)
    End Function

    ' COM1 is COM1 on Windows and /dev/ttyS0 elsewhere.
    Private Shared Function PortName(name As String, ByRef number As Integer) As String
      number = 0
      If name.StartsWith("COM", StringComparison.OrdinalIgnoreCase) AndAlso Integer.TryParse(name.Substring(3), number) Then
        Return If(OperatingSystem.IsWindows(), "COM" & number, "/dev/ttyS" & (number - 1))
      End If
      Return name
    End Function

    Private Shared Function OpenPort(spec As String, ByRef number As Integer) As IO.Ports.SerialPort

      Dim colon = spec.IndexOf(":"c)
      Dim portName = File.PortName(spec.Substring(0, colon), number)
      Dim options = spec.Substring(colon + 1).Split(","c).Select(Function(o) o.Trim()).ToList()

      ' QBasic's defaults: 300 baud, even parity, 7 data bits, 1 stop bit.
      Dim baud = 300, parity = IO.Ports.Parity.Even, data = 7, stopBits = IO.Ports.StopBits.One
      Dim readBuffer = 4096, writeBuffer = 2048

      If options.Count > 0 AndAlso options(0) <> "" Then baud = Integer.Parse(options(0), CultureInfo.InvariantCulture)
      If options.Count > 1 AndAlso options(1) <> "" Then
        Select Case options(1).ToUpperInvariant()
          Case "N" : parity = IO.Ports.Parity.None
          Case "E", "PE" : parity = IO.Ports.Parity.Even
          Case "O" : parity = IO.Ports.Parity.Odd
          Case "S" : parity = IO.Ports.Parity.Space
          Case "M" : parity = IO.Ports.Parity.Mark
          Case Else : QBError.Raise(64, "Bad file name")
        End Select
      End If
      If options.Count > 2 AndAlso options(2) <> "" Then data = Integer.Parse(options(2), CultureInfo.InvariantCulture)
      If options.Count > 3 AndAlso options(3) <> "" Then
        Select Case options(3)
          Case "1" : stopBits = IO.Ports.StopBits.One
          Case "1.5" : stopBits = IO.Ports.StopBits.OnePointFive
          Case "2" : stopBits = IO.Ports.StopBits.Two
          Case Else : QBError.Raise(64, "Bad file name")
        End Select
      ElseIf baud <= 110 Then
        stopBits = IO.Ports.StopBits.Two
      End If

      ' The less-often-used options: buffer sizes are used; the handshake and timeout options are accepted and ignored.
      For i = 4 To options.Count - 1
        Dim o = options(i).ToUpperInvariant()
        If o.StartsWith("RB", StringComparison.Ordinal) AndAlso o.Length > 2 Then readBuffer = Math.Max(1, Integer.Parse(o.Substring(2), CultureInfo.InvariantCulture))
        If o.StartsWith("TB", StringComparison.Ordinal) AndAlso o.Length > 2 Then writeBuffer = Math.Max(1, Integer.Parse(o.Substring(2), CultureInfo.InvariantCulture))
      Next

      Dim port As New IO.Ports.SerialPort(portName, baud, parity, data, stopBits) With {
        .ReadBufferSize = readBuffer, .WriteBufferSize = writeBuffer,
        .ReadTimeout = IO.Ports.SerialPort.InfiniteTimeout, .WriteTimeout = IO.Ports.SerialPort.InfiniteTimeout}
      port.Open()
      Return port

    End Function

    ''' <summary>The number of bytes waiting in the receive buffer of COMn (0 if it isn't open).</summary>
    Friend Shared Function ComBytesWaiting(number As Integer) As Integer
      For Each e In m_files.Values
        If e.ComNumber = number AndAlso e.Port IsNot Nothing Then Return e.Port.BytesToRead
      Next
      Return 0
    End Function

#End Region

#Region "Writing: PRINT # and WRITE #"

    ''' <summary>Writes text and a CR LF (unless <paramref name="noCr"/>), like PRINT #; TAB(n) and Tab characters work.</summary>
    Friend Shared Sub PRINT(filenum%, text$, Optional noCr As Boolean = False)
      Dim e = WritableEntry(filenum)
      If e.IsPrinter Then
        Printer.Print(text, noCr) ' (The printer keeps its own position and width.)
      Else
        e.Write(PrintFormat.Format(text, e.Column, e.Width, noCr))
      End If
    End Sub

    Friend Shared Sub PRINT_USING(filenum%, template$, ParamArray values() As Object)
      PRINT(filenum, PrintUsing.Format(template, values))
    End Sub

    ''' <summary>The text WRITE produces: strings in quotes, numbers plain, separated by commas.</summary>
    Friend Shared Function FormatWrite(values() As Object) As String
      Dim parts As New List(Of String)
      For Each v In values
        Select Case True
          Case v Is Nothing : parts.Add("")
          Case TypeOf v Is String : parts.Add(ControlChars.Quote & DirectCast(v, String) & ControlChars.Quote)
          Case TypeOf v Is Single : parts.Add(Plain(DirectCast(v, Single).ToString("R", CultureInfo.InvariantCulture)))
          Case TypeOf v Is Double : parts.Add(Plain(DirectCast(v, Double).ToString("R", CultureInfo.InvariantCulture)))
          Case TypeOf v Is Decimal : parts.Add(Plain(DirectCast(v, Decimal).ToString(CultureInfo.InvariantCulture)))
          Case Else : parts.Add(Convert.ToString(v, CultureInfo.InvariantCulture))
        End Select
      Next
      Return String.Join(",", parts)
    End Function

    ' QBasic writes 0.5 as .5
    Private Shared Function Plain(s As String) As String
      If s.StartsWith("0.") Then Return s.Substring(1)
      If s.StartsWith("-0.") Then Return "-" & s.Substring(2)
      Return s
    End Function

    ''' <summary>Writes values to a file in a form INPUT # can read back, then a CR LF.</summary>
    Friend Shared Sub WRITE(filenum%, ParamArray values() As Object)
      PRINT(filenum, FormatWrite(values))
    End Sub

#End Region

#Region "Reading: INPUT #, LINE INPUT # and INPUT$"

    ''' <summary>Reads a line (up to a CR LF, CR or LF).</summary>
    Friend Shared Sub LINE_INPUT(filenum%, ByRef value$)
      Dim e = ReadableEntry(filenum)
      If e.PeekByte() = -1 Then QBError.Raise(62, "Input past end of file")
      Dim sb As New StringBuilder
      Do
        Dim b = e.ReadByte()
        If b = -1 OrElse b = 10 Then Exit Do
        If b = 13 Then
          If Not e.IsKeyboard AndAlso e.PeekByte() = 10 Then e.ReadByte()
          Exit Do
        End If
        sb.Append(Cp437.ToChar(CByte(b)))
      Loop
      value = sb.ToString
    End Sub

    ''' <summary>Reads n characters from a file.</summary>
    Friend Shared Function INPUT$(n%, filenum%)
      Dim e = ReadableEntry(filenum)
      Dim sb As New StringBuilder
      For i = 1 To n
        Dim b = e.ReadByte()
        If b = -1 Then QBError.Raise(62, "Input past end of file")
        sb.Append(Cp437.ToChar(CByte(b)))
      Next
      Return sb.ToString
    End Function

    ' Reads one INPUT # field: leading spaces skipped; quoted, or up to a comma / line end.
    Private Shared Function ReadField(e As FileEntry) As String

      Dim b = e.PeekByte()
      Do While b = 32 OrElse b = 9
        e.ReadByte() : b = e.PeekByte()
      Loop
      If b = -1 Then QBError.Raise(62, "Input past end of file")

      Dim sb As New StringBuilder
      If b = 34 Then
        e.ReadByte()
        Do
          b = e.ReadByte()
          If b = -1 OrElse b = 34 Then Exit Do
          sb.Append(Cp437.ToChar(CByte(b)))
        Loop
        b = e.PeekByte()
        Do While b = 32 OrElse b = 9
          e.ReadByte() : b = e.PeekByte()
        Loop
      Else
        Do While b <> -1 AndAlso b <> 44 AndAlso b <> 13 AndAlso b <> 10
          sb.Append(Cp437.ToChar(CByte(e.ReadByte())))
          b = e.PeekByte()
        Loop
      End If

      ' The delimiter belongs to the field.
      If b = 44 Then
        e.ReadByte()
      ElseIf b = 13 Then
        e.ReadByte()
        If Not e.IsKeyboard AndAlso e.PeekByte() = 10 Then e.ReadByte()
      ElseIf b = 10 Then
        e.ReadByte()
      End If

      Return sb.ToString

    End Function

    Private Shared Function ConvertField(Of T)(field As String) As T
      If GetType(T) Is GetType(String) Then Return DirectCast(DirectCast(field, Object), T)
      Dim d As Double
      If Not Double.TryParse(field.Trim, NumberStyles.Float, CultureInfo.InvariantCulture, d) Then d = 0
      Return DirectCast(System.Convert.ChangeType(d, GetType(T), CultureInfo.InvariantCulture), T)
    End Function

    ' A field converted strictly: False when it isn't a valid number (or doesn't fit) for a numeric variable.
    Friend Shared Function TryConvertField(Of T)(field As String, ByRef value As T) As Boolean
      If GetType(T) Is GetType(String) Then
        value = DirectCast(DirectCast(field, Object), T)
        Return True
      End If
      Dim d As Double
      If Not Double.TryParse(field.Trim, NumberStyles.Float, CultureInfo.InvariantCulture, d) Then Return False
      Try
        value = DirectCast(System.Convert.ChangeType(d, GetType(T), CultureInfo.InvariantCulture), T)
        Return True
      Catch ex As OverflowException
        Return False
      End Try
    End Function

    ''' <summary>Reads a value (String or any numeric type) from a file, as INPUT # does.</summary>
    Friend Shared Sub INPUT(Of T)(filenum%, ByRef value As T)
      value = ConvertField(Of T)(ReadField(ReadableEntry(filenum)))
    End Sub

    Friend Shared Sub INPUT(Of T1, T2)(filenum%, ByRef value1 As T1, ByRef value2 As T2)
      Dim e = ReadableEntry(filenum)
      value1 = ConvertField(Of T1)(ReadField(e))
      value2 = ConvertField(Of T2)(ReadField(e))
    End Sub

    Friend Shared Sub INPUT(Of T1, T2, T3)(filenum%, ByRef value1 As T1, ByRef value2 As T2, ByRef value3 As T3)
      Dim e = ReadableEntry(filenum)
      value1 = ConvertField(Of T1)(ReadField(e))
      value2 = ConvertField(Of T2)(ReadField(e))
      value3 = ConvertField(Of T3)(ReadField(e))
    End Sub

    Friend Shared Sub INPUT(Of T1, T2, T3, T4)(filenum%, ByRef value1 As T1, ByRef value2 As T2, ByRef value3 As T3, ByRef value4 As T4)
      Dim e = ReadableEntry(filenum)
      value1 = ConvertField(Of T1)(ReadField(e))
      value2 = ConvertField(Of T2)(ReadField(e))
      value3 = ConvertField(Of T3)(ReadField(e))
      value4 = ConvertField(Of T4)(ReadField(e))
    End Sub

#End Region

#Region "Random and binary access: GET and PUT"

    Private Shared Function RecordEntry(filenum As Integer) As FileEntry
      Dim e = GetEntry(filenum)
      If e.Mode <> OpenMode.Random AndAlso e.Mode <> OpenMode.Binary Then QBError.Raise(54, "Bad file mode")
      Return e
    End Function

    ' Where a GET or PUT starts, and (random files) which record it is.
    Private Shared Function StartOf(e As FileEntry, hasPosition As Boolean, position As Long, ByRef record As Long) As Long
      If hasPosition AndAlso position < 1 Then QBError.Raise(63, "Bad record number")
      If e.Mode = OpenMode.Random Then
        record = If(hasPosition, position, e.NextRecord)
        Return (record - 1) * e.RecordLength
      End If
      Return If(hasPosition, position - 1, e.Position)
    End Function

    Friend Shared Sub GetRecord(Of T)(filenum%, hasPosition As Boolean, position As Long, ByRef value As T)

      Dim e = RecordEntry(filenum)
      Dim size = Records.SizeOf(value)
      If e.Mode = OpenMode.Random AndAlso size > e.RecordLength Then QBError.Raise(59, "Bad record length")

      Dim record As Long
      Dim start = StartOf(e, hasPosition, position, record)
      If start >= e.Stream.Length AndAlso size > 0 Then
        ' A random file's GET past the end isn't an error (the variable keeps what it had); a binary file's is.
        If e.Mode <> OpenMode.Random Then QBError.Raise(62, "Input past end of file")
        e.LastRecord = record : e.NextRecord = record + 1
        Return
      End If

      e.Position = start
      Dim buf(size - 1) As Byte
      Dim got = 0
      Do While got < size
        Dim n = e.Stream.Read(buf, got, size - got)
        If n = 0 Then Exit Do
        got += n
      Loop
      If e.Mode = OpenMode.Binary AndAlso got < size Then QBError.Raise(62, "Input past end of file")

      value = Records.FromBytes(buf, value)

      If e.Mode = OpenMode.Random Then
        e.LastRecord = record : e.NextRecord = record + 1
      End If

    End Sub

    Friend Shared Sub PutRecord(filenum%, hasPosition As Boolean, position As Long, value As Object)

      Dim e = RecordEntry(filenum)
      Dim bytes = Records.ToBytes(value)
      If e.Mode = OpenMode.Random AndAlso bytes.Length > e.RecordLength Then QBError.Raise(59, "Bad record length")

      Dim record As Long
      Dim start = StartOf(e, hasPosition, position, record)

      e.Position = start
      e.Stream.Write(bytes, 0, bytes.Length)

      If e.Mode = OpenMode.Random Then
        ' A record always takes its whole length in the file.
        If e.Stream.Length < start + e.RecordLength Then e.Stream.SetLength(start + e.RecordLength)
        e.LastRecord = record : e.NextRecord = record + 1
      End If

    End Sub

#End Region

#Region "FIELD, GET_FIELD and PUT_FIELD"

    ''' <summary>FIELD #n, w1 AS a$, w2 AS b$ ...: splits the record buffer of a random file into fields (see <see cref="FieldRecord"/>).</summary>
    Public Shared Function FIELD(filenum%, ParamArray widths%()) As FieldRecord
      Dim e = GetEntry(filenum)
      If e.Mode <> OpenMode.Random Then QBError.Raise(54, "Bad file mode")
      If widths.Length = 0 OrElse widths.Any(Function(w) w < 1) Then Throw New ArgumentException("Illegal function call")
      If widths.Sum() > e.RecordLength Then QBError.Raise(50, "FIELD overflow")
      If e.RecordBuffer Is Nothing Then
        e.RecordBuffer = New Byte(e.RecordLength - 1) {}
        Array.Fill(e.RecordBuffer, CByte(32))
      End If
      Return New FieldRecord(e.RecordBuffer, widths)
    End Function

    Private Shared Function FieldEntry(filenum As Integer) As FileEntry
      Dim e = GetEntry(filenum)
      If e.Mode <> OpenMode.Random Then QBError.Raise(54, "Bad file mode")
      If e.RecordBuffer Is Nothing Then QBError.Raise(54, "Bad file mode (no FIELD for this file)")
      Return e
    End Function

    ''' <summary>GET #n, record: reads a record into the FIELD buffer.</summary>
    Public Shared Sub GET_FIELD(filenum%, record As Long)
      Dim e = FieldEntry(filenum)
      If record < 1 Then QBError.Raise(63, "Bad record number")
      Dim start = (record - 1) * e.RecordLength
      If start >= e.Stream.Length Then
        e.LastRecord = record : e.NextRecord = record + 1   ' (past the end: the buffer keeps what it had)
        Return
      End If
      e.Position = start
      Dim got = 0
      Do While got < e.RecordLength
        Dim n = e.Stream.Read(e.RecordBuffer, got, e.RecordLength - got)
        If n = 0 Then Exit Do
        got += n
      Loop
      e.LastRecord = record : e.NextRecord = record + 1
    End Sub

    ''' <summary>GET #n: reads the next record into the FIELD buffer.</summary>
    Public Shared Sub GET_FIELD(filenum%)
      GET_FIELD(filenum, GetEntry(filenum).NextRecord)
    End Sub

    ''' <summary>PUT #n, record: writes the FIELD buffer as a record.</summary>
    Public Shared Sub PUT_FIELD(filenum%, record As Long)
      Dim e = FieldEntry(filenum)
      If record < 1 Then QBError.Raise(63, "Bad record number")
      e.Position = (record - 1) * e.RecordLength
      e.Stream.Write(e.RecordBuffer, 0, e.RecordLength)
      e.LastRecord = record : e.NextRecord = record + 1
    End Sub

    ''' <summary>PUT #n: writes the FIELD buffer as the next record.</summary>
    Public Shared Sub PUT_FIELD(filenum%)
      PUT_FIELD(filenum, GetEntry(filenum).NextRecord)
    End Sub

#End Region

#Region "Files and directories"

    ''' <summary>Deletes the file(s) matching a spec (? and * allowed).</summary>
    Public Shared Sub KILL(filespec$)
      Dim dir = Path.GetDirectoryName(filespec)
      If String.IsNullOrEmpty(dir) Then dir = "."
      Dim matches = Directory.GetFiles(dir, Path.GetFileName(filespec))
      If matches.Length = 0 Then QBError.Raise(53, "File not found")
      For Each f In matches
        IO.File.Delete(f)
      Next
    End Sub

    Public Shared Sub NAME(oldfilespec$, newfilespec$)
      If IO.File.Exists(newfilespec) Then QBError.Raise(58, "File already exists")
      IO.File.Move(oldfilespec, newfilespec)
    End Sub

    Public Shared Sub MKDIR(path$)
      Directory.CreateDirectory(path)
    End Sub

    Public Shared Sub RMDIR(path$)
      Directory.Delete(path)
    End Sub

    Public Shared Sub CHDIR(path$)
      Directory.SetCurrentDirectory(path)
    End Sub

#End Region

  End Class

End Namespace
