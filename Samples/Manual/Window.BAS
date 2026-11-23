' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/WINDOW.htm
'This example requires a color graphics adapter.
SCREEN 1
FOR i% = 1 TO 10 STEP 2
    WINDOW (-160 / i%, -100 / i%)-(160 / i%, 100 / i%)
    CIRCLE (0, 0), 10
NEXT i%
