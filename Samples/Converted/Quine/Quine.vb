' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim d$
    Dim x$
    DATA("DATA")
    DATA("READ d$")
    DATA("DO")
    DATA("READ x$")
    DATA("PRINT x$")
    DATA("LOOP UNTIL LEN(x$) < 1")
    DATA("RESTORE")
    DATA("DO")
    DATA("READ x$")
    DATA("PRINT d$; CHR$(34); x$; CHR$(34)")
    DATA("LOOP UNTIL LEN(x$) < 1")
    DATA("END")
    DATA("")
    
    READ(d$)
    Do
        READ(x$)
        PRINT(x$)
    Loop Until Len(x$) < 1
    RESTORE()
    Do
        READ(x$)
        PRINT(d$ & QBChr(34) & x$ & QBChr(34))
    Loop Until Len(x$) < 1
    End
    
    ' DATA "DATA "   (DATA is listed at the start of the program)
    ' DATA "READ d$"   (DATA is listed at the start of the program)
    ' DATA "DO"   (DATA is listed at the start of the program)
    ' DATA "    READ x$"   (DATA is listed at the start of the program)
    ' DATA "    PRINT x$"   (DATA is listed at the start of the program)
    ' DATA "LOOP UNTIL LEN(x$) < 1"   (DATA is listed at the start of the program)
    ' DATA "RESTORE"   (DATA is listed at the start of the program)
    ' DATA "DO"   (DATA is listed at the start of the program)
    ' DATA "    READ x$"   (DATA is listed at the start of the program)
    ' DATA "    PRINT d$; CHR$(34); x$; CHR$(34)"   (DATA is listed at the start of the program)
    ' DATA "LOOP UNTIL LEN(x$) < 1"   (DATA is listed at the start of the program)
    ' DATA "END"   (DATA is listed at the start of the program)
    ' DATA ""   (DATA is listed at the start of the program)
  End Sub

End Module
