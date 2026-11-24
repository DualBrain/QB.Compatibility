' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    FloydTriangle((5))
    PRINT()
    FloydTriangle((14))
  End Sub

  Sub FloydTriangle(ByRef fila As Single)
      Dim numColum(CInt(fila)) As Single
      For colum! = 1 To fila
          numColum(CInt(colum)) = Len(Str$(colum + ((fila * (fila - 1)) / 2)))
      Next colum
  
      PRINT("output for " & Str$(fila)) : PRINT()
      Dim thisNum = 1!
      For r! = 1 To fila
          For colum! = 1 To r
              PRINT(Right$("  " + Str$(thisNum), CInt(numColum(CInt(colum)))) & " ", True)
              thisNum = thisNum + 1
          Next colum
          PRINT()
      Next
  End Sub
  
End Module
