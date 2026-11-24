' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim n = 4!
    Dim a(CInt(n)) As Single
    For i! = 1 To UBound(a)
        a(CInt(i)) = i
        PRINT("[", True)
        For j! = 1 To i
            PRINT(Str$(a(CInt(j))) & " " & " ", True)
        Next j
        PRINT("] = " & Str$(getLantern(a)) & " ")
    Next i
    End
  End Sub

  Function getLantern(ByRef arr() As Single) As Single
      Dim res = 0!
      For i! = 1 To UBound(arr)
          If arr(CInt(i)) <> 0 Then
              arr(CInt(i)) = arr(CInt(i)) - 1
              res = res + getLantern(arr)
              arr(CInt(i)) = arr(CInt(i)) + 1
          End If
      Next i
      If res = 0 Then res = 1
      getLantern = res
  End Function
  
End Module
