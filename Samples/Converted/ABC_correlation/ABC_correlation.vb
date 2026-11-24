' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Do
        Dim s As String
        INPUT("Enter a string or 'q' to quit: ", s, False)
        If s = "q" Then Exit Do
        Dim res As Short
        res = CShort(areAbcCountsEqual(s))
        PRINT("The counts of 'a', 'b', 'c' are ", True)
        If res <> 0 Then
            PRINT("equal.")
        Else
            PRINT("not equal.")
        End If
        PRINT()
    Loop
  End Sub

  
  Function areAbcCountsEqual(ByRef s As String) As Single
    Dim a!
    Dim b!
    Dim c!
      a = 0 : b = 0 : c = 0
      s = LCase$(s)
      For i! = 1 To Len(s)
          If Mid$(s, CInt(i), 1) = "a" Then
              a = a + 1
          ElseIf Mid$(s, CInt(i), 1) = "b" Then
              b = b + 1
          ElseIf Mid$(s, CInt(i), 1) = "c" Then
              c = c + 1
          End If
      Next i
      areAbcCountsEqual = CSng((a = b) And (b = c))
  End Function
  
End Module
