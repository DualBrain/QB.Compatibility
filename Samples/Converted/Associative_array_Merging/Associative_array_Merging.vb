' Converted from QBasic by qb2vb.

Module Program

  Structure Dictionary
    Implements IFileRecord
    Private m_keyy As String
    Public Property keyy As String
      Get
        Return If(m_keyy, FixedString.Blank(5))
      End Get
      Set(value As String)
        m_keyy = FixedString.Fit(value, 5)
      End Set
    End Property
    Private m_value As String
    Public Property value As String
      Get
        Return If(m_value, FixedString.Blank(13))
      End Get
      Set(value As String)
        m_value = FixedString.Fit(value, 13)
      End Set
    End Property
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.WriteString(keyy, 5)
      writer.WriteString(value, 13)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      keyy = reader.ReadString(5)
      value = reader.ReadString(13)
    End Sub
  End Structure
  

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB merge (original() AS ANY, update() AS ANY, result() AS ANY)
    
    ' TYPE Dictionary (see the Structure at the top)
    
    Dim original(2) As Dictionary
    original(0).keyy = "name" : original(0).value = "Rocket Skates"
    original(1).keyy = "price" : original(1).value = "12.75"
    original(2).keyy = "color" : original(2).value = "yellow"
    
    Dim update(2) As Dictionary
    update(0).keyy = "price" : update(0).value = "15.25"
    update(1).keyy = "color" : update(1).value = "red"
    update(2).keyy = "year" : update(2).value = "1974"
    
    Dim merged((UBound(update) + UBound(original)) - 1) As Dictionary
    merge(original, update, merged)
    
    For i! = 0 To UBound(merged)
        PRINT("keyy: " & merged(CInt(i)).keyy & ", value: " & merged(CInt(i)).value)
    Next i
    End
  End Sub

  
  Sub merge(ByRef original() As Dictionary, ByRef update() As Dictionary, ByRef result() As Dictionary)
      Dim i As Short, j As Short, index As Short, found As Short
  
      For i = 0 To CShort(UBound(update))
          result(i) = update(i)
      Next i
  
      index = i
      For i = 0 To CShort(UBound(original))
          found = 0
          For j = 0 To CShort(UBound(update))
              If original(i).keyy = update(j).keyy Then
                  found = 1
                  Exit For
              End If
          Next j
          If found = 0 Then
              result(index) = original(i)
              index = CShort(index + 1)
          End If
      Next i
  End Sub
  
End Module
