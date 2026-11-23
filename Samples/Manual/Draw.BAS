' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/DRAW.htm
SCREEN 1
Triangle$ = "F60 L120 E60"
DRAW "C2 X" + VARPTR$(Triangle$)
DRAW "BD30 P1,2 C3 M-30,-30"
