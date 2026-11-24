' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    'Example: Find color of a screen pixel in QBasic (adapted from QBasic Help file).
    '  POINT(x%, y%)  returns color of pixel at coordinates x,y.
    SCREEN(7)  'Choose color graphics screen (1,2,4,7,8,9,11,12,13).
    LINE(0, 0, 100, 100, 2)  'Draw diagonal line in color attribute 2.
    LOCATE(14, 1)  'Locate below diagonal line to show output.
    For y = 1S To 10S
      For x = 1S To 10S
        PRINT(Str$(POINT(x, y)) & " ", True)  'POINT(x%, y%) displays pixel color.
      Next x
      PRINT()
    Next y
    End
  End Sub

End Module
