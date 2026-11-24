' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    perms((4))
  End Sub

  Sub perms(ByRef n As Single)
      Dim a(CInt(n - 1)) As Single, c(CInt(n - 1)) As Single
  
      For j! = 0 To n - 1
          a(CInt(j)) = j + 1
          PRINT(Str$(a(CInt(j))) & " ", True)
      Next j
      PRINT()
  
      Dim i = 0!
      While i < n
          If c(CInt(i)) < i Then
              If (CInt(i) And 1) = 0 Then
                  SWAP(a(0), a(CInt(i)))
              Else
                  SWAP(a(CInt(c(CInt(i)))), a(CInt(i)))
              End If
              For j! = 0 To n - 1
                  PRINT(Str$(a(CInt(j))) & " ", True)
              Next j
              PRINT()
              c(CInt(i)) = c(CInt(i)) + 1
              i = 0
          Else
              c(CInt(i)) = 0
              i = i + 1
          End If
      End While
  End Sub
  
End Module
