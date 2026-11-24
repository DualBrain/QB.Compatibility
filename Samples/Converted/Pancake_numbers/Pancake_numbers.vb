' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim n!
    Dim c!
    ' DECLARE FUNCTION pancake! (n)
    
    For i = 0! To 3!
        For j = 1! To 5!
            n = (i * 5) + j
            c = c + 1
            PRINT_USING("p(##) = ##  ", n, pancake(n))
            If (CInt(c) Mod 5) = 0 Then PRINT()
        Next j
    Next i
  End Sub

  
  Function pancake(ByRef n As Single) As Single
      Dim gap = 2!
      Dim sum = 2!
      Dim adj = -1!
      While sum < n
          adj = adj + 1
          gap = (gap * 2) - 1
          sum = sum + gap
      End While
      pancake = n + adj
  End Function
  
End Module
