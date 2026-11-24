' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    PRINT("     x     p    |     -x^p      -(x)^p      (-x)^p    -(x^p)")
    PRINT("----------------+---------------------------------------------")
    For x = -5! To 5! Step 10!
        For p = 2! To 3!
            PRINT_USING("    ##    ##    |    ####       ####       ####       ####", x, p, (-(x ^ p)), (-((x) ^ p)), ((-x) ^ p), (-(x ^ p)))
        Next p
    Next x
  End Sub

End Module
