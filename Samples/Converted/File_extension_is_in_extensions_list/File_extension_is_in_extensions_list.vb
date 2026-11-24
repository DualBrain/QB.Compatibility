' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim d$
    Dim flag!
    DATA(".zip", ".rar", ".7z", ".gz", ".archive", ".a##", ".tar.bz2")
    DATA("MyData.a##", "MyData.tar.gz", "MyData.gzip", "MyData.7z.backup")
    DATA("MyData...", "MyData", "MyData_v1.0.tar.bz2", "MyData_v1.0.bz2")
    
    Dim extensions$(7)
    For i = 1! To 7!
        READ(d$) : extensions$(CInt(i)) = d$
    Next i
    
    Dim filenames$(8)
    For i = 1! To 8!
        READ(d$) : filenames$(CInt(i)) = d$
    Next i
    
    For n! = 1 To UBound(filenames$)
        flag = 0
        For m! = 1 To UBound(extensions$)
            If Right$(filenames$(CInt(n)), Len(extensions$(CInt(m)))) = extensions$(CInt(m)) Then
                flag = 1
                PRINT(filenames$(CInt(n)) & " -> " & extensions$(CInt(m)) & " -> " & " true")
                Exit For
            End If
        Next m
        If flag = 0 Then PRINT(filenames$(CInt(n)) & " -> " & "false")
    Next n
    End
    
    ' DATA ".zip", ".rar", ".7z", ".gz", ".archive", ".a##", ".tar.bz2"   (DATA is listed at the start of the program)
    ' DATA "MyData.a##", "MyData.tar.gz", "MyData.gzip", "MyData.7z.backup"   (DATA is listed at the start of the program)
    ' DATA "MyData...", "MyData", "MyData_v1.0.tar.bz2", "MyData_v1.0.bz2"   (DATA is listed at the start of the program)
  End Sub

End Module
