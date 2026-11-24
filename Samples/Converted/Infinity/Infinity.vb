' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION f! ()
    
    On Error GoTo TratoError
    PRINT(Str$(0!) & " ")
    PRINT(Str$(0 / (-1.5!)) & " ")
    PRINT(Str$(1.5! / 0) & " ")
    PRINT(Str$(CSng(0 / 0)) & " ")
    PRINT(Str$(f!()) & " ")
    End
    
    TratoError:
    PRINT("Error " & Str$(Err.Number) & " " & " on line " & Str$(Erl) & " " & QBChr(9) & " --> ", True)
    Select Case Err.Number
    Case 6
        PRINT("Overflow")
        Resume Next
    Case 11
        PRINT("Division by zero")
        Resume Next
    Case Else
        PRINT("Unexpected error, ending program.")
        End
    End Select
  End Sub

  
  Function f!()
      f! = (-1.5!) / 0
  End Function
  
End Module
