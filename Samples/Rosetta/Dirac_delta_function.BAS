DECLARE FUNCTION diracDelta$ (x AS DOUBLE)

PRINT diracDelta$(0)
PRINT diracDelta$(1)
END

FUNCTION diracDelta$ (x AS DOUBLE)
    diracDelta$ = "0"
    IF x = 0 THEN diracDelta$ = "Infinity"
END FUNCTION
