' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim P$
    ' TOKEN.BAS
    '
    ' Demonstrates a BASIC version of the strtok C function.
    '
    ' DECLARE FUNCTION StrTok$(Source$,Delimiters$)
    
    LINE_INPUT("Enter string: ", P$)
    ' Set up the characters that separate tokens.
    Dim Delimiters$ = (" ,;:().?" + QBChr(9)) + QBChr(34)
    ' Invoke StrTok$ with the string to tokenize.
    Dim Token$ = StrTok$(P$, Delimiters$)
    While Token$ <> ""
       PRINT(Token$)
       ' Call StrTok$ with a null string so it knows this
       ' isn't the first call.
       Token$ = StrTok$("", Delimiters$)
    End While
  End Sub

  
  Function StrTok$(ByRef Srce$, ByRef Delim$)
    Dim BegPos As Short
    Dim Ln As Short
  Static Start As Short : Static SaveStr$
  
     ' If first call, make a copy of the string.
     If Srce$ <> "" Then
        Start = 1 : SaveStr$ = Srce$
     End If
  
     BegPos = Start : Ln = CShort(Len(SaveStr$))
     ' Look for start of a token (character that isn't delimiter).
     While (BegPos <= Ln) And (InStr(Delim$, Mid$(SaveStr$, BegPos, 1)) <> 0)
        BegPos = CShort(BegPos + 1)
     End While
     ' Test for token start found.
     If BegPos > Ln Then
        StrTok$ = "" : Exit Function
     End If
     ' Find the end of the token.
     Dim EndPos As Short = BegPos
     While (EndPos <= Ln) And (InStr(Delim$, Mid$(SaveStr$, EndPos, 1)) = 0)
        EndPos = CShort(EndPos + 1)
     End While
     StrTok$ = Mid$(SaveStr$, BegPos, EndPos - BegPos)
     ' Set starting point for search for next token.
     Start = EndPos
  
  End Function
  
End Module
