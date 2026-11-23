' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/ON%20PEN.htm
    'This example requires a light pen.
    ON PEN GOSUB Handler
    PEN ON
    PRINT "Press Esc to exit."
    DO UNTIL INKEY$ = CHR$(27): LOOP
    END

Handler:
    PRINT "Pen is at row"; PEN(6); ", column"; PEN(7)
    RETURN
