' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB showarray (vect3!())
    CLS()
    Dim vect1(2) As Single
    vect1(1) = 5 : vect1(2) = 7
    Dim vect2(2) As Single
    vect2(1) = 2 : vect2(2) = 3
    Dim vect3(UBound(vect1)) As Single
    For n! = 1 To UBound(vect1)
        vect3(CInt(n)) = vect1(CInt(n)) + vect2(CInt(n))
    Next n
    PRINT("[" & Str$(vect1(1)) & "," & Str$(vect1(2)) & " ] + [" & (Str$(vect2(1)) + ",") & Str$(vect2(2)) & " ] = ", True)
    showarray(vect3)
    For n! = 1 To UBound(vect1)
        vect3(CInt(n)) = vect1(CInt(n)) - vect2(CInt(n))
    Next n
    PRINT("[" & Str$(vect1(1)) & "," & Str$(vect1(2)) & " ] - [" & (Str$(vect2(1)) + ",") & Str$(vect2(2)) & " ] = ", True)
    showarray(vect3)
    For n! = 1 To UBound(vect1)
        vect3(CInt(n)) = vect1(CInt(n)) * 11
    Next n
    PRINT("[" & Str$(vect1(1)) & "," & Str$(vect1(2)) & " ] *  11      = ", True)
    showarray(vect3)
    For n! = 1 To UBound(vect1)
        vect3(CInt(n)) = vect1(CInt(n)) / 2
    Next n
    PRINT("[" & Str$(vect1(1)) & "," & Str$(vect1(2)) & " ] /  2       = ", True)
    showarray(vect3)
    End
  End Sub

  
  Sub showarray(ByRef vect3() As Single)
      PRINT("[", True)
      Dim svect$ = ""
      For n! = 1 To UBound(vect3)
          svect$ = (svect$ + Str$(vect3(CInt(n)))) + ","
      Next n
      svect$ = Left$(svect$, Len(svect$) - 1)
      PRINT(svect$, True)
      PRINT(" ]")
  End Sub
  
End Module
