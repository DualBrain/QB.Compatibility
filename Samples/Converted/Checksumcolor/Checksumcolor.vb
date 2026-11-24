' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB colourhex (s$)
    
    CLS()
    colourhex(("#0123456789ABCDEF"))
    End
  End Sub

  
  Sub colourhex(ByRef s$)
    Dim ch$
    Dim k!
      For i! = 1 To Len(s$)
          ch$ = Mid$(s$, CInt(i), 1)
          k = InStr("123456789ABCDEF", ch$)
          COLOR(CInt(15 - k), CInt(k))
          PRINT(ch$, True)
      Next
      COLOR(15, 0)  'text_color, background_color
  End Sub
  
End Module
