' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i!
    Dim j!
    INPUT("Enter two positive integers, separated by a comma", i, j)
    Dim array(CInt(i), CInt(j)) As Single
    array(CInt(i), CInt(j)) = i * j
    PRINT("a(" & Str$(i) & "," & Str$(j) & " ) = " & Str$(array(CInt(i), CInt(j))) & " ")
    Array.Clear(array, 0, array.Length)
  End Sub

End Module
