' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim seconds!
    Dim secondstot!
    Dim stepstot!
    RANDOMIZE(CInt(Timer))  ' RANDOMIZE  for True BASIC
    Dim stepsbehind = 0!
    Dim stairslength = 100!
    
    PRINT("Seconds" & vbTab & "steps behind" & vbTab & "steps ahead")
    For trial = 1! To 10000!
        stepsbehind = 0
        seconds = 0
        stairslength = 100
        Do While stepsbehind < stairslength
            stepsbehind = stepsbehind + 1
            For j = 1! To 5!
                If Int(Rnd() * stairslength) < stepsbehind Then stepsbehind = stepsbehind + 1
                stairslength = stairslength + 1
            Next j
            seconds = seconds + 1
            If ((trial = 1) And (seconds > 599)) And (seconds < 610) Then PRINT(Str$(seconds) & " " & vbTab & Str$(stepsbehind) & " " & vbTab & Str$(stairslength - stepsbehind) & " ")
        Loop
        secondstot = secondstot + seconds
        stepstot = stepstot + stairslength
    Next trial
    
    PRINT("Average time taken: " & Str$(secondstot / 10000) & " " & " seconds.")
    PRINT("Average final staircase length: " & Str$(stepstot / 10000) & " " & " steps.")
    End
  End Sub

End Module
