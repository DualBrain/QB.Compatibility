suffix$ = "s"
FOR bottles = 4 TO 1 STEP -1
    PRINT STR$(bottles) + " bottle" + suffix$ + " of beer on the wall, ";
    PRINT STR$(bottles) + " bottle" + suffix$ + " of beer."
    PRINT "Take one down and pass it around, ";
    IF bottles > 1 THEN
        IF bottles = 2 THEN suffix$ = ""
        PRINT STR$(bottles - 1) + " bottle" + suffix$ + " of beer on the wall..."
    ELSE
        PRINT "no more bottles of beer on the wall..."
    END IF
    PRINT
NEXT bottles

PRINT "No more bottles of beer on the wall, no more bottles of beer."
PRINT "Go to the store and buy some more, 99 bottles of beer on the wall."
END
