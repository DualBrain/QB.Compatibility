' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim parseMe As String
    parseMe = "Hello,How,Are,You,Today"
    
    Dim tmpLng1 As Short, tmpLng2 As Short, parsedCount As Short
    tmpLng2 = 1
    parsedCount = -1
    
    'count number of tokens
    Do
        tmpLng1 = CShort(InStr(tmpLng2, parseMe, ","))
        If tmpLng1 <> 0 Then
            parsedCount = CShort(parsedCount + 1)
            tmpLng2 = CShort(tmpLng1 + 1)
        Else
            If tmpLng2 < (Len(parseMe) + 1) Then parsedCount = CShort(parsedCount + 1)
            Exit Do
        End If
    Loop
    
    If parsedCount > (-1) Then
        Dim parsed(parsedCount) As String
        tmpLng2 = 1
        parsedCount = -1
    
        'parse
        Do
            tmpLng1 = CShort(InStr(tmpLng2, parseMe, ","))
            If tmpLng1 <> 0 Then
                parsedCount = CShort(parsedCount + 1)
                parsed(parsedCount) = Mid$(parseMe, tmpLng2, tmpLng1 - tmpLng2)
                tmpLng2 = CShort(tmpLng1 + 1)
            Else
                If tmpLng2 < (Len(parseMe) + 1) Then
                    parsedCount = CShort(parsedCount + 1)
                    parsed(parsedCount) = Mid$(parseMe, tmpLng2)
                End If
                Exit Do
            End If
        Loop
    
        PRINT(parsed(0), True)
        For L0! = 1 To parsedCount
            PRINT("." & parsed(CInt(L0)), True)
        Next
    End If
  End Sub

End Module
