' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim hexi$
    ' DECLARE FUNCTION isPrime! (num!)
    ' DECLARE FUNCTION Reverse$ (text$)
    
    Dim inicio = 2!
    Dim final = 499!
    Dim cnt = 0!
    
    For i! = inicio To final
        hexi$ = Hex$(i)
    
        If (CInt(isPrime(i)) And CShort(hexi$ = Reverse$(hexi$))) <> 0 Then
            cnt = cnt + 1
            PRINT(hexi$ & "  ", True)
        End If
    Next i
    
    PRINT(QBChr(10))
    PRINT("Encontrados" & Str$(cnt) & " " & "primos palindromicos entre" & Str$(inicio) & " " & "y" & Str$(final) & " ")
    End
  End Sub

  
  Function isPrime(ByRef num As Single) As Single
      For i! = 2 To CSng(Sqrt(num))
          If (CInt(num) Mod CInt(i)) = 0 Then
              isPrime = 0
              Exit Function
          End If
      Next i
      isPrime = -1
  End Function
  
  
  Function Reverse$(ByRef text$)
      Dim text2$ = text$
      Dim lt! = Len(text$)
      For x! = 0 To (CInt(lt) \ 2) - 1
          Mid(text2$, CInt(x + 1), 1) = Mid$(text$, CInt(lt - x), 1)
          Mid(text2$, CInt(lt - x), 1) = Mid$(text$, CInt(x + 1), 1)
      Next x
      Reverse$ = text2$
  End Function
  
End Module
