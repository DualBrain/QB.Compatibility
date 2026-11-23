' ON TIMER  -  https://qbasic.net/en/qb-manual/Statement/ON%20TIMER.htm
' A VB translation of the QBasic example.  ON TIMER(1) GOSUB TimeUpdate becomes ON_TIMER(1, AddressOf TimeUpdate): the
' handler is a Sub, and returns by ending.  Events run when the program calls into the library, so the busy loop
' below has to call Poll().  (The loop runs for 4 seconds here instead of 10.)

Imports QB
Imports QB.Events
Imports QB.Graphics

Namespace Examples

  Friend Module OnTimerExample

    Private Sub TimeUpdate()
      LOCATE(1, 7) : PRINT(Format(Now, "HH:mm:ss"))
    End Sub

    Public Sub Run()
      ON_TIMER(1, AddressOf TimeUpdate)
      TIMER_ON()
      CLS()
      PRINT("Time: " & Format(Now, "HH:mm:ss"))
      Dim startTime = Timer
      Dim timePast As Double = 0
      Do While timePast < 4
        timePast = Timer - startTime
        Poll() ' (Nothing in this loop calls the library, so give the events a chance to run.)
      Loop
      TIMER_OFF()
    End Sub

  End Module

End Namespace
