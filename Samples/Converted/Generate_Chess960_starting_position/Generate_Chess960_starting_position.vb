' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim inicio$
    Dim pieza$
    Dim posic!
    RANDOMIZE(CInt(Timer))  ' DELETE for Run BASIC, Just Basic & Liberty BASIC
    
    For i = 1! To 10!
        inicio$ = "RKR"
        pieza$ = "QNN"
    
        For n! = 1 To Len(pieza$)
            posic = Int(Rnd() * (Len(inicio$) + 1)) + 1
            inicio$ = (Left$(inicio$, CInt(posic - 1)) + Mid$(pieza$, CInt(n), 1)) + Right$(inicio$, CInt((Len(inicio$) - posic) + 1))
        Next n
        posic = Int(Rnd() * (Len(inicio$) + 1)) + 1
        inicio$ = (Left$(inicio$, CInt(posic - 1)) + "B") + Right$(inicio$, CInt((Len(inicio$) - posic) + 1))
        posic = (posic + 1) + (2 * Int(Int(Rnd() * (Len(inicio$) - posic)) / 2))
        inicio$ = (Left$(inicio$, CInt(posic - 1)) + "B") + Right$(inicio$, CInt((Len(inicio$) - posic) + 1))
        PRINT(inicio$)
    Next i
    End
  End Sub

End Module
