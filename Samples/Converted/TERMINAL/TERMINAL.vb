' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim KeyInput$
    Dim ModemInput$
    ' DEFINT A-Z
    
    ' DECLARE SUB Filter (InString$)
    
    COLOR(7, 1)  ' Set screen color.
    CLS()
    
    Dim Quit$ = QBChr(0) + QBChr(16)  ' Value returned by INKEY$
                                    ' when ALT+q is pressed.
    
    ' Set up prompt on bottom line of screen and turn cursor on:
    LOCATE(24, 1, 1)
    PRINT(QBString(80, "_"), True)
    LOCATE(25, 1)
    PRINT(TAB(30) & "Press ALT+q to quit", True)
    
    VIEW_PRINT(1, 23)  ' Print between lines 1 & 23.
    
    ' Open communications (1200 baud, no parity, 8-bit data,
    ' 1 stop bit, 256-byte input buffer):
    OPEN("COM1:1200,N,8,1", OpenMode.Random, 1, 256)
    
    Do  ' Main communications loop.
    
       KeyInput$ = INKEY$()  ' Check the keyboard.
    
       If KeyInput$ = Quit$ Then  ' Exit the loop if the user
          Exit Do  ' pressed ALT+q.
    
       ElseIf KeyInput$ <> "" Then  ' Otherwise, if the user has
          PRINT(1, KeyInput$, True)  ' pressed a key, send the
       End If  ' character typed to the modem.
    
       ' Check the modem. If characters are waiting (EOF(1) is
       ' true), get them and print them to the screen:
       If Not EOF(1) Then
    
          ' LOC(1) gives the number of characters waiting:
          ModemInput$ = InputStr(CInt(LOC(1)), 1)
    
          Filter(ModemInput$)  ' Filter out line feeds and
          PRINT(ModemInput$, True)  ' backspaces, then print.
       End If
    Loop
    
    CLOSE()  ' End communications.
    CLS()
    End
  End Sub

  '
  ' ========================= FILTER ==========================
  '     Filters characters in an input string.
  ' ============================================================
  '
  Sub Filter(ByRef InString$)
    Static BackSpace As Short
    Static LineFeed As Short
  
     ' Look for backspace characters and recode them to
     ' CHR$(29) (the LEFT cursor key):
     Do
        BackSpace = CShort(InStr(InString$, QBChr(8)))
        If BackSpace <> 0 Then
           Mid(InString$, BackSpace) = QBChr(29)
        End If
     Loop While BackSpace <> 0
  
     ' Look for line-feed characters and remove any found:
     Do
        LineFeed = CShort(InStr(InString$, QBChr(10)))
        If LineFeed <> 0 Then
           InString$ = Left$(InString$, LineFeed - 1) + Mid$(InString$, LineFeed + 1)
        End If
     Loop While LineFeed <> 0
  
  End Sub
  
End Module
