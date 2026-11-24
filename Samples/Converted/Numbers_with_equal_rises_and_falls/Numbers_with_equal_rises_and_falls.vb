' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION eqrf! (n AS LONG)
    
    Dim c As Integer, i As Integer
    c = 0 : i = 1
    
    While c < 10000001
        If eqrf(i) <> 0 Then
            c = c + 1
            If c <= 200 Then PRINT_USING_NOCR("### ", i)
            If c = 1E+07! Then PRINT() : PRINT(Str$(i) & " ")
        End If
        i = i + 1
    End While
    End
  End Sub

  
  Function eqrf(ByRef n As Integer) As Single
      Dim sn As String
      sn = LTrim$(Str$(n))
      Dim q As Short, i As Short
      q = 0
  
      For i = 2 To CShort(Len(sn))
          If Asc(Mid$(sn, i, 1)) > Asc(Mid$(sn, i - 1, 1)) Then
              q = CShort(q + 1)
          ElseIf Asc(Mid$(sn, i, 1)) < Asc(Mid$(sn, i - 1, 1)) Then
              q = CShort(q - 1)
          End If
      Next i
  
      If q = 0 Then
          eqrf = -1
      Else
          eqrf = 0
      End If
  End Function
  
End Module
