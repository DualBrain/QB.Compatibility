Option Explicit On
Option Strict On
Option Infer On

Imports System.Diagnostics

Namespace QB2VB.Tests

  ''' <summary>Compiles converted VB with the Roslyn compiler (vbc) that ships in the .NET SDK, against QB.Compatibility.</summary>
  Friend NotInheritable Class VbCompiler

    Private Sub New()
    End Sub

    ' <root>/shared/Microsoft.NETCore.App/<version>/ -> <root>
    Private Shared Function DotnetRoot() As String
      Dim dir = Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory().TrimEnd(IO.Path.DirectorySeparatorChar)
      Return IO.Path.GetFullPath(IO.Path.Combine(dir, "..", "..", ".."))
    End Function

    ' The highest version folder under parent that contains the given sub path.
    Private Shared Function Newest(parent As String, subPath As String) As String
      Dim best As String = Nothing
      Dim bestVersion As Version = Nothing
      For Each d In IO.Directory.GetDirectories(parent)
        Dim v As Version = Nothing
        Dim name = IO.Path.GetFileName(d)
        Dim dash = name.IndexOf("-"c)
        If Version.TryParse(If(dash > 0, name.Substring(0, dash), name), v) AndAlso IO.Directory.Exists(IO.Path.Combine(d, subPath)) OrElse
           (v IsNot Nothing AndAlso IO.File.Exists(IO.Path.Combine(d, subPath))) Then
          If bestVersion Is Nothing OrElse v > bestVersion Then best = d : bestVersion = v
        End If
      Next
      If best Is Nothing Then Throw New IO.DirectoryNotFoundException("No " & subPath & " under " & parent)
      Return best
    End Function

    ''' <summary>The compiler's error lines (empty when the source compiles).</summary>
    Friend Shared Function Compile(vbSource As String, mainModule As String) As String

      Dim root = DotnetRoot()
      Dim vbc = IO.Path.Combine(Newest(IO.Path.Combine(root, "sdk"), IO.Path.Combine("Roslyn", "bincore", "vbc.dll")), "Roslyn", "bincore", "vbc.dll")
      Dim refDir = IO.Path.Combine(Newest(IO.Path.Combine(root, "packs", "Microsoft.NETCore.App.Ref"), IO.Path.Combine("ref", "net10.0")), "ref", "net10.0")
      Dim work = IO.Path.Combine(IO.Path.GetTempPath(), "qb2vb-" & Guid.NewGuid().ToString("N"))
      IO.Directory.CreateDirectory(work)
      Try
        Dim src = IO.Path.Combine(work, "program.vb")
        IO.File.WriteAllText(src, vbSource)

        Dim psi As New ProcessStartInfo("dotnet") With {.RedirectStandardOutput = True, .RedirectStandardError = True}
        psi.ArgumentList.Add(vbc)
        For Each a In {"-nologo", "-target:exe", "-main:" & mainModule, "-nostdlib", "-noconfig", "-vbruntime-", "-define:_MYTYPE=""Empty""",
                       "-imports:Microsoft.VisualBasic,System,System.Collections.Generic,System.IO,System.Linq",
                       "-optionstrict+", "-optionexplicit+", "-optioninfer+", "-out:" & IO.Path.Combine(work, "program.dll")}
          psi.ArgumentList.Add(a)
        Next
        For Each r In IO.Directory.GetFiles(refDir, "*.dll")
          psi.ArgumentList.Add("-r:" & r)
        Next
        psi.ArgumentList.Add("-r:" & GetType(QB.Video).Assembly.Location)
        psi.ArgumentList.Add(src)

        Using p = Process.Start(psi)
          Dim output = p.StandardOutput.ReadToEnd() & p.StandardError.ReadToEnd()
          p.WaitForExit()
          Return If(p.ExitCode = 0, "", output)
        End Using
      Finally
        Try
          IO.Directory.Delete(work, True)
        Catch ex As IO.IOException
        End Try
      End Try

    End Function

  End Class

End Namespace
