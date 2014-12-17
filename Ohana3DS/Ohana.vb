Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D

Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Globalization
Imports System.Text.RegularExpressions
Public Class Ohana

#Region "Declares"
    Private Device As Device

    Public Structure Data_Entry
        Dim Offset As Integer
        Dim Length As Integer
        Dim Format As Integer
    End Structure
    Public Structure OhanaVertex
        Dim X, Y, Z As Single
        Dim NX, NY, NZ As Single
        Dim Color As Integer
        Dim U, V As Single

        '---

        Dim Bone_1, Bone_2, Bone_3, Bone_4 As Integer
        Dim Weight_1, Weight_2, Weight_3, Weight_4 As Single
    End Structure
    Public Structure VertexList
        Dim Texture_ID As Integer

        Dim Vertex_Entry As Data_Entry
        Dim Vertice() As OhanaVertex

        Dim Index() As Integer
        Dim Per_Face_Entry As List(Of Data_Entry)
        Dim Per_Face_Index As List(Of Integer())
        Dim Texture_ID_Offset As Integer
    End Structure
    Public Structure OhanaTexture
        Dim Name As String
        Dim Image As Bitmap
        Dim Image_Mirrored As Bitmap
        Dim Texture As Texture
        Dim Has_Alpha As Boolean

        Dim Offset As Integer
        Dim Format As Integer
    End Structure
    Public Structure OhanaBone
        Dim Name As String
        Dim Parent_ID As Integer
        Dim Translation As Vector3
        Dim Rotation As Vector3
        Dim Scale As Vector3
    End Structure
    Public Enum ModelType
        Character
        Map
    End Enum
    Private Enum BCH_Version
        XY
        ORAS
    End Enum

    Public Model_Object() As VertexList
    Public Model_Texture As List(Of OhanaTexture)
    Public Model_Bone() As OhanaBone
    Public Model_Texture_Index() As String
    Public Model_Bump_Map_Index() As String

    Public Magic As String
    Public Model_Type As ModelType
    Public mapVals As UInteger(,)

    Public Scale As Single = 32.0F
    Public Model_Mirror_X As Boolean = True
    Public Lighting As Boolean = True
    Public Load_Scale As Single
    Public Load_Mirror As Boolean

    Public Structure OhanaInfo
        Dim Vertex_Count As Integer
        Dim Triangles_Count As Integer
        Dim Bones_Count As Integer
        Dim Textures_Count As Integer
    End Structure
    Public Info As OhanaInfo

    Private Total_Vertex As Integer

    Private SWidth, SHeight As Integer
    Public Zoom As Single = 1.0F
    Public Rotation As Vector2
    Public Translation As Vector2

    Public Max_Y_Neg, Max_Y_Pos, Max_Y As Single
    Private Has_Normals As Boolean

    Public Rendering As Boolean
    Public Current_Model As String
    Public Current_Texture As String
    Public Temp_Model_File As String
    Public Temp_Texture_File As String

    Public Selected_Object As Integer
    Public Selected_Face As Integer
    Public Edit_Mode As Boolean

    Private Tile_Order() As Integer = _
        {0, 1, 8, 9, 2, 3, 10, 11, _
         16, 17, 24, 25, 18, 19, 26, 27, _
         4, 5, 12, 13, 6, 7, 14, 15, _
         20, 21, 28, 29, 22, 23, 30, 31, _
         32, 33, 40, 41, 34, 35, 42, 43, _
         48, 49, 56, 57, 50, 51, 58, 59, _
         36, 37, 44, 45, 38, 39, 46, 47, _
         52, 53, 60, 61, 54, 55, 62, 63}
    Private Modulation_Table(,) As Integer = _
        {{2, 8, -2, -8}, _
        {5, 17, -5, -17}, _
        {9, 29, -9, -29}, _
        {13, 42, -13, -42}, _
        {18, 60, -18, -60}, _
        {24, 80, -24, -80}, _
        {33, 106, -33, -106}, _
        {47, 183, -47, -183}}
#End Region

#Region "DirectX Initialize"
    Public Sub Initialize(Picture As PictureBox)
        Dim Present As New PresentParameters
        With Present
            .BackBufferCount = 1
            .BackBufferFormat = Manager.Adapters(0).CurrentDisplayMode.Format
            .BackBufferWidth = Picture.Width
            .BackBufferHeight = Picture.Height
            SWidth = Picture.Width
            SHeight = Picture.Height
            .Windowed = True
            .SwapEffect = SwapEffect.Discard
            .EnableAutoDepthStencil = True
            .AutoDepthStencilFormat = DepthFormat.D16
            Dim Samples As MultiSampleType
            For Samples = MultiSampleType.SixteenSamples To MultiSampleType.None Step -1
                If Manager.CheckDeviceMultiSampleType(0, DeviceType.Hardware, Format.D16, True, Samples) Then Exit For
            Next
            .MultiSample = Samples
        End With

        Device = New Device(0, DeviceType.Hardware, Picture.Handle, CreateFlags.HardwareVertexProcessing, Present)
        With Device
            .RenderState.CullMode = Cull.None
            .RenderState.ZBufferEnable = True
            .RenderState.AlphaBlendEnable = True
            .RenderState.SourceBlend = Blend.SourceAlpha
            .RenderState.DestinationBlend = Blend.InvSourceAlpha
            .RenderState.BlendOperation = BlendOperation.Add

            .SamplerState(0).MinFilter = TextureFilter.Anisotropic
            .SamplerState(0).MagFilter = TextureFilter.Anisotropic
            .SamplerState(0).MaxAnisotropy = 16
        End With
    End Sub
#End Region

