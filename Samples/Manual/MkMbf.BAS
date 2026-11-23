' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/CVSMBF.htm
TYPE Buffer
    SngNum AS STRING * 4
    DblNum AS STRING * 8
END TYPE
DIM RecBuffer AS Buffer
OPEN "TESTDAT.DAT" FOR RANDOM AS #1 LEN = 12
SNum = 98.9
DNum = 645.3235622#
RecBuffer.SngNum = MKSMBF$(SNum)
RecBuffer.DblNum = MKDMBF$(DNum)
PUT #1, 1, RecBuffer
GET #1, 1, RecBuffer
CLOSE #1
PRINT CVSMBF(RecBuffer.SngNum), CVDMBF(RecBuffer.DblNum)
