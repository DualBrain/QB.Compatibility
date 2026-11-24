' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION split$ (instring$)
    
    PRINT(split$("gHHH5YY++///\"))
    End
  End Sub

  
  Function split$(ByRef instring$)
      If Len(instring$) < 2 Then split$ = instring$
      Dim ret$ = Left$(instring$, 1)
      For i! = 2 To Len(instring$)
          If Mid$(instring$, CInt(i), 1) <> Mid$(instring$, CInt(i - 1), 1) Then ret$ = ret$ + ", "
          ret$ = ret$ + Mid$(instring$, CInt(i), 1)
      Next i
      split$ = ret$
  End Function
  
End Module
