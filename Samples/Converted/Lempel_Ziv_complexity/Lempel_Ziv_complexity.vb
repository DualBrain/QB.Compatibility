' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB LempelZivComplexity (seq$, subs$(), nsubs%)
    ' DECLARE FUNCTION isInSet% (subs$(), cnt%, txt$)
    
    Dim tests(5) As String
    tests(0) = "AZSEDRFTGYGUJIJOKB"
    tests(1) = "ABCABCABCABCABCABC"
    tests(2) = "101001010010111110"
    tests(3) = "1001111011000010"
    tests(4) = "0001"
    tests(5) = "010"
    
    Dim t As Short, i As Short, cnt As Short
    
    For t = 0 To 5
        Dim substrings(50) As String  ' Size LARGER than required
    
        cnt = 0
        LempelZivComplexity(tests(t), substrings, cnt)
    
    	PRINT((tests(t) + " -> complexity = ") & Str$(cnt) & " ")
        PRINT("substrings are: <", True)
        For i = 0 To CShort(cnt - 1)
    	PRINT(substrings(i), True)
    	If i < (cnt - 1) Then PRINT(", ", True)
        Next
        PRINT(">")
        PRINT()
    Next
  End Sub

  
  Function isInSet(ByRef subs$(), ByRef cnt As Short, ByRef txt$) As Short
      For i As Short = 0 To CShort(cnt - 1)
  	If subs$(i) = txt$ Then
  	    isInSet = -1
  	    Exit Function
  	End If
      Next
      isInSet = 0
  End Function
  
  
  Sub LempelZivComplexity(ByRef seq$, ByRef subs$(), ByRef nsubs As Short)
      Dim n As Short : Dim i As Short : Dim k As Short : Dim tmp$
  
      n = CShort(Len(seq$))
      nsubs = 0
      If n = 0 Then
  	subs$(0) = ""
  	Exit Sub
      End If
      i = 0
      Do While i < n
  	k = 1
  	While ((i + k) <= n) And (isInSet(subs$, nsubs, Mid$(seq$, i + 1, k)) <> 0)
  	    k = CShort(k + 1)
  	End While
  	If (i + k) <= n Then
  	    ' Add only if it is not already
  	    tmp$ = Mid$(seq$, i + 1, k)
  	    If isInSet(subs$, nsubs, tmp$) = 0 Then
  		subs$(nsubs) = tmp$
  		nsubs = CShort(nsubs + 1)
  	    End If
  	    i = i + k
  	Else
  	    tmp$ = Mid$(seq$, i + 1, n - i)
  	    If isInSet(subs$, nsubs, tmp$) = 0 Then
  		subs$(nsubs) = tmp$
  		nsubs = CShort(nsubs + 1)
  	    End If
  	    Exit Do
  	End If
      Loop
  End Sub
  
End Module
