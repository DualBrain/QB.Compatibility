' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim m!
    
    PRINT("The pairs of amicable numbers below 20,000 are :")
    PRINT()
    
    Dim size = 18500!
    For n! = 1 To size
        m = amicable(n)
        If (m > n) And (amicable(m) = n) Then
    	PRINT_USING("##### and #####", n, m)
    	End If
    Next
    End
  End Sub

  Function amicable(ByRef nr As Single) As Single
       Dim suma = 1!
       For d! = 2 To CSng(Sqrt(nr))
  	 If (CInt(nr) Mod CInt(d)) = 0 Then suma = (suma + d) + (nr / d)
       Next
  	 amicable = suma
  End Function
  
End Module
