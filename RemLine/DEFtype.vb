Option Explicit Off

Imports QB.Console
Imports QB.Core

Module DEFtype

  Sub Main()

    'Dim a, g As Double 'DEFDBL A, G
    Dim i As Integer 'DEFINT i
    Dim x As Long 'DEFLNG X
    'Dim s As Single 'DEFSNG s
    'Dim y As String 'DEFSTR y

    CLS()

    grandTotal# = 0
    x = 1

    Dim yesNo$ = ""

    Do

      PRINT("How many items on receipt", False)
      INPUT(itemsOnReceipt%)

      subTotal! = 0

      For i = 1 To itemsOnReceipt
        PRINT($"Enter amount of item {i}", False)
        INPUT(amt!)
        subTotal! += amt!
        grandTotal# += amt!
        PRINT($"{TAB(70)}{[USING]("##,###.##", subTotal!)}")
      Next

      PRINT($"{TAB(70)}---------")
      PRINT($"{TAB(70)}{[USING]("##,###.##", grandTotal#)}")
      PRINT()

      INPUT("Do you want to add up more receipts", yesNo$)
      x += 1

    Loop Until UCase(yesNo$) = "N"

  End Sub

End Module
