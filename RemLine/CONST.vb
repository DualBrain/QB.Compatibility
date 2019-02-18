Option Explicit Off


Imports QB

Module CONSTsample

  Const ESC = 27, ENTER = 13, CTRLBACKSPACE = 127

  Sub Main()

    CLS()
    PRINT("Type something... (press Ctrl - Backspace to quit).")
    PRINT()
    LOCATE(,, 1)

    Do
      Do
        inky$ = INKEY$()
      Loop While inky$ = ""

      Select Case Asc(inky$)
        Case ENTER
          PRINT(" [ENTER] ", True)
        Case ESC
          PRINT(" [ESC] ", True)
        Case CTRLBACKSPACE
          PRINT(" [CTRL-BACKSPACE] ", True)
          Exit Do
        Case Else
          PRINT(inky$, True)
      End Select

    Loop

  End Sub

End Module