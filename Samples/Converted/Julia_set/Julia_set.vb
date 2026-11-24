' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim zreal!
    Dim zimag!
    Dim BR!
    Dim escala! = CSng(1 / 81)
    Dim zeroX = 160!
    Dim zeroY = 100!
    Dim maxiter = 32!
    
    Dim CR! = -.798!
    Dim CI! = .1618!
    
    SCREEN(13)
    For x! = 0 To (2 * zeroX) - 1
        For y! = 0 To (2 * zeroY) - 1
            zreal = (x - zeroX) * escala
            zimag = (zeroY - y) * escala
            For iter! = 1 To maxiter
                BR = (CR + (zreal * zreal)) - (zimag * zimag)
                zimag = CI + ((2 * zreal) * zimag)
                zreal = BR
                If ((zreal * zreal) + (zimag * zimag)) > 4 Then
                    PSET(x, y, 1 + (CInt(iter) Mod 16))
                    Exit For
                End If
            Next iter
        Next y
    Next x
    End
  End Sub

End Module
