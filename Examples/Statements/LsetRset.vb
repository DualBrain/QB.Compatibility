' LSET / RSET / FIELD  -  https://qbasic.net/en/qb-manual/Statement/LSET.htm
' A VB translation of the QBasic example.  VB can't bind variables to a file buffer, so FIELD returns a FieldRecord whose
' fields are numbered: FIELD #1, 5 AS Ls$, 5 AS Rs$ is Dim f = FIELD(1, 5, 5), LSET Ls$ = "LSET" is f.LSET(0, "LSET"),
' and PUT #1, 1 / GET #1, 1 are PUT_FIELD(1, 1) / GET_FIELD(1, 1).

Imports QB
Imports QB.File
Imports QB.Graphics

Namespace Examples

  Friend Module LsetRsetExample

    Public Sub Run()
      OPEN("FILEDAT.DAT", OpenMode.Random, 1, 10)
      Dim fields1 = FIELD(1, 5, 5)
      fields1.LSET(0, "LSET")
      fields1.RSET(1, "RSET")
      PUT_FIELD(1, 1)
      CLOSE(1)
      OPEN("FILEDAT.DAT", OpenMode.Random, 1, 10)
      Dim fields2 = FIELD(1, 5, 5)
      GET_FIELD(1, 1)
      PRINT("*" & fields2(0) & "*" & vbTab & "*" & fields2(1) & "*")
      CLOSE(1)
      KILL("FILEDAT.DAT") ' (Clean up; not in the original.)
    End Sub

  End Module

End Namespace
