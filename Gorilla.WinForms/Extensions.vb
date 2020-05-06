﻿Option Explicit On
Option Strict On
Option Infer On

Imports System.Runtime.CompilerServices

Module Extensions

  <Extension>
  Public Function Between(value As Integer, min As Integer, max As Integer) As Boolean
    Return value >= min AndAlso value <= max
  End Function

End Module
