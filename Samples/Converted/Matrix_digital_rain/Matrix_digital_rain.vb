' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim y!
    Dim l!
    ' DECLARE SUB d (p!, s!, x!, y!)
    Dim t(80) As Single
    For i = 1! To 80!
        t(CInt(i)) = Int((-50) * Rnd())
    Next
    Dim s! = CSng(Timer)
    
    CLS()
    While 1 <> 0
        For i = 1! To 80!
            If t(CInt(i)) > 28 Then t(CInt(i)) = 0
            t(CInt(i)) = t(CInt(i)) + 1
            y = t(CInt(i))
            d(0, 0, i, y - 6)
            d(2, 0, i, y - 5)
            d(2, 0, i, y - 4)
            d(10, 0, i, y - 3)
            d(10, 0, i, y - 2)
            d(11, 0, i, y - 1)
            d(0, 2, i, y)
        Next
    
        l = CSng(Timer)
        While l = Timer : EMPTYLOOP()
        End While
    End While
  End Sub

  
  Sub d(ByRef p As Single, ByRef s As Single, ByRef x As Single, ByRef y As Single)
      COLOR(CInt(p), CInt(s))
      If (y > 0) And (y < 24) Then LOCATE(CInt(y), CInt(x)) : PRINT(QBChr(33 + (CInt((x * y)) Mod 200)), True)
  End Sub
  
End Module
