' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(1, 2, 3, 4, 5, 6, 7, 8, 9)
    DATA(10, 11, 12, 13, 14, 15, 16, 17, 18)
    DATA(19, 20, 21, 22, 23, 24, 25, 26, 27)
    
    Dim i As Short
    Dim list1(9) As Short
    ' DATA 1,2,3,4,5,6,7,8,9   (DATA is listed at the start of the program)
    For i = 1 To 9
        READ(list1(i))
    Next i
    Dim list2(9) As Short
    ' DATA 10,11,12,13,14,15,16,17,18   (DATA is listed at the start of the program)
    For i = 1 To 9
        READ(list2(i))
    Next i
    Dim list3(9) As Short
    ' DATA 19,20,21,22,23,24,25,26,27   (DATA is listed at the start of the program)
    For i = 1 To 9
        READ(list3(i))
    Next i
    
    Dim catlist(9) As Integer
    Dim tmp As String
    
    For i = 1 To 9
        tmp = (Str$(list1(i)) + Str$(list2(i))) + Str$(list3(i))
        catlist(i) = CInt(Val(tmp))
        PRINT(Str$(catlist(i)) & " " & " ", True)
    Next i
  End Sub

End Module
