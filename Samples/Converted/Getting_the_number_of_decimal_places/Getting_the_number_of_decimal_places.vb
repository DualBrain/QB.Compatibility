' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(7, 12, 12.345, 12.345677, 0.142857142857142)
    
    ' DECLARE FUNCTION dec! (n!)
    Dim n(4) As Single
    ' DATA 7,12.00,12.345,12.345677,0.142857142857142   (DATA is listed at the start of the program)
    For i! = 0 To UBound(n)
        READ(n(CInt(i)))
        PRINT(Str$(n(CInt(i))) & " " & "has" & Str$(dec(n(CInt(i)))) & " " & "decimals")
    Next i
    End
  End Sub

  
  Function dec(ByRef n As Single) As Single
      Dim c$ = Str$(n)
      If InStr(c$, ".") <> 0 Then dec = Len(Mid$(c$, InStr(c$, ".") + 1)) Else dec = 0
  End Function
  
End Module
