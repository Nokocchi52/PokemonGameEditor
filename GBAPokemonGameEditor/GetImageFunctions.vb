﻿Imports System
Imports System.IO

Module GetImageFunctions

    Public Function DrawBlockToTile(ByVal Destination As Bitmap, ByVal Source As Bitmap, ByVal BlockNum As Integer, ByVal yflip As Integer, ByVal xflip As Integer, ByVal Tile As Integer, ByVal section As Integer) As Bitmap
        Dim Output As Bitmap = Destination
        Dim PixelColor As Color

        Dim HeightLoop As Integer
        Dim HeightCounter As Integer

        Dim TileAgain As Integer

        Dim xdes As Integer
        Dim ydes As Integer
        Dim xsrc As Integer
        Dim ysrc As Integer
        Dim x As Integer
        Dim y As Integer

        'For destination


        HeightLoop = Tile
        HeightCounter = 0

        While HeightLoop > 7

            HeightLoop = HeightLoop - 8
            HeightCounter = HeightCounter + 1

        End While

        ydes = (HeightCounter * 16) '+ (section * 8)

        xdes = ((Tile - (HeightCounter * 8)) * 16) '+ (section * 8)

        If section = 1 Then
            xdes = xdes + 8
        ElseIf section = 2 Then
            ydes = ydes + 8
        ElseIf section = 3 Then
            xdes = xdes + 8
            ydes = ydes + 8
        End If

        'For the source
        If BlockNum < 512 Then

            TileAgain = BlockNum

        Else

            TileAgain = BlockNum - 512

        End If

        HeightLoop = TileAgain
        HeightCounter = 0

        While HeightLoop > 15

            HeightLoop = HeightLoop - 16
            HeightCounter = HeightCounter + 1

        End While

        ysrc = HeightCounter * 8

        xsrc = (TileAgain - (HeightCounter * 16)) * 8


        If xflip = 0 And yflip = 0 Then

            For x = 0 To 7
                For y = 0 To 7
                    PixelColor = Source.GetPixel((xsrc) + x, (ysrc) + y)

                    Output.SetPixel((xdes) + x, (ydes) + y, PixelColor)

                Next y

            Next x
        End If

        If xflip = 1 And yflip = 0 Then

            For x = 0 To 7
                For y = 0 To 7
                    PixelColor = Source.GetPixel((xsrc) + x, (ysrc) + y)

                    Output.SetPixel((xdes + 7) - x, (ydes) + y, PixelColor)

                Next y

            Next x
        End If

        If xflip = 0 And yflip = 1 Then

            For x = 0 To 7
                For y = 0 To 7
                    PixelColor = Source.GetPixel((xsrc) + x, (ysrc) + y)

                    Output.SetPixel((xdes) + x, (ydes + 7) - y, PixelColor)

                Next y

            Next x
        End If

        If xflip = 1 And yflip = 1 Then

            For x = 0 To 7
                For y = 0 To 7
                    PixelColor = Source.GetPixel((xsrc) + x, (ysrc) + y)

                    Output.SetPixel((xdes + 7) - x, (ydes + 7) - y, PixelColor)

                Next y

            Next x
        End If
        DrawBlockToTile = Output
    End Function

    Public Sub GetAndDrawItemPic(ByVal picBox As PictureBox, ByVal index As Integer)
        Dim sOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "ItemIMGData", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon front sprites, + 8 = Bulbasaur.
        Dim pOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "ItemIMGData", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) + 4 'Pointer to Pokemon normal palettes, + 8 = Bulbasaur.
        Dim Temp(&HFFF) As Byte
        Dim Image(&HFFFF) As Byte
        Dim Palette15(&HFFF) As Byte
        Dim Palette32() As Color
        Dim bSprite As Bitmap
        Using fs As New FileStream(LoadedROM, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs)
                fs.Position = sOffset
                sOffset = r.ReadInt32 - &H8000000
                fs.Position = sOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Image)

                ReDim Temp(&HFFF)
                fs.Position = pOffset
                pOffset = r.ReadInt32 - &H8000000
                fs.Position = pOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Palette15)

                Palette32 = LoadPalette(Palette15)
            End Using
        End Using


        bSprite = LoadSprite(Image, Palette32, 24, 24, GetString(AppPath & "GBAPGESettings.ini", "Settings", "TransparentImages", "0"))
        picBox.Image = bSprite
        picBox.Refresh()

    End Sub

    Public Sub GetAndDrawFrontPokemonPic(ByVal picBox As PictureBox, ByVal index As Integer)
        Dim sOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "PokemonFrontSprites", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon front sprites, + 8 = Bulbasaur.
        Dim pOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "PokemonNormalPal", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon normal palettes, + 8 = Bulbasaur.
        Dim Temp(&HFFF) As Byte
        Dim Image(&HFFFF) As Byte
        Dim Palette15(&HFFF) As Byte
        Dim Palette32() As Color
        Dim bSprite As Bitmap
        Using fs As New FileStream(LoadedROM, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs)
                fs.Position = sOffset
                sOffset = r.ReadInt32 - &H8000000
                fs.Position = sOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Image)

                ReDim Temp(&HFFF)
                fs.Position = pOffset
                pOffset = r.ReadInt32 - &H8000000
                fs.Position = pOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Palette15)

                Palette32 = LoadPalette(Palette15)
            End Using
        End Using


        bSprite = LoadSprite(Image, Palette32, 64, 64, GetString(AppPath & "GBAPGESettings.ini", "Settings", "TransparentImages", "0"))
        picBox.Image = bSprite
        picBox.Refresh()

    End Sub

    Public Sub GetAndDrawBackPokemonPic(ByVal picBox As PictureBox, ByVal index As Integer)
        Dim sOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "PokemonBackSprites", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon front sprites, + 8 = Bulbasaur.
        Dim pOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "PokemonShinyPal", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon normal palettes, + 8 = Bulbasaur.
        Dim Temp(&HFFF) As Byte
        Dim Image(&HFFFF) As Byte
        Dim Palette15(&HFFF) As Byte
        Dim Palette32() As Color
        Dim bSprite As Bitmap
        Using fs As New FileStream(LoadedROM, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs)
                fs.Position = sOffset
                sOffset = r.ReadInt32 - &H8000000
                fs.Position = sOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Image)

                ReDim Temp(&HFFF)
                fs.Position = pOffset
                pOffset = r.ReadInt32 - &H8000000
                fs.Position = pOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Palette15)

                Palette32 = LoadPalette(Palette15)
            End Using
        End Using


        bSprite = LoadSprite(Image, Palette32, 64, 64, GetString(AppPath & "GBAPGESettings.ini", "Settings", "TransparentImages", "0"))
        picBox.Image = bSprite
        picBox.Refresh()

    End Sub

    Public Sub GetAndDrawAnimationPokemonPic(ByVal picBox As PictureBox, ByVal index As Integer)
        Dim sOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "PokemonAnimations", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon front sprites, + 8 = Bulbasaur.
        Dim pOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "PokemonNormalPal", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon normal palettes, + 8 = Bulbasaur.
        Dim Temp(&HFFF) As Byte
        Dim Image(&HFFFF) As Byte
        Dim Palette15(&HFFF) As Byte
        Dim Palette32() As Color
        Dim bSprite As Bitmap
        Using fs As New FileStream(LoadedROM, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs)
                fs.Position = sOffset
                sOffset = r.ReadInt32 - &H8000000
                fs.Position = sOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Image)

                ReDim Temp(&HFFF)
                fs.Position = pOffset
                pOffset = r.ReadInt32 - &H8000000
                fs.Position = pOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Palette15)

                Palette32 = LoadPalette(Palette15)
            End Using
        End Using


        bSprite = LoadSprite(Image, Palette32, 64, 128, GetString(AppPath & "GBAPGESettings.ini", "Settings", "TransparentImages", "0"))
        picBox.Image = bSprite
        picBox.Refresh()

    End Sub

    Public Sub GetAndDrawShadowAnimationPokemonPic(ByVal picBox As PictureBox, ByVal index As Integer)
        Dim sOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "ShadowFronts", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon front sprites, + 8 = Bulbasaur.
        Dim pOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "ShadowPals", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon normal palettes, + 8 = Bulbasaur.
        Dim Temp(&HFFF) As Byte
        Dim Image(&HFFFF) As Byte
        Dim Palette15(&HFFF) As Byte
        Dim Palette32() As Color
        Dim bSprite As Bitmap
        Using fs As New FileStream(LoadedROM, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs)
                fs.Position = sOffset
                sOffset = r.ReadInt32 - &H8000000
                fs.Position = sOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Image)

                ReDim Temp(&HFFF)
                fs.Position = pOffset
                pOffset = r.ReadInt32 - &H8000000
                fs.Position = pOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Palette15)

                Palette32 = LoadPalette(Palette15)
            End Using
        End Using


        bSprite = LoadSprite(Image, Palette32, 64, 128, GetString(AppPath & "GBAPGESettings.ini", "Settings", "TransparentImages", "0"))
        picBox.Image = bSprite
        picBox.Refresh()

    End Sub

    Public Sub GetAndDrawBackShadowPokemonPic(ByVal picBox As PictureBox, ByVal index As Integer)
        Dim sOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "ShadowBacks", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon front sprites, + 8 = Bulbasaur.
        Dim pOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "ShadowPals", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon normal palettes, + 8 = Bulbasaur.
        Dim Temp(&HFFF) As Byte
        Dim Image(&HFFFF) As Byte
        Dim Palette15(&HFFF) As Byte
        Dim Palette32() As Color
        Dim bSprite As Bitmap
        Using fs As New FileStream(LoadedROM, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs)
                fs.Position = sOffset
                sOffset = r.ReadInt32 - &H8000000
                fs.Position = sOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Image)

                ReDim Temp(&HFFF)
                fs.Position = pOffset
                pOffset = r.ReadInt32 - &H8000000
                fs.Position = pOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Palette15)

                Palette32 = LoadPalette(Palette15)
            End Using
        End Using


        bSprite = LoadSprite(Image, Palette32, 64, 64, GetString(AppPath & "GBAPGESettings.ini", "Settings", "TransparentImages", "0"))
        picBox.Image = bSprite
        picBox.Refresh()

    End Sub

    Public Sub GetAndDrawShadowFrontPokemonPic(ByVal picBox As PictureBox, ByVal index As Integer)
        Dim sOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "ShadowFronts", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon front sprites, + 8 = Bulbasaur.
        Dim pOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "ShadowPals", ""), System.Globalization.NumberStyles.HexNumber) + (index * 8) 'Pointer to Pokemon normal palettes, + 8 = Bulbasaur.
        Dim Temp(&HFFF) As Byte
        Dim Image(&HFFFF) As Byte
        Dim Palette15(&HFFF) As Byte
        Dim Palette32() As Color
        Dim bSprite As Bitmap
        Using fs As New FileStream(LoadedROM, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs)
                fs.Position = sOffset
                sOffset = r.ReadInt32 - &H8000000
                fs.Position = sOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Image)

                ReDim Temp(&HFFF)
                fs.Position = pOffset
                pOffset = r.ReadInt32 - &H8000000
                fs.Position = pOffset
                r.Read(Temp, 0, &HFFF)
                LZ77UnComp(Temp, Palette15)

                Palette32 = LoadPalette(Palette15)
            End Using
        End Using


        bSprite = LoadSprite(Image, Palette32, 64, 64, GetString(AppPath & "GBAPGESettings.ini", "Settings", "TransparentImages", "0"))
        picBox.Image = bSprite
        picBox.Refresh()

    End Sub

    Public Function LoadPalette(ByVal Bits() As Byte) As Color()
        Dim Temp As UShort
        Dim Colors(15) As Color
        Dim C1 As Byte
        Dim C2 As Byte
        Dim R As UShort, G As UShort, B As UShort
        Dim i As Byte

        For i = 0 To &H1F Step 2
            C1 = Bits(i)
            C2 = Bits(i + 1)
            Temp = C2 * &H100 + C1

            R = (Temp And &H1F) * &H8
            G = (Temp And &H3E0) / &H4
            B = (Temp And &H7C00) / &H80

            Colors(i / 2) = Color.FromArgb(&HFF, R, G, B)
        Next

        LoadPalette = Colors
    End Function

    Public Function LoadSprite(ByRef Bits() As Byte, ByVal Palette() As Color, Optional ByVal Width As Integer = 64, Optional ByVal Height As Integer = 64, Optional ByVal ShowBackColor As Boolean = True) As Bitmap
        On Error GoTo ErrorHandle
        Dim x1 As Integer, y1 As Integer
        Dim x2 As Integer, y2 As Integer
        Dim bmpTiles As New Bitmap(Width, Height)
        Dim Temp As Byte
        Dim i As Integer

        For y1 = 0 To Height - 8 Step 8
            For x1 = 0 To Width - 8 Step 8
                For y2 = 0 To 7
                    For x2 = 0 To 7 Step 2
                        Temp = Bits(i)
                        If ShowBackColor = True Then
                            bmpTiles.SetPixel(x1 + x2 + 1, y1 + y2, Palette((Temp And &HF0) / &H10))
                            bmpTiles.SetPixel(x1 + x2, y1 + y2, Palette(Temp And &HF))
                        Else

                            ' If Temp And &HF0 <> 0 Then
                            If Palette((Temp And &HF0) / &H10) <> Palette(0) Then


                                'MsgBox(Temp And &HF0)
                                ' MsgBox("hit")
                                bmpTiles.SetPixel(x1 + x2 + 1, y1 + y2, Palette((Temp And &HF0) / &H10))

                            End If
                            If Palette((Temp And &HF)) <> Palette(0) Then
                                ' If Temp And &HF <> 0 Then
                                '  MsgBox("hit")

                                bmpTiles.SetPixel(x1 + x2, y1 + y2, Palette((Temp And &HF)))

                            End If
                        End If
                        i = i + 1
                    Next
                Next
            Next
        Next

        LoadSprite = bmpTiles
