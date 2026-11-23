' MKSMBF$ / MKDMBF$ / CVSMBF / CVDMBF  -  https://qbasic.net/en/qb-manual/Function/CVSMBF.htm
' A VB translation of the QBasic example.  The TYPE becomes a Structure that implements IFileRecord (see GET and PUT for files).

Imports QB
Imports QB.Devices
Imports QB.File
Imports QB.Graphics

Namespace Examples

  Friend Module MkMbfExample

    ' TYPE Buffer
    '   SngNum AS STRING * 4
    '   DblNum AS STRING * 8
    ' END TYPE
    Structure Buffer
      Implements IFileRecord

      <VBFixedString(4)> Public SngNum As String
      <VBFixedString(8)> Public DblNum As String

      Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
        writer.WriteString(SngNum, 4)
        writer.WriteString(DblNum, 8)
      End Sub

      Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
        SngNum = reader.ReadString(4)
        DblNum = reader.ReadString(8)
      End Sub

    End Structure

    Public Sub Run()
      Dim recBuffer As New Buffer
      OPEN("TESTDAT.DAT", OpenMode.Random, 1, 12)
      Dim sNum As Single = 98.9F
      Dim dNum As Double = 645.3235622
      recBuffer.SngNum = MKSMBF(sNum)
      recBuffer.DblNum = MKDMBF(dNum)
      PUT(1, 1, recBuffer)
      [GET](1, 1, recBuffer)
      CLOSE(1)
      PRINT(Str(CVSMBF(recBuffer.SngNum)) & vbTab & Str(CVDMBF(recBuffer.DblNum)))
      KILL("TESTDAT.DAT") ' (Clean up; not in the original.)
    End Sub

  End Module

End Namespace
