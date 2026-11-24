' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim test$(3)
    test$(0) = "AB"
    test$(1) = "AB,CD"
    test$(2) = "AB,CD,DB"
    test$(3) = "HIK,AB,CD,DB,FGH"
    For t = 0! To 3!
        consolidate(test$(CInt(t)))
    Next t
  End Sub

  Sub consolidate(ByRef s$)
    Dim p!
    Dim ts$ = ""
      Dim sets$(100)
      Dim n = 0!
      For i! = 1 To Len(s$)
          If Mid$(s$, CInt(i), 1) = "," Then
              n = n + 1
          Else
              sets$(CInt(n)) = sets$(CInt(n)) + Mid$(s$, CInt(i), 1)
          End If
      Next i
  
      For i! = 0 To n
          p = i
          ts$ = ""
          For j! = i To 0 Step -1
              If ts$ = "" Then
                  p = j
              End If
              ts$ = ""
              For k! = 1 To Len(sets$(CInt(p)))
                  If j > 0 Then
                      If InStr(sets$(CInt(j - 1)), Mid$(sets$(CInt(p)), CInt(k), 1)) = 0 Then
                          ts$ = ts$ + Mid$(sets$(CInt(p)), CInt(k), 1)
                      End If
                  End If
              Next k
              If Len(ts$) < Len(sets$(CInt(p))) Then
                  If j > 0 Then
                      sets$(CInt(j - 1)) = sets$(CInt(j - 1)) + ts$
                      sets$(CInt(p)) = "-"
                      ts$ = ""
                  End If
              Else
                  p = i
              End If
          Next j
      Next i
  
      Dim temp$ = sets$(0)
      For i! = 1 To n
          temp$ = (temp$ + ",") + sets$(CInt(i))
      Next i
  
      PRINT(s$ & " = " & temp$)
  End Sub
  
End Module
