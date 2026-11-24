' Converted from QBasic by qb2vb.

Module Program

  Dim lista(9) As Single

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(1, 2, 3, 4, 5, 6, 7, 8, 9)
    
    ' DIM SHARED lista(1 TO 9)
    ' DATA 1,2,3,4,5,6,7,8,9   (DATA is listed at the start of the program)
    For i = 1! To 9!
        READ(lista(CInt(i)))
    Next i
    
    shiftLeftN(lista, 3)
    
    For i = 1! To 9!
        PRINT(Str$(lista(CInt(i))) & " ", True)
        If i < 9 Then PRINT(", ", True)
    Next i
    End
  End Sub

  
  Sub shiftLeft(ByRef lista_Sng() As Single)
      Dim lo! = LBound(lista_Sng)
      Dim hi! = UBound(lista_Sng)
      Dim first! = lista_Sng(CInt(lo))
      For i! = lo To hi
  	lista_Sng(CInt(i)) = lista_Sng(CInt(i + 1))
      Next i
      lista_Sng(CInt(hi)) = first
  End Sub
  
  
  Sub shiftLeftN(ByRef lista_Sng() As Single, ByRef n As Single)
      For i! = 1 To n
  	shiftLeft(lista_Sng)
  	Next i
  End Sub
  
End Module
