CONST BALLS = 80 ' Maximum number of BALLS
DIM Bx(BALLS - 1) AS INTEGER, By(BALLS - 1) AS INTEGER ' Character cell coordinates of each ball
DIM w AS INTEGER, i AS INTEGER, j AS INTEGER, peg AS INTEGER, direc AS INTEGER

SCREEN 0: WIDTH 80, 50
COLOR 15
CLS

w = 80 ' Screen width in characters

' Draw pegs
FOR peg = 1 TO 10
    FOR i = 0 TO peg
        LOCATE 6 + peg, 6 + (12 - peg) + i * 2: PRINT "*";
    NEXT
NEXT

' Draw slots
FOR j = 0 TO 11
    FOR i = 0 TO 11
        LOCATE 29 + j, 7 + i * 2: PRINT ": ";
    NEXT
NEXT

' Draw bottom
FOR i = 0 TO 22
    LOCATE 41, 7 + i: PRINT ".";
NEXT

' Initial ball positions
FOR i = 0 TO BALLS - 1
    Bx(i) = 17
    By(i) = 1
NEXT

COLOR 12 ' Make BALLS bright red

' Animation loop
DO
    FOR i = 0 TO BALLS - 1
        LOCATE By(i), Bx(i): PRINT " " ' Erase ball's initial position

        ' Check below
        IF SCREEN(By(i) + 1, Bx(i)) = 32 THEN ' Is ball above empty location?
            By(i) = By(i) + 1 ' Yes: fall straight down
        ELSE
            direc = INT(RND * 3) - 1 ' No: randomly fall right or left
            IF SCREEN(By(i) + 1, Bx(i) + direc) = 32 THEN
                Bx(i) = Bx(i) + direc
                By(i) = By(i) + 1
            END IF
        END IF

        LOCATE By(i), Bx(i): PRINT "o" ' Continue until a key is struck
    NEXT

    SLEEP 1
LOOP UNTIL INKEY$ <> "" ' Continue until a key is struck
