' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement%20%28Graphics%29/PUT.htm
'This example requires a color graphics adapter.
SCREEN 1
DIM Box%(1 TO 200)
x1% = 0: x2% = 10: y1% = 0: y2% = 10
LINE (x1%, y1%)-(x2%, y2%), 2, BF
GET (x1%, y1%)-(x2%, y2%), Box%
DO
    PUT (x1%, y1%), Box%, XOR
    x1% = RND * 300
    y1% = RND * 180
    PUT (x1%, y1%), Box%
LOOP WHILE INKEY$ = ""
