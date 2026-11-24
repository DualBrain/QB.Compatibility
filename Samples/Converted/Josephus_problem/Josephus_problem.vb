' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION josephus! (n!, k!, m!)
    
    Dim n = 41!
    Dim k = 3!
    PRINT("n = " & Str$(n) & " " & vbTab & "k = " & Str$(k) & " " & vbTab & "final survivor = " & Str$(josephus(n, k, 0)) & " ")
    End
  End Sub

  
  Function josephus(ByRef n As Single, ByRef k As Single, ByRef m As Single) As Single
      Dim lm! = m
      For i! = m + 1 To n
          lm = CInt((lm + k)) Mod CInt(i)
      Next i
      josephus = lm
  End Function
  
End Module
