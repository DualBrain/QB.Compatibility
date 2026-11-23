' SWAP  -  https://qbasic.net/en/qb-manual/Statement/SWAP.htm
' A VB translation of the QBasic example; see the notes in the code where VB differs.

Imports QB
Imports QB.Core
Imports QB.Video

Namespace Examples

  Friend Module SwapExample

    Public Sub Run()
    Dim a = 1, b = 2
    PRINT("Before: " & Str(a) & vbTab & Str(b))
    SWAP(a, b)
    PRINT("After: " & Str(a) & vbTab & Str(b))
    PRINT()
    ' Sort the word list using a Shell sort (QBasic arrays start at 1; element 0 is unused here).
    Dim num = 4
    Dim array(num) As String
    array(1) = "New York"
    array(2) = "Boston"
    array(3) = "Chicago"
    array(4) = "Seattle"
    Dim span = num \ 2
    Do While span > 0
      For i = span To num - 1
        For j = (i - span + 1) To 1 Step -span
          If String.Compare(array(j), array(j + span), StringComparison.Ordinal) <= 0 Then Exit For
          ' Swap array elements that are out of order.
          SWAP(array(j), array(j + span))
        Next
      Next
      span \= 2
    Loop
    For i = 1 To num
      PRINT(array(i))
    Next
    End Sub

  End Module

End Namespace
