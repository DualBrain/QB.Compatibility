' Converted from QBasic by qb2vb.

Module Program

  Const cyan As Short = 3
  Const red As Short = 4
  Dim w!
  Dim h!

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    
    SCREEN(13)
    w = 320 : h = 200
    ' CONST cyan = 3, red = 4
    
    rellenar((12))
    establecePixel(10, 10, cyan)
    LOCATE(12)
    PRINT("pixel 10,10 is " & Str$(POINT(10, 10)) & " ")
    PRINT("pixel 20,20 is " & Str$(POINT(20, 10)) & " ")
  End Sub

  Sub establecePixel(ByRef x As Short, ByRef y As Short, ByRef c As Short)
      PSET(x, y, CInt(cyan))
  End Sub
  
  
  Sub rellenar(ByRef c As Short)
      ' SHARED w, h   (shared variables are declared at module level)
      LINE(0, 0, w / 3, h / 3, CInt(red), LineOption.BF)
  End Sub
  
End Module
