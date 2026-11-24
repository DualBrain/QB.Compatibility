' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION lcs$ (list$())
    
    Dim pre$(2)  '= {"baabababc","baabc","bbbabc"}   'abc
    pre$(0) = "baabababc"
    pre$(1) = "baabc"
    pre$(2) = "bbbabc"
    Dim longitud! = UBound(pre$)
    
    PRINT("There are" & Str$(longitud) & " " & "words in the list:  ", True)
    For n! = 1 To longitud
        PRINT(pre$(CInt(n)) & "  ", True)
    Next n
    
    PRINT()
    PRINT("The longest common suffix is:  " & lcs$(pre$))
  End Sub

  
  Function lcs$(ByRef list$())
    Dim sub_$ = ""
      If UBound(list$) = (-1) Then lcs$ = "" : Exit Function
  
      Dim ref$ = list$(0)
      For i! = 1 To UBound(list$)
          If Len(list$(CInt(i))) < Len(ref$) Then ref$ = list$(CInt(i))
      Next
  
      For i! = 1 To Len(ref$)
          sub_$ = Right$(ref$, CInt(i))
          For j! = 0 To UBound(list$)
              If Right$(list$(CInt(j)), CInt(i)) <> sub_$ Then lcs$ = Right$(ref$, CInt(i - 1)) : Exit Function
          Next
      Next
  
      lcs$ = ref$
  End Function
  
End Module
