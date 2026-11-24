' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim ln$ = ""
    
    OPEN("input.fasta", OpenMode.Input, 1)
    
    Dim first = 1!
    
    Do While Not EOF(1)
        LINE_INPUT(1, ln$)
        If Left$(ln$, 1) = ">" Then
            If (Not CInt(first)) <> 0 Then PRINT()
            PRINT(Mid$(ln$, 2) & ": ", True)
            If first <> 0 Then first = 0
        ElseIf first <> 0 Then
            PRINT() : PRINT("Error : File does not begin with '>'")
            Exit Do
        Else
            If checkNoSpaces(ln$) <> 0 Then
                PRINT(ln$, True)
            Else
                PRINT() : PRINT("Error : Sequence contains space(s)")
                Exit Do
            End If
        End If
    Loop
    CLOSE(1)
  End Sub

  Function checkNoSpaces(ByRef s$) As Single
      For i! = 1 To Len(s$) - 1
          If (Mid$(s$, CInt(i), 1) = QBChr(32)) Or (Mid$(s$, CInt(i), 1) = QBChr(9)) Then checkNoSpaces = 0
      Next i
      checkNoSpaces = 1
  End Function
  
End Module
