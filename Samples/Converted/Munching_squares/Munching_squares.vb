' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim w = 254!
    
    SCREEN(13)
    VIEW(0, 0, CInt(w / 2), CInt(w / 2), -1, 0)
    
    For x! = 0 To w
        For y! = 0 To w
            COLOR(((CInt(x) Xor CInt(y)) And 255))
            PSET(x, y)
        Next y
    Next x
  End Sub

End Module
