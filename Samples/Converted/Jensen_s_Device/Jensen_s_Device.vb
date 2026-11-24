' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Evaluation()
    End
  End Sub

  
  Sub Evaluation()
    Dim lo!
    Dim hi!
    Dim temp!
      lo = 1
      hi = 100
      temp = 0
      For i! = lo To hi
          temp = temp + (1 / i)
      Next i
      PRINT(Str$(temp) & " ")
  End Sub
  
End Module
