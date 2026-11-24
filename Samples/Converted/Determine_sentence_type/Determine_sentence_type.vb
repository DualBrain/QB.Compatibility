' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE FUNCTION SentenceType$ (s AS STRING)
    
    Dim spam(3) As String
    spam(0) = "hi there, how are you today?"
    spam(1) = "I'd like to present to you the washing machine 9001."
    spam(2) = "You have been nominated to win one of these!"
    spam(3) = "Just make sure you don't break it"
    
    For i! = 0 To UBound(spam)
        PRINT(spam(CInt(i)) & " -> " & SentenceType$(spam(CInt(i))))
    Next i
  End Sub

  
  Function SentenceType$(ByRef s As String)
      Select Case Right$(s, 1)
          Case "?"
              SentenceType$ = "Q"
          Case "!"
              SentenceType$ = "E"
          Case "."
              SentenceType$ = "S"
          Case Else
              SentenceType$ = "N"
      End Select
  End Function
  
End Module