ErrorHandle:
    End Function

    Public Sub GetAndDrawPokemonIconPic(ByVal picBox As PictureBox, ByVal index As Integer, ByVal palindex As Integer)
        Dim sOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "IconPointerTable", ""), System.Globalization.NumberStyles.HexNumber) + (4 + (index * 4))
        Dim pOffset As Integer = Int32.Parse(GetString(AppPath & "ini\roms.ini", header, "IconPals", ""), System.Globalization.NumberStyles.HexNumber) + (palindex * 32)
        Dim Temp(&HFFF) As Byte
        Dim Image(&HFFFF) As Byte
        Dim Palette15(&HFFF) As Byte
        Dim Palette32() As Color
        Dim bSprite As Bitmap
        Using fs As New FileStream(LoadedROM, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs)
                fs.Position = sOffset
                sOffset = r.ReadInt32 - &H8000000
                fs.Position = sOffset
                r.Read(Temp, 0, &HFFF)
                'LZ77UnComp(Temp, Image)
                Image = Temp

                ReDim Temp(&HFFF)
                'fs.Position = pOffset
                'pOffset = r.ReadInt32 - &H8000000
                fs.Position = pOffset
                r.Read(Temp, 0, &HFFF)
                'LZ77UnComp(Temp, Palette15)

                Palette32 = LoadPalette(Temp)
            End Using
        End Using


        bSprite = LoadSprite(Image, Palette32, 32, 64, GetString(AppPath & "GBAPGESettings.ini", "Settings", "TransparentImages", "0"))
        picBox.Image = bSprite
        picBox.Refresh()

    End Sub
End Module