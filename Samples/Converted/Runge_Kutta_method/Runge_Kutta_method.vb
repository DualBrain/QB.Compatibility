' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim t!
    Dim actual!
    Dim k1!
    Dim k2!
    Dim k3!
    Dim k4!
    Dim y! = 1
    For i = 0! To 100!
        t = i / 10
    
        If t = Int(t) Then
           actual! = CSng((((t ^ 2) + 4) ^ 2) / 16)
            PRINT_USING_NOCR("y(##) = ###.######  Error = ", t, y!)
           PRINT(Str$(actual! - y!) & " ")
           End If
    
        k1! = CSng(t * Sqrt(y!))
        k2! = CSng((t + .05!) * Sqrt(y! + (.05! * k1!)))
        k3! = CSng((t + .05!) * Sqrt(y! + (.05! * k2!)))
        k4! = CSng((t + .1!) * Sqrt(y! + (.1! * k3!)))
        y! = y! + ((.1! * ((k1! + (2 * (k2! + k3!))) + k4!)) / 6)
    Next i
  End Sub

End Module
