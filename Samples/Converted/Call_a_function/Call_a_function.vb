' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Saludo()
    PRINT(Copialo$("Saludos ", 6, ""))
    PRINT(Copialo$("Saludos ", 3, "!!"))
    PRINT()
    testNumeros(1, 2, 3)
    testNumeros(1, 2, 0)
    PRINT()
    testCadenas("1, 2, 3, 4, cadena, 6, 7, 8, \'incluye texto\'")
  End Sub

  Function Copialo$(ByRef txt$, ByRef siNo As Single, ByRef final$)
      Dim nuevaCadena$
  
      For cont! = 1 To siNo
  	nuevaCadena$ = nuevaCadena$ + txt$
      Next cont
  
      Copialo$ = LTrim$(RTrim$(nuevaCadena$)) + final$
  End Function
  
  
  Sub Saludo()
      PRINT("Hola mundo!")
  End Sub
  
  
  Sub testCadenas(ByRef txt$)
      For cont! = 1 To Len(txt$)
  	PRINT(Mid$(txt$, CInt(cont), 1) & "", True)
      Next cont
  End Sub
  
  
  Sub testNumeros(ByRef a As Single, ByRef b As Single, ByRef c As Single)
  	PRINT(Str$(a) & " " & vbTab & Str$(b) & " " & vbTab & Str$(c) & " ")
  End Sub
  
End Module
