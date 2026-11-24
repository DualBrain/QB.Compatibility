' Converted from QBasic by qb2vb.

Module Program

  Const t As Short = 15
  Dim p(t) As Single

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB padovanN (n!, p!())
    ' CONST t = 15
    ' DIM SHARED p(t)
    
    PRINT("First" & Str$(t) & " " & " terms of the Padovan n-step number sequences:")
    For n = 2! To 8!
        PRINT(Str$(n) & " " & ":", True)
    
        padovanN(n, p)
    
        For i! = 0 To t - 1
            PRINT_USING_NOCR("### ", p(CInt(i)))
        Next i
        PRINT()
    Next n
  End Sub

  
  Sub padovanN(ByRef n As Single, ByRef p_Sng() As Single)
      If (n < 2) Or (t < 3) Then
          For i! = 0 To t - 1
              p_Sng(CInt(i)) = 1
          Next i
          Exit Sub
      End If
  
      padovanN(n - 1, p_Sng)
  
      For i! = n + 1 To t - 1
          p_Sng(CInt(i)) = 0
          For j! = i - 2 To (i - n) - 1 Step -1
              p_Sng(CInt(i)) = p_Sng(CInt(i)) + p_Sng(CInt(j))
          Next j
      Next i
  End Sub
  
End Module
