' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB pRand ()
    
    RANDOMIZE(CInt(Timer))
    For num = 1! To 5!
        pRand()
    Next num
    End
  End Sub

  
  Sub pRand()
    Dim aleat!
      Dim randCheck(21) As Single
      Dim nr = 1!
      Do
          aleat = Int(Rnd() * 20) + 1
          If randCheck(CInt(aleat)) <> 1 Then
              randCheck(CInt(aleat)) = 1
              PRINT(Str$(aleat) & " ", True)
          End If
  
          For n! = 1 To UBound(randCheck)
              If randCheck(CInt(nr)) = 1 Then nr = nr + 1
          Next n
      Loop Until nr = 21
      PRINT()
  End Sub
  
End Module
