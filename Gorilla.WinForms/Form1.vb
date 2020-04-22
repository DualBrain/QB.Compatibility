﻿Option Explicit On
Option Strict Off 'On
Option Infer On

Imports QB.Core
Imports QB.Multimedia
Imports QB.Devices
Imports QB.Development
Imports QB.LineOption
Imports QB.PutOption
Imports QB.Video

Imports VB = Microsoft.VisualBasic

Imports System.Math
Imports QB

Public Class Form1

  '                         Q B a s i c   G o r i l l a s
  '
  '                   Copyright (C) Microsoft Corporation 1990
  '
  ' Your mission is to hit your opponent with the exploding banana
  ' by varying the angle and power of your throw, taking into account
  ' wind speed, gravity, and the city skyline.
  '
  ' Speed of this game is determined by the constant SPEEDCONST.  If the
  ' program is too slow or too fast adjust the "CONST SPEEDCONST = 500" line
  ' below.  The larger the number the faster the game will go.
  '
  ' To run this game, press Shift+F5.
  '
  ' To exit QBasic, press Alt, F, X.
  '
  ' To get help on a BASIC keyword, move the cursor to the keyword and press
  ' F1 or click the right mouse button.
  '

  'Set default data type to integer for faster game play
  REM DEFINT A-Z

  'Sub Declarations
  REM Declare Sub DoSun (Mouth)
  REM Declare Sub SetScreen ()
  REM Declare Sub EndGame ()
  REM Declare Sub Center (Row, Text$)
  REM Declare Sub Intro ()
  REM Declare Sub SparklePause ()
  REM Declare Sub GetInputs (Player1$, Player2$, NumGames)
  REM Declare Sub PlayGame (Player1$, Player2$, NumGames)
  REM Declare Sub DoExplosion (x#, y#)
  REM Declare Sub MakeCityScape (BCoor() As ANY)
  REM Declare Sub PlaceGorillas (BCoor() As ANY)
  REM Declare Sub UpdateScores (Record(), PlayerNum, Results)
  REM Declare Sub DrawGorilla (x, y, arms)
  REM Declare Sub GorillaIntro (Player1$, Player2$)
  REM Declare Sub Rest (t#)
  REM Declare Sub VictoryDance (Player)
  REM Declare Sub ClearGorillas ()
  REM Declare Sub DrawBan (xc#, yc#, r, bc)
  REM Declare Function Scl (n!)
  REM Declare Function GetNum# (Row, Col)
  REM Declare Function DoShot (PlayerNum, x, y)
  REM Declare Function ExplodeGorilla (x#, y#)
  REM Declare Function Getn# (Row, Col)
  REM Declare Function PlotShot (StartX, StartY, Angle#, Velocity, PlayerNum)
  REM Declare Function CalcDelay! ()

  'Make all arrays Dynamic
  '$DYNAMIC

  'User-Defined TYPEs
  Structure XYPoint
    Public XCoor As Integer
    Public YCoor As Integer
  End Structure

  'Constants
  Const SPEEDCONST = 500
  REM Const True = -1
  REM Const False = Not True
  Const HITSELF = 1
  Const BACKATTR = 0
  Const OBJECTCOLOR = 1
  Const WINDOWCOLOR = 14
  Const SUNATTR = 3
  Const SUNHAPPY = False
  Const SUNSHOCK = True
  Const RIGHTUP = 1
  Const LEFTUP = 2
  Const ARMSDOWN = 3

  'Global Variables
  Private GorillaX(0 To 2) ' base1  'Location of the two gorillas
  Private GorillaY(0 To 2) ' base1
  Private LastBuilding%

  Private pi#
  Private LBan&(8), RBan&(8), UBan&(8), DBan&(8) 'Graphical picture of banana
  Private GorD& '(120)        'Graphical picture of Gorilla arms down
  Private GorL& '(120)        'Gorilla left arm raised
  Private GorR& '(120)        'Gorilla right arm raised

  Private gravity#
  Private Wind%

  'Screen Mode Variables
  Private ScrHeight%
  Private ScrWidth%
  Private Mode%
  Private MaxCol%

  'Screen Color Variables
  Private ExplosionColor%
  Private SunColor%
  Private qbBackColor%
  Private SunHit%

  Private SunHt%
  Private GHeight%
  Private MachSpeed As Single

  '  DEF FnRan(x) = Int(Rnd(1) * x) + 1
  Private Function FnRan(x%)
    Return Int(Rnd(1) * x) + 1
  End Function

  Sub Todo()

    ' The following was at the very start of the program...

    REM DEF SEG = 0                         ' Set NumLock to ON
    Dim KeyFlags = PEEK(1047)
    If (KeyFlags And 32) = 0 Then
      POKE(1047, KeyFlags Or 32)
    End If
    REM DEF SEG

    '------

    REM DEF SEG = 0                         ' Restore NumLock state
    POKE(1047, KeyFlags)
    REM DEF SEG
    End

    'CGABanana:
    '    'BananaLeft
    '    DATA(327686, -252645316, 60)
    '    'BananaDown
    '    DATA(196618, -1057030081, 49344)
    '    'BananaUp
    '    DATA(196618, -1056980800, 63)
    '    'BananaRight
    '    DATA(327686, 1010580720, 240)

    'EGABanana:
    '    'BananaLeft
    '    DATA(458758, 202116096, 471604224, 943208448, 943208448, 943208448, 471604224, 202116096, 0)
    '    'BananaDown
    '    DATA(262153, -2134835200, -2134802239, -2130771968, -2130738945, 8323072, 8323199, 4063232, 4063294)
    '    'BananaUp
    '    DATA(262153, 4063232, 4063294, 8323072, 8323199, -2130771968, -2130738945, -2134835200, -2134802239)
    '    'BananaRight
    '    DATA(458758, -1061109760, -522133504, 1886416896, 1886416896, 1886416896, -522133504, -1061109760, 0)

    'InitVars:
    '    pi# = 4 * Atan(1.0#)

    '    'This is a clever way to pick the best graphics mode available
    '    On Error GoTo ScreenModeError
    '    Mode = 9
    '    SCREEN(Mode)
    '    On Error GoTo PaletteError
    '    If Mode = 9 Then PALETTE(4, 0)   'Check for 64K EGA
    '    On Error GoTo 0

    '    MachSpeed = CalcDelay()

    '    If Mode = 9 Then
    '      ScrWidth = 640
    '      ScrHeight = 350
    '      GHeight = 25
    '      RESTORE EGABanana
    '      ReDim LBan&(8), RBan&(8), UBan&(8), DBan&(8)

    '      For i = 0 To 8
    '        READ(LBan&(i))
    '      Next i

    '      For i = 0 To 8
    '        READ(DBan&(i))
    '      Next i

    '      For i = 0 To 8
    '        READ(UBan&(i))
    '      Next i

    '      For i = 0 To 8
    '        READ(RBan&(i))
    '      Next i

    '      SunHt = 39

    '    Else

    '      ScrWidth = 320
    '      ScrHeight = 200
    '      GHeight = 12
    '      RESTORE CGABanana
    '      ReDim LBan&(2), RBan&(2), UBan&(2), DBan&(2)
    '      REM ReDim GorL&(20), GorD&(20), GorR&(20)

    '      For i = 0 To 2
    '        READ(LBan&(i))
    '      Next i
    '      For i = 0 To 2
    '        READ(DBan&(i))
    '      Next i
    '      For i = 0 To 2
    '        READ(UBan&(i))
    '      Next i
    '      For i = 0 To 2
    '        READ(RBan&(i))
    '      Next i

    '      MachSpeed = MachSpeed * 1.3
    '      SunHt = 20
    '    End If
    '    Return

ScreenModeError:
    If Mode = 1 Then
      CLS()
      LOCATE(10, 5)
      PRINT("Sorry, you must have CGA, EGA color, or VGA graphics to play GORILLA.BAS")
      End
    Else
      Mode = 1
      Resume
    End If

PaletteError:
    Mode = 1            '64K EGA cards will run in CGA mode.
    Resume Next

  End Sub

  REM $STATIC
  'CalcDelay:
  '  Checks speed of the machine.
  Function CalcDelay!()

    Dim s! = QBTimer()
    Dim i!

    Do
      i! = i! + 1
    Loop Until QBTimer() - s! >= 0.5
    CalcDelay! = i!

  End Function

  ' Center:
  '   Centers and prints a text string on a given row
  ' Parameters:
  '   Row - screen row number
  '   Text$ - text to be printed
  '
  Sub Center(ByRef Row%, ByRef Text$)
    Dim Col = MaxCol \ 2
    LOCATE(Row, Col - (Len(Text$) / 2 + 0.5))
    PRINT(Text$, True)
  End Sub

  ' DoExplosion:
  '   Produces explosion when a shot is fired
  ' Parameters:
  '   X#, Y# - location of explosion
  '
  Sub DoExplosion(ByRef x#, ByRef y#)
    PLAY("MBO0L32EFGEFDC")
    Dim Radius = ScrHeight / 50
    Dim Inc#
    If Mode = 9 Then Inc# = 0.5 Else Inc# = 0.41
    For c# = 0 To Radius Step Inc#
      CIRCLE(x#, y#, c#, ExplosionColor)
    Next c#
    For c# = Radius To 0 Step (-1 * Inc#)
      CIRCLE(x#, y#, c#, BACKATTR)
      For i = 1 To 100
      Next i
      Rest(0.005)
    Next c#
  End Sub

  ' DoShot:
  '   Controls banana shots by accepting player input and plotting
  '   shot angle
  ' Parameters:
  '   PlayerNum - Player
  '   x, y - Player's gorilla position
  '
  Function DoShot(ByRef PlayerNum, ByRef x, ByRef y)

    'Input shot
    Dim LocateCol%
    If PlayerNum = 1 Then
      LocateCol = 1
    Else
      If Mode = 9 Then
        LocateCol = 66
      Else
        LocateCol = 26
      End If
    End If

    LOCATE(2, LocateCol)
    PRINT("Angle:", True)
    Dim Angle# = GetNum#(2, LocateCol + 7)

    LOCATE(3, LocateCol)
    PRINT("Velocity:", True)
    Dim Velocity = GetNum#(3, LocateCol + 10)

    If PlayerNum = 2 Then
      Angle# = 180 - Angle#
    End If

    'Erase input
    For i = 1 To 4
      LOCATE(i, 1)
      PRINT(Space(30 \ (80 \ MaxCol)), True)
      LOCATE(i, (50 \ (80 \ MaxCol)))
      PRINT(Space(30 \ (80 \ MaxCol)), True)
    Next

    SunHit = False
    Dim PlayerHit = PlotShot(x, y, Angle#, Velocity, PlayerNum)
    If PlayerHit = 0 Then
      DoShot = False
    Else
      DoShot = True
      If PlayerHit = PlayerNum Then PlayerNum = 3 - PlayerNum
      VictoryDance(PlayerNum)
    End If

  End Function

  ' DoSun:
  '   Draws the sun at the top of the screen.
  ' Parameters:
  '   Mouth - If TRUE draws "O" mouth else draws a smile mouth.
  '
  Sub DoSun(ByRef Mouth)

    Dim x%, y%

    'set position of sun
    x = ScrWidth \ 2 : y = Scl(25)

    'clear old sun
    LINE(x - Scl(22), y - Scl(18), x + Scl(22), y + Scl(18), BACKATTR, BF)

    'draw new sun:
    'body
    CIRCLE(x, y, Scl(12), SUNATTR)
    QbPaint(x, y, SUNATTR)

    'rays
    LINE(x - Scl(20), y, x + Scl(20), y, SUNATTR)
    LINE(x, y - Scl(15), x, y + Scl(15), SUNATTR)

    LINE(x - Scl(15), y - Scl(10), x + Scl(15), y + Scl(10), SUNATTR)
    LINE(x - Scl(15), y + Scl(10), x + Scl(15), y - Scl(10), SUNATTR)

    LINE(x - Scl(8), y - Scl(13), x + Scl(8), y + Scl(13), SUNATTR)
    LINE(x - Scl(8), y + Scl(13), x + Scl(8), y - Scl(13), SUNATTR)

    LINE(x - Scl(18), y - Scl(5), x + Scl(18), y + Scl(5), SUNATTR)
    LINE(x - Scl(18), y + Scl(5), x + Scl(18), y - Scl(5), SUNATTR)

    'mouth
    If Mouth Then  'draw "o" mouth
      CIRCLE(x, y + Scl(5), Scl(2.9), 0)
      QbPaint(x, y + Scl(5), 0, 0)
    Else           'draw smile
      CIRCLE(x, y, Scl(8), 0, (210 * pi# / 180), (330 * pi# / 180))
    End If

    'eyes
    CIRCLE(x - 3, y - 2, 1, 0)
    CIRCLE(x + 3, y - 2, 1, 0)
    PSET(x - 3, y - 2, 0)
    PSET(x + 3, y - 2, 0)

  End Sub

  'DrawBan:
  '  Draws the banana
  'Parameters:
  '  xc# - Horizontal Coordinate
  '  yc# - Vertical Coordinate
  '  r - rotation position (0-3). (  \_/  ) /-\
  '  bc - if TRUE then DrawBan draws the banana ELSE it erases the banana
  Sub DrawBan(ByRef xc#, ByRef yc#, ByRef r%, ByRef bc%)

    Select Case r
      Case 0
        If bc Then PUT(xc#, yc#, LBan&, PUT_PSET) Else PUT(xc#, yc#, LBan&, PUT_XOR)
      Case 1
        If bc Then PUT(xc#, yc#, UBan&, PUT_PSET) Else PUT(xc#, yc#, UBan&, PUT_XOR)
      Case 2
        If bc Then PUT(xc#, yc#, DBan&, PUT_PSET) Else PUT(xc#, yc#, DBan&, PUT_XOR)
      Case 3
        If bc Then PUT(xc#, yc#, RBan&, PUT_PSET) Else PUT(xc#, yc#, RBan&, PUT_XOR)
    End Select

  End Sub

  'DrawGorilla:
  '  Draws the Gorilla in either CGA or EGA mode
  '  and saves the graphics data in an array.
  'Parameters:
  '  x - x coordinate of gorilla
  '  y - y coordinate of the gorilla
  '  arms - either Left up, Right up, or both down
  Sub DrawGorilla(ByRef x, ByRef y, ByRef arms)
    Dim i As Single   ' Local index must be single precision

    'draw head
    LINE(x - Scl(4), y, x + Scl(2.9), y + Scl(6), OBJECTCOLOR, BF)
    LINE(x - Scl(5), y + Scl(2), x + Scl(4), y + Scl(4), OBJECTCOLOR, BF)

    'draw eyes/brow
    LINE(x - Scl(3), y + Scl(2), x + Scl(2), y + Scl(2), 0)

    'draw nose if ega
    If Mode = 9 Then
      For i = -2 To -1
        PSET(x + i, y + 4, 0)
        PSET(x + i + 3, y + 4, 0)
      Next i
    End If

    'neck
    LINE(x - Scl(3), y + Scl(7), x + Scl(2), y + Scl(7), OBJECTCOLOR)

    'body
    LINE(x - Scl(8), y + Scl(8), x + Scl(6.9), y + Scl(14), OBJECTCOLOR, BF)
    LINE(x - Scl(6), y + Scl(15), x + Scl(4.9), y + Scl(20), OBJECTCOLOR, BF)

    'legs
    For i = 0 To 4
      CIRCLE(x + Scl(i), y + Scl(25), Scl(10), OBJECTCOLOR, 3 * pi# / 4, 9 * pi# / 8)
      CIRCLE(x + Scl(-6) + Scl(i - 0.1), y + Scl(25), Scl(10), OBJECTCOLOR, 15 * pi# / 8, pi# / 4)
    Next

    'chest
    CIRCLE(x - Scl(4.9), y + Scl(10), Scl(4.9), 0, 3 * pi# / 2, 0)
    CIRCLE(x + Scl(4.9), y + Scl(10), Scl(4.9), 0, pi#, 3 * pi# / 2)

    For i = -5 To -1
      Select Case arms
        Case 1
          'Right arm up
          CIRCLE(x + Scl(i - 0.1), y + Scl(14), Scl(9), OBJECTCOLOR, 3 * pi# / 4, 5 * pi# / 4)
          CIRCLE(x + Scl(4.9) + Scl(i), y + Scl(4), Scl(9), OBJECTCOLOR, 7 * pi# / 4, pi# / 4)
          [GET](x - Scl(15), y - Scl(1), x + Scl(14), y + Scl(28), GorR&)
        Case 2
          'Left arm up
          CIRCLE(x + Scl(i - 0.1), y + Scl(4), Scl(9), OBJECTCOLOR, 3 * pi# / 4, 5 * pi# / 4)
          CIRCLE(x + Scl(4.9) + Scl(i), y + Scl(14), Scl(9), OBJECTCOLOR, 7 * pi# / 4, pi# / 4)
          [GET](x - Scl(15), y - Scl(1), x + Scl(14), y + Scl(28), GorL&)
        Case 3
          'Both arms down
          CIRCLE(x + Scl(i - 0.1), y + Scl(14), Scl(9), OBJECTCOLOR, 3 * pi# / 4, 5 * pi# / 4)
          CIRCLE(x + Scl(4.9) + Scl(i), y + Scl(14), Scl(9), OBJECTCOLOR, 7 * pi# / 4, pi# / 4)
          [GET](x - Scl(15), y - Scl(1), x + Scl(14), y + Scl(28), GorD&)
      End Select
    Next i
  End Sub

  'ExplodeGorilla:
  '  Causes gorilla explosion when a direct hit occurs
  'Parameters:
  '  X#, Y# - shot location
  Function ExplodeGorilla(ByRef x#, ByRef y#)
    Dim YAdj = Scl(12)
    Dim XAdj = Scl(5)
    Dim SclX# = ScrWidth / 320
    Dim SclY# = ScrHeight / 200
    Dim PlayerHit%
    If x# < ScrWidth / 2 Then PlayerHit = 1 Else PlayerHit = 2
    PLAY("MBO0L16EFGEFDC")

    For i = 1 To 8 * SclX#
      CIRCLE(GorillaX(PlayerHit) + 3.5 * SclX# + XAdj, GorillaY(PlayerHit) + 7 * SclY# + YAdj, i, ExplosionColor, Nothing, Nothing, -1.57)
      LINE(GorillaX(PlayerHit) + 7 * SclX#, GorillaY(PlayerHit) + 9 * SclY# - i, GorillaX(PlayerHit), GorillaY(PlayerHit) + 9 * SclY# - i, ExplosionColor)
    Next i

    For i = 1 To 16 * SclX#
      If i < (8 * SclX#) Then CIRCLE(GorillaX(PlayerHit) + 3.5 * SclX# + XAdj, GorillaY(PlayerHit) + 7 * SclY# + YAdj, (8 * SclX# + 1) - i, BACKATTR, Nothing, Nothing, -1.57)
      CIRCLE(GorillaX(PlayerHit) + 3.5 * SclX# + XAdj, GorillaY(PlayerHit) + YAdj, i, i Mod 2 + 1, Nothing, Nothing, -1.57)
    Next i

    For i = 24 * SclX# To 1 Step -1
      CIRCLE(GorillaX(PlayerHit) + 3.5 * SclX# + XAdj, GorillaY(PlayerHit) + YAdj, i, BACKATTR, Nothing, Nothing, -1.57)
      For Count = 1 To 200
      Next
    Next i

    ExplodeGorilla = PlayerHit
  End Function

  'GetInputs:
  '  Gets user inputs at beginning of game
  'Parameters:
  '  Player1$, Player2$ - player names
  '  NumGames - number of games to play
  Async Function GetInputsAsync() As Task(Of (Player1 As String, Player2 As String, NumGames As Integer))

    COLOR(7, 0)
    CLS()

    LOCATE(8, 15)
    REM LINE_INPUT("Name of Player 1 (Default = 'Player 1'): ", Player1$)
    Dim Player1$ = Await LineInputAsync("Name of Player 1 (Default = 'Player 1'): ")
    If Player1$ = "" Then
      Player1$ = "Player 1"
    Else
      Player1$ = VB.Left(Player1$, 10)
    End If

    LOCATE(10, 15)
    REM LINE_INPUT("Name of Player 2 (Default = 'Player 2'): ", Player2$)
    Dim Player2$ = Await LineInputAsync("Name of Player 2 (Default = 'Player 2'): ")
    If Player2$ = "" Then
      Player2$ = "Player 2"
    Else
      Player2$ = VB.Left(Player2$, 10)
    End If

    Dim game$, NumGames%
    Do
      LOCATE(12, 56) : PRINT(Space(25), True)
      LOCATE(12, 13)
      REM INPUT("Play to how many total points (Default = 3)", game$)
      game$ = Await InputAsync("Play to how many total points (Default = 3)")
      NumGames = Val(VB.Left(game$, 2))
    Loop Until NumGames > 0 And Len(game$) < 3 Or Len(game$) = 0
    If NumGames = 0 Then NumGames = 3

    Dim grav$
    Do
      LOCATE(14, 53) : PRINT(Space(28), True)
      LOCATE(14, 17)
      'INPUT("Gravity in Meters/Sec (Earth = 9.8)", grav$)
      grav$ = Await InputAsync("Gravity in Meters/Sec (Earth = 9.8)")
      gravity# = Val(grav$)
    Loop Until gravity# > 0 Or Len(grav$) = 0
    If gravity# = 0 Then gravity# = 9.8

    Return (Player1, Player2, NumGames)

  End Function

  'GetNum:
  '  Gets valid numeric input from user
  'Parameters:
  '  Row, Col - location to echo input
  Function GetNum#(Row, Col)
    Dim Result$ = ""
    Dim Done = False
    While INKEY$() <> "" : End While   'Clear keyboard buffer

    Do While Not Done

      LOCATE(Row, Col)
      PRINT($"{Result$}{QBChr(95)}    ", True)

      Dim Kbd$ = INKEY$()

      Select Case Kbd$
        Case "0" To "9"
          Result$ = Result$ + Kbd$
        Case "."
          If InStr(Result$, ".") = 0 Then
            Result$ = Result$ + Kbd$
          End If
        Case QBChr(13)
          If Val(Result$) > 360 Then
            Result$ = ""
          Else
            Done = True
          End If
        Case QBChr(8)
          If Len(Result$) > 0 Then
            Result$ = VB.Left(Result$, Len(Result$) - 1)
          End If
        Case Else
          If Len(Kbd$) > 0 Then
            Beep()
          End If
      End Select
    Loop

    LOCATE(Row, Col)
    PRINT($"{Result$} ", True)

    GetNum# = Val(Result$)
  End Function

  'GorillaIntro:
  '  Displays gorillas on screen for the first time
  '  allows the graphical data to be put into an array
  'Parameters:
  '  Player1$, Player2$ - The names of the players
  '
  Sub GorillaIntro(ByRef Player1$, ByRef Player2$)
    LOCATE(16, 34) : PRINT("--------------")
    LOCATE(18, 34) : PRINT("V = View Intro")
    LOCATE(19, 34) : PRINT("P = Play Game")
    LOCATE(21, 35) : PRINT("Your Choice?")

    Dim Ch$
    Do While Ch$ = ""
      Ch$ = INKEY$()
    Loop

    Dim x%, y%
    If Mode = 1 Then
      x = 125
      y = 100
    Else
      x = 278
      y = 175
    End If

    SCREEN(Mode)
    SetScreen()

    If Mode = 1 Then Center(5, "Please wait while gorillas are drawn.")

    VIEW_PRINT(9, 24)

    If Mode = 9 Then PALETTE(OBJECTCOLOR, qbBackColor)

    DrawGorilla(x, y, ARMSDOWN)
    CLS(2)
    DrawGorilla(x, y, LEFTUP)
    CLS(2)
    DrawGorilla(x, y, RIGHTUP)
    CLS(2)

    VIEW_PRINT(1, 25)
    If Mode = 9 Then PALETTE(OBJECTCOLOR, 46)

    If UCase(Ch$) = "V" Then
      Center(2, "Q B A S I C   G O R I L L A S")
      Center(5, "             STARRING:               ")
      Dim P$ = Player1$ + " AND " + Player2$
      Center(7, P$)

      PUT(x - 13, y, GorD&, PUT_PSET)
      PUT(x + 47, y, GorD&, PUT_PSET)
      Rest(1)

      PUT(x - 13, y, GorL&, PUT_PSET)
      PUT(x + 47, y, GorR&, PUT_PSET)
      PLAY("t120o1l16b9n0baan0bn0bn0baaan0b9n0baan0b")
      Rest(0.3)

      PUT(x - 13, y, GorR&, PUT_PSET)
      PUT(x + 47, y, GorL&, PUT_PSET)
      PLAY("o2l16e-9n0e-d-d-n0e-n0e-n0e-d-d-d-n0e-9n0e-d-d-n0e-")
      Rest(0.3)

      PUT(x - 13, y, GorL&, PUT_PSET)
      PUT(x + 47, y, GorR&, PUT_PSET)
      PLAY("o2l16g-9n0g-een0g-n0g-n0g-eeen0g-9n0g-een0g-")
      Rest(0.3)

      PUT(x - 13, y, GorR&, PUT_PSET)
      PUT(x + 47, y, GorL&, PUT_PSET)
      PLAY("o2l16b9n0baan0g-n0g-n0g-eeen0o1b9n0baan0b")
      Rest(0.3)

      For i = 1 To 4
        PUT(x - 13, y, GorL&, PUT_PSET)
        PUT(x + 47, y, GorR&, PUT_PSET)
        PLAY("T160O0L32EFGEFDC")
        Rest(0.1)
        PUT(x - 13, y, GorR&, PUT_PSET)
        PUT(x + 47, y, GorL&, PUT_PSET)
        PLAY("T160O0L32EFGEFDC")
        Rest(0.1)
      Next
    End If
  End Sub

  'Intro:
  '  Displays game introduction
  Async Function IntroAsync() As Task

    SCREEN(0)
    QbWidth(80, 25)
    MaxCol = 80
    COLOR(15, 0)
    CLS()

    Await Task.Delay(1)

    Center(4, "Q B a s i c    G O R I L L A S")
    COLOR(7)
    Center(6, "Copyright (C) Microsoft Corporation 1990")
    Center(8, "Your mission is to hit your opponent with the exploding")
    Center(9, "banana by varying the angle and power of your throw, taking")
    Center(10, "into account wind speed, gravity, and the city skyline.")
    Center(11, "The wind speed is shown by a directional arrow at the bottom")
    Center(12, "of the playing field, its length relative to its strength.")
    Center(24, "Press any key to continue")

    PLAY("MBT160O1L8CDEDCDL4ECC")
    Await SparklePauseAsync()
    If Mode = 1 Then MaxCol = 40

  End Function

  'MakeCityScape:
  '  Creates random skyline for game
  'Parameters:
  '  BCoor() - a user-defined type array which stores the coordinates of
  '  the upper left corner of each building.
  Sub MakeCityScape(ByRef BCoor() As XYPoint)

    Dim x = 2

    Dim NewHt%, BottomLine%, HtInc%, DefBWidth%, RandomHeight%, WWidth%, WHeight%
    Dim WDifV%, WDifh%, MaxHeight%, BuildingColor%, BackGround%

    'Set the sloping trend of the city scape. NewHt is new building height
    Dim Slope = FnRan(6)
    Select Case Slope
      Case 1 : NewHt = 15                 'Upward slope
      Case 2 : NewHt = 130                'Downward slope
      Case 3 To 5 : NewHt = 15            '"V" slope - most common
      Case 6 : NewHt = 130                'Inverted "V" slope
    End Select

    If Mode = 9 Then
      BottomLine = 335                   'Bottom of building
      HtInc = 10                         'Increase value for new height
      DefBWidth = 37                     'Default building height
      RandomHeight = 120                 'Random height difference
      WWidth = 3                         'Window width
      WHeight = 6                        'Window height
      WDifV = 15                         'Counter for window spacing - vertical
      WDifh = 10                         'Counter for window spacing - horizontal
    Else
      BottomLine = 190
      HtInc = 6
      NewHt = NewHt * 20 \ 35            'Adjust for CGA
      DefBWidth = 18
      RandomHeight = 54
      WWidth = 1
      WHeight = 2
      WDifV = 5
      WDifh = 4
    End If

    Dim CurBuilding = 1
    Do

      Select Case Slope
        Case 1
          NewHt = NewHt + HtInc
        Case 2
          NewHt = NewHt - HtInc
        Case 3 To 5
          If x > ScrWidth \ 2 Then
            NewHt = NewHt - 2 * HtInc
          Else
            NewHt = NewHt + 2 * HtInc
          End If
        Case 4
          If x > ScrWidth \ 2 Then
            NewHt = NewHt + 2 * HtInc
          Else
            NewHt = NewHt - 2 * HtInc
          End If
      End Select

      'Set width of building and check to see if it would go off the screen
      Dim BWidth = FnRan(DefBWidth) + DefBWidth
      If x + BWidth > ScrWidth Then BWidth = ScrWidth - x - 2

      'Set height of building and check to see if it goes below screen
      Dim BHeight = FnRan(RandomHeight) + NewHt
      If BHeight < HtInc Then BHeight = HtInc

      'Check to see if Building is too high
      If BottomLine - BHeight <= MaxHeight + GHeight Then BHeight = MaxHeight + GHeight - 5

      'Set the coordinates of the building into the array
      BCoor(CurBuilding).XCoor = x
      BCoor(CurBuilding).YCoor = BottomLine - BHeight

      If Mode = 9 Then BuildingColor = FnRan(3) + 4 Else BuildingColor = 2

      'Draw the building, outline first, then filled
      LINE(x - 1, BottomLine + 1, x + BWidth + 1, BottomLine - BHeight - 1, BACKGROUND, B)
      LINE(x, BottomLine, x + BWidth, BottomLine - BHeight, BuildingColor, BF)

      'Draw the windows
      Dim c = x + 3
      Do
        For i = BHeight - 3 To 7 Step -WDifV
          Dim WinColr%
          If Mode <> 9 Then
            WinColr = (FnRan(2) - 2) * -3
          ElseIf FnRan(4) = 1 Then
            WinColr = 8
          Else
            WinColr = WINDOWCOLOR
          End If
          LINE(c, BottomLine - i, c + WWidth, BottomLine - i + WHeight, WinColr, BF)
        Next
        c = c + WDifh
      Loop Until c >= x + BWidth - 3

      x = x + BWidth + 2

      CurBuilding = CurBuilding + 1

    Loop Until x > ScrWidth - HtInc

    LastBuilding = CurBuilding - 1

    'Set Wind speed
    Wind = FnRan(10) - 5
    If FnRan(3) = 1 Then
      If Wind > 0 Then
        Wind = Wind + FnRan(10)
      Else
        Wind = Wind - FnRan(10)
      End If
    End If

    'Draw Wind speed arrow
    If Wind <> 0 Then
      Dim WindLine = Wind * 3 * (ScrWidth \ 320)
      LINE(ScrWidth \ 2, ScrHeight - 5, ScrWidth \ 2 + WindLine, ScrHeight - 5, ExplosionColor)
      Dim arrowDir%
      If Wind > 0 Then ArrowDir = -2 Else ArrowDir = 2
      LINE(ScrWidth / 2 + WindLine, ScrHeight - 5, ScrWidth / 2 + WindLine + ArrowDir, ScrHeight - 5 - 2, ExplosionColor)
      LINE(ScrWidth / 2 + WindLine, ScrHeight - 5, ScrWidth / 2 + WindLine + ArrowDir, ScrHeight - 5 + 2, ExplosionColor)
    End If
  End Sub

  'PlaceGorillas:
  '  PUTs the Gorillas on top of the buildings.  Must have drawn
  '  Gorillas first.
  'Parameters:
  '  BCoor() - user-defined TYPE array which stores upper left coordinates
  '  of each building.
  Sub PlaceGorillas(ByRef BCoor() As XYPoint)

    Dim XAdj%, YAdj%, SclX#, SclY#, BNum%, BWidth%

    If Mode = 9 Then
      XAdj = 14
      YAdj = 30
    Else
      XAdj = 7
      YAdj = 16
    End If
    SclX# = ScrWidth / 320
    SclY# = ScrHeight / 200

    'Place gorillas on second or third building from edge
    For i = 1 To 2
      If i = 1 Then BNum = FnRan(2) + 1 Else BNum = LastBuilding - FnRan(2)

      BWidth = BCoor(BNum + 1).XCoor - BCoor(BNum).XCoor
      GorillaX(i) = BCoor(BNum).XCoor + BWidth / 2 - XAdj
      GorillaY(i) = BCoor(BNum).YCoor - YAdj
      PUT(GorillaX(i), GorillaY(i), GorD&, PUT_PSET)
    Next i

  End Sub

  'PlayGame:
  '  Main game play routine
  'Parameters:
  '  Player1$, Player2$ - player names
  '  NumGames - number of games to play
  Sub PlayGame(Player1$, Player2$, NumGames%)

    Dim BCoor(0 To 30) As XYPoint
    Dim TotalWins%(0 To 2) ' was 1 To 2

    Dim J%, Tosser%, Tossee%
    Dim Hit As Boolean

    J = 1

    For i = 1 To NumGames

      CLS()
      Randomize(QBTimer)
      Call MakeCityScape(BCoor)
      Call PlaceGorillas(BCoor)
      DoSun(SUNHAPPY)
      Hit = False
      Do While Hit = False
        J = 1 - J
        LOCATE(1, 1)
        PRINT(Player1$)
        LOCATE(1, (MaxCol - 1 - Len(Player2$)))
        PRINT(Player2$)
        Center(23, LTrim(Str(TotalWins(1))) + ">Score<" + LTrim(Str(TotalWins(2))))
        Tosser = J + 1 : Tossee = 3 - J

        'Plot the shot.  Hit is true if Gorilla gets hit.
        Hit = DoShot(Tosser, GorillaX(Tosser), GorillaY(Tosser))

        'Reset the sun, if it got hit
        If SunHit Then DoSun(SUNHAPPY)

        If Hit = True Then Call UpdateScores(TotalWins, Tosser, Hit)
      Loop
      SLEEP(1)
    Next i

    SCREEN(0)
    QbWidth(80, 25)
    COLOR(7, 0)
    MaxCol = 80
    CLS()

    Center(8, "GAME OVER!")
    Center(10, "Score:")
    LOCATE(11, 30) : PRINT($"{Player1$}{TAB(50)}{TotalWins(1)}")
    LOCATE(12, 30) : PRINT($"{Player2$}{TAB(50)}{TotalWins(2)}")
    Center(24, "Press any key to continue")
    REM Await SparklePauseAsync()
    COLOR(7, 0)
    CLS()
  End Sub

  'PlayGame:
  '  Plots banana shot across the screen
  'Parameters:
  '  StartX, StartY - starting shot location
  '  Angle - shot angle
  '  Velocity - shot velocity
  '  PlayerNum - the banana thrower
  Function PlotShot(ByVal StartX%, ByVal StartY%, ByVal Angle#, ByVal Velocity%, ByVal PlayerNum%)

    Angle# = Angle# / 180 * pi#  'Convert degree angle to radians
    Dim Radius = Mode Mod 7

    Dim InitXVel# = Cos(Angle#) * Velocity
    Dim InitYVel# = Sin(Angle#) * Velocity

    Dim oldx# = StartX
    Dim oldy# = StartY

    'draw gorilla toss
    If PlayerNum = 1 Then
      PUT(StartX, StartY, GorL&, PUT_PSET)
    Else
      PUT(StartX, StartY, GorR&, PUT_PSET)
    End If

    'throw sound
    PLAY("MBo0L32A-L64CL16BL64A+")
    Rest(0.1)

    'redraw gorilla
    PUT(StartX, StartY, GorD&, PUT_PSET)

    Dim adjust = Scl(4)                   'For scaling CGA

    Dim xedge = Scl(9) * (2 - PlayerNum)  'Find leading edge of banana for check

    Dim Impact = False
    Dim ShotInSun = False
    Dim OnScreen = True
    Dim PlayerHit = 0
    Dim NeedErase = False

    Dim StartXPos = StartX
    Dim StartYPos = StartY - adjust - 3

    Dim direction%
    If PlayerNum = 2 Then
      StartXPos = StartXPos + Scl(25)
      direction = Scl(4)
    Else
      direction = Scl(-4)
    End If

    Dim x#, y#, pointVal%
    If Velocity < 2 Then              'Shot too slow - hit self
      x# = StartX
      y# = StartY
      pointval = OBJECTCOLOR
    End If

    Dim oldrot%, t#

    Do While (Not Impact) And OnScreen

      Rest(0.02)

      'Erase old banana, if necessary
      If NeedErase Then
        NeedErase = False
        Call DrawBan(oldx#, oldy#, oldrot, False)
      End If

      x# = StartXPos + (InitXVel# * t#) + (0.5 * (Wind / 5) * t# ^ 2)
      y# = StartYPos + ((-1 * (InitYVel# * t#)) + (0.5 * gravity# * t# ^ 2)) * (ScrHeight / 350)

      If (x# >= ScrWidth - Scl(10)) Or (x# <= 3) Or (y# >= ScrHeight - 3) Then
        OnScreen = False
      End If


      If OnScreen And y# > 0 Then

        Dim rot%

        'check it
        Dim LookY = 0
        Dim LookX = Scl(8 * (2 - PlayerNum))
        Do
          pointval = POINT(x# + LookX, y# + LookY)
          If pointval = 0 Then
            Impact = False
            If ShotInSun = True Then
              If Abs(ScrWidth \ 2 - x#) > Scl(20) Or y# > SunHt Then ShotInSun = False
            End If
          ElseIf pointval = SUNATTR And y# < SunHt Then
            If Not SunHit Then DoSun(SUNSHOCK)
            SunHit = True
            ShotInSun = True
          Else
            Impact = True
          End If
          LookX = LookX + direction
          LookY = LookY + Scl(6)
        Loop Until Impact Or LookX <> Scl(4)

        If Not ShotInSun And Not Impact Then
          'plot it
          rot = (t# * 10) Mod 4
          Call DrawBan(x#, y#, rot, True)
          NeedErase = True
        End If

        oldx# = x#
        oldy# = y#
        oldrot = rot

      End If


      t# = t# + 0.1

    Loop

    If pointval <> OBJECTCOLOR And Impact Then
      Call DoExplosion(x# + adjust, y# + adjust)
    ElseIf pointval = OBJECTCOLOR Then
      PlayerHit = ExplodeGorilla(x#, y#)
    End If

    PlotShot = PlayerHit

  End Function

  'Rest:
  '  pauses the program
  Sub Rest(ByRef t#)
    Dim s# = QBTimer()
    Dim t2# = MachSpeed * t# / SPEEDCONST
    Do
    Loop Until QBTimer() - s# > t2#
  End Sub

  'Scl:
  '  Pass the number in to scaling for cga.  If the number is a decimal, then we
  '  want to scale down for cga or scale up for ega.  This allows a full range
  '  of numbers to be generated for scaling.
  '  (i.e. for 3 to get scaled to 1, pass in 2.9)
  Function Scl(ByRef n!)

    If n! <> Int(n!) Then
      If Mode = 1 Then n! = n! - 1
    End If
    If Mode = 1 Then
      Scl = CInt(n! / 2 + 0.1)
    Else
      Scl = CInt(n!)
    End If

  End Function

  'SetScreen:
  '  Sets the appropriate color statements
  Sub SetScreen()

    If Mode = 9 Then
      ExplosionColor = 2
      qbBackColor = 1
      PALETTE(0, 1)
      PALETTE(1, 46)
      PALETTE(2, 44)
      PALETTE(3, 54)
      PALETTE(5, 7)
      PALETTE(6, 4)
      PALETTE(7, 3)
      PALETTE(9, 63)       'Display Color
    Else
      ExplosionColor = 2
      qbBackColor = 0
      COLOR(qbBackColor, 2)

    End If

  End Sub

  Private Async Sub Me_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    Me.Show()
    Await Task.Delay(1)

    Video.PictureBox = PictureBox1

    If False Then
      Video.LOCATE(5, 5)
      Video.PRINT("Hello World!")
    Else

      pi# = 4 * Atan(1.0#)

      'This is a clever way to pick the best graphics mode available
      Mode = 9
      SCREEN(Mode)
      REM If Mode = 9 Then PALETTE(4, 0)   'Check for 64K EGA

      MachSpeed = CalcDelay()

      If Mode = 9 Then
        ScrWidth = 640
        ScrHeight = 350
        GHeight = 25
        REM RESTORE EGABanana

        RESTORE()

EGABanana:
        'BananaLeft
        DATA(458758, 202116096, 471604224, 943208448, 943208448, 943208448, 471604224, 202116096, 0)
        'BananaDown
        DATA(262153, -2134835200, -2134802239, -2130771968, -2130738945, 8323072, 8323199, 4063232, 4063294)
        'BananaUp
        DATA(262153, 4063232, 4063294, 8323072, 8323199, -2130771968, -2130738945, -2134835200, -2134802239)
        'BananaRight
        DATA(458758, -1061109760, -522133504, 1886416896, 1886416896, 1886416896, -522133504, -1061109760, 0)

        ReDim LBan&(8), RBan&(8), UBan&(8), DBan&(8)

        For i = 0 To 8
          READ(LBan&(i))
        Next i

        For i = 0 To 8
          READ(DBan&(i))
        Next i

        For i = 0 To 8
          READ(UBan&(i))
        Next i

        For i = 0 To 8
          READ(RBan&(i))
        Next i

        SunHt = 39

      Else

        ScrWidth = 320
        ScrHeight = 200
        GHeight = 12

        REM RESTORE CGABanana
        RESTORE()

CGABanana:
        'BananaLeft
        DATA(327686, -252645316, 60)
        'BananaDown
        DATA(196618, -1057030081, 49344)
        'BananaUp
        DATA(196618, -1056980800, 63)
        'BananaRight
        DATA(327686, 1010580720, 240)

        ReDim LBan&(2), RBan&(2), UBan&(2), DBan&(2)
        REM ReDim GorL&(20), GorD&(20), GorR&(20)

        For i = 0 To 2
          READ(LBan&(i))
        Next i
        For i = 0 To 2
          READ(DBan&(i))
        Next i
        For i = 0 To 2
          READ(UBan&(i))
        Next i
        For i = 0 To 2
          READ(RBan&(i))
        Next i

        MachSpeed = MachSpeed * 1.3
        SunHt = 20
      End If

      ' -----

      ' Sub Program 1
      Await IntroAsync()
      ' Sub Progam 2
      REM GetInputs(Name1$, Name2$, NumGames)
      Dim r = Await GetInputsAsync()
      Dim Name1$ = r.Player1 : Dim Name2$ = r.Player2 : Dim NumGames = r.NumGames

      ' Sub Program 3
      GorillaIntro(Name1$, Name2$)
      ' Sub Program 4
      REM PlayGame(Name1$, Name2$, NumGames)

      MsgBox("Done!")

    End If

  End Sub

  Private Sub Me_Resize(sender As Object, e As EventArgs) Handles Me.Resize

    ' Resizes the picture box preserving the aspect ratio of 4x3.

    Dim clientW = ClientSize.Width
    Dim clientH = ClientSize.Height

    If clientH > clientW * 0.75 Then
      PictureBox1.Width = clientW
      PictureBox1.Height = clientW * 0.75
      PictureBox1.Left = 0
      PictureBox1.Top = (clientH - PictureBox1.Height) \ 2
    Else
      PictureBox1.Height = clientH
      PictureBox1.Width = clientH * 1.333
      PictureBox1.Top = 0
      PictureBox1.Left = (clientW - PictureBox1.Width) \ 2
    End If

  End Sub

  'SparklePause:
  '  Creates flashing border for intro and game over screens
  Async Function SparklePauseAsync() As Task

    Dim a%, b%

    COLOR(14, 0)
    Dim Aa$ = "*    *    *    *    *    *    *    *    *    *    *    *    *    *    *    *    *    "
    While INKEY$() <> "" : End While 'Clear keyboard buffer

    While INKEY$() = ""

      For a = 1 To 5
        LOCATE(1, 1)                             'print horizontal sparkles
        PRINT(Mid(Aa$, a, 80), True)
        LOCATE(22, 1)
        PRINT(Mid(Aa$, 6 - a, 80), True)

        For b = 2 To 21                         'Print Vertical sparkles
          Dim c = (a + b) Mod 5
          If c = 1 Then
            LOCATE(b, 80)
            PRINT("*", True)
            LOCATE(23 - b, 1)
            PRINT("*", True)
          Else
            LOCATE(b, 80)
            PRINT(" ", True)
            LOCATE(23 - b, 1)
            PRINT(" ", True)
          End If
        Next b
        Await Task.Delay(1)
      Next a

    End While

  End Function

  'UpdateScores:
  '  Updates players' scores
  'Parameters:
  '  Record - players' scores
  '  PlayerNum - player
  '  Results - results of player's shot
  Sub UpdateScores(ByVal Record%(), ByVal PlayerNum%, ByVal Results%)
    If Results = HITSELF Then
      Record(Abs(PlayerNum - 3)) = Record(Abs(PlayerNum - 3)) + 1
    Else
      Record(PlayerNum) = Record(PlayerNum) + 1
    End If
  End Sub

  'VictoryDance:
  '  gorilla dances after he has eliminated his opponent
  'Parameters:
  '  Player - which gorilla is dancing
  Sub VictoryDance(ByVal Player%)

    For i# = 1 To 4
      PUT(GorillaX(Player), GorillaY(Player), GorL&, PUT_PSET)
      PLAY("MFO0L32EFGEFDC")
      Rest(0.2)
      PUT(GorillaX(Player), GorillaY(Player), GorR&, PUT_PSET)
      PLAY("MFO0L32EFGEFDC")
      Rest(0.2)
    Next
  End Sub

  Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
    m_keys.Push(e.KeyCode) : e.Handled = True : e.SuppressKeyPress = True
  End Sub

End Class
