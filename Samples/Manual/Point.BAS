' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/POINT.htm
SCREEN 1
LINE (0, 0)-(100, 100), 2
LOCATE 14, 1
FOR y% = 1 TO 10
    FOR x% = 1 TO 10
        PRINT POINT(x%, y%);
    NEXT x%
    PRINT
NEXT y%
