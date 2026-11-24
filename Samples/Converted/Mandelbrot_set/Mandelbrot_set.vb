' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim x!
    Dim y!
    Dim iteration!
    Dim maxIteration!
    Dim xtemp!
    Dim c!
    SCREEN(13)
    WINDOW(-2, 1.5!, 2, -1.5!)
    For x0! = -2 To 2 Step .01!
        For y0! = -1.5! To 1.5! Step .01!
            x = 0
            y = 0
    
            iteration = 0
            maxIteration = 223
    
            While ((((x * x) + (y * y)) <= (2 * 2)) And (iteration < maxIteration))
                xtemp = ((x * x) - (y * y)) + x0
                y = ((2 * x) * y) + y0
    
                x = xtemp
    
                iteration = iteration + 1
            End While
    
            If iteration <> maxIteration Then
                c = iteration
            Else
                c = 0
            End If
    
            PSET(x0, y0, CInt(c + 32))
        Next
    Next
  End Sub

End Module
