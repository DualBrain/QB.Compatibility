' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim d As Integer
    d = 1234567
    
    Dim b(8) As Integer
    b(0) = -d
    b(1) = (-d) + 1
    b(2) = -2
    b(3) = -1
    b(4) = 0
    b(5) = 1
    b(6) = 2
    b(7) = d - 2
    b(8) = d - 1
    
    For i! = 0 To UBound(b)
        PRINT(Str$(b(CInt(i))) & " " & " -> " & Str$(-b(CInt(i))) & " ")  ' * -1
    Next i
  End Sub

End Module
