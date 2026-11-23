' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/ON%20KEY.htm
'This example requires Caps Lock and Num Lock to be off.
CONST ESC = 27
KEY 15, CHR$(&H4) + CHR$(&H1F)              'Set up Ctrl+S as KEY 15.
ON KEY(15) GOSUB PauseHandler
KEY(15) ON
WHILE INKEY$ <> CHR$(ESC)
    PRINT "Press Esc to stop, Ctrl+S to pause."
    PRINT
WEND
END

PauseHandler:
    SLEEP 1
    RETURN
