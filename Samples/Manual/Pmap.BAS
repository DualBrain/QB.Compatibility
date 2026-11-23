' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/PMAP.htm
'This example requires a graphics adapter that supports screen mode 1.
SCREEN 1
WINDOW SCREEN (0, 0)-(100, 100)
PRINT "Logical x=50, physical x="; PMAP(50, 0)
PRINT "Logical y=50, physical y="; PMAP(50, 1)
