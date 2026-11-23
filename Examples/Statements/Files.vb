' FILES  -  https://qbasic.net/en/qb-manual/Statement/FILES.htm
' A VB translation of the QBasic example (the DOS paths are dropped: it works in the current directory).

Imports QB
Imports QB.File
Imports QB.Graphics

Namespace Examples

  Friend Module FilesExample

    Public Sub Run()
      MKDIR("TEST")
      FILES()
      RMDIR("TEST")
    End Sub

  End Module

End Namespace
