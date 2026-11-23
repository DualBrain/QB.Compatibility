Option Explicit On
Option Strict On
Option Infer On

Imports System.Diagnostics

Namespace Global.QB

  Partial Public NotInheritable Class Development

    ' DOS and program management

    ''' <summary>
    ''' RUN program: the converted program of that name (next to this one) replaces the running one, as far as a separate process can:
    ''' it runs to its end and then this one ends too.  RUN does not pass variables.
    ''' </summary>
    Public Shared Sub RUN(path$)
      CHAIN(path$)
    End Sub

    ''' <summary>
    ''' CHAIN program: as RUN, but passes the values of the COMMON variables (in the order they were declared) to the program, which
    ''' receives them in its own COMMON statement (see COMMON_VALUE).
    ''' </summary>
    Public Shared Sub CHAIN(path$, ParamArray common() As Object)
      Dim stem = IO.Path.GetFileNameWithoutExtension(path$)
      Dim dir = AppContext.BaseDirectory
      Dim exe As String = Nothing, dll As String = Nothing
      For Each f In IO.Directory.EnumerateFiles(dir)
        Dim name = IO.Path.GetFileName(f)
        If name.Equals(stem, StringComparison.OrdinalIgnoreCase) OrElse name.Equals(stem & ".exe", StringComparison.OrdinalIgnoreCase) Then exe = f
        If name.Equals(stem & ".dll", StringComparison.OrdinalIgnoreCase) Then dll = f
      Next
      If exe Is Nothing AndAlso dll Is Nothing Then Throw New IO.FileNotFoundException("File not found", path$)
      Dim info As New ProcessStartInfo(If(exe, "dotnet")) With {.UseShellExecute = False}
      If exe Is Nothing Then info.ArgumentList.Add(dll)
      Dim items As New List(Of String)
      For Each c In common
        Dim text = If(c Is Nothing, "", Global.System.Convert.ToString(c, Globalization.CultureInfo.InvariantCulture))
        items.Add(Global.System.Convert.ToBase64String(Global.System.Text.Encoding.UTF8.GetBytes(text)))
      Next
      info.Environment("QB_COMMON") = String.Join(",", items)
      Dim code = 0
      Using p = Process.Start(info)
        p.WaitForExit()
        code = p.ExitCode
      End Using
      Environment.Exit(code)
    End Sub

    Private Shared s_common As String()

    ''' <summary>COMMON: the value that the program that CHAINed here passed for this position (current when there is none).</summary>
    Public Shared Function COMMON_VALUE(Of T)(index%, current As T) As T
      If s_common Is Nothing Then
        Dim raw = Environment.GetEnvironmentVariable("QB_COMMON")
        s_common = If(String.IsNullOrEmpty(raw), Array.Empty(Of String)(), raw.Split(","c))
      End If
      If index < 0 OrElse index >= s_common.Length Then Return current
      Dim text = Global.System.Text.Encoding.UTF8.GetString(Global.System.Convert.FromBase64String(s_common(index)))
      Return CType(Global.System.Convert.ChangeType(text, GetType(T), Globalization.CultureInfo.InvariantCulture), T)
    End Function

    ''' <summary>SHELL command: runs an operating system command through the command interpreter and waits for it.</summary>
    Public Shared Sub SHELL(command$)
      Dim info As ProcessStartInfo
      If OperatingSystem.IsWindows() Then
        info = New ProcessStartInfo("cmd", "/c " & command)
      Else
        info = New ProcessStartInfo("/bin/sh") With {.UseShellExecute = False}
        info.ArgumentList.Add("-c")
        info.ArgumentList.Add(command)
      End If
      Using p = Process.Start(info)
        p.WaitForExit()
      End Using
    End Sub

    ''' <summary>SYSTEM: ends the program.</summary>
    Public Shared Sub SYSTEM()
      Environment.Exit(0)
    End Sub

    ''' <summary>RESET: closes all open files.</summary>
    Public Shared Sub RESET()
      File.CLOSE()
    End Sub

    ''' <summary>CLEAR: closes the open files (the variables belong to the VB program).  The memory sizes of CLEAR ,,stack are ignored.</summary>
    Public Shared Sub CLEAR(Optional dynamicSize% = -1, Optional stackSize% = -1, Optional unused% = -1)
      File.CLOSE()
    End Sub

    ''' <summary>ENVIRON$(name): the value of an environment variable ("" if it isn't set).</summary>
    Public Shared Function ENVIRON$(name$)
      Return If(Environment.GetEnvironmentVariable(name), "")
    End Function

    ''' <summary>ENVIRON$(n): the nth environment string, "NAME=value" ("" past the end).</summary>
    Public Shared Function ENVIRON$(n%)
      Dim vars = Environment.GetEnvironmentVariables()
      Dim i = 0
      For Each key In vars.Keys
        i += 1
        If i = n Then Return key.ToString & "=" & vars(key)?.ToString
      Next
      Return ""
    End Function

    ''' <summary>ENVIRON "NAME=value": sets an environment variable for this program (named ENVIRON_SET because VB can't overload it with ENVIRON$).</summary>
    Public Shared Sub ENVIRON_SET(setting$)
      Dim eq = setting.IndexOf("="c)
      If eq < 1 Then QBError.Raise(5, "Illegal function call")
      Environment.SetEnvironmentVariable(setting.Substring(0, eq).Trim, setting.Substring(eq + 1).Trim)
    End Sub

    ''' <summary>COMMAND$: the command line arguments, joined with spaces (upper case, as QBasic returns them).</summary>
    Public Shared Function COMMAND$()
      Return String.Join(" ", Environment.GetCommandLineArgs().Skip(1)).ToUpperInvariant()
    End Function

    ''' <summary>TRON: trace on (there is no trace; accepted so programs convert).</summary>
    Public Shared Sub TRON()
    End Sub

    Public Shared Sub TROFF()
    End Sub

    ''' <summary>ERROR n: raises QBasic error number n, as an error in the program would.</summary>
    Public Shared Sub ERROR_(number%)
      If number < 1 OrElse number > 255 Then QBError.Raise(5, "Illegal function call")
      QBError.Raise(number, QBError.Message(number))
    End Sub

    ''' <summary>ERDEV: the device error code (always 0 here).</summary>
    Public Shared Function ERDEV%()
      Return 0
    End Function

    ''' <summary>ERDEV$: the name of the device that had the last error ("" here; named ERDEV_STR because of ERDEV).</summary>
    Public Shared Function ERDEV_STR() As String
      Return ""
    End Function

    ' Port and memory

    ''' <summary>FRE(x): free memory in bytes (an estimate).</summary>
    Public Shared Function FRE&(value%)
      Dim info = GC.GetGCMemoryInfo()
      Return Math.Max(0L, Math.Min(Integer.MaxValue, info.TotalAvailableMemoryBytes - GC.GetTotalMemory(False)))
    End Function

    ''' <summary>INP(port): reads a hardware port (there is none: 0).</summary>
    Public Shared Function INP%(address%)
      Return 0
    End Function

    ''' <summary>OUT port, value: writes a hardware port (there is none: nothing happens).</summary>
    Public Shared Sub OUT(address%, value%)
    End Sub

    ''' <summary>WAIT port, and-mask [, xor-mask]: waits for a hardware port (there is none: returns at once).</summary>
    Public Shared Sub WAIT(address%, andMask%, Optional xorMask% = 0)
    End Sub

    ' ---- DEF SEG, PEEK and POKE ----------------------------------------------------------------------
    ' Memory is simulated: a sparse array of bytes that starts out zero.  That is enough for the common uses (the keyboard
    ' flags at 0:1047 that programs read and restore, scratch bytes); there is no real hardware or screen memory behind it.

    Private Shared ReadOnly m_memory As New Dictionary(Of Long, Byte)
    Private Const DefaultSegment As Integer = &H1000
    Private Shared m_segment As Integer = DefaultSegment

    ''' <summary>DEF SEG = segment (0 to 65535); with no argument, the default data segment again.</summary>
    Public Shared Sub DEF_SEG(Optional segment% = -1)
      m_segment = If(segment = -1, DefaultSegment, segment And &HFFFF)
    End Sub

    Private Shared Function MemoryAddress(offset As Integer) As Long
      Dim o = offset And &HFFFF
      Return CLng(m_segment) * 16 + o
    End Function

    ''' <summary>PEEK(offset): the byte at an offset in the current DEF SEG segment (0 unless it was POKEd).</summary>
    Public Shared Function PEEK(offset As Integer) As Integer
      If offset < -32768 OrElse offset > 65535 Then Throw New ArgumentException("Illegal function call")
      Dim b As Byte
      Return If(m_memory.TryGetValue(MemoryAddress(offset), b), CInt(b), 0)
    End Function

    ''' <summary>POKE offset, value: stores a byte (0 to 255).</summary>
    Public Shared Sub POKE(offset As Integer, value As Integer)
      If offset < -32768 OrElse offset > 65535 OrElse value < 0 OrElse value > 255 Then Throw New ArgumentException("Illegal function call")
      m_memory(MemoryAddress(offset)) = CByte(value)
    End Sub

    ''' <summary>BSAVE file, offset, length: saves bytes of the simulated memory (in the DEF SEG segment) to a file, with BLOAD's header.</summary>
    Public Shared Sub BSAVE(file$, offset%, length%)
      If length < 0 OrElse length > 65535 Then QBError.Raise(5, "Illegal function call")
      Dim bytes(6 + length) As Byte
      bytes(0) = &HFD
      bytes(1) = CByte(m_segment And &HFF) : bytes(2) = CByte((m_segment >> 8) And &HFF)
      bytes(3) = CByte(offset And &HFF) : bytes(4) = CByte((offset >> 8) And &HFF)
      bytes(5) = CByte(length And &HFF) : bytes(6) = CByte((length >> 8) And &HFF)
      For i = 0 To length - 1
        bytes(7 + i) = CByte(PEEK(offset + i))
      Next
      Global.System.IO.File.WriteAllBytes(file, bytes)
    End Sub

    ''' <summary>BLOAD file [, offset]: loads a BSAVE file into the simulated memory (in the DEF SEG segment).</summary>
    Public Shared Sub BLOAD(file$, Optional offset% = -1)
      Dim bytes = Global.System.IO.File.ReadAllBytes(file)
      If bytes.Length < 7 OrElse bytes(0) <> &HFD Then QBError.Raise(54, "Bad file mode")
      Dim start = If(offset = -1, bytes(3) Or (bytes(4) << 8), offset)
      Dim length = bytes(5) Or (bytes(6) << 8)
      For i = 0 To Math.Min(length, bytes.Length - 7) - 1
        POKE(start + i, bytes(7 + i))
      Next
    End Sub

    Public Shared Function SETMEM(value As Long) As Long
      If value <> 0 Then
      End If
      Return 0
    End Function

    Public Shared Function STACK() As Integer
      Return 0
    End Function

    Public Shared Function STACK(num%) As Integer
      If num <> 0 Then
      End If
      Return 0
    End Function

    ' Mixed Language

    Public Shared Function VARSEG%(address&)
      If address <> 0 Then
      End If
      Throw New NotImplementedException
      Return 0
    End Function

    ' Metacommands

    ' Debugging

  End Class

End Namespace