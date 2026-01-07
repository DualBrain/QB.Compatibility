Imports QB.Video
Imports System.Math

Public Class Form1

  Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    Init(Me, PictureBox1)

    SCREEN(13)
    COLOR(2)
    PSET(140 + Cos(0), 20)
    For i = 0 To 70 Step 0.1
      LINE(i * Cos(i) + 140, 20 - i * (Sin(i) * 0.2 - 2))
    Next
    COLOR(14)
    LINE(136, 20, 140, 12)
    LINE(144, 20) : LINE(134, 16)
    LINE(146, 16) : LINE(136, 20)

  End Sub

  Private Async Function LineDemoAsync() As Task

    SCREEN(9)

    Dim px = Rnd() * 640
    Dim py = Rnd() * 350

    Dim c = 1

    Do

      Dim x = Rnd() * 640
      Dim y = Rnd() * 350
      'Dim r = Rnd() * 100

      'CIRCLE(x, y, r)
      'LINE(px, py, x, y, c, BF)
      LINE(px, py, x, y, c)

      px = x : py = y : c += 1 : If c > 15 Then c = 1

      Await Task.Delay(1) ' Let WinForms "refresh".

    Loop

  End Function

End Class