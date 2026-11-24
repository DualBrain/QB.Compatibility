' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    EuclideanRhythm(5, 13)
    EuclideanRhythm(3, 8)
  End Sub

  Sub EuclideanRhythm(ByRef m As Single, ByRef n As Single)
    Dim apos!
    Dim bpos!
      Dim r$(CInt(n))
      Dim apio = 1!
      Dim afin! = m
      Dim bpio! = m + 1
      Dim bfin! = n
      For i! = 1 To m
          r$(CInt(i)) = "1"
      Next i
      For i! = m + 1 To n
          r$(CInt(i)) = "0"
      Next i
      While (afin > apio) And (bfin > bpio)
          apos = apio
          bpos = bpio
          While (apos <= afin) And (bpos <= bfin)
              r$(CInt(apos)) = r$(CInt(apos)) + r$(CInt(bpos))
              apos = apos + 1
              bpos = bpos + 1
          End While
          If bpos < bfin Then
              bpio = bpos
          Else
              bpio = apos
              bfin = afin
              afin = apos - 1
          End If
      End While
      Dim result$ = ""
      For i! = apio To afin
          result$ = result$ + r$(CInt(i))
      Next i
      For i! = bpio To bfin
          result$ = result$ + r$(CInt(i))
      Next i
      PRINT(result$)
  End Sub
  
End Module
