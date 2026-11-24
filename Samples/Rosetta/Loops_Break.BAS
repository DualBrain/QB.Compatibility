RANDOMIZE TIMER
DO
    a = INT(RND * 20)
    PRINT a
    IF a = 10 THEN EXIT DO
    b = INT(RND * 20)
    PRINT b
LOOP
