' Converted from QBasic by qb2vb.

Module Program

  Structure TipoUsuario
    Implements IFileRecord
    Private m_nombre As String
    Public Property nombre As String
      Get
        Return If(m_nombre, FixedString.Blank(15))
      End Get
      Set(value As String)
        m_nombre = FixedString.Fit(value, 15)
      End Set
    End Property
    Private m_apellido As String
    Public Property apellido As String
      Get
        Return If(m_apellido, FixedString.Blank(30))
      End Get
      Set(value As String)
        m_apellido = FixedString.Fit(value, 30)
      End Set
    End Property
    Public edad As Short
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(nombre, 15)
      writer.WriteString(apellido, 30)
      writer.Write(edad)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      nombre = reader.ReadString(15)
      apellido = reader.ReadString(30)
      edad = reader.ReadInt16()
    End Sub
  End Structure
  

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' TYPE TipoUsuario (see the Structure at the top)
    
    Dim usuario(20) As TipoUsuario
    
    usuario(1).nombre = "Juan"
    usuario(1).apellido = "Hdez."
    usuario(1).edad = 49
    
    PRINT(usuario(1).nombre & vbTab & usuario(1).apellido & vbTab & Str$(usuario(1).edad) & " ")
  End Sub

End Module
