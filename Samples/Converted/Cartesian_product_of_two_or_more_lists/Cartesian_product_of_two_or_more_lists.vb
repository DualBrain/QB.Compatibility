' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB cartesian (arr!())
    
    CLS()
    Dim array(2, 2) As Single
    array(1, 1) = 1 : array(1, 2) = 2
    array(2, 1) = 3 : array(2, 2) = 4
    cartesian(array)
    array(1, 1) = 3 : array(1, 2) = 4
    array(2, 1) = 1 : array(2, 2) = 2
    cartesian(array)
    End
  End Sub

  
  Sub cartesian(ByRef arr(,) As Single)
    Dim u1!
    Dim u2!
  u1 = 2 : u2 = 2
  For i! = 1 To u1
    PRINT("{", True)
    For j! = 1 To u2
      PRINT(Str$(arr(CInt(i), CInt(j))) & " ", True)
      If j < u1 Then PRINT(",", True)
    Next j
    PRINT("}", True)
    If i < u2 Then PRINT(" x ", True)
  Next i
  PRINT(" = {", True)
  For i! = 1 To u1
    For j! = 1 To u2
      PRINT("{" & Str$(arr(1, CInt(i))) & " " & "," & Str$(arr(2, CInt(j))) & " " & "}", True)
      If i < u2 Then
        PRINT(", ", True)
      Else
        If j < u1 Then PRINT(", ", True)
      End If
    Next j
  Next i
  PRINT("}")
  End Sub
  
End Module
