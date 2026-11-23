Option Explicit On
Option Strict On
Option Infer On

Imports System.Diagnostics

Namespace Global.QB

  Partial Public NotInheritable Class Development

    ' DOS and program management

    ''' <summary>RUN program: starts another program (QBasic would replace the running one).</summary>
    Public Shared Sub RUN(path$)
      Process.Start(path$)
    End Sub

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