' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/INPUT.htm
CLS
OPEN "LIST" FOR OUTPUT AS #1
DO
    INPUT "   NAME:       ", Name$  'Read entries from the keyboard.
    INPUT "   AGE:        ", Age$
    WRITE #1, Name$, Age$
    INPUT "Add another entry"; R$
LOOP WHILE UCASE$(R$) = "Y"
CLOSE #1
'Echo the file back.
OPEN "LIST" FOR INPUT AS #1
CLS
PRINT "Entries in file:": PRINT
DO WHILE NOT EOF(1)
    LINE INPUT #1, REC$  'Read entries from the file.
    PRINT REC$           'Print the entries on the screen.
LOOP
CLOSE #1
KILL "LIST"
