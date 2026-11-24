' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim xp!
    Dim yp!
    SCREEN(12)
    CLS()
    Dim a = 200!
    Dim b = 200!
    Dim n! = 2.5!
    Dim na! = 2 / n
    Dim t! = .01!
    
    LINE(False, 520, 245, 0, LineOption.BF)
    For i = 0! To 314!
        xp = CSng(((a * Sign(Cos(t))) * (Abs((Cos(t))) ^ na)) + 320)
        yp = CSng(((b * Sign(Sin(t))) * (Abs((Sin(t))) ^ na)) + 240)
        t = t + .02!
        LINE(False, xp, yp, 1, LineOption.BF)
    Next i
  End Sub

End Module
