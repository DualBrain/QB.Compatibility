Namespace Examples

  ''' <summary>An example that can be run.</summary>
  Friend NotInheritable Class ExampleInfo

    ''' <summary>The statement(s) the example demonstrates; the first is the name used to run it.</summary>
    Public ReadOnly Names() As String
    ''' <summary>Where the QBasic original is documented.</summary>
    Public ReadOnly Source As String
    Public ReadOnly Run As Action
    ''' <summary>Keys the headless run types in (with \r for Enter) instead of pressing keys now and then.</summary>
    Public Property Input As String = ""
    ''' <summary>True when the example isn't from the QBasic documentation.</summary>
    Public Property Extra As Boolean
    ''' <summary>True when the example never ends by itself (it is skipped by the headless run).</summary>
    Public Property InteractiveOnly As Boolean

    Public Sub New(names() As String, source As String, run As Action)
      Me.Names = names
      Me.Source = source
      Me.Run = run
    End Sub

    Public ReadOnly Property Name As String
      Get
        Return Names(0)
      End Get
    End Property

  End Class

  ''' <summary>
  ''' All the examples.  (A list rather than reflection, so the program can be trimmed and compiled with Native AOT.)
  ''' Add a line here when adding a file to Statements.
  ''' </summary>
  Friend Module Registry

    Friend ReadOnly All As New List(Of ExampleInfo) From {
      New ExampleInfo({"BEEP"}, "https://qbasic.net/en/qb-manual/Statement/BEEP.htm", AddressOf BeepExample.Run) With {.Input = "y\rn\r"},
      New ExampleInfo({"CIRCLE"}, "https://qbasic.net/en/qb-manual/Statement/CIRCLE.htm", AddressOf CircleExample.Run),
      New ExampleInfo({"CLOSE"}, "https://qbasic.net/en/qb-manual/Statement/CLOSE.htm", AddressOf CloseExample.Run) With {.Input = "TEST.TXT\r"},
      New ExampleInfo({"CLS"}, "https://qbasic.net/en/qb-manual/Statement/CLS.htm", AddressOf ClsExample.Run),
      New ExampleInfo({"COLOR"}, "https://qbasic.net/en/qb-manual/Statement/COLOR.htm", AddressOf ColorExample.Run),
      New ExampleInfo({"DRAW"}, "https://qbasic.net/en/qb-manual/Statement/DRAW.htm", AddressOf DrawExample.Run),
      New ExampleInfo({"EOF"}, "https://qbasic.net/en/qb-manual/Function/EOF.htm", AddressOf EofExample.Run),
      New ExampleInfo({"FILES"}, "https://qbasic.net/en/qb-manual/Statement/FILES.htm", AddressOf FilesExample.Run),
      New ExampleInfo({"GET_FILE", "PUT_FILE"}, "https://qbasic.net/en/qb-manual/Statement%20%28File%20I%C2%A7O%29/GET.htm", AddressOf GetPutFileExample.Run),
      New ExampleInfo({"GET", "PUT"}, "https://qbasic.net/en/qb-manual/Statement%20%28Graphics%29/PUT.htm", AddressOf GetPutExample.Run),
      New ExampleInfo({"INKEY$", "INKEY"}, "https://qbasic.net/en/qb-manual/Function/INKEY%24.htm", AddressOf InkeyExample.Run),
      New ExampleInfo({"INPUT", "LINE_INPUT"}, "https://qbasic.net/en/qb-manual/Statement/INPUT.htm", AddressOf InputExample.Run) With {.Input = "Bob\r30\ry\rAnn\r41\rn\r"},
      New ExampleInfo({"INPUT$", "INPUT_FUNCTION"}, "https://qbasic.net/en/qb-manual/Function/INPUT%24.htm", AddressOf InputFunctionExample.Run),
      New ExampleInfo({"KILL"}, "https://qbasic.net/en/qb-manual/Statement/KILL.htm", AddressOf KillExample.Run) With {.Input = "DELETEME.TMP\r"},
      New ExampleInfo({"LOCATE", "CSRLIN"}, "https://qbasic.net/en/qb-manual/Statement/LOCATE.htm", AddressOf LocateExample.Run),
      New ExampleInfo({"LOF"}, "https://qbasic.net/en/qb-manual/Function/LOF.htm", AddressOf LofExample.Run) With {.Input = "TEST.DAT\r"},
      New ExampleInfo({"ON_KEY", "KEY_ON"}, "https://qbasic.net/en/qb-manual/Statement/ON%20KEY.htm", AddressOf OnKeyExample.Run),
      New ExampleInfo({"ON_PLAY", "PLAY_ON"}, "https://qbasic.net/en/qb-manual/Statement/ON%20PLAY.htm", AddressOf OnPlayExample.Run),
      New ExampleInfo({"ON_TIMER", "TIMER_ON"}, "https://qbasic.net/en/qb-manual/Statement/ON%20TIMER.htm", AddressOf OnTimerExample.Run),
      New ExampleInfo({"STICK"}, "https://qbasic.net/en/qb-manual/Function/STICK.htm", AddressOf StickExample.Run),
      New ExampleInfo({"STRIG"}, "https://qbasic.net/en/qb-manual/Function/STRIG.htm", AddressOf StrigFunctionExample.Run),
      New ExampleInfo({"PEN"}, "https://qbasic.net/en/qb-manual/Function/PEN.htm", AddressOf PenFunctionExample.Run) With {.InteractiveOnly = True},
      New ExampleInfo({"ON_STRIG", "STRIG_ON"}, "https://qbasic.net/en/qb-manual/Statement/ON%20STRIG.htm", AddressOf OnStrigExample.Run),
      New ExampleInfo({"ON_PEN", "PEN_ON"}, "https://qbasic.net/en/qb-manual/Statement/ON%20PEN.htm", AddressOf OnPenExample.Run),
      New ExampleInfo({"OPEN_COM"}, "https://qbasic.net/en/qb-manual/Statement/OPEN%20COM.htm", AddressOf OpenComExample.Run) With {.InteractiveOnly = True},
      New ExampleInfo({"PAINT"}, "https://qbasic.net/en/qb-manual/Statement/PAINT.htm", AddressOf PaintExample.Run),
      New ExampleInfo({"PAINT_TILE"}, "", AddressOf PaintTileExample.Run) With {.Extra = True},
      New ExampleInfo({"PALETTE", "PALETTE_USING"}, "https://qbasic.net/en/qb-manual/Statement/PALETTE.htm", AddressOf PaletteExample.Run),
      New ExampleInfo({"PCOPY"}, "https://qbasic.net/en/qb-manual/Statement/PCOPY.htm", AddressOf PcopyExample.Run),
      New ExampleInfo({"PLAY"}, "https://qbasic.net/en/qb-manual/Statement%20%28Music%29/PLAY.htm", AddressOf PlayExample.Run),
      New ExampleInfo({"PLAY_FUNCTION"}, "https://qbasic.net/en/qb-manual/Function/PLAY.htm", AddressOf PlayFunctionExample.Run),
      New ExampleInfo({"PMAP"}, "https://qbasic.net/en/qb-manual/Function/PMAP.htm", AddressOf PmapExample.Run),
      New ExampleInfo({"POINT"}, "https://qbasic.net/en/qb-manual/Function/POINT.htm", AddressOf PointExample.Run),
      New ExampleInfo({"POS"}, "https://qbasic.net/en/qb-manual/Function/POS.htm", AddressOf PosExample.Run) With {.InteractiveOnly = True},
      New ExampleInfo({"PRINT_FILE"}, "https://qbasic.net/en/qb-manual/Statement/PRINT%20%23.htm", AddressOf PrintFileExample.Run),
      New ExampleInfo({"PRINT_USING", "USING"}, "https://qbasic.net/en/qb-manual/Statement/PRINT%20USING.htm", AddressOf PrintUsingExample.Run),
      New ExampleInfo({"PSET", "PRESET"}, "https://qbasic.net/en/qb-manual/Statement/PSET.htm", AddressOf PsetExample.Run),
      New ExampleInfo({"PUT_PRESET"}, "", AddressOf PutPresetExample.Run) With {.Extra = True},
      New ExampleInfo({"SCREEN", "LINE"}, "https://qbasic.net/en/qb-manual/Statement/SCREEN.htm", AddressOf ScreenExample.Run),
      New ExampleInfo({"SCREEN_FUNCTION"}, "https://qbasic.net/en/qb-manual/Function/SCREEN.htm", AddressOf ScreenFunctionExample.Run),
      New ExampleInfo({"SEEK", "LOC"}, "https://qbasic.net/en/qb-manual/Function/SEEK.htm", AddressOf SeekExample.Run),
      New ExampleInfo({"SLEEP"}, "https://qbasic.net/en/qb-manual/Statement/SLEEP.htm", AddressOf SleepExample.Run),
      New ExampleInfo({"SOUND"}, "https://qbasic.net/en/qb-manual/Statement/SOUND.htm", AddressOf SoundExample.Run),
      New ExampleInfo({"SPC"}, "https://qbasic.net/en/qb-manual/Function/SPC.htm", AddressOf SpcExample.Run),
      New ExampleInfo({"SWAP"}, "https://qbasic.net/en/qb-manual/Statement/SWAP.htm", AddressOf SwapExample.Run),
      New ExampleInfo({"TAB"}, "https://qbasic.net/en/qb-manual/Function/TAB.htm", AddressOf TabExample.Run),
      New ExampleInfo({"TREE"}, "", AddressOf TreeExample.Run) With {.Extra = True},
      New ExampleInfo({"VIEW"}, "https://qbasic.net/en/qb-manual/Statement/VIEW.htm", AddressOf ViewExample.Run),
      New ExampleInfo({"VIEW_PRINT"}, "https://qbasic.net/en/qb-manual/Statement/VIEW%20PRINT.htm", AddressOf ViewPrintExample.Run),
      New ExampleInfo({"WINDOW"}, "https://qbasic.net/en/qb-manual/Statement/WINDOW.htm", AddressOf WindowExample.Run),
      New ExampleInfo({"WRITE"}, "https://qbasic.net/en/qb-manual/Statement/WRITE%20%23.htm", AddressOf WriteExample.Run) With {.Input = "Bob\r30\ry\rAnn\r41\rn\r"}
    }

  End Module

End Namespace
