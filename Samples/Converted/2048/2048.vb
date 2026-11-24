' Converted from QBasic by qb2vb.

Module Program

  Dim gDebug As Single
  Dim gOriginX As Single
  Dim gOriginY As Single
  Dim gTextOriginX As Single
  Dim gTextOriginY As Single
  Dim gSquareSide As Single
  Dim gGridSize As Single
  Dim gGrid(CInt(gGridSize), CInt(gGridSize)) As Single
  Dim gScore As Single

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim k$ = ""
    SCREEN(13)
    PALETTE(1, CInt(pColor(35, 33, 31)))
    PALETTE(2, CInt(pColor(46, 46, 51)))
    PALETTE(3, CInt(pColor(59, 56, 50)))
    PALETTE(4, CInt(pColor(61, 44, 30)))
    PALETTE(5, CInt(pColor(61, 37, 25)))
    PALETTE(6, CInt(pColor(62, 31, 24)))
    PALETTE(7, CInt(pColor(62, 24, 15)))
    PALETTE(8, CInt(pColor(59, 52, 29)))
    PALETTE(9, CInt(pColor(59, 51, 24)))
    PALETTE(10, CInt(pColor(59, 50, 20)))
    PALETTE(11, CInt(pColor(59, 49, 16)))
    PALETTE(12, CInt(pColor(59, 49, 12)))
    PALETTE(13, CInt(pColor(15, 15, 13)))
    PALETTE(14, CInt(pColor(23, 22, 20)))
    
    ' DIM SHARED gDebug
    ' DIM SHARED gOriginX
    ' DIM SHARED gOriginY
    ' DIM SHARED gTextOriginX
    ' DIM SHARED gTextOriginY
    ' DIM SHARED gSquareSide
    ' DIM SHARED gGridSize
    
    gGridSize = 4  ' grid size (4 -> 4x4)
    
    ' DIM SHARED gGrid(gGridSize, gGridSize)
    ' DIM SHARED gScore
    
    ' Don't touch these numbers, seriously
    
    gOriginX = 75  'pixel X of top left of grid
    gOriginY = 12  'pixel Y of top right of grid
    gTextOriginX = 11
    gTextOriginY = 3
    gSquareSide = 38  'width/height of block in pixels
    
    
    'set up all the things!
    gDebug = 0
    
    RANDOMIZE(CInt(Timer))
    CLS()
    
    start:
    initGrid()
    initGraphicGrid()
    renderGrid()
    updateScore()
    
    gScore = 0
    
    LOCATE(23, 1)
    PRINT("Move with arrow keys. (R)estart, (Q)uit")
    
    ' keyboard input loop
    Do
      Do
        k$ = INKEY$()
      Loop Until k$ <> ""
    
      Select Case k$
        Case QBChr(0) + QBChr(72)  'up
          processMove(("u"))
        Case QBChr(0) + QBChr(80)  'down
          processMove(("d"))
        Case QBChr(0) + QBChr(77)  'right
          processMove(("r"))
        Case QBChr(0) + QBChr(75)  'left
          processMove(("l"))
        Case QBChr(27)  'escape
          GoTo programEnd
        Case "q"
          GoTo programEnd
        Case "Q"
          GoTo programEnd
        Case "r"
          GoTo start
        Case "R"
          GoTo start
      End Select
    Loop
    
    programEnd:
  End Sub

  
  Sub addblock()
    Dim index!
    Dim num!
    Dim emptyCells(CInt(gGridSize * gGridSize), 2) As Single
    Dim emptyCellCount = 0!
  
    For x! = 0 To gGridSize - 1
      For y! = 0 To gGridSize - 1
        If gGrid(CInt(x), CInt(y)) = 0 Then
          emptyCells(CInt(emptyCellCount), 0) = x
          emptyCells(CInt(emptyCellCount), 1) = y
          emptyCellCount = emptyCellCount + 1
        End If
      Next y
    Next x
  
    If emptyCellCount > 0 Then
      index = Int(Rnd() * emptyCellCount)
      num = CShort(Rnd() + 1) * 2
      gGrid(CInt(emptyCells(CInt(index), 0)), CInt(emptyCells(CInt(index), 1))) = num
    End If
  
  End Sub
  
  
  Sub drawNumber(ByRef num As Single, ByRef xPos As Single, ByRef yPos As Single)
    Dim c!
    Select Case num
      Case 0 : c = 16
      Case 2 : c = 2
      Case 4 : c = 3
      Case 8 : c = 4
      Case 16 : c = 5
      Case 32 : c = 6
      Case 64 : c = 7
      Case 128 : c = 8
      Case 256 : c = 9
      Case 512 : c = 10
      Case 1024 : c = 11
      Case 2048 : c = 12
      Case 4096 : c = 13
      Case 8192 : c = 13
      Case Else : c = 13
    End Select
  
    Dim x! = ((xPos * (gSquareSide + 2)) + gOriginX) + 1
    Dim y! = ((yPos * (gSquareSide + 2)) + gOriginY) + 1
    LINE(x + 1, y + 1, (x + gSquareSide) - 1, (y + gSquareSide) - 1, CInt(c), LineOption.BF)
  
    If num > 0 Then
      LOCATE(CInt((gTextOriginY + 1) + (yPos * 5)), CInt(gTextOriginX + (xPos * 5)))
      PRINT("    ")
      LOCATE(CInt((gTextOriginY + 2) + (yPos * 5)), CInt(gTextOriginX + (xPos * 5)))
      PRINT(pad$(num))
      LOCATE(CInt((gTextOriginY + 3) + (yPos * 5)), CInt(gTextOriginX + (xPos * 5)))
      'PRINT "    "
    End If
  
  End Sub
  
  
  Function getAdjacentCell(ByRef x As Single, ByRef y As Single, ByRef d As String) As Single
  
    If ((((d = "l") And (x = 0)) Or ((d = "r") And (x = (gGridSize - 1)))) Or ((d = "u") And (y = 0))) Or ((d = "d") And (y = (gGridSize - 1))) Then
      getAdjacentCell = -1
    Else
      Select Case d
        Case "l" : getAdjacentCell = gGrid(CInt(x - 1), CInt(y))
        Case "r" : getAdjacentCell = gGrid(CInt(x + 1), CInt(y))
  
        Case "u" : getAdjacentCell = gGrid(CInt(x), CInt(y - 1))
        Case "d" : getAdjacentCell = gGrid(CInt(x), CInt(y + 1))
      End Select
    End If
  
  End Function
  
  
  'Draws the outside grid (doesn't render tiles)
  Sub initGraphicGrid()
  
    Dim gridSide! = (gSquareSide + 2) * gGridSize
  
    LINE(gOriginX, gOriginY, gOriginX + gridSide, gOriginY + gridSide, 14, LineOption.BF)  'outer square, 3 thick
    LINE(gOriginX, gOriginY, gOriginX + gridSide, gOriginY + gridSide, 1, LineOption.B)  'outer square, 3 thick
    LINE(gOriginX - 1, gOriginY - 1, (gOriginX + gridSide) + 1, (gOriginY + gridSide) + 1, 1, LineOption.B)
    LINE(gOriginX - 2, gOriginY - 2, (gOriginX + gridSide) + 2, (gOriginY + gridSide) + 2, 1, LineOption.B)
  
    For x! = (gOriginX + gSquareSide) + 2 To gOriginX + ((gSquareSide + 2) * gGridSize) Step gSquareSide + 2  ' horizontal lines
      LINE(x, gOriginY, x, gOriginY + gridSide, 1)
    Next x
  
    For y! = (gOriginY + gSquareSide) + 2 To gOriginY + ((gSquareSide + 2) * gGridSize) Step gSquareSide + 2  ' vertical lines
      LINE(gOriginX, y, gOriginX + gridSide, y, 1)
    Next y
  
  End Sub
  
  
  'Init the (data) grid with 0s
  Sub initGrid()
    For x = 0! To 3!
      For y = 0! To 3!
        gGrid(CInt(x), CInt(y)) = 0
      Next y
    Next x
  
    addblock()
    addblock()
  
  End Sub
  
  
  Sub moveBlock(ByRef sourceX As Single, ByRef sourceY As Single, ByRef targetX As Single, ByRef targetY As Single, ByRef merge As Single)
  
    If (((sourceX < 0) Or (sourceX >= gGridSize)) Or (sourceY < 0)) Or ((sourceY >= gGridSize) And (gDebug = 1)) Then
      LOCATE(0, 0)
      PRINT("moveBlock: source coords out of bounds")
    End If
  
    If (((targetX < 0) Or (targetX >= gGridSize)) Or (targetY < 0)) Or ((targetY >= gGridSize) And (gDebug = 1)) Then
      LOCATE(0, 0)
      PRINT("moveBlock: source coords out of bounds")
    End If
  
    Dim sourceSquareValue! = gGrid(CInt(sourceX), CInt(sourceY))
    Dim targetSquareValue! = gGrid(CInt(targetX), CInt(targetY))
  
    If merge = 1 Then
  
      If sourceSquareValue = targetSquareValue Then
        gGrid(CInt(sourceX), CInt(sourceY)) = 0
        gGrid(CInt(targetX), CInt(targetY)) = targetSquareValue * 2
        gScore = gScore + (targetSquareValue * 2)  ' Points!
      ElseIf gDebug = 1 Then
        LOCATE(0, 0)
        PRINT("moveBlock: Attempted to merge unequal sqs")
      End If
  
    Else
  
      If targetSquareValue = 0 Then
        gGrid(CInt(sourceX), CInt(sourceY)) = 0
        gGrid(CInt(targetX), CInt(targetY)) = sourceSquareValue
      ElseIf gDebug = 1 Then
        LOCATE(0, 0)
        PRINT("moveBlock: Attempted to move to non-empty block")
      End If
  
    End If
  
  End Sub
  
  
  Function pad$(ByRef num As Single)
    Dim strNum$ = LTrim$(Str$(num))
  
    Select Case Len(strNum$)
      Case 1 : pad$ = ("  " + strNum$) + " "
      Case 2 : pad$ = (" " + strNum$) + " "
      Case 3 : pad$ = " " + strNum$
      Case 4 : pad$ = strNum$
    End Select
  
  End Function
  
  
  Function pColor(ByRef r As Single, ByRef g As Single, ByRef b As Single) As Single
    pColor = ((r + (g * 256)) + (b * 65536))
  End Function
  
  
  Sub processMove(ByRef dir_Arg As String)
    Dim hasMoved!
    Dim y!
    Dim wasMerge!
    Dim x!
    Dim curX!
    Dim curY!
    Dim merge!
    Dim mergeDirX!
    Dim mergeDirY!
    Dim dir As String = dir_Arg
    Dim processBlock As Action
    Dim moveToObstacle As Action
    moveToObstacle = Sub()
      curX = x
      curY = y
  
      Do While getAdjacentCell(curX, curY, dir) = 0
        Select Case dir
          Case "l" : curX = curX - 1
          Case "r" : curX = curX + 1
          Case "u" : curY = curY - 1
          Case "d" : curY = curY + 1
        End Select
      Loop
    Return
    End Sub
    processBlock = Sub()
  
      merge = 0
      If gGrid(CInt(x), CInt(y)) <> 0 Then  ' have block
  
        moveToObstacle()  ' figure out where it can be moved to
        If (getAdjacentCell(curX, curY, dir) = gGrid(CInt(x), CInt(y))) And (wasMerge = 0) Then  ' obstacle can be merged with
          merge = 1
          wasMerge = 1
        Else
          wasMerge = 0
        End If
  
        If ((curX <> x) Or (curY <> y)) Or (merge = 1) Then
          mergeDirX = 0
          mergeDirY = 0
          If merge = 1 Then
            Select Case dir
              Case "l" : mergeDirX = -1
              Case "r" : mergeDirX = 1
              Case "u" : mergeDirY = -1
              Case "d" : mergeDirY = 1
            End Select
          End If
  
          moveBlock(x, y, curX + mergeDirX, curY + mergeDirY, merge)  ' move to before obstacle or merge
          hasMoved = 1
        End If
      End If
    Return
    End Sub
    ' dir can be 'l', 'r', 'u', or 'd'
  
    hasMoved = 0
  
    If dir = "l" Then
  
      For y = 0 To gGridSize - 1
        wasMerge = 0
        For x = 0 To gGridSize - 1
          processBlock()
        Next x
      Next y
  
    ElseIf dir = "r" Then
  
      For y = 0 To gGridSize - 1
        wasMerge = 0
        For x = gGridSize - 1 To 0 Step -1
          processBlock()
        Next x
      Next y
  
    ElseIf dir = "u" Then
      For x = 0 To gGridSize - 1
        wasMerge = 0
        For y = 0 To gGridSize - 1
          processBlock()
        Next y
      Next x
  
    ElseIf dir = "d" Then
      For x = 0 To gGridSize - 1
        wasMerge = 0
        For y = gGridSize - 1 To 0 Step -1
          processBlock()
        Next y
      Next x
  
    End If
  
    GoTo processMoveEnd
  
  
  
  processMoveEnd:
  
    If hasMoved = 1 Then addblock()
    renderGrid()
    updateScore()
  
    dir_Arg = dir
  End Sub
  
  
  Sub renderGrid()
    For x! = 0 To gGridSize - 1
      For y! = 0 To gGridSize - 1
        drawNumber(gGrid(CInt(x), CInt(y)), x, y)
      Next y
    Next x
  End Sub
  
  
  Sub updateScore()
    LOCATE(1, 10)
    PRINT(("Score:" + Str$(gScore)))
  End Sub
  
End Module