#Region "Model"
    Public Function Load_Model(File_Name As String, Optional Create_DX_Texture As Boolean = True) As Boolean
        Dim Version As BCH_Version

        Total_Vertex = 0
        Max_Y_Neg = 0
        Max_Y_Pos = 0
        Has_Normals = False
        Rendering = False
        Model_Object = Nothing
        Current_Model = Nothing
        If Temp_Model_File <> Nothing Then
            File.Delete(Temp_Model_File)
            Temp_Model_File = Nothing
        End If

        Load_Scale = Scale
        Load_Mirror = Model_Mirror_X

        Dim Temp() As Byte = File.ReadAllBytes(File_Name)
        Dim File_Magic As String = Nothing
        For i As Integer = 0 To 2
            File_Magic &= Chr(Temp(i))
        Next
        Magic = File_Magic
        Dim BCH_Offset As Integer
        If Left(File_Magic, 2) = "PC" Or Left(File_Magic, 2) = "MM" Then
            BCH_Offset = &H80
            Model_Type = ModelType.Character
        ElseIf File_Magic = "BCH" Then
            BCH_Offset = 0
            Model_Type = ModelType.Character
        ElseIf Left(File_Magic, 2) = "GR" Then
            BCH_Offset = Read32(Temp, 8)
            Model_Type = ModelType.Map
        Else

            Return False
        End If

        Current_Model = File_Name
        Temp_Model_File = Path.GetTempFileName
        File.WriteAllBytes(Temp_Model_File, Temp)

        Dim Data(Temp.Length - BCH_Offset) As Byte
        Buffer.BlockCopy(Temp, BCH_Offset, Data, 0, Temp.Length - BCH_Offset)

        Dim Header_Offset As Integer = Read32(Data, 8)
        If Header_Offset = &H44 Then Version = BCH_Version.ORAS Else Version = BCH_Version.XY

        Dim Texture_Names_Offset As Integer = Read32(Data, &HC)
        Dim Description_Offset As Integer = Read32(Data, &H10)
        Dim Data_Offset As Integer = Read32(Data, &H14)
        Dim Texture_Names_Length As Integer = Read32(Data, &H20)
        Dim Table_Offset As Integer = (Header_Offset + &H34) + Read32(Data, Header_Offset + Read32(Data, Header_Offset))

        Dim Texture_Entries As Integer = Read32(Data, Table_Offset + 4)
        Dim Bone_Entries As Integer = Read32(Data, Table_Offset + &H40)
        Dim Bones_Offset As Integer = Header_Offset + Read32(Data, Table_Offset + &H44)
        Dim Texture_Table_Offset As Integer
        Dim Vertex_Table_Offset As Integer = Read32(Data, Table_Offset + &HC)

        Dim Entries As Integer = Read32(Data, Table_Offset + &H10)

        Dim MM_Texture_Table_Offset As Integer = Header_Offset + Read32(Data, Header_Offset + &H24)
        Dim MM_Texture_Count As Integer = Read32(Data, Header_Offset + &H28)
        If Version = BCH_Version.XY Then
            Texture_Table_Offset = &H78 + Read32(Data, Table_Offset)
        ElseIf Version = BCH_Version.ORAS Then
            Texture_Table_Offset = &H48 + Read32(Data, Table_Offset)
        End If
        Table_Offset = Header_Offset + Read32(Data, Table_Offset + &H14)

        '+==========+
        '| Vertices |
        '+==========+
        Dim Vertex_Offsets As New List(Of Integer)
        Dim Face_Offsets As New List(Of Integer)
        For Entry As Integer = 0 To Entries - 1
            Dim Base_Offset As Integer = Table_Offset + (Entry * &H38)

            Dim Vertex_Offset As Integer = Description_Offset + Read32(Data, Base_Offset + 8)
            Vertex_Offsets.Add(Data_Offset + Read32(Data, Vertex_Offset + &H30))

            Dim Face_Offset As Integer = Read32(Data, Base_Offset + &H10)
            Face_Offset = Description_Offset + Read32(Data, Face_Offset + (Header_Offset + &H2C))
            Face_Offsets.Add(Data_Offset + Read32(Data, Face_Offset + &H10))
        Next
        Vertex_Offsets.Sort()
        Face_Offsets.Sort()
        Vertex_Offsets.Add(Face_Offsets(0))

        Dim Texture_ID_List As New List(Of Integer)
        ReDim Model_Object(Entries - 1)
        Dim Vertex_Count As Integer
        For Entry As Integer = 0 To Entries - 1
            Dim Base_Offset As Integer = Table_Offset + (Entry * &H38)
            Dim Texture_ID As Integer = Read16(Data, Base_Offset)
            If Not Texture_ID_List.Contains(Texture_ID) Then Texture_ID_List.Add(Texture_ID)
            Dim Vertex_Offset As Integer = Description_Offset + Read32(Data, Base_Offset + 8)
            Dim Face_Offset As Integer = Read32(Data, Base_Offset + &H10)
            Dim Face_Count As Integer = Read32(Data, Base_Offset + &H14)
            Dim Faces(Face_Count - 1) As Integer
            For Index As Integer = 0 To Face_Count - 1
                Faces(Index) = Description_Offset + Read32(Data, Face_Offset + (Header_Offset + &H2C) + (Index * &H34))
            Next

            Face_Offset = Description_Offset + Read32(Data, Face_Offset + (Header_Offset + &H2C))
            Dim Vertex_Data_Offset As Integer = Data_Offset + Read32(Data, Vertex_Offset + &H30)

            Dim Vertex_Data_Format As Integer = Data(Vertex_Offset + &H3A)
            Dim Vertex_Flags As Integer = Read32(Data, Face_Offset)
            Dim Vertex_Data_Length As Integer
            If Version = BCH_Version.XY Then
                Dim Face_Data_Offset As Integer = Data_Offset + Read32(Data, Face_Offset + &H10)
                Vertex_Data_Length = Face_Data_Offset - Vertex_Data_Offset
            ElseIf Version = BCH_Version.ORAS Then
                Vertex_Data_Length = Vertex_Offsets(Vertex_Offsets.IndexOf(Vertex_Data_Offset) + 1) - Vertex_Data_Offset
            End If

            Dim Index_List As New List(Of Integer)
            Model_Object(Entry).Per_Face_Entry = New List(Of Data_Entry)
            Model_Object(Entry).Per_Face_Index = New List(Of Integer())
            If Entry = Entries - 1 Then
                Dim Face_Total_Length As Integer = 0
                For Each Face As Integer In Faces
                    Face_Total_Length += Read32(Data, Face + &H18)
                Next
                Dim Temp_Length As Integer = Face_Total_Length * Vertex_Data_Format
                If Temp_Length < Vertex_Data_Length Then Vertex_Data_Length = Temp_Length
            End If
            Dim Count As Integer = Vertex_Data_Length \ Vertex_Data_Format
            For Each Face As Integer In Faces
                Dim Face_Data_Offset As Integer = Data_Offset + Read32(Data, Face + &H10)
                Dim Face_Data_Length As Integer = Read32(Data, Face + &H18)
                Dim Face_Data_Format As Integer = 2

                Dim Temp_Offset As Integer = Face_Data_Offset
                For Index As Integer = 0 To Face_Data_Length - 1 Step 3
                    Dim Temp_1 As Integer = Convert.ToInt32(Read16(Data, Temp_Offset))
                    Dim Temp_2 As Integer = Convert.ToInt32(Read16(Data, Temp_Offset + 2))
                    Dim Temp_3 As Integer = Convert.ToInt32(Read16(Data, Temp_Offset + 4))

                    If Temp_1 > Count Or Temp_2 > Count Or Temp_3 > Count Then
                        Face_Data_Format = 1
                        Exit For
                    End If
                    Temp_Offset += 6
                Next

                Dim Face_Entry As Data_Entry
                Face_Entry.Offset = Face_Data_Offset + BCH_Offset
                Face_Entry.Length = Face_Data_Length * Face_Data_Format
                Face_Entry.Format = Face_Data_Format
                Model_Object(Entry).Per_Face_Entry.Add(Face_Entry)

                Dim CurrOffset As Integer = Face_Data_Offset
                Dim Per_Face_Index As New List(Of Integer)
                For Index As Integer = 0 To Face_Data_Length - 1 Step 3
                    If Face_Data_Format = 2 Then
                        Per_Face_Index.Add(Convert.ToInt32(Read16(Data, CurrOffset)))
                        Per_Face_Index.Add(Convert.ToInt32(Read16(Data, CurrOffset + 2)))
                        Per_Face_Index.Add(Convert.ToInt32(Read16(Data, CurrOffset + 4)))
                    Else
                        Per_Face_Index.Add(Data(CurrOffset))
                        Per_Face_Index.Add(Data(CurrOffset + 1))
                        Per_Face_Index.Add(Data(CurrOffset + 2))
                    End If
                    CurrOffset += 3 * Face_Data_Format
                    Total_Vertex += 3
                Next
                Index_List.AddRange(Per_Face_Index)
                Model_Object(Entry).Per_Face_Index.Add(Per_Face_Index.ToArray())
            Next

            ReDim Model_Object(Entry).Vertice(Count - 1)
            Dim Offset As Integer = Vertex_Data_Offset
            For Index As Integer = 0 To Count - 1
                With Model_Object(Entry).Vertice(Index)
                    .X = (BitConverter.ToSingle(Data, Offset) / Scale) * If(Model_Mirror_X, -1, 1)
                    .Y = BitConverter.ToSingle(Data, Offset + 4) / Scale
                    .Z = BitConverter.ToSingle(Data, Offset + 8) / Scale

                    .Color = &HFFFFFFFF
                    Select Case Vertex_Data_Format
                        Case &H10
                            .Color = Read32(Data, Offset + 12)
                        Case &H14, &H18, &H1C
                            .U = BitConverter.ToSingle(Data, Offset + 12)
                            .V = BitConverter.ToSingle(Data, Offset + 16)
                        Case &H20, &H30, &H38
                            .NX = BitConverter.ToSingle(Data, Offset + 12) / Scale
                            .NY = BitConverter.ToSingle(Data, Offset + 16) / Scale
                            .NZ = BitConverter.ToSingle(Data, Offset + 20) / Scale

                            .U = BitConverter.ToSingle(Data, Offset + 24)
                            .V = BitConverter.ToSingle(Data, Offset + 28)

                            Has_Normals = True
                        Case &H24, &H28, &H2C
                            .NX = BitConverter.ToSingle(Data, Offset + 12) / Scale
                            .NY = BitConverter.ToSingle(Data, Offset + 16) / Scale
                            .NZ = BitConverter.ToSingle(Data, Offset + 20) / Scale

                            .U = BitConverter.ToSingle(Data, Offset + 24)
                            .V = BitConverter.ToSingle(Data, Offset + 28)

                            If Vertex_Data_Format = &H24 Then
                                Select Case Vertex_Flags And &HFFFF
                                    Case &H8E81, &H8E82, &HAA83, &HAADB, &HAC81, &HAE83
                                        .Color = Read32(Data, Offset + 32)
                                End Select
                            Else
                                Select Case Vertex_Flags And &HFFFF
                                    Case &HAA83, &HAB83, &HAE83, &HAF83, &HEF83, &HEFD3
                                        .Color = Read32(Data, Offset + 32)
                                End Select
                            End If

                            Has_Normals = True
                        Case &H34
                            .NX = BitConverter.ToSingle(Data, Offset + 12) / Scale
                            .NY = BitConverter.ToSingle(Data, Offset + 16) / Scale
                            .NZ = BitConverter.ToSingle(Data, Offset + 20) / Scale

                            .U = BitConverter.ToSingle(Data, Offset + 24)
                            .V = BitConverter.ToSingle(Data, Offset + 28)

                            Has_Normals = True
                    End Select

                    If Model_Mirror_X Then .NX *= -1

                    If .Y > Max_Y_Pos Then Max_Y_Pos = .Y
                    If .Y < Max_Y_Neg Then Max_Y_Neg = .Y
                End With

                Vertex_Count += 1
                Offset += Vertex_Data_Format
            Next

            With Model_Object(Entry)
                .Index = Index_List.ToArray
                .Texture_ID = Texture_ID
                .Texture_ID_Offset = Base_Offset + BCH_Offset

                .Vertex_Entry.Offset = Vertex_Data_Offset + BCH_Offset
                .Vertex_Entry.Length = Vertex_Data_Length
                .Vertex_Entry.Format = Vertex_Data_Format
            End With
        Next
        Max_Y = Math.Abs(Max_Y_Pos - Max_Y_Neg)

        '+==========+
        '| Texturas |
        '+==========+
        Dim Name_Table_Base_Pointer As Integer
        Dim Name_Table_Length As Integer
        If Version = BCH_Version.XY Then
            If Model_Type = ModelType.Character Then
                Name_Table_Base_Pointer = 8
            ElseIf Model_Type = ModelType.Map Then
                Name_Table_Base_Pointer = &H14
            End If
            Name_Table_Length = &H58
        ElseIf Version = BCH_Version.ORAS Then
            Name_Table_Base_Pointer = &H18
            Name_Table_Length = &H2C
        End If

        If Left(File_Magic, 2) = "MM" Or File_Magic = "BCH" Then
            Current_Texture = File_Name
            If Temp_Texture_File <> Nothing Then File.Delete(Temp_Texture_File)
            Temp_Texture_File = Path.GetTempFileName
            File.WriteAllBytes(Temp_Texture_File, Temp)

            Model_Texture = New List(Of OhanaTexture)

            ReDim Model_Texture_Index(Texture_Entries - 1)
            ReDim Model_Bump_Map_Index(Texture_Entries - 1)
            For Index = 0 To MM_Texture_Count - 1
                Dim Name_Offset As Integer = Texture_Names_Offset + Read32(Data, (Header_Offset + Read32(Data, MM_Texture_Table_Offset + (Index * 4))) + &H1C)
                Dim Texture_Name As String = Nothing
                Do
                    Dim Value As Integer = Data(Name_Offset)
                    Name_Offset += 1
                    If Value <> 0 Then Texture_Name &= Chr(Value) Else Exit Do
                Loop

                If Index < Texture_Entries Then
                    Dim Model_Texture_Name_Offset As Integer = Texture_Names_Offset + Read32(Data, (Texture_Table_Offset + Index * Name_Table_Length) + Name_Table_Base_Pointer + If(File_Magic = "BCH", 0, 4))
                    Dim Model_Texture_Name As String = Nothing
                    Do
                        Dim Value As Integer = Data(Model_Texture_Name_Offset)
                        Model_Texture_Name_Offset += 1
                        If Value <> 0 Then Model_Texture_Name &= Chr(Value) Else Exit Do
                    Loop

                    If Model_Texture_Name <> Nothing Then
                        Model_Texture_Index(Index) = Model_Texture_Name
                    Else 'Workaround
                        Model_Texture_Index(Index) = Texture_Name
                    End If
                End If

                Dim Texture_Description As Integer = Description_Offset + Read32(Data, Header_Offset + Read32(Data, MM_Texture_Table_Offset + (Index * 4)))
                Dim Height As Integer = Read16(Data, Texture_Description)
                Dim Width As Integer = Read16(Data, Texture_Description + 2)
                Dim Texture_Offset As Integer = Data_Offset + Read32(Data, Texture_Description + If(Version = BCH_Version.XY, 8, &H10))
                Dim Texture_Format As Integer = Read32(Data, Texture_Description + If(Version = BCH_Version.XY, &H10, &H18))

                Dim Out() As Byte = Convert_Texture(Data, Texture_Offset, Texture_Format, Width, Height)

                Dim MyTex As New OhanaTexture
                Dim Img As New Bitmap(Width, Height, Imaging.PixelFormat.Format32bppArgb)
                Dim ImgData As BitmapData = Img.LockBits(New Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)
                Marshal.Copy(Out, 0, ImgData.Scan0, Out.Length)
                Img.UnlockBits(ImgData)
                MyTex.Image = Img
                MyTex.Image.RotateFlip(RotateFlipType.RotateNoneFlipY)

                Dim Texture As Texture = Nothing
                If Create_DX_Texture Then
                    Texture = New Texture(Device, Width, Height, 1, Usage.None, Direct3D.Format.A8R8G8B8, Pool.Managed)
                    Dim pData As GraphicsStream = Texture.LockRectangle(0, LockFlags.None)
                    pData.Write(Out, 0, Out.Length)
                    Texture.UnlockRectangle(0)
                End If

                With MyTex
                    If Create_DX_Texture Then .Texture = Texture
                    .Name = Texture_Name
                    .Has_Alpha = Check_Alpha(Out)
                    .Offset = Texture_Offset + BCH_Offset
                    .Format = Texture_Format
                End With
                Model_Texture.Add(MyTex)
            Next
        Else
            ReDim Model_Texture_Index(Texture_Entries - 1)
            ReDim Model_Bump_Map_Index(Texture_Entries - 1)
            For Index As Integer = 0 To Texture_Entries - 1
                Dim Texture_Offset As Integer = Texture_Names_Offset + Read32(Data, (Texture_Table_Offset + Index * Name_Table_Length) + Name_Table_Base_Pointer)
                Dim Normal_Offset As Integer = Texture_Names_Offset + Read32(Data, (Texture_Table_Offset + Index * Name_Table_Length) + Name_Table_Base_Pointer + 8)

                'Textura
                Dim Texture_Name As String = Nothing
                Do
                    Dim Value As Integer = Data(Texture_Offset)
                    Texture_Offset += 1
                    If Value <> 0 Then Texture_Name &= Chr(Value) Else Exit Do
                Loop
                Model_Texture_Index(Index) = Texture_Name

                If Model_Type = ModelType.Character Then
                    'Mapa de Normals/Bump Map
                    Dim Normal_Name As String = Nothing
                    Do
                        Dim Value As Integer = Data(Normal_Offset)
                        Normal_Offset += 1
                        If Value <> 0 Then Normal_Name &= Chr(Value) Else Exit Do
                    Loop
                    Model_Bump_Map_Index(Index) = Normal_Name
                End If
            Next
        End If

        '+=======+
        '| Bones |
        '+=======+
        ReDim Model_Bone(Bone_Entries - 1)
        Bones_Offset += (Bone_Entries * &HC) + &HC
        For Bone As Integer = 0 To Bone_Entries - 1
            Dim Bone_Parent As Integer = Signed_Short(Read16(Data, Bones_Offset + 4))
            Dim Bone_Name_Offset As Integer = Read32(Data, Bones_Offset + 92)

            Dim MyBone As OhanaBone
            With MyBone
                .Name = Nothing
                Do
                    Dim Value As Integer = Data(Texture_Names_Offset + Bone_Name_Offset)
                    Bone_Name_Offset += 1
                    If Value <> 0 Then .Name &= Chr(Value) Else Exit Do
                Loop
                .Parent_ID = Bone_Parent

                .Translation.X = BitConverter.ToSingle(Data, Bones_Offset + 32) / Scale
                .Translation.Y = BitConverter.ToSingle(Data, Bones_Offset + 36) / Scale
                .Translation.Z = BitConverter.ToSingle(Data, Bones_Offset + 40) / Scale

                .Rotation.X = BitConverter.ToSingle(Data, Bones_Offset + 20)
                .Rotation.Y = BitConverter.ToSingle(Data, Bones_Offset + 24)
                .Rotation.Z = BitConverter.ToSingle(Data, Bones_Offset + 28)

                .Scale.X = BitConverter.ToSingle(Data, Bones_Offset + 8)
                .Scale.Y = BitConverter.ToSingle(Data, Bones_Offset + 12)
                .Scale.Z = BitConverter.ToSingle(Data, Bones_Offset + 16)
            End With
            Model_Bone(Bone) = MyBone

            Dim Bone_Mtx As New Matrix

            Bone_Mtx.M11 = BitConverter.ToSingle(Data, Bones_Offset + 44)
            Bone_Mtx.M12 = BitConverter.ToSingle(Data, Bones_Offset + 48)
            Bone_Mtx.M13 = BitConverter.ToSingle(Data, Bones_Offset + 52)
            Bone_Mtx.M14 = BitConverter.ToSingle(Data, Bones_Offset + 56)

            Bone_Mtx.M21 = BitConverter.ToSingle(Data, Bones_Offset + 60)
            Bone_Mtx.M22 = BitConverter.ToSingle(Data, Bones_Offset + 64)
            Bone_Mtx.M23 = BitConverter.ToSingle(Data, Bones_Offset + 68)
            Bone_Mtx.M24 = BitConverter.ToSingle(Data, Bones_Offset + 72)

            Bone_Mtx.M31 = BitConverter.ToSingle(Data, Bones_Offset + 76)
            Bone_Mtx.M32 = BitConverter.ToSingle(Data, Bones_Offset + 80)
            Bone_Mtx.M33 = BitConverter.ToSingle(Data, Bones_Offset + 84)
            Bone_Mtx.M34 = BitConverter.ToSingle(Data, Bones_Offset + 88)

            Bones_Offset += 100
        Next

        Dim TempLst As New List(Of String)
        For Index As Integer = 0 To Model_Texture_Index.Length - 1
            If Not TempLst.Contains(Model_Texture_Index(Index)) Then TempLst.Add(Model_Texture_Index(Index))
        Next
        With Info
            .Vertex_Count = Vertex_Count
            .Triangles_Count = Total_Vertex \ 3
            .Bones_Count = Bone_Entries
            .Textures_Count = Texture_ID_List.Count - (Texture_Entries - TempLst.Count)
        End With

        Return True
    End Function

    Public Sub Export_SMD(File_Name As String)
        Dim Info As New NumberFormatInfo
        Info.NumberDecimalSeparator = "."
        Info.NumberDecimalDigits = 6

        Dim Out As New StringBuilder
        Out.AppendLine("version 1")
        Out.AppendLine("nodes")
        Dim Node_Index As Integer
        For Each Bone As OhanaBone In Model_Bone
            With Bone
                Out.AppendLine(Node_Index & " """ & Bone.Name & """ " & Bone.Parent_ID)
                Node_Index += 1
            End With
        Next
        Out.AppendLine("end")
        Out.AppendLine("skeleton")
        Out.AppendLine("time 0")
        Dim Bone_Index As Integer
        For Each Bone As OhanaBone In Model_Bone
            With Bone
                Out.AppendLine(Bone_Index & _
                               " " & .Translation.X.ToString("N", Info) & " " & .Translation.Y.ToString("N", Info) & " " & .Translation.Z.ToString("N", Info) & _
                               " " & .Rotation.X.ToString("N", Info) & " " & .Rotation.Y.ToString("N", Info) & " " & .Rotation.Z.ToString("N", Info))
                Bone_Index += 1
            End With
        Next
        Out.AppendLine("end")
        Out.AppendLine("triangles")
        Dim Temp_Count As Integer
        For Each Model As VertexList In Model_Object
            With Model
                For Each Index As Integer In .Index
                    If Temp_Count = 0 Then Out.AppendLine(Model_Texture_Index(.Texture_ID) & ".png")
                    Dim CurrVert As OhanaVertex
                    If Index < .Vertice.Length Then CurrVert = .Vertice(Index)

                    Dim Bone_Info As String = Nothing
                    Dim Links As Integer = 0
                    If CurrVert.Weight_1 > 0 Then
                        Links = 1
                        Bone_Info = " " & CurrVert.Bone_1 & " " & CurrVert.Weight_1.ToString(Info)
                    End If
                    If CurrVert.Weight_2 > 0 Then
                        Links += 1
                        Bone_Info &= " " & CurrVert.Bone_2 & " " & CurrVert.Weight_2.ToString(Info)
                    End If
                    If CurrVert.Weight_3 > 0 Then
                        Links += 1
                        Bone_Info &= " " & CurrVert.Bone_3 & " " & CurrVert.Weight_3.ToString(Info)
                    End If
                    If CurrVert.Weight_4 > 0 Then
                        Links += 1
                        Bone_Info &= " " & CurrVert.Bone_4 & " " & CurrVert.Weight_4.ToString(Info)
                    End If
                    Bone_Info = Links & Bone_Info

                    Out.AppendLine(Model_Bone.Length & " " & _
                                   CurrVert.X.ToString(Info) & " " & CurrVert.Y.ToString(Info) & " " & CurrVert.Z.ToString(Info) & " " & _
                                   CurrVert.NX.ToString(Info) & " " & CurrVert.NY.ToString(Info) & " " & CurrVert.NZ.ToString(Info) & " " & _
                                   CurrVert.U.ToString(Info) & " " & CurrVert.V.ToString(Info) & " " & Bone_Info)
                    If Temp_Count < 2 Then Temp_Count += 1 Else Temp_Count = 0
                Next
            End With
        Next
        Out.AppendLine("end")

        File.WriteAllText(File_Name, Out.ToString)
    End Sub
#End Region

#Region "Textures"
    Public Sub Load_Textures(File_Name As String, Optional Create_DX_Texture As Boolean = True)
        Dim Version As BCH_Version

        Dim Data() As Byte = File.ReadAllBytes(File_Name)
        Current_Texture = File_Name
        If Temp_Texture_File <> Nothing Then File.Delete(Temp_Texture_File)
        Temp_Texture_File = Path.GetTempFileName
        File.WriteAllBytes(Temp_Texture_File, Data)

        Dim CLIM_Magic As String = Nothing
        For i As Integer = Data.Length - &H28 To Data.Length - &H25
            CLIM_Magic &= Chr(Data(i))
        Next

        If CLIM_Magic = "CLIM" Then
            Model_Texture = New List(Of OhanaTexture)

            Dim Header_Offset As Integer = Data.Length - &H14
            Dim Index As Integer
            While Header_Offset > 0
                Dim Image_Magic As String = Nothing
                For i As Integer = Header_Offset To Header_Offset + 3
                    Image_Magic &= Chr(Data(i))
                Next
                If Image_Magic <> "imag" Then Exit While

                Dim Width As Integer = Read16(Data, Header_Offset + 8)
                Width = Convert.ToInt32(Math.Pow(2, Math.Ceiling(Math.Log(Width) / Math.Log(2)))) 'Arredonda para o mais próximo 2^n
                Dim Height As Integer = Read16(Data, Header_Offset + 10)
                Height = Convert.ToInt32(Math.Pow(2, Math.Ceiling(Math.Log(Height) / Math.Log(2)))) 'Arredonda para o mais próximo 2^n
                Dim Format As Integer = Read32(Data, Header_Offset + 12)
                Dim Length As Integer = Read32(Data, Header_Offset + 16)
                Dim Offset As Integer = Header_Offset - &H14 - Length

                Dim Actual_Format As Integer
                Select Case Format
                    Case 0 : Actual_Format = 7
                    Case 1 : Actual_Format = 8
                    Case 2 : Actual_Format = 9
                    Case 3 : Actual_Format = 5
                    Case 4 : Actual_Format = 6
                    Case 5 : Actual_Format = 3
                    Case 6 : Actual_Format = 1
                    Case 7 : Actual_Format = 2
                    Case 8 : Actual_Format = 4
                    Case 9 : Actual_Format = 0
                    Case 10 : Actual_Format = 12
                    Case 11 : Actual_Format = 13
                    Case 12 : Actual_Format = 10
                End Select

                Dim Out() As Byte = Convert_Texture(Data, Offset, Actual_Format, Width, Height)

                Dim MyTex As New OhanaTexture
                Dim Img As New Bitmap(Width, Height, Imaging.PixelFormat.Format32bppArgb)
                Dim ImgData As BitmapData = Img.LockBits(New Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)
                Marshal.Copy(Out, 0, ImgData.Scan0, Out.Length)
                Img.UnlockBits(ImgData)
                MyTex.Image = Img
                MyTex.Image.RotateFlip(RotateFlipType.RotateNoneFlipY)

                Dim Texture As Texture = Nothing
                If Create_DX_Texture Then
                    Texture = New Texture(Device, Width, Height, 1, Usage.None, Direct3D.Format.A8R8G8B8, Pool.Managed)
                    Dim pData As GraphicsStream = Texture.LockRectangle(0, LockFlags.None)
                    pData.Write(Out, 0, Out.Length)
                    Texture.UnlockRectangle(0)
                End If

                With MyTex
                    If Create_DX_Texture Then .Texture = Texture
                    .Name = "bclim_" & Index
                    Select Case Format
                        Case 0, 2, 4, 5, 13
                            .Has_Alpha = True
                    End Select

                    .Offset = Offset
                    .Format = Actual_Format
                End With
                Model_Texture.Add(MyTex)

                Header_Offset -= (Length + &H80)
                Index += 1
            End While
        Else
            Dim File_Magic As String = Nothing
            For i As Integer = 0 To 1
                File_Magic &= Chr(Data(i))
            Next
            Dim BCH_Table_Offset As Integer = If(File_Magic = "PT", 4, 8)
            Dim BCH_Offset As Integer = Read32(Data, BCH_Table_Offset)

            Dim Header_Offset As Integer = Read32(Data, BCH_Offset + 8)
            If Header_Offset = &H44 Then Version = BCH_Version.ORAS Else Version = BCH_Version.XY

            Dim Texture_Description_Length As Integer
            Dim Desc_Texture_Pointer As Integer
            Dim Desc_Texture_Format As Integer
            If Version = BCH_Version.XY Then
                Texture_Description_Length = &H20
                Desc_Texture_Pointer = 8
                Desc_Texture_Format = &H10
            ElseIf Version = BCH_Version.ORAS Then
                Texture_Description_Length = &H30
                Desc_Texture_Pointer = &H10
                Desc_Texture_Format = &H18
            End If

            Model_Texture = New List(Of OhanaTexture)
            While BCH_Offset > 0
                If BCH_Offset = Data.Length Then Exit While
                Dim Magic As String = Nothing
                For i As Integer = 0 To 2
                    Magic &= Chr(Data(BCH_Offset + i))
                Next

                If Magic = "BCH" Then
                    Dim Texture_Names_Offset As Integer = BCH_Offset + Read32(Data, BCH_Offset + &HC)
                    Dim Description_Offset As Integer = BCH_Offset + Read32(Data, BCH_Offset + &H10)
                    Dim Data_Offset As Integer = BCH_Offset + Read32(Data, BCH_Offset + &H14)
                    Dim Texture_Count As Integer
                    If Version = BCH_Version.XY Then
                        Texture_Count = Read32(Data, BCH_Offset + &H60)
                    ElseIf Version = BCH_Version.ORAS Then
                        Texture_Count = Read32(Data, BCH_Offset + &H6C)
                    End If

                    Dim Texture_Names(Texture_Count - 1) As String
                    Dim Tmp As Integer = 0
                    For i As Integer = 0 To Texture_Count - 1
                        Dim Str As String = Nothing
                        Do
                            Dim Value As Integer = Data(Texture_Names_Offset + Tmp)
                            Tmp += 1
                            If Value <> 0 Then Str &= Chr(Value) Else Exit Do
                        Loop
                        Texture_Names(i) = Str
                    Next

                    Dim Index As Integer = 0
                    While Index < Texture_Count
                        If Read32(Data, Description_Offset + Desc_Texture_Pointer - Texture_Description_Length) <> Read32(Data, Description_Offset + Desc_Texture_Pointer) Then
                            Dim Width As Integer = Read16(Data, Description_Offset + 2)
                            Dim Height As Integer = Read16(Data, Description_Offset)
                            Dim Format As Integer = Data(Description_Offset + Desc_Texture_Format)
                            Dim Texture_Data_Offset As Integer = Data_Offset + Read32(Data, Description_Offset + Desc_Texture_Pointer)

                            Dim Out() As Byte = Convert_Texture(Data, Texture_Data_Offset, Format, Width, Height)

                            Dim MyTex As New OhanaTexture
                            Dim Img As New Bitmap(Width, Height, Imaging.PixelFormat.Format32bppArgb)
                            Dim ImgData As BitmapData = Img.LockBits(New Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)
                            Marshal.Copy(Out, 0, ImgData.Scan0, Out.Length)
                            Img.UnlockBits(ImgData)
                            MyTex.Image = Img
                            MyTex.Image.RotateFlip(RotateFlipType.RotateNoneFlipY)

                            If File_Magic = "PT" Then
                                Out = Mirror_Texture(Out, Width, Height)
                                Width *= 2
                            End If

                            Dim Texture As Texture = Nothing
                            If Create_DX_Texture Then
                                Texture = New Texture(Device, Width, Height, 1, Usage.None, Direct3D.Format.A8R8G8B8, Pool.Managed)
                                Dim pData As GraphicsStream = Texture.LockRectangle(0, LockFlags.None)
                                pData.Write(Out, 0, Out.Length)
                                Texture.UnlockRectangle(0)
                            End If

                            With MyTex
                                If Create_DX_Texture Then .Texture = Texture
                                .Name = Texture_Names(Index)
                                .Has_Alpha = Check_Alpha(Out)
                                .Offset = Texture_Data_Offset
                                .Format = Format
                            End With
                            Model_Texture.Add(MyTex)

                            Index += 1
                        End If

                        Description_Offset += Texture_Description_Length
                    End While
                End If

                If File_Magic = "PT" Then
                    If BCH_Table_Offset < 8 Then BCH_Table_Offset += 4 Else Exit While
                Else
                    BCH_Table_Offset += &H10
                End If

                BCH_Offset = Read32(Data, BCH_Table_Offset)
            End While
        End If
    End Sub

    Private Function Convert_Texture(Data() As Byte, Texture_Data_Offset As Integer, Format As Integer, Width As Integer, Height As Integer, Optional Linear As Boolean = False) As Byte()
        Dim Out((Width * Height * 4) - 1) As Byte
        Dim Offset As Integer = Texture_Data_Offset
        Dim Low_High_Toggle As Boolean = False

        If Format = 12 Or Format = 13 Then 'ETC1 (iPACKMAN)
            Dim Temp_Buffer(((Width * Height) \ 2) - 1) As Byte
            Dim Alphas(Temp_Buffer.Length - 1) As Byte
            If Format = 12 Then
                Buffer.BlockCopy(Data, Offset, Temp_Buffer, 0, Temp_Buffer.Length)
                For j As Integer = 0 To Alphas.Length - 1
                    Alphas(j) = &HFF
                Next
            Else
                Dim k As Integer = 0
                For j As Integer = 0 To (Width * Height) - 1
                    Buffer.BlockCopy(Data, Offset + j + 8, Temp_Buffer, k, 8)
                    Buffer.BlockCopy(Data, Offset + j, Alphas, k, 8)
                    k += 8
                    j += 15
                Next
            End If
            Dim Temp_2() As Byte = ETC1_Decompress(Temp_Buffer, Alphas, Width, Height)

            'Os tiles com compressão ETC1 no 3DS estão embaralhados
            Dim Tile_Scramble() As Integer = Get_ETC1_Scramble(Width, Height)

            Dim i As Integer = 0
            For Tile_Y As Integer = 0 To (Height \ 4) - 1
                For Tile_X As Integer = 0 To (Width \ 4) - 1
                    Dim TX As Integer = Tile_Scramble(i) Mod (Width \ 4)
                    Dim TY As Integer = (Tile_Scramble(i) - TX) \ (Width \ 4)
                    For Y As Integer = 0 To 3
                        For X As Integer = 0 To 3
                            Dim Out_Offset As Integer = ((Tile_X * 4) + X + (((Height - 1) - ((Tile_Y * 4) + Y)) * Width)) * 4
                            Dim Image_Offset As Integer = ((TX * 4) + X + (((TY * 4) + Y) * Width)) * 4

                            Out(Out_Offset) = Temp_2(Image_Offset)
                            Out(Out_Offset + 1) = Temp_2(Image_Offset + 1)
                            Out(Out_Offset + 2) = Temp_2(Image_Offset + 2)
                            Out(Out_Offset + 3) = Temp_2(Image_Offset + 3)
                        Next
                    Next
                    i += 1
                Next
            Next
        Else
            For Tile_Y As Integer = 0 To (Height \ 8) - 1
                For Tile_X As Integer = 0 To (Width \ 8) - 1
                    For i As Integer = 0 To 63
                        Dim X As Integer = Tile_Order(i) Mod 8
                        Dim Y As Integer = (Tile_Order(i) - X) \ 8
                        Dim Out_Offset As Integer = ((Tile_X * 8) + X + (((Height - 1) - ((Tile_Y * 8) + Y)) * Width)) * 4
                        Select Case Format
                            Case 0 'R8G8B8A8
                                Buffer.BlockCopy(Data, Offset + 1, Out, Out_Offset, 3)
                                Out(Out_Offset + 3) = Data(Offset)
                                Offset += 4
                            Case 1 'R8G8B8 (sem transparência)
                                Buffer.BlockCopy(Data, Offset, Out, Out_Offset, 3)
                                Out(Out_Offset + 3) = &HFF
                                Offset += 3
                            Case 2 'R5G5B5A1
                                Dim Pixel_Data As Integer = Read16(Data, Offset)
                                Out(Out_Offset + 2) = Convert.ToByte(((Pixel_Data >> 11) And &H1F) * 8)
                                Out(Out_Offset + 1) = Convert.ToByte(((Pixel_Data >> 6) And &H1F) * 8)
                                Out(Out_Offset) = Convert.ToByte(((Pixel_Data >> 1) And &H1F) * 8)
                                Out(Out_Offset + 3) = Convert.ToByte((Pixel_Data And 1) * &HFF)
                                Offset += 2
                            Case 3 'R5G6B5
                                Dim Pixel_Data As Integer = Read16(Data, Offset)
                                Out(Out_Offset + 2) = Convert.ToByte(((Pixel_Data >> 11) And &H1F) * 8)
                                Out(Out_Offset + 1) = Convert.ToByte(((Pixel_Data >> 5) And &H3F) * 4)
                                Out(Out_Offset) = Convert.ToByte(((Pixel_Data) And &H1F) * 8)
                                Out(Out_Offset + 3) = &HFF
                                Offset += 2
                            Case 4 'R4G4B4A4
                                Dim Pixel_Data As Integer = Read16(Data, Offset)
                                Out(Out_Offset) = Convert.ToByte(((Pixel_Data >> 12) And &HF) * &H11)
                                Out(Out_Offset + 1) = Convert.ToByte(((Pixel_Data >> 8) And &HF) * &H11)
                                Out(Out_Offset + 2) = Convert.ToByte(((Pixel_Data >> 4) And &HF) * &H11)
                                Out(Out_Offset + 3) = Convert.ToByte((Pixel_Data And &HF) * &H11)
                                Offset += 2
                            Case 5 'L8A8
                                Dim Pixel_Data As Byte = Data(Offset + 1)
                                Out(Out_Offset) = Pixel_Data
                                Out(Out_Offset + 1) = Pixel_Data
                                Out(Out_Offset + 2) = Pixel_Data
                                Out(Out_Offset + 3) = Data(Offset)
                                Offset += 2
                            Case 6 'HILO8
                            Case 7 'L8
                                Out(Out_Offset) = Data(Offset)
                                Out(Out_Offset + 1) = Data(Offset)
                                Out(Out_Offset + 2) = Data(Offset)
                                Out(Out_Offset + 3) = &HFF
                                Offset += 1
                            Case 8 'A8
                                Out(Out_Offset) = &HFF
                                Out(Out_Offset + 1) = &HFF
                                Out(Out_Offset + 2) = &HFF
                                Out(Out_Offset + 3) = Data(Offset)
                                Offset += 1
                            Case 9 'L4A4
                                Dim Luma As Integer = Data(Offset) And &HF
                                Dim Alpha As Integer = (Data(Offset) And &HF0) >> 4
                                Out(Out_Offset) = Convert.ToByte((Luma << 4) + Luma)
                                Out(Out_Offset + 1) = Convert.ToByte((Luma << 4) + Luma)
                                Out(Out_Offset + 2) = Convert.ToByte((Luma << 4) + Luma)
                                Out(Out_Offset + 3) = Convert.ToByte((Alpha << 4) + Alpha)
                            Case 10 'L4
                                Dim Pixel_Data As Integer
                                If Low_High_Toggle Then
                                    Pixel_Data = Data(Offset) And &HF
                                    Offset += 1
                                Else
                                    Pixel_Data = (Data(Offset) And &HF0) >> 4
                                End If
                                Out(Out_Offset) = Convert.ToByte(Pixel_Data * &H11)
                                Out(Out_Offset + 1) = Convert.ToByte(Pixel_Data * &H11)
                                Out(Out_Offset + 2) = Convert.ToByte(Pixel_Data * &H11)
                                Out(Out_Offset + 3) = &HFF
                                Low_High_Toggle = Not Low_High_Toggle
                        End Select
                    Next
                Next
            Next
        End If

        Return Out
    End Function

#Region "Texture inserter/ETC1 Compressor"
    Public Sub Insert_Texture(File_Name As String, LstIndex As Integer)
        Dim Offset As Integer = Model_Texture(LstIndex).Offset
        Dim Format As Integer = Model_Texture(LstIndex).Format

        Dim Img As New Bitmap(File_Name)
        If (Img.Width <> Model_Texture(LstIndex).Image.Width) Or (Img.Height <> Model_Texture(LstIndex).Image.Height) Then
            MsgBox("Images need to have the same resolution!" & vbCrLf & "Create a new file to change the resolution!", vbExclamation)
            Exit Sub
        End If

        Dim ImgData As BitmapData = Img.LockBits(New Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadOnly, Img.PixelFormat)
        Dim Data((ImgData.Height * ImgData.Stride) - 1) As Byte
        Marshal.Copy(ImgData.Scan0, Data, 0, Data.Length)
        Img.UnlockBits(ImgData)

        Dim BPP As Integer = 24
        If Img.PixelFormat = PixelFormat.Format32bppArgb Then BPP = 32

        Dim Out_Data() As Byte = Nothing

        Select Case Format
            Case 12, 13
                'Os tiles com compressão ETC1 no 3DS estão embaralhados
                Dim Out((Img.Width * Img.Height * 4) - 1) As Byte
                Dim Tile_Scramble() As Integer = Get_ETC1_Scramble(Img.Width, Img.Height)

                Dim i As Integer = 0
                For Tile_Y As Integer = 0 To (Img.Height \ 4) - 1
                    For Tile_X As Integer = 0 To (Img.Width \ 4) - 1
                        Dim TX As Integer = Tile_Scramble(i) Mod (Img.Width \ 4)
                        Dim TY As Integer = (Tile_Scramble(i) - TX) \ (Img.Width \ 4)
                        For Y As Integer = 0 To 3
                            For X As Integer = 0 To 3
                                Dim Out_Offset As Integer = ((TX * 4) + X + ((((TY * 4) + Y)) * Img.Width)) * 4
                                Dim Image_Offset As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * (BPP \ 8)

                                Out(Out_Offset) = Data(Image_Offset + 2)
                                Out(Out_Offset + 1) = Data(Image_Offset + 1)
                                Out(Out_Offset + 2) = Data(Image_Offset)
                                If BPP = 32 Then Out(Out_Offset + 3) = Data(Image_Offset + 3) Else Out(Out_Offset + 3) = &HFF
                            Next
                        Next
                        i += 1
                    Next
                Next

                ReDim Out_Data(((Img.Width * Img.Height) \ If(Format = 12, 2, 1)) - 1)
                Dim Out_Data_Offset As Integer

                For Tile_Y As Integer = 0 To (Img.Height \ 4) - 1
                    For Tile_X As Integer = 0 To (Img.Width \ 4) - 1
                        Dim Flip As Boolean = False

                        Dim Test_R1 As Integer = 0, Test_G1 As Integer = 0, Test_B1 As Integer = 0
                        Dim Test_R2 As Integer = 0, Test_G2 As Integer = 0, Test_B2 As Integer = 0
                        For Y As Integer = 0 To 1
                            For X As Integer = 0 To 1
                                Dim Image_Offset_1 As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4
                                Dim Image_Offset_2 As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + (2 + Y)) * Img.Width)) * 4

                                Test_R1 += Out(Image_Offset_1)
                                Test_G1 += Out(Image_Offset_1 + 1)
                                Test_B1 += Out(Image_Offset_1 + 2)

                                Test_R2 += Out(Image_Offset_2)
                                Test_G2 += Out(Image_Offset_2 + 1)
                                Test_B2 += Out(Image_Offset_2 + 2)
                            Next
                        Next

                        Test_R1 \= 8
                        Test_G1 \= 8
                        Test_B1 \= 8

                        Test_R2 \= 8
                        Test_G2 \= 8
                        Test_B2 \= 8

                        Dim Test_Luma_1 As Integer = Convert.ToInt32(0.299F * Test_R1 + 0.587F * Test_G1 + 0.114F * Test_B1)
                        Dim Test_Luma_2 As Integer = Convert.ToInt32(0.299F * Test_R2 + 0.587F * Test_G2 + 0.114F * Test_B2)
                        Dim Test_Flip_Diff As Integer = Math.Abs(Test_Luma_1 - Test_Luma_2)
                        If Test_Flip_Diff > 48 Then Flip = True

                        'Primeiro, cálcula a média de cores de cada bloco
                        Dim Avg_R1 As Integer = 0, Avg_G1 As Integer = 0, Avg_B1 As Integer = 0
                        Dim Avg_R2 As Integer = 0, Avg_G2 As Integer = 0, Avg_B2 As Integer = 0

                        If Flip Then
                            For Y As Integer = 0 To 1
                                For X As Integer = 0 To 3
                                    Dim Image_Offset_1 As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4
                                    Dim Image_Offset_2 As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + (2 + Y)) * Img.Width)) * 4

                                    Avg_R1 += Out(Image_Offset_1)
                                    Avg_G1 += Out(Image_Offset_1 + 1)
                                    Avg_B1 += Out(Image_Offset_1 + 2)

                                    Avg_R2 += Out(Image_Offset_2)
                                    Avg_G2 += Out(Image_Offset_2 + 1)
                                    Avg_B2 += Out(Image_Offset_2 + 2)
                                Next
                            Next
                        Else
                            For Y As Integer = 0 To 3
                                For X As Integer = 0 To 1
                                    Dim Image_Offset_1 As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4
                                    Dim Image_Offset_2 As Integer = ((Tile_X * 4) + (2 + X) + (((Tile_Y * 4) + Y) * Img.Width)) * 4

                                    Avg_R1 += Out(Image_Offset_1)
                                    Avg_G1 += Out(Image_Offset_1 + 1)
                                    Avg_B1 += Out(Image_Offset_1 + 2)

                                    Avg_R2 += Out(Image_Offset_2)
                                    Avg_G2 += Out(Image_Offset_2 + 1)
                                    Avg_B2 += Out(Image_Offset_2 + 2)
                                Next
                            Next
                        End If

                        Avg_R1 \= 8
                        Avg_G1 \= 8
                        Avg_B1 \= 8

                        Avg_R2 \= 8
                        Avg_G2 \= 8
                        Avg_B2 \= 8

                        Dim Block_Top As Integer = 0
                        Dim Block_Bottom As Integer = 0

                        Block_Top = ((Avg_R2 And &HF0) >> 4) Or (Avg_R1 And &HF0)
                        Block_Top = Block_Top Or (((Avg_G2 And &HF0) << 4) Or ((Avg_G1 And &HF0) << 8))
                        Block_Top = Block_Top Or (((Avg_B2 And &HF0) << 12) Or ((Avg_B1 And &HF0) << 16))

                        If Flip Then Block_Top = Block_Top Or &H1000000

                        Avg_R1 = (Avg_R1 And &HF0) + ((Avg_R1 And &HF0) >> 4)
                        Avg_G1 = (Avg_G1 And &HF0) + ((Avg_G1 And &HF0) >> 4)
                        Avg_B1 = (Avg_B1 And &HF0) + ((Avg_B1 And &HF0) >> 4)

                        Avg_R2 = (Avg_R2 And &HF0) + ((Avg_R2 And &HF0) >> 4)
                        Avg_G2 = (Avg_G2 And &HF0) + ((Avg_G2 And &HF0) >> 4)
                        Avg_B2 = (Avg_B2 And &HF0) + ((Avg_B2 And &HF0) >> 4)

                        Dim Mod_Table_1 As Integer = 0
                        Dim Min_Diff_1(7) As Integer
                        For a As Integer = 0 To 7
                            Min_Diff_1(a) = 0
                        Next
                        For Y As Integer = 0 To If(Flip, 1, 3)
                            For X As Integer = 0 To If(Flip, 3, 1)
                                Dim Image_Offset As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4
                                Dim Luma As Integer = Convert.ToInt32(0.299F * Out(Image_Offset) + 0.587F * Out(Image_Offset + 1) + 0.114F * Out(Image_Offset + 2))

                                For a As Integer = 0 To 7
                                    Dim Optimal_Diff As Integer = 255 * 4
                                    For b As Integer = 0 To 3
                                        Dim CR As Integer = Avg_R1 + Modulation_Table(a, b)
                                        Dim CG As Integer = Avg_G1 + Modulation_Table(a, b)
                                        Dim CB As Integer = Avg_B1 + Modulation_Table(a, b)

                                        Dim Test_Luma As Integer = Convert.ToInt32(0.299F * CR + 0.587F * CG + 0.114F * CB)
                                        Dim Diff As Integer = Math.Abs(Luma - Test_Luma)
                                        If Diff < Optimal_Diff Then Optimal_Diff = Diff
                                    Next
                                    Min_Diff_1(a) += Optimal_Diff
                                Next
                            Next
                        Next

                        Dim Temp_1 As Integer = 255 * 32
                        For a As Integer = 0 To 7
                            If Min_Diff_1(a) < Temp_1 Then
                                Temp_1 = Min_Diff_1(a)
                                Mod_Table_1 = a
                            End If
                        Next

                        Dim Mod_Table_2 As Integer = 0
                        Dim Min_Diff_2(7) As Integer
                        For a As Integer = 0 To 7
                            Min_Diff_2(a) = 0
                        Next
                        For Y As Integer = If(Flip, 2, 0) To 3
                            For X As Integer = If(Flip, 0, 2) To 3
                                Dim Image_Offset As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4
                                Dim Luma As Integer = Convert.ToInt32(0.299F * Out(Image_Offset) + 0.587F * Out(Image_Offset + 1) + 0.114F * Out(Image_Offset + 2))

                                For a As Integer = 0 To 7
                                    Dim Optimal_Diff As Integer = 255 * 4
                                    For b As Integer = 0 To 3
                                        Dim CR As Integer = Avg_R2 + Modulation_Table(a, b)
                                        Dim CG As Integer = Avg_G2 + Modulation_Table(a, b)
                                        Dim CB As Integer = Avg_B2 + Modulation_Table(a, b)

                                        Dim Test_Luma As Integer = Convert.ToInt32(0.299F * CR + 0.587F * CG + 0.114F * CB)
                                        Dim Diff As Integer = Math.Abs(Luma - Test_Luma)
                                        If Diff < Optimal_Diff Then Optimal_Diff = Diff
                                    Next
                                    Min_Diff_2(a) += Optimal_Diff
                                Next
                            Next
                        Next

                        Dim Temp_2 As Integer = 255 * 32
                        For a As Integer = 0 To 7
                            If Min_Diff_2(a) < Temp_2 Then
                                Temp_2 = Min_Diff_2(a)
                                Mod_Table_2 = a
                            End If
                        Next

                        Block_Top = Block_Top Or (Mod_Table_1 << 29)
                        Block_Top = Block_Top Or (Mod_Table_2 << 26)

                        For Y As Integer = 0 To If(Flip, 1, 3)
                            For X As Integer = 0 To If(Flip, 3, 1)
                                Dim Image_Offset As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4
                                Dim Luma As Integer = Convert.ToInt32(0.299F * Out(Image_Offset) + 0.587F * Out(Image_Offset + 1) + 0.114F * Out(Image_Offset + 2))

                                Dim Col_Diff As Integer = 255
                                Dim Pix_Table_Index As Integer = 0
                                For b As Integer = 0 To 3
                                    Dim CR As Integer = Clip(Avg_R1 + Modulation_Table(Mod_Table_1, b))
                                    Dim CG As Integer = Clip(Avg_G1 + Modulation_Table(Mod_Table_1, b))
                                    Dim CB As Integer = Clip(Avg_B1 + Modulation_Table(Mod_Table_1, b))

                                    Dim Test_Luma As Integer = Convert.ToInt32(0.299F * CR + 0.587F * CG + 0.114F * CB)
                                    Dim Diff As Integer = Math.Abs(Luma - Test_Luma)
                                    If Diff < Col_Diff Then
                                        Col_Diff = Diff
                                        Pix_Table_Index = b
                                    End If
                                Next

                                Dim Index As Integer = X * 4 + Y
                                If Index < 8 Then
                                    Block_Bottom = Block_Bottom Or (((Pix_Table_Index And 2) >> 1) << (Index + 8))
                                    Block_Bottom = Block_Bottom Or ((Pix_Table_Index And 1) << (Index + 24))
                                Else
                                    Block_Bottom = Block_Bottom Or (((Pix_Table_Index And 2) >> 1) << (Index - 8))
                                    Block_Bottom = Block_Bottom Or ((Pix_Table_Index And 1) << (Index + 8))
                                End If
                            Next
                        Next

                        For Y As Integer = If(Flip, 2, 0) To 3
                            For X As Integer = If(Flip, 0, 2) To 3
                                Dim Image_Offset As Integer = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * 4
                                Dim Luma As Integer = Convert.ToInt32(0.299F * Out(Image_Offset) + 0.587F * Out(Image_Offset + 1) + 0.114F * Out(Image_Offset + 2))

                                Dim Col_Diff As Integer = 255
                                Dim Pix_Table_Index As Integer = 0
                                For b As Integer = 0 To 3
                                    Dim CR As Integer = Clip(Avg_R2 + Modulation_Table(Mod_Table_2, b))
                                    Dim CG As Integer = Clip(Avg_G2 + Modulation_Table(Mod_Table_2, b))
                                    Dim CB As Integer = Clip(Avg_B2 + Modulation_Table(Mod_Table_2, b))

                                    Dim Test_Luma As Integer = Convert.ToInt32(0.299F * CR + 0.587F * CG + 0.114F * CB)
                                    Dim Diff As Integer = Math.Abs(Luma - Test_Luma)
                                    If Diff < Col_Diff Then
                                        Col_Diff = Diff
                                        Pix_Table_Index = b
                                    End If
                                Next

                                Dim Index As Integer = X * 4 + Y
                                If Index < 8 Then
                                    Block_Bottom = Block_Bottom Or (((Pix_Table_Index And 2) >> 1) << (Index + 8))
                                    Block_Bottom = Block_Bottom Or ((Pix_Table_Index And 1) << (Index + 24))
                                Else
                                    Block_Bottom = Block_Bottom Or (((Pix_Table_Index And 2) >> 1) << (Index - 8))
                                    Block_Bottom = Block_Bottom Or ((Pix_Table_Index And 1) << (Index + 8))
                                End If
                            Next
                        Next

                        Dim Block(7) As Byte
                        Buffer.BlockCopy(BitConverter.GetBytes(Block_Top), 0, Block, 0, 4)
                        Buffer.BlockCopy(BitConverter.GetBytes(Block_Bottom), 0, Block, 4, 4)
                        Dim New_Block(7) As Byte
                        For j As Integer = 0 To 7
                            New_Block(7 - j) = Block(j)
                        Next
                        If Format = 13 Then
                            Dim Alphas(7) As Byte
                            Dim Alpha_Offset As Integer = 0
                            For TX As Integer = 0 To 3
                                For TY As Integer = 0 To 3 Step 2
                                    Dim Img_Offset_1 As Integer = (Tile_X * 4 + TX + ((Tile_Y * 4 + TY) * Img.Width)) * 4
                                    Dim Img_Offset_2 As Integer = (Tile_X * 4 + TX + ((Tile_Y * 4 + TY + 1) * Img.Width)) * 4

                                    Dim Alpha_1 As Byte = Out(Img_Offset_1 + 3) >> 4
                                    Dim Alpha_2 As Byte = Out(Img_Offset_2 + 3) >> 4

                                    Alphas(Alpha_Offset) = Alpha_1 Or (Alpha_2 << 4)

                                    Alpha_Offset += 1
                                Next
                            Next

                            Buffer.BlockCopy(Alphas, 0, Out_Data, Out_Data_Offset, 8)
                            Buffer.BlockCopy(New_Block, 0, Out_Data, Out_Data_Offset + 8, 8)
                            Out_Data_Offset += 16
                        ElseIf Format = 12 Then
                            Buffer.BlockCopy(New_Block, 0, Out_Data, Out_Data_Offset, 8)
                            Out_Data_Offset += 8
                        End If
                    Next
                Next
            Case Else
                Select Case Format
                    Case 0 : ReDim Out_Data((Img.Width * Img.Height * 4) - 1)
                    Case 1 : ReDim Out_Data((Img.Width * Img.Height * 3) - 1)
                    Case 2, 3, 4, 5 : ReDim Out_Data((Img.Width * Img.Height * 2) - 1)
                    Case 7, 8 : ReDim Out_Data((Img.Width * Img.Height) - 1)
                End Select
                Dim Out_Data_Offset As Integer
                For Tile_Y As Integer = 0 To (Img.Height \ 8) - 1
                    For Tile_X As Integer = 0 To (Img.Width \ 8) - 1
                        For i As Integer = 0 To 63
                            Dim X As Integer = Tile_Order(i) Mod 8
                            Dim Y As Integer = (Tile_Order(i) - X) \ 8
                            Dim Img_Offset As Integer = ((Tile_X * 8) + X + (((Tile_Y * 8) + Y)) * Img.Width) * (BPP \ 8)
                            Select Case Format
                                Case 0 'R8G8B8A8
                                    If BPP = 32 Then Out_Data(Out_Data_Offset) = Data(Img_Offset + 3) Else Out_Data(Out_Data_Offset) = &HFF
                                    Buffer.BlockCopy(Data, Img_Offset, Out_Data, Out_Data_Offset + 1, 3)
                                    Out_Data_Offset += 4
                                Case 1 'R8G8B8 (sem transparência)
                                    Buffer.BlockCopy(Data, Img_Offset, Out_Data, Out_Data_Offset, 3)
                                    Out_Data_Offset += 3
                                Case 2 'R5G5B5A1
                                    Out_Data(Out_Data_Offset + 1) = Convert.ToByte((Data(Img_Offset + 1) And &HE0) >> 5)
                                    Out_Data(Out_Data_Offset + 1) += Convert.ToByte(Data(Img_Offset + 2) And &HF8)
                                    Out_Data(Out_Data_Offset) = Convert.ToByte((Data(Img_Offset) And &HF8) >> 2)
                                    Out_Data(Out_Data_Offset) += Convert.ToByte((Data(Img_Offset + 1) And &H18) << 3)
                                    If (BPP = 32 And Data(Img_Offset + 3) = &HFF) Or BPP = 24 Then Out_Data(Out_Data_Offset) += Convert.ToByte(1)
                                    Out_Data_Offset += 2
                                Case 3 'R5G6B5
                                    Out_Data(Out_Data_Offset + 1) = Convert.ToByte((Data(Img_Offset + 1) And &HE0) >> 5)
                                    Out_Data(Out_Data_Offset + 1) += Convert.ToByte(Data(Img_Offset + 2) And &HF8)
                                    Out_Data(Out_Data_Offset) = Convert.ToByte(Data(Img_Offset) >> 3)
                                    Out_Data(Out_Data_Offset) += Convert.ToByte((Data(Img_Offset + 1) And &H1C) << 3)
                                    Out_Data_Offset += 2
                                Case 4 'R4G4B4A4
                                    Out_Data(Out_Data_Offset + 1) = Convert.ToByte((Data(Img_Offset + 1) And &HF0) >> 4)
                                    Out_Data(Out_Data_Offset + 1) += Convert.ToByte(Data(Img_Offset + 2) And &HF0)
                                    Out_Data(Out_Data_Offset) = Convert.ToByte(Data(Img_Offset) And &HF0)
                                    If BPP = 32 Then
                                        Out_Data(Out_Data_Offset) += Convert.ToByte((Data(Img_Offset + 3) And &HF0) >> 4)
                                    Else
                                        Out_Data(Out_Data_Offset) += Convert.ToByte(&HF)
                                    End If
                                    Out_Data_Offset += 2
                                Case 5 'L8A8
                                    Dim Luma As Byte = Convert.ToByte(0.299F * Data(Img_Offset) + 0.587F * Data(Img_Offset + 1) + 0.114F * Data(Img_Offset + 2))
                                    Out_Data(Out_Data_Offset + 1) = Luma
                                    If BPP = 32 Then Out_Data(Out_Data_Offset) = Data(Img_Offset + 3) Else Out_Data(Out_Data_Offset) = &HFF
                                    Out_Data_Offset += 2
                                Case 7 'L8
                                    Dim Luma As Byte = Convert.ToByte(0.299F * Data(Img_Offset) + 0.587F * Data(Img_Offset + 1) + 0.114F * Data(Img_Offset + 2))
                                    Out_Data(Out_Data_Offset) = Luma
                                    Out_Data_Offset += 1
                                Case 8 'A8
                                    If BPP = 32 Then
                                        Out_Data(Out_Data_Offset) = Data(Img_Offset + 3)
                                    Else
                                        Out_Data(Out_Data_Offset) = &HFF
                                    End If
                                    Out_Data_Offset += 1
                            End Select
                        Next
                    Next
                Next
        End Select

        Dim Temp() As Byte = Convert_Texture(Out_Data, 0, Format, Img.Width, Img.Height)
        Dim Img2 As New Bitmap(Img.Width, Img.Height, Imaging.PixelFormat.Format32bppArgb)
        Dim ImgData2 As BitmapData = Img2.LockBits(New Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)
        Marshal.Copy(Temp, 0, ImgData2.Scan0, Temp.Length)
        Img2.UnlockBits(ImgData2)

        Dim Temp_List() As OhanaTexture = Model_Texture.ToArray
        Model_Texture = New List(Of OhanaTexture)

        Dim Temp_Data() As Byte = File.ReadAllBytes(Temp_Texture_File)
        Dim File_Magic As String = Nothing
        For i As Integer = 0 To 1
            File_Magic &= Chr(Temp_Data(i))
        Next
        If File_Magic = "PT" Then
            Dim Temp2() As Byte = Mirror_Texture(Temp, Img.Width, Img.Height)
            Dim Img3 As New Bitmap(Img.Width * 2, Img.Height, Imaging.PixelFormat.Format32bppArgb)
            Dim ImgData3 As BitmapData = Img3.LockBits(New Rectangle(0, 0, Img.Width * 2, Img.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)
            Marshal.Copy(Temp2, 0, ImgData3.Scan0, Temp2.Length)
            Img3.UnlockBits(ImgData2)
            Temp_List(LstIndex).Texture = Get_Texture(Img3)
        Else
            Temp_List(LstIndex).Texture = Get_Texture(Img2)
        End If

        Img2.RotateFlip(RotateFlipType.RotateNoneFlipY)

        Temp_List(LstIndex).Image = Img2
        Model_Texture.AddRange(Temp_List)
        If FrmMain.LstTextures.SelectedIndex = LstIndex Then
            FrmMain.ImgTexture.Image = Img2
            FrmMain.ImgTexture.Refresh()
        End If

        Buffer.BlockCopy(Out_Data, 0, Temp_Data, Model_Texture(LstIndex).Offset, Out_Data.Length)
        File.WriteAllBytes(Temp_Texture_File, Temp_Data)
    End Sub
#End Region

#Region "ETC1 Decompressor"
    Private Function ETC1_Decompress(Data() As Byte, Alphas() As Byte, Width As Integer, Height As Integer) As Byte()
        Dim Out((Width * Height * 4) - 1) As Byte
        Dim Offset As Integer
        For Y As Integer = 0 To (Height \ 4) - 1
            For X As Integer = 0 To (Width \ 4) - 1
                Dim Block(7) As Byte
                Dim Alphas_Block(7) As Byte
                For i As Integer = 0 To 7
                    Block(7 - i) = Data(Offset + i)
                    Alphas_Block(i) = Alphas(Offset + i)
                Next
                Offset += 8
                Block = ETC1_Decompress_Block(Block)

                Dim Low_High_Toggle As Boolean = False
                Dim Alpha_Offset As Integer = 0
                For TX As Integer = 0 To 3
                    For TY As Integer = 0 To 3
                        Dim Out_Offset As Integer = (X * 4 + TX + ((Y * 4 + TY) * Width)) * 4
                        Dim Block_Offset As Integer = (TX + (TY * 4)) * 4
                        Buffer.BlockCopy(Block, Block_Offset, Out, Out_Offset, 3)

                        Dim Alpha_Data As Integer
                        If Low_High_Toggle Then
                            Alpha_Data = (Alphas_Block(Alpha_Offset) And &HF0) >> 4
                            Alpha_Offset += 1
                        Else
                            Alpha_Data = Alphas_Block(Alpha_Offset) And &HF
                        End If
                        Low_High_Toggle = Not Low_High_Toggle
                        Out(Out_Offset + 3) = Convert.ToByte((Alpha_Data << 4) + Alpha_Data)
                    Next
                Next
            Next
        Next
        Return Out
    End Function
    Private Function ETC1_Decompress_Block(Data() As Byte) As Byte()
        'Ericsson Texture Compression
        Dim Block_Top As Integer = Read32(Data, 0)
        Dim Block_Bottom As Integer = Read32(Data, 4)

        Dim Flip As Boolean = (Block_Top And &H1000000) > 0
        Dim Difference As Boolean = (Block_Top And &H2000000) > 0

        Dim R1, G1, B1, R2, G2, B2 As Integer
        Dim R, G, B As Integer

        If Difference Then
            R1 = Block_Top And &HF8
            G1 = (Block_Top And &HF800) >> 8
            B1 = (Block_Top And &HF80000) >> 16

            R = Signed_Byte(Convert.ToByte(R1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top And 7) << 5)) >> 5)
            G = Signed_Byte(Convert.ToByte(G1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top And &H700) >> 3)) >> 5)
            B = Signed_Byte(Convert.ToByte(B1 >> 3)) + (Signed_Byte(Convert.ToByte((Block_Top And &H70000) >> 11)) >> 5)

            R2 = R
            G2 = G
            B2 = B

            R1 = R1 + (R1 >> 5)
            G1 = G1 + (G1 >> 5)
            B1 = B1 + (B1 >> 5)

            R2 = (R2 << 3) + (R2 >> 2)
            G2 = (G2 << 3) + (G2 >> 2)
            B2 = (B2 << 3) + (B2 >> 2)
        Else
            R1 = Block_Top And &HF0
            R1 = R1 + (R1 >> 4)
            G1 = (Block_Top And &HF000) >> 8
            G1 = G1 + (G1 >> 4)
            B1 = (Block_Top And &HF00000) >> 16
            B1 = B1 + (B1 >> 4)

            R2 = (Block_Top And &HF) << 4
            R2 = R2 + (R2 >> 4)
            G2 = (Block_Top And &HF00) >> 4
            G2 = G2 + (G2 >> 4)
            B2 = (Block_Top And &HF0000) >> 12
            B2 = B2 + (B2 >> 4)
        End If

        Dim Mod_Table_1 As Integer = (Block_Top >> 29) And 7
        Dim Mod_Table_2 As Integer = (Block_Top >> 26) And 7

        Dim Out((4 * 4 * 4) - 1) As Byte
        If Flip = False Then
            For Y As Integer = 0 To 3
                For X As Integer = 0 To 1
                    Dim Col_1 As Color = Modify_Pixel(R1, G1, B1, X, Y, Block_Bottom, Mod_Table_1)
                    Dim Col_2 As Color = Modify_Pixel(R2, G2, B2, X + 2, Y, Block_Bottom, Mod_Table_2)
                    Out((Y * 4 + X) * 4) = Col_1.R
                    Out(((Y * 4 + X) * 4) + 1) = Col_1.G
                    Out(((Y * 4 + X) * 4) + 2) = Col_1.B
                    Out((Y * 4 + X + 2) * 4) = Col_2.R
                    Out(((Y * 4 + X + 2) * 4) + 1) = Col_2.G
                    Out(((Y * 4 + X + 2) * 4) + 2) = Col_2.B
                Next
            Next
        Else
            For Y As Integer = 0 To 1
                For X As Integer = 0 To 3
                    Dim Col_1 As Color = Modify_Pixel(R1, G1, B1, X, Y, Block_Bottom, Mod_Table_1)
                    Dim Col_2 As Color = Modify_Pixel(R2, G2, B2, X, Y + 2, Block_Bottom, Mod_Table_2)
                    Out((Y * 4 + X) * 4) = Col_1.R
                    Out(((Y * 4 + X) * 4) + 1) = Col_1.G
                    Out(((Y * 4 + X) * 4) + 2) = Col_1.B
                    Out(((Y + 2) * 4 + X) * 4) = Col_2.R
                    Out((((Y + 2) * 4 + X) * 4) + 1) = Col_2.G
                    Out((((Y + 2) * 4 + X) * 4) + 2) = Col_2.B
                Next
            Next
        End If

        Return Out
    End Function
    Private Function Modify_Pixel(R As Integer, G As Integer, B As Integer, X As Integer, Y As Integer, Mod_Block As Integer, Mod_Table As Integer) As Color
        Dim Index As Integer = X * 4 + Y
        Dim Pixel_Modulation As Integer
        Dim MSB As Integer = Mod_Block << 1

        If Index < 8 Then
            Pixel_Modulation = Modulation_Table(Mod_Table, ((Mod_Block >> (Index + 24)) And 1) + ((MSB >> (Index + 8)) And 2))
        Else
            Pixel_Modulation = Modulation_Table(Mod_Table, ((Mod_Block >> (Index + 8)) And 1) + ((MSB >> (Index - 8)) And 2))
        End If

        R = Clip(R + Pixel_Modulation)
        G = Clip(G + Pixel_Modulation)
        B = Clip(B + Pixel_Modulation)

        Return Color.FromArgb(B, G, R)
    End Function
    Private Function Clip(Value As Integer) As Byte
        If Value > &HFF Then
            Return &HFF
        ElseIf Value < 0 Then
            Return 0
        Else
            Return Convert.ToByte(Value And &HFF)
        End If
    End Function
#End Region

#Region "Misc. functions"
    Private Function Signed_Byte(Byte_To_Convert As Byte) As SByte
        If (Byte_To_Convert < &H80) Then Return Convert.ToSByte(Byte_To_Convert)
        Return Convert.ToSByte(Byte_To_Convert - &H100)
    End Function
    Private Function Signed_Short(Short_To_Convert As Integer) As Integer
        If (Short_To_Convert < &H8000) Then Return Short_To_Convert
        Return Short_To_Convert - &H10000
    End Function

    Public Function Get_Texture(Image As Bitmap) As Texture
        Return New Texture(Device, Image, Usage.None, Pool.Managed)
    End Function
    Private Function Mirror_Texture(Data() As Byte, Width As Integer, Height As Integer) As Byte()
        Dim Out(((Width * 2) * Height * 4) - 1) As Byte
        For Y As Integer = 0 To Height - 1
            For X As Integer = 0 To Width - 1
                Dim Offset As Integer = (X + (Y * Width)) * 4
                Dim Offset_2 As Integer = (X + (Y * (Width * 2))) * 4
                Dim Offset_3 As Integer = ((Width + (Width - X - 1)) + (Y * (Width * 2))) * 4
                Buffer.BlockCopy(Data, Offset, Out, Offset_2, 4)
                Buffer.BlockCopy(Data, Offset, Out, Offset_3, 4)
            Next
        Next
        Return Out
    End Function
    Private Function Check_Alpha(Img() As Byte) As Boolean
        For Offset As Integer = 0 To Img.Length - 1 Step 4
            If Img(Offset + 3) < &HFF Then Return True
        Next
        Return False
    End Function

    Private Function Get_ETC1_Scramble(Width As Integer, Height As Integer) As Integer()
        Dim Tile_Scramble(((Width \ 4) * (Height \ 4)) - 1) As Integer
        Dim Base_Accumulator As Integer = 0, Line_Accumulator As Integer = 0
        Dim Base_Number As Integer = 0, Line_Number As Integer = 0

        For Tile As Integer = 0 To Tile_Scramble.Length - 1
            If (Tile Mod (Width \ 4) = 0) And Tile > 0 Then
                If Line_Accumulator < 1 Then
                    Line_Accumulator += 1
                    Line_Number += 2
                    Base_Number = Line_Number
                Else
                    Line_Accumulator = 0
                    Base_Number -= 2
                    Line_Number = Base_Number
                End If
            End If

            Tile_Scramble(Tile) = Base_Number

            If Base_Accumulator < 1 Then
                Base_Accumulator += 1
                Base_Number += 1
            Else
                Base_Accumulator = 0
                Base_Number += 3
            End If
        Next

        Return Tile_Scramble
    End Function
#End Region

#End Region

#Region "Renderer"
    Public Sub Render()
        'Define a posição da "câmera"
        Device.Transform.Projection = Matrix.PerspectiveFovLH(Math.PI / 4, CSng(SWidth / SHeight), 0.1F, 500.0F)
        Device.Transform.View = Matrix.LookAtLH(New Vector3(0.0F, 0.0F, 20.0F), New Vector3(0.0F, 0.0F, 0.0F), New Vector3(0.0F, 1.0F, 0.0F))

        'Configura iluminação
        Switch_Lighting(Lighting)

        Rendering = True
        While Rendering
            If Model_Object IsNot Nothing Then
                Device.Clear(ClearFlags.Target, Color.Black, 1.0F, 0)
                Device.Clear(ClearFlags.ZBuffer, Color.Black, 1.0F, 0)
                Device.BeginScene()

                Dim MyMaterial As New Material
                MyMaterial.Diffuse = Color.White
                MyMaterial.Ambient = Color.White
                Device.Material = MyMaterial

                Dim Rotation_Matrix As Matrix = Matrix.RotationYawPitchRoll(Rotation.X / 200.0F, -Rotation.Y / 200.0F, 0)
                Dim Translation_Matrix As Matrix = Matrix.Translation(New Vector3(Translation.X / 50.0F, (Translation.Y / 50.0F), Zoom))
                Device.Transform.World = Rotation_Matrix * Translation_Matrix

                If Edit_Mode Then
                    With Model_Object(Selected_Object)
                        If .Texture_ID < Model_Texture_Index.Length Then
                            Dim Texture_Name As String = Model_Texture_Index(.Texture_ID)
                            If Model_Texture IsNot Nothing Then
                                For Each Current_Texture As OhanaTexture In Model_Texture
                                    If Current_Texture.Name = Texture_Name Then
                                        Device.SetTexture(0, Current_Texture.Texture)
                                    End If
                                Next
                            End If
                        End If

                        Dim Vertex_Format As VertexFormats = VertexFormats.Position Or VertexFormats.Normal Or VertexFormats.Texture1 Or VertexFormats.Diffuse
                        Dim VtxBuffer As New VertexBuffer(GetType(OhanaVertex), .Vertice.Length, Device, Usage.None, Vertex_Format, Pool.Managed)
                        VtxBuffer.SetData(.Vertice, 0, LockFlags.None)
                        Device.VertexFormat = Vertex_Format
                        Device.SetStreamSource(0, VtxBuffer, 0)

                        If Selected_Face > -1 Then
                            Dim Index_Buffer As New IndexBuffer(GetType(Integer), .Per_Face_Index(Selected_Face).Length, Device, Usage.WriteOnly, Pool.Managed)
                            Index_Buffer.SetData(.Per_Face_Index(Selected_Face), 0, LockFlags.None)
                            Device.Indices = Index_Buffer

                            Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, .Vertice.Length, 0, .Per_Face_Index(Selected_Face).Length \ 3)
                            Index_Buffer.Dispose()
                        Else
                            Dim Index_Buffer As New IndexBuffer(GetType(Integer), .Index.Length, Device, Usage.WriteOnly, Pool.Managed)
                            Index_Buffer.SetData(.Index, 0, LockFlags.None)
                            Device.Indices = Index_Buffer

                            Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, .Vertice.Length, 0, .Index.Length \ 3)
                            Index_Buffer.Dispose()
                        End If

                        VtxBuffer.Dispose()


                        Device.SetTexture(0, Nothing)
                    End With
                Else
                    For Phase As Integer = 0 To 1
                        For Index As Integer = 0 To Model_Object.Length - 1
                            With Model_Object(Index)
                                Dim Has_Alpha As Boolean

                                If .Texture_ID < Model_Texture_Index.Length Then
                                    Dim Texture_Name As String = Model_Texture_Index(.Texture_ID)
                                    If Model_Texture IsNot Nothing Then
                                        For Each Current_Texture As OhanaTexture In Model_Texture
                                            If Current_Texture.Name = Texture_Name Then
                                                Has_Alpha = Current_Texture.Has_Alpha
                                                If Not Has_Alpha Or (Has_Alpha And Phase > 0) Then Device.SetTexture(0, Current_Texture.Texture)
                                            End If
                                        Next
                                    End If
                                End If

                                If Not Has_Alpha Or (Has_Alpha And Phase > 0) Then
                                    Dim Vertex_Format As VertexFormats = VertexFormats.Position Or VertexFormats.Normal Or VertexFormats.Texture1 Or VertexFormats.Diffuse
                                    Dim VtxBuffer As New VertexBuffer(GetType(OhanaVertex), .Vertice.Length, Device, Usage.None, Vertex_Format, Pool.Managed)
                                    VtxBuffer.SetData(.Vertice, 0, LockFlags.None)
                                    Device.VertexFormat = Vertex_Format
                                    Device.SetStreamSource(0, VtxBuffer, 0)

                                    Dim Index_Buffer As New IndexBuffer(GetType(Integer), .Index.Length, Device, Usage.WriteOnly, Pool.Managed)
                                    Index_Buffer.SetData(.Index, 0, LockFlags.None)
                                    Device.Indices = Index_Buffer

                                    Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, .Vertice.Length, 0, .Index.Length \ 3)

                                    VtxBuffer.Dispose()
                                    Index_Buffer.Dispose()
                                End If

                                Device.SetTexture(0, Nothing)
                            End With
                        Next
                    Next
                End If

                Device.EndScene()
                Device.Present()
            End If

            Application.DoEvents()
        End While
    End Sub

    Public Sub Switch_Lighting(Enabled As Boolean)
        With Device
            If Enabled And Has_Normals Then
                .RenderState.Lighting = True
                .RenderState.Ambient = Color.FromArgb(64, 64, 64)
                .Lights(0).Type = LightType.Point
                .Lights(0).Diffuse = Color.White
                .Lights(0).Position = New Vector3(0.0F, 10.0F, 30.0F)
                .Lights(0).Range = 520.0F
                .Lights(0).Attenuation0 = 2.0F / Scale
                .Lights(0).Enabled = True
            Else
                .RenderState.Lighting = False
                .RenderState.Ambient = Color.White
                .Lights(0).Enabled = False
            End If
        End With
    End Sub
#End Region

#Region "OBJ Inserter"
    Public Sub Insert_OBJ(File_Name As String)
        Dim Data() As Byte = File.ReadAllBytes(Temp_Model_File)
        Dim Obj As String = File.ReadAllText(File_Name)

        Dim Vertices As MatchCollection = Regex.Matches(Obj, "v\s+([-]?\d+(\.\d+)?)\s+([-]?\d+(\.\d+)?)\s+([-]?\d+(\.\d+)?)")
        Dim Normals As MatchCollection = Regex.Matches(Obj, "vn\s+([-]?\d+(\.\d+)?)\s+([-]?\d+(\.\d+)?)\s+([-]?\d+(\.\d+)?)")
        Dim UVs As MatchCollection = Regex.Matches(Obj, "vt\s+([-]?\d+(\.\d+)?)\s+([-]?\d+(\.\d+)?)")

        Dim Faces As MatchCollection = Regex.Matches(Obj, "f\s+(\d+)/\d+(/\d+)?\s+(\d+)/\d+(/\d+)?\s+(\d+)/\d+(/\d+)?")

        With Model_Object(Selected_Object)
            'Insere vertices presentes no .obj até onde der
            Dim All_Vertices_Inserted As Boolean = True
            Dim Current_Vertex_Offset As Integer = .Vertex_Entry.Offset
            Dim Vertex_Length As Integer = .Vertex_Entry.Length

            Dim Vertice_Index As Integer
            Dim Offset As Integer = Current_Vertex_Offset
            Dim Match_Count As Integer = 1
            For Each Vertex As Match In Vertices
                Dim X_Bytes() As Byte = BitConverter.GetBytes(Single.Parse(Vertex.Groups(1).Value, CultureInfo.InvariantCulture))
                Dim Y_Bytes() As Byte = BitConverter.GetBytes(Single.Parse(Vertex.Groups(3).Value, CultureInfo.InvariantCulture))
                Dim Z_Bytes() As Byte = BitConverter.GetBytes(Single.Parse(Vertex.Groups(5).Value, CultureInfo.InvariantCulture))

                Buffer.BlockCopy(X_Bytes, 0, Data, Offset, 4)
                Buffer.BlockCopy(Y_Bytes, 0, Data, Offset + 4, 4)
                Buffer.BlockCopy(Z_Bytes, 0, Data, Offset + 8, 4)

                With Model_Object(Selected_Object).Vertice(Vertice_Index)
                    .X = Single.Parse(Vertex.Groups(1).Value, CultureInfo.InvariantCulture) / Load_Scale
                    .Y = Single.Parse(Vertex.Groups(3).Value, CultureInfo.InvariantCulture) / Load_Scale
                    .Z = Single.Parse(Vertex.Groups(5).Value, CultureInfo.InvariantCulture) / Load_Scale
                    If Load_Mirror Then .X *= -1
                End With

                Vertice_Index += 1
                Offset += .Vertex_Entry.Format
                If Offset - .Vertex_Entry.Offset >= Vertex_Length Then
                    If Match_Count < Vertices.Count Then All_Vertices_Inserted = False
                    Exit For
                End If

                Match_Count += 1
            Next

            Vertice_Index = 0
            Offset = Current_Vertex_Offset
            If Normals.Count = 0 Then
                If MessageBox.Show("Your OBJ file doesn't have normals..." & Environment.NewLine & "Would like to remove them from the object?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Dim NX_Bytes() As Byte = BitConverter.GetBytes(Convert.ToSingle(0))
                    Dim NY_Bytes() As Byte = BitConverter.GetBytes(Convert.ToSingle(0))
                    Dim NZ_Bytes() As Byte = BitConverter.GetBytes(Convert.ToSingle(0))

                    For Each Vertex As OhanaVertex In Model_Object(Selected_Object).Vertice
                        Select Case .Vertex_Entry.Format
                            Case &H20, &H24, &H28, &H2C, &H30, &H34, &H38
                                Buffer.BlockCopy(NX_Bytes, 0, Data, Offset + 12, 4)
                                Buffer.BlockCopy(NY_Bytes, 0, Data, Offset + 16, 4)
                                Buffer.BlockCopy(NZ_Bytes, 0, Data, Offset + 20, 4)
                        End Select

                        With Model_Object(Selected_Object).Vertice(Vertice_Index)
                            .NX = 0
                            .NY = 0
                            .NZ = 0
                        End With

                        Vertice_Index += 1
                        Offset += .Vertex_Entry.Format
                        If Offset - .Vertex_Entry.Offset >= Vertex_Length Then Exit For
                    Next
                End If
            Else
                For Each Normal As Match In Normals
                    Dim NX_Bytes() As Byte = BitConverter.GetBytes(Single.Parse(Normal.Groups(1).Value, CultureInfo.InvariantCulture))
                    Dim NY_Bytes() As Byte = BitConverter.GetBytes(Single.Parse(Normal.Groups(3).Value, CultureInfo.InvariantCulture))
                    Dim NZ_Bytes() As Byte = BitConverter.GetBytes(Single.Parse(Normal.Groups(5).Value, CultureInfo.InvariantCulture))

                    Select Case .Vertex_Entry.Format
                        Case &H20, &H24, &H28, &H2C, &H30, &H34, &H38
                            Buffer.BlockCopy(NX_Bytes, 0, Data, Offset + 12, 4)
                            Buffer.BlockCopy(NY_Bytes, 0, Data, Offset + 16, 4)
                            Buffer.BlockCopy(NZ_Bytes, 0, Data, Offset + 20, 4)
                    End Select

                    With Model_Object(Selected_Object).Vertice(Vertice_Index)
                        .NX = Single.Parse(Normal.Groups(1).Value, CultureInfo.InvariantCulture) / Load_Scale
                        .NY = Single.Parse(Normal.Groups(3).Value, CultureInfo.InvariantCulture) / Load_Scale
                        .NZ = Single.Parse(Normal.Groups(5).Value, CultureInfo.InvariantCulture) / Load_Scale
                        If Load_Mirror Then .NX *= -1
                    End With

                    Vertice_Index += 1
                    Offset += .Vertex_Entry.Format
                    If Offset - .Vertex_Entry.Offset >= Vertex_Length Then Exit For
                Next
            End If


            Vertice_Index = 0
            Offset = Current_Vertex_Offset
            For Each UV As Match In UVs
                Dim U_Bytes() As Byte = BitConverter.GetBytes(Single.Parse(UV.Groups(1).Value, CultureInfo.InvariantCulture))
                Dim V_Bytes() As Byte = BitConverter.GetBytes(Single.Parse(UV.Groups(3).Value, CultureInfo.InvariantCulture))

                Select Case .Vertex_Entry.Format
                    Case &H14, &H18, &H1C
                        Buffer.BlockCopy(U_Bytes, 0, Data, Offset + 12, 4)
                        Buffer.BlockCopy(V_Bytes, 0, Data, Offset + 16, 4)
                    Case &H20, &H24, &H28, &H2C, &H30, &H34, &H38
                        Buffer.BlockCopy(U_Bytes, 0, Data, Offset + 24, 4)
                        Buffer.BlockCopy(V_Bytes, 0, Data, Offset + 28, 4)
                End Select

                Model_Object(Selected_Object).Vertice(Vertice_Index).U = Single.Parse(UV.Groups(1).Value, CultureInfo.InvariantCulture)
                Model_Object(Selected_Object).Vertice(Vertice_Index).V = Single.Parse(UV.Groups(3).Value, CultureInfo.InvariantCulture)

                Vertice_Index += 1
                Offset += .Vertex_Entry.Format
                If Offset - .Vertex_Entry.Offset >= Vertex_Length Then Exit For
            Next

            'Insere Faces presentes no .obj até onde der
            Dim CurrFace As Integer = 0
            Dim Current_Face_Offset As Integer = .Per_Face_Entry(0).Offset
            Dim Face_Length As Integer = .Per_Face_Entry(0).Length

            For Each Entry As Data_Entry In .Per_Face_Entry
                For i As Integer = Entry.Offset To Entry.Offset + Entry.Length
                    Data(i) = 0
                Next
            Next

            For i As Integer = 0 To .Index.Length - 1
                .Index(i) = 0
            Next

            For i As Integer = 0 To .Per_Face_Index.Count - 1
                For j As Integer = 0 To .Per_Face_Index(i).Length - 1
                    .Per_Face_Index(i)(j) = 0
                Next
            Next

            Dim Face_Index As Integer
            Dim Per_Face_Index As Integer
            For Each Face As Match In Faces
                Dim a As Integer = Convert.ToInt32(Face.Groups(1).Value) - 1
                Dim b As Integer = Convert.ToInt32(Face.Groups(3).Value) - 1
                Dim c As Integer = Convert.ToInt32(Face.Groups(5).Value) - 1
                If a < .Vertice.Length And b < .Vertice.Length And c < .Vertice.Length Then
                    If .Per_Face_Entry(CurrFace).Format = 1 Then
                        If a > &HFF Or b > &HFF Or c > &HFF Then
                            While .Per_Face_Entry(CurrFace).Format = 1
                                Face_Index += (.Per_Face_Entry(CurrFace).Length - Per_Face_Index)
                                CurrFace += 1
                                Per_Face_Index = 0
                                If CurrFace < .Per_Face_Entry.Count Then
                                    Current_Face_Offset = .Per_Face_Entry(CurrFace).Offset
                                    Face_Length = .Per_Face_Entry(CurrFace).Length
                                Else
                                    Exit For
                                End If
                            End While

                            '16 bits
                            Data(Current_Face_Offset) = Convert.ToByte(a And &HFF)
                            Data(Current_Face_Offset + 1) = Convert.ToByte((a And &HFF00) >> 8)
                            Data(Current_Face_Offset + 2) = Convert.ToByte(b And &HFF)
                            Data(Current_Face_Offset + 3) = Convert.ToByte((b And &HFF00) >> 8)
                            Data(Current_Face_Offset + 4) = Convert.ToByte(c And &HFF)
                            Data(Current_Face_Offset + 5) = Convert.ToByte((c And &HFF00) >> 8)
                            Current_Face_Offset += 6
                        Else
                            '8 bits
                            Data(Current_Face_Offset) = Convert.ToByte(a And &HFF)
                            Data(Current_Face_Offset + 1) = Convert.ToByte(b And &HFF)
                            Data(Current_Face_Offset + 2) = Convert.ToByte(c And &HFF)
                            Current_Face_Offset += 3
                        End If
                    Else
                        '16 bits
                        Data(Current_Face_Offset) = Convert.ToByte(a And &HFF)
                        Data(Current_Face_Offset + 1) = Convert.ToByte((a And &HFF00) >> 8)
                        Data(Current_Face_Offset + 2) = Convert.ToByte(b And &HFF)
                        Data(Current_Face_Offset + 3) = Convert.ToByte((b And &HFF00) >> 8)
                        Data(Current_Face_Offset + 4) = Convert.ToByte(c And &HFF)
                        Data(Current_Face_Offset + 5) = Convert.ToByte((c And &HFF00) >> 8)
                        Current_Face_Offset += 6
                    End If

                    Model_Object(Selected_Object).Index(Face_Index) = a
                    Model_Object(Selected_Object).Index(Face_Index + 1) = b
                    Model_Object(Selected_Object).Index(Face_Index + 2) = c

                    Model_Object(Selected_Object).Per_Face_Index(CurrFace)(Per_Face_Index) = a
                    Model_Object(Selected_Object).Per_Face_Index(CurrFace)(Per_Face_Index + 1) = b
                    Model_Object(Selected_Object).Per_Face_Index(CurrFace)(Per_Face_Index + 2) = c

                    Face_Index += 3
                    Per_Face_Index += 3

                    If Current_Face_Offset - .Per_Face_Entry(CurrFace).Offset >= Face_Length Then
                        CurrFace += 1
                        Per_Face_Index = 0
                        If CurrFace < .Per_Face_Entry.Count Then
                            Current_Face_Offset = .Per_Face_Entry(CurrFace).Offset
                            Face_Length = .Per_Face_Entry(CurrFace).Length
                        Else
                            Exit For
                        End If
                    End If
                End If
            Next

            If Not All_Vertices_Inserted And Face_Index < .Index.Length Then
                MessageBox.Show("The inserted object have too much faces and vertices." & vbCrLf & "Try limiting it to the original counts.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ElseIf Face_Index \ 3 < Faces.Count Then
                MessageBox.Show("The inserted object have more faces than the original one." & vbCrLf & "Some faces couldn't be added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ElseIf Not All_Vertices_Inserted Then
                MessageBox.Show("The inserted object have more vertices than the original one." & vbCrLf & "Some vertices couldn't be added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End With

        File.WriteAllBytes(Temp_Model_File, Data)
    End Sub
#End Region

#Region "Map"
    Public Sub makeMapIMG(byteArray As Byte())
        Using dataStream As Stream = New MemoryStream(byteArray)
            Using br As New BinaryReader(dataStream)
                Dim width As UShort = br.ReadUInt16()
                Dim height As UShort = br.ReadUInt16()
                Dim img As New Bitmap(width * 8, height * 8)
                Dim c As New Color()
                For i As Integer = 0 To width * height - 1
                    Dim col2 As UInteger = br.ReadUInt32()
                    If col2 = &H1000021 Then
                        c = Color.Black
                    Else
                        col2 = LCG(col2, 4)
                        c = Color.FromArgb(&HFF, &HFF - CByte(col2 And &HFF), &HFF - CByte((col2 >> 8) And &HFF), &HFF - CByte(col2 >> 24 And &HFF))
                    End If
                    For x As Integer = 0 To 7
                        For y As Integer = 0 To 7
                            img.SetPixel((x + (i * 8) Mod (img.Width)), y + ((i \ width) * 8), c)
                        Next
                    Next
                Next
                br.Close()
                FrmMapProp.mapPicBox.Image = img
            End Using
        End Using
    End Sub
    Public Function LCG(seed As Long, ctr As Integer) As UInteger
        For i As Integer = 0 To ctr - 1
            seed *= &H41C64E6D
            seed += &H6073
        Next
        Return seed
    End Function
#End Region

End Class
