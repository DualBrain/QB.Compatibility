' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim list1$(2, 3)  '= {{"a", "b", "c"}, {"a", "b", "c"}}
    Dim list2$(2, 3)  '= {{"1", "2", "3"}, {"1", "2", "3"}}
    
    permutation((list1$))
    PRINT()
    permutation((list2$))
    End
  End Sub

  
  Sub permutation(ByRef list1$(,))
  
      For n! = 1 To UBound(list1$, 1)
          For m! = 1 To UBound(list1$, 2)
              PRINT(list1$(1, CInt(n)) & " " & list1$(2, CInt(m)))
          Next m
      Next n
      PRINT()
  End Sub
  
End Module
