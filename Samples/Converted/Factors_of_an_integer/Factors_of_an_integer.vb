' Converted from QBasic by qb2vb.

Module Program

  Dim factors(10) As Short

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB factor (what AS INTEGER)
    
    ReDim factors(0)
    
    Dim i As Short, L As Short
    
    INPUT("Gimme a number", i)
    
    factor(i)
    
    PRINT(Str$(factors(0)) & " ", True)
    For L = 1 To CShort(UBound(factors))
        PRINT("," & Str$(factors(L)) & " ", True)
    Next
    PRINT()
  End Sub

  
  Sub factor(ByRef what As Short)
      Dim tmpint1 As Short
      Dim L0 As Short, L1 As Short
  
      Dim tmp(0) As Short
      ReDim factors(0)
      factors(0) = 1
  
      For L0 = 2 To what
          If (0 = (what Mod L0)) Then
              'all this REDIMing and copying can be replaced with:
              'REDIM PRESERVE factors(UBOUND(factors)+1)
              'in languages that support the PRESERVE keyword
              ReDim tmp(UBound(factors))
              For L1 = 0 To CShort(UBound(factors))
                  tmp(L1) = factors(L1)
              Next
              ReDim factors(UBound(factors) + 1)
              For L1 = 0 To CShort(UBound(factors) - 1)
                  factors(L1) = tmp(L1)
              Next
              factors(UBound(factors)) = L0
          End If
      Next
  End Sub
  
End Module
