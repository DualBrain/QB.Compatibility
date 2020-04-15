Module Module1

  Sub Main()

    Nibbles.SubMain()
    'RemLineOrg.SubMain()
    'RemLineNew.SubMain()
    'CONSTsample.Main()
    'DEFtype.Main()

    If Debugger.IsAttached Then
      Console.WriteLine()
      Console.WriteLine()
      Console.WriteLine("Press any key to continue...")
      Console.ReadKey()
    End If

  End Sub

End Module
