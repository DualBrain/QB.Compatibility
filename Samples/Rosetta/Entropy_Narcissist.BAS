DIM i AS INTEGER, byteValue AS INTEGER
DIM totalchar(0 TO 255) AS LONG
DIM entropy AS DOUBLE, prop AS DOUBLE
Dim c AS INTEGER

DIM filename AS STRING: filename = "ENTROPY.BAS"
DIM ff AS INTEGER: ff = FREEFILE

OPEN filename FOR BINARY AS #ff

IF ERR <> 0 THEN
    PRINT "Error opening the file"
    BEEP
    END
END IF

DIM fileSize AS LONG: fileSize = LOF(ff)

IF fileSize = 0 THEN
    CLOSE #ff
    PRINT "The file is empty"
    END
END IF

' Read one byte at a time.
' In QBasic, buffer must be a string of length 1.
DIM buffer AS STRING: buffer = SPACE$(1)

FOR i = 1 TO fileSize
    GET #ff, , buffer

    byteValue = ASC(buffer)
    totalchar(byteValue) = totalchar(byteValue) + 1
NEXT i

CLOSE #ff

FOR i = 0 TO 255
    IF totalchar(i) <> 0 THEN
        prop = totalchar(i) / fileSize
        entropy = entropy - prop * (LOG(prop) / LOG(2))
    END IF
NEXT i

i = 0
FOR c = LEN(filename) TO 1 STEP -1
    IF MID$(filename, i, 1) = "\" OR MID$(filename, i, 1) = "/" THEN i = c: EXIT FOR
NEXT c

IF i > 0 THEN filename = MID$(filename, i + 1)

PRINT "My name is "; filename
PRINT
PRINT "The Entropy of myself is"; entropy
END
