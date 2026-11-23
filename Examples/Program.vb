Imports QB.Video

Namespace Examples

  ''' <summary>
  ''' Runs the examples:
  '''   Examples                   lists them
  '''   Examples NAME              runs one in a window (press a key at the end to close it)
  '''   Examples --headless DIR [NAME ...]
  '''                              runs examples (all by default) without a window, simulating key
  '''                              presses, and saves the final screen of each as DIR/NAME.ppm
  ''' </summary>
  Friend Module Program

    Private Function Find(all As List(Of ExampleInfo), name As String) As ExampleInfo
      Return all.FirstOrDefault(Function(e) e.Names.Any(Function(n) String.Equals(n, name, StringComparison.OrdinalIgnoreCase)))
    End Function

    Public Sub Main(args As String())

      Dim all = Registry.All.OrderBy(Function(e) e.Name, StringComparer.OrdinalIgnoreCase).ToList()

      If args.Length = 0 OrElse args(0) = "--list" Then
        Dim width = all.Max(Function(e) String.Join(", ", e.Names).Length) + 2
        For Each e In all
          Console.WriteLine(String.Join(", ", e.Names).PadRight(width) & If(e.Extra, "(extra) ", "") & e.Source)
        Next
        Return
      End If

      If args(0) = "--headless" Then
        Headless(all, args)
        Return
      End If

      Dim entry = Find(all, args(0))
      If entry Is Nothing Then
        Console.WriteLine($"No example named {args(0)}.  Run with no arguments to list them.")
        Return
      End If

      Reset()
      entry.Run()
      SLEEP() ' Keep the window until a key is pressed.

    End Sub

    ' Programs start in SCREEN 0 with the default colors.
    Private Sub Reset()
      SCREEN(0)
      WIDTH(80, 25)
      VIEW_PRINT()
    End Sub

    Private Sub Headless(all As List(Of ExampleInfo), args As String())

      Dim dir = IO.Path.GetFullPath(If(args.Length > 1, args(1), "."))
      IO.Directory.CreateDirectory(dir)
      ' Examples that write files do it in a work folder.
      Dim work = IO.Path.Combine(dir, "work")
      IO.Directory.CreateDirectory(work)
      IO.Directory.SetCurrentDirectory(work)

      Dim display As New QB.HeadlessDisplay
      QB.Video.Display = display
      Dim audio As New QB.HeadlessAudio
      QB.Multimedia.Audio = audio

      Dim chosen As New List(Of ExampleInfo)
      If args.Length > 2 Then
        For i = 2 To args.Length - 1
          Dim e = Find(all, args(i))
          If e Is Nothing Then Console.WriteLine($"No example named {args(i)}.") Else chosen.Add(e)
        Next
      Else
        chosen.AddRange(all.Where(Function(e) Not e.InteractiveOnly))
      End If

      Dim failures = 0
      For Each e In chosen

        Reset()
        audio.Clear()

        ' Type the example's input, or else press a key (alternately a space and Esc) every quarter of a second.
        Dim n = 0
        Dim timer As Threading.Timer = Nothing
        If e.Input <> "" Then
          For Each ch In e.Input.Replace("\r", vbCr)
            display.EnqueueKey(ch.ToString)
          Next
        Else
          timer = New Threading.Timer(Sub()
                                        n += 1
                                        display.EnqueueKey(If(n Mod 2 = 1, " ", ChrW(27)))
                                      End Sub, Nothing, 250, 250)
        End If

        Dim status = "ok"
        Dim worker As New Threading.Thread(Sub()
                                             Try
                                               e.Run()
                                             Catch ex As Exception
                                               status = "FAILED: " & ex.Message
                                             End Try
                                           End Sub) With {.IsBackground = True}
        worker.Start()
        If Not worker.Join(6000) Then status = "TIMEOUT"
        timer?.Dispose()

        display.SavePpm(IO.Path.Combine(dir, e.Name & ".ppm"))
        Dim sound = If(audio.Tones.Count > 0, $" ({audio.Tones.Count} tones, {audio.TotalSeconds:0.0} s of sound)", "")
        Console.WriteLine($"{e.Name,-14}{status}{sound}")
        If status <> "ok" Then failures += 1
        If status = "TIMEOUT" Then Exit For ' The abandoned example would disturb the next one.

      Next

      Environment.Exit(If(failures = 0, 0, 1))

    End Sub

  End Module

End Namespace
