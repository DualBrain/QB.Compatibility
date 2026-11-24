' Converted from QBasic by qb2vb.

Module Program

  Dim d(16) As Short
  Dim dbp$(16)
  Dim sh(3) As Short

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB printPuzzle ()
    ' DECLARE SUB buildBoard (level AS INTEGER)
    ' DECLARE FUNCTION introAndLevel! ()
    ' DECLARE FUNCTION isMoveValid! (piece AS INTEGER, piecePos AS INTEGER, emptyPos AS INTEGER)
    ' DECLARE FUNCTION piecePosition! (piece AS INTEGER)
    ' DECLARE FUNCTION isPuzzleComplete! ()
    
    ' DIM SHARED d(16) AS INTEGER
    ' DIM SHARED dbp$(16) ' Board pieces
    ' DIM SHARED sh(3) AS INTEGER
    Dim level As Short, x As Short, y As Short, z As Short
    
    ' Main Program
    level = CShort(introAndLevel())
    buildBoard((level))
    Do
        printPuzzle()
        PRINT("To move a piece, enter its number (0 to quit): ")
        INPUT(x)
        If x = 0 Then End
    
        While isMoveValid(x, y, z) = 0
            PRINT("Wrong move.")
            printPuzzle()
            PRINT("To move a piece, enter its number (0 to quit): ")
            INPUT(x)
            If x = 0 Then End
        End While
    
        d(z) = x
        d(y) = 0
        CLS()
    Loop Until isPuzzleComplete() <> 0
    PRINT("YOU WON!")
    End
  End Sub

  
  Sub buildBoard(ByRef level As Short)
    Dim x!
    Dim r!
      ' Set pieces in correct order first
      For p = 1! To 15!
          d(CInt(p)) = CShort(p)
      Next p
      d(16) = 0  ' 0 = empty piece/slot
      Dim z = 16!  ' z = empty position
  
      PRINT()
      PRINT("Shuffling pieces", True)
      RANDOMIZE(CInt(Timer))
      For n! = 1 To sh(level)
          PRINT(".", True)
          Do
              x = Int(Rnd() * 4) + 1
              r = 0
              Select Case x
                  Case 1 : r = z - 4
                  Case 2 : r = z + 4
                  Case 3 : If (CInt((z - 1)) Mod 4) <> 0 Then r = z - 1
                  Case 4 : If (CInt(z) Mod 4) <> 0 Then r = z + 1
              End Select
          Loop While (r < 1) Or (r > 16)
          d(CInt(z)) = d(CInt(r))
          z = r
          d(CInt(z)) = 0
      Next n
      CLS()
  End Sub
  
  
  Function introAndLevel() As Single
    Dim level!
      CLS()
      sh(1) = 10
      sh(2) = 50
      sh(3) = 100
      PRINT("15 PUZZLE GAME") : PRINT() : PRINT()
      PRINT("Please enter level of difficulty,")
      Do
          PRINT("1 (easy), 2 (medium) or 3 (hard): ", True)
          INPUT(level)
      Loop While (level < 1) Or (level > 3)
      introAndLevel = level
  End Function
  
  
  Function isMoveValid(ByRef piece As Short, ByRef piecePos As Short, ByRef emptyPos As Short) As Single
      Dim mv = 0!
      If (piece >= 1) And (piece <= 15) Then
          piecePos = CShort(piecePosition(piece))
          emptyPos = CShort(piecePosition(0))
          If ((((piecePos - 4) = emptyPos) Or ((piecePos + 4) = emptyPos)) Or (((piecePos - 1) = emptyPos) And ((emptyPos Mod 4) <> 0))) Or (((piecePos + 1) = emptyPos) And ((piecePos Mod 4) <> 0)) Then
              mv = 1
          End If
      End If
      isMoveValid = mv
  End Function
  
  
  Function isPuzzleComplete() As Single
      Dim pc = 0!
      Dim p = 1!
      While (p < 16) And (d(CInt(p)) = p)
          p = p + 1
      End While
      If p = 16 Then pc = 1
      isPuzzleComplete = pc
  End Function
  
  
  Function piecePosition(ByRef piece As Short) As Single
      Dim p = 1!
      While d(CInt(p)) <> piece
          p = p + 1
          If p > 16 Then
              PRINT("UH OH!")
              End
          End If
      End While
      piecePosition = p
  End Function
  
  
  Sub printPuzzle()
      For p = 1! To 16!
          If d(CInt(p)) = 0 Then
              dbp$(CInt(p)) = "     "
          Else
              dbp$(CInt(p)) = Right$("  " + LTrim$(Str$(d(CInt(p)))), 3) + "  "
          End If
      Next p
  
      PRINT("+-----+-----+-----+-----+")
      PRINT("|" & dbp$(1) & "|" & dbp$(2) & "|" & dbp$(3) & "|" & dbp$(4) & "|")
      PRINT("+-----+-----+-----+-----+")
      PRINT("|" & dbp$(5) & "|" & dbp$(6) & "|" & dbp$(7) & "|" & dbp$(8) & "|")
      PRINT("+-----+-----+-----+-----+")
      PRINT("|" & dbp$(9) & "|" & dbp$(10) & "|" & dbp$(11) & "|" & dbp$(12) & "|")
      PRINT("+-----+-----+-----+-----+")
      PRINT("|" & dbp$(13) & "|" & dbp$(14) & "|" & dbp$(15) & "|" & dbp$(16) & "|")
      PRINT("+-----+-----+-----+-----+")
  End Sub
  
End Module
