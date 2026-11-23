' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/CLS.htm
RANDOMIZE TIMER
SCREEN 1
' Set up a graphics viewport with a border.
VIEW (5,5)-(100,80),3,1
' Set up a text viewport.
VIEW PRINT 12 TO 24
' Print a message on the screen outside the text viewport.
LOCATE 25,1 : PRINT "Press any key to stop."
Count=0
DO
   ' Draw a circle with a random radius.
   CIRCLE (50,40),INT((35-4)*RND+5),(Count MOD 4)
   ' Clear the graphics viewport every 30 times.
   IF (Count MOD 30)=0 THEN CLS 1
   PRINT "Hello. ";
   ' Clear the text viewport every 45 times.
   IF (Count MOD 45)=0 THEN CLS 2
   Count=Count+1
LOOP UNTIL INKEY$ <> ""
