Option Explicit On
Option Strict On
Option Infer On

Imports System.Collections.Concurrent
Imports System.Runtime.InteropServices
Imports SDL2

Namespace Global.QB

  ''' <summary>The default display: an SDL2 window that works on Windows, Linux and macOS.</summary>
  Friend NotInheritable Class SdlDisplay
    Implements IDisplay, IGameInput, IWindowDisplay, IGraphicsDisplay, IWindowTitle

    Private m_source As IVideoSource
    Private m_thread As Threading.Thread
    Private m_running As Boolean
    Private ReadOnly m_ready As New Threading.ManualResetEventSlim(False)
    Private m_startError As Exception
    Private ReadOnly m_keys As New ConcurrentQueue(Of String)
    Private m_newTitle As String

    ' the window's thread puts it on (SDL wants its windows touched from where they were made)
    Private Sub SetTitle(title As String) Implements IWindowTitle.SetTitle
      Threading.Volatile.Write(m_newTitle, title)
    End Sub

    ' the Q icon: a bold white Q on the blue of the editor
    Private Shared Sub SetWindowIcon(window As IntPtr)
      Dim size As Integer
      Dim pixels = WindowIcon.Pixels(size)
      Dim handle = GCHandle.Alloc(pixels, GCHandleType.Pinned)
      Try
        Dim surface = SDL.SDL_CreateRGBSurfaceFrom(handle.AddrOfPinnedObject(), size, size, 32, size * 4, &HFF0000UI, &HFF00UI, &HFFUI, &HFF000000UI)
        If surface <> IntPtr.Zero Then
          SDL.SDL_SetWindowIcon(window, surface)
          SDL.SDL_FreeSurface(surface)
        End If
      Finally
        handle.Free()
      End Try
    End Sub

    Public Sub Open(source As IVideoSource) Implements IDisplay.Open
      m_source = source
      m_running = True
      m_thread = New Threading.Thread(AddressOf Run) With {.IsBackground = True, .Name = "QB.Display"}
      m_thread.Start()
      m_ready.Wait()
      If m_startError IsNot Nothing Then
        Throw New InvalidOperationException("Unable to open the display: " & m_startError.Message, m_startError)
      End If
    End Sub

    Public Function ReadKey() As String Implements IDisplay.ReadKey
      Dim k As String = Nothing
      Return If(m_keys.TryDequeue(k), k, "")
    End Function

    Public Sub Close() Implements IDisplay.Close
      m_running = False
      m_thread?.Join(1000)
    End Sub

    Private Sub Run()

      Dim window As IntPtr = IntPtr.Zero
      Dim renderer As IntPtr = IntPtr.Zero
      Dim texture As IntPtr = IntPtr.Zero

      Try

        ' Let the OS screen saver (and display sleep) work normally; SDL disables it by default.
        SDL.SDL_SetHint(SDL.SDL_HINT_VIDEO_ALLOW_SCREENSAVER, "1")
        SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "0") ' Nearest-neighbor; keep pixels crisp.

        If SDL.SDL_Init(SDL.SDL_INIT_VIDEO) <> 0 Then
          Throw New InvalidOperationException(SDL.SDL_GetError())
        End If
        SDL.SDL_EnableScreenSaver()

        window = SDL.SDL_CreateWindow(Video.WindowTitle,
                                      SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                                      800, 600,
                                      SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN Or SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE)
        If window = IntPtr.Zero Then Throw New InvalidOperationException(SDL.SDL_GetError())
        SetWindowIcon(window)

        renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED)
        If renderer = IntPtr.Zero Then
          renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_SOFTWARE)
        End If
        If renderer = IntPtr.Zero Then Throw New InvalidOperationException(SDL.SDL_GetError())

        SDL.SDL_StartTextInput()

        ' Joysticks (their "added" events open them).  Failing to start them doesn't matter.
        SDL.SDL_SetHint(SDL.SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS, "1")
        SDL.SDL_InitSubSystem(SDL.SDL_INIT_JOYSTICK)

      Catch ex As Exception
        m_startError = ex
        m_ready.Set()
        Return
      End Try

      m_ready.Set()

      Dim argb() As UInteger = Nothing
      Dim texW, texH As Integer
      Dim redraw = True

      Do While m_running

        Dim e As SDL.SDL_Event
        Do While SDL.SDL_PollEvent(e) = 1
          Select Case e.type
            Case SDL.SDL_EventType.SDL_QUIT
              m_running = False
              Video.WindowClosed()
              Environment.Exit(0) ' Closing the window ends the program, as it would in QBasic.
            Case SDL.SDL_EventType.SDL_WINDOWEVENT
              redraw = True
              If e.window.windowEvent = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE Then
                SyncLock m_inputLock
                  m_pointer.LeaveX = m_pointer.X : m_pointer.LeaveY = m_pointer.Y
                End SyncLock
              End If
            Case SDL.SDL_EventType.SDL_JOYDEVICEADDED, SDL.SDL_EventType.SDL_JOYDEVICEREMOVED,
                 SDL.SDL_EventType.SDL_JOYAXISMOTION, SDL.SDL_EventType.SDL_JOYBUTTONDOWN, SDL.SDL_EventType.SDL_JOYBUTTONUP,
                 SDL.SDL_EventType.SDL_MOUSEMOTION, SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN, SDL.SDL_EventType.SDL_MOUSEBUTTONUP
              HandleInputEvent(e)
            Case SDL.SDL_EventType.SDL_KEYDOWN
              HandleKeyDown(e.key.keysym)
            Case SDL.SDL_EventType.SDL_TEXTINPUT
              HandleText(e)
          End Select
        Loop

        Dim title = Threading.Interlocked.Exchange(m_newTitle, Nothing)
        If title IsNot Nothing Then SDL.SDL_SetWindowTitle(window, title)

        Dim w, h As Integer
        If m_source.TryCopyFrame(argb, w, h) Then
          If texture = IntPtr.Zero OrElse w <> texW OrElse h <> texH Then
            If texture <> IntPtr.Zero Then SDL.SDL_DestroyTexture(texture)
            texture = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ARGB8888,
                                            CInt(SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING), w, h)
            texW = w : texH = h
          End If
          Dim handle = GCHandle.Alloc(argb, GCHandleType.Pinned)
          Try
            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, handle.AddrOfPinnedObject(), w * 4)
          Finally
            handle.Free()
          End Try
          redraw = True
        End If

        If redraw AndAlso texture <> IntPtr.Zero Then
          Dim ww, wh As Integer
          SDL.SDL_GetWindowSize(window, ww, wh)
          ' Always show the screen as 4:3 (as a monitor would), centered in the window.
          Dim dest As SDL.SDL_Rect
          If wh * 4 > ww * 3 Then
            dest.w = ww : dest.h = ww * 3 \ 4
          Else
            dest.h = wh : dest.w = wh * 4 \ 3
          End If
          dest.x = (ww - dest.w) \ 2
          dest.y = (wh - dest.h) \ 2
          SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255)
          SDL.SDL_RenderClear(renderer)
          SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, dest)
          SyncLock m_inputLock
            m_destX = dest.x : m_destY = dest.y : m_destW = dest.w : m_destH = dest.h
            m_texW = texW : m_texH = texH
          End SyncLock
          SDL.SDL_RenderPresent(renderer)
          redraw = False
        End If

        SDL.SDL_Delay(8)

      Loop

      If texture <> IntPtr.Zero Then SDL.SDL_DestroyTexture(texture)
      For i = 0 To 1
        If m_sticks(i) <> IntPtr.Zero Then SDL.SDL_JoystickClose(m_sticks(i))
      Next
      SDL.SDL_DestroyRenderer(renderer)
      SDL.SDL_DestroyWindow(window)
      SDL.SDL_Quit()

    End Sub

    ' ---- Joysticks and the mouse (IGameInput) ----------------------------------------------------

    Private ReadOnly m_axes(1, 1) As Integer
    Private ReadOnly m_buttons(1, 1) As Boolean
    Private ReadOnly m_presses(1, 1) As Integer
    Private ReadOnly m_sticks(1) As IntPtr
    Private ReadOnly m_stickIds() As Integer = {-1, -1}
    Private ReadOnly m_inputLock As New Object
    Private m_pointer As PointerState
    Private m_destX, m_destY, m_destW, m_destH As Integer
    Private m_texW, m_texH As Integer

    Private Function StickAxis(stick As Integer, axis As Integer) As Integer Implements IGameInput.StickAxis
      Return Threading.Volatile.Read(m_axes(stick, axis))
    End Function

    Private Function StickButtonDown(stick As Integer, button As Integer) As Boolean Implements IGameInput.StickButtonDown
      Return m_buttons(stick, button)
    End Function

    Private Function StickButtonPresses(stick As Integer, button As Integer) As Integer Implements IGameInput.StickButtonPresses
      Return Threading.Volatile.Read(m_presses(stick, button))
    End Function

    Private Function Pointer() As PointerState Implements IGameInput.Pointer
      SyncLock m_inputLock
        Return m_pointer
      End SyncLock
    End Function

    Private Function SlotOf(instanceId As Integer) As Integer
      For i = 0 To 1
        If m_stickIds(i) = instanceId Then Return i
      Next
      Return -1
    End Function

    Private Sub OpenStick(deviceIndex As Integer)
      Dim joystick = SDL.SDL_JoystickOpen(deviceIndex)
      If joystick = IntPtr.Zero Then Return
      Dim id = SDL.SDL_JoystickInstanceID(joystick)
      If SlotOf(id) >= 0 Then
        SDL.SDL_JoystickClose(joystick) ' Already open.
        Return
      End If
      For i = 0 To 1
        If m_stickIds(i) = -1 Then
          m_sticks(i) = joystick : m_stickIds(i) = id
          Return
        End If
      Next
      SDL.SDL_JoystickClose(joystick) ' Only joysticks A and B are used.
    End Sub

    Private Sub CloseStick(instanceId As Integer)
      Dim slot = SlotOf(instanceId)
      If slot < 0 Then Return
      SDL.SDL_JoystickClose(m_sticks(slot))
      m_sticks(slot) = IntPtr.Zero : m_stickIds(slot) = -1
      m_axes(slot, 0) = 0 : m_axes(slot, 1) = 0
      m_buttons(slot, 0) = False : m_buttons(slot, 1) = False
    End Sub

    ' Window position to screen pixels (the screen is drawn 4:3 and centered in the window).
    Private Sub ToScreen(wx As Integer, wy As Integer, ByRef sx As Integer, ByRef sy As Integer)
      sx = 0 : sy = 0
      If m_destW <= 0 OrElse m_destH <= 0 OrElse m_texW <= 0 Then Return
      sx = Math.Max(0, Math.Min(m_texW - 1, CInt((wx - m_destX) * CLng(m_texW) \ m_destW)))
      sy = Math.Max(0, Math.Min(m_texH - 1, CInt((wy - m_destY) * CLng(m_texH) \ m_destH)))
    End Sub

    Private Sub HandleInputEvent(ByRef e As SDL.SDL_Event)
      Select Case e.type
        Case SDL.SDL_EventType.SDL_JOYDEVICEADDED
          OpenStick(e.jdevice.which)
        Case SDL.SDL_EventType.SDL_JOYDEVICEREMOVED
          CloseStick(e.jdevice.which)
        Case SDL.SDL_EventType.SDL_JOYAXISMOTION
          Dim slot = SlotOf(e.jaxis.which)
          If slot >= 0 AndAlso e.jaxis.axis < 2 Then Threading.Volatile.Write(m_axes(slot, e.jaxis.axis), CInt(e.jaxis.axisValue))
        Case SDL.SDL_EventType.SDL_JOYBUTTONDOWN, SDL.SDL_EventType.SDL_JOYBUTTONUP
          Dim slot = SlotOf(e.jbutton.which)
          If slot >= 0 AndAlso e.jbutton.button < 2 Then
            Dim down = (e.type = SDL.SDL_EventType.SDL_JOYBUTTONDOWN)
            m_buttons(slot, e.jbutton.button) = down
            If down Then Threading.Interlocked.Increment(m_presses(slot, e.jbutton.button))
          End If
        Case SDL.SDL_EventType.SDL_MOUSEMOTION
          SyncLock m_inputLock
            ToScreen(e.motion.x, e.motion.y, m_pointer.X, m_pointer.Y)
          End SyncLock
        Case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN, SDL.SDL_EventType.SDL_MOUSEBUTTONUP
          If e.button.button = CByte(SDL.SDL_BUTTON_LEFT) Then
            SyncLock m_inputLock
              ToScreen(e.button.x, e.button.y, m_pointer.X, m_pointer.Y)
              If e.type = SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN Then
                m_pointer.Down = True
                m_pointer.Presses += 1
                m_pointer.PressX = m_pointer.X : m_pointer.PressY = m_pointer.Y
              Else
                m_pointer.Down = False
              End If
            End SyncLock
          End If
      End Select
    End Sub

    Private m_suppressTextUntil As Long   ' Alt+key combinations also produce text input; it is ignored.

    Private Sub HandleKeyDown(k As SDL.SDL_Keysym)

      Dim shift = (k.mod And SDL.SDL_Keymod.KMOD_SHIFT) <> 0
      Dim ctrl = (k.mod And SDL.SDL_Keymod.KMOD_CTRL) <> 0
      Dim alt = (k.mod And SDL.SDL_Keymod.KMOD_ALT) <> 0

      Dim key As ConsoleKey = 0
      Dim ch = ChrW(0)

      Select Case k.sym
        Case SDL.SDL_Keycode.SDLK_RETURN, SDL.SDL_Keycode.SDLK_KP_ENTER : key = ConsoleKey.Enter : ch = ChrW(13)
        Case SDL.SDL_Keycode.SDLK_BACKSPACE : key = ConsoleKey.Backspace : ch = ChrW(8)
        Case SDL.SDL_Keycode.SDLK_TAB : key = ConsoleKey.Tab : ch = ChrW(9)
        Case SDL.SDL_Keycode.SDLK_ESCAPE : key = ConsoleKey.Escape : ch = ChrW(27)
        Case SDL.SDL_Keycode.SDLK_DELETE : key = ConsoleKey.Delete
        Case SDL.SDL_Keycode.SDLK_INSERT : key = ConsoleKey.Insert
        Case SDL.SDL_Keycode.SDLK_HOME : key = ConsoleKey.Home
        Case SDL.SDL_Keycode.SDLK_END : key = ConsoleKey.End
        Case SDL.SDL_Keycode.SDLK_PAGEUP : key = ConsoleKey.PageUp
        Case SDL.SDL_Keycode.SDLK_PAGEDOWN : key = ConsoleKey.PageDown
        Case SDL.SDL_Keycode.SDLK_UP : key = ConsoleKey.UpArrow
        Case SDL.SDL_Keycode.SDLK_DOWN : key = ConsoleKey.DownArrow
        Case SDL.SDL_Keycode.SDLK_LEFT : key = ConsoleKey.LeftArrow
        Case SDL.SDL_Keycode.SDLK_RIGHT : key = ConsoleKey.RightArrow
        Case SDL.SDL_Keycode.SDLK_F1 To SDL.SDL_Keycode.SDLK_F12
          key = CType(CInt(ConsoleKey.F1) + (CInt(k.sym) - CInt(SDL.SDL_Keycode.SDLK_F1)), ConsoleKey)
        Case SDL.SDL_Keycode.SDLK_a To SDL.SDL_Keycode.SDLK_z
          If alt AndAlso Not ctrl Then
            ' Alt+letter is Chr(0) and the letter's scan code.
            m_suppressTextUntil = Environment.TickCount64 + 80
            m_keys.Enqueue(KeyCodec.Extended(KeyCodec.LetterScan(ChrW(CInt(k.sym) - 32))))
            Return
          End If
          ' Ctrl+letter gives the control character; plain letters arrive as text input.
          If ctrl Then ch = ChrW(CInt(k.sym) - CInt(SDL.SDL_Keycode.SDLK_a) + 1)
        Case SDL.SDL_Keycode.SDLK_0 To SDL.SDL_Keycode.SDLK_9
          If alt Then ' Alt+1 to Alt+9 and Alt+0.
            m_suppressTextUntil = Environment.TickCount64 + 80
            m_keys.Enqueue(KeyCodec.Extended(If(k.sym = SDL.SDL_Keycode.SDLK_0, 129, 119 + CInt(k.sym) - CInt(SDL.SDL_Keycode.SDLK_0))))
          End If
          Return
        Case SDL.SDL_Keycode.SDLK_KP_1 To SDL.SDL_Keycode.SDLK_KP_9, SDL.SDL_Keycode.SDLK_KP_0, SDL.SDL_Keycode.SDLK_KP_PERIOD
          ' With NumLock on the keypad types digits (text input); with it off the keys are cursor keys.
          If (k.mod And SDL.SDL_Keymod.KMOD_NUM) <> 0 Then Return
          Select Case k.sym
            Case SDL.SDL_Keycode.SDLK_KP_8 : key = ConsoleKey.UpArrow
            Case SDL.SDL_Keycode.SDLK_KP_2 : key = ConsoleKey.DownArrow
            Case SDL.SDL_Keycode.SDLK_KP_4 : key = ConsoleKey.LeftArrow
            Case SDL.SDL_Keycode.SDLK_KP_6 : key = ConsoleKey.RightArrow
            Case SDL.SDL_Keycode.SDLK_KP_7 : key = ConsoleKey.Home
            Case SDL.SDL_Keycode.SDLK_KP_1 : key = ConsoleKey.End
            Case SDL.SDL_Keycode.SDLK_KP_9 : key = ConsoleKey.PageUp
            Case SDL.SDL_Keycode.SDLK_KP_3 : key = ConsoleKey.PageDown
            Case SDL.SDL_Keycode.SDLK_KP_0 : key = ConsoleKey.Insert
            Case SDL.SDL_Keycode.SDLK_KP_PERIOD : key = ConsoleKey.Delete
            Case Else : Return ' (KP_5 has no key.)
          End Select
        Case Else
          Return
      End Select

      Dim s = KeyCodec.Encode(key, ch, shift, ctrl, alt)
      If s.Length > 0 Then m_keys.Enqueue(s)

    End Sub

    Private Sub HandleText(e As SDL.SDL_Event)
      If Environment.TickCount64 < m_suppressTextUntil Then Return
      ' The text lives in a fixed buffer SDL2-CS exposes awkwardly; read it from the raw event.
      Dim size = Marshal.SizeOf(Of SDL.SDL_Event)()
      Dim p = Marshal.AllocHGlobal(size)
      Try
        Marshal.StructureToPtr(e, p, False)
        Dim text = Marshal.PtrToStringUTF8(p + 12)
        If text IsNot Nothing Then
          For Each c In text
            If AscW(c) >= 32 AndAlso AscW(c) < 256 Then m_keys.Enqueue(c.ToString)
          Next
        End If
      Finally
        Marshal.FreeHGlobal(p)
      End Try
    End Sub

  End Class

End Namespace
