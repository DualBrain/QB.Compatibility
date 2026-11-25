# QB.Compatibility

QuickBASIC 4.5 and QBasic Compatibility Library for Visual Basic (.NET).

This is a (seriously) work-in-progress project that serves to create a compatibility layer for the purpose of migrating QB code to .NET.  It is not lost upon me that there isn't an overwhelming need for such a thing; however, from a mental excercise point of view this project also serves as a way to revisit a clear connection with Visual Basic history to review what we might have lost (and thus what might be gained) along the way.  Additionally, there is a certain *style* of development that QB provided that might be able to be recaptured by having access to some of this *old stuff*.  Finally, this project will (eventually, most likely) be used as part of another pet project that I'd like to do...

If you have QB projects that you'd like to share, I'm always on the lookout for more samples.

Examples of projects that have been converted using the library:

- REMLINE.BAS
- NIBBLES.BAS
- GORILLA.BAS

## Getting started

The library is a set of NuGet packages, released together with the same version:

| Package | For |
|---|---|
| `QB.Compatibility` | every program: text, files, errors, events, sound |
| `QB.Compatibility.Sdl` | programs that draw (`SCREEN` 1 and up), or use the joystick or the pen; brings SDL2 |
| `QB.Compatibility.Ports` | programs that open a serial port |
| `qb2vb` | the tool that converts QBasic source to VB (not a library; see below) |

```bash
dotnet add package QB.Compatibility
dotnet add package QB.Compatibility.Sdl     # only if the program draws
dotnet tool install --global qb2vb
```

## Platforms and what a program needs

QB.Compatibility targets .NET 10 and runs on Windows, Linux and macOS (no WinForms).  What a program needs follows from what it uses:

| The program uses | Import | Reference | Shown in |
|---|---|---|---|
| text only: `PRINT`, `INPUT`, `LOCATE`, `COLOR`, `CLS`, files, errors, `BEEP` | `QB.Video` | `QB.Compatibility` | the terminal |
| graphics (`SCREEN` 1 and up, `PSET`, `LINE`, `CIRCLE`, `PAINT`, `GET`/`PUT`, `DRAW`, `PALETTE`, `VIEW`, `WINDOW`), the joystick or the pen | `QB.Graphics` (instead of `QB.Video`) | plus `QB.Compatibility.Sdl` | an SDL2 window |
| `SOUND`, `PLAY`, `BEEP` | `QB.Multimedia` | `QB.Compatibility` | the system's audio |
| a serial port (`OPEN "COM1:..."`, `ON COM`) | `QB.Serial` (instead of `QB.File`) | plus `QB.Compatibility.Ports` | the port |

Nothing has to be set up: using a graphics statement installs the window the first time it runs, and `SOUND` or `PLAY` opens the system's audio.  A program that doesn't draw doesn't need `QB.Compatibility.Sdl` or SDL2 at all (a Native AOT text program is about 3.5 MB, with no SDL2 file; one that draws adds SDL2's 9 MB).  `qb2vb --project` chooses the imports and writes a project that references only the packages the program needs.

**The terminal.**  Text programs run in the terminal by default.  A program that only prints and reads lines scrolls like any command-line tool; if the output is redirected it stays plain text (no escape sequences).  The first time a program uses the screen by position (`LOCATE`, `VIEW PRINT`, `WIDTH`, the `SCREEN` function) the terminal switches to a full-screen view of the 80 x 25 text screen (the terminal's alternate screen), redrawn when the text changes, and the last picture is left in the terminal when the program ends.  A terminal smaller than the screen is asked to resize (many honor it); otherwise the rows around the cursor are shown.  On Windows virtual terminal processing is switched on for the console.  Keys come from `Console.ReadKey`, or from the input when it is redirected (reading past the end is QBasic's "Input past end of file").  `BEEP` rings the bell when there is no audio device.

**The window.**  Graphics draw into an in-memory screen of palette indices (like VGA memory) that is shown in an SDL2 window.  The package brings SDL2 for Windows and Linux; on Linux a system SDL2 can be used instead (Debian/Ubuntu: `sudo apt install libsdl2-2.0-0`).  The window keeps the screen at 4:3 and leaves the OS screen saver enabled. It is titled with the program's name and carries a QB icon; `Video.WindowTitle = "Gorillas"` (or `Graphics.WindowTitle`) renames it, at any time.  `QB.SdlBackend.Use()` makes a text program use the window too.

