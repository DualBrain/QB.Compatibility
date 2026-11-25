Option Explicit On
Option Strict On
Option Infer On

Imports Xunit

Namespace Global.QB.Tests

  Public Class WindowTests

    <Fact>
    Public Sub TheWindowCarriesTheNameOfTheProgramUntilItIsSet()
      Video.WindowTitle = Nothing

      Dim name = Video.WindowTitle

      Assert.False(String.IsNullOrEmpty(name))
      Assert.NotEqual("QB.Compatibility", name)   ' under the test host this is the host's name: the program's, not the library's
    End Sub

    <Fact>
    Public Sub SettingTheTitleRenamesTheOpenWindowAndNothingGoesBackToTheName()
      Dim display As New HeadlessDisplay
      Video.Display = display
      Dim name = Video.DefaultWindowTitle()

      Video.WindowTitle = "Gorillas"
      Assert.Equal("Gorillas", Video.WindowTitle)
      Assert.Equal("Gorillas", display.Title)

      Video.WindowTitle = Nothing
      Assert.Equal(name, Video.WindowTitle)
      Assert.Equal(name, display.Title)
    End Sub

    <Fact>
    Public Sub TheIconIsABoldQOnBlue()
      Dim width As Integer
      Dim pixels = WindowIcon.Pixels(width)

      Assert.Equal(32, width)
      Assert.Equal(width * width, pixels.Length)
      Assert.Equal(WindowIcon.Background, pixels(0))                       ' the corners are the background
      Assert.Equal(WindowIcon.Background, pixels(pixels.Length - 1))
      Assert.All(pixels, Sub(p) Assert.True(p = WindowIcon.Background OrElse p = WindowIcon.Letters))
      Assert.True(pixels.Count(Function(p) p = WindowIcon.Letters) > 250)  ' heavy: more than a quarter of the icon
    End Sub

    <Fact>
    Public Sub TheQIsCenteredInTheIcon()
      Dim width As Integer
      Dim pixels = WindowIcon.Pixels(width)

      Dim minX = width, maxX = -1, minY = width, maxY = -1
      For y = 0 To width - 1
        For x = 0 To width - 1
          If pixels(y * width + x) = WindowIcon.Letters Then
            minX = Math.Min(minX, x) : maxX = Math.Max(maxX, x)
            minY = Math.Min(minY, y) : maxY = Math.Max(maxY, y)
          End If
        Next
      Next

      Assert.InRange(Math.Abs(minX - (width - 1 - maxX)), 0, 1)
      Assert.InRange(Math.Abs(minY - (width - 1 - maxY)), 0, 1)
      Assert.True(maxX - minX >= 20)   ' large: most of the width
    End Sub

  End Class

End Namespace
