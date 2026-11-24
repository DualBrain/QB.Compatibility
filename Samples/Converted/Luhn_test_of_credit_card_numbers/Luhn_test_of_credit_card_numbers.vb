' Converted from QBasic by qb2vb.

Module Program

  Const [True] As Short = -1

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim [False] As Single
    Dim odd!
    Dim sum!
    Dim num!
    ' CONST True = -1: False = NOT True
    [False] = Not [True]
    
    Dim card$(5)
    card$(1) = "49927398716"
    card$(2) = "49927398717"
    card$(3) = "1234567812345678"
    card$(4) = "1234567812345670"
    
    For test = 1! To 4!
        odd = [True]
        sum = 0
        For n! = Len(card$(CInt(test))) To 1 Step -1
            num = CSng(Val(Mid$(card$(CInt(test)), CInt(n), 1)))
            If odd <> 0 Then
               sum = sum + num
               odd = [False]
            Else
               num = num * 2
               If num <= 9 Then
                  sum = sum + num
               Else
                  sum = (sum + num) - 9
               End If
               odd = [True]
            End If
        Next
        If (CInt(sum) Mod 10) = 0 Then
           PRINT(card$(CInt(test)) & vbTab & "True")
        Else
           PRINT(card$(CInt(test)) & vbTab & "False")
        End If
    Next test
  End Sub

End Module
