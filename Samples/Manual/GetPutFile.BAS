' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement%20%28File%20I%C2%A7O%29/GET.htm
TYPE TestRecord
    Student AS STRING * 20
    Score AS SINGLE
END TYPE
DIM MyClass AS TestRecord
OPEN "FINAL.DAT" FOR RANDOM AS #1 LEN = LEN(MyClass)
MyClass.Student = "MarySa"
MyClass.Score = 99
PUT #1, 1, MyClass
CLOSE #1
OPEN "FINAL.DAT" FOR RANDOM AS #1 LEN = LEN(MyClass)
GET #1, 1, MyClass
PRINT "STUDENT:", MyClass.Student
PRINT "SCORE:", MyClass.Score
CLOSE #1
KILL "FINAL.DAT"
