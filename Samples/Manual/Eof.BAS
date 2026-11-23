' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/EOF.htm
CLS
OPEN "TEST.DAT" FOR OUTPUT AS #1
FOR i% = 1 TO 10
    WRITE #1, i%, 2 * i%, 5 * i%
NEXT i%
CLOSE #1
OPEN "TEST.DAT" FOR INPUT AS #1
DO
    LINE INPUT #1, a$
    PRINT a$
LOOP UNTIL (EOF(1))
