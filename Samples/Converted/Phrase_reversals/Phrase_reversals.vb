' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim sString As String
    sString = "rosetta code phrase reversal"  'The string
    Dim sNewString As String  'String variables
    Dim sTemp As String  'Temporary string variable
    Dim siCount As Short  'Counter
    Dim sWord(100) As String  'Word store
    Dim i As Short  'Index variable
    Dim startPos As Short  'Start position for word extraction
    Dim endPos As Short  'End position for word extraction
    Dim wordCount As Short  'Number of words
    
    ' Loop backwards through the string
    For siCount = CShort(Len(sString)) To 1 Step -1
        sNewString = sNewString + Mid$(sString, siCount, 1)  'Add each character to the new string
    Next
    
    PRINT(("Original string => " + sString))  'Print the original string
    PRINT(("Reversed string => " + sNewString))  'Print the reversed string
    
    sNewString = ""  'Reset sNewString
    
    ' Manually split the original string by spaces
    startPos = 1
    wordCount = 0
    For i = 1 To CShort(Len(sString))
        If (Mid$(sString, i, 1) = " ") Or (i = Len(sString)) Then
            If i = Len(sString) Then
                endPos = i
            Else
                endPos = CShort(i - 1)
            End If
            sWord(wordCount) = Mid$(sString, startPos, (endPos - startPos) + 1)
            wordCount = CShort(wordCount + 1)
            startPos = CShort(i + 1)
        End If
    Next
    
    ' Loop backward through each word in sWord
    For siCount = CShort(wordCount - 1) To 0 Step -1
        sNewString = (sNewString + sWord(siCount)) + " "  'Add each word to sNewString
    Next
    
    PRINT(("Reversed order  => " + sNewString))  'Print reversed word order
    
    sNewString = ""  'Reset sNewString
    
    ' For each word in sWord
    For i = 0 To CShort(wordCount - 1)
        sTemp = sWord(i)
        ' Loop backward through the word
        For siCount = CShort(Len(sTemp)) To 1 Step -1
            sNewString = sNewString + Mid$(sTemp, siCount, 1)  'Add the characters to sNewString
        Next
        sNewString = sNewString + " "  'Add a space at the end of each word
    Next
    
    PRINT(("Reversed words  => " + sNewString))  'Print words reversed
  End Sub

End Module
