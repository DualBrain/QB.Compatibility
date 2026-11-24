' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION AlphaLeft$ (ct$, pt$, CharPos!)
    ' DECLARE FUNCTION AlphaRight$ (ct$, pt$, CharPos!)
    ' DECLARE FUNCTION Decode$ (Text$, ct$, pt$)
    ' DECLARE FUNCTION Encode$ (Text$, ct$, pt$)
    
    CLS()
    
    ' Deciphering a Chaocipher-encrypted message is identical to the steps used
    ' for enciphering. The sole difference is that the decipherer locates the
    ' known ciphertext letter in the left (ct$) alphabet, with the plaintext
    ' letter being the corresponding letter in the right (pt$) alphabet
    '
    ' Alphabet permuting is identical in enciphering and deciphering
    
    ' Start of Main Code
    
    ' LEFT (Cipher Text$): HXUCZVAMDSLKPEFJRIGTWOBNYQ
    Dim tLeft$ = "HXUCZVAMDSLKPEFJRIGTWOBNYQ"
    
    ' RIGHT (Plain Text$): PTLNBQDEOYSFAVZKGJRIHWXUMC
    Dim tRight$ = "PTLNBQDEOYSFAVZKGJRIHWXUMC"
    
    ' Cipher Message (Used to verify a good encoding)
    Dim cText$ = "OAHQHCNYNXTSZJRRHJBYHQKSOUJY"
    
    ' Plain Text$ Message
    Dim pText$ = "WELLDONEISBETTERTHANWELLSAID"
    PRINT(" Plain  Text$: " & pText$)
    PRINT()
    
    Dim ctLeft$ = tLeft$
    Dim ptRight$ = tRight$
    
    ' Final Cipher Text$
    Dim eText$ = Encode$(pText$, ctLeft$, ptRight$)
    PRINT(" Cipher Text$: " & eText$)
    PRINT()
    
    If eText$ = cText$ Then PRINT("Successful") Else PRINT("Failed")
    
    ctLeft$ = tLeft$
    ptRight$ = tRight$
    Dim dText$ = Decode$(eText$, ctLeft$, ptRight$)
    PRINT()
    PRINT(" Plain Text$: " & dText$)
    PRINT()
    
    If dText$ = pText$ Then PRINT("Successful") Else PRINT("Failed")
    End
  End Sub

  
  ' Left Alphabet
  Function AlphaLeft$(ByRef ct$, ByRef pt$, ByRef CharPos As Single)
      Dim tStr$ = ct$
  
      ' 1. Shift the entire left alphabet cyclically so the ciphertext letter
      ' just enciphered is positioned at the zenith (i.e., position 1).
      tStr$ = Right$(ct$, CInt((Len(ct$) - CharPos) + 1)) + Left$(ct$, CInt(CharPos - 1))
  
      ' 2. Extract the letter found at position zenith+1 (i.e., the letter to
      ' the right of the zenith), taking it out of the alphabet, temporarily
      ' leaving an unfilled "Hole$"
  
      Dim Hole$ = Mid$(tStr$, 2, 1)
  	Mid(tStr$, 2, 1) = " "
  
      ' 3. Shift all letters in positions zenith+2 up to, and including, the
      ' nadir (zenith+13), moving them one position to the left
  
      tStr$ = ((Left$(tStr$, 1) + Mid$(tStr$, 3, 12)) + " ") + Right$(tStr$, 12)
  
      ' 4. Insert the just-extracted letter into the nadir position
      ' (i.e., zenith+13)
  
      Mid(tStr$, 14, 1) = Hole$
  
      AlphaLeft$ = tStr$
  End Function
  
  
  ' Right Alphabet
  Function AlphaRight$(ByRef ct$, ByRef pt$, ByRef CharPos As Single)
      Dim tStr$ = pt$
  
      ' 1. Shift the entire right alphabet cyclically so the plaintext letter
      ' just enciphered is positioned at the zenith.
  
      tStr$ = Right$(tStr$, CInt((Len(tStr$) - CharPos) + 1)) + Left$(tStr$, CInt(CharPos - 1))
  
      ' 2. Now shift the entire alphabet one more position to the left (i.e.,
      ' the leftmost letter moves cyclically to the far right), moving a new
      ' letter into the zenith position.
  
      tStr$ = Right$(tStr$, 25) + Left$(tStr$, 1)
  
      ' 3. Extract the letter at position zenith+2, taking it out of the
      ' alphabet, temporarily leaving an unfilled "Hole$".
  
      Dim Hole$ = Mid$(tStr$, 3, 1)
  	Mid(tStr$, 3, 1) = " "
  
      ' 4. Shift all letters beginning with zenith+3 up to, and including, the
      ' nadir (zenith+13), moving them one position to the left.
  
      tStr$ = ((Left$(tStr$, 2) + Mid$(tStr$, 4, 11)) + " ") + Right$(tStr$, 12)
  
      ' 5. Insert the just-extracted letter into the nadir position (zenith+13)
  
      Mid(tStr$, 14, 1) = Hole$
  
      AlphaRight$ = tStr$
  End Function
  
  
  Function Decode$(ByRef Text$, ByRef ct$, ByRef pt$)
    Dim Char_$
    Dim CharPos!
      Dim tStr$ = ""
  
      For t! = 1 To Len(Text$)
  	Char_$ = Mid$(Text$, CInt(t), 1)
  	CharPos = InStr(ct$, Char_$)
  
  	ct$ = AlphaLeft$(ct$, pt$, CharPos)
  	pt$ = AlphaRight$(ct$, pt$, CharPos)
  
  	tStr$ = tStr$ + Right$(pt$, 1)
      Next
  
      Decode$ = tStr$
  End Function
  
  
  Function Encode$(ByRef Text$, ByRef ct$, ByRef pt$)
    Dim Char_$
    Dim CharPos!
      Dim tStr$ = ""
  
      For t! = 1 To Len(Text$)
  	Char_$ = Mid$(Text$, CInt(t), 1)
  	CharPos = InStr(pt$, Char_$)
  
  	ct$ = AlphaLeft$(ct$, pt$, CharPos)
  	pt$ = AlphaRight$(ct$, pt$, CharPos)
  
  	tStr$ = tStr$ + Left$(ct$, 1)
      Next
  
      Encode$ = tStr$
  End Function
  
End Module
