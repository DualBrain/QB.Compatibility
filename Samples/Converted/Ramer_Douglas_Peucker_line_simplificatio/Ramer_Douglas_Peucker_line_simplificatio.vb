' Converted from QBasic by qb2vb.

Module Program

  Const [True] As Short = -1

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(0, 0, 1, 0.1, 2, -0.1, 3, 5, 4, 6, 5, 7, 6, 8.1, 7, 9, 8, 9, 9, 9)
    
    ' DECLARE SUB DRDP (ListaDePuntos() AS DOUBLE, ini AS INTEGER, fin AS INTEGER, epsilon AS DOUBLE)
    ' DECLARE FUNCTION DistanciaPerpendicular! (tabla() AS DOUBLE, i AS INTEGER, ini AS INTEGER, fin AS INTEGER)
    
    ' CONST True = -1
    Dim matriz(10, 3) As Double
    ' DATA 0,0,1,0.1,2,-0.1,3,5,4,6,5,7,6,8.1,7,9,8,9,9,9   (DATA is listed at the start of the program)
    For i! = 1 To UBound(matriz)
        READ(matriz(CInt(i), 1)) : READ(matriz(CInt(i), 2))
    Next i
    
    DRDP(matriz, 1, 10, 1)
    
    PRINT("Remaining points after simplifying:")
    matriz(1, 3) = [True] : matriz(10, 3) = [True]
    For i! = 1 To UBound(matriz)
        If matriz(CInt(i), 3) <> 0 Then PRINT("(" & Str$(matriz(CInt(i), 1)) & " " & ", " & Str$(matriz(CInt(i), 2)) & " " & ")  ", True)
    Next i
    End
  End Sub

  
  Function DistanciaPerpendicular(ByRef tabla(,) As Double, ByRef i As Short, ByRef ini As Short, ByRef fin As Short) As Single
      Dim dx As Double, dy As Double, mag As Double, pvx As Double, pvy As Double
      Dim pvdot As Double, dsx As Double, dsy As Double, ax As Double, ay As Double
  
      dx = tabla(fin, 1) - tabla(ini, 1)
      dy = tabla(fin, 2) - tabla(ini, 2)
  
      'Normalise
      mag = ((dx ^ 2) + (dy ^ 2)) ^ .5!
      If mag > 0 Then dx = dx / mag : dy = dy / mag
  
      pvx = tabla(i, 1) - tabla(ini, 1)
      pvy = tabla(i, 2) - tabla(ini, 2)
  
      'Get dot product (project pv onto normalized direction)
      pvdot = (dx * pvx) + (dy * pvy)
  
      'Scale line direction vector
      dsx = pvdot * dx
      dsy = pvdot * dy
  
      'Subtract this from pv
      ax = pvx - dsx
      ay = pvy - dsy
  
      DistanciaPerpendicular = CSng(((ax ^ 2) + (ay ^ 2)) ^ .5!)
  End Function
  
  
  Sub DRDP(ByRef ListaDePuntos(,) As Double, ByRef ini As Short, ByRef fin As Short, ByRef epsilon As Double)
      Dim dmax As Double, d As Double
      Dim indice As Short, i As Short
      ' Find the point with the maximum distance
  
      If UBound(ListaDePuntos) < 2 Then PRINT("Not enough points to simplify") : End
  
      For i = CShort(ini + 1) To fin
          d = DistanciaPerpendicular(ListaDePuntos, i, ini, fin)
          If d > dmax Then indice = i : dmax = d
      Next
  
      ' If max distance is greater than epsilon, recursively simplify
      If dmax > epsilon Then
          ListaDePuntos(indice, 3) = [True]
          ' Recursive call
          DRDP(ListaDePuntos, ini, indice, epsilon)
          DRDP(ListaDePuntos, indice, fin, epsilon)
      End If
  End Sub
  
End Module
