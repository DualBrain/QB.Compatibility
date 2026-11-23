Option Explicit On
Option Strict On
Option Infer On

Namespace QB2VB

  ''' <summary>A QBasic type.  As the static type of a VB expression the names mean VB's: Short is 16 bit, Long is a VB Integer (32 bit).</summary>
  Friend Enum QT
    [Short]     ' INTEGER (%): VB Short
    [Long]      ' LONG (&): VB Integer
    [Single]    ' SINGLE (!)
    [Double]    ' DOUBLE (#)
    [String]    ' STRING ($)
    Bool        ' only in expressions (comparisons)
    User        ' a TYPE record
    Unknown
  End Enum

  Friend NotInheritable Class Types

    Private Sub New()
    End Sub

    Friend Shared Function VbName(t As QT) As String
      Select Case t
        Case QT.Short : Return "Short"
        Case QT.Long : Return "Integer"
        Case QT.Single : Return "Single"
        Case QT.Double : Return "Double"
        Case QT.String : Return "String"
        Case QT.Bool : Return "Boolean"
        Case Else : Return "Object"
      End Select
    End Function

    Friend Shared Function Conv(t As QT) As String
      Select Case t
        Case QT.Short : Return "CShort"
        Case QT.Long : Return "CInt"
        Case QT.Single : Return "CSng"
        Case QT.Double : Return "CDbl"
        Case QT.String : Return "CStr"
        Case Else : Return ""
      End Select
    End Function

    Friend Shared Function Suffix(t As QT) As String
      Select Case t
        Case QT.Short : Return "%"
        Case QT.Long : Return "&"
        Case QT.Single : Return "!"
        Case QT.Double : Return "#"
        Case QT.String : Return "$"
        Case Else : Return ""
      End Select
    End Function

    ''' <summary>The suffix VB writes for the type: VB's % is a 32 bit Integer (QBasic's LONG); a 16 bit Short has none.</summary>
    Friend Shared Function VbSuffix(t As QT) As String
      Select Case t
        Case QT.Short : Return ""
        Case QT.Long : Return "%"
        Case Else : Return Suffix(t)
      End Select
    End Function

    Friend Shared Function FromSuffix(c As Char) As QT
      Select Case c
        Case "%"c : Return QT.Short
        Case "&"c : Return QT.Long
        Case "!"c : Return QT.Single
        Case "#"c : Return QT.Double
        Case "$"c : Return QT.String
        Case Else : Return QT.Unknown
      End Select
    End Function

    Friend Shared Function IsNumeric(t As QT) As Boolean
      Return t = QT.Short OrElse t = QT.Long OrElse t = QT.Single OrElse t = QT.Double
    End Function

    ''' <summary>Widening order of the numeric types (VB converts upward by itself).</summary>
    Friend Shared Function Rank(t As QT) As Integer
      Select Case t
        Case QT.Short : Return 0
        Case QT.Long : Return 1
        Case QT.Single : Return 2
        Case QT.Double : Return 3
        Case Else : Return -1
      End Select
    End Function

    Friend Shared Function Wider(a As QT, b As QT) As QT
      Return If(Rank(a) >= Rank(b), a, b)
    End Function

  End Class

  ''' <summary>A variable, array or constant.</summary>
  Friend Class VarSym
    Public QbName As String = ""         ' as written, without any suffix
    Public Qt As QT
    Public VbName As String = ""         ' the name used in the VB (with a suffix when the QBasic had one)
    Public HasSuffix As Boolean
    Public IsArray As Boolean
    Public IsShared As Boolean
    Public IsParam As Boolean
    Public IsConst As Boolean
    Public ConstText As String = ""
    Public Declared As Boolean           ' the declaration has been written
    Public DeclaredAs As Boolean         ' declared with AS, so the type belongs to the bare name
    Public UserType As String = ""
    Public Dims As String = ""           ' for implicit arrays: the bounds text
    Public FixedLen As Integer           ' STRING * n
    Public DeferRank As Integer          ' an array whose DIM is in a procedure with GOSUB routines: declared at the top without bounds, sized by ReDim where the DIM was
    Public FieldVar As String = ""       ' a FIELD variable: the FieldRecord that holds it, and its index in it
    Public FieldIndex As Integer
  End Class

  Friend Class ParamSym
    Public QbName As String = ""
    Public Qt As QT
    Public IsArray As Boolean
    Public HasSuffix As Boolean
    Public UserType As String = ""
    Public Rank As Integer = 1            ' dimensions of an array parameter (from how the body indexes it)
    Public CopyIn As Boolean              ' used by a GOSUB routine, which is a lambda and can't capture a ByRef parameter: the body works on a copy
  End Class

  Friend Class ProcSym
    Public QbName As String = ""
    Public IsDefFn As Boolean             ' DEF FNname: a one-line function (parameters by value)
    Public DefBody As List(Of Token)      ' the expression of a DEF FN
    Public IsFunction As Boolean
    Public RetQt As QT
    Public HasSuffix As Boolean
    Public Params As New List(Of ParamSym)
    Public IsStatic As Boolean
    Public HeaderLine As Integer = -1    ' index of the SUB/FUNCTION line (-1 when only DECLAREd)
  End Class

  Friend Class TypeSym
    Public QbName As String = ""
    Public Fields As New List(Of (Name As String, Qt As QT, Length As Integer, UserType As String))
  End Class

  ''' <summary>The statement words of QBasic (so that a label isn't mistaken for one) and the words VB reserves.</summary>
  Friend NotInheritable Class Keywords

    Private Sub New()
    End Sub

    Private Shared ReadOnly s_statements As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
      "BEEP", "BLOAD", "BSAVE", "CALL", "CHAIN", "CHDIR", "CIRCLE", "CLEAR", "CLOSE", "CLS", "COLOR", "COM", "COMMON", "CONST",
      "DATA", "DECLARE", "DEF", "DEFDBL", "DEFINT", "DEFLNG", "DEFSNG", "DEFSTR", "DIM", "DO", "DRAW", "ELSE", "ELSEIF", "END",
      "ENVIRON", "ERASE", "ERROR", "EXIT", "FIELD", "FILES", "FOR", "FUNCTION", "GET", "GOSUB", "GOTO", "IF", "INPUT", "IOCTL",
      "KEY", "KILL", "LET", "LINE", "LOCATE", "LOCK", "LOOP", "LPRINT", "LSET", "MKDIR", "NAME", "NEXT", "ON", "OPEN", "OPTION",
      "OUT", "PAINT", "PALETTE", "PCOPY", "PEN", "PLAY", "POKE", "PRESET", "PRINT", "PSET", "PUT", "RANDOMIZE", "READ", "REDIM",
      "REM", "RESET", "RESTORE", "RESUME", "RETURN", "RMDIR", "RSET", "RUN", "SCREEN", "SEEK", "SELECT", "SHARED", "SHELL", "SLEEP",
      "SOUND", "STATIC", "STOP", "STRIG", "SUB", "SWAP", "SYSTEM", "THEN", "TIMER", "TROFF", "TRON", "TYPE", "UNLOCK", "VIEW",
      "WAIT", "WEND", "WHILE", "WIDTH", "WINDOW", "WRITE"}

    Friend Shared Function IsStatementWord(word As String) As Boolean
      If word.Length > 0 AndAlso "$%&!#".IndexOf(word(word.Length - 1)) >= 0 Then Return False   ' a name with a suffix is a variable (end$)
      Return s_statements.Contains(word)
    End Function

    Private Shared ReadOnly s_vbReserved As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
      "AddHandler", "AddressOf", "Alias", "And", "AndAlso", "As", "Boolean", "ByRef", "Byte", "ByVal", "Call", "Case", "Catch", "CBool",
      "CByte", "CChar", "CDate", "CDbl", "CDec", "Char", "CInt", "Class", "CLng", "CObj", "Const", "Continue", "CSByte", "CShort", "CSng",
      "CStr", "CType", "CUInt", "CULng", "CUShort", "Date", "Decimal", "Declare", "Default", "Delegate", "Dim", "DirectCast", "Do",
      "Double", "Each", "Else", "ElseIf", "End", "EndIf", "Enum", "Erase", "Error", "Event", "Exit", "False", "Finally", "For", "Friend",
      "Function", "Get", "GetType", "GetXMLNamespace", "Global", "GoSub", "GoTo", "Handles", "If", "Implements", "Imports", "In",
      "Inherits", "Integer", "Interface", "Is", "IsNot", "Let", "Lib", "Like", "Long", "Loop", "Me", "Mod", "Module", "MustInherit",
      "MustOverride", "MyBase", "MyClass", "Namespace", "Narrowing", "New", "Next", "Not", "Nothing", "NotInheritable", "NotOverridable",
      "Object", "Of", "On", "Operator", "Option", "Optional", "Or", "OrElse", "Overloads", "Overridable", "Overrides", "ParamArray",
      "Partial", "Private", "Property", "Protected", "Public", "RaiseEvent", "ReadOnly", "ReDim", "REM", "RemoveHandler", "Resume",
      "Return", "SByte", "Select", "Set", "Shadows", "Shared", "Short", "Single", "Static", "Step", "Stop", "String", "Structure", "Sub",
      "SyncLock", "Then", "Throw", "To", "True", "Try", "TryCast", "TypeOf", "UInteger", "ULong", "UShort", "Using", "Variant", "Wend",
      "When", "While", "Widening", "With", "WithEvents", "WriteOnly", "Xor"}

    Friend Shared Function IsVbReserved(word As String) As Boolean
      Return s_vbReserved.Contains(word)
    End Function

  End Class

End Namespace
