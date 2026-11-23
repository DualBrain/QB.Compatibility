Option Explicit On
Option Strict On
Option Infer On

Imports System.IO
Imports System.Text

Namespace QB2VB

  Friend Module Program

    Public Function Main(args As String()) As Integer

      Dim input = ""
      Dim output = ""
      Dim i = 0
      Do While i < args.Length
        Dim a = args(i)
        If a = "-o" AndAlso i + 1 < args.Length Then
          output = args(i + 1) : i += 2
        ElseIf a = "-h" OrElse a = "--help" Then
          Console.WriteLine("qb2vb input.bas [-o output.vb]    converts a QBasic program to VB that uses QB.Compatibility")
          Return 0
        Else
          input = a : i += 1
        End If
      Loop
      If input = "" Then
        Console.Error.WriteLine("usage: qb2vb input.bas [-o output.vb]")
        Return 2
      End If

      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
      Dim source = File.ReadAllText(input, Encoding.GetEncoding(437))
      Dim moduleName = Path.GetFileNameWithoutExtension(input)
      Dim sb As New StringBuilder
      For Each c In moduleName
        sb.Append(If(Char.IsLetterOrDigit(c), c, "_"c))
      Next
      moduleName = sb.ToString
      If moduleName = "" OrElse Char.IsDigit(moduleName(0)) Then moduleName = "Program_" & moduleName

      Dim conv As New Converter(source)
      Dim vb = conv.Convert(moduleName)
      If output = "" Then output = Path.ChangeExtension(input, ".vb")
      File.WriteAllText(output, vb, New UTF8Encoding(False))

      Console.WriteLine($"{input} -> {output}")
      For Each n In conv.Notes
        Console.WriteLine("  " & n)
      Next
      Return If(conv.Notes.Count > 0, 1, 0)

    End Function

  End Module

End Namespace
