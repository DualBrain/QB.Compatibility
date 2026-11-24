' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    On Error GoTo TratoError
    PRINT(Str$(2 / 3) & " ")
    PRINT(Str$(3 / 5) & " ")
    PRINT(Str$(4 / 0) & " ")
    End
    
    TratoError:
    PRINT("Error" & Str$(Err.Number) & " " & "in the line" & Str$(Erl) & " ")
    If Err.Number = 11 Then PRINT("Division by zero in line" & Str$(Erl) & " ") : Resume Next
  End Sub

End Module
