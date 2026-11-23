Option Explicit On
Option Strict On
Option Infer On

Imports Xunit

Namespace QB.Tests

  Public Class FixedStringTests

    <Fact>
    Public Sub ShortTextIsPaddedWithSpacesAndLongTextIsCut()
      Assert.Equal("ab   ", FixedString.Fit("ab", 5))
      Assert.Equal("abcde", FixedString.Fit("abcdefg", 5))
      Assert.Equal("abcde", FixedString.Fit("abcde", 5))
      Assert.Equal("     ", FixedString.Fit(Nothing, 5))
    End Sub

    <Fact>
    Public Sub ANewFixedStringHoldsNulCharacters()
      Dim s = FixedString.Blank(3)
      Assert.Equal(3, s.Length)
      Assert.Equal(0, AscW(s(0)))
    End Sub

    <Fact>
    Public Sub ArraysAreFilledWithBlanks()
      Dim a(2) As String
      FixedString.Fill(a, 4)
      Assert.All(a, Sub(x) Assert.Equal(FixedString.Blank(4), x))
      Dim b(1, 1) As String
      FixedString.Fill(b, 2)
      Assert.Equal(FixedString.Blank(2), b(1, 1))
    End Sub

  End Class

End Namespace
