Imports System.Drawing

Imports QB.Console
Imports QB.ConsoleEx
Imports QB.Core
Imports QB.Development
Imports QB.Devices
Imports QB.File
Imports QB.LineOption
Imports QB.Multimedia
Imports QB.PutOption
Imports QB.Video

Public Class Form1

  Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    QB.Video.Init(Me, PictureBox1)

    QB.Video.SCREEN(9)

    Do

      Dim x = Rnd() * 640
      Dim y = Rnd() * 350
      Dim r = Rnd() * 100

      CIRCLE(x, y, r)

      Await Task.Delay(1)

    Loop


  End Sub

End Class
