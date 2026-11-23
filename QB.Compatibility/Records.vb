Option Explicit On
Option Strict On
Option Infer On

Imports System.Diagnostics.CodeAnalysis
Imports System.IO
Imports System.Reflection
Imports Microsoft.VisualBasic

Namespace Global.QB

  ''' <summary>
  ''' A record (a QBasic TYPE) that writes and reads its own fields, in order.  This works everywhere,
  ''' including trimmed and Native AOT builds.  Structures that don't implement it are handled by reflection
  ''' (strings need <c>&lt;VBFixedString(n)&gt;</c>), which doesn't work in an AOT build.
  ''' </summary>
  Public Interface IFileRecord
    Sub WriteTo(writer As RecordWriter)
    Sub ReadFrom(reader As RecordReader)
  End Interface

  ''' <summary>Writes the fields of a record: numbers as QBasic stores them, strings padded with spaces to a fixed length.</summary>
  Public NotInheritable Class RecordWriter

    Private ReadOnly m_writer As BinaryWriter

    Friend Sub New(writer As BinaryWriter)
      m_writer = writer
    End Sub

    Public Sub Write(value As Byte)
      m_writer.Write(value)
    End Sub

    Public Sub Write(value As Short)
      m_writer.Write(value)
    End Sub

    Public Sub Write(value As Integer)
      m_writer.Write(value)
    End Sub

    Public Sub Write(value As Long)
      m_writer.Write(value)
    End Sub

    Public Sub Write(value As Single)
      m_writer.Write(value)
    End Sub

    Public Sub Write(value As Double)
      m_writer.Write(value)
    End Sub

    ''' <summary>Writes a string of exactly <paramref name="length"/> characters (padded with spaces or cut short).</summary>
    Public Sub WriteString(value As String, length As Integer)
      Dim s = If(value, "")
      For i = 0 To length - 1
        m_writer.Write(If(i < s.Length, Cp437.ToByte(s(i)), CByte(32)))
      Next
    End Sub

    ''' <summary>Writes a record inside a record.</summary>
    Public Sub Write(record As IFileRecord)
      record.WriteTo(Me)
    End Sub

  End Class

  ''' <summary>Reads the fields of a record in the order they were written.</summary>
  Public NotInheritable Class RecordReader

    Private ReadOnly m_reader As BinaryReader

    Friend Sub New(reader As BinaryReader)
      m_reader = reader
    End Sub

    Public Function ReadByte() As Byte
      Return m_reader.ReadByte()
    End Function

    Public Function ReadInt16() As Short
      Return m_reader.ReadInt16()
    End Function

    Public Function ReadInt32() As Integer
      Return m_reader.ReadInt32()
    End Function

    Public Function ReadInt64() As Long
      Return m_reader.ReadInt64()
    End Function

    Public Function ReadSingle() As Single
      Return m_reader.ReadSingle()
    End Function

    Public Function ReadDouble() As Double
      Return m_reader.ReadDouble()
    End Function

    ''' <summary>Reads a string of exactly <paramref name="length"/> characters (trailing spaces included, as QBasic does).</summary>
    Public Function ReadString(length As Integer) As String
      Dim chars(length - 1) As Char
      For i = 0 To length - 1
        chars(i) = Cp437.ToChar(m_reader.ReadByte())
      Next
      Return New String(chars)
    End Function

    ''' <summary>Reads a record inside a record.</summary>
    Public Sub Read(Of T As IFileRecord)(ByRef record As T)
      record.ReadFrom(Me)
    End Sub

  End Class

  ''' <summary>
  ''' Turns values into the bytes QBasic stores in random and binary files and back: Short (QBasic INTEGER),
  ''' Integer (LONG), Single, Double and Byte; strings (as they are); and records, either implementing
  ''' <see cref="IFileRecord"/> or (not in AOT builds) structures made of those with fixed-length strings.
  ''' </summary>
  Friend NotInheritable Class Records

    Private Sub New()
    End Sub

    ' ---- Numbers and strings: no reflection ----------------------------------------------------

    Private Shared Function IsPrimitive(t As Type) As Boolean
      Select Case Type.GetTypeCode(t)
        Case TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
             TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double
          Return True
      End Select
      Return False
    End Function

    Private Shared Sub WritePrimitive(bw As BinaryWriter, value As Object, code As TypeCode)
      Select Case code
        Case TypeCode.Byte : bw.Write(DirectCast(value, Byte))
        Case TypeCode.SByte : bw.Write(DirectCast(value, SByte))
        Case TypeCode.Int16 : bw.Write(DirectCast(value, Short))
        Case TypeCode.UInt16 : bw.Write(DirectCast(value, UShort))
        Case TypeCode.Int32 : bw.Write(DirectCast(value, Integer))
        Case TypeCode.UInt32 : bw.Write(DirectCast(value, UInteger))
        Case TypeCode.Int64 : bw.Write(DirectCast(value, Long))
        Case TypeCode.UInt64 : bw.Write(DirectCast(value, ULong))
        Case TypeCode.Single : bw.Write(DirectCast(value, Single))
        Case TypeCode.Double : bw.Write(DirectCast(value, Double))
      End Select
    End Sub

    Private Shared Function ReadPrimitive(br As BinaryReader, code As TypeCode) As Object
      Select Case code
        Case TypeCode.Byte : Return br.ReadByte()
        Case TypeCode.SByte : Return br.ReadSByte()
        Case TypeCode.Int16 : Return br.ReadInt16()
        Case TypeCode.UInt16 : Return br.ReadUInt16()
        Case TypeCode.Int32 : Return br.ReadInt32()
        Case TypeCode.UInt32 : Return br.ReadUInt32()
        Case TypeCode.Int64 : Return br.ReadInt64()
        Case TypeCode.UInt64 : Return br.ReadUInt64()
        Case TypeCode.Single : Return br.ReadSingle()
        Case Else : Return br.ReadDouble()
      End Select
    End Function

    ' ---- Public entry points -----------------------------------------------------------------

    ''' <summary>The bytes of a value (a plain String is written as is, with no padding).</summary>
    Friend Shared Function ToBytes(Of T)(value As T) As Byte()
      Return ToBytes(CObj(value), GetType(T))
    End Function

    ''' <summary>The bytes of a value, using its actual type.</summary>
    Friend Shared Function ToBytes(boxed As Object) As Byte()
      Return ToBytes(boxed, boxed.GetType())
    End Function

    Private Shared Function ToBytes(boxed As Object, t As Type) As Byte()
      Dim ms As New MemoryStream
      Using bw As New BinaryWriter(ms)
        Dim rec = TryCast(boxed, IFileRecord)
        If rec IsNot Nothing Then
          rec.WriteTo(New RecordWriter(bw))
        ElseIf TypeOf boxed Is String Then
          For Each c In DirectCast(boxed, String)
            bw.Write(Cp437.ToByte(c))
          Next
        ElseIf IsPrimitive(t) Then
          WritePrimitive(bw, boxed, Type.GetTypeCode(t))
        Else
          WriteByReflection(bw, boxed, t)
        End If
      End Using
      Return ms.ToArray
    End Function

    ''' <summary>
    ''' Builds a value from bytes.  For a plain String, <paramref name="current"/> gives the length to read.
    ''' </summary>
    Friend Shared Function FromBytes(Of T)(data() As Byte, current As T) As T
      Using br As New BinaryReader(New MemoryStream(data))
        Dim boxed As Object = current
        Dim rec = TryCast(boxed, IFileRecord)
        If rec IsNot Nothing Then
          ' (For a structure this changes the boxed copy, which is then unboxed.)
          rec.ReadFrom(New RecordReader(br))
          Return DirectCast(rec, T)
        ElseIf TypeOf boxed Is String Then
          Dim chars(data.Length - 1) As Char
          For i = 0 To data.Length - 1
            chars(i) = Cp437.ToChar(data(i))
          Next
          Return DirectCast(DirectCast(New String(chars), Object), T)
        ElseIf IsPrimitive(GetType(T)) Then
          Return DirectCast(ReadPrimitive(br, Type.GetTypeCode(GetType(T))), T)
        Else
          Return DirectCast(ReadByReflection(br, GetType(T)), T)
        End If
      End Using
    End Function

    ''' <summary>The number of bytes a value takes in a file (like LEN).</summary>
    Friend Shared Function SizeOf(Of T)(value As T) As Integer
      Dim boxed As Object = value
      If TypeOf boxed Is String Then Return DirectCast(boxed, String).Length
      If IsPrimitive(GetType(T)) Then Return ToBytes(value).Length
      If TypeOf boxed Is IFileRecord Then Return ToBytes(value).Length
      Return SizeByReflection(GetType(T))
    End Function

    ' ---- Structures by reflection (JIT only) ---------------------------------------------------

    Private Const NeedsInterface As String = "Reflection over a record isn't available in a trimmed or Native AOT build; implement IFileRecord on the structure."

    Private Shared Sub RequireReflection()
      If Not System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported Then
        Throw New NotSupportedException(NeedsInterface)
      End If
    End Sub

    Private Shared Function FixedLengthOf(field As FieldInfo) As Integer
      Dim attr = field.GetCustomAttribute(Of VBFixedStringAttribute)
      Return If(attr Is Nothing, -1, attr.Length)
    End Function

    <UnconditionalSuppressMessage("Trimming", "IL2070", Justification:="Falls back to IFileRecord when reflection isn't available.")>
    Private Shared Function FieldsOf(t As Type) As FieldInfo()
      Return t.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
    End Function

    Private Shared Function SizeOfType(t As Type, fixedLength As Integer) As Integer
      If t Is GetType(String) Then
        If fixedLength < 0 Then Throw New NotSupportedException("A string in a record needs <VBFixedString(n)>.")
        Return fixedLength
      End If
      Select Case Type.GetTypeCode(t)
        Case TypeCode.Byte, TypeCode.SByte : Return 1
        Case TypeCode.Int16, TypeCode.UInt16 : Return 2
        Case TypeCode.Int32, TypeCode.UInt32, TypeCode.Single : Return 4
        Case TypeCode.Int64, TypeCode.UInt64, TypeCode.Double : Return 8
        Case TypeCode.Object
          If t.IsValueType AndAlso Not t.IsEnum Then
            Dim total = 0
            For Each f In FieldsOf(t)
              total += SizeOfType(f.FieldType, FixedLengthOf(f))
            Next
            Return total
          End If
      End Select
      Throw New NotSupportedException($"{t.Name} can't be stored in a file record.")
    End Function

    Private Shared Function SizeByReflection(t As Type) As Integer
      RequireReflection()
      Return SizeOfType(t, -1)
    End Function

    Private Shared Sub WriteField(bw As BinaryWriter, value As Object, t As Type, fixedLength As Integer)
      If t Is GetType(String) Then
        If fixedLength < 0 Then Throw New NotSupportedException("A string in a record needs <VBFixedString(n)>.")
        Dim s = If(DirectCast(value, String), "")
        For i = 0 To fixedLength - 1
          bw.Write(If(i < s.Length, Cp437.ToByte(s(i)), CByte(32)))
        Next
      ElseIf IsPrimitive(t) Then
        WritePrimitive(bw, value, Type.GetTypeCode(t))
      Else
        For Each f In FieldsOf(t)
          WriteField(bw, f.GetValue(value), f.FieldType, FixedLengthOf(f))
        Next
      End If
    End Sub

    Private Shared Sub WriteByReflection(bw As BinaryWriter, value As Object, t As Type)
      RequireReflection()
      WriteField(bw, value, t, -1)
    End Sub

    <UnconditionalSuppressMessage("Trimming", "IL2067", Justification:="Only reached after RequireReflection, which refuses to run in AOT builds.")>
    Private Shared Function ReadField(br As BinaryReader, t As Type, fixedLength As Integer) As Object
      If t Is GetType(String) Then
        If fixedLength < 0 Then Throw New NotSupportedException("A string in a record needs <VBFixedString(n)>.")
        Dim chars(fixedLength - 1) As Char
        For i = 0 To fixedLength - 1
          chars(i) = Cp437.ToChar(br.ReadByte())
        Next
        Return New String(chars)
      End If
      If IsPrimitive(t) Then Return ReadPrimitive(br, Type.GetTypeCode(t))
      ' (A boxed structure has to be held as ValueType: VB copies Object variables when they are passed
      ' on, so the fields set through reflection would be lost.)
      Dim boxed As ValueType = DirectCast(Activator.CreateInstance(t), ValueType)
      For Each f In FieldsOf(t)
        f.SetValue(boxed, ReadField(br, f.FieldType, FixedLengthOf(f)))
      Next
      Return boxed
    End Function

    Private Shared Function ReadByReflection(br As BinaryReader, t As Type) As Object
      RequireReflection()
      Return ReadField(br, t, -1)
    End Function

  End Class

End Namespace
