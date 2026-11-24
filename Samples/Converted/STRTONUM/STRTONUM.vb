' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim A$
    ' DECLARE FUNCTION Filter$ (Txt$, FilterString$)
    
    ' Input a line:
    LINE_INPUT("Enter a number with commas: ", A$)
    
    ' Look for only valid numeric characters (0123456789.-) in the
    ' input string:
    Dim CleanNum$ = Filter$(A$, "0123456789.-")
    
    ' Convert the string to a number:
    PRINT("The number's value = " & Str$(Val(CleanNum$)) & " ")
    End
  End Sub

  '
  ' ========================== FILTER ==========================
  '         Takes unwanted characters out of a string by
  '        comparing them with a filter string containing
  '             only acceptable numeric characters
  ' ============================================================
  '
  Function Filter$(ByRef Txt$, ByRef FilterString$)
    Static Temp$
    Static TxtLength!
    Static I!
    Static C$
     Temp$ = ""
     TxtLength = Len(Txt$)
  
     For I = 1 To TxtLength  ' Isolate each character in
        C$ = Mid$(Txt$, CInt(I), 1)  ' the string.
  
        ' If the character is in the filter string, save it:
        If InStr(FilterString$, C$) <> 0 Then
           Temp$ = Temp$ + C$
        End If
     Next I
  
     Filter$ = Temp$
  End Function
  
End Module