A different display can be plugged in through `QB.IDisplay` (`Video.Display = ...`); `HeadlessDisplay` is provided for tests.  See the `Examples` project for small programs.

## Random-access files

`OPEN(file, OpenMode.Random, n, LEN(record))`, then `PUT(n, record#, value)` and `[GET](n, record#, value)` (the record number may be left out: `PUT(n, value)`); `OpenMode.Binary` files use byte positions.  `SEEK`, `LOC` and `LOF` follow the file mode.  A value can be a number (a QBasic `INTEGER` is a VB `Short`, a `LONG` an `Integer`), a string (as it is), or a record (a QBasic `TYPE`).  A record is a Structure that implements `IFileRecord` (`WriteTo` and `ReadFrom`, using `RecordWriter.WriteString(value, length)` for fixed-length strings); this works everywhere, including AOT.  A Structure that doesn't implement it is read and written by reflection (strings need `<VBFixedString(n)>`), which doesn't work in a Native AOT build.

## Native AOT

The library and the examples can be compiled ahead of time, for example (`gcc` can stand in for `clang`; Windows needs the Visual Studio C++ tools):

```bash
dotnet publish Examples -c Release -r linux-x64 -p:PublishAot=true -p:CppCompilerAndLinker=gcc
```

The window, sound, files and all the examples work in that build.  Things that need reflection (records without `IFileRecord`) don't, and the examples are listed in `Examples/Registry.vb` rather than found by reflection for that reason: add a line there when adding an example.

## Events, joysticks, the pen and serial ports

