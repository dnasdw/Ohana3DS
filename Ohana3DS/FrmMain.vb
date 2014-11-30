Imports System.IO
Imports System.Threading
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Text
Public Class FrmMain
    'Classes de gerenciamento 3D e compressão/extração
    Dim MyOhana As New Ohana
    Dim MyNako As New Nako

    'Movimentação do modelo
    Dim Rot_InitX, Rot_InitY, Rot_FinalX, Rot_FinalY As Integer
    Dim Mov_InitX, Mov_InitY, Mov_FinalX, Mov_FinalY As Integer

    Private Enum TextureMode
        Original
        FlipY
        Mirror
        FlipY_Mirror
    End Enum
    Dim Texture_Mode As TextureMode = TextureMode.Original

    Dim Mdl_BCH_Version As Ohana.BCH_Version = Ohana.BCH_Version.XY
    Dim Tex_BCH_Version As Ohana.BCH_Version = Ohana.BCH_Version.XY

    Dim Model_Export_Thread As Thread
    Dim Texture_Export_Thread As Thread
    Dim GARC_Thread As Thread
    Dim Search_Thread As Thread

    Dim Old_Index As Integer = -1
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg <> &HA3 Then MyBase.WndProc(m)
        Select Case m.Msg
            Case &H84 : If m.Result = New IntPtr(1) Then m.Result = New IntPtr(2)
        End Select
    End Sub
    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SideBar.AddItem("Model")
        SideBar.AddItem("Textures")
        SideBar.AddItem("Text")
        SideBar.AddItem("-")
        SideBar.AddItem("GARC Explorer")
        SideBar.AddItem("Search Tool")
        SideBar.AddItem("ROM Tool")

        MyOhana.Initialize(Screen.Handle)
        Show()
        MyOhana.Render()
    End Sub

#Region "GUI"

#Region "General"
    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        End
    End Sub
    Private Sub BtnMinimize_Click(sender As Object, e As EventArgs) Handles BtnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub Button_MouseEnter(sender As Object, e As EventArgs) Handles BtnMinimize.MouseEnter
        Dim Lbl As Label = CType(sender, Label)
        Lbl.BackColor = Color.FromArgb(15, 82, 186)
        Lbl.ForeColor = Color.White
    End Sub
    Private Sub BtnClose_MouseEnter(sender As Object, e As EventArgs) Handles BtnClose.MouseEnter
        Dim Lbl As Label = CType(sender, Label)
        Lbl.BackColor = Color.Crimson
        Lbl.ForeColor = Color.WhiteSmoke
    End Sub
    Private Sub Button_MouseLeave(sender As Object, e As EventArgs) Handles BtnClose.MouseLeave, BtnMinimize.MouseLeave
        Dim Lbl As Label = CType(sender, Label)
        Lbl.BackColor = Color.Transparent
        Lbl.ForeColor = Color.White
    End Sub

    '---

    Private Sub Hide_Panels()
        'Esconda todos os paineis aqui!
        PanelSplash.Visible = False
        PanelModel.Visible = False
        PanelTextures.Visible = False
        PanelGARC.Visible = False
        PanelSearch.Visible = False
    End Sub
    Private Sub SideBar_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SideBar.Click
        If SideBar.SelectedIndex > -1 And SideBar.SelectedIndex <> Old_Index Then
            Hide_Panels()
            Select Case SideBar.SelectedIndex
                Case 0 'Model
                    Screen.Select()
                    PanelModel.Visible = True
                Case 1 : PanelTextures.Visible = True 'Textures
                Case 3 : PanelGARC.Visible = True
                Case 4 : PanelSearch.Visible = True
            End Select

            Old_Index = SideBar.SelectedIndex
        End If
    End Sub
#End Region

