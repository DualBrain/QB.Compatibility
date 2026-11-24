' Converted from QBasic by qb2vb.

Module Program

  Const PACKETSIZE As Short = 10000
  Const [TRUE] As Short = -1

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim FileName$ = ""
    Dim Pattern$ = ""
    Dim Buffer$ = ""
    Dim Start As Short
    Dim StringPos As Short
    Dim FoundIt As Short
    ' DEFLNG A-Z              ' Default variable type is long integer.
    LINE_INPUT("File to search: ", FileName$)
    LINE_INPUT("Pattern to search for: ", Pattern$)
    OPEN(FileName$, OpenMode.Binary, 1)
    
    ' CONST PACKETSIZE = 10000, TRUE = -1
    Dim PatternLength As Short = CShort(Len(Pattern$))
    Dim FileLength% = CInt(LOF(1))
    Dim BytesLeft% = FileLength
    Dim FileOffset = 0%
    
    ' Keep searching as long as there are enough bytes left in
    ' the file to contain the pattern you're searching for:
    Do While BytesLeft > PatternLength
    
       ' Read either 10,000 bytes or the number of bytes left in the file,
       ' whichever is smaller, then store them in Buffer$. (If the number
       ' of bytes left is less than PACKETSIZE, the following statement
       ' still reads just the remaining bytes, since binary I/O doesn't
       ' give "read past end" errors):
       Buffer$ = InputStr(PACKETSIZE, 1)
    
       ' Find every occurrence of the pattern in Buffer$:
       Start = 1
       Do
          StringPos = CShort(InStr(Start, Buffer$, Pattern$))
          If StringPos > 0 Then
    
             ' Found the pattern, so print the byte position in the file
             ' where the pattern starts:
             PRINT("Found pattern at byte number", True)
             PRINT(Str$(FileOffset + StringPos) & " ")
             Start = CShort(StringPos + 1)
             FoundIt = [TRUE]
          End If
       Loop While StringPos > 0
    
       ' Find the byte position where the next I/O operation would take place,
       ' then back up the file pointer a distance equal to the length of the
       ' pattern (in case the pattern straddles a 10,000-byte boundary):
       FileOffset = CInt(SEEK(1) - PatternLength)
       SEEK(1, FileOffset + 1)
    
       BytesLeft = FileLength - FileOffset
    Loop
    
    CLOSE(1)
    
    If (Not FoundIt) <> 0 Then PRINT("Pattern not found.")
    
  End Sub

End Module
