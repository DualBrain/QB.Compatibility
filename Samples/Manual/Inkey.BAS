' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/INKEY%24.htm
PRINT "Press Esc to exit..."
DO
LOOP UNTIL INKEY$ = CHR$(27)    '27 is the ASCII code for Esc.
