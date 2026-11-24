' Converted from QBasic by qb2vb.

Module Program

  Dim point$(32)

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim d$
    Dim heading!
    Dim ind!
    DATA("North", "North by east", "North-northeast", "Northeast by north")
    DATA("Northeast", "Northeast by east", "East-northeast", "East by north")
    DATA("East", "East by south", "East-southeast", "Southeast by east")
    DATA("Southeast", "Southeast by south", "South-southeast", "South by east")
    DATA("South", "South by west", "South-southwest", "Southwest by south")
    DATA("Southwest", "Southwest by west", "West-southwest", "West by south")
    DATA("West", "West by north", "West-northwest", "Northwest by west")
    DATA("Northwest", "Northwest by north", "North-northwest", "North by west")
    
    ' DECLARE FUNCTION compasspoint$ (h!)
    
    ' DIM SHARED point$(32)
    
    For i = 1! To 32!
        READ(d$) : point$(CInt(i)) = d$
    Next i
    
    For i = 0! To 32!
        heading = i * 11.25!
        If (CInt(i) Mod 3) = 1 Then
            heading = heading + 5.62!
        Else
            If (CInt(i) Mod 3) = 2 Then heading = heading - 5.62!
        End If
        ind = (CInt(i) Mod 32) + 1
        PRINT(Str$(ind) & " " & vbTab & compasspoint$(heading) & vbTab & Str$(heading) & " ")
    Next i
    End
    
    ' DATA  "North             ", "North by east     ", "North-northeast   ", "Northeast by north"   (DATA is listed at the start of the program)
    ' DATA  "Northeast         ", "Northeast by east ", "East-northeast    ", "East by north     "   (DATA is listed at the start of the program)
    ' DATA  "East              ", "East by south     ", "East-southeast    ", "Southeast by east "   (DATA is listed at the start of the program)
    ' DATA  "Southeast         ", "Southeast by south", "South-southeast   ", "South by east     "   (DATA is listed at the start of the program)
    ' DATA  "South             ", "South by west     ", "South-southwest   ", "Southwest by south"   (DATA is listed at the start of the program)
    ' DATA  "Southwest         ", "Southwest by west ", "West-southwest    ", "West by south     "   (DATA is listed at the start of the program)
    ' DATA  "West              ", "West by north     ", "West-northwest    ", "Northwest by west "   (DATA is listed at the start of the program)
    ' DATA  "Northwest         ", "Northwest by north", "North-northwest   ", "North by west     "   (DATA is listed at the start of the program)
  End Sub

  
  Function compasspoint$(ByRef h As Single)
      Dim x! = (h / 11.25!) + 1.5!
      If (x >= 33!) Then x = x - 32!
      compasspoint$ = point$(CInt(Int(x)))
  End Function
  
End Module
