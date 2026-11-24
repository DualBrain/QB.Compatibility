' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim i!
    Dim xyear!
    Dim yelement$
    Dim yanimal$
    Dim yyinyang$
    Dim nn!
    DATA_LABEL("290")
    DATA("Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig")
    DATA_LABEL("300")
    DATA("Wood", "Fire", "Earth", "Metal", "Water")
    DATA_LABEL("310")
    DATA(1935, 1938, 1968, 1972, 1976, 1984, 2017)
    
    10:  ' Chinese zodiac
    20: CLS()
    30: Dim animals$(12)
    40: Dim elements$(5)
    50: Dim yinyang$(2)
    70: For i = 0 To 11
    80: READ(animals$(CInt(i)))
    90: Next i
    110: For i = 0 To 4
    120: READ(elements$(CInt(i)))
    130: Next i
    140: yinyang$(0) = "Yang" : yinyang$(1) = "Yin"
    150: Dim years(7) As Single
    170: For i = 0 To 6
    180: READ(years(CInt(i)))
    190: Next i
    200: For i = 0 To 6
    210: xyear = years(CInt(i))
    220: yelement$ = elements$(CInt((CInt((xyear - 4)) Mod 10) / 2))
    230: yanimal$ = animals$(CInt((xyear - 4)) Mod 12)
    240: yyinyang$ = yinyang$(CInt(xyear) Mod 2)
    250: nn = (CInt((xyear - 4)) Mod 60) + 1
    260: PRINT(Str$(xyear) & " " & "is the year of the " & yelement$ & " " & yanimal$ & " (" & yyinyang$ & ").")
    270: Next i
    280: End
    ' 290 DATA "Rat","Ox","Tiger","Rabbit","Dragon","Snake","Horse","Goat","Monkey","Rooster","Dog","Pig"   (DATA is listed at the start of the program)
    ' 300 DATA "Wood","Fire","Earth","Metal","Water"   (DATA is listed at the start of the program)
    ' 310 DATA 1935,1938,1968,1972,1976,1984,2017   (DATA is listed at the start of the program)
  End Sub

End Module
