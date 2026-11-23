Option Explicit On
Option Strict On
Option Infer On

Imports System.Globalization
Imports System.Text

Namespace QB2VB

  ''' <summary>A converted expression: its VB text and its types.</summary>
  Friend Class E
    Public Text As String = ""
    ''' <summary>The QBasic type.</summary>
    Public Qt As QT = QT.Unknown
    ''' <summary>The static type of the VB text (Long means VB's Integer).</summary>
    Public Vb As QT = QT.Unknown
    ''' <summary>True for something that needs no parentheses (a name, a number, a call, a parenthesized expression).</summary>
    Public Atom As Boolean
    ''' <summary>A variable or array element (can be passed by reference).</summary>
    Public IsLValue As Boolean
    ''' <summary>For a variable or array element that is a STRING * n: n.</summary>
    Public FixedLen As Integer
    Public IsLiteral As Boolean
    Public LiteralValue As Double
    Public Sub New()
    End Sub
    Public Sub New(text As String, qty As QT, vb As QT, atom As Boolean)
      Me.Text = text : Me.Qt = qty : Me.Vb = vb : Me.Atom = atom
    End Sub
  End Class

  ''' <summary>What the converter has to tell the expression parser about names.</summary>
  Friend Interface IContext
    ''' <summary>A variable or array reference (declares it implicitly if needed).</summary>
    Function ResolveVariable(name As String, isArrayRef As Boolean) As VarSym
    Function IsArray(name As String) As Boolean
    Function FindProc(name As String) As ProcSym
    Function FindType(name As String) As TypeSym
    ''' <summary>The type of a field of a TYPE (its QBasic type and, for a nested record, its TYPE name).</summary>
    Function FieldOf(userType As String, field As String, ByRef fieldType As QT, ByRef fieldUserType As String) As Boolean
    Sub Note(message As String)
    ''' <summary>The program uses a feature that needs more than the text screen: "graphics" (SCREEN 1 and up, drawing, joystick, pen) or "sound" (SOUND, PLAY).</summary>
    Sub Feature(kind As String)
  End Interface

  Friend Class BuiltinFn
    Public VbName As String
    Public Qt As QT
    Public Vb As QT
    ''' <summary>True when the result has the type of the (first) argument.</summary>
    Public SameAsArg As Boolean
    Public Sub New(vbName As String, qty As QT, vb As QT, Optional sameAsArg As Boolean = False)
      Me.VbName = vbName : Me.Qt = qty : Me.Vb = vb : Me.SameAsArg = sameAsArg
    End Sub
  End Class

  Friend NotInheritable Class Builtins

    Private Sub New()
    End Sub

    Friend Shared ReadOnly Table As New Dictionary(Of String, BuiltinFn)(StringComparer.OrdinalIgnoreCase) From {
      {"ABS", New BuiltinFn("Abs", QT.Single, QT.Double, True)},
      {"ASC", New BuiltinFn("Asc", QT.Short, QT.Long)},
      {"ATN", New BuiltinFn("Atan", QT.Single, QT.Double)},
      {"CDBL", New BuiltinFn("CDbl", QT.Double, QT.Double)},
      {"CHR$", New BuiltinFn("QBChr", QT.String, QT.String)},
      {"CINT", New BuiltinFn("CShort", QT.Short, QT.Short)},
      {"CLNG", New BuiltinFn("CInt", QT.Long, QT.Long)},
      {"COS", New BuiltinFn("Cos", QT.Single, QT.Double)},
      {"CSNG", New BuiltinFn("CSng", QT.Single, QT.Single)},
      {"CSRLIN", New BuiltinFn("CSRLIN", QT.Short, QT.Long)},
      {"CVD", New BuiltinFn("CVD", QT.Double, QT.Double)},
      {"CVI", New BuiltinFn("CVI", QT.Short, QT.Short)},
      {"CVL", New BuiltinFn("CVL", QT.Long, QT.Long)},
      {"CVS", New BuiltinFn("CVS", QT.Single, QT.Single)},
      {"DATE$", New BuiltinFn("DateString", QT.String, QT.String)},
      {"EOF", New BuiltinFn("EOF", QT.Bool, QT.Bool)},
      {"ERL", New BuiltinFn("Erl", QT.Short, QT.Long)},
      {"ERR", New BuiltinFn("Err.Number", QT.Short, QT.Long)},
      {"EXP", New BuiltinFn("Exp", QT.Single, QT.Double)},
      {"FIX", New BuiltinFn("Fix", QT.Single, QT.Double, True)},
      {"FREEFILE", New BuiltinFn("FREEFILE", QT.Short, QT.Long)},
      {"HEX$", New BuiltinFn("Hex$", QT.String, QT.String)},
      {"INKEY$", New BuiltinFn("INKEY$", QT.String, QT.String)},
      {"INSTR", New BuiltinFn("InStr", QT.Short, QT.Long)},
      {"INT", New BuiltinFn("Int", QT.Single, QT.Double, True)},
      {"LCASE$", New BuiltinFn("LCase$", QT.String, QT.String)},
      {"LEFT$", New BuiltinFn("Left$", QT.String, QT.String)},
      {"LEN", New BuiltinFn("Len", QT.Short, QT.Long)},
      {"LOC", New BuiltinFn("LOC", QT.Long, QT.Double)},
      {"LOF", New BuiltinFn("LOF", QT.Long, QT.Double)},
      {"LOG", New BuiltinFn("Log", QT.Single, QT.Double)},
      {"LTRIM$", New BuiltinFn("LTrim$", QT.String, QT.String)},
      {"MID$", New BuiltinFn("Mid$", QT.String, QT.String)},
      {"OCT$", New BuiltinFn("Oct$", QT.String, QT.String)},
      {"POINT", New BuiltinFn("POINT", QT.Short, QT.Long)},
      {"POS", New BuiltinFn("POS", QT.Short, QT.Long)},
      {"RIGHT$", New BuiltinFn("Right$", QT.String, QT.String)},
      {"RND", New BuiltinFn("Rnd", QT.Single, QT.Single)},
      {"RTRIM$", New BuiltinFn("RTrim$", QT.String, QT.String)},
      {"SEEK", New BuiltinFn("SEEK", QT.Long, QT.Double)},
      {"SGN", New BuiltinFn("Sign", QT.Short, QT.Long)},
      {"SIN", New BuiltinFn("Sin", QT.Single, QT.Double)},
      {"SPACE$", New BuiltinFn("Space$", QT.String, QT.String)},
      {"SPC", New BuiltinFn("SPC", QT.String, QT.String)},
      {"SQR", New BuiltinFn("Sqrt", QT.Single, QT.Double)},
      {"STR$", New BuiltinFn("Str$", QT.String, QT.String)},
      {"STRING$", New BuiltinFn("QBString", QT.String, QT.String)},
      {"TAB", New BuiltinFn("TAB", QT.String, QT.String)},
      {"TAN", New BuiltinFn("Tan", QT.Single, QT.Double)},
      {"TIME$", New BuiltinFn("TimeString", QT.String, QT.String)},
      {"TIMER", New BuiltinFn("Timer", QT.Single, QT.Double)},
      {"UCASE$", New BuiltinFn("UCase$", QT.String, QT.String)},
      {"VAL", New BuiltinFn("Val", QT.Double, QT.Double)}}

    ''' <summary>Library functions that keep their QBasic names (no table entry needed beyond the type).</summary>
    Friend Shared ReadOnly LibraryNames As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
      "PEEK", "SCREEN", "LPOS", "PMAP", "STICK", "STRIG", "PEN", "PLAY", "FRE", "INP", "ERDEV", "FILEATTR", "ENVIRON$", "COMMAND$",
      "MKI$", "MKL$", "MKS$", "MKD$", "MKSMBF$", "MKDMBF$", "CVSMBF", "CVDMBF", "INPUT$", "VARPTR$"}

    ''' <summary>Library functions that return a number (the rest of the library names return text).</summary>
    Friend Shared ReadOnly NumericLibrary As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
      "PEEK", "SCREEN", "LPOS", "PMAP", "STICK", "STRIG", "PEN", "PLAY", "FRE", "INP", "ERDEV", "FILEATTR", "CVSMBF", "CVDMBF"}

  End Class

  ''' <summary>Parses a QBasic expression into VB text.</summary>
  Friend Class ExprParser

    Private ReadOnly m_toks As List(Of Token)
    Private m_pos As Integer
    Private ReadOnly m_ctx As IContext

    Friend Sub New(tokens As List(Of Token), start As Integer, ctx As IContext)
      m_toks = tokens : m_pos = start : m_ctx = ctx
    End Sub

    Friend ReadOnly Property Position As Integer
      Get
        Return m_pos
      End Get
    End Property

    Private ReadOnly Property Cur As Token
      Get
        Return If(m_pos < m_toks.Count, m_toks(m_pos), Nothing)
      End Get
    End Property

    Friend Function AtEnd() As Boolean
      Return m_pos >= m_toks.Count
    End Function

    ' ---- Conversions ----------------------------------------------------------------------------

    Friend Shared Function Paren(e As E) As String
      Return If(e.Atom, e.Text, "(" & e.Text & ")")
    End Function

    ''' <summary>The expression as a VB Boolean (a numeric one is compared with 0, as QBasic's IF does).</summary>
    Friend Shared Function AsBool(e As E) As String
      If e.Qt = QT.Bool OrElse e.Vb = QT.Bool Then Return e.Text
      Return Paren(e) & " <> 0"
    End Function

    ''' <summary>The expression converted for a target type (nothing added when VB converts by itself).</summary>
    Friend Shared Function Coerce(e As E, target As QT) As String
      If target = QT.String OrElse target = QT.Unknown OrElse target = QT.User Then Return e.Text
      If Not Types.IsNumeric(target) Then Return e.Text
      If e.Vb = QT.Bool OrElse e.Qt = QT.Bool Then Return Types.Conv(target) & "(" & e.Text & ")"
      If e.Vb = target Then Return e.Text
      If e.IsLiteral Then
        Dim v = e.LiteralValue
        Select Case target
          Case QT.Short : If v = Math.Floor(v) AndAlso v >= -32768 AndAlso v <= 32767 Then Return e.Text
          Case QT.Long : If v = Math.Floor(v) AndAlso v >= Integer.MinValue AndAlso v <= Integer.MaxValue Then Return e.Text
                    Case QT.Single, QT.Double : Return e.Text
        End Select
      End If
      If Types.Rank(e.Vb) >= 0 AndAlso Types.Rank(e.Vb) < Types.Rank(target) Then Return e.Text ' widening
      Return Types.Conv(target) & "(" & e.Text & ")"
    End Function

    Private Shared Function Numeric(e As E) As E
      ' A Boolean used as a number (-1 / 0).
      If e.Qt = QT.Bool OrElse e.Vb = QT.Bool Then
        Return New E("CShort(" & e.Text & ")", QT.Short, QT.Short, True)
      End If
      Return e
    End Function

    ' ---- Operators ------------------------------------------------------------------------------

    Friend Function ParseExpr() As E
      Return ParseImp()
    End Function

    Private Function IsWord(w As String) As Boolean
      Return Cur IsNot Nothing AndAlso Cur.IsWord(w)
    End Function

    Private Function ParseImp() As E
      Dim l = ParseEqv()
      Do While IsWord("IMP")
        m_pos += 1
        Dim r = ParseEqv()
        l = Logical("Or", Not_(l), r)
      Loop
      Return l
    End Function

    Private Function ParseEqv() As E
      Dim l = ParseXor()
      Do While IsWord("EQV")
        m_pos += 1
        Dim r = ParseXor()
        l = Not_(Logical("Xor", l, r))
      Loop
      Return l
    End Function

    Private Function ParseXor() As E
      Dim l = ParseOr()
      Do While IsWord("XOR")
        m_pos += 1
        l = Logical("Xor", l, ParseOr())
      Loop
      Return l
    End Function

    Private Function ParseOr() As E
      Dim l = ParseAnd()
      Do While IsWord("OR")
        m_pos += 1
        l = Logical("Or", l, ParseAnd())
      Loop
      Return l
    End Function

    Private Function ParseAnd() As E
      Dim l = ParseNot()
      Do While IsWord("AND")
        m_pos += 1
        l = Logical("And", l, ParseNot())
      Loop
      Return l
    End Function

    Private Function ParseNot() As E
      If IsWord("NOT") Then
        m_pos += 1
        Return Not_(ParseNot())
      End If
      Return ParseComparison()
    End Function

    Private Shared Function Not_(e As E) As E
      Dim isBool = e.Qt = QT.Bool OrElse e.Vb = QT.Bool
      Dim t = If(isBool, QT.Bool, e.Qt)
      Dim v = If(isBool, QT.Bool, e.Vb)
      Return New E("Not " & Paren(e), t, v, False)
    End Function

    Private Shared Function Logical(op As String, l As E, r As E) As E
      Dim lb = l.Qt = QT.Bool OrElse l.Vb = QT.Bool
      Dim rb = r.Qt = QT.Bool OrElse r.Vb = QT.Bool
      If lb AndAlso rb Then
        Return New E(Paren(l) & " " & op & " " & Paren(r), QT.Bool, QT.Bool, False)
      End If
      Dim a = Numeric(l), b = Numeric(r)
      Dim t = Types.Wider(If(Types.Rank(a.Qt) >= 0, a.Qt, QT.Short), If(Types.Rank(b.Qt) >= 0, b.Qt, QT.Short))
      Dim v = Types.Wider(If(Types.Rank(a.Vb) >= 0, a.Vb, QT.Short), If(Types.Rank(b.Vb) >= 0, b.Vb, QT.Short))
      Return New E(Paren(a) & " " & op & " " & Paren(b), t, v, False)
    End Function

    Private Function ParseComparison() As E
      Dim l = ParseAdd()
      Do While Cur IsNot Nothing AndAlso Cur.Kind = TokKind.Op AndAlso (Cur.Text = "=" OrElse Cur.Text = "<>" OrElse Cur.Text = "<" OrElse Cur.Text = ">" OrElse
                                                                         Cur.Text = "<=" OrElse Cur.Text = ">=" OrElse Cur.Text = "=<" OrElse Cur.Text = "=>" OrElse Cur.Text = "><")
        Dim op = Cur.Text
        m_pos += 1
        Dim r = ParseAdd()
        Select Case op
          Case "=<" : op = "<="
          Case "=>" : op = ">="
          Case "><" : op = "<>"
        End Select
        l = New E(Paren(Numeric(l)) & " " & op & " " & Paren(Numeric(r)), QT.Bool, QT.Bool, False)
      Loop
      Return l
    End Function

    Private Function ParseAdd() As E
      Dim l = ParseMod()
      Do While Cur IsNot Nothing AndAlso (Cur.Is_("+") OrElse Cur.Is_("-"))
        Dim op = Cur.Text
        m_pos += 1
        l = Arith(op, l, ParseMod())
      Loop
      Return l
    End Function

    Private Function ParseMod() As E
      Dim l = ParseIntDiv()
      Do While IsWord("MOD")
        m_pos += 1
        l = Arith("Mod", l, ParseIntDiv())
      Loop
      Return l
    End Function

    Private Function ParseIntDiv() As E
      Dim l = ParseMul()
      Do While Cur IsNot Nothing AndAlso Cur.Is_("\")
        m_pos += 1
        l = Arith("\", l, ParseMul())
      Loop
      Return l
    End Function

    Private Function ParseMul() As E
      Dim l = ParseUnary()
      Do While Cur IsNot Nothing AndAlso (Cur.Is_("*") OrElse Cur.Is_("/"))
        Dim op = Cur.Text
        m_pos += 1
        l = Arith(op, l, ParseUnary())
      Loop
      Return l
    End Function

    Private Shared Function Arith(op As String, l As E, r As E) As E
      Dim a = Numeric(l), b = Numeric(r)
      ' QBasic's MOD rounds its operands to integers first (VB's Mod works on the floating point numbers).
      If op = "Mod" Then
        If Types.Rank(a.Vb) > Types.Rank(QT.Long) Then a = New E("CInt(" & a.Text & ")", QT.Long, QT.Long, True)
        If Types.Rank(b.Vb) > Types.Rank(QT.Long) Then b = New E("CInt(" & b.Text & ")", QT.Long, QT.Long, True)
      End If
      If a.Qt = QT.String OrElse b.Qt = QT.String Then
        ' String concatenation.
        Return New E(Paren(a) & " " & op & " " & Paren(b), QT.String, QT.String, False)
      End If
      Dim qty = Types.Wider(If(Types.Rank(a.Qt) >= 0, a.Qt, QT.Single), If(Types.Rank(b.Qt) >= 0, b.Qt, QT.Single))
      Dim vb = Types.Wider(If(Types.Rank(a.Vb) >= 0, a.Vb, QT.Double), If(Types.Rank(b.Vb) >= 0, b.Vb, QT.Double))
      If op = "/" Then
        qty = If(Types.Rank(qty) < Types.Rank(QT.Single), QT.Single, qty)
        vb = If(Types.Rank(vb) <= Types.Rank(QT.Long), QT.Double, vb)
      ElseIf op = "\" OrElse op = "Mod" Then
        qty = If(Types.Rank(qty) > Types.Rank(QT.Long), QT.Long, qty)
        vb = If(Types.Rank(vb) > Types.Rank(QT.Long), QT.Double, vb) ' (VB turns the operands into Long)
      ElseIf op = "^" Then
        qty = QT.Double : vb = QT.Double
      End If
      Dim e = New E(Paren(a) & " " & op & " " & Paren(b), qty, vb, False)
      ' A sum of literals stays a literal.
      Return e
    End Function

    Private Function ParseUnary() As E
      If Cur IsNot Nothing AndAlso Cur.Is_("-") Then
        m_pos += 1
        Dim e = Numeric(ParseUnary())
        Dim r = New E("-" & Paren(e), e.Qt, e.Vb, False)
        If e.IsLiteral Then r.IsLiteral = True : r.LiteralValue = -e.LiteralValue
        Return r
      End If
      If Cur IsNot Nothing AndAlso Cur.Is_("+") Then
        m_pos += 1
        Return ParseUnary()
      End If
      Return ParsePower()
    End Function

    Private Function ParsePower() As E
      Dim l = ParsePrimary()
      Do While Cur IsNot Nothing AndAlso Cur.Is_("^")
        m_pos += 1
        l = Arith("^", l, ParseUnary())
      Loop
      Return l
    End Function

    ' ---- Primary expressions ---------------------------------------------------------------------

    Private Function ParsePrimary() As E

      Dim t = Cur
      If t Is Nothing Then Throw New ConvertException("Expression expected")

      Select Case t.Kind

        Case TokKind.Number
          m_pos += 1
          Return Literal(t.Text)

        Case TokKind.String
          m_pos += 1
          Return New E(Quote(t.Text), QT.String, QT.String, True)

        Case TokKind.Op
          If t.Text = "(" Then
            m_pos += 1
            Dim inner = ParseExpr()
            Expect(")")
            Dim r = New E("(" & inner.Text & ")", inner.Qt, inner.Vb, True)
            r.IsLiteral = inner.IsLiteral : r.LiteralValue = inner.LiteralValue
            Return r
          End If
          Throw New ConvertException("Unexpected '" & t.Text & "' in an expression")

        Case Else
          Return ParseName()

      End Select

    End Function

    Private Sub Expect(op As String)
      If Cur Is Nothing OrElse Not Cur.Is_(op) Then Throw New ConvertException("'" & op & "' expected")
      m_pos += 1
    End Sub

    Friend Shared Function Quote(s As String) As String
      Dim sb As New StringBuilder("""")
      For Each c In s
        If c = """"c Then sb.Append("""""") Else sb.Append(c)
      Next
      sb.Append(""""c)
      Return sb.ToString
    End Function

    ' A numeric literal: its QBasic and VB types and a VB form of it.
    Private Shared Function Literal(text As String) As E
      Dim upper = text.ToUpperInvariant()

      If upper.StartsWith("&H", StringComparison.Ordinal) OrElse upper.StartsWith("&O", StringComparison.Ordinal) Then
        Dim isHex = upper(1) = "H"c
        Dim digits = upper.Substring(2).TrimEnd("&"c)
        Dim value As Long = 0
        Try
          value = Convert.ToInt64(digits, If(isHex, 16, 8))
        Catch ex As Exception
          Throw New ConvertException("Bad number " & text)
        End Try
        Dim isLong = upper.EndsWith("&", StringComparison.Ordinal) OrElse digits.Length > If(isHex, 4, 6)
        If Not isLong AndAlso value > 32767 Then value -= 65536       ' &HFFFF is -1 in QBasic's 16 bits
        If isLong AndAlso value > Integer.MaxValue Then value -= 4294967296L
        Dim e = New E(value.ToString(CultureInfo.InvariantCulture), If(isLong, QT.Long, QT.Short), QT.Long, True)
        e.IsLiteral = True : e.LiteralValue = value
        If value < 0 Then e.Atom = False
        Return e
      End If

      Dim suffix = ""
      Dim body = upper
      If body.Length > 0 AndAlso "%&!#".IndexOf(body(body.Length - 1)) >= 0 Then
        suffix = body.Substring(body.Length - 1)
        body = body.Substring(0, body.Length - 1)
      End If
      Dim isDouble = body.Contains("D"c)
      body = body.Replace("D"c, "E"c)
      Dim isFloat = body.Contains("."c) OrElse body.Contains("E"c)
      Dim num As Double
      If Not Double.TryParse(body, NumberStyles.Float, CultureInfo.InvariantCulture, num) Then Throw New ConvertException("Bad number " & text)

      Select Case True
        Case suffix = "%" OrElse (suffix = "" AndAlso Not isFloat AndAlso num >= -32768 AndAlso num <= 32767)
          Dim e = New E(body, QT.Short, QT.Long, True)
          e.IsLiteral = True : e.LiteralValue = num
          Return e
        Case suffix = "&" OrElse (suffix = "" AndAlso Not isFloat)
          Dim e = New E(body, QT.Long, QT.Long, True)
          e.IsLiteral = True : e.LiteralValue = num
          Return e
        Case suffix = "#" OrElse isDouble
          Dim e = New E(body & If(body.EndsWith("#"), "", "#"), QT.Double, QT.Double, True)
          e.IsLiteral = True : e.LiteralValue = num
          Return e
        Case Else
          ' SINGLE: a VB literal with a decimal point is Double, so say Single.
          Dim e = New E(body & "!", QT.Single, QT.Single, True)
          e.IsLiteral = True : e.LiteralValue = num
          Return e
      End Select
    End Function

    ' A name: a variable, an array element, a function call.
    Private Function ParseName() As E

      Dim nameTok = Cur
      m_pos += 1
      Dim name = nameTok.Text
      Dim hasParen = Cur IsNot Nothing AndAlso Cur.Is_("(")

      Dim args As New List(Of E)
      Dim emptyParens = False
      If hasParen Then
        m_pos += 1
        If Cur IsNot Nothing AndAlso Cur.Is_(")") Then
          emptyParens = True
          m_pos += 1
        Else
          Do
            args.Add(ParseExpr())
            If Cur IsNot Nothing AndAlso Cur.Is_(",") Then
              m_pos += 1
              Continue Do
            End If
            Exit Do
          Loop
          Expect(")")
        End If
      End If

      Dim upper = name.ToUpperInvariant()

      ' Addresses of variables can't be given in .NET.
      If (upper = "VARPTR" OrElse upper = "VARSEG" OrElse upper = "SADD") AndAlso hasParen AndAlso Not m_ctx.IsArray(name) Then
        Throw New ConvertException(upper & " (the address of a variable) isn't supported")
      End If

      ' Arrays (and whole-array arguments) first.
      If m_ctx.IsArray(name) AndAlso (hasParen OrElse False) Then
        Dim v = m_ctx.ResolveVariable(name, True)
        If emptyParens Then Return New E(VarText(v), v.Qt, v.Qt, True) With {.IsLValue = False}
        Dim e = New E(VarText(v) & "(" & String.Join(", ", args.Select(Function(a) Coerce(a, QT.Long))) & ")", v.Qt, v.Qt, True)
        e.IsLValue = True
        e.FixedLen = v.FixedLen
        Return Members(e, v.UserType)
      End If

      ' User procedures.
      Dim proc = m_ctx.FindProc(name)
      If proc IsNot Nothing AndAlso proc.IsFunction Then
        Dim texts As New List(Of String)
        For i = 0 To args.Count - 1
          If i < proc.Params.Count Then
            Dim p = proc.Params(i)
            texts.Add(If(p.IsArray, args(i).Text, Coerce(args(i), p.Qt)))
          Else
            texts.Add(args(i).Text)
          End If
        Next
        Dim callText = ProcName(proc) & "(" & String.Join(", ", texts) & ")"
        Return New E(callText, proc.RetQt, proc.RetQt, True)
      End If

      ' Built-in functions.
      Dim fn As BuiltinFn = Nothing
      If upper = "POINT" OrElse upper = "PMAP" OrElse upper = "STICK" OrElse upper = "STRIG" OrElse upper = "PEN" Then m_ctx.Feature("graphics")
      If upper = "PLAY" Then m_ctx.Feature("sound")
      If Builtins.Table.TryGetValue(upper, fn) Then
        Dim qty = fn.Qt, vb = fn.Vb
        If fn.SameAsArg AndAlso args.Count > 0 Then
          qty = args(0).Qt : vb = args(0).Vb
          If qty = QT.Bool Then qty = QT.Short : vb = QT.Short
        End If
        Dim texts = args.Select(Function(a) a.Text).ToList()
        Select Case upper
          Case "ERR", "ERL", "TIMER", "RND", "INKEY$", "DATE$", "TIME$", "CSRLIN", "FREEFILE"
            If args.Count = 0 AndAlso Not hasParen Then
              Dim noArgs = If(upper = "ERR" OrElse upper = "ERL" OrElse upper = "TIMER", fn.VbName, fn.VbName & "()")
              Return New E(noArgs, qty, vb, True)
            End If
        End Select
        ' Arguments that VB wants as Integer.
        Select Case upper
          Case "SPACE$", "LEFT$", "RIGHT$", "CHR$", "STRING$", "MID$", "TAB", "SPC", "INSTR"
            For i = 0 To texts.Count - 1
              If args(i).Qt <> QT.String Then texts(i) = Coerce(args(i), QT.Long)
            Next
        End Select
        Dim call_ = fn.VbName & "(" & String.Join(", ", texts) & ")"
        Return New E(call_, qty, vb, True)
      End If

      ' Library functions and other names with parentheses that aren't arrays.
      If hasParen AndAlso (Builtins.LibraryNames.Contains(upper) OrElse (proc Is Nothing AndAlso Not m_ctx.IsArray(name) AndAlso LooksLikeLibraryFunction(name))) Then
        Dim qty = TypeOfLibrary(name, upper)
        Return New E(name & "(" & String.Join(", ", args.Select(Function(a) a.Text)) & ")", qty, If(qty = QT.Short, QT.Long, qty), True)
      End If

      ' Everything else is a variable, or an array that is being used without having been DIMmed.
      If hasParen Then
        Dim v = m_ctx.ResolveVariable(name, True)
        Dim e = New E(VarText(v) & "(" & String.Join(", ", args.Select(Function(a) Coerce(a, QT.Long))) & ")", v.Qt, v.Qt, True)
        e.IsLValue = True
        e.FixedLen = v.FixedLen
        Return Members(e, v.UserType)
      End If

      ' A user FUNCTION without arguments.
      If proc IsNot Nothing AndAlso proc.IsFunction Then Return New E(ProcName(proc) & "()", proc.RetQt, proc.RetQt, True)

      Dim variable = m_ctx.ResolveVariable(name, False)
      Dim ve = New E(VarText(variable), variable.Qt, variable.Qt, True)
      ve.IsLValue = True
      ve.FixedLen = variable.FixedLen
      ' A constant's value is known (arrays can be sized by it).
      Return ve

    End Function

    Private Shared Function LooksLikeLibraryFunction(name As String) As Boolean
      ' A name that ends with $ and isn't an array is almost certainly a string function of the library.
      Return name.EndsWith("$", StringComparison.Ordinal) AndAlso Builtins.LibraryNames.Contains(name)
    End Function

    Private Shared Function TypeOfLibrary(name As String, upper As String) As QT
      If name.EndsWith("$", StringComparison.Ordinal) Then Return QT.String
      If Builtins.NumericLibrary.Contains(upper) Then Return QT.Short
      Return QT.Short
    End Function

    ' .field after an array element of a TYPE.
    Private Function Members(e As E, userType As String) As E
      Do While userType <> "" AndAlso Cur IsNot Nothing AndAlso Cur.Is_(".") AndAlso m_pos + 1 < m_toks.Count
        m_pos += 1
        Dim field = Cur.Text
        m_pos += 1
        Dim fq As QT, fut As String = ""
        If Not m_ctx.FieldOf(userType, field, fq, fut) Then Throw New ConvertException("unknown field " & field & " of " & userType)
        e = New E(e.Text & "." & FieldName(field), fq, fq, True) With {.IsLValue = True}
        userType = fut
      Loop
      Return e
    End Function

    ''' <summary>The name of a TYPE field in VB (no suffix; a reserved word in brackets).</summary>
    Friend Shared Function FieldName(field As String) As String
      Dim bare = field
      If bare.Length > 1 AndAlso Types.FromSuffix(bare(bare.Length - 1)) <> QT.Unknown Then bare = bare.Substring(0, bare.Length - 1)
      Return If(Keywords.IsVbReserved(bare), "[" & bare & "]", bare)
    End Function

    ''' <summary>The VB text for a variable.</summary>
    Friend Shared Function VarText(v As VarSym) As String
      Return v.VbName & If(v.HasSuffix, Types.VbSuffix(v.Qt), "")
    End Function

    Friend Shared Function ProcName(p As ProcSym) As String
      Dim n = p.QbName
      If Keywords.IsVbReserved(n) Then n = If(p.HasSuffix, n & "_", "[" & n & "]")
      Return n & If(p.HasSuffix, Types.VbSuffix(p.RetQt), "")
    End Function

  End Class

  Friend Class ConvertException
    Inherits Exception
    Public Sub New(message As String)
      MyBase.New(message)
    End Sub
  End Class

End Namespace
