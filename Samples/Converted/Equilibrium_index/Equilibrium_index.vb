' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA(-7, 1, 5, 2, -4, 3, 0)
    DATA(2, 4, 6)
    DATA(2, 9, 2)
    DATA(1, -1, 1, -1, 1, -1, 1)
    
    ' Equilibrium index
    ' DECLARE SUB PrintArray (Nums() AS INTEGER, NL AS INTEGER)
    ' DECLARE SUB PrintIndicesOfTrue (Bools() AS INTEGER, NL AS INTEGER)
    ' DECLARE SUB GetIndices (Nums() AS INTEGER, Result() AS INTEGER)
    Dim I As Short
    Dim X1(6) As Short, X1Result(6) As Short
    For I = 0 To 6
       READ(X1(I))
    Next I
    ' DATA -7,  1, 5,  2, -4,  3, 0   (DATA is listed at the start of the program)
    GetIndices(X1, X1Result)
    Dim X2(2) As Short, X2Result(2) As Short
    For I = 0 To 2
       READ(X2(I))
    Next I
    ' DATA  2,  4, 6   (DATA is listed at the start of the program)
    GetIndices(X2, X2Result)
    Dim X3(2) As Short, X3Result(2) As Short
    For I = 0 To 2
       READ(X3(I))
    Next I
    ' DATA 2,  9, 2   (DATA is listed at the start of the program)
    GetIndices(X3, X3Result)
    Dim X4(6) As Short, X4Result(6) As Short
    For I = 0 To 6
       READ(X4(I))
    Next I
    ' DATA 1, -1, 1, -1,  1 ,-1, 1   (DATA is listed at the start of the program)
    GetIndices(X4, X4Result)
    PRINT("Results:")
    PRINT()
    PRINT("X1:", True)
    PrintArray(X1, 1)
    PRINT("Eqs:", True)
    PrintIndicesOfTrue(X1Result, 1)
    PRINT()
    PRINT("X2:", True)
    PrintArray(X2, 1)
    PRINT("Eqs:", True)
    PrintIndicesOfTrue(X2Result, 1)
    PRINT()
    PRINT("X3:", True)
    PrintArray(X3, 1)
    PRINT("Eqs:", True)
    PrintIndicesOfTrue(X3Result, 1)
    PRINT()
    PRINT("X4:", True)
    PrintArray(X4, 1)
    PRINT("Eqs:", True)
    PrintIndicesOfTrue(X4Result, 1)
    End
  End Sub

  
  Sub GetIndices(ByRef Nums() As Short, ByRef Result() As Short)
  If (LBound(Nums) <> LBound(Result)) Or (UBound(Nums) <> UBound(Result)) Then
     PRINT("Range of indices in both arrays should be the same")
     End
  End If
  Dim LeftSum As Short, RightSum As Short
  Dim I As Short
  LeftSum = 0
  RightSum = 0
  For I = CShort(LBound(Nums)) To CShort(UBound(Nums))
    RightSum = RightSum + Nums(I)
  Next I
  For I = CShort(LBound(Nums)) To CShort(UBound(Nums))
    RightSum = RightSum - Nums(I)
    Result(I) = CShort((LeftSum = RightSum))
    LeftSum = LeftSum + Nums(I)
  Next I
  End Sub
  
  
  Sub PrintArray(ByRef Nums() As Short, ByRef NL As Short)
  For I! = LBound(Nums) To UBound(Nums)
    PRINT(Str$(Nums(CInt(I))) & " ", True)
  Next I
  If NL <> 0 Then PRINT()
  End Sub
  
  
  Sub PrintIndicesOfTrue(ByRef Bools() As Short, ByRef NL As Short)
  For I! = LBound(Bools) To UBound(Bools)
    If Bools(CInt(I)) <> 0 Then PRINT(Str$(I) & " ", True)
  Next I
  If NL <> 0 Then PRINT()
  End Sub
  
End Module
