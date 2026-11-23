' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/INPUT%24.htm
OPEN "TEST.DAT" FOR OUTPUT AS #1
PRINT #1, "The text"
CLOSE
OPEN "TEST.DAT" FOR INPUT AS #1
PRINT INPUT$(3, 1)        'Print first 3 characters.
CLOSE
