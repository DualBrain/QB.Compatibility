' Converted from QBasic by qb2vb.

Module Program

  Const MAXN As Short = 1000

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim d!
    Dim entry$
    ' CONST MAXN = 1000
    Dim x(MAXN) As Short
    Dim y(MAXN) As Short
    Dim e(4 * MAXN) As Short
    
    Dim xp = 127!
    Dim yp = 127!
    Dim a = 0!
    Dim na = 0!
    RANDOMIZE(CInt(Timer))
    Dim f! = Int(Rnd() * 4)
    
    Do
        a = na
        For n! = 0 To na - 1
            If (x(CInt(n)) = xp) And (y(CInt(n)) = yp) Then
                a = n
                Exit For
            End If
        Next n
    
        If a = na Then
            If na < MAXN Then
                x(CInt(na)) = CShort(xp)
                y(CInt(na)) = CShort(yp)
    
                For n = 0! To 3!
                    e(CInt((4 * na) + n)) = CShort(Int(Rnd() * 2))
                Next n
    
                For n! = 0 To na - 1
                    If (x(CInt(n)) = (x(CInt(na)) + 1)) And (y(CInt(n)) = y(CInt(na))) Then
                        e(CInt((4 * na) + 0)) = e(CInt((4 * n) + 2))
                    ElseIf (x(CInt(n)) = x(CInt(na))) And (y(CInt(n)) = (y(CInt(na)) + 1)) Then
                        e(CInt((4 * na) + 1)) = e(CInt((4 * n) + 3))
                    ElseIf (x(CInt(n)) = (x(CInt(na)) - 1)) And (y(CInt(n)) = y(CInt(na))) Then
                        e(CInt((4 * na) + 2)) = e(CInt((4 * n) + 0))
                    ElseIf (x(CInt(n)) = x(CInt(na))) And (y(CInt(n)) = (y(CInt(na)) - 1)) Then
                        e(CInt((4 * na) + 3)) = e(CInt((4 * n) + 1))
                    End If
                Next n
    
                na = na + 1
            Else
                PRINT("Max array size reached.")
                End
            End If
        End If
    
        PRINT("Paths:", True)
        Dim paths$(3)
        paths$(0) = " ahead"
        paths$(1) = " right"
        paths$(2) = " back"
        paths$(3) = " left"
    
        For n = 0! To 3!
            If e(CInt((4 * a) + (CInt((f + n)) Mod 4))) = 1 Then PRINT(paths$(CInt(n)), True)
        Next n
        PRINT()
    
        d = -1
        Do While d < 0
            INPUT("> ", entry$, False)
            entry$ = LCase$(entry$)
    
            Select Case entry$
                Case "ahead"
                    d = CInt((f + 0)) Mod 4
                Case "right"
                    d = CInt((f + 1)) Mod 4
                Case "back"
                    d = CInt((f + 2)) Mod 4
                Case "left"
                    d = CInt((f + 3)) Mod 4
                Case "quit"
                    End
                Case Else
                    PRINT("Invalid input.")
                    d = -1
            End Select
    
            If d >= 0 Then
                Select Case d
                    Case 0
                        If e(CInt((4 * a) + 0)) = 1 Then xp = xp + 1 : f = d Else d = -1
                    Case 1
                        If e(CInt((4 * a) + 1)) = 1 Then yp = yp + 1 : f = d Else d = -1
                    Case 2
                        If e(CInt((4 * a) + 2)) = 1 Then xp = xp - 1 : f = d Else d = -1
                    Case 3
                        If e(CInt((4 * a) + 3)) = 1 Then yp = yp - 1 : f = d Else d = -1
                End Select
            End If
    
            If d < 0 Then PRINT("No path.")
        Loop
    Loop
  End Sub

End Module
