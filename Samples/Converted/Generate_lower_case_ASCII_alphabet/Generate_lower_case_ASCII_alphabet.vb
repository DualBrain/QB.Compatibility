' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim a$(27)
    
    For i = 1! To 26!
        a$(CInt(i)) = QBChr(CInt(i + 96))
        PRINT(a$(CInt(i)), True)
    Next i
    End
  End Sub

End Module
