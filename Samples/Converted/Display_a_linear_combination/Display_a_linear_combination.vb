' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim cadena$ = ""
    Dim scalar!
    Dim scalars(10, 4) As Single
    scalars(1, 1) = 1 : scalars(1, 2) = 2 : scalars(1, 3) = 3
    scalars(2, 1) = 0 : scalars(2, 2) = 1 : scalars(2, 3) = 2 : scalars(2, 4) = 3
    scalars(3, 1) = 1 : scalars(3, 2) = 0 : scalars(3, 3) = 3 : scalars(3, 4) = 4
    scalars(4, 1) = 1 : scalars(4, 2) = 2 : scalars(4, 3) = 0
    scalars(5, 1) = 0 : scalars(5, 2) = 0 : scalars(5, 3) = 0
    scalars(6, 1) = 0
    scalars(7, 1) = 1 : scalars(7, 2) = 1 : scalars(7, 3) = 1
    scalars(8, 1) = -1 : scalars(8, 2) = -1 : scalars(8, 3) = -1
    scalars(9, 1) = -1 : scalars(9, 2) = -2 : scalars(9, 3) = 0 : scalars(9, 4) = -3
    scalars(10, 1) = -1
    
    CLS()
    For n! = 1 To UBound(scalars)
        cadena$ = ""
        scalar = 0
        For m! = 1 To UBound(scalars, 2)
            scalar = scalars(CInt(n), CInt(m))
            If scalar <> 0 Then
                If scalar = 1 Then
                    cadena$ = (cadena$ + "+e") + Str$(m)
                ElseIf scalar = (-1) Then
                   cadena$ = (cadena$ + "-e") + Str$(m)
                Else
                    If scalar > 0 Then
                        cadena$ = (((cadena$ + QBChr(43)) + Str$(scalar)) + "*e") + Str$(m)
                    Else
                        cadena$ = ((cadena$ + Str$(scalar)) + "*e") + Str$(m)
                    End If
                End If
            End If
        Next m
        If cadena$ = "" Then cadena$ = "0"
        If Left$(cadena$, 1) = "+" Then cadena$ = Right$(cadena$, Len(cadena$) - 1)
        PRINT(cadena$)
    Next n
    End
  End Sub

End Module
