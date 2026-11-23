' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/ON%20PLAY.htm
ON PLAY(3) GOSUB Background
PLAY ON
Music$ = "MBo3L8ED+ED+Eo2Bo3DCL2o2A"
PLAY Music$
LOCATE 2, 1: PRINT "Press any key to stop.";
DO WHILE INKEY$ = "": LOOP
END

Background:
    i% = i% + 1
    LOCATE 1, 1: PRINT "Background called "; i%; "time(s)";
    PLAY Music$
    RETURN
