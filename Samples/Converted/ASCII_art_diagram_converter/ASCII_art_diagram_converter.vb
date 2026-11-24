' Converted from QBasic by qb2vb.

Module Program

  Structure TableEntry
    Implements IFileRecord
    Private m_nombre As String
    Public Property nombre As String
      Get
        Return If(m_nombre, FixedString.Blank(8))
      End Get
      Set(value As String)
        m_nombre = FixedString.Fit(value, 8)
      End Set
    End Property
    Public bits As Short
    Public startPos As Short
    Public length As Short
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(nombre, 8)
      writer.Write(bits)
      writer.Write(startPos)
      writer.Write(length)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      nombre = reader.ReadString(8)
      bits = reader.ReadInt16()
      startPos = reader.ReadInt16()
      length = reader.ReadInt16()
    End Sub
  End Structure
  

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB ParseASCIIArt ()
    ' DECLARE FUNCTION HexToBinary$ (hexString AS STRING)
    
    ' TYPE TableEntry (see the Structure at the top)
    
    ParseASCIIArt()
    End
  End Sub

  
  Function HexToBinary$(ByRef hexString As String)
    Dim hexDigit$
    Dim idx!
      Dim hexMap(15) As String
      hexMap(0) = "0000" : hexMap(1) = "0001" : hexMap(2) = "0010" : hexMap(3) = "0011"
      hexMap(4) = "0100" : hexMap(5) = "0101" : hexMap(6) = "0110" : hexMap(7) = "0111"
      hexMap(8) = "1000" : hexMap(9) = "1001" : hexMap(10) = "1010" : hexMap(11) = "1011"
      hexMap(12) = "1100" : hexMap(13) = "1101" : hexMap(14) = "1110" : hexMap(15) = "1111"
  
      Dim result$ = ""
      For i! = 0 To Len(hexString) - 1
          hexDigit$ = UCase$(Mid$(hexString, CInt(i + 1), 1))
          If (hexDigit$ >= "0") And (hexDigit$ <= "9") Then
              idx = CSng(Val(hexDigit$))
          Else
              idx = (Asc(hexDigit$) - Asc("A")) + 10
          End If
          If (idx >= 0) And (idx <= 15) Then result$ = result$ + hexMap(CInt(idx))
      Next
      HexToBinary$ = result$
  End Function
  
  
  Sub ParseASCIIArt()
    Dim bitPattern$
      Dim header As String
      header = "  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+" + QBChr(10)
      header = (header + "  |                      ID                       |") + QBChr(10)
      header = (header + "  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+") + QBChr(10)
      header = (header + "  |QR|   Opcode  |AA|TC|RD|RA|   Z    |   RCODE   |") + QBChr(10)
      header = (header + "  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+") + QBChr(10)
      header = (header + "  |                    QDCOUNT                    |") + QBChr(10)
      header = (header + "  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+") + QBChr(10)
      header = (header + "  |                    ANCOUNT                    |") + QBChr(10)
      header = (header + "  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+") + QBChr(10)
      header = (header + "  |                    NSCOUNT                    |") + QBChr(10)
      header = (header + "  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+") + QBChr(10)
      header = (header + "  |                    ARCOUNT                    |") + QBChr(10)
      header = header + "  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+"
  
      Dim fields(12) As TableEntry
      fields(0).nombre = "     ID " : fields(0).bits = 16 : fields(0).startPos = 0 : fields(0).length = 16
      fields(1).nombre = "     QR " : fields(1).bits = 1 : fields(1).startPos = 16 : fields(1).length = 1
      fields(2).nombre = " Opcode " : fields(2).bits = 4 : fields(2).startPos = 17 : fields(2).length = 4
      fields(3).nombre = "     AA " : fields(3).bits = 1 : fields(3).startPos = 21 : fields(3).length = 1
      fields(4).nombre = "     TC " : fields(4).bits = 1 : fields(4).startPos = 22 : fields(4).length = 1
      fields(5).nombre = "     RD " : fields(5).bits = 1 : fields(5).startPos = 23 : fields(5).length = 1
      fields(6).nombre = "     RA " : fields(6).bits = 1 : fields(6).startPos = 24 : fields(6).length = 1
      fields(7).nombre = "      Z " : fields(7).bits = 3 : fields(7).startPos = 25 : fields(7).length = 3
      fields(8).nombre = "  RCODE " : fields(8).bits = 4 : fields(8).startPos = 28 : fields(8).length = 4
      fields(9).nombre = "QDCOUNT " : fields(9).bits = 16 : fields(9).startPos = 32 : fields(9).length = 16
      fields(10).nombre = "ANCOUNT " : fields(10).bits = 16 : fields(10).startPos = 48 : fields(10).length = 16
      fields(11).nombre = "NSCOUNT " : fields(11).bits = 16 : fields(11).startPos = 64 : fields(11).length = 16
      fields(12).nombre = "ARCOUNT " : fields(12).bits = 16 : fields(12).startPos = 80 : fields(12).length = 16
  
      Dim hexStr$ = "78477bbf5496e12e1bf169a4"
      Dim binStr$ = HexToBinary$(hexStr$)
  
      PRINT("RFC 1035 message diagram header:")
      PRINT(header)
      PRINT()
      PRINT(" Decoded:")
      PRINT(" Name     Bits  Start  End")
      PRINT(" =======  ====  =====  ===")
  
      For i = 0! To 12!
          PRINT_USING(" \     \  ####  #####  ###", fields(CInt(i)).nombre, fields(CInt(i)).bits, fields(CInt(i)).startPos, (fields(CInt(i)).startPos + fields(CInt(i)).length) - 1)
      Next
  
      PRINT()
      PRINT(" Test string in hex:")
      PRINT(" " & hexStr$)
      PRINT()
      PRINT(" Test string in binary:")
      PRINT(" " & binStr$)
      PRINT()
      PRINT(" Unpacked:")
      PRINT(" Name     Size  Bit Pattern")
      PRINT(" =======  ====  ===============")
  
      For i = 0! To 12!
          bitPattern$ = Mid$(binStr$, fields(CInt(i)).startPos + 1, fields(CInt(i)).length)
          bitPattern$ = Left$(bitPattern$ + Space$(16), 16)
          PRINT_USING(" \     \  ####  \             \", fields(CInt(i)).nombre, fields(CInt(i)).bits, bitPattern$)
      Next
  End Sub
  
End Module
