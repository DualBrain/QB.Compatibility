DECLARE SUB quickSort (arr() AS STRING, first AS INTEGER, last AS INTEGER)
DECLARE FUNCTION formatOID$ (oid AS STRING)

DIM i AS INTEGER, j AS INTEGER
DIM oids(5) AS STRING
oids(0) = "1.3.6.1.4.1.11.2.17.19.3.4.0.10"
oids(1) = "1.3.6.1.4.1.11.2.17.5.2.0.79"
oids(2) = "1.3.6.1.4.1.11.2.17.19.3.4.0.4"
oids(3) = "1.3.6.1.4.1.11150.3.4.0.1"
oids(4) = "1.3.6.1.4.1.11.2.17.19.3.4.0.1"
oids(5) = "1.3.6.1.4.1.11150.3.4.0"

FOR i = 0 TO UBOUND(oids)
    oids(i) = formatOID$(oids(i))
NEXT

quickSort oids(), 0, UBOUND(oids)

FOR i = 0 TO UBOUND(oids)
    DIM tmp AS STRING: tmp = ""
    FOR j = 1 TO LEN(oids(i))
        IF MID$(oids(i), j, 1) <> " " THEN tmp = tmp + MID$(oids(i), j, 1)
    NEXT
    PRINT tmp
NEXT
END

FUNCTION formatOID$ (oid AS STRING)
    DIM result AS STRING: result = ""
    DIM segment AS STRING: segment = ""
    DIM i AS INTEGER: i = 1
    DIM lenOID AS INTEGER: lenOID = LEN(oid)

    WHILE i <= lenOID
        IF MID$(oid, i, 1) = "." THEN
            result = result + SPACE$(5 - LEN(segment)) + segment + "."
            segment = ""
        ELSE
            segment = segment + MID$(oid, i, 1)
        END IF
        i = i + 1
    WEND
    result = result + SPACE$(5 - LEN(segment)) + segment
    formatOID = result
END FUNCTION

SUB quickSort (arr() AS STRING, first AS INTEGER, last AS INTEGER)
    IF first >= last THEN EXIT SUB

    DIM i AS INTEGER: i = first
    DIM j AS INTEGER: j = last
    DIM pivot AS STRING: pivot = arr((first + last) \ 2)
    DIM temp AS STRING

    DO
        WHILE arr(i) < pivot: i = i + 1: WEND
        WHILE arr(j) > pivot: j = j - 1: WEND

        IF i <= j THEN
            ' Swap
            temp = arr(i)
            arr(i) = arr(j)
            arr(j) = temp
            i = i + 1
            j = j - 1
        END IF
    LOOP UNTIL i > j

    IF first < j THEN quickSort arr(), first, j
    IF i < last THEN quickSort arr(), i, last
END SUB
