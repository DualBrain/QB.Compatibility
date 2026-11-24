' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim s$ = "Rosetta Code"
    
    PRINT(s$)
    PRINT(Mid$(s$, 2))  'strip first
    PRINT(Left$(s$, Len(s$) - 1))  'strip last
    PRINT(Mid$(s$, 2, Len(s$) - 2))  'strip first and last
  End Sub

End Module
