' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim w!
    Dim h!
    Dim col!
    Dim y1!
    SCREEN(12)
    w = 640 : h = 480
    
    h = CInt(h) \ 4
    Dim y2! = h - 1
    
    For ps = 1! To 4!
        col = 0
        For x! = 0 To ((w - ps) - 1) Step ps
            LINE(x, y1, (x + ps) - 1, y2, CInt(col), LineOption.BF)
            col = 15 - col  ' col alternate between 0 (black) and 15 (white)
        Next x
        y1 = y1 + h : y2 = y2 + h
    Next ps
  End Sub

End Module
