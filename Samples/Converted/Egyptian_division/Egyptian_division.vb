' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim table(32, 2) As Single
    Dim dividend = 580!
    Dim divisor = 34!
    
    Dim i = 1!
    table(CInt(i), 1) = 1
    table(CInt(i), 2) = divisor
    
    While table(CInt(i), 2) < dividend
        i = i + 1
        table(CInt(i), 1) = table(CInt(i - 1), 1) * 2
        table(CInt(i), 2) = table(CInt(i - 1), 2) * 2
    End While
    i = i - 1
    Dim answer! = table(CInt(i), 1)
    Dim accumulator! = table(CInt(i), 2)
    
    While i > 1
        i = i - 1
        If (table(CInt(i), 2) + accumulator) <= dividend Then
            answer = answer + table(CInt(i), 1)
            accumulator = accumulator + table(CInt(i), 2)
        End If
    End While
    
    PRINT(Str$(dividend) & " divided by " & Str$(divisor) & " using Egytian division", True)
    PRINT(" returns " & Str$(answer) & " mod(ulus) " & Str$(dividend - accumulator))
  End Sub

End Module
