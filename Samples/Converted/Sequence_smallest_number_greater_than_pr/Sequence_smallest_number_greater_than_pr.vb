' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim n!
    
    Dim UPTO = 15!
    Dim i = 2!
    Dim nfound = 1!
    
    PRINT(Str$(1) & " ", True)  'special case
    
    While nfound < UPTO
        n = divisors(i)
        If n = (nfound + 1) Then
            nfound = nfound + 1
            PRINT(Str$(i) & " ", True)
        End If
        i = i + 1
    End While
  End Sub

  Function divisors(ByRef n As Single) As Single
      'find the number of divisors of an integer
      Dim r = 2!
      For i! = 2 To CInt(n) \ 2
          If (CInt(n) Mod CInt(i)) = 0 Then r = r + 1
      Next i
      divisors = r
  End Function
  
End Module
