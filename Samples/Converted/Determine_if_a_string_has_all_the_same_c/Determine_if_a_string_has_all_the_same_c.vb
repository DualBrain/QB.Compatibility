' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE Sub CaracteresUnicos (cad$)
    
    CaracteresUnicos((""))
    CaracteresUnicos(("."))
    CaracteresUnicos(("abcABC"))
    CaracteresUnicos(("XYZ ZYX"))
    CaracteresUnicos(("1234567890ABCDEFGHIJKLMN0PQRSTUVWXYZ"))
    End
  End Sub

  
  Sub CaracteresUnicos(ByRef cad$)
      Dim lngt! = Len(cad$)
      PRINT("String = '" & cad$ & "' length = " & Str$(lngt) & " ")
      For i! = 1 To lngt
          For j! = i + 1 To lngt
              If Mid$(cad$, CInt(i), 1) = Mid$(cad$, CInt(j), 1) Then
                  PRINT(" First difference at position " & Str$(i) & " " & " and " & Str$(j) & " " & ", character = '" & Mid$(cad$, CInt(i), 1) & "', hex value = " & Hex$(Asc(Mid$(cad$, CInt(i), 1))))
                  PRINT()
                  Exit Sub
              End If
          Next j
      Next i
      PRINT(" All characters are the same." & QBChr(10))
  End Sub
  
End Module
