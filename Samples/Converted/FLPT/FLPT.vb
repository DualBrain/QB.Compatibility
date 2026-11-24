' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Video.WaitForKeyAtExit = True
#If QB_WINDOW Then
    QB.SdlBackend.Use()
#End If
    Dim A$ = ""
    Dim RealValue!
    Dim AsLong%
    Dim Strout$ = ""
    Dim SignBit%
    Dim Sign_ As Short
    Dim Exponent As Short
    Dim Mant1 As Short
    '
    ' FLPT.BAS
    '
    ' Displays how a given real value is stored in memory.
    '
    '
    ' DEFINT A-Z
    ' DECLARE FUNCTION MHex$ (X AS INTEGER)
    Dim Bytes(3) As Short
    
    CLS()
    PRINT("Internal format of IEEE number (all values in hexadecimal)")
    PRINT()
    Do
    
       ' Get the value and calculate the address of the variable.
       INPUT("Enter a real number (or END to quit): ", A$, False)
       If UCase$(A$) = "END" Then Exit Do
       RealValue! = CSng(Val(A$))
       ' Convert the real value to a long without changing any of
       ' the bits.
       AsLong% = CVL(MKS$(RealValue!))
       ' Make a string of hex digits, and add leading zeroes.
       Strout$ = Hex$(AsLong%)
       Strout$ = QBString(8 - Len(Strout$), "0") + Strout$
    
       ' Save the sign bit, and then eliminate it so it doesn't
       ' affect breaking out the bytes
       SignBit% = AsLong% And Integer.MinValue
       AsLong% = AsLong% And 2147483647
       ' Split the real value into four separate bytes
       ' --the AND removes unwanted bits; dividing by 256 shifts
       ' the value right 8 bit positions.
       For I = 0S To 3S
          Bytes(I) = CShort(AsLong% And 255)
          AsLong% = AsLong% \ 256
       Next I
       ' Display how the value appears in memory.
       PRINT()
       PRINT("Bytes in Memory")
       PRINT(" High    Low")
       For I = 1S To 7S Step 2S
          PRINT(" " & Mid$(Strout$, I, 2), True)
       Next I
       PRINT() : PRINT()
    
       ' Set the value displayed for the sign bit.
       Sign_ = Abs(CShort(SignBit% <> 0))
    
       ' The exponent is the right seven bits of byte 3 and the
       ' leftmost bit of byte 2. Multiplying by 2 shifts left and
       ' makes room for the additional bit from byte 2.
       Exponent = CShort((Bytes(3) * 2) + (Bytes(2) \ 128))
    
       ' The first part of the mantissa is the right seven bits
       ' of byte 2.  The OR operation makes sure the implied bit
       ' is displayed by setting the leftmost bit.
       Mant1 = CShort((Bytes(2) Or 128))
       PRINT(" Bit 31    Bits 30-23  Implied Bit & Bits 22-0")
       PRINT("Sign Bit  Exponent Bits     Mantissa Bits")
       PRINT(TAB(4) & Str$(Sign_) & " " & TAB(17) & MHex$(Exponent), True)
       PRINT(TAB(33) & MHex$(Mant1) & MHex$(Bytes(1)) & MHex$(Bytes(0)))
       PRINT()
    
    Loop
    
  End Sub

  
  ' MHex$ makes sure we always get two hex digits.
  Function MHex$(ByRef X As Short)
    Static D$ = ""
     D$ = Hex$(X)
     If Len(D$) < 2 Then D$ = "0" + D$
     MHex$ = D$
  End Function
  
End Module
