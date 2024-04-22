Imports QB.Video

Public Class Form1

  Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    Init(Me, PictureBox1)

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

  End Sub

End Class