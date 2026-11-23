Option Explicit On
Option Strict On
Option Infer On

Imports Xunit

Namespace QB.Tests

  Public Class DrawAndPlayVariableTests

    <Fact>
    Public Sub ANumberVariableIsReadWithEquals()
      Dim n As Short = 25
      Assert.Equal("U25;", Core.ExpandExecute("U=" & Core.VARPTR$(n) & ";"))
      Dim d As Double = 7.6
      Assert.Equal("L8", Core.ExpandExecute("L=" & Core.VARPTR$(d)))
      Dim l As Integer = 100000
      Assert.Equal("R100000", Core.ExpandExecute("R=" & Core.VARPTR$(l)))
    End Sub

    <Fact>
    Public Sub ASignInFrontOfEqualsMakesTheMoveRelative()
      Dim x As Short = 5, y As Short = -3
      Assert.Equal("M+5,-3", Core.ExpandExecute("M+=" & Core.VARPTR$(x) & ",=" & Core.VARPTR$(y)))
      Assert.Equal("M-5,-3", Core.ExpandExecute("M-=" & Core.VARPTR$(x) & ",=" & Core.VARPTR$(y)))
      Assert.Equal("M+3", Core.ExpandExecute("M-=" & Core.VARPTR$(y)))   ' minus a negative value
    End Sub

    <Fact>
    Public Sub AStringVariableIsExecutedWithX()
      Dim shape = "F10 L20"
      Assert.Equal("C2 F10 L20", Core.ExpandExecute("C2 X" & Core.VARPTR$(shape)))
    End Sub

    <Fact>
    Public Sub DRAWUsesTheVariable()
      Video.Display = New HeadlessDisplay
      Video.SCREEN(1)
      Video.PSET(100, 100, 3)
      Dim n As Short = 10
      Video.DRAW("C3 U=" & Core.VARPTR$(n) & ";")
      Assert.Equal(3, Video.POINT(100, 95))    ' drawn up from (100, 100) by the value of n
      Assert.Equal(0, Video.POINT(100, 80))    ' and no further
    End Sub

    <Fact>
    Public Sub TextWithoutTokensIsLeftAlone()
      Assert.Equal("U10 R=5", Core.ExpandExecute("U10 R=5"))
      Assert.Null(Core.ExpandExecute(Nothing))
    End Sub

  End Class

End Namespace
