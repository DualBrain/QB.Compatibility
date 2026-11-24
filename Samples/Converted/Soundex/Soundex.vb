' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim nombre$
    DATA("Aschraft", "Ashcroft", "Euler", "Gauss", "Ghosh", "Hilbert", "Heilbronn", "Lee", "Lissajous", "Lloyd")
    DATA("Moses", "Pfister", "Robert", "Rupert", "Rubin", "Tymczak", "VanDeusen", "Wheaton", "Soundex$", "Example")
    
    ' DECLARE FUNCTION getCode$ (c$)
    ' DECLARE FUNCTION Soundex$ (palabra$)
    
    For i = 1! To 20!
        READ(nombre$)
        PRINT("" & "" & nombre$ & "" & "" & TAB(15) & Soundex$(nombre$))
    Next i
    
    ' DATA "Aschraft","Ashcroft","Euler","Gauss","Ghosh","Hilbert","Heilbronn","Lee","Lissajous","Lloyd"   (DATA is listed at the start of the program)
    ' DATA "Moses","Pfister","Robert","Rupert","Rubin","Tymczak","VanDeusen","Wheaton","Soundex$","Example"   (DATA is listed at the start of the program)
  End Sub

  
  Function getCode$(ByRef c$)
      If InStr("BFPV", c$) <> 0 Then getCode$ = "1"
      If InStr("CGJKQSXZ", c$) <> 0 Then getCode$ = "2"
      If InStr("DT", c$) <> 0 Then getCode$ = "3"
      If "L" = c$ Then getCode$ = "4"
      If InStr("MN", c$) <> 0 Then getCode$ = "5"
      If "R" = c$ Then getCode$ = "6"
      If InStr("HW", c$) <> 0 Then getCode$ = "."
  End Function
  
  
  Function Soundex$(ByRef palabra$)
    Dim actual$
      palabra$ = UCase$(palabra$)
      Dim code$ = Mid$(palabra$, 1, 1)
      Dim previo$ = getCode$(Left$(palabra$, 1))  ''""
  
      For i! = 2 To (Len(palabra$))
          actual$ = getCode$(Mid$(palabra$, CInt(i), 1))
          If actual$ <> "." Then
              If (Len(actual$) > 0) And (actual$ <> previo$) Then code$ = code$ + actual$
              previo$ = actual$
              If Len(code$) = 4 Then Exit For
          End If
      Next i
      If Len(code$) < 4 Then code$ = code$ + QBString(4 - Len(code$), "0")
      Soundex$ = Left$(code$, 4)
  End Function
  
End Module
