' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
  End Sub

  Function math(ByRef a!, ByRef b!) As Single
  	PRINT(Str$(a! + b!) & " ")
  	PRINT(Str$(a! - b!) & " ")
  	PRINT(Str$(a! * b!) & " ")
  	PRINT(Str$(a! / b!) & " ")
  	PRINT(Str$(CInt(a!) Mod CInt(b!)) & " ")
  End Function
  
End Module
