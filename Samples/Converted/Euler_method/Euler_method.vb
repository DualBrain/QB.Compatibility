' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim temperatura!
    ' DECLARE SUB Euler (paso AS INTEGER)
    
    CLS()
    PRINT("Time    ", True)
    Dim tiempo! = 0!
    While tiempo <= 100.1!
        PRINT_USING("######", tiempo)
        tiempo = tiempo + 10!
    End While
    PRINT()
    
    PRINT("Dif eq  ", True)
    tiempo = 0!
    While tiempo <= 100.1!
        temperatura = CSng(20! + ((100! - 20!) * Exp((-.07!) * tiempo)))
        PRINT_USING("###.##", temperatura)
        tiempo = tiempo + 10!
    End While
    PRINT()
    
    Euler((2))
    Euler((5))
    Euler((10))
    End
  End Sub

  
  Sub Euler(ByRef paso As Short)
      Dim tiempo = 0!
      Dim temperatura! = 100!
      PRINT_USING("Step ## ", paso)
  
      Do While tiempo <= 100
          If (CInt(tiempo) Mod 10) = 0 Then PRINT_USING("###.##", temperatura)
          temperatura = temperatura + (paso * ((-.07!) * (temperatura - 20!)))
          tiempo = tiempo + paso
      Loop
      PRINT()
  End Sub
  
End Module
