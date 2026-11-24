' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim limit = 120!
    
    Dim flags(CInt(limit)) As Single
    For n! = 2 To limit
        flags(CInt(n)) = 1
    Next n
    
    PRINT("Prime numbers less than or equal to " & Str$(limit) & " " & " are: ")
    For n! = 2 To CSng(Sqrt(limit))
        If flags(CInt(n)) = 1 Then
            For i! = n * n To limit Step n
                flags(CInt(i)) = 0
        Next i
        End If
    Next n
    
    For n! = 1 To limit
        If flags(CInt(n)) <> 0 Then PRINT_USING_NOCR("####", n)
    Next n
  End Sub

End Module
