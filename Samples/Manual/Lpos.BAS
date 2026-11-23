' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/LPOS.htm
LPRINT
FOR i% = 1 TO 20
    LPRINT i%;
    IF LPOS(1) >= 10 THEN LPRINT     'Begin a new line.
NEXT i%
