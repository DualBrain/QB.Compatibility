' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION sufijo$ (n%)
    ' DECLARE SUB imprimeOrdinal (loLim%, hiLim%)
    
    imprimeOrdinal(0, 25)
    imprimeOrdinal(250, 265)
    imprimeOrdinal(1000, 1025)
    End
    
    ' DEFINT A-Z
  End Sub

  Sub imprimeOrdinal(ByRef loLim As Short, ByRef hiLim As Short)
    loLim = Int(loLim)
    hiLim = Int(hiLim)
    For i As Short = loLim To hiLim
      PRINT(((Str$(i) + sufijo$(i)) + " "), True)
    Next i
    PRINT()
  End Sub
  
  
  Function sufijo$(ByRef n As Short)
    n = Int(n)
    Dim NMod10 As Short = CShort(n Mod 10)
    Dim NMod100 As Short = CShort(n Mod 100)
    If (NMod10 = 1) And (NMod100 <> 11) Then
      sufijo$ = "st"
    Else
      If (NMod10 = 2) And (NMod100 <> 12) Then
        sufijo$ = "nd"
      Else
        If (NMod10 = 3) And (NMod100 <> 13) Then
          sufijo$ = "rd"
        Else
          sufijo$ = "th"
        End If
      End If
    End If
  End Function
  
End Module
