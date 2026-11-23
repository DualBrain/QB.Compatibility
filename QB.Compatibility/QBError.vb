Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  ''' <summary>
  ''' QBasic runtime errors with their numbers: an exception built here has the QBasic number as its <c>Err.Number</c>, so
  ''' ON ERROR code (<c>SELECT CASE ERR</c>) works.  Common ones: 4 Out of DATA, 5 Illegal function call, 6 Overflow,
  ''' 9 Subscript out of range, 11 Division by zero, 13 Type mismatch, 24 Device timeout, 25 Device fault, 50 FIELD overflow,
  ''' 52 Bad file name or number, 53 File not found, 54 Bad file mode, 55 File already open, 58 File already exists,
  ''' 59 Bad record length, 62 Input past end of file, 63 Bad record number, 64 Bad file name, 67 Too many files,
  ''' 70 Permission denied, 75 Path/File access error, 76 Path not found.
  ''' </summary>
  Friend NotInheritable Class QBError

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Raises a QBasic error.  This calls VB's Err.Raise right where the error happens (catching and rethrowing the exception
    ''' would lose the number), so <c>Err.Number</c> is the QBasic number in the program's error handler.
    ''' </summary>
    Friend Shared Sub Raise(number As Integer, description As String)
      Microsoft.VisualBasic.Information.Err.Raise(number, "QB", description)
    End Sub

    ''' <summary>QBasic's message for an error number.</summary>
    Friend Shared Function Message(number As Integer) As String
      Select Case number
        Case 1 : Return "NEXT without FOR"
        Case 2 : Return "Syntax error"
        Case 3 : Return "RETURN without GOSUB"
        Case 4 : Return "Out of DATA"
        Case 5 : Return "Illegal function call"
        Case 6 : Return "Overflow"
        Case 7 : Return "Out of memory"
        Case 8 : Return "Label not defined"
        Case 9 : Return "Subscript out of range"
        Case 10 : Return "Duplicate definition"
        Case 11 : Return "Division by zero"
        Case 13 : Return "Type mismatch"
        Case 14 : Return "Out of string space"
        Case 16 : Return "String formula too complex"
        Case 17 : Return "Cannot continue"
        Case 18 : Return "Function not defined"
        Case 19 : Return "No RESUME"
        Case 20 : Return "RESUME without error"
        Case 24 : Return "Device timeout"
        Case 25 : Return "Device fault"
        Case 26 : Return "FOR without NEXT"
        Case 27 : Return "Out of paper"
        Case 29 : Return "WHILE without WEND"
        Case 30 : Return "WEND without WHILE"
        Case 33 : Return "Duplicate label"
        Case 35 : Return "Subprogram not defined"
        Case 37 : Return "Argument-count mismatch"
        Case 38 : Return "Array not defined"
        Case 40 : Return "Variable required"
        Case 50 : Return "FIELD overflow"
        Case 51 : Return "Internal error"
        Case 52 : Return "Bad file name or number"
        Case 53 : Return "File not found"
        Case 54 : Return "Bad file mode"
        Case 55 : Return "File already open"
        Case 56 : Return "FIELD statement active"
        Case 57 : Return "Device I/O error"
        Case 58 : Return "File already exists"
        Case 59 : Return "Bad record length"
        Case 61 : Return "Disk full"
        Case 62 : Return "Input past end of file"
        Case 63 : Return "Bad record number"
        Case 64 : Return "Bad file name"
        Case 67 : Return "Too many files"
        Case 68 : Return "Device unavailable"
        Case 69 : Return "Communication-buffer overflow"
        Case 70 : Return "Permission denied"
        Case 71 : Return "Disk not ready"
        Case 72 : Return "Disk-media error"
        Case 73 : Return "Advanced feature unavailable"
        Case 74 : Return "Rename across disks"
        Case 75 : Return "Path/File access error"
        Case 76 : Return "Path not found"
        Case Else : Return "Unprintable error"
      End Select
    End Function

  End Class

End Namespace
