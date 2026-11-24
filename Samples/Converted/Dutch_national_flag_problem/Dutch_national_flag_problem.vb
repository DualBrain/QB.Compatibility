' Converted from QBasic by qb2vb.

Module Program

  Const CBLUE As Short = 0
  Const CWHITE As Short = 1
  Const CRED As Short = 2
  Const N As Short = 20

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' Dutch national flag problem
    ' DECLARE SUB PrintColorSeq (T() AS INTEGER)
    ' DECLARE SUB SortByColor (T() AS INTEGER)
    
    ' CONST CBLUE% = 0, CWHITE% = 1, CRED% = 2
    ' CONST N% = 20
    Dim T(N - 1) As Short
    Dim I As Short
    
    ' Set colors
    RANDOMIZE(CInt(Timer))
    For I = 0 To CShort(N - 1)
      T(I) = CShort(Int(Rnd() * 3))
    Next I
    
    PRINT("Unsorted:")
    PrintColorSeq(T)
    SortByColor(T)
    PRINT()
    PRINT("Sorted:")
    PrintColorSeq(T)
    End
  End Sub

  
  Sub PrintColorSeq(ByRef T() As Short)
    Dim I As Short
    For I = 0 To CShort(UBound(T))
      Select Case T(I)
        Case CBLUE
          PRINT("B", True)
        Case CWHITE
          PRINT("W", True)
        Case CRED
          PRINT("R", True)
      End Select
    Next I
    PRINT()
  End Sub
  
  
  Sub SortByColor(ByRef T() As Short)
    Dim B As Short, W As Short, R As Short
    B = 0
    W = 0
    R = CShort(UBound(T))
    Do While W <= R
      Select Case T(W)
        Case CWHITE
          W = CShort(W + 1)
        Case CBLUE
          SWAP(T(B), T(W))
          B = CShort(B + 1)
          W = CShort(W + 1)
        Case CRED
          SWAP(T(W), T(R))
          R = CShort(R - 1)
      End Select
    Loop
  End Sub
  
End Module
