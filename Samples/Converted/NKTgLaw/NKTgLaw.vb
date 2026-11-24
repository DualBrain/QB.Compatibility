' Converted from QBasic by qb2vb.

Module Program

  Structure NKTResult
    Implements IFileRecord
    Public p As Double
    Public NKTg1 As Double
    Public NKTg2 As Double
    Private m_Tendency1 As String
    Public Property Tendency1 As String
      Get
        Return If(m_Tendency1, FixedString.Blank(40))
      End Get
      Set(value As String)
        m_Tendency1 = FixedString.Fit(value, 40)
      End Set
    End Property
    Private m_Tendency2 As String
    Public Property Tendency2 As String
      Get
        Return If(m_Tendency2, FixedString.Blank(40))
      End Get
      Set(value As String)
        m_Tendency2 = FixedString.Fit(value, 40)
      End Set
    End Property
    Public Sub WriteTo(writer As RecordWriter) Implements IFileRecord.WriteTo
      writer.Write(p)
      writer.Write(NKTg1)
      writer.Write(NKTg2)
      writer.WriteString(Tendency1, 40)
      writer.WriteString(Tendency2, 40)
    End Sub
    Public Sub ReadFrom(reader As RecordReader) Implements IFileRecord.ReadFrom
      p = reader.ReadDouble()
      NKTg1 = reader.ReadDouble()
      NKTg2 = reader.ReadDouble()
      Tendency1 = reader.ReadString(40)
      Tendency2 = reader.ReadString(40)
    End Sub
  End Structure
  

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    ' DECLARE SUB nktg (x AS DOUBLE, v AS DOUBLE, m AS DOUBLE, dmDt AS DOUBLE, res AS ANY)
    
    ' TYPE NKTResult (see the Structure at the top)
    
    Dim r As NKTResult
    nktg(2, 3, 4, -.5!, r)
    PRINT("p        = " & Str$(r.p) & " ")
    PRINT("NKTg1    = " & Str$(r.NKTg1) & " ")
    PRINT("NKTg2    = " & Str$(r.NKTg2) & " ")
    PRINT("Tendency1= " & r.Tendency1)
    PRINT("Tendency2= " & r.Tendency2)
    End
  End Sub

  
  Sub nktg(ByRef x As Double, ByRef v As Double, ByRef m As Double, ByRef dmDt As Double, ByRef res As NKTResult)
      res.p = m * v
      res.NKTg1 = x * res.p
      res.NKTg2 = dmDt * res.p
  
      If res.NKTg1 > 0 Then
          res.Tendency1 = "Moving away from stable state"
      ElseIf res.NKTg1 < 0 Then
          res.Tendency1 = "Moving toward stable state"
      Else
          res.Tendency1 = "Stable equilibrium"
      End If
  
      If res.NKTg2 > 0 Then
          res.Tendency2 = "Mass variation supports movement"
      ElseIf res.NKTg2 < 0 Then
          res.Tendency2 = "Mass variation resists movement"
      Else
          res.Tendency2 = "No mass variation effect"
      End If
  End Sub
  
End Module
