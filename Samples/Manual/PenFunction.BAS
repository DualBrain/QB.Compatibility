' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/PEN.htm
DO
  P = PEN(3)
  LOCATE 1, 1: PRINT "Pen is ";
  IF P THEN PRINT "down" ELSE PRINT "up  "
  PRINT "X ="; PEN(4), " Y ="; PEN(5); "  "
LOOP
