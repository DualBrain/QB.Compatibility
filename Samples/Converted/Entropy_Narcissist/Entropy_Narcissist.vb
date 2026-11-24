' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i As Short, byteValue As Short
    Dim totalchar(255) As Integer
    Dim entropy As Double, prop As Double
    Dim c As Short
    
    Dim filename As String : filename = "ENTROPY.BAS"
    Dim ff As Short : ff = CShort(FREEFILE())
    
    OPEN(filename, OpenMode.Binary, ff)
    
    If Err.Number <> 0 Then
        PRINT("Error opening the file")
        BEEP()
        End
    End If
    
    Dim fileSize As Integer : fileSize = CInt(LOF(ff))
    
    If fileSize = 0 Then
        CLOSE(ff)
        PRINT("The file is empty")
        End
    End If
    
    ' Read one byte at a time.
    ' In QBasic, buffer must be a string of length 1.
    Dim buffer As String : buffer = Space$(1)
    
    For i = 1 To CShort(fileSize)
        [GET](ff, buffer)
    
        byteValue = CShort(Asc(buffer))
        totalchar(byteValue) = totalchar(byteValue) + 1
    Next i
    
    CLOSE(ff)
    
    For i = 0 To 255
        If totalchar(i) <> 0 Then
            prop = totalchar(i) / fileSize
            entropy = entropy - (prop * (Log(prop) / Log(2)))
        End If
    Next i
    
    i = 0
    For c = CShort(Len(filename)) To 1 Step -1
        If (Mid$(filename, i, 1) = "\") Or (Mid$(filename, i, 1) = "/") Then i = c : Exit For
    Next c
    
    If i > 0 Then filename = Mid$(filename, i + 1)
    
    PRINT("My name is " & filename)
    PRINT()
    PRINT("The Entropy of myself is" & Str$(entropy) & " ")
    End
  End Sub

End Module
