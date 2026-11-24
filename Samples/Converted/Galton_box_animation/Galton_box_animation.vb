' Converted from QBasic by qb2vb.

Module Program

  Const BALLS As Short = 80

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' CONST BALLS = 80 ' Maximum number of BALLS
    Dim Bx(BALLS - 1) As Short, By(BALLS - 1) As Short  ' Character cell coordinates of each ball
    Dim w As Short, i As Short, j As Short, peg As Short, direc As Short
    
    SCREEN(0) : WIDTH(80, 50)
    COLOR(15)
    CLS()
    
    w = 80  ' Screen width in characters
    
    ' Draw pegs
    For peg = 1 To 10
        For i = 0 To peg
            LOCATE(6 + peg, (6 + (12 - peg)) + (i * 2)) : PRINT("*", True)
        Next
    Next
    
    ' Draw slots
    For j = 0 To 11
        For i = 0 To 11
            LOCATE(29 + j, 7 + (i * 2)) : PRINT(": ", True)
        Next
    Next
    
    ' Draw bottom
    For i = 0 To 22
        LOCATE(41, 7 + i) : PRINT(".", True)
    Next
    
    ' Initial ball positions
    For i = 0 To CShort(BALLS - 1)
        Bx(i) = 17
        By(i) = 1
    Next
    
    COLOR(12)  ' Make BALLS bright red
    
    ' Animation loop
    Do
        For i = 0 To CShort(BALLS - 1)
            LOCATE(By(i), Bx(i)) : PRINT(" ")  ' Erase ball's initial position
    
            ' Check below
            If SCREEN(By(i) + 1, Bx(i)) = 32 Then  ' Is ball above empty location?
                By(i) = CShort(By(i) + 1)  ' Yes: fall straight down
            Else
                direc = CShort(Int(Rnd() * 3) - 1)  ' No: randomly fall right or left
                If SCREEN(By(i) + 1, Bx(i) + direc) = 32 Then
                    Bx(i) = Bx(i) + direc
                    By(i) = CShort(By(i) + 1)
                End If
            End If
    
            LOCATE(By(i), Bx(i)) : PRINT("o")  ' Continue until a key is struck
        Next
    
        SLEEP(1)
    Loop Until INKEY$() <> ""  ' Continue until a key is struck
  End Sub

End Module
