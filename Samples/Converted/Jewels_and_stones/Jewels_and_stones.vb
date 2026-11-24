' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION contarjoyas (piedras$, joyas$)
    
    PRINT(Str$(contarjoyas("aAAbbbb", "aA")) & " ")
    PRINT(Str$(contarjoyas("ZZ", "z")) & " ")
    PRINT(Str$(contarjoyas("ABCDEFGHIJKLMNOPQRSTUVWXYZ@abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ@abcdefghijklmnopqrstuvwxyz")) & " ")
    PRINT(Str$(contarjoyas("AB", "")) & " ")
    End
  End Sub

  
  Function contarjoyas(ByRef piedras$, ByRef joyas$) As Single
    Dim bc!
      Dim cnt = 0!
      For i! = 1 To Len(piedras$)
          bc = InStr(1, joyas$, Mid$(piedras$, CInt(i), 1))
          If bc <> 0 Then cnt = cnt + 1
      Next i
      contarjoyas = cnt
  End Function
  
End Module
