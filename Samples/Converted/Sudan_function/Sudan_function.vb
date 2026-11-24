' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    PRINT("F(2,1,1) = " & Str$(F(2, 1, 1)) & " ")
    PRINT("F(3,1,1) = " & Str$(F(3, 1, 1)) & " ")
    PRINT("F(2,2,1) = " & Str$(F(2, 2, 1)) & " ")
    End
  End Sub

  Function F(ByRef n As Single, ByRef x As Single, ByRef y As Single) As Single
      If n = 0 Then
         F = x + y
      Else
         If y = 0 Then
            F = x
         Else
            F = F(n - 1, F(n, x, y - 1), F(n, x, y - 1) + y)
         End If
      End If
  End Function
  
End Module
