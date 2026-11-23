' OPEN COM  -  https://qbasic.net/en/qb-manual/Statement/OPEN%20COM.htm
' A VB translation of the QBasic example (OPEN "COM1:..." FOR RANDOM AS #1), with a little around it to show the port working.
' COM1 is COM1 on Windows and /dev/ttyS0 elsewhere; a device path works too ("/dev/ttyUSB0:9600,N,8,1").  Set the QB_COM
' environment variable to the whole open string to use another port.

Imports QB
Imports QB.File
Imports QB.Video

Namespace Examples

  Friend Module OpenComExample

    Public Sub Run()
      ' Use this example for trouble shooting serial communications problems.
      ' Slow baud, hardware handshaking is ignored and buffers are enlarged.
      Dim spec = Environment.GetEnvironmentVariable("QB_COM")
      If String.IsNullOrEmpty(spec) Then spec = "COM1:300,N,8,1,CD0,CS0,DS0,OP0,RS,TB2048,RB2048"
      Try
        OPEN(spec, OpenMode.Random, 1)
      Catch ex As Exception
        PRINT("Can't open " & spec & ": " & ex.Message)
        Return
      End Try
      PRINT("Opened " & spec)
      PRINT(1, "HELLO")
      PRINT("Sent HELLO; waiting for a reply...")
      Dim sw = Diagnostics.Stopwatch.StartNew
      Do While EOF(1) AndAlso sw.ElapsedMilliseconds < 5000
        Threading.Thread.Sleep(10)
      Loop
      If EOF(1) Then
        PRINT("Nothing came back.")
      Else
        Dim reply As String = ""
        LINE_INPUT(1, reply)
        PRINT("Received: " & reply)
      End If
      CLOSE(1)
    End Sub

  End Module

End Namespace
