' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/LSET.htm
OPEN "FILEDAT.DAT" FOR RANDOM AS #1 LEN = 10
FIELD #1, 5 AS Ls1$, 5 AS Rs1$
LSET Ls1$ = "LSET"
RSET Rs1$ = "RSET"
PUT #1, 1
CLOSE #1
OPEN "FILEDAT.DAT" FOR RANDOM AS #1 LEN = 10
FIELD #1, 5 AS Ls2$, 5 AS Rs2$
GET #1, 1
PRINT "*" + Ls2$ + "*", "*" + Rs2$ + "*"
CLOSE #1
