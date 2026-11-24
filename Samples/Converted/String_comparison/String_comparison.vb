' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim s1 As String, s2 As String, s3 As String
    
    s1 = "Dog" : s2 = "Dog"
    PRINT(s1 & StringCompare$(s1, s2, 0) & s2)
    s2 = "Cat"
    PRINT(s1 & StringCompare$(s1, s2, 0) & s2)
    s2 = "Rat"
    PRINT(s1 & StringCompare$(s1, s2, 0) & s2)
    s2 = "dog"
    PRINT(s1 & StringCompare$(s1, s2, 0) & s2)
    PRINT(s1 & StringCompare$(s1, s2, 1) & s2 & " if case is ignored")
    s1 = "Dog" : s2 = "Pig"
    s3 = StringCompare$(s1, s2, 0)
    If s3 <> " is equal to " Then PRINT(s1 & " is not equal to " & s2)
    End
  End Sub

  Function StringCompare$(ByRef s1 As String, ByRef s2 As String, ByRef ignoreCase As Single)
      Dim s As String, t As String
      If ignoreCase <> 0 Then
          s = LCase$(s1)
          t = LCase$(s2)
      Else
          s = s1
          t = s2
      End If
      If s < t Then StringCompare$ = " comes before " : Exit Function
      If s = t Then StringCompare$ = " is equal to " : Exit Function
      StringCompare$ = " comes after "
  End Function
  
End Module
