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
    Public Sub TheIconIsTheLettersQBOnBlue()
      Dim width As Integer
      Dim pixels = WindowIcon.Pixels(width)

      Assert.Equal(32, width)
      Assert.Equal(width * width, pixels.Length)
      Assert.Equal(WindowIcon.Background, pixels(0))                       ' the corners are the background
      Assert.Equal(WindowIcon.Background, pixels(pixels.Length - 1))
      Assert.Contains(WindowIcon.Letters, pixels)                          ' with the letters on it
      Assert.All(pixels, Sub(p) Assert.True(p = WindowIcon.Background OrElse p = WindowIcon.Letters))
    End Sub

    <Fact>
    Public Sub TheLettersSitInsideTheIconAndBothAreThere()
      Dim width As Integer
      Dim pixels = WindowIcon.Pixels(width)

      Dim left = 0, right = 0
      For y = 0 To width - 1
        For x = 0 To width - 1
          If pixels(y * width + x) = WindowIcon.Letters Then
            If x < width \ 2 Then left += 1 Else right += 1
          End If
        Next
      Next

      Assert.True(left > 30)    ' the Q
      Assert.True(right > 30)   ' the B
    End Sub

  End Class

End Namespace
