' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim o$
    CLS()
    
    'header row
    PRINT("     ", True)
    For n = 1! To 12!
        'do it this way for alignment purposes
        o$ = "    "
        Mid(o$, (Len(o$) - Len(Str$(n))) + 1) = Str$(n)
        PRINT(o$, True)
    Next
    PRINT() : PRINT("    " & QBString(49, "-"), True)
    
    For n = 1! To 12!
        PRINT()
        If n < 10 Then PRINT(" ", True)
        PRINT(Str$(n) & " " & "|", True)  'row labels
        For m! = 1 To n - 1
            PRINT("    ", True)
        Next
        For m! = n To 12
            'alignment again
            o$ = "    "
            Mid(o$, (Len(o$) - Len(Str$(m * n))) + 1) = Str$(m * n)
            PRINT(o$, True)
        Next
    Next
  End Sub

End Module
