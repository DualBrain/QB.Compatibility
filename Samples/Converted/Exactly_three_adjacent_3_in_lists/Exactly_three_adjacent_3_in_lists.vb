' Converted from QBasic by qb2vb.

Module Program

  Const [False] As Short = 0

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim [True] As Single
    Dim go!
    Dim pass!
    Dim c!
    DATA(9, 3, 3, 3, 2, 1, 7, 8, 5)
    DATA(5, 2, 9, 3, 3, 7, 8, 4, 1)
    DATA(1, 4, 3, 6, 7, 3, 8, 3, 2)
    DATA(1, 2, 3, 4, 5, 6, 7, 8, 9)
    DATA(4, 6, 8, 7, 2, 3, 3, 3, 1)
    
    ' CONST False = 0: True = NOT False
    [True] = Not [False]
    
    ' DATA 9,3,3,3,2,1,7,8,5   (DATA is listed at the start of the program)
    ' DATA 5,2,9,3,3,7,8,4,1   (DATA is listed at the start of the program)
    ' DATA 1,4,3,6,7,3,8,3,2   (DATA is listed at the start of the program)
    ' DATA 1,2,3,4,5,6,7,8,9   (DATA is listed at the start of the program)
    ' DATA 4,6,8,7,2,3,3,3,1   (DATA is listed at the start of the program)
    Dim lista(5, 9) As Short
    For i! = 1 To UBound(lista)
        For j! = 1 To UBound(lista, 2)
            READ(lista(CInt(i), CInt(j)))
        Next j
    Next i
    
    For i! = 1 To UBound(lista)
        go = [False]
        pass = [True]
        c = 0
        For j! = 1 To UBound(lista, 2)
            If lista(CInt(i), CInt(j)) = 3 Then
                c = c + 1
                go = [True]
            Else
                If (go = [True]) And (c <> 3) Then pass = [False]
                go = [False]
            End If
        Next j
        PRINT(Str$(i) & " " & "   ", True)
        If (CShort(c = 3) And CInt(pass)) <> 0 Then PRINT("True") Else PRINT("False")
    Next i
  End Sub

End Module
