' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim n!
    Dim n1!
    Dim e1!
    n = 1 : n1 = 1
    
    Dim e! = CSng(1 / 1)
    
    Do While e! <> e1
       e1 = e!
       e! = e! + (1 / n)
       n1 = n1 + 1
       n = n * n1
    Loop
    
    PRINT("The value of e =" & Str$(e!) & " ")
  End Sub

End Module
