' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim pnum!
    PRINT("the first 15 terms of the sequence are:")
    For n = 1! To 15!
        For m = 1! To 4100!
            pnum = 0
            For p = 1! To 4100!
                If (CInt(m) Mod CInt(p)) = 0 Then pnum = pnum + 1
            Next p
            If pnum = n Then
               PRINT(Str$(m) & " ", True)
               Exit For
            End If
        Next m
    Next n
  End Sub

End Module
