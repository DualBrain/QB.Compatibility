' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim tau!
    PRINT("The first 100 tau numbers are:")
    
    Dim n = 0!
    Dim num = 0!
    Dim limit = 100!
    Do
          n = n + 1
          tau = 0
          For m! = 1 To n
              If (CInt(n) Mod CInt(m)) = 0 Then tau = tau + 1
          Next m
          If (CInt(n) Mod CInt(tau)) = 0 Then
             num = num + 1
             If (CInt(num) Mod 10) = 1 Then PRINT()
             PRINT_USING(" ####", n)  '""; n; " ";
          End If
    Loop While num < limit
    End
  End Sub

End Module
