' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB Lindenmayer (s AS STRING, rules() AS STRING, count AS INTEGER)
    Dim rules(3) As String
    rules(0) = "I"
    rules(1) = "M"
    rules(2) = "M"
    rules(3) = "MI"
    
    Lindenmayer("I", rules, 5)
    End
  End Sub

  
  Sub Lindenmayer(ByRef s As String, ByRef rules() As String, ByRef count As Short)
      Dim i As Short, j As Short, k As Short, found As Short
      Dim t As String, nxt As String, c As String, rep As String
  
      For i = 0 To count
          PRINT(s)
          nxt = ""
          For j = 1 To CShort(Len(s))
              c = Mid$(s, j, 1)
              found = 0
              For k = CShort(LBound(rules)) To CShort(UBound(rules)) Step 2
                  If c = rules(k) Then
                      rep = rules(k + 1)
                      found = -1
                      Exit For
                  End If
              Next k
              If found = (-1) Then t = rep Else t = c
              nxt = nxt + t
          Next j
          s = nxt
      Next i
  End Sub
  
End Module
