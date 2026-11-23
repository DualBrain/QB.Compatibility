' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/POS.htm
CLS
LOCATE 5, 5
MyRow% = CSRLIN
MyCol% = POS(0)
PRINT "Position 1 (Press any key)"
DO
LOOP WHILE INKEY$ = ""
LOCATE (MyRow% + 2), (MyCol% + 2)
PRINT "Position 2"
