' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim prev As Double, sgte As Double
    Dim cnt As Short
    
    cnt = 1
    INPUT(prev)
    
    sgte = (prev + 3) * .86!
    While prev <> sgte
      PRINT(Str$(sgte) & " ")
      prev = sgte
      cnt = CShort(cnt + 1)
      sgte = (prev + 3) * .86!
    End While
    PRINT("Took " & Str$(cnt) & " " & " iterations.")
    End
  End Sub

End Module
