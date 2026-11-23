' GET (file I/O)  -  https://qbasic.net/en/qb-manual/Statement%20%28File%20I%C2%A7O%29/GET.htm
' A VB translation of the QBasic example.  The TYPE becomes a Structure that implements IFileRecord, which
' works in trimmed and Native AOT builds; a Structure with <VBFixedString(n)> strings and no IFileRecord
' also works when the program isn't compiled ahead of time.

Imports QB
Imports QB.File
Imports QB.Video

Namespace Examples

  Friend Module GetPutFileExample

    ' TYPE TestRecord
    '   Student AS STRING * 20
    '   Score AS SINGLE
    ' END TYPE
    Structure TestRecord
      Implements IFileRecord

      <VBFixedString(20)> Public Student As String
      Public Score As Single

      Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
        writer.WriteString(Student, 20)
        writer.Write(Score)
      End Sub

      Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
        Student = reader.ReadString(20)
        Score = reader.ReadSingle()
      End Sub

    End Structure

    Public Sub Run()
      Dim record As New TestRecord
      OPEN("FINAL.DAT", OpenMode.Random, 1, LEN(record))
      record.Student = "MarySa"
      record.Score = 99
      PUT(1, 1, record)
      CLOSE(1)
      OPEN("FINAL.DAT", OpenMode.Random, 1, LEN(record))
      [GET](1, 1, record)
      PRINT("STUDENT:" & vbTab & record.Student)
      PRINT("SCORE:" & vbTab & Str(record.Score))
      CLOSE(1)
      KILL("FINAL.DAT")
    End Sub

  End Module

End Namespace
