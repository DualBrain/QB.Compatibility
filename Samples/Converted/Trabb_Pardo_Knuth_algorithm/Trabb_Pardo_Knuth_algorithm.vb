' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i!
    Dim x!
    
    Dim s(11) As Single
    PRINT("enter 11 numbers")
    For i = 1 To 11
        PRINT(Str$(i), True)
        INPUT(" => ", s(CInt(i)), False)
    Next i
    
    PRINT() : PRINT(QBString(20, "-"))
    
    i = i - 1
    Do
        PRINT("f(" & Str$(s(CInt(i))) & ") = ", True)
        x = f(s(CInt(i)))
        If x > 400 Then
            PRINT("-=< overflow >=-")
        Else
            PRINT(Str$(x) & " ")
        End If
        i = i - 1
    Loop Until i < 1
    End
  End Sub

  Function f(ByRef n!) As Single
      f = CSng(Sqrt(Abs(n!)) + (5 * (n! ^ 3)))
  End Function
  
End Module
