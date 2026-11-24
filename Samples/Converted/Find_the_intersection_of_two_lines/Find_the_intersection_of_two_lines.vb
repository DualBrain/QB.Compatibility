' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim xa!
    Dim xb!
    Dim xc!
    Dim xd!
    Dim ya!
    Dim yb!
    Dim yc!
    Dim yd!
    xa = 4 : xb = 6 : xc = 0 : xd = 10
    ya = 0 : yb = 10 : yc = 3 : yd = 7
    PRINT("The two lines are:")
    PRINT("yab =" & Str$((ya - (xa * ((yb - ya) / (xb - xa))))) & " " & "+ x*" & Str$(((yb - ya) / (xb - xa))) & " ")
    PRINT("ycd =" & Str$((yc - (xc * ((yd - yc) / (xd - xc))))) & " " & "+ x*" & Str$(((yd - yc) / (xd - xc))) & " ")
    Dim x! = ((yc - (xc * ((yd - yc) / (xd - xc)))) - (ya - (xa * ((yb - ya) / (xb - xa))))) / (((yb - ya) / (xb - xa)) - ((yd - yc) / (xd - xc)))
    PRINT("x =" & Str$(x) & " ")
    Dim y! = (ya - (xa * ((yb - ya) / (xb - xa)))) + (x * ((yb - ya) / (xb - xa)))
    PRINT("yab =" & Str$(y) & " ")
    PRINT("ycd =" & Str$(((yc - (xc * ((yd - yc) / (xd - xc)))) + (x * ((yd - yc) / (xd - xc))))) & " ")
    PRINT("intersection: (" & Str$(x) & " " & "," & Str$(y) & " " & ")")
  End Sub

End Module
