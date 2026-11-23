' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/PRINT%20%23.htm
OPEN "TEST.DAT" FOR OUTPUT AS #1
PRINT #1, USING "##.###  "; 12.12345
CLOSE
OPEN "TEST.DAT" FOR INPUT AS #1
INPUT #1, a$
PRINT a$
LPRINT "This is a line"; 1
LPRINT "This is a line",
LPRINT 2
