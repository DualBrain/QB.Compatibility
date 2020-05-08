Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  Partial Public NotInheritable Class Development

    ' DOS and Program Management

    Public Shared Sub RUN(path$)
      Process.Start(path$)
    End Sub

    'Public Shared Sub SHELL()

    'End Sub

    'Public Shared Sub SHELL(value As String)

    'End Sub

    Public Shared Sub SYSTEM()
      'TODO: 'NOTE: End statement cannot be used in a class library project.
      Throw New NotImplementedException
    End Sub

    ' Port and Memory

    Public Shared Function FRE%(value%)
      If value <> 0 Then
      End If
      Throw New NotImplementedException
      Return 0
    End Function

    Public Shared Function INP%(address%)
      If address <> 0 Then
        ' Do nothing...
      Else
        ' Do nothing...
      End If
      'Throw New NotImplementedException
      Return 0
    End Function

    Public Shared Sub OUT(address%, value%)
      If address <> 0 OrElse value <> 0 Then
        ' Do nothing...
      Else
        ' Do nothing...
      End If
      'Throw New NotImplementedException
    End Sub

    Public Shared Function PEEK(address As Integer) As Integer
      If address <> 0 Then
      End If
      Throw New NotImplementedException
      Return 0
    End Function

    Public Shared Function POKE(address As Integer, value%) As Integer
      If address <> 0 OrElse value <> 0 Then
      End If
      Throw New NotImplementedException
      Return 0
    End Function

    Public Shared Function SETMEM(value As Long) As Long
      If value <> 0 Then
      End If
      Return 0
    End Function

    Public Shared Function STACK() As Integer
      Return 0
    End Function

    Public Shared Function STACK(num%) As Integer
      If num <> 0 Then
      End If
      Return 0
    End Function

    ' Mixed Language

    Public Shared Function VARSEG%(address&)
      If address <> 0 Then
      End If
      Throw New NotImplementedException
      Return 0
    End Function

    ' Metacommands

    ' Debugging

  End Class

End Namespace