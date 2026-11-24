' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim cities$(10, 10)
    cities$(1, 1) = "Lagos" : cities$(1, 2) = "21.0"
    cities$(2, 1) = "Cairo" : cities$(2, 2) = "15.2"
    cities$(3, 1) = "Kinshasa-Brazzaville" : cities$(3, 2) = "11.3"
    cities$(4, 1) = "Greater Johannesburg" : cities$(4, 2) = "7.55"
    cities$(5, 1) = "Mogadishu" : cities$(5, 2) = "5.85"
    cities$(6, 1) = "Khartoum-Omdurman" : cities$(6, 2) = "4.98"
    cities$(7, 1) = "Dar Es Salaam" : cities$(7, 2) = "4.7"
    cities$(8, 1) = "Alexandria" : cities$(8, 2) = "4.58"
    cities$(9, 1) = "Abidjan" : cities$(9, 2) = "4.4"
    cities$(10, 1) = "Casablanca" : cities$(10, 2) = "3.98"
    
    For n! = 1 To UBound(cities$)
        If cities$(CInt(n), 1) = "Dar Es Salaam" Then PRINT(Str$(n - 1) & " ")
    Next n
    
    For n! = 1 To UBound(cities$)
        If Val(cities$(CInt(n), 2)) < 5! Then PRINT(cities$(CInt(n), 1)) : Exit For
    Next n
    
    For n! = 1 To UBound(cities$)
        If Left$(cities$(CInt(n), 1), 1) = "A" Then PRINT(cities$(CInt(n), 2)) : Exit For
    Next n
  End Sub

End Module
