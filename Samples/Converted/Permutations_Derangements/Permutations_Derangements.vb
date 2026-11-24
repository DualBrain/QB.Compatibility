' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim n! = 4
    Dim subfac!(7)
    
    Subfactorial(subfac!)
    
    PRINT("permutations derangements for n = " & Str$(n!) & " ")
    Dim i! = permsderange(n!, 0)
    PRINT("count returned =" & Str$(i!) & " " & " , !" & Str$(n!) & " " & " calculated =" & Str$(subfac!(CInt(n!))) & " ")
    
    PRINT()
    PRINT("count  counted subfactorial")
    PRINT("---------------------------")
    For i! = 0 To 7
        PRINT_USING(" ###: ########     ########", i!, permsderange(i!, 1), subfac!(CInt(i!)))
    Next i!
  End Sub

  ' Heap's algorithm non-recursive
  Function permsderange(ByRef n!, ByRef flag!) As Single
    Dim j!
    Dim i!
    Dim count!
    Dim c1!
      If n! = 0 Then permsderange = 1
  
      Dim a!(CInt(n!)), c!(CInt(n!))
  
      For j = 0 To n! - 1 : a!(CInt(j)) = j : Next j
  
      While i < n!
          If c!(CInt(i)) < i Then
              If (CInt(i) And 1) = 0 Then
                  SWAP(a!(0), a!(CInt(i)))
              Else
                  SWAP(a!(CInt(c!(CInt(i)))), a!(CInt(i)))
              End If
              For j = 0 To n! - 1
                  If a!(CInt(j)) = j Then j = 99
              Next j
              If j < 99 Then
                  count = count + 1
                  If flag! = 0 Then
                      c1 = c1 + 1
                      For j = 0 To n! - 1
                          PRINT(Str$(a!(CInt(j))) & " ", True)
                      Next j
                      If c1 > 12 Then
                          PRINT() : c1 = 0
                      Else
                          PRINT()
                      End If
                  End If
              End If
              c!(CInt(i)) = c!(CInt(i)) + 1
              i = 0
          Else
              c!(CInt(i)) = 0
              i = i + 1
          End If
      End While
      If (flag! = 0) And (c1 <> 0) Then PRINT()
      permsderange = count
  End Function
  
  
  Sub Subfactorial(ByRef a!())
    Dim num!
      For i! = 0 To UBound(a!)
          num = num * i
          If (CInt(i) And 1) = 1 Then
              num = num - 1
          Else
              num = num + 1
          End If
          a!(CInt(i)) = num
      Next i
  End Sub
  
End Module
