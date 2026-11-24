' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim N!
    Dim G!
    Dim a!
    Dim k!
    Dim h!
    Dim max!
    N = 7 : G = 5 : a = CSng(2 ^ (N + 1))  ' Author: DANILIN & Editor: Jjuanhdez or Unknow
    RANDOMIZE(CInt(Timer))
    Dim L(CInt(N)) As Single : Dim C(CInt(N)) As Single : Dim j(CInt(N)) As Single : Dim q(CInt(a)) As Single : Dim d(CInt(a)) As Single : Dim q_Str$(CInt(a))
    
    For i! = a - 1 To CInt((a - 1)) \ 2 Step -1
        k = i
        Do  ' cipher 0-1
            q_Str$(CInt(i)) = LTrim$(Str$(CInt(k) Mod 2)) + q_Str$(CInt(i))
            k = Int(k / 2)
        Loop Until k = 0
        q_Str$(CInt(i)) = Mid$(q_Str$(CInt(i)), 2, Len(q_Str$(CInt(i))))
    Next i
    
    PRINT(" #            Mass          Cost")
    For i! = 1 To N
        L(CInt(i)) = Int((Rnd() * 3) + 1)  ' mass & cost
        C(CInt(i)) = 10 + Int(Rnd() * 9)
        PRINT(Str$(i) & " " & vbTab & Str$(L(CInt(i))) & " " & vbTab & Str$(C(CInt(i))) & " ")
    Next i  ' origin
    
    PRINT((QBChr(10) + "Mass          Cost           Chifer"))
    For h = a - 1 To (a - 1) / 2 Step -1
        For k = 1 To N
            j(CInt(k)) = CSng(Val(Mid$(q_Str$(CInt(h)), CInt(k), 1)))  ' j() = cipher
            q(CInt(h)) = q(CInt(h)) + ((L(CInt(k)) * j(CInt(k))) * C(CInt(k)))  ' 0 or 1
            d(CInt(h)) = d(CInt(h)) + (L(CInt(k)) * j(CInt(k)))
        Next k
        If d(CInt(h)) <= G Then PRINT(Str$(d(CInt(h))) & " " & vbTab & Str$(q(CInt(h))) & " " & vbTab & q_Str$(CInt(h)))
    Next h
    
    PRINT((QBChr(10) + "Mass          MAX            Chifer"))
    max = 0 : h = 1
    For i! = 1 To a
        If (d(CInt(i)) <= G) And (q(CInt(i)) > max) Then max = q(CInt(i)) : h = i
    Next i
    PRINT(Str$(d(CInt(h))) & " " & vbTab & Str$(q(CInt(h))) & " " & vbTab & q_Str$(CInt(h)))
  End Sub

End Module
