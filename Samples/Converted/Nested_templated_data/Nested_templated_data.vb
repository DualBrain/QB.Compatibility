' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim [True] As Single
    Dim [False] As Single
    [True] = -1 : [False] = Not CInt([True])
    
    Dim p(6) As Single
    Dim p_Str$(6)
    p_Str$(0) = "Payload#0" : p_Str$(1) = "Payload#1"
    p_Str$(2) = "Payload#2" : p_Str$(3) = "Payload#3"
    p_Str$(4) = "Payload#4" : p_Str$(5) = "Payload#5"
    p_Str$(6) = "Payload#6"
    Dim q(6) As Single
    Dim t(2, 3) As Single
    t(0, 0) = 1 : t(0, 1) = 2
    t(1, 0) = 3 : t(1, 1) = 4 : t(1, 2) = 1
    t(2, 0) = 5
    
    For i! = 0 To UBound(t)  '0 To 2
        For j! = 0 To UBound(t, 2)  '0 To 3
            If t(CInt(i), CInt(j)) <> 0 Then
                q(CInt(t(CInt(i), CInt(j)))) = [True]
                t(CInt(i), CInt(j)) = t(CInt(i), CInt(j)) + 1
            End If
        Next j
    Next i
    
    For i! = 0 To UBound(t)  '0 To 2
        For j! = 0 To UBound(t, 2)  '0 To 3
            If t(CInt(i), CInt(j)) <> 0 Then PRINT(p_Str$(CInt(t(CInt(i), CInt(j)) - 1)) & " ", True)
        Next j
        PRINT()
    Next i
    
    For i! = 0 To UBound(q)
        If q(CInt(i)) = [False] Then PRINT(p_Str$(CInt(i)) & " is not used")
    Next i
  End Sub

End Module
