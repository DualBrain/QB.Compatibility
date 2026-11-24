' Converted from QBasic by qb2vb.

Module Program

  Const pi As Single = 3.141593!
  Const radio As Single = 6372.8!

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' CONST pi = 3.141593     ' define pi
    ' CONST radio = 6372.8    ' radio de la tierra en km
    
    PRINT() : PRINT(" Distancia de Haversine:", True)
    PRINT(Str$(Haversine!(36.12!, -86.67!, 33.94!, -118.4!)) & " " & "km")
    End
  End Sub

  
  Function Haversine!(ByRef lat1!, ByRef long1!, ByRef lat2!, ByRef long2!)
      Dim deg2rad! = pi / 180  ' define grados a radianes 0.01745..
  
      Dim dLong! = deg2rad! * (long1! - long2!)
      Dim theta1! = deg2rad! * lat1!
      Dim theta2! = deg2rad! * lat2!
      Dim dx! = CSng((Cos(dLong!) * Cos(theta1!)) - Cos(theta2!))
      Dim dy! = CSng(Sin(dLong!) * Cos(theta1!))
      Dim dz! = CSng(Sin(theta1!) - Sin(theta2!))
  
      Haversine! = CSng(((Sqrt(((dx! * dx!) + (dy! * dy!)) + (dz! * dz!)) / 2) * radio) * 2)
  End Function
  
End Module
