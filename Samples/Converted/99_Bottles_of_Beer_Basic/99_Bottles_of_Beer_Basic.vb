' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim suffix$ = "s"
    For bottles = 4! To 1! Step -1!
        PRINT((((Str$(bottles) + " bottle") + suffix$) + " of beer on the wall, "), True)
        PRINT((((Str$(bottles) + " bottle") + suffix$) + " of beer."))
        PRINT("Take one down and pass it around, ", True)
        If bottles > 1 Then
            If bottles = 2 Then suffix$ = ""
            PRINT((((Str$(bottles - 1) + " bottle") + suffix$) + " of beer on the wall..."))
        Else
            PRINT("no more bottles of beer on the wall...")
        End If
        PRINT()
    Next bottles
    
    PRINT("No more bottles of beer on the wall, no more bottles of beer.")
    PRINT("Go to the store and buy some more, 99 bottles of beer on the wall.")
    End
  End Sub

End Module
