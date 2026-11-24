' Converted from QBasic by qb2vb.

Module Program

  Dim x As String

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION IsPangram! (sentence AS STRING)
    
    
    x = "My dog has fleas."
    doIt()
    x = "The lazy dog jumps over the quick brown fox."
    doIt()
    x = "Jackdaws love my big sphinx of quartz."
    doIt()
    x = "What's a jackdaw?"
    doIt()
    
    End
    
  End Sub

  Sub doIt()
      PRINT(Str$(IsPangram!(x)) & " " & vbTab & x)
      Return
  End Sub
  
  
  Function IsPangram!(ByRef sentence As String)
      'returns -1 (true) if sentence is a pangram, 0 (false) otherwise
      Dim l As Short, s As String, t As Short
      Dim letters(25) As Short
  
      For l = 1 To CShort(Len(sentence))
          s = UCase$(Mid$(sentence, l, 1))
          Select Case s
              Case "A" To "Z"
                  t = CShort(Asc(s) - 65)
                  letters(t) = 1
          End Select
      Next
  
      For l = 0 To 25
          If letters(l) < 1 Then
              IsPangram! = 0
              Exit Function
          End If
      Next
  
      IsPangram! = -1
  End Function
  
End Module
