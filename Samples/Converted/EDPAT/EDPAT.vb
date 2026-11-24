' Converted from QBasic by qb2vb.

Module Program

  Dim Pattern$
  Dim Esc$
  Dim Bit(7) As Short
  Dim PatternSize As Short

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim OK$
    ' DECLARE SUB DrawPattern ()
    ' DECLARE SUB EditPattern ()
    ' DECLARE SUB Initialize ()
    ' DECLARE SUB ShowPattern (OK$)
    
    
    Do
       Initialize()
       EditPattern()
       ShowPattern(OK$)
    Loop While OK$ = "Y"
    
    End
  End Sub

  '
  ' ======================== DRAWPATTERN =======================
  '    Draws a patterned rectangle on the right side of screen
  ' ============================================================
  '
  Sub DrawPattern()
  ' SHARED Pattern$   (shared variables are declared at module level)
  
     VIEW(320, 24, 622, 160, 0, 1)  ' Set view to rectangle
     PAINT(1, 1, Pattern$)  ' Use PAINT to fill it
     VIEW()  ' Set view to full screen
  
  End Sub
  
  '
  ' ======================== EDITPATTERN =======================
  '                  Edits a tile-byte pattern
  ' ============================================================
  '
  Sub EditPattern()
    Static ByteNum As Short
    Static BitNum As Short
    Static Null$
    Static X As Short
    Static Y As Short
    Static State As Short
    Static RefTime!
    Static Check$
    Static CurrentByte As Short
    Static CurrentColor As Short
  ' SHARED Pattern$, Esc$, Bit%(), PatternSize%   (shared variables are declared at module level)
  
     ByteNum = 1  ' Starting position.
     BitNum = 7
     Null$ = QBChr(0)  ' CHR$(0) is the first byte of the
                          ' two-byte string returned when a
                          ' direction key such as UP or DOWN is
                          ' pressed.
     Do
  
        ' Calculate starting location on screen of this bit:
        X = CShort(((7 - BitNum) * 16) + 80)
        Y = CShort((ByteNum + 2) * 8)
  
        ' Wait for a key press (and flash cursor each 3/10 second):
        State = 0
        RefTime = 0
        Do
  
           ' Check timer and switch cursor state if 3/10 second:
           If Abs(Timer - RefTime) > .3! Then
              RefTime = CSng(Timer)
              State = CShort(1 - State)
  
              ' Turn the  border of bit on and off:
              LINE(False, X - 1, Y - 1, [STEP], 15, 8, State, LineOption.B)
           End If
  
           Check$ = INKEY$()  ' Check for key press.
  
        Loop While Check$ = ""  ' Loop until a key is pressed.
  
        ' Erase cursor:
        LINE(False, X - 1, Y - 1, [STEP], 15, 8, 0, LineOption.B)
  
        Select Case Check$  ' Respond to key press.
  
           Case QBChr(27)  ' ESC key pressed:
              Exit Sub  ' exit this subprogram
  
           Case QBChr(32)  ' SPACEBAR pressed:
                                  ' reset state of bit
  
              ' Invert bit in pattern string:
              CurrentByte = CShort(Asc(Mid$(Pattern$, ByteNum, 1)))
              CurrentByte = CurrentByte Xor Bit(BitNum)
              Mid(Pattern$, ByteNum) = QBChr(CurrentByte)
  
              ' Redraw bit on screen:
              If (CurrentByte And Bit(BitNum)) <> 0 Then
                  CurrentColor = 1
              Else
                  CurrentColor = 0
              End If
              LINE(False, X + 1, Y + 1, [STEP], 11, 4, CurrentColor, LineOption.BF)
  
           Case QBChr(13)  ' ENTER key pressed:
              DrawPattern()  ' draw pattern in box on right.
  
           Case Null$ + QBChr(75)  ' LEFT key: move cursor left
  
              BitNum = CShort(BitNum + 1)
              If BitNum > 7 Then BitNum = 0
  
           Case Null$ + QBChr(77)  ' RIGHT key: move cursor right
  
              BitNum = CShort(BitNum - 1)
              If BitNum < 0 Then BitNum = 7
  
           Case Null$ + QBChr(72)  ' UP key: move cursor up
  
              ByteNum = CShort(ByteNum - 1)
              If ByteNum < 1 Then ByteNum = PatternSize
  
           Case Null$ + QBChr(80)  ' DOWN key: move cursor down
  
              ByteNum = CShort(ByteNum + 1)
              If ByteNum > PatternSize Then ByteNum = 1
  
           Case Else
              ' User pressed a key other than ESC, SPACEBAR,
              ' ENTER, UP, DOWN, LEFT, or RIGHT, so don't
              ' do anything.
        End Select
     Loop
  End Sub
  
  '
  ' ======================== INITIALIZE ========================
  '               Sets up starting pattern and screen
  ' ============================================================
  '
  Sub Initialize()
    Static I As Short
    Static X As Short
    Static Y As Short
    Static J As Short
  ' SHARED Pattern$, Esc$, Bit%(), PatternSize%   (shared variables are declared at module level)
  
     Esc$ = QBChr(27)  ' ESC character is ASCII 27.
  
     ' Set up an array holding bits in positions 0 to 7:
     For I = 0 To 7
        Bit(I) = CShort(2 ^ I)
     Next I
  
     CLS()
  
     ' Input the pattern size (in number of bytes):
     LOCATE(5, 5)
     PRINT("Enter pattern size (1-16 rows):", True)
     Do
        LOCATE(5, 38)
        PRINT("    ", True)
        LOCATE(5, 38)
        INPUT("", PatternSize, False)
     Loop While (PatternSize < 1) Or (PatternSize > 16)
  
     ' Set initial pattern to all bits set:
     Pattern$ = QBString(PatternSize, 255)
  
     SCREEN(2)  ' 640 x 200 monochrome graphics mode.
  
     ' Draw dividing lines:
     LINE(0, 10, 635, 10, 1)
     LINE(300, 0, 300, 199)
     LINE(302, 0, 302, 199)
  
     ' Print titles:
     LOCATE(1, 13) : PRINT("Pattern Bytes")
     LOCATE(1, 53) : PRINT("Pattern View")
  
     ' Draw editing screen for pattern:
     For I = 1 To PatternSize
  
        ' Print label on left of each line:
        LOCATE(I + 3, 8)
        PRINT_USING("##:", I)
  
        ' Draw "bit" boxes:
        X = 80
        Y = CShort((I + 2) * 8)
        For J = 1 To 8
           LINE(False, X, Y, [STEP], 13, 6, 1, LineOption.BF)
           X = CShort(X + 16)
        Next J
     Next I
  
     DrawPattern()  ' Draw  "Pattern View" box.
  
     LOCATE(21, 1)
     PRINT("DIRECTION keys........Move cursor")
     PRINT("SPACEBAR............Changes point")
     PRINT("ENTER............Displays pattern")
     PRINT("ESC.........................Quits", True)
  
  End Sub
  
  '
  ' ======================== SHOWPATTERN =======================
  '     Prints the CHR$ values used by PAINT to make pattern
  ' ============================================================
  '
  Sub ShowPattern(ByRef OK$)
    Static I As Short
    Static PatternByte As Short
  ' SHARED Pattern$, PatternSize%   (shared variables are declared at module level)
  
     ' Return screen to 80-column text mode:
     SCREEN(0, 0)
     WIDTH(80)
  
     PRINT("The following characters make up your pattern:")
     PRINT()
  
     ' Print out the value for each pattern byte:
     For I = 1 To PatternSize
        PatternByte = CShort(Asc(Mid$(Pattern$, I, 1)))
        PRINT("CHR$(" & LTrim$(Str$(PatternByte)) & ")")
     Next I
  
     PRINT()
     LOCATE(-1, -1, 1)
     PRINT("New pattern? ", True)
     OK$ = UCase$(InputStr(1))
  End Sub
  
End Module
