' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    
    Dim dimen = 5!
    Dim a(CInt(dimen)) As Single
    Dim b(CInt(dimen)) As Single
    
    For i! = 1 To dimen
    	a(CInt(i)) = i
    	b(CInt(i)) = i + dimen
    Next i
    
    Dim nt! = ConcatArrays(a, b)
    
    For i! = 1 To nt
    	PRINT(Str$(a(CInt(i))) & " ", True)
    	If i < nt Then PRINT(", ", True)
    Next i
  End Sub

  Function ConcatArrays(ByRef a() As Single, ByRef b() As Single) As Single
  	Dim ta! = UBound(a)
  	Dim tb! = UBound(b)
  
  	Dim nt! = ta + tb
  
  	For i! = ta + 1 To nt
  		a(CInt(i)) = b(CInt(i - ta))
  	Next i
  
  	ConcatArrays = nt
  End Function
  
End Module