#Region "Common"
    Private Delegate Sub Up_Progress(ProgressBar As MyProgressbar, Percentage As Single, Msg As String)
    Private Delegate Sub Add_Item(List As MyListview, Item As MyListview.ListItem)
    Private Delegate Sub Update_Button(Button As Button, Text As String)
    Private Sub Update_Progress(ProgressBar As MyProgressbar, Percentage As Single, Msg As String)
        If ProgressBar.InvokeRequired Then
            Me.Invoke(New Up_Progress(AddressOf Update_Progress), ProgressBar, Percentage, Msg)
        Else
            ProgressBar.Text = Msg
            ProgressBar.Percentage = Percentage
            ProgressBar.Refresh()
        End If
    End Sub
    Private Sub Add_List_Item(List As MyListview, Item As MyListview.ListItem)
        If List.InvokeRequired Then
            Me.Invoke(New Add_Item(AddressOf Add_List_Item), List, Item)
        Else
            List.AddItem(Item)
            List.Refresh()
        End If
    End Sub
    Private Sub Update_Button_Text(Button As Button, Text As String)
        If Button.InvokeRequired Then
            Me.Invoke(New Update_Button(AddressOf Update_Button_Text), Button, Text)
        Else
            Button.Text = Text
            Button.Refresh()
        End If
    End Sub
#End Region

