' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    DATA("tochka", "liniya", "dyuim", "vershok", "piad", "fut", "arshin", "sazhen", "versta", "milia", "centimeter", "meter", "kilometer")
    DATA(0.0254, 0.254, 2.54, 4.445, 17.78, 30.48, 71.12, 213.36, 106680, 746760, 1, 100, 100000)
    
    Dim units(13) As String
    For i! = 1 To UBound(units)
    READ(units(CInt(i)))
    Next i
    ' DATA "tochka", "liniya", "dyuim", "vershok", "piad", "fut", "arshin", "sazhen", "versta", "milia", "centimeter", "meter", "kilometer"   (DATA is listed at the start of the program)
    
    ' all expressed in centimeters
    Dim convs(13) As Single
    For i = 1! To 13!
    READ(convs(CInt(i)))
    Next i
    ' DATA 0.0254, 0.254, 2.54, 4.445, 17.78, 30.48, 71.12, 213.36, 106680, 746760, 1, 100, 100000   (DATA is listed at the start of the program)
    Dim unit As Short
    Dim value As Single
    Dim yn As String
    
    Do
        CLS()
        PRINT()
        For i = 1! To 13!
            PRINT_USING_NOCR("## ", i)
            PRINT(units(CInt(i)))
        Next i
        PRINT()
        Do
            INPUT("Please choose a unit 1 to 13 : ", unit, False)
        Loop Until (unit >= 1) And (unit <= 13)
        PRINT()
        Do
            INPUT("Now enter a value in that unit : ", value, False)
        Loop Until value >= 0
        PRINT()
        PRINT("The equivalent in the remaining units is : ")
        PRINT()
        For i = 1! To 13!
            If i <> unit Then PRINT(" " & units(CInt(i)) & vbTab & " : " & Str$((value * convs(unit)) / convs(CInt(i))) & " ")
        Next i
        PRINT()
        Do
            INPUT("Do another one y/n : ", yn, False)
            yn = LCase$(yn)
        Loop Until (yn = "y") Or (yn = "n")
    Loop Until yn = "n"
  End Sub

End Module
