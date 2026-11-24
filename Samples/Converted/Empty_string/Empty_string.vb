' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim s As String  ' implicitly assigned an empty string
    IsEmpty((s))
    Dim t$ = ""  ' explicitly assigned an empty string
    IsEmpty((t$))
    Dim u$ = "not empty"
    IsEmpty((u$))
  End Sub

  Sub IsEmpty(ByRef s As String)
      If Len(s) = 0 Then
      PRINT("String is empty")
      Else
      PRINT("String is not empty")
      End If
      If s = "" Then PRINT("yes, the string is empty")
      If s <> "" Then PRINT("no, the string is not empty")
  End Sub
  
End Module
