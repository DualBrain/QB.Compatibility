' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION life$ (lastGen$)
    ' DECLARE FUNCTION getNeighbors! (group$)
    CLS()
    Dim start$ = "_###_##_#_#_#_#__#__"
    Dim numGens = 10!
    For i! = 0 To numGens - 1
    	PRINT("Generation" & Str$(i) & " " & ": " & start$)
    	start$ = life$(start$)
    Next i
  End Sub

  
  Function getNeighbors(ByRef group$) As Single
  		Dim ans = 0!
  		If (Mid$(group$, 1, 1) = "#") Then ans = ans + 1
  		If (Mid$(group$, 3, 1) = "#") Then ans = ans + 1
  		getNeighbors = ans
  End Function
  
  
  Function life$(ByRef lastGen$)
    Dim neighbors!
  		Dim newGen$ = ""
  		For i! = 1 To Len(lastGen$)
  			neighbors = 0
  			If (i = 1) Then  'left edge
  				If Mid$(lastGen$, 2, 1) = "#" Then
  					neighbors = 1
  				Else
  					neighbors = 0
  				End If
  			ElseIf (i = Len(lastGen$)) Then  'right edge
  				If Mid$(lastGen$, Len(lastGen$) - 1, 1) = "#" Then
  					neighbors = 1
  				Else
  					neighbors = 0
  				End If
  			Else  'middle
  				neighbors = getNeighbors(Mid$(lastGen$, CInt(i - 1), 3))
  			End If
  
  			If (neighbors = 0) Then  'dies or stays dead with no neighbors
  				newGen$ = newGen$ + "_"
  			End If
  			If (neighbors = 1) Then  'stays with one neighbor
  				newGen$ = newGen$ + Mid$(lastGen$, CInt(i), 1)
  			End If
  			If (neighbors = 2) Then  'flips with two neighbors
  				If Mid$(lastGen$, CInt(i), 1) = "#" Then
  					newGen$ = newGen$ + "_"
  				Else
  					newGen$ = newGen$ + "#"
  				End If
  			End If
  		Next i
  		life$ = newGen$
  End Function
  
End Module
