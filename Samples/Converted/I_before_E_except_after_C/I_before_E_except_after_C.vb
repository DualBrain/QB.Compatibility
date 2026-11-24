' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim CI As Short
    Dim XI As Short
    Dim CE As Short
    Dim XE As Short
    ' DEFINT A-Z
    Dim W As String
    CLS()
    OPEN("UNIXDICT.TXT", OpenMode.Input, 1)
    Do
        LINE_INPUT(1, W)
        If InStr(W, "ie") <> 0 Then If InStr(W, "cie") <> 0 Then CI = CShort(CI + 1) Else XI = CShort(XI + 1)
        If InStr(W, "ei") <> 0 Then If InStr(W, "cei") <> 0 Then CE = CShort(CE + 1) Else XE = CShort(XE + 1)
    Loop While Not EOF(1)
    CLOSE(1)
    
    PRINT("CIE:" & Str$(CI) & " ")
    PRINT("xIE:" & Str$(XI) & " ")
    PRINT("CEI:" & Str$(CE) & " ")
    PRINT("xEI:" & Str$(XE) & " ")
    PRINT()
    PRINT("I before E when not preceded by C: ", True)
    If (2 * XI) <= CI Then PRINT("not ", True)
    PRINT("plausible.")
    PRINT("E before I when preceded by C: ", True)
    If (2 * CE) <= XE Then PRINT("not ", True)
    PRINT("plausible.")
  End Sub

End Module
