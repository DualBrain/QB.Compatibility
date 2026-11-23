' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement%20%28Music%29/PLAY.htm
'Play scale in 7 different octaves
scale$ = "CDEFGAB"
PLAY "L16"
FOR i% = 0 TO 6
    PLAY "O" + STR$(i%)
    PLAY "X" + VARPTR$(scale$)
NEXT i%
