' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim w!
    Dim h!
    Dim col!
    Dim y!
    SCREEN(12)
    w = 640 : h = 480
    
    h = CInt(h) \ 4
    Dim y2! = h - 1
    
    For i = 1! To 4!
        col = 0
        y = (i - 1) * h
        For x! = 1 To w Step i
            If (CInt(col) Mod 15) = 0 Then col = 0
            LINE(x, y, x + i, y + h, CInt(col), LineOption.BF)
            col = col + 1
        Next x
    Next i
  End Sub

End Module
