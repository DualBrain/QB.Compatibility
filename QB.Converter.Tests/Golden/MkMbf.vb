' Converted from QBasic by qb2vb.

Imports System.Math
Imports QB
Imports QB.Core
Imports QB.Development
Imports QB.Devices
Imports QB.Events
Imports QB.File
Imports QB.Multimedia
Imports QB.Printer
Imports QB.Video

Module Program

  Structure Buffer
    Implements IFileRecord
    Private m_SngNum As String
    Public Property SngNum As String
      Get
        Return If(m_SngNum, FixedString.Blank(4))
      End Get
      Set(value As String)
        m_SngNum = FixedString.Fit(value, 4)
      End Set
    End Property
    Private m_DblNum As String
    Public Property DblNum As String
      Get
        Return If(m_DblNum, FixedString.Blank(8))
      End Get
      Set(value As String)
        m_DblNum = FixedString.Fit(value, 8)
      End Set
    End Property
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(SngNum, 4)
      writer.WriteString(DblNum, 8)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      SngNum = reader.ReadString(4)
      DblNum = reader.ReadString(8)
    End Sub
  End Structure
  

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Function/CVSMBF.htm
    ' TYPE Buffer (see the Structure at the top)
    Dim RecBuffer As Buffer
    OPEN("TESTDAT.DAT", OpenMode.Random, 1, 12)
    Dim SNum! = 98.9!
    Dim DNum! = 645.3235622#
    RecBuffer.SngNum = MKSMBF$(SNum)
    RecBuffer.DblNum = MKDMBF$(DNum)
    PUT(1, 1, RecBuffer)
    [GET](1, 1, RecBuffer)
    CLOSE(1)
    PRINT(Str$(CVSMBF(RecBuffer.SngNum)) & " " & vbTab & Str$(CVDMBF(RecBuffer.DblNum)) & " ")
  End Sub

End Module
