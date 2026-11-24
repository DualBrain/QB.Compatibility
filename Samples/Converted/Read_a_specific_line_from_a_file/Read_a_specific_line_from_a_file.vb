' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim linea$
    Dim f! = FREEFILE()
    OPEN("input.txt", OpenMode.Input, CInt(f))
    
    Dim lineapedida = 7!
    Dim cont = 0!
    Do While Not EOF(CInt(f))
       LINE_INPUT(CInt(f), linea$)
       cont = cont + 1
       If cont = lineapedida Then
          If linea$ = "" Then PRINT("The 7th line is empty") Else PRINT(linea$)
          Exit Do
       End If
    Loop
    If cont < lineapedida Then PRINT("There are only " & Str$(cont) & " " & " lines in the file")
    CLOSE(1)
  End Sub

End Module
