' Converted from QBasic by qb2vb.

Module Program

  Const PI As Double = 3.141592653589#

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim EndLoop!
    Dim Y!
    Dim Esc$ = ""
    ' DECLARE FUNCTION GetArraySize (WLeft, WRight, WTop, WBottom)
    
    SCREEN(2)
    CLS()
    VIEW(20, 10, 620, 190, -1, 1)
    
    ' CONST PI = 3.141592653589#
    
    WINDOW(-3.15!, -.14!, 3.56!, 1.01!)
    
    ' $DYNAMIC
    ' The rectangle is smaller than the one in the previous
    ' program, which means Array is also smaller:
    Dim WLeft! = -.18!
    Dim WRight! = .18!
    Dim WTop! = .05!
    Dim WBottom! = -.05!
    
    Dim ArraySize As Short = CShort(GetArraySize(WLeft, WRight, WTop, WBottom))
    
    Dim Array(ArraySize) As Short
    
    CIRCLE(0, 0, .18!)
    PAINT(0, 0)
    
    [GET](WLeft, WTop, WRight, WBottom, Array)
    CLS()
    
    LINE(-3, .8!, 3.4!, .2!, -1, LineOption.B)
    Dim Pattern$ = ((QBChr(126) + QBChr(0)) + QBChr(126)) + QBChr(126)
    PAINT(0, .5!, Pattern$)
    
    LOCATE(21, 29)
    PRINT("Press any key to end")
    
    Dim StepSize! = .02!
    Dim StartLoop! = CSng(-PI)
    Dim Decay = 1!
    
    Do
       EndLoop = -StartLoop
       For X! = StartLoop To EndLoop Step StepSize
          Y = CSng((Abs(Cos(X)) * Decay) - .14!)
    
          ' The first PUT statement places the image on
          ' the screen:
          PUT(X, Y, Array, PutOption.PUT_XOR)
    
          ' An empty FOR...NEXT loop to delay the program and
          ' reduce image flicker:
          For I = 1! To 5! : EMPTYLOOP() : Next I
    
          If Y < (-.13!) Then Decay = Decay * .9!
          Esc$ = INKEY$()
          If (Esc$ <> "") Or (Decay < .01!) Then Exit For
    
          ' The second PUT statement erases the image and
          ' restores the background:
          PUT(X, Y, Array, PutOption.PUT_XOR)
       Next X
    
       StepSize = -StepSize
       StartLoop = -StartLoop
    Loop Until (Esc$ <> "") Or (Decay < .01!)
    
    Dim Pause$ = InputStr(1)
    End
  End Sub

  ' $STATIC
  ' $DYNAMIC
  Function GetArraySize(ByRef WLeft As Single, ByRef WRight As Single, ByRef WTop As Single, ByRef WBottom As Single) As Single
    Static VLeft!
    Static VRight!
    Static VTop!
    Static VBottom!
    Static RectHeight!
    Static RectWidth!
    Static ByteSize!
     VLeft = CSng(PMAP(WLeft, 0))
     VRight = CSng(PMAP(WRight, 0))
     VTop = CSng(PMAP(WTop, 1))
     VBottom = CSng(PMAP(WBottom, 1))
  
     RectHeight = Abs(VBottom - VTop) + 1
     RectWidth = Abs(VRight - VLeft) + 1
  
     ByteSize = 4 + (RectHeight * Int((RectWidth + 7) / 8))
     GetArraySize = (CInt(ByteSize) \ 2) + 1
  End Function
  
End Module
