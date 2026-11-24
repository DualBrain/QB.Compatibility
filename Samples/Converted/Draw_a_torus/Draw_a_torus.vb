' Converted from QBasic by qb2vb.

Module Program

  Public Sub Main()
    QB.Graphics.WaitForKeyAtExit = True
    Dim col!
    Dim yp!
    SCREEN(13)
    CLS()
    
    Dim Rext As Single, Rint As Single, A As Single, B As Single
    Dim sinA As Single, cosA As Single, sinB As Single, cosB As Single
    Dim jj As Short, ii As Short, bright As Short, xp As Short
    Dim i As Single, sini As Single, cosi As Single
    Dim j As Single, sinj As Single, cosj As Single
    Dim h As Single, x As Single, y As Single, z As Single
    Dim tmp As Single, lum As Single, ooz As Single
    
    Rext = 90  '60
    Rint = 40  '25
    
    A = .5!
    B = .5!
    
    sinA = CSng(Sin(A))
    cosA = CSng(Cos(A))
    sinB = CSng(Sin(B))
    cosB = CSng(Cos(B))
    
    For jj = 0 To 628
        j = CSng(jj / 100)
        ii = 0
        While ii < 628
            i = CSng(ii / 100)
    
            sini = CSng(Sin(i))
            cosi = CSng(Cos(i))
            sinj = CSng(Sin(j))
            cosj = CSng(Cos(j))
    
            h = Rext + (Rint * cosj)
    
            x = (h * ((cosB * cosi) + ((sinA * sinB) * sini))) - (((Rint * cosA) * sinB) * sinj)
            y = (h * ((sinB * cosi) - ((sinA * cosB) * sini))) + (((Rint * cosA) * cosB) * sinj)
            z = ((h * cosA) * sini) + ((Rint * sinA) * sinj)
    
            tmp = (((cosj * cosi) * sinB) - (((sinA * cosj) * sini) * cosB)) - ((cosA * sinj) * cosB)
            lum = 8 * (tmp - ((cosi * sinj) * sinA))
    
            If lum > 0 Then
                bright = CShort(Int(lum * 1.5!))
                If bright > 15 Then bright = 15
                If bright < 1 Then bright = 1
    
                col = bright
    
                ooz = 1 / (z + 200)
                xp = CShort(160 + ((x * ooz) * 120))
                yp = 120 - ((y * ooz) * 120)
    
                If (((xp >= 0) And (xp < 320)) And (yp >= 0)) And (yp < 200) Then
                    PSET(xp, yp, CInt(col))
                End If
            End If
    
            ii = CShort(ii + 4)
        End While
    Next jj
    
    SLEEP()
  End Sub

End Module
