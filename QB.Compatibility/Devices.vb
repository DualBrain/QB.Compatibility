Option Explicit On
Option Strict On
Option Infer On

Namespace Global

  Partial Public NotInheritable Class QB

    ' Keyboard

    ' Printer

    ' Communications Port

    ' Files

#Region "CVI, CVS, CVD, CVII$, MKI, MKS, MKD"

    Public Shared Function CVI(X As String) As Short

      Dim b(7) As Short
      For i = 1 To 2
        b(i) = CShort(AscW(X.Substring(2 - i)))
      Next

      Dim d(16) As Short
      Dim n As Short = 0
      For j = 1 To 2
        Dim c = b(j)
        For k = 1 To 8
          n = n + CShort(1)
          Dim y = CShort(Fix(c / (2 ^ (8 - k))))
          If y = 1 Then
            d(n) = y ' Binary array
            c = CShort(c - (2 ^ (8 - k)))
          Else : d(n) = 0
          End If

        Next
      Next

      Dim total As Short = 0 ' Calculation - Power

      For i = 0 To 14 ' Calculation - Binary
        n = CShort(14 - i)
        total = CShort(total + d(i + 2) * (2 ^ n))
      Next
      If d(1) = 1 Then ' Sign of the value
        total = total * CShort(-1)
      End If

      Return total

    End Function

    Public Shared Function CVS(X As String) As Single

      Dim D(24) As Short, B(3) As Short
      Dim i As Short, J As Short, k As Short
      Dim N As Short, Y As Short, C As Short
      Dim Expt As Short, PNT As Short, Pwr As Short
      Dim TOT As Single, STOT As Single

      'Expt = AscW(Right(X, 1)) ' Exponent part
      Expt = CShort(AscW(X.Substring(X.Length - 1))) ' Exponent part
      For i = 1 To 3
        'B(i) = AscW(Mid(X, 4 - i)) ' Mantissa part
        B(i) = CShort(AscW(X.Substring(3 - i))) ' Mantissa part
      Next

      N = 0
      For J = 1 To 3
        C = B(J)
        For k = 1 To 8
          N = N + CShort(1)
          Y = CShort(Fix(C / (2 ^ (8 - k))))
          If Y = 1 Then
            D(N) = Y ' Binary array for mantissa
            C = CShort(C - (2 ^ (8 - k)))
          Else : D(N) = 0
          End If

        Next
      Next

      ' For i = 1 To 24
      ' Debug.Print D(i);
      ' Next
      ' Debug.Print


      Pwr = Expt - CShort(129) ' Power of 2
      If Pwr = -129 Then ' Calculation - Power
        TOT = 0
      Else : TOT = CShort(2 ^ Pwr)
      End If

      PNT = Pwr + CShort(2) ' Starting Point of Binary decimal
      STOT = 0 ' sub total Binary array
      k = 1 ' array element number

      For i = 0 To Pwr - CShort(1) ' Calculation - Binary
        N = (Pwr - CShort(1)) - i
        k = k + CShort(1)
        If i < 23 Then
          STOT = CShort(STOT + D(i + 2) * (2 ^ N))
        End If
      Next

      If PNT < 0 Then ' set - power binary decimal
        N = PNT - CShort(2)
      End If

      For J = k + CShort(1) To 24 ' Calculation - Binary Decimal
        If PNT < 0 Then
          N = N - CShort(1)
        Else : N = PNT - (J + CShort(1))
        End If
        STOT = CShort(STOT + D(J) * (2 ^ N))
      Next

      TOT = TOT + STOT ' Full Total

      If D(1) = 1 Then ' Sign of the value
        TOT = TOT * (-1)
      End If

      Return TOT

    End Function

    Public Shared Function CVD(X As String) As Double

      Dim D(56) As Short, B(7) As Short
      Dim i As Short, J As Short, k As Short
      Dim N As Short, Y As Short, C As Short
      Dim Expt As Short, PNT As Short, Pwr As Short
      Dim TOT As Double, STOT As Double

      'Expt = AscW(Right(X, 1)) ' Exponent part
      Expt = CShort(AscW(X.Substring(X.Length - 1))) ' Exponent part

      For i = 1 To 7
        'B(i) = AscW(Mid(X, 8 - i)) ' Mantissa part
        B(i) = CShort(AscW(X.Substring(7 - i))) ' Mantissa part
      Next

      N = 0
      For J = 1 To 7
        C = B(J)
        For k = 1 To 8
          N = N + CShort(1)
          Y = CShort(Fix(C / (2 ^ (8 - k))))
          If Y = 1 Then
            D(N) = Y ' Binary array for mantissa
            C = CShort(C - (2 ^ (8 - k)))
          Else : D(N) = 0
          End If

        Next
      Next

      ' For i = 1 To 56
      ' Debug.Print D(i);
      ' Next
      ' Debug.Print


      Pwr = Expt - CShort(129) ' Power of 2
      If Pwr = -129 Then ' Calculation - Power
        TOT = 0
      Else : TOT = 2 ^ Pwr
      End If

      PNT = Pwr + CShort(2) ' Starting Point of Binary decimal
      STOT = 0 ' sub total Binary array
      k = 1 ' array element number

      For i = 0 To Pwr - CShort(1) ' Calculation - Binary
        N = CShort((Pwr - 1) - i)
        k = k + CShort(1)
        If i < 55 Then
          STOT = STOT + D(i + 2) * (2 ^ N)
        End If
      Next

      If PNT < 0 Then ' set - power binary decimal
        N = CShort(PNT - 2)
      End If

      For J = k + CShort(1) To 56 ' Calculation - Binary Decimal
        If PNT < 0 Then
          N = N - CShort(1)
        Else : N = PNT - (J + CShort(1))
        End If
        STOT = STOT + D(J) * (2 ^ N)
      Next

      TOT = TOT + STOT ' Full Total

      If D(1) = 1 Then ' Sign of the value
        TOT = TOT * (-1)
      End If

      Return TOT
    End Function

    Public Shared Function CVII$(X As String)
      '    Dim S, SS As Short
      '    On Error GoTo Err
      '    S = AscW(Left(X, 1)) + AscW(Right(X, 1)) * 256
      'Err:
      '    If Err.Number = 6 Then
      '      SS = 255 - AscW(Left(X, 1)) + (255 - AscW(Right(X, 1))) * 256 + 1
      '      S = (SS) * -1
      '    End If
      '    CVII$ = S

      Dim S, SS As Short
      Try
        S = CShort(AscW(X.Substring(0, 1)) + AscW(X.Substring(X.Length - 1)) * 256)
      Catch
        SS = CShort(255 - AscW(X.Substring(0, 1)) + (255 - AscW(X.Substring(X.Length - 1))) * 256 + 1)
        S = CShort((SS) * -1)
      End Try
      CVII$ = S.ToString

    End Function

    Public Shared Function MKI(X As Short) As String

      Dim D(16) As Byte, V(2) As Byte
      Dim i As Short, J As Short, k As Short
      Dim N As Short, C As Short

      If X < 0 Then D(1) = 1 ' Sign of the number

      C = Math.Abs(X) ' Absolute Value
      For i = 0 To 14 ' Set Binary array
        N = CShort(14) - i
        If Conversion.Fix(C / 2 ^ N) = 1 Then
          D(i + 2) = 1
          C = CShort(C - 2 ^ N)
        Else : D(i + 2) = 0
        End If
      Next

      'For i = 1 To 16
      ' Debug.Print D(i);
      'Next
      'Debug.Print

      N = 0
      For J = 1 To 2 ' Divide - 8 bit bytes
        For k = 1 To 8
          N = N + CShort(1)
          V(J) = CByte(V(J) + D(N) * 2 ^ (8 - k))
        Next
      Next

      Return ChrW(V(2)) + ChrW(V(1)) ' Setup String

    End Function

    Public Shared Function MKS(X As Single) As String

      Dim D(24) As Byte, V(3) As Byte
      Dim MxExp As Short, AExp As Short
      Dim i As Short, J As Short, k As Short
      Dim N As Short, E As Single
      Dim Found As Boolean

      If X < 0 Then ' Sign of the number
        D(1) = 1
      Else : D(1) = 0
      End If

      E = CSng(Math.Abs(X)) ' Absolute Value
      Found = False
      For i = 0 To 256 ' Cal Maximum power
        N = CShort(128 - i)
        If E >= (2 ^ N) Then
          MxExp = N
          i = 256
          Found = True
        End If
      Next

      If E = 0 Or (Not Found) Then ' Set Max Power for String
        AExp = 0
      Else
        AExp = CShort(129 + MxExp)
        E = CSng(E - (2 ^ MxExp))
      End If

      For k = 1 To 23 ' Set Binary array
        N = MxExp - k
        If Conversion.Fix(E / (2 ^ N)) = 1 Then
          D(k + 1) = 1
          E = CSng(E - (2 ^ N))
        Else : D(k + 1) = 0
        End If
      Next

      ' For i = 1 To 24
      ' Debug.Print D(i);
      ' Next
      ' Debug.Print

      N = 0
      For J = 1 To 3 ' Divide - 8 bit bytes
        For k = 1 To 8
          N = CShort(N + 1)
          V(J) = CByte(V(J) + D(N) * 2 ^ (8 - k))
        Next
      Next

      ' Set-up String
      Return ChrW(V(3)) + ChrW(V(2)) + ChrW(V(1)) + ChrW(AExp)

    End Function

    Public Shared Function MKD(X As Double) As String

      Dim D(56) As Byte, V(7) As Byte
      Dim MxExp As Short, AExp As Short ', C As Short
      Dim i As Short, J As Short, k As Short
      Dim N As Short, E As Double
      Dim Found As Boolean
      Dim tMKD As String

      If X < 0 Then ' Sign of the number
        D(1) = 1
      Else : D(1) = 0
      End If

      E = Math.Abs(X) ' Absolute Value
      Found = False
      For i = 0 To 256 ' Cal Maximum power
        N = CShort(128 - i)
        If E >= (2 ^ N) Then
          MxExp = N
          i = 256
          Found = True
        End If
      Next

      If E = 0 Or (Not Found) Then ' Set Max Power for String
        AExp = 0
      Else
        AExp = CShort(129 + MxExp)
        E = E - (2 ^ MxExp)
      End If

      For k = 1 To 55 ' Set Binary array
        N = MxExp - k
        If Conversion.Fix(E / (2 ^ N)) = 1 Then
          D(k + 1) = 1
          E = E - (2 ^ N)
        Else : D(k + 1) = 0
        End If
      Next

      ' For i = 1 To 24
      ' Debug.Print D(i);
      ' Next
      ' Debug.Print

      N = 0
      For J = 1 To 7 ' Divide - 8 bit bytes
        For k = 1 To 8
          N = CShort(N + 1)
          V(J) = CByte(V(J) + D(N) * 2 ^ (8 - k))
        Next
      Next

      tMKD = ""
      For i = 1 To 7 ' Set-up String
        k = CShort(8 - i)
        tMKD = tMKD + ChrW(V(k))
      Next
      tMKD = tMKD + ChrW(AExp)

      Return tMKD
    End Function

