' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB rep (func AS STRING, c AS INTEGER)
    ' DECLARE SUB proc ()
    
    rep("proc", 5)
    PRINT("Loop Ended")
  End Sub

  
  Sub proc()
  	PRINT(" Inside loop")
  End Sub
  
  
  Sub rep(ByRef func As String, ByRef c As Short)
  	For i! = 1 To c
  		proc()
  	Next
  End Sub
  
End Module
