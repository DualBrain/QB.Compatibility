' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/LOF.htm
INPUT "Enter filename: "; f$
OPEN f$ FOR BINARY AS #1
PRINT "File length = "; LOF(1)
CLOSE
