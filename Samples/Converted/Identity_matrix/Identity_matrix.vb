' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim n!
    
    Do
        INPUT("Enter size of matrix ", n)
    Loop Until n > 0
    
    Dim identity(CInt(n), CInt(n)) As Single
    
    inicio(identity)
    mostrar(identity)
  End Sub

  Sub inicio(ByRef identity(,) As Single)
      For i! = LBound(identity, 1) To UBound(identity, 1)
          For j! = LBound(identity, 2) To UBound(identity, 2)
              identity(CInt(i), CInt(j)) = 0
          Next j
          identity(CInt(i), CInt(i)) = 1
      Next i
  End Sub
  
  
  Sub mostrar(ByRef identity(,) As Single)
      For i! = LBound(identity, 1) To UBound(identity, 1)
          For j! = LBound(identity, 2) To UBound(identity, 2)
              PRINT(Str$(identity(CInt(i), CInt(j))) & " ", True)
          Next j
          PRINT()
      Next i
  End Sub
  
End Module
