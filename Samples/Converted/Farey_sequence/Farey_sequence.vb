' Converted from QBasic by qb2vb.

Module Program

  Const [TRUE] As Short = -1

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim [FALSE] As Single
    Dim t!
    
    CLS()
    ' CONST TRUE = -1: FALSE = NOT TRUE
    [FALSE] = Not [TRUE]
    
    For i = 1! To 9!
        PRINT_USING_NOCR("F# =", i)
        t = farey(i, [FALSE])
    Next i
    For i = 10! To 11!
        PRINT_USING_NOCR("F## =", i)
    	t = farey(i, [FALSE])
    Next i
    PRINT()
    For i = 100! To 900! Step 100!
        PRINT_USING("F#### = ######", i, farey(i, [FALSE]))
    Next i
    PRINT_USING("F1000 = ######", farey(1000, [FALSE]))
  End Sub

  Function farey(ByRef n As Single, ByRef dsc As Single) As Single
    Dim b!
    Dim c!
    Dim d!
    Dim a!
    Dim cnt!
    Dim aa!
    Dim bb!
    Dim cc!
    Dim dd!
    Dim k!
      b = 1 : c = 1 : d = n
  
      If dsc = [TRUE] Then a = 1 : c = n - 1
  
      cnt = cnt + 1
      If n < 12 Then PRINT(Str$(a) & " " & "/" & Str$(b) & " ", True)
  
      While ((CShort((c <= n)) And (Not CInt(dsc))) Or (CShort((a > 0)) And CInt(dsc))) <> 0
  	aa = a : bb = b : cc = c : dd = d
  	k = CInt((n + b)) \ CInt(d)
  	a = cc : b = dd : c = (k * cc) - aa : d = (k * dd) - bb
  	cnt = cnt + 1
  	If n < 12 Then PRINT(Str$(a) & " " & "/" & Str$(b) & " ", True)
      End While
  
      If n < 12 Then PRINT()
  
      farey = cnt
  End Function
  
End Module
