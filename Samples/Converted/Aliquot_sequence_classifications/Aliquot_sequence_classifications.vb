' Converted from QBasic by qb2vb.

Module Program

  Const limite As Integer = 10000000

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 28, 496)
    DATA(220, 1184, 12496, 790, 909, 562, 1064, 1488)
    
    ' DECLARE FUNCTION PDtotal! (n!)
    ' DECLARE SUB PrintAliquotClassifier (K!)
    CLS()
    ' CONST limite = 10000000
    
    Dim nums(22) As Single
    ' DATA 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 28, 496   (DATA is listed at the start of the program)
    ' DATA 220, 1184, 12496, 790, 909, 562, 1064, 1488   (DATA is listed at the start of the program)
    
    For n! = 1 To UBound(nums)
        READ(nums(CInt(n)))
        PRINT("Number" & Str$(nums(CInt(n))) & " " & " :", True)
        PrintAliquotClassifier((nums(CInt(n))))
    Next n
    
    PRINT("Program normal end.")
    End
  End Sub

  
  Function PDtotal(ByRef n As Single) As Single
      Dim total = 0!
      For y! = 2 To n
          If (CInt(n) Mod CInt(y)) = 0 Then total = total + (n / y)
      Next y
      PDtotal = total
  End Function
  
  
  Sub PrintAliquotClassifier(ByRef K As Single)
    Dim longit!
    Dim n!
    Dim clase!
    Dim priorn!
    Dim inc!
      longit = 52 : n = K : clase = 0 : priorn = 0 : inc = 0
      Dim Aseq(CInt(longit)) As Single
  
      For element! = 2 To longit
          Aseq(CInt(element)) = PDtotal(n)
          PRINT(Str$(Aseq(CInt(element))) & " " & " ", True)
          COLOR(3)
          Select Case Aseq(CInt(element))
          Case 0
              PRINT(" Terminating") : clase = 1 : Exit For
          Case CInt(K) And CShort(element = 2)
              PRINT(" Perfect") : clase = 2 : Exit For
          Case CInt(K) And CShort(element = 3)
              PRINT(" Amicable") : clase = 3 : Exit For
          Case CInt(K) And CShort(element > 3)
              PRINT(" Sociable") : clase = 4 : Exit For
          Case CSng((Aseq(CInt(element)) <> K) And (Aseq(CInt(element - 1)) = Aseq(CInt(element))))
              PRINT(" Aspiring") : clase = 5 : Exit For
          Case CSng((Aseq(CInt(element)) <> K) And (Aseq(CInt(element - 2)) = Aseq(CInt(element))))
              PRINT(" Cyclic") : clase = 6 : Exit For
          End Select
          COLOR(7)
          n = Aseq(CInt(element))
          If n > priorn Then priorn = n : inc = inc + 1 Else inc = 0 : priorn = 0
          If (inc = 11) Or (n > limite) Then Exit For
      Next element
      If clase = 0 Then COLOR(12) : PRINT(" non-terminating")
      COLOR(7)
  End Sub
  
End Module