#Region "Model"
    Private Sub BtnModelOpen_Click(sender As Object, e As EventArgs) Handles BtnModelOpen.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open Pokémon BCH Model"
        OpenDlg.Filter = "BCH Model|*.*"
        If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                MyOhana.Load_Model(OpenDlg.FileName, Mdl_BCH_Version)
                Update_Info()
            Catch
                MyOhana.Model_Object = Nothing
                Screen.Refresh()
                MsgBox("Sorry, something went wrong." & vbCrLf & "Make sure you selected the correct BCH Version (X/Y or OR/AS).", vbExclamation, "Error")
            End Try
        End If
    End Sub
    Private Sub BtnModelExport_Click(sender As Object, e As EventArgs) Handles BtnModelExport.Click
        If MyOhana.Model_Object IsNot Nothing Then
            Dim SaveDlg As New SaveFileDialog
            SaveDlg.Title = "Save model"
            SaveDlg.Filter = "Valve SMD|*.smd"
            If SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                MyOhana.Export_SMD(SaveDlg.FileName)
            End If
        End If
    End Sub
    Private Sub BtnModelExportAllFF_Click(sender As Object, e As EventArgs) Handles BtnModelExportAllFF.Click
        If Model_Export_Thread IsNot Nothing Then
            If Model_Export_Thread.IsAlive Then
                Model_Export_Thread.Abort()
                BtnModelExportAllFF.Text = "Export all from folder"
                ProgressModels.Text = Nothing
                ProgressModels.Percentage = 0
                ProgressModels.Refresh()

                Exit Sub
            End If
        End If

        Dim InputDlg As New FolderBrowserDialog
        If InputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim OutputDlg As New FolderBrowserDialog
            If OutputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                Model_Export_Thread = New Thread(Sub() Model_Exporter(InputDlg.SelectedPath, OutputDlg.SelectedPath))
                Model_Export_Thread.Start()

                BtnModelExportAllFF.Text = "Cancel"
            End If
        End If
    End Sub
    Private Sub Model_Exporter(InFolder As String, OutFolder As String)
        Dim Exporter As New Ohana
        Dim Input_Files() As FileInfo = New DirectoryInfo(InFolder).GetFiles()
        Dim Total_Index, Index As Integer
        For Each File As FileInfo In Input_Files
            Try
                Exporter.Load_Model(File.FullName, Mdl_BCH_Version)
                If Exporter.Model_Object.Length > 0 Then
                    Dim File_Name As String = Path.Combine(OutFolder, "model_" & Index & ".smd")
                    Exporter.Export_SMD(File_Name)
                    Index += 1
                End If
            Catch ex As Exception
                Debug.WriteLine("Model Exporter -> Erro ao exportar modelo: " & Path.GetFileName(File.FullName))
            End Try
            Update_Progress(ProgressModels, Convert.ToSingle((Total_Index / Input_Files.Count) * 100), "Exporting " & Path.GetFileName(File.FullName) & "...")
            Total_Index += 1
        Next

        Update_Progress(ProgressModels, 0, Nothing)
        Update_Button_Text(BtnModelExportAllFF, "Export all from folder")
    End Sub
    Private Sub BtnModelBCHVer_Click(sender As Object, e As EventArgs) Handles BtnModelBCHVer.Click
        If Mdl_BCH_Version = Ohana.BCH_Version.XY Then
            Mdl_BCH_Version = Ohana.BCH_Version.ORAS
            BtnModelBCHVer.Text = "OR/AS"
        ElseIf Mdl_BCH_Version = Ohana.BCH_Version.ORAS Then
            Mdl_BCH_Version = Ohana.BCH_Version.XY
            BtnModelBCHVer.Text = "X/Y"
        End If
    End Sub
    Private Sub BtnModelScale_Click(sender As Object, e As EventArgs) Handles BtnModelScale.Click
        If MyOhana.Scale = 1 Then
            MyOhana.Scale = 32
            BtnModelScale.Text = "Scale 1:32"
        ElseIf MyOhana.Scale = 32 Then
            MyOhana.Scale = 64
            BtnModelScale.Text = "Scale 1:64"
        ElseIf MyOhana.Scale = 64 Then
            MyOhana.Scale = 1
            BtnModelScale.Text = "Scale 1:1"
        End If
        BtnModelScale.Refresh()
    End Sub
    Private Sub BtnModelTexturesMore_Click(sender As Object, e As EventArgs) Handles BtnModelTexturesMore.Click
        If MyOhana.Model_Object IsNot Nothing Then
            FrmTextureInfo.LstModelTextures.Clear()

            Dim Header_1 As MyListview.ListItem
            ReDim Header_1.Text(2)
            Header_1.Text(0).Text = "#"
            Header_1.Text(1).Left = 40
            Header_1.Text(1).Text = "Texture"
            Header_1.Text(2).Left = 280
            Header_1.Text(2).Text = "Normal(Bump) map"
            Header_1.Header = True
            FrmTextureInfo.LstModelTextures.AddItem(Header_1)

            For Index As Integer = 0 To MyOhana.Model_Texture_Index.Length - 1
                Dim Item As MyListview.ListItem
                ReDim Item.Text(2)
                Item.Text(0).Text = Index.ToString
                Item.Text(1).Left = 40
                Item.Text(1).Text = MyOhana.Model_Texture_Index(Index)
                Item.Text(2).Left = 280
                Item.Text(2).Text = MyOhana.Model_Bump_Map_Index(Index)
                FrmTextureInfo.LstModelTextures.AddItem(Item)
            Next

            FrmTextureInfo.Show()
        End If
    End Sub

    Private Sub Update_Info()
        LblInfoVertices.Text = MyOhana.Info.Vertex_Count.ToString
        LblInfoTriangles.Text = MyOhana.Info.Triangles_Count.ToString
        LblInfoBones.Text = MyOhana.Info.Bones_Count.ToString
        LblInfoTextures.Text = MyOhana.Info.Textures_Count.ToString
    End Sub
    Private Sub PicMouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseDown
        If e.Button = MouseButtons.Left Then
            Rot_InitX = MousePosition.X
            Rot_InitY = MousePosition.Y
        ElseIf e.Button = MouseButtons.Right Then
            Mov_InitX = MousePosition.X
            Mov_InitY = MousePosition.Y
        End If
    End Sub
    Private Sub PicMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseUp
        If e.Button = MouseButtons.Left Then
            Rot_FinalX += (Rot_InitX - MousePosition.X)
            Rot_FinalY += (Rot_InitY - MousePosition.Y)
        ElseIf e.Button = MouseButtons.Right Then
            Mov_FinalX += (Mov_InitX - MousePosition.X)
            Mov_FinalY += (Mov_InitY - MousePosition.Y)
        End If
    End Sub
    Private Sub PicMouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseMove
        If Not Screen.Focused Then Screen.Select()

        If e.Button = MouseButtons.Left Then
            MyOhana.Rotation.X = (Rot_InitX - MousePosition.X) + Rot_FinalX
            MyOhana.Rotation.Y = (Rot_InitY - MousePosition.Y) + Rot_FinalY
        ElseIf e.Button = MouseButtons.Right Then
            MyOhana.Translation.X = (Mov_InitX - MousePosition.X) + Mov_FinalX
            MyOhana.Translation.Y = (Mov_InitY - MousePosition.Y) + Mov_FinalY
        End If
    End Sub
    Private Sub PicZoom(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseWheel
        If e.Delta > 0 Then
            MyOhana.Zoom += 1.5F
        Else
            MyOhana.Zoom -= 1.5F
        End If
    End Sub
#End Region

#Region "Textures"
    Private Sub BtnTextureOpen_Click(sender As Object, e As EventArgs) Handles BtnTextureOpen.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open Pokémon BCH Texture"
        OpenDlg.Filter = "BCH Texture|*.*"
        If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                LstTextures.Clear()
                MyOhana.Load_Textures(OpenDlg.FileName, Tex_BCH_Version)
                For Each Texture As Ohana.OhanaTexture In MyOhana.Model_Texture
                    LstTextures.AddItem(Texture.Name)
                Next
                LstTextures.Refresh()
            Catch
                MsgBox("Sorry, something went wrong." & vbCrLf & "Make sure you selected the correct BCH Version (X/Y or OR/AS).", vbExclamation, "Error")
            End Try
        End If
    End Sub
    Private Sub LstTextures_Click(sender As Object, e As EventArgs) Handles LstTextures.Click
        LstTextures_Container.Focus()
        If LstTextures.SelectedIndex > -1 Then
            With MyOhana.Model_Texture(LstTextures.SelectedIndex)
                ImgTexture.Image = .Image

                LblInfoTextureIndex.Text = LstTextures.SelectedIndex + 1 & "/" & MyOhana.Model_Texture.Count
                LblInfoTextureResolution.Text = .Image.Width & "x" & .Image.Height
                Select Case .Format
                    Case 0
                        LblInfoTextureFormat.Text = "32BPP"
                        LblInfoTextureCD.Text = "32BPP"
                    Case 1
                        LblInfoTextureFormat.Text = "24BPP"
                        LblInfoTextureCD.Text = "24BPP"
                    Case 2
                        LblInfoTextureFormat.Text = "RGBA5551"
                        LblInfoTextureCD.Text = "16BPP"
                    Case 3
                        LblInfoTextureFormat.Text = "RGB565"
                        LblInfoTextureCD.Text = "16BPP"
                    Case 4
                        LblInfoTextureFormat.Text = "RGBA4444"
                        LblInfoTextureCD.Text = "16BPP"
                    Case 5
                        LblInfoTextureFormat.Text = "L8A8 (Grayscale)"
                        LblInfoTextureCD.Text = "16BPP"
                    Case 6
                        LblInfoTextureFormat.Text = "HILO8"
                        LblInfoTextureCD.Text = "8BPP"
                    Case 7
                        LblInfoTextureFormat.Text = "L8 (Grayscale)"
                        LblInfoTextureCD.Text = "8BPP"
                    Case 8
                        LblInfoTextureFormat.Text = "A8 (Alpha only)"
                        LblInfoTextureCD.Text = "8BPP"
                    Case 9
                        LblInfoTextureFormat.Text = "L4A4 (Grayscale)"
                        LblInfoTextureCD.Text = "8BPP"
                    Case 10
                        LblInfoTextureFormat.Text = "L4 (Grayscale)"
                        LblInfoTextureCD.Text = "4BPP"
                    Case 12
                        LblInfoTextureFormat.Text = "ETC1 (iPACKMAN)"
                        LblInfoTextureCD.Text = "24BPP"
                    Case 13
                        LblInfoTextureFormat.Text = "ETC1 + Alpha"
                        LblInfoTextureCD.Text = "32BPP"
                End Select
            End With
        End If
    End Sub
    Private Sub BtnTextureExport_Click(sender As Object, e As EventArgs) Handles BtnTextureExport.Click
        If MyOhana.Model_Texture IsNot Nothing Then
            If LstTextures.SelectedIndex > -1 Then
                Dim SaveDlg As New SaveFileDialog
                SaveDlg.Title = "Save image"
                SaveDlg.Filter = "Image|*.png"
                If SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim Img As New Bitmap(MyOhana.Model_Texture(LstTextures.SelectedIndex).Image)
                    If Texture_Mode = TextureMode.FlipY Or Texture_Mode = TextureMode.FlipY_Mirror Then Img.RotateFlip(RotateFlipType.RotateNoneFlipY)
                    If Texture_Mode = TextureMode.Mirror Or Texture_Mode = TextureMode.FlipY_Mirror Then Img = Mirror_Image(Img)
                    Img.Save(SaveDlg.FileName)
                End If
            End If
        End If
    End Sub
    Private Sub BtnTextureExportAll_Click(sender As Object, e As EventArgs) Handles BtnTextureExportAll.Click
        If MyOhana.Model_Texture IsNot Nothing Then
            Dim OutputDlg As New FolderBrowserDialog
            If OutputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                For Each Texture As Ohana.OhanaTexture In MyOhana.Model_Texture
                    Dim File_Name As String = Path.Combine(OutputDlg.SelectedPath, Texture.Name & ".png")
                    Dim Img As New Bitmap(Texture.Image)
                    If Texture_Mode = TextureMode.FlipY Or Texture_Mode = TextureMode.FlipY_Mirror Then Img.RotateFlip(RotateFlipType.RotateNoneFlipY)
                    If Texture_Mode = TextureMode.Mirror Or Texture_Mode = TextureMode.FlipY_Mirror Then Img = Mirror_Image(Img)
                    Img.Save(File_Name)
                Next
            End If
        End If
    End Sub
    Private Sub BtnTextureExportAllFF_Click(sender As Object, e As EventArgs) Handles BtnTextureExportAllFF.Click
        If Texture_Export_Thread IsNot Nothing Then
            If Texture_Export_Thread.IsAlive Then
                Texture_Export_Thread.Abort()
                BtnTextureExportAllFF.Text = "Export all from folder"
                ProgressTextures.Text = Nothing
                ProgressTextures.Percentage = 0
                ProgressTextures.Refresh()

                Exit Sub
            End If
        End If

        Dim InputDlg As New FolderBrowserDialog
        If InputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim OutputDlg As New FolderBrowserDialog
            If OutputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                Texture_Export_Thread = New Thread(Sub() Texture_Exporter(InputDlg.SelectedPath, OutputDlg.SelectedPath))
                Texture_Export_Thread.Start()

                BtnTextureExportAllFF.Text = "Cancel"
            End If
        End If
    End Sub
    Private Sub Texture_Exporter(InFolder As String, OutFolder As String)
        Dim Exporter As New Ohana
        Dim Input_Files() As FileInfo = New DirectoryInfo(InFolder).GetFiles()
        Dim Total_Index, Index As Integer
        For Each File As FileInfo In Input_Files
            Try
                Exporter.Load_Textures(File.FullName, Tex_BCH_Version, False)
                If Exporter.Model_Texture.Count > 0 Then
                    Dim Output_Folder As String = Path.Combine(OutFolder, "group_" & Index)
                    Directory.CreateDirectory(Output_Folder)
                    For Each Texture As Ohana.OhanaTexture In Exporter.Model_Texture
                        Dim File_Name As String = Path.Combine(Output_Folder, Texture.Name & ".png")
                        If Texture_Mode = TextureMode.Original Then
                            Texture.Image.Save(File_Name)
                        Else
                            Dim Img As New Bitmap(Texture.Image)
                            If Texture_Mode = TextureMode.FlipY Or Texture_Mode = TextureMode.FlipY_Mirror Then Img.RotateFlip(RotateFlipType.RotateNoneFlipY)
                            If Texture_Mode = TextureMode.Mirror Or Texture_Mode = TextureMode.FlipY_Mirror Then Img = Mirror_Image(Img)
                            Img.Save(File_Name)
                        End If
                    Next
                    Index += 1
                End If
            Catch ex As Exception
                Debug.WriteLine("Texture Exporter -> Erro ao exportar textura: " & Path.GetFileName(File.FullName))
            End Try
            Update_Progress(ProgressTextures, Convert.ToSingle((Total_Index / Input_Files.Count) * 100), "Exporting " & Path.GetFileName(File.FullName) & "...")
            Total_Index += 1
        Next

        Update_Progress(ProgressTextures, 0, Nothing)
        Update_Button_Text(BtnTextureExportAllFF, "Export all from folder")
    End Sub
    Private Sub BtnTextureBCHVer_Click(sender As Object, e As EventArgs) Handles BtnTextureBCHVer.Click
        If Tex_BCH_Version = Ohana.BCH_Version.XY Then
            Tex_BCH_Version = Ohana.BCH_Version.ORAS
            BtnTextureBCHVer.Text = "OR/AS"
        ElseIf Tex_BCH_Version = Ohana.BCH_Version.ORAS Then
            Tex_BCH_Version = Ohana.BCH_Version.XY
            BtnTextureBCHVer.Text = "X/Y"
        End If
    End Sub
    Private Sub BtnTextureMode_Click(sender As Object, e As EventArgs) Handles BtnTextureMode.Click
        Select Case Texture_Mode
            Case TextureMode.Original
                Texture_Mode = TextureMode.FlipY
                BtnTextureMode.Text = "Flip-Y"
            Case TextureMode.FlipY
                Texture_Mode = TextureMode.Mirror
                BtnTextureMode.Text = "Mirror-X"
            Case TextureMode.Mirror
                Texture_Mode = TextureMode.FlipY_Mirror
                BtnTextureMode.Text = "Flip/Mirror"
            Case TextureMode.FlipY_Mirror
                Texture_Mode = TextureMode.Original
                BtnTextureMode.Text = "Original"
        End Select
    End Sub

    Private Function Mirror_Image(Img As Bitmap) As Bitmap
        Dim ImgData As BitmapData = Img.LockBits(New Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
        Dim Data(ImgData.Height * ImgData.Stride) As Byte
        Marshal.Copy(ImgData.Scan0, Data, 0, Data.Length)

        Dim Out(((Img.Width * 2) * Img.Height * 4) - 1) As Byte
        For Y As Integer = 0 To Img.Height - 1
            For X As Integer = 0 To Img.Width - 1
                Dim Offset As Integer = (X + (Y * Img.Width)) * 4
                Dim Offset_2 As Integer = (X + (Y * (Img.Width * 2))) * 4
                Dim Offset_3 As Integer = ((Img.Width + (Img.Width - X - 1)) + (Y * (Img.Width * 2))) * 4
                Buffer.BlockCopy(Data, Offset, Out, Offset_2, 4)
                Buffer.BlockCopy(Data, Offset, Out, Offset_3, 4)
            Next
        Next

        Dim Output As New Bitmap(Img.Width * 2, Img.Height, PixelFormat.Format32bppArgb)
        Dim OutData As BitmapData = Output.LockBits(New Rectangle(0, 0, Output.Width, Output.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)
        Marshal.Copy(Out, 0, OutData.Scan0, Out.Length)
        Output.UnlockBits(OutData)

        Return Output
    End Function
#End Region

#Region "GARC"
    Private Sub BtnOpenGARC_Click(sender As Object, e As EventArgs) Handles BtnOpenGARC.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open GARC container"
        OpenDlg.Filter = "GARC file|*.*"
        If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            MyNako.Load(OpenDlg.FileName)
            Dim Index As Integer
            LstFiles.Clear()

            Dim Header As MyListview.ListItem
            ReDim Header.Text(2)
            Header.Text(0).Text = "File"
            Header.Text(1).Left = 400
            Header.Text(1).Text = "Compressed size"
            Header.Text(2).Left = 500
            Header.Text(2).Text = "Uncompressed size"
            Header.Header = True
            LstFiles.AddItem(Header)

            For Each File As Nako.GARC_File In MyNako.Files
                Dim Item As MyListview.ListItem
                ReDim Item.Text(2)
                With File
                    Item.Text(0).Text = "file_" & Index
                    Item.Text(1).Left = 400
                    If .Compressed Then
                        Item.Text(1).Text = Format_Size(.Length)
                    Else
                        Item.Text(1).Text = "---"
                    End If
                    Item.Text(2).Left = 500
                    Item.Text(2).Text = Format_Size(.Uncompressed_Length)
                End With
                LstFiles.AddItem(Item)

                Index += 1
            Next
            LstFiles.Refresh()
        End If
    End Sub
    Private Function Format_Size(Bytes As Integer) As String
        If Bytes >= 1073741824 Then
            Return Format(Bytes / 1024 / 1024 / 1024, "#0.00") & " GB"
        ElseIf Bytes >= 1048576 Then
            Return Format(Bytes / 1024 / 1024, "#0.00") & " MB"
        ElseIf Bytes >= 1024 Then
            Return Format(Bytes / 1024, "#0.00") & " KB"
        Else
            Return Bytes.ToString & " B"
        End If
    End Function
    Private Sub BtnGARCExtract_Click(sender As Object, e As EventArgs) Handles BtnGARCExtract.Click
        If MyNako.Files IsNot Nothing Then
            If LstFiles.SelectedIndex > -1 Then
                Dim SaveDlg As New SaveFileDialog
                SaveDlg.Title = "Extract file"
                If SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    MyNako.Extract(SaveDlg.FileName, LstFiles.SelectedIndex)
                End If
            End If
        End If
    End Sub
    Private Sub BtnGARCExtractAll_Click(sender As Object, e As EventArgs) Handles BtnGARCExtractAll.Click
        If MyNako.Files IsNot Nothing Then
            If GARC_Thread IsNot Nothing Then
                If GARC_Thread.IsAlive Then
                    GARC_Thread.Abort()
                    BtnGARCExtractAll.Text = "Extract all"
                    ProgressGARC.Text = Nothing
                    ProgressGARC.Percentage = 0
                    ProgressGARC.Refresh()

                    Exit Sub
                End If
            End If

            Dim OutputDlg As New FolderBrowserDialog
            If OutputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                GARC_Thread = New Thread(Sub() GARC_ExtractAll(OutputDlg.SelectedPath))
                GARC_Thread.Start()

                BtnGARCExtractAll.Text = "Cancel"
            End If
        End If
    End Sub
    Private Sub GARC_ExtractAll(OutFolder As String)
        For Index As Integer = 0 To MyNako.Files.Length - 1
            MyNako.Extract(Path.Combine(OutFolder, "file_" & Index), Index)
            Update_Progress(ProgressGARC, Convert.ToSingle((Index / (MyNako.Files.Length - 1)) * 100), "Extracting file_" & Index & "...")
        Next

        Update_Progress(ProgressGARC, 0, Nothing)
        Update_Button_Text(BtnGARCExtractAll, "Extract all")
    End Sub
#End Region

#Region "Search"
    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If TxtSearch.Text <> Nothing Then
            If Search_Thread IsNot Nothing Then
                If Search_Thread.IsAlive Then
                    Search_Thread.Abort()
                    BtnSearch.Text = "Search"
                    ProgressSearch.Text = Nothing
                    ProgressSearch.Percentage = 0
                    ProgressSearch.Refresh()

                    Exit Sub
                End If
            End If

            Dim InputDlg As New FolderBrowserDialog
            If InputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                LstMatches.Clear()
                Dim Header As MyListview.ListItem
                ReDim Header.Text(1)
                Header.Text(0).Text = "Name"
                Header.Text(1).Left = 400
                Header.Text(1).Text = "Offset"
                Header.Header = True
                LstMatches.AddItem(Header)
                LstMatches.Refresh()

                Search_Thread = New Thread(Sub() File_String_Search(InputDlg.SelectedPath, TxtSearch.Text))
                Search_Thread.Start()

                BtnSearch.Text = "Cancel"
            End If
        End If
    End Sub
    Private Sub File_String_Search(InFolder As String, Text As String)
        Dim Index As Integer
        Dim Input_Files() As FileInfo = New DirectoryInfo(InFolder).GetFiles()
        Dim Search_Term() As Byte = Encoding.UTF8.GetBytes(Text)
        For Each CurrFile As FileInfo In Input_Files
            Dim Data() As Byte = File.ReadAllBytes(CurrFile.FullName)
            Dim Found As Boolean = False
            For Offset As Integer = 0 To Data.Length - Search_Term.Length
                For Offset_2 As Integer = 0 To Search_Term.Length - 1
                    If Data(Offset + Offset_2) <> Search_Term(Offset_2) Then
                        Exit For
                    ElseIf Offset_2 = Search_Term.Length - 1 Then
                        Dim Item As MyListview.ListItem
                        ReDim Item.Text(1)
                        Item.Text(0).Text = CurrFile.Name
                        Item.Text(1).Left = 400
                        Item.Text(1).Text = "0x" & Hex(Offset)
                        Add_List_Item(LstMatches, Item)
                        Found = True
                        Exit For
                    End If
                Next
                If Found Then Exit For
            Next

            Update_Progress(ProgressSearch, Convert.ToSingle((Index / (Input_Files.Count - 1)) * 100), "Searching on file " & CurrFile.Name & "...")
            Index += 1
        Next

        Update_Progress(ProgressSearch, 0, Nothing)
        Update_Button_Text(BtnSearch, "Search")
    End Sub
#End Region

#End Region

End Class