#End Region

    'NOTE: The following file routines have to be "rebuilt" as the 
    '      ones available as part of the VisualBasic.FileSystem aren't
    '      capable of being utilized separately across multiple methods;
    '      in other words, FileSystem does some "magic" with regards
    '      to it's internal variables where these are localized to 
    '      a particular scope; if you attempt to use the filenum from 
    '      a different scope, the method will fail.

    Private Shared m_files As New Dictionary(Of Integer, IO.FileStream)

    Public Shared Sub CLOSE(ParamArray filenums%())

      If filenums%.Count = 0 Then
        ' Close all files.
        For filenum% = 1 To 255
          If m_files.ContainsKey(filenum%) Then
            m_files(filenum%).Close()
            m_files.Remove(filenum%)
          End If
        Next
      Else
        ' Close one or more specific files.
        For Each filenum% In filenums%
          m_files(filenum%).Close()
          m_files.Remove(filenum%)
        Next
      End If

    End Sub

    Public Shared Function EOF(filenum%) As Boolean
      If m_files.ContainsKey(filenum%) Then
        If m_files(filenum%).Position = m_files(filenum%).Length - 1 Then
          Return True
        Else
          Return False
        End If
      Else
        Throw New ArgumentException("Bad file name or number")
      End If
    End Function

    Public Shared Function FREEFILE() As Integer
      If m_files.Count > 15 Then
        Throw New InvalidOperationException("Too many files")
      End If
      For index = 1 To 255
        If Not m_files.ContainsKey(index) Then
          Return index
        End If
      Next
      Throw New Exception() ' <--- Should never reach here.
    End Function

    Public Shared Sub LINE_INPUT(filenum%, ByRef value$)
      value$ = ""
      Dim b(0) As Byte
      Do
        Dim result = m_files(filenum%).Read(b, 0, 1)
        If result = 0 Then
          Return
        End If
        value$ &= Chr(b(0))
        If b(0) = 13 Then
          ' nearing end of line, assume next character is a LF...
          result = m_files(filenum%).Read(b, 0, 1)
          value$ &= Chr(b(0))
          value$ = value$.Substring(0, value$.Length - 2) ' Trim of the CRLF
          Return
        End If
      Loop
    End Sub

    Public Shared Sub LSET(ByRef variable$, value$)

      variable$ = Space(Len(variable$))

      If Len(variable$) >= Len(value$) Then
        variable$ = value$ + variable$.Substring(Len(value$))
      Else
        ' ????
      End If

    End Sub

    Public Shared Sub NAME(oldfilespec$, newfilespec$)
      IO.File.Move(oldfilespec$, newfilespec$)
    End Sub

    Public Shared Sub OPEN(file$, accessMode As OpenMode, filenum%)

      Dim fm As IO.FileMode = IO.FileMode.Open
      Select Case accessMode
        Case OpenMode.Append : fm = IO.FileMode.Append
        Case OpenMode.Binary : fm = IO.FileMode.OpenOrCreate
        Case OpenMode.Input : fm = IO.FileMode.Open
        Case OpenMode.Output : fm = IO.FileMode.OpenOrCreate
        Case OpenMode.Random : fm = IO.FileMode.OpenOrCreate
        Case Else
      End Select

      Select Case accessMode
        Case OpenMode.Output
          If IO.File.Exists(file$) Then
            IO.File.Delete(file$)
          End If
      End Select

      If m_files.ContainsKey(filenum%) Then
        Throw New ArgumentException
      Else
        Dim fs = New IO.FileStream(file$, fm)
        m_files.Add(filenum%, fs)
      End If

    End Sub

    Public Shared Sub PRINT(filenum%, value$)
      If m_files.ContainsKey(filenum%) Then
        Dim b = Text.ASCIIEncoding.ASCII.GetBytes(value$ & vbCrLf)
        m_files(filenum%).Write(b, 0, b.Length)
      End If
    End Sub

  End Class

End Namespace