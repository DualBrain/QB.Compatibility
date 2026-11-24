' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    perms((3))
    PRINT()
    perms((4))
  End Sub

  Sub perms(ByRef n As Single)
    Dim i!
    Dim k!
      Dim p(CInt((n + 1) * 4)) As Single
      For i = 1 To n
      p(CInt(i)) = -i
      Next i
      Dim s = 1!
  
      Do
      PRINT("Perm: (", True)
      For i = 1 To n
          PRINT(Str$(Abs(p(CInt(i)))) & " " & "", True)
      Next i
      PRINT(") Sign: " & Str$(s) & " ")
  
      k = 0
      For i = 2 To n
          If ((p(CInt(i)) < 0) And (Abs(p(CInt(i))) > Abs(p(CInt(i - 1))))) And (Abs(p(CInt(i))) > Abs(p(CInt(k)))) Then k = i
      Next i
      For i = 1 To n - 1
          If ((p(CInt(i)) > 0) And (Abs(p(CInt(i))) > Abs(p(CInt(i + 1))))) And (Abs(p(CInt(i))) > Abs(p(CInt(k)))) Then k = i
      Next i
      If k <> 0 Then
          For i = 1 To n  'reverse elements > k
          If Abs(p(CInt(i))) > Abs(p(CInt(k))) Then p(CInt(i)) = -p(CInt(i))
          Next i
          'if p(k) < 0 then i = k-1 else i = k+1
          i = k + Sign(p(CInt(k)))
          SWAP(p(CInt(k)), p(CInt(i)))
          'temp = p(k)
          'p(k) = p(i)
          'p(i) = temp
          s = -s
      End If
      Loop Until k = 0
  End Sub
  
End Module
