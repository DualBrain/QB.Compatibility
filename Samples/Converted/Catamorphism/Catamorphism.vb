' Converted from QBasic by qb2vb.

Module Program

  Dim n(10) As Single

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DIM SHARED n(10)
    For i = 1! To 10! : n(CInt(i)) = i : Next i
    
    PRINT("  +: " & " " & Str$(cat#(10, "+")) & " ")
    PRINT("  -: " & " " & Str$(cat#(10, "-")) & " ")
    PRINT("  *: " & " " & Str$(cat#(10, "*")) & " ")
    PRINT("  /: " & " " & Str$(cat#(10, "/")) & " ")
    PRINT("  ^: " & " " & Str$(cat#(10, "^")) & " ")
    PRINT("min: " & " " & Str$(cat#(10, "min")) & " ")
    PRINT("max: " & " " & Str$(cat#(10, "max")) & " ")
    PRINT("avg: " & " " & Str$(cat#(10, "avg")) & " ")
  End Sub

  
  Function FNMIN(ByRef a As Single, ByRef b As Single) As Single
  If (a < b) Then FNMIN = a Else FNMIN = b
  End Function
  
  Function FNMAX(ByRef a As Single, ByRef b As Single) As Single
  If (a < b) Then FNMAX = b Else FNMAX = a
  End Function
  
  
  Function cat#(ByRef cont As Single, ByRef op$)
  Dim temp! = n(1)
  For i! = 2 To cont
   If op$ = "+" Then temp = temp + n(CInt(i))
   If op$ = "-" Then temp = temp - n(CInt(i))
   If op$ = "*" Then temp = temp * n(CInt(i))
   If op$ = "/" Then temp = temp / n(CInt(i))
   If op$ = "^" Then temp = CSng(temp ^ n(CInt(i)))
   If op$ = "max" Then temp = FNMAX(temp, n(CInt(i)))
   If op$ = "min" Then temp = FNMIN(temp, n(CInt(i)))
   If op$ = "avg" Then temp = temp + n(CInt(i))
  Next i
  If op$ = "avg" Then temp = temp / cont
  cat# = temp
  End Function
  
End Module
