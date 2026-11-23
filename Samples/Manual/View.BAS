' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/VIEW.htm
'This example requires a color graphics adapter.
SCREEN 1
VIEW (10, 10)-(300, 180), , 1
LOCATE 1, 11: PRINT "A big graphics viewport";
VIEW SCREEN (80, 80)-(200, 125), , 1
LOCATE 11, 11: PRINT "A small graphics viewport";
