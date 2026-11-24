' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim fmt$ = "#### -> &"
    PRINT_USING(fmt$, 5, BIN$(5))
    PRINT_USING(fmt$, 50, BIN$(50))
    PRINT_USING(fmt$, 9000, BIN$(9000))
  End Sub

  Function BIN$(ByRef N As Single)
      N = Abs(Int(N))
      Dim B$ = ""
      Do
          B$ = Str$(CInt(N) Mod 2) + B$
          N = Int(N / 2)
      Loop While N > 0
      BIN$ = B$
  End Function
  
End Module
