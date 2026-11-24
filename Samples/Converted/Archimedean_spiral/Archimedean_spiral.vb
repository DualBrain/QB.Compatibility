' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim A!
    Dim B!
    Dim T!
    Dim X!
    Dim Y!
    SCREEN(12)
    WINDOW(-2.67!, -2!, 2.67!, 2!)
    Dim PI! = CSng(4 * Atan(1))
    Dim H! = PI / 40
    A = .2! : B = .05!
    PSET(A, 0)
    For I = 0! To 400!
        T = I * H
        X = CSng((A + (B * T)) * Cos(T))
        Y = CSng((A + (B * T)) * Sin(T))
        LINE(X, Y)
    Next
  End Sub

End Module
