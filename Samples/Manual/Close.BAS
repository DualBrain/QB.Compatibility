' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/CLOSE.htm
CLS
INPUT "Enter filename: ", n$
OPEN n$ FOR OUTPUT AS #1
PRINT #1, "This is saved to the file."
CLOSE
OPEN n$ FOR INPUT AS #1
INPUT #1, a$
PRINT "Read from file: "; a$
CLOSE
