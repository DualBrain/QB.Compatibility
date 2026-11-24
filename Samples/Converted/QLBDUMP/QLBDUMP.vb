' Converted from QBasic by qb2vb.

Module Program

  Structure ExeHdr
    Implements IFileRecord
    Private m_other1 As String
    Public Property other1 As String
      Get
        Return If(m_other1, FixedString.Blank(8))
      End Get
      Set(value As String)
        m_other1 = FixedString.Fit(value, 8)
      End Set
    End Property
    Public CParHdr As Short
    Private m_other2 As String
    Public Property other2 As String
      Get
        Return If(m_other2, FixedString.Blank(10))
      End Get
      Set(value As String)
        m_other2 = FixedString.Fit(value, 10)
      End Set
    End Property
    Public IP As Short
    Public CS As Short
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(other1, 8)
      writer.Write(CParHdr)
      writer.WriteString(other2, 10)
      writer.Write(IP)
      writer.Write(CS)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      other1 = reader.ReadString(8)
      CParHdr = reader.ReadInt16()
      other2 = reader.ReadString(10)
      IP = reader.ReadInt16()
      CS = reader.ReadInt16()
    End Sub
  End Structure
  
  Structure QBHdr
    Implements IFileRecord
    Private m_QBHead As String
    Public Property QBHead As String
      Get
        Return If(m_QBHead, FixedString.Blank(6))
      End Get
      Set(value As String)
        m_QBHead = FixedString.Fit(value, 6)
      End Set
    End Property
    Public Magic As Short
    Public SymStart As Short
    Public DatStart As Short
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(QBHead, 6)
      writer.Write(Magic)
      writer.Write(SymStart)
      writer.Write(DatStart)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      QBHead = reader.ReadString(6)
      Magic = reader.ReadInt16()
      SymStart = reader.ReadInt16()
      DatStart = reader.ReadInt16()
    End Sub
  End Structure
  
  Structure QbSym
    Implements IFileRecord
    Public Flags As Short
    Public NameStart As Short
    Private m_other As String
    Public Property other As String
      Get
        Return If(m_other, FixedString.Blank(4))
      End Get
      Set(value As String)
        m_other = FixedString.Fit(value, 4)
      End Set
    End Property
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.Write(Flags)
      writer.Write(NameStart)
      writer.WriteString(other, 4)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      Flags = reader.ReadInt16()
      NameStart = reader.ReadInt16()
      other = reader.ReadString(4)
    End Sub
  End Structure
  

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim FileName$ = ""
    Dim OutFile$ = ""
    ' This program prints the names of QuickLibrary procedures
    
    ' DECLARE SUB DumpSym (SymStart AS INTEGER, QHdrPos AS LONG)
    
    ' TYPE ExeHdr (see the Structure at the top)
    
    ' TYPE QBHdr (see the Structure at the top)
    
    ' TYPE QbSym (see the Structure at the top)
    
    Dim EHdr As ExeHdr, Qhdr As QBHdr, QHdrPos As Integer
    
    INPUT("Enter QuickLibrary file name: ", FileName$, False)
    FileName$ = UCase$(FileName$)
    If InStr(FileName$, ".QLB") = 0 Then FileName$ = FileName$ + ".QLB"
    
    INPUT("Enter output file name or press ENTER for screen: ", OutFile$, False)
    OutFile$ = UCase$(OutFile$)
    If OutFile$ = "" Then OutFile$ = "CON"
    
    OPEN(FileName$, OpenMode.Binary, 1)
    OPEN(OutFile$, OpenMode.Output, 2)
    
    [GET](1, EHdr)  ' Read the EXE format header.
    
    QHdrPos = (((EHdr.CParHdr + EHdr.CS) * 16) + EHdr.IP) + 1
    
    [GET](1, QHdrPos, Qhdr)  ' Read the QuickLib format header.
    
    If Qhdr.Magic <> 27765 Then PRINT("Not a QB UserLibrary") : End
    
    PRINT(2, "Code Symbols:") : PRINT(2)
    DumpSym(Qhdr.SymStart, QHdrPos)  ' dump code symbols
    PRINT(2)
    
    PRINT(2, "Data Symbols:") : PRINT(2, "")
    DumpSym(Qhdr.DatStart, QHdrPos)  ' dump data symbols
    PRINT(2)
    
    End
    
  End Sub

  
  Sub DumpSym(ByRef SymStart As Short, ByRef QHdrPos As Integer)
    Dim Prospect$ = ""
    Dim SName$ = ""
     Dim QlbSym As QbSym
     Dim NextSym As Integer, CurrentSym As Integer
  
     ' Calculate the location of the first symbol entry, then read that entry:
     NextSym = QHdrPos + SymStart
     [GET](1, NextSym, QlbSym)
  
     Do
        NextSym = CInt(SEEK(1))  ' Save the location of the next
                                   ' symbol.
        CurrentSym = QHdrPos + QlbSym.NameStart
        SEEK(1, CurrentSym)  ' Use SEEK to move to the name
                                   ' for the current symbol entry.
        Prospect$ = InputStr(40, 1)  ' Read the longest legal string,
                                   ' plus one additonal byte for the
                                   ' final null character (CHR$(0)).
  
        ' Extract the null-terminated name:
        SName$ = Left$(Prospect$, InStr(Prospect$, QBChr(0)))
  
        ' Print only those names that do not begin with "__", "$", or "b$"
        ' as these names are usually considered reserved:
        If ((Left$(SName$, 2) <> "__") And (Left$(SName$, 1) <> "$")) And (UCase$(Left$(SName$, 2)) <> "B$") Then
           PRINT(2, ("  " + SName$))
        End If
  
        [GET](1, NextSym, QlbSym)  ' Read a symbol entry.
     Loop While QlbSym.Flags <> 0  ' Flags=0 (false) means end of table.
  End Sub
  
End Module
