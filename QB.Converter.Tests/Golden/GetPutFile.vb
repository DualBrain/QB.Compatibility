' Converted from QBasic by qb2vb.  Lines marked TODO(QB2VB) need attention.
Option Explicit On
Option Strict On
Option Infer On

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

Module GetPutFile

  Structure TestRecord
    Implements IFileRecord
    Private m_Student As String
    Public Property Student As String
      Get
        Return If(m_Student, FixedString.Blank(20))
      End Get
      Set(value As String)
        m_Student = FixedString.Fit(value, 20)
      End Set
    End Property
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
  

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement%20%28File%20I%C2%A7O%29/GET.htm
    ' TYPE TestRecord (see the Structure at the top)
    Dim [MyClass] As TestRecord
    OPEN("FINAL.DAT", OpenMode.Random, 1, Len([MyClass]))
    [MyClass].Student = "MarySa"
    [MyClass].Score = 99
    PUT(1, 1, [MyClass])
    CLOSE(1)
    OPEN("FINAL.DAT", OpenMode.Random, 1, Len([MyClass]))
    [GET](1, 1, [MyClass])
    PRINT("STUDENT:" & vbTab & [MyClass].Student)
    PRINT("SCORE:" & vbTab & Str$([MyClass].Score) & " ")
    CLOSE(1)
    KILL("FINAL.DAT")
  End Sub

End Module
