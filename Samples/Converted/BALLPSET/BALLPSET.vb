' Converted from QBasic by qb2vb.

Module Program

  Const PI As Double = 3.141592653589#

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim EndLoop!
    Dim Y!
    Dim Esc$
    ' DECLARE FUNCTION GetArraySize (WLeft, WRight, WTop, WBottom)
    
    SCREEN(2)
    CLS()
    
    ' Define a viewport and draw a border around it:
    VIEW(20, 10, 620, 190, -1, 1)
    
    ' CONST PI = 3.141592653589#
    
    ' Redefine the coordinates of the viewport with logical
    ' coordinates:
    WINDOW(-3.15!, -.14!, 3.56!, 1.01!)
    
    ' Arrays in program are now dynamic:
    ' $DYNAMIC
    
    ' Calculate the logical coordinates for the top and bottom of a
    ' rectangle large enough to hold the image that will be drawn
    ' with CIRCLE and PAINT:
    Dim WLeft! = -.21!
    Dim WRight! = .21!
    Dim WTop! = .07!
    Dim WBottom! = -.07!
    
    ' Call the GetArraySize function, passing it the rectangle's
    ' logical coordinates:
    Dim ArraySize As Short = CShort(GetArraySize(WLeft, WRight, WTop, WBottom))
    
    Dim Array(ArraySize) As Short
    
    ' Draw and paint the circle:
    CIRCLE(0, 0, .18!)
    PAINT(0, 0)
    
    ' Store the rectangle in Array:
    [GET](WLeft, WTop, WRight, WBottom, Array)
    CLS()
    
    ' Draw a box and fill it with a pattern:
    LINE(-3, .8!, 3.4!, .2!, -1, LineOption.B)
    Dim Pattern$ = ((QBChr(126) + QBChr(0)) + QBChr(126)) + QBChr(126)
    PAINT(0, .5!, Pattern$)
    
    LOCATE(21, 29)
    PRINT("Press any key to end")
    
    ' Initialize loop variables:
    Dim StepSize! = .02!
    Dim StartLoop! = CSng(-PI)
    Dim Decay = 1!
    
    Do
       EndLoop = -StartLoop
       For X! = StartLoop To EndLoop Step StepSize
    
          ' Each time the ball "bounces" (hits the bottom of the
          ' viewport), the Decay variable gets smaller, making the
          ' height of the next bounce smaller:
          Y = CSng((Abs(Cos(X)) * Decay) - .14!)
          If Y < (-.13!) Then Decay = Decay * .9!
    
          ' Stop if a key pressed or if Decay is less than .01:
          Esc$ = INKEY$()
          If (Esc$ <> "") Or (Decay < .01!) Then Exit For
    
          ' Put the image on the screen.  The StepSize offset is
          ' smaller than the border around the circle, so each time
          ' the image moves, it erases any traces left from the
          ' previous PUT (it also erases anything else on the
          ' screen):
          PUT(X, Y, Array, PutOption.PUT_PSET)
       Next X
    
       ' Reverse direction:
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
  
     ' Map the logical coordinates passed to this function to
     ' their physical-coordinate equivalents:
     VLeft = CSng(PMAP(WLeft, 0))
     VRight = CSng(PMAP(WRight, 0))
     VTop = CSng(PMAP(WTop, 1))
     VBottom = CSng(PMAP(WBottom, 1))
  
     ' Calculate the height and width in pixels of the
     ' enclosing rectangle:
     RectHeight = Abs(VBottom - VTop) + 1
     RectWidth = Abs(VRight - VLeft) + 1
  
     ' Calculate size in bytes of array:
     ByteSize = 4 + (RectHeight * Int((RectWidth + 7) / 8))
  
     ' Array is integer, so divide bytes by two:
     GetArraySize = (CInt(ByteSize) \ 2) + 1
  End Function
  
End Module
