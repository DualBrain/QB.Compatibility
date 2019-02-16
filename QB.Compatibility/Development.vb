Option Explicit On
Option Strict On
Option Infer On

Namespace Global

  Partial Public NotInheritable Class QB

    ' DOS and Program Management

    Public Shared Sub RUN(path$)
      Process.Start(path$)
    End Sub

    Public Shared Sub SHELL()

    End Sub

    Public Shared Sub SHELL(value As String)

    End Sub

    Public Shared Sub SYSTEM()
      'TODO: End 'NOTE: End statement cannot be used in a class library project.
    End Sub

    ' Port and Memory

    Public Shared Function FRE%(value%)
      Return 0
    End Function

    Public Shared Function INP%(address%)
      Return 0
    End Function

    Public Shared Sub OUT(address%, value%)

    End Sub

    Public Shared Function PEEK(address As Integer) As Integer
      Return 0
    End Function

    Public Shared Function SETMEM(value As Long) As Long
      Return 0
    End Function

    Public Shared Function STACK() As Integer
      Return 0
    End Function

    Public Shared Function STACK(num%) As Integer
      Return 0
    End Function

    ' Mixed Language

    Public Shared Function VARSEG%(address&)
      Return 0
    End Function

    ' Metacommands

    ' Debugging

  End Class

End Namespace