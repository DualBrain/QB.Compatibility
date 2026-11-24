' Converted from QBasic by qb2vb.

Module Program

  Const [FALSE] As Short = 0
  Const [TRUE] As Short = Not [FALSE]

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim Balance!
    Dim TransacNum As Short
    Dim Swaps As Short
    Dim Amount(100) As Single
    ' CONST FALSE = 0, TRUE = NOT FALSE
    
    ' Get account's starting balance:
    CLS()
    INPUT("Type starting balance, then press <ENTER>: ", Balance, False)
    
    ' Get transactions. Continue accepting input until the
    ' input is zero for a transaction, or until 100
    ' transactions have been entered:
    For TransacNum = 1 To 100
       PRINT(Str$(TransacNum) & " ", True)
       PRINT(") Enter transaction amount (0 to end): ", True)
       INPUT("", Amount(TransacNum), False)
       If Amount(TransacNum) = 0 Then
          TransacNum = CShort(TransacNum - 1)
          Exit For
       End If
    Next
    
    ' Sort transactions in ascending order,
    ' using a "bubble sort":
    Dim Limit As Short = TransacNum
    Do
       Swaps = [FALSE]
       For I As Short = 1 To CShort((Limit - 1))
    
          ' If two adjacent elements are out of order, switch
          ' those elements:
          If Amount(I) < Amount(I + 1) Then
             SWAP(Amount(I), Amount(I + 1))
             Swaps = I
          End If
       Next I
    
       ' Sort on next pass only to where the last switch was made:
       If Swaps <> 0 Then Limit = Swaps
    
    ' Sort until no elements are exchanged:
    Loop While Swaps <> 0
    
    ' Print the sorted transaction array. If a transaction
    ' is greater than zero, print it as a "CREDIT"; if a
    ' transaction is less than zero, print it as a "DEBIT":
    For I As Short = 1 To TransacNum
       If Amount(I) > 0 Then
          PRINT_USING("CREDIT: $$#####.##", Amount(I))
       ElseIf Amount(I) < 0 Then
          PRINT_USING("DEBIT:  $$#####.##", Amount(I))
       End If
    
       ' Update balance:
       Balance = Balance + Amount(I)
    Next I
    
    ' Print the final balance:
    PRINT()
    PRINT("--------------------------")
    PRINT_USING("Final Total: $$######.##", Balance)
    End
  End Sub

End Module
