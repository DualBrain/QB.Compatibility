' Converted from QBasic by qb2vb.

Module Program

  Dim queens As Short
  Dim arrayqcol(queens) As Integer
  Dim nsoluciones As Integer

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DIM SHARED queens AS INTEGER
    CLS()
    COLOR(15)
    INPUT("Numero de reinas", queens)
    If queens <= 0 Then End
    
    CLS()
    PRINT("queens: Calcula el problema de las" & Str$(queens) & " " & " reinas.")
    ' DIM SHARED arrayqcol(queens) AS LONG ' columnas de reinas
    ' DIM SHARED nsoluciones AS LONG
    
    dofila((1))  ' comenzar en la fila 1
    COLOR(14) : LOCATE(6 + (2 * queens), 1) : PRINT((("Hay " + Str$(nsoluciones)) + " soluciones"))
    End
  End Sub

  
  Sub dofila(ByRef ifila As Single)  ' comienza con la fila de abajo
    Dim s1$
      For icol! = 1 To queens
          For iqueen! = 1 To ifila - 1  ' Comprueba conflictos con las reinas anteriores
              If arrayqcol(CInt(iqueen)) = icol Then GoTo continue1  ' misma columna?
              ' iqueen también es fila de la reina
              If (iqueen + arrayqcol(CInt(iqueen))) = (ifila + icol) Then GoTo continue1  ' diagonal derecha?
              If (iqueen - arrayqcol(CInt(iqueen))) = (ifila - icol) Then GoTo continue1  ' diagonal izquierda?
          Next iqueen
          ' En este punto podemos añadir una reina
          arrayqcol(CInt(ifila)) = CInt(icol)  ' añadir al array
          COLOR(8)
          LOCATE(CInt(ifila + 2), CInt(icol)) : PRINT("x", True)  ' mostrar progreso
  
          COLOR(15)
          If ifila = queens Then  ' solucion?
              nsoluciones = nsoluciones + 1
              LOCATE(4 + queens, 1) : PRINT(("Solucion #" + Str$(nsoluciones)))
              For i1! = 1 To queens  ' filas
                  s1$ = QBString(queens, ".")  ' columnas
                  Mid(s1$, arrayqcol(CInt(i1)), 1) = "Q"  ' Q en la columna reina
                  PRINT(s1$)
              Next i1
              PRINT("")
          Else
              dofila((ifila + 1))  ' llamada recursiva a la siguiente fila
          End If
          COLOR(7) : LOCATE(CInt(ifila + 2), CInt(icol)) : PRINT(".", True)  ' quitar reina
  continue1:
      Next icol
  End Sub
  
End Module
