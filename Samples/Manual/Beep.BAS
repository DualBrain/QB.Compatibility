' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/BEEP.htm
CLS                ' Clear the screen
DO
   INPUT "Hear a beep (Y or N)"; Response$
   R$ = UCASE$ (MID$ (Response$,1,1))
   IF R$ <> "Y" OR R$ = "N" THEN EXIT DO
   BEEP
LOOP