**Events** (`QB.Events`): `ON_TIMER(5, AddressOf Tick) : TIMER_ON()`, `ON_KEY(n, AddressOf H) : KEY_ON(n)` (`KEY_DEFINE(15, &H4, &H1F)` for user keys),
`ON_PLAY(n, ...) : PLAY_ON()`, `ON_STRIG(n, ...) : STRIG_ON(n)`, `ON_PEN(...) : PEN_ON()` and `ON_COM(n, ...) : COM_ON(n)`, each with `_OFF` and `_STOP`.
A handler is a `Sub` with no arguments.  Handlers never interrupt the program from another thread: the event is noted when it happens and the handler runs, on
the program's own thread, the next time the program calls into the library (drawing, `INKEY`, `SLEEP`, `PLAY`, ...) or `Events.Poll()`.  A loop that calls
nothing from the library has to call `Poll()`.  While a handler runs its event is stopped, as in QBasic.  (`ON ERROR` is VB's own.)

**Joysticks and the pen** (`QB.Devices`): `STICK(n)` (0 to 200, 100 is the center), `STRIG(n)` and `PEN(n)` read the first two joysticks SDL finds (A and B; button 0
is the lower trigger, 1 the upper) and the mouse, which is the light pen (left button = pen switch; coordinates are screen pixels of the current mode).

**Serial ports** (`Imports QB.Serial` instead of `QB.File`, which needs the `QB.Compatibility.Ports` assembly; the other programs don't carry `System.IO.Ports`): `OPEN("COM1:9600,N,8,1", OpenMode.Random, 1)` opens a port (`System.IO.Ports`; COM1 is `/dev/ttyS0` off Windows, and a device path such as
`/dev/ttyUSB0:9600,N,8,1` works too).  `PRINT #`, `INPUT #`, `LINE INPUT #`, `INPUT$`, `EOF` (nothing waiting), `LOC` (bytes waiting), `LOF` (free transmit
space) and `CLOSE` work on it.  The handshake and time-out options are accepted and ignored.

## Printer

`LPRINT`, `LPRINT_USING`, `LPOS` and `WIDTH_LPRINT` (in `QB.Printer`), and files opened with `OPEN("LPT1:", OpenMode.Output, 1)` (`PRINT #`, `WRITE #`, `WIDTH_FILE(1, 132)`).
Print positions follow QBasic: a Tab character is the comma's 14-column print zone, `TAB(n)` works, and long lines wrap at the line width (80 by default; 255 never wraps).
Nothing is printed by accident: by default the output is appended to the text file `LPT1.TXT`.  `Printer.UseSystemPrinter()` sends it to the real printer as one job
(`lp`, or PowerShell's `Out-Printer` on Windows; `UseSystemPrinter("command", "arguments")` takes any command that reads the text from standard input), sent when the printer
file is closed, at `Printer.Flush()` and when the program ends.  `Printer.Device` can be set to any `IPrinterDevice` (`HeadlessPrinter` keeps the text, for tests).

## Numbers in strings, LSET and FIELD

`MKI`, `MKL`, `MKS`, `MKD` and `CVI`, `CVL`, `CVS`, `CVD` (in `QB.Devices`; `MKI$(x)` also works as a call) convert numbers to and from 2, 4, 4 and 8 byte strings (IEEE, little endian; a QBasic
INTEGER is a VB `Short`, a LONG an `Integer`); `MKSMBF`, `MKDMBF`, `CVSMBF` and `CVDMBF` do the same for the Microsoft Binary format older programs stored.  `LSET(variable, text)` and
`RSET(variable, text)` justify text inside a string variable, keeping its length.  Variables can't be bound to a file buffer in VB, so `FIELD` is numbered:
`Dim f = FIELD(1, 20, 4)` (`FIELD #1, 20 AS name$, 4 AS score$`), `f.LSET(0, "Mary")`, `f.RSET(1, MKS(99.0F))`, `PUT_FIELD(1, 1)` (`PUT #1, 1`), `GET_FIELD(1, 1)` and `f(0)`.

## DATA, PEEK and POKE

QBasic's `DATA` lines become calls, in order: `DATA_LABEL("Pieces") : DATA(1, 2, "three")`.  `READ(x)` reads the next item into a `Short`, `Integer`, `Long`, `Single`, `Double`
or `String` (numbers and strings are kept as text, so either can be read into either), `RESTORE()` starts again and `RESTORE("Pieces")` jumps to a label.  Running out of data
raises QBasic error 4 and text read into a number error 2, so `Err.Number` works in a handler.  `DEF_SEG(segment)`, `PEEK(offset)` and `POKE(offset, value)` use simulated memory
(a sparse array of bytes that starts out 0, with `DEF_SEG()` returning to the default segment): enough for the usual keyboard-flag reads and restores at `DEF_SEG(0)`, `PEEK(1047)`,
but there is no real hardware or screen memory behind it.  Several `FIELD` statements for one file overlay the same buffer, as in QBasic.

## Errors, devices and the rest of the system statements

**QBasic error numbers.**  Library errors carry QBasic's numbers, so code written for `ON ERROR` (`SELECT CASE ERR`) works with VB's `Err.Number`: 4 Out of DATA, 24 Device timeout, 25 Device fault,
50 FIELD overflow, 52 Bad file name or number, 53 File not found, 54 Bad file mode, 55 File already open, 58 File already exists, 59 Bad record length, 62 Input past end of file, 63 Bad record number,
64 Bad file name, 67 Too many files, 70 Permission denied, 76 Path not found, and 5 Illegal function call.  `ERROR_(n)` is the `ERROR n` statement (`ERDEV` and `ERDEV_STR` are stubs).

**Device files.**  `OPEN("CON", OpenMode.Output, 1)` (also `"SCRN:"`, `"CONS:"`) prints to the screen, `"KYBD:"` reads the keyboard, `"NUL"` discards, `"LPT1:"` is the printer and `"COM1:..."` a serial port.

**Keyboard.**  `INPUT(prompt, a, b, c)` reads up to four comma-separated values (asking `??` for missing ones and "Redo from start" for bad ones); `INKEY$` also returns the keypad cursor keys (NumLock off), Alt+letter and Alt+digit,
Ctrl+cursor keys and Shift+Tab with QBasic's codes.  `KEY_ON()`, `KEY_OFF()`, `KEY_LIST()` and `KEY_SET(n, text)` (in `QB.Events`) show, hide and list the function key bar.
`Video.WaitForKeyAtExit = True` keeps a finished program's window open until a key is pressed, as QBasic leaves the last screen up.

**System statements** (in `QB.Development`): `SHELL(command)`, `SYSTEM()`, `RESET()`, `CLEAR()`, `ENVIRON$(name)` / `ENVIRON_SET("NAME=value")`, `COMMAND$()`, `FRE(x)`, `TRON`/`TROFF`/`WAIT`/`INP`/`OUT` (accepted,
no hardware), `BSAVE`/`BLOAD` (the simulated memory) and, in `QB.File`, `LOCK`/`UNLOCK` and `FILEATTR`.  Not supported: `VARPTR`/`CALL ABSOLUTE`, `CHAIN`, `IOCTL` and setting the date or time.

## Sound

`SOUND`, `PLAY` and `BEEP` (in `QB.Multimedia`) make square-wave tones through the system's own audio, with no library to add: on Windows the waveform API (winmm), on macOS `afplay`, and on Linux a player that reads raw audio from its input (`paplay`, `pw-cat` or `aplay`, whichever is installed).  Without one, or with no sound device, the timing is kept silently and `BEEP` rings the terminal bell.  `QB.Speaker` (with `QB.Compatibility.Sdl`) can be imported instead to use SDL2 audio.  `PLAY` understands the music macro language (notes, `O`, `<`, `>`, `N`, `L`, `P`, `T`, `MF`/`MB`, `MN`/`ML`/`MS`, dots, sharps and flats), `MB` plays in the background, and `PLAY(0)` returns the number of notes still waiting.  `Multimedia.Audio` can be set to another `IAudioDevice` (`HeadlessAudio` records the tones for tests).

## Examples

The `Examples` project collects VB translations of the examples in the QBasic manual (https://qbasic.net/en/qb-manual/overview.htm), one file per statement in `Examples/Statements`.  Each file names the manual page it comes from and notes where VB differs (for example a QBasic `INTEGER` is a VB `Short`, `DIM` arrays start at 0, `;` becomes `&` or `PRINT(text, True)`).

```bash
dotnet run --project Examples                                   # list the examples
dotnet run --project Examples -- CIRCLE                         # run one in a window
dotnet run --project Examples -- --headless out                 # run them all without a window, saving out/NAME.ppm
```

File examples (`OPEN`, `WRITE #`, `INPUT #`, `LINE INPUT #`, ...) use `Imports QB.File` together with `Imports QB.Video`:
`OPEN("LIST", OpenMode.Output, 1)`, `WRITE(1, name, age)`, `INPUT(1, rec1, rec2)`, `LINE_INPUT(1, rec)`, `CLOSE(1)`.  VB can't import one name from two
classes, so the file-number forms of `PRINT`, `WRITE`, `INPUT`, `LINE_INPUT` and `INPUT$` are overloads in `QB.Video`, while
`OPEN`, `CLOSE`, `EOF`, `LOF`, `LOC`, `SEEK`, `FREEFILE`, `KILL`, `NAME`, `MKDIR`, `RMDIR` and `CHDIR` are in `QB.File`.  A comma after an `INPUT`
prompt (no question mark) is `INPUT(prompt, value, False)`, and `INPUT;` (the cursor stays after the typing) adds a fourth argument: `INPUT(prompt, value, False, True)`; `LINE_INPUT(prompt, value, True)` is `LINE INPUT;`.  `FILES(spec)` lists a directory.  `INPUT$(n)` from the keyboard is `InputStr(n)` (VB can't overload it with `INPUT`); `INPUT$(n, #1)` is `INPUT$(n, 1)` or `InputStr(n, 1)`.

To add one, copy a file in `Examples/Statements` (a module with a `Run` sub) and add a line for it to `Examples/Registry.vb`.

## Converting QBasic programs (qb2vb)

`qb2vb` is a console tool (installed as a .NET tool) that converts a QBasic source file to VB that calls this library:

```bash
qb2vb REMLINE.BAS -o REMLINE.vb              # just the VB module
qb2vb REMLINE.BAS -o REMLINE.vb --project    # also a REMLINE.vbproj that references the QB.Compatibility packages it needs
dotnet run --project REMLINE.vbproj
```

`--aot` (with `--project`) makes the project publish as a Native AOT program.  Working in a checkout of this repository, `dotnet run --project QB.Converter -- ...` runs the tool from source, and `--repo <repository folder>` writes project references to the checkout's projects instead of package references.

The output is deliberately literal: one QBasic statement becomes one library call on the same line, and labels, GOTOs, comments, QBasic variable names (suffixes included) and the layout are kept. What has to change is limited to what VB requires (`CONST`/`DIM SHARED`/`TYPE`/`DATA` move to the top of the module, `SUB ... STATIC` gets `Static` locals, `Resume` handlers that live at module level are copied into the procedure that names them, reserved words are renamed). Anything the converter can't convert yet is left as a `' TODO(QB2VB):` comment and listed in the report printed at the end.

Converted so far: `REMLINE.BAS`, `NIBBLES.BAS`, `GORILLA.BAS` and `MONEY.BAS` (all compile against the library and run; MONEY still has two `CALL ABSOLUTE` machine-code calls flagged). A `GOSUB` routine in the module-level code becomes a `Sub` (the module's variables become module-level so it can share them); inside a `SUB` or `FUNCTION` it becomes a lambda over the procedure's locals (a routine that uses `ON ERROR` is expanded where it is called, since a lambda can't hold an error handler). `ON ... GOSUB/GOTO` becomes a `Select Case`, a one-line `DEF FN` becomes a `Function`, array parameters get their number of dimensions from how the procedure indexes them, and `FIELD` names become the numbered fields of a `FieldRecord` (`LSET name$ = x` is `Fields1.LSET(0, x)`). The graphics statements (`PSET`, `LINE`, `CIRCLE`, `PAINT`, `GET`, `PUT`, `VIEW`, `WINDOW`) keep their shape: `LINE (1, 2)-(3, 4), 5, BF` becomes `LINE(1, 2, 3, 4, 5, LineOption.BF)`. Loops that wait on `TIMER`, and empty `FOR` loops, get an `EMPTYLOOP()` call (`QB.Core.EmptyLoopMicroseconds`, 400 by default) so programs that calibrate themselves by counting get a measurable result. `VARPTR$` works with the `X` and `=` commands of `DRAW` and `PLAY` (`DRAW "C2 X" + VARPTR$(shape$)`, `DRAW "U=" + VARPTR$(n%) + ";"`): the token holds the variable's value at the moment `VARPTR$` runs, where QBasic's holds its address. `STRING * n` variables, arrays and `TYPE` fields keep their length: everything assigned, read with `INPUT`, `LINE INPUT` or `READ` goes through `FixedString.Fit` (padded with spaces or cut), a new one holds `CHR$(0)` characters as in QBasic, and each `TYPE` becomes a `Structure` that implements `IFileRecord` (so records in random and binary files work in Native AOT builds). Still to come: `CALL ABSOLUTE`, `VARPTR`/`VARSEG` (addresses of variables) and multi-line `DEF FN`.

`Samples/Converted` holds the converted programs as projects, one folder per program (the four MS-DOS programs `GORILLA`, `MONEY`, `NIBBLES` and `REMLINE`, the programs of `Samples/QuickBASIC` and those of `Samples/Rosetta`), each with its generated `.vb` and a `.vbproj` that references the library; they are part of the solution, so `dotnet run --project Samples/Converted/NIBBLES` runs one.  They were made with `qb2vb Samples/NIBBLES.BAS -o NIBBLES.vb --repo <repository folder>` and can be made again the same way.

`Samples/Rosetta` holds 234 QBasic programs from [Rosetta Code](https://rosettacode.org) (GNU Free Documentation License 1.2; see the README there), with the output each task page shows where it has one.  A converter test converts every one without notes and compiles the result.

`Samples/QuickBASIC` holds the 23 other example programs that come with QuickBASIC 4.5 (taken from its `EXAMPLES.ZIP`), checked by a converter test in the same way.

The converter's output for every program in `Samples` and every manual example in `Samples/Manual` is kept in `QB.Converter.Tests/Golden`; the tests convert them again and compare, and compile them all together. After a change to the converter that alters the output on purpose, run `UPDATE_GOLDEN=1 dotnet test QB.Converter.Tests` and review the difference with `git diff` before committing.
