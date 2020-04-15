Option Explicit On
Option Strict On
Option Infer On

Imports System.IO
Imports QB.Core

Namespace Global.QB

  Public NotInheritable Class File

    ' Contains anything that is doable using Microsoft.VisualBasic.FileSystem
    ' Recommendation: Either import this namespace -OR- convert to use MS.VB.FS.

    Private Sub New()
    End Sub

    Friend Structure FileThing
      Public Filename As String
      Public FileSize As Long
      Public Stream As IO.FileStream
    End Structure

    Friend Shared m_files As New Dictionary(Of Integer, FileThing)

    Public Shared Sub CLOSE(ParamArray filenums%())

      If filenums%.Count = 0 Then
        ' Close all files.
        For filenum% = 1 To 255
          If m_files.ContainsKey(filenum%) Then
            m_files(filenum%).Stream.Close()
            m_files.Remove(filenum%)
          End If
        Next
      Else
        ' Close one or more specific files.
        For Each filenum% In filenums%
          If m_files.ContainsKey(filenum%) Then
            m_files(filenum%).Stream.Close()
            m_files.Remove(filenum%)
          End If
        Next
      End If

    End Sub

    Public Shared Function EOF(filenum%) As Boolean
      If m_files.ContainsKey(filenum%) Then
        If m_files(filenum%).Stream.Position >= m_files(filenum%).Stream.Length - 1 Then
          Return True
        Else
          Return False
        End If
      Else
        Throw New ArgumentException("Bad file name or number")
      End If
    End Function

    Public Shared Function LOF(filenum%) As Long
      If m_files.ContainsKey(filenum%) Then
        If m_files(filenum%).Stream.Position >= m_files(filenum%).Stream.Length - 1 Then
          Return m_files(filenum%).FileSize
        Else
          Return -1
        End If
      Else
        Throw New ArgumentException("Bad file name or number")
      End If
    End Function

    Public Shared Function FREEFILE() As Integer
      If m_files.Count > 15 Then
        Throw New InvalidOperationException("Too many files")
      End If
      For index = 1 To 255
        If Not m_files.ContainsKey(index) Then
          Return index
        End If
      Next
      Throw New Exception() ' <--- Should never reach here.
    End Function

    Public Shared Sub OPEN(file$, accessMode As OpenMode, filenum%)

      Dim fm As IO.FileMode = IO.FileMode.Open
      Select Case accessMode
        Case OpenMode.Append : fm = IO.FileMode.Append
        Case OpenMode.Binary : fm = IO.FileMode.OpenOrCreate
        Case OpenMode.Input : fm = IO.FileMode.Open
        Case OpenMode.Output : fm = IO.FileMode.OpenOrCreate
        Case OpenMode.Random : fm = IO.FileMode.OpenOrCreate
        Case Else
      End Select

      Select Case accessMode
        Case OpenMode.Output
          If IO.File.Exists(file$) Then
            IO.File.Delete(file$)
          End If
      End Select

      If m_files.ContainsKey(filenum%) Then
        Throw New ArgumentException
      Else
        Dim thing As FileThing
        thing.Filename = file$
        Dim fi = New FileInfo(file$)
        thing.FileSize = fi.Length
        thing.Stream = New IO.FileStream(file$, fm)
        m_files.Add(filenum%, thing)
      End If

    End Sub

    Public Shared Sub NAME(oldfilespec$, newfilespec$)
      IO.File.Move(oldfilespec$, newfilespec$)
    End Sub

  End Class

End Namespace