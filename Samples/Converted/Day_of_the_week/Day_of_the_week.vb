' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION wd! (m!, d!, y!)
    
    For yr = 2008! To 2121!
        If wd(12, 25, yr) = 0 Then PRINT("Dec 25 " & Str$(yr) & " ")
    Next yr
    End
  End Sub

  
  Function wd(ByRef m As Single, ByRef d As Single, ByRef y As Single) As Single
      If m < 3 Then
         m = m + 12
         y = y - 1
      End If
      wd = (CInt((((((y + Int(y / 4)) - Int(y / 100)) + Int(y / 400)) + d) + Int(((153 * m) + 8) / 5))) Mod 7)
  End Function
  
End Module
