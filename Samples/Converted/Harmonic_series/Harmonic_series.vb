' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim n!
    Dim h! = 0!
    
    PRINT("The first twenty harmonic numbers are:")
    For n = 1 To 20
        h = h + (1! / n)
        PRINT(Str$(n) & " " & vbTab & Str$(h) & " ")
    Next n
    PRINT()
    
    h = 1 : n = 2
    For i = 2! To 10!
        While h < i
            h = h + (1! / n)
            n = n + 1
        End While
        PRINT("The first harmonic number greater than " & Str$(i) & " " & " is " & Str$(h) & " " & ", at position " & Str$(n - 1) & " ")
    Next i
    End
  End Sub

End Module
