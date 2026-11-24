' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION ack! (m!, n!)
  End Sub

  
  Function ack(ByRef m!, ByRef n!) As Single
         If m! = 0 Then ack = n! + 1
  
         If (m! > 0) And (n! = 0) Then
                 ack = ack(m! - 1, 1)
         End If
         If (m! > 0) And (n! > 0) Then
                 ack = ack(m! - 1, ack(m!, n! - 1))
         End If
  End Function
  
End Module
