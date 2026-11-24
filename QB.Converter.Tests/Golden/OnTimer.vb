' Converted from QBasic by qb2vb.

Imports System.Math
Imports QB
Imports QB.Core
Imports QB.Development
Imports QB.Devices
Imports QB.Events
Imports QB.File
Imports QB.Multimedia
Imports QB.Printer
Imports QB.Video

Module Program

  Dim StartTime!
  Dim TimePast!

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/ON%20TIMER.htm
    ON_TIMER(1, AddressOf TimeUpdate)
    TIMER_ON()
    CLS()
    PRINT("Time: " & (TimeString))
    StartTime = CSng(Timer)
    While TimePast < 10
        TimePast = CSng(Timer - StartTime)
    End While
    End
    
  End Sub

  Sub TimeUpdate()
      LOCATE(1, 7) : PRINT((TimeString))
      Return
  End Sub
  
End Module
