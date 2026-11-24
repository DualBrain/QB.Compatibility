' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    PRINT(Str$(multSum35(999)) & " ")
  End Sub

  Function multSum35(ByRef n As Single) As Single
    If n = 0 Then multSum35 = 0
    Dim suma = 0!
    For i! = 1 To n
      If ((CInt(i) Mod 3) = 0) Or ((CInt(i) Mod 5) = 0) Then suma = suma + i
    Next i
    multSum35 = suma
  End Function
  
End Module
