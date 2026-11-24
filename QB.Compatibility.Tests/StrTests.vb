Option Explicit On
Option Strict On
Option Infer On

Imports Xunit

Namespace QB.Tests

  ' STR$ writes numbers the way QBasic does.
  Public Class StrTests

    <Fact>
    Public Sub WholeNumbersHaveASpaceOrAMinusInFront()
      Assert.Equal(" 5", Core.Str$(CShort(5)))
      Assert.Equal("-5", Core.Str$(CShort(-5)))
      Assert.Equal(" 100000", Core.Str$(100000))
      Assert.Equal(" 0", Core.Str$(0.0F))
    End Sub

    <Fact>
    Public Sub SinglesKeepSevenDigits()
      Assert.Equal(" 3.141593", Core.Str$(CSng(Math.PI)))
      Assert.Equal(" .3333333", Core.Str$(1.0F / 3.0F))
      Assert.Equal(" 1.5", Core.Str$(1.5F))
      Assert.Equal(" 1234567", Core.Str$(1234567.0F))
      Assert.Equal(" 1.234568E+07", Core.Str$(12345678.0F))
      Assert.Equal(" .001", Core.Str$(0.001F))
      Assert.Equal(" 1E-04", Core.Str$(0.0001F))
      Assert.Equal("-.5", Core.Str$(-0.5F))
    End Sub

    <Fact>
    Public Sub DoublesKeepSixteenDigits()
      Assert.Equal(" 3.703680038452148", Core.Str$(3.7036800384521484))
      Assert.Equal(" .3333333333333333", Core.Str$(1.0 / 3.0))
      Assert.Equal(" 25264", Core.Str$(25264.0))
      Assert.Equal(" 1D+16", Core.Str$(1.0E+16))
      Assert.Equal(" 1.5D-05", Core.Str$(0.000015))
    End Sub

  End Class

End Namespace
