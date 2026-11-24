' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    PRINT("First line.")
    sub1()
    PRINT("Fifth line.")
    GoTo Ending
    
    
    Ending:
    PRINT("We're just about done...")
    GoTo Finished
    
    
    Finished:
    PRINT("... with goto and gosub, thankfully.")
    End
  End Sub

  Sub sub1()
  PRINT("Second line.")
  sub2()
  PRINT("Fourth line.")
  Return
  End Sub
  
  Sub sub2()
  PRINT("Third line.")
  Return
  End Sub
  
End Module
