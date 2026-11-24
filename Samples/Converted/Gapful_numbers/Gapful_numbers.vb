' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    muestraGapful(100, 30)
    muestraGapful(1000000, 15)
    muestraGapful(1000000000, 10)
    muestraGapful(7123, 25)
  End Sub

  Function isGapful(ByRef n As Single) As Single
      Dim m! = n
      Dim l! = CInt(n) Mod 10
      While (m >= 10)
          m = Int(m / 10)
      End While
      isGapful = (m * 10) + l
  End Function
  
  
  Sub muestraGapful(ByRef n As Single, ByRef gaps As Single)
      Dim inc = 0!
      PRINT("First " & Str$(gaps) & " " & "gapful numbers >= " & Str$(n) & " " & ":")
      While inc < gaps
          If (CInt(n) Mod CInt(isGapful(n))) = 0 Then
              PRINT(Str$(n) & " " & " ", True)
              inc = inc + 1
          End If
          n = n + 1
      End While
      PRINT(QBChr(10))
  End Sub
  
End Module
