' Converted from QBasic by qb2vb.

Module Program

  Const PI As Double = 3.141592653589#

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim Cycles!
    Dim Y!
    SCREEN(2)
    
    ' View port sized to proper scale for graph:
    VIEW(20, 2, 620, 172, -1, 1)
    
    ' CONST PI = 3.141592653589#
    
    ' Make window large enough to graph sine wave from
    ' 0 radians to 2π radians:
    WINDOW(0, -1.1!, 2 * PI, 1.1!)
    
    Dim Style = -256S  ' Use to make dashed line.
    
    VIEW_PRINT(23, 24)  ' Scroll printed output in
                                    ' rows 23 and 24.
    Do
       PRINT(TAB(20), True)
       INPUT("Number of cycles (0 to end): ", Cycles, False)
       CLS()
       LINE(2 * PI, 0, 0, 0, -1, LineOption.None, Style)  ' Draw the x (horizontal) axis.
       If Cycles > 0 Then
          ' Start at (0,0) and plot the graph:
          For X! = 0 To CSng(2 * PI) Step .01!
             Y = CSng(Sin(Cycles * X))  ' Calculate the y coordinate.
             LINE(X, Y)  ' Draw a line from the last
                                    ' point to the new point.
          Next X
       End If
    Loop While Cycles > 0
    
  End Sub

End Module
