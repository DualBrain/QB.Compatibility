' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB quickSort (arr() AS STRING, first AS INTEGER, last AS INTEGER)
    ' DECLARE FUNCTION formatOID$ (oid AS STRING)
    
    Dim i As Short, j As Short
    Dim oids(5) As String
    oids(0) = "1.3.6.1.4.1.11.2.17.19.3.4.0.10"
    oids(1) = "1.3.6.1.4.1.11.2.17.5.2.0.79"
    oids(2) = "1.3.6.1.4.1.11.2.17.19.3.4.0.4"
    oids(3) = "1.3.6.1.4.1.11150.3.4.0.1"
    oids(4) = "1.3.6.1.4.1.11.2.17.19.3.4.0.1"
    oids(5) = "1.3.6.1.4.1.11150.3.4.0"
    
    For i = 0 To CShort(UBound(oids))
        oids(i) = formatOID$(oids(i))
    Next
    
    quickSort(oids, 0, CShort(UBound(oids)))
    
    For i = 0 To CShort(UBound(oids))
        Dim tmp As String : tmp = ""
        For j = 1 To CShort(Len(oids(i)))
            If Mid$(oids(i), j, 1) <> " " Then tmp = tmp + Mid$(oids(i), j, 1)
        Next
        PRINT(tmp)
    Next
    End
  End Sub

  
  Function formatOID$(ByRef oid As String)
      Dim result As String : result = ""
      Dim segment As String : segment = ""
      Dim i As Short : i = 1
      Dim lenOID As Short : lenOID = CShort(Len(oid))
  
      While i <= lenOID
          If Mid$(oid, i, 1) = "." Then
              result = ((result + Space$(5 - Len(segment))) + segment) + "."
              segment = ""
          Else
              segment = segment + Mid$(oid, i, 1)
          End If
          i = CShort(i + 1)
      End While
      result = (result + Space$(5 - Len(segment))) + segment
      formatOID$ = result
  End Function
  
  
  Sub quickSort(ByRef arr() As String, ByRef first As Short, ByRef last As Short)
      If first >= last Then Exit Sub
  
      Dim i As Short : i = first
      Dim j As Short : j = last
      Dim pivot As String : pivot = arr((first + last) \ 2)
      Dim temp As String
  
      Do
          While arr(i) < pivot : i = CShort(i + 1) : End While
          While arr(j) > pivot : j = CShort(j - 1) : End While
  
          If i <= j Then
              ' Swap
              temp = arr(i)
              arr(i) = arr(j)
              arr(j) = temp
              i = CShort(i + 1)
              j = CShort(j - 1)
          End If
      Loop Until i > j
  
      If first < j Then quickSort(arr, first, j)
      If i < last Then quickSort(arr, i, last)
  End Sub
  
End Module
