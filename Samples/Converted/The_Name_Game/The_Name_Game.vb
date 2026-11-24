' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB TheGameName (nombre AS STRING)
    
    Dim listanombres(5) As String
    listanombres(0) = "Gary"
    listanombres(1) = "EARL"
    listanombres(2) = "billy"
    listanombres(3) = "FeLiX"
    listanombres(4) = "Mary"
    listanombres(5) = "ShirlEY"
    
    For i! = 0 To UBound(listanombres)
        TheGameName((listanombres(CInt(i))))
    Next i
  End Sub

  
  Sub TheGameName(ByRef nombre As String)
      Dim x As String
      x = LCase$(nombre)
      x = UCase$(Mid$(x, 1, 1)) + (Mid$(x, 2, Len(x) - 1))
      Dim x0 As String
      x0 = UCase$(Mid$(x, 1, 1))
  
      Dim y As String
      If ((((x0 = "A") Or (x0 = "E")) Or (x0 = "I")) Or (x0 = "O")) Or (x0 = "U") Then
  	y = LCase$(x)
      Else
  	y = Mid$(x, 2)
      End If
  
      Dim b As String
      b = "b" + y
  	Dim f As String
  	f = "f" + y
  	Dim m As String
  	m = "m" + y
  
      If x0 = "B" Then
  	b = y
      ElseIf x0 = "F" Then
  	f = y
      ElseIf x0 = "M" Then
  	m = y
      End If
  
      PRINT(((((x + ", ") + x) + ", bo-") + b))
      PRINT(("Banana-fana fo-" + f))
      PRINT(("Fee-fi-mo-" + m))
      PRINT(((x + "!") + QBChr(10)))
  End Sub
  
End Module
