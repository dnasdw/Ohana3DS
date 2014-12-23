Imports System.IO
Imports System.Threading
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Public Class FrmMain

#Region "Declares"
    'Classes de textos, Compressão/Extração
    Dim MyMinko As New Minko

    'Movimentação do modelo
    Dim Rot_InitX, Rot_InitY, Rot_FinalX, Rot_FinalY As Integer
    Dim Mov_InitX, Mov_InitY, Mov_FinalX, Mov_FinalY As Integer

    Dim Current_Model As String
    Dim Current_Opened_Text As String
    Dim Current_Text_Temp As String

    Private Enum TextureMode
        Original
        FlipY
        Mirror
        FlipY_Mirror
    End Enum
    Dim Texture_Mode As TextureMode = TextureMode.Original

    Dim Model_Export_Thread As Thread
    Dim Texture_Thread As Thread
    Dim GARC_Thread As Thread
    Dim Search_Thread As Thread

    Dim Lighting As Boolean = True
    Dim First_Click As Boolean

    Private Structure NCCH
        Dim Offset As Integer
        Dim Length As Integer
        Dim Product_Code As String
        Dim Partition_ID() As Byte
        Dim ExHeader_Size As Long
        Dim ExeFS_Offset, ExeFS_Size As Long
        Dim RomFS_Offset, RomFS_Size As Long
    End Structure
    Dim NCCH_Container(7) As NCCH
    Private Structure Info_Header
        Dim Offset As Integer
        Dim Length As Integer
    End Structure
    Private Structure Rom_Directory
        Dim Parent_Offset As Integer
        Dim Sibling_Offset As Integer
        Dim Children_Offset As Integer
        Dim File_Offset As Integer
        Dim Unknow As Integer
        Dim Name As String
    End Structure
    Private Structure Rom_File
        Dim Parent_Offset As Integer
        Dim Sibling_Offset As Integer
        Dim Data_Offset As UInt64
        Dim Data_Length As UInt64
        Dim Unknow As Integer
        Dim Name As String
    End Structure
    Dim Current_ROM, Current_XORPad As String
#End Region

#Region "GUI"

#Region "General"
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg <> &HA3 Then MyBase.WndProc(m)
        Select Case m.Msg
            Case &H84
                Dim Mouse_Position As Point = PointToClient(Cursor.Position)
                If Mouse_Position.Y < 32 Then If m.Result = New IntPtr(1) Then m.Result = New IntPtr(2)
        End Select
    End Sub
    Private Delegate Sub FileDrop(File_Name As String)
    Private MyFileDrop As FileDrop
    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For i As Integer = 0 To 7
            Power_Of_Two(i) = Convert.ToByte(2 ^ i)
        Next

        Me.AllowDrop = True
        MyFileDrop = New FileDrop(AddressOf File_Dropped)

        MyOhana.Scale = My.Settings.ModelScale
        BtnModelScale.Text = "Model scale: 1:" & My.Settings.ModelScale
        Select Case My.Settings.TextureFlipMirror
            Case 0
                BtnTextureMode.Text = "Original"
                Texture_Mode = TextureMode.Original
            Case 1
                BtnTextureMode.Text = "Flip-Y"
                Texture_Mode = TextureMode.FlipY
            Case 2
                BtnTextureMode.Text = "Mirror-X"
                Texture_Mode = TextureMode.Mirror
            Case 3
                BtnTextureMode.Text = "Flip/Mirror"
                Texture_Mode = TextureMode.FlipY_Mirror
        End Select
        MyNako.Fast_Compression = My.Settings.FastCompression
        If MyNako.Fast_Compression Then BtnGARCCompression.Text = "Fast compression"

        Disable_Model_Buttons()
        Disable_Texture_Buttons()
        Disable_Text_Buttons()
        Disable_GARC_Buttons()
        BtnROMDecrypt.Enabled = False

        MyOhana.Initialize(Screen)
        Show()
        MyOhana.Render()
    End Sub
    Private Sub FrmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If File.Exists(Current_Model) Then
            If e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then
                Dim Model_Name As String = Path.GetFileName(Current_Model)
                Dim Input_Files() As FileInfo = New DirectoryInfo(Path.GetDirectoryName(Current_Model)).GetFiles()
                Select Case e.KeyCode
                    Case Keys.Left
                        For Index As Integer = 1 To Input_Files.Count - 1
                            If Input_Files(Index).Name = Model_Name Then
                                Dim i As Integer = 1
                                Do
                                    Dim CurrFile As String = Input_Files(Index - i).FullName
                                    If IsModel(CurrFile) Then
                                        If Open_Model(CurrFile, False) Then Exit Sub
                                    End If
                                    i += 1
                                    If Index - i < 1 Then Exit For
                                Loop
                                Exit For
                            End If
                        Next
                    Case Keys.Right
                        For Index As Integer = 0 To Input_Files.Count - 2
                            If Input_Files(Index).Name = Model_Name Then
                                Dim i As Integer = 1
                                Do
                                    Dim CurrFile As String = Input_Files(Index + i).FullName
                                    If IsModel(CurrFile) Then
                                        If Open_Model(CurrFile, False) Then Exit Sub
                                    End If
                                    i += 1
                                    If Index + i > Input_Files.Count - 2 Then Exit Sub
                                Loop
                                Exit For
                            End If
                        Next
                End Select
            End If

            Select Case e.KeyCode
                Case Keys.F9
                    MyOhana.Lighting = Not MyOhana.Lighting
                    MyOhana.Switch_Lighting(MyOhana.Lighting)
            End Select
        End If
    End Sub
    Private Sub FrmMain_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        Dim Files() As String = DirectCast(e.Data.GetData(DataFormats.FileDrop), String())
        Me.BeginInvoke(MyFileDrop, Files(0))
        Me.Activate()
    End Sub
    Private Sub FrmMain_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub Splash_Click(sender As Object, e As EventArgs) Handles Splash.Click
        MainTabs.Visible = True
        Splash.Visible = False
    End Sub
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

    Private Sub File_Dropped(File_Name As String)
        Dim Temp As New FileStream(File_Name, FileMode.Open)

        Dim Magic_2_Bytes As String = ReadMagic(Temp, 0, 2)
        Dim Magic_3_Bytes As String = ReadMagic(Temp, 0, 3)
        Dim Magic_4_Bytes As String = ReadMagic(Temp, 0, 4)
        Dim CLIM_Magic As String = ReadMagic(Temp, Convert.ToInt32(Temp.Length - 40), 4)
        Dim Text_Check As Boolean = Read32(Temp, 4) = Temp.Length - &H10
        Temp.Close()

        If Magic_2_Bytes = "PC" Or Magic_2_Bytes = "MM" Or Magic_2_Bytes = "GR" Or Magic_3_Bytes = "BCH" Then
            MainTabs.SelectTab(0)
            Open_Model(File_Name)
        ElseIf Magic_2_Bytes = "PT" Or CLIM_Magic = "CLIM" Then
            MainTabs.SelectTab(1)
            Open_Texture(File_Name)
        ElseIf Magic_4_Bytes = "CRAG" Then
            MainTabs.SelectTab(3)
            Open_GARC(File_Name)
        ElseIf Text_Check Then
            MainTabs.SelectTab(2)
            Open_Text(File_Name)
        End If
    End Sub
    Private Function IsModel(File_Name As String) As Boolean
        Dim Input As New FileStream(File_Name, FileMode.Open)
        Dim Magic As String = ReadMagic(Input, 0, 3)
        Dim Magic_2_Bytes As String = Magic.Substring(0, 2)
        Dim Length As Long = Input.Length
        Input.Close()
        If (Magic_2_Bytes <> "MM" And _
            Magic_2_Bytes <> "TM" And _
            Magic_2_Bytes <> "PC" And _
            Magic_2_Bytes <> "GR" And _
            Magic <> "BCH") Or _
            Length < &H80 Then 'Verifica se é um modelo válido
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Common"
    Private Delegate Sub Up_Progress(ProgressBar As MyProgressbar, Percentage As Single, Msg As String)
    Private Delegate Sub Add_Item(List As MyListview, Item As MyListview.ListItem, Scroll_To_End As Boolean)
    Private Delegate Sub Update_Button(Button As Button, Text As String)
    Private Delegate Sub Change_Picture(Ctrl As MyPicturebox, Img As Image)
    Private Delegate Sub Change_Enabled(Ctrl As Control, Enabled As Boolean)
    Private Sub Update_Progress(ProgressBar As MyProgressbar, Percentage As Single, Msg As String)
        If ProgressBar.InvokeRequired Then
            Me.Invoke(New Up_Progress(AddressOf Update_Progress), ProgressBar, Percentage, Msg)
        Else
            ProgressBar.Text = Msg
            ProgressBar.Percentage = Percentage
            ProgressBar.Refresh()
        End If
    End Sub
    Private Sub Add_List_Item(List As MyListview, Item As MyListview.ListItem, Scroll_To_End As Boolean)
        If List.InvokeRequired Then
            Me.Invoke(New Add_Item(AddressOf Add_List_Item), List, Item, Scroll_To_End)
        Else
            List.AddItem(Item)
            If Scroll_To_End Then
                List.Scroll_To_End()
                List.Refresh()
            End If
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
    Private Sub Set_Image(Picture As MyPicturebox, Img As Image)
        If Picture.InvokeRequired Then
            Me.Invoke(New Change_Picture(AddressOf Set_Image), Picture, Img)
        Else
            Picture.Image = Img
            Picture.Refresh()
        End If
    End Sub
    Private Sub Set_Enabled(Ctrl As Control, Enabled As Boolean)
        If Ctrl.InvokeRequired Then
            Me.Invoke(New Change_Enabled(AddressOf Set_Enabled), Ctrl, Enabled)
        Else
            Ctrl.Enabled = Enabled
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
#End Region

#Region "Model"
    Private Sub BtnModelOpen_Click(sender As Object, e As EventArgs) Handles BtnModelOpen.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open Pokémon BCH model"
        OpenDlg.Filter = "BCH Model|*.*"
        If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            First_Click = True
            If File.Exists(OpenDlg.FileName) Then Open_Model(OpenDlg.FileName)
        End If
    End Sub
    Private Function Open_Model(File_Name As String, Optional Show_Warning As Boolean = True) As Boolean
        Dim Response As Boolean

        Try
            Current_Model = File_Name
            MyOhana.Rendering = False
            Response = MyOhana.Load_Model(File_Name)
            If Response Then
                MyOhana.Rendering = True
                LblModelName.Text = Path.GetFileName(File_Name)
                ModelNameTip.SetToolTip(LblModelName, LblModelName.Text)

                If MyOhana.BCH_Have_Textures Then
                    Enable_Texture_Buttons()
                    LstTextures.Clear()
                    ImgTexture.Image = Nothing

                    For Index As Integer = 0 To MyOhana.Model_Texture.Count - 1
                        LstTextures.AddItem(MyOhana.Model_Texture(Index).Name)
                    Next
                End If

                Rot_InitX = 0
                Rot_InitY = 0
                Rot_FinalX = 0
                Rot_FinalY = 0
                Mov_InitX = 0
                Mov_InitY = 0
                Mov_FinalX = 0
                Mov_FinalY = 0

                MyOhana.Rotation.X = 0
                MyOhana.Rotation.Y = 0
                MyOhana.Translation.X = 0
                MyOhana.Translation.Y = 0

                LblInfoVertices.Text = MyOhana.Info.Vertex_Count.ToString()
                LblInfoTriangles.Text = MyOhana.Info.Triangles_Count.ToString()
                LblInfoBones.Text = MyOhana.Info.Bones_Count.ToString()
                LblInfoTextures.Text = MyOhana.Info.Textures_Count.ToString()

                Enable_Model_Buttons()
                If MyOhana.Magic.Substring(0, 2) = "GR" Then
                    BtnModelMapEditor.Enabled = True
                    If FrmMapProp.IsHandleCreated Then FrmMapProp.makeMapIMG(MapProps())
                Else
                    BtnModelMapEditor.Enabled = False
                End If
            Else
                If Show_Warning Then MessageBox.Show("This file is not a model file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch
            Response = False
            MyOhana.Model_Object = Nothing
            If Show_Warning Then MessageBox.Show("Sorry, something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        If Not Response Then
            LblModelName.Text = Nothing
            ModelNameTip.SetToolTip(LblModelName, Nothing)
            LblInfoVertices.Text = "0"
            LblInfoTriangles.Text = "0"
            LblInfoBones.Text = "0"
            LblInfoTextures.Text = "0"
            Disable_Model_Buttons()
        End If

        Application.DoEvents() 'Processa o click que foi para o PictureBox, porém será ignorado devido ao First_Click
        First_Click = False
        Screen.Refresh()

        Return Response
    End Function
    Private Sub Enable_Model_Buttons()
        BtnModelExport.Enabled = True
        BtnModelSave.Enabled = True
        BtnModelVertexEditor.Enabled = True
        BtnModelTexturesMore.Enabled = True
    End Sub
    Private Sub Disable_Model_Buttons()
        BtnModelExport.Enabled = False
        BtnModelSave.Enabled = False
        BtnModelVertexEditor.Enabled = False
        BtnModelMapEditor.Enabled = False
        BtnModelTexturesMore.Enabled = False
    End Sub
    Private Sub BtnModelExport_Click(sender As Object, e As EventArgs) Handles BtnModelExport.Click
        If MyOhana.Model_Object IsNot Nothing Then
            Dim SaveDlg As New SaveFileDialog
            SaveDlg.Title = "Save model"
            SaveDlg.Filter = "Valve SMD|*.smd"
            SaveDlg.FileName = Path.GetFileNameWithoutExtension(MyOhana.Current_Model) & ".smd"
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
                If Exporter.Load_Model(File.FullName, False) Then
                    If Exporter.Model_Object.Length > 0 Then
                        Dim File_Name As String = Path.Combine(OutFolder, File.Name & ".smd")
                        Exporter.Export_SMD(File_Name)
                        Index += 1
                    End If
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
    Private Sub BtnModelScale_Click(sender As Object, e As EventArgs) Handles BtnModelScale.Click
        If MyOhana.Scale = 1 Then
            MyOhana.Scale = 32
            BtnModelScale.Text = "Model scale: 1:32"
        ElseIf MyOhana.Scale = 32 Then
            MyOhana.Scale = 64
            BtnModelScale.Text = "Model scale: 1:64"
        ElseIf MyOhana.Scale = 64 Then
            MyOhana.Scale = 1
            BtnModelScale.Text = "Model scale: 1:1"
        End If
        My.Settings.ModelScale = Convert.ToInt32(MyOhana.Scale)
        My.Settings.Save()
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
                If MyOhana.Model_Texture IsNot Nothing Then
                    Dim Found As Boolean = False
                    For Texture_Index As Integer = 0 To MyOhana.Model_Texture.Count - 1
                        If MyOhana.Model_Texture(Texture_Index).Name = MyOhana.Model_Texture_Index(Index) Then
                            Found = True
                            Exit For
                        End If
                    Next
                    If Not Found Then Item.Text(1).ForeColor = Color.Crimson
                Else
                    Item.Text(1).ForeColor = Color.Crimson
                End If

                Item.Text(2).Left = 280
                Item.Text(2).Text = MyOhana.Model_Bump_Map_Index(Index)
                FrmTextureInfo.LstModelTextures.AddItem(Item)
            Next

            FrmTextureInfo.Show()
            FrmTextureInfo.LstModelTextures.Refresh()
        End If
    End Sub
    Private Sub BtnModelVertexEditor_Click(sender As Object, e As EventArgs) Handles BtnModelVertexEditor.Click
        If MyOhana.Model_Object IsNot Nothing Then FrmVertexEditor.Show()
    End Sub
    Private Sub BtnModelSave_Click(sender As Object, e As EventArgs) Handles BtnModelSave.Click
        If MyOhana.Current_Model IsNot Nothing Then
            File.Delete(MyOhana.Current_Model)
            File.Copy(MyOhana.Temp_Model_File, MyOhana.Current_Model)
        End If
    End Sub

    Private Sub Screen_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseDown
        If Not First_Click Then
            If e.Button = MouseButtons.Left Then
                Rot_InitX = MousePosition.X
                Rot_InitY = MousePosition.Y
            ElseIf e.Button = MouseButtons.Right Then
                Mov_InitX = MousePosition.X
                Mov_InitY = MousePosition.Y
            End If
        End If
    End Sub
    Private Sub Screen_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseUp
        If Not First_Click Then
            If e.Button = MouseButtons.Left Then
                Rot_FinalX += (Rot_InitX - MousePosition.X)
                Rot_FinalY += (Rot_InitY - MousePosition.Y)
            ElseIf e.Button = MouseButtons.Right Then
                Mov_FinalX += (Mov_InitX - MousePosition.X)
                Mov_FinalY += (Mov_InitY - MousePosition.Y)
            End If
        End If
    End Sub
    Private Sub Screen_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseMove
        If Not Screen.Focused Then Screen.Select()
        If Not First_Click Then
            If e.Button = MouseButtons.Left Then
                MyOhana.Rotation.X = (Rot_InitX - MousePosition.X) + Rot_FinalX
                MyOhana.Rotation.Y = (Rot_InitY - MousePosition.Y) + Rot_FinalY
            ElseIf e.Button = MouseButtons.Right Then
                MyOhana.Translation.X = (Mov_InitX - MousePosition.X) + Mov_FinalX
                MyOhana.Translation.Y = (Mov_InitY - MousePosition.Y) + Mov_FinalY
            End If
        End If
    End Sub
    Private Sub Screen_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseWheel
        Dim z As Single = If((Control.ModifierKeys And Keys.Control) = Keys.Control, 0.05F, 1.0F)
        If e.Delta > 0 Then
            MyOhana.Zoom += z
        Else
            MyOhana.Zoom -= z
        End If
    End Sub
#End Region

#Region "Textures"
    Private Sub BtnTextureOpen_Click(sender As Object, e As EventArgs) Handles BtnTextureOpen.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open Pokémon BCH Texture"
        OpenDlg.Filter = "BCH Texture|*.*"
        If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            If File.Exists(OpenDlg.FileName) Then Open_Texture(OpenDlg.FileName)
        End If
    End Sub
    Private Sub Open_Texture(File_Name As String, Optional Show_Warning As Boolean = True)
        Try
            LstTextures.Clear()
            ImgTexture.Image = Nothing

            MyOhana.Load_Textures(File_Name)
            If MyOhana.Model_Texture.Count > 0 Then
                Enable_Texture_Buttons()
                For Each Texture As Ohana.OhanaTexture In MyOhana.Model_Texture
                    LstTextures.AddItem(Texture.Name)
                Next
                LstTextures.SelectedIndex = 0
                LstTextures.Refresh()
                Select_Texture(0)
            Else
                Disable_Texture_Buttons()
            End If
        Catch
            Disable_Texture_Buttons()
            If Show_Warning Then MessageBox.Show("Sorry, something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Enable_Texture_Buttons()
        BtnTextureExport.Enabled = True
        BtnTextureExportAll.Enabled = True
        BtnTextureInsert.Enabled = True
        BtnTextureInsertAll.Enabled = True
        BtnTextureSave.Enabled = True
    End Sub
    Private Sub Disable_Texture_Buttons()
        BtnTextureExport.Enabled = False
        BtnTextureExportAll.Enabled = False
        BtnTextureInsert.Enabled = False
        BtnTextureInsertAll.Enabled = False
        BtnTextureSave.Enabled = False
    End Sub
    Private Sub LstTextures_SelectedIndexChanged(Index As Integer) Handles LstTextures.SelectedIndexChanged
        If Index > -1 Then Select_Texture(Index)
    End Sub
    Private Sub Select_Texture(Index As Integer)
        With MyOhana.Model_Texture(Index)
            ImgTexture.Image = .Image
            ImgTexture.Refresh()

            LblInfoTextureIndex.Text = Index + 1 & "/" & MyOhana.Model_Texture.Count
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
    End Sub
    Private Sub BtnTextureExport_Click(sender As Object, e As EventArgs) Handles BtnTextureExport.Click
        If MyOhana.Model_Texture IsNot Nothing Then
            If LstTextures.SelectedIndex > -1 Then
                Dim SaveDlg As New SaveFileDialog
                SaveDlg.Title = "Save image"
                SaveDlg.Filter = "Image|*.png"
                SaveDlg.FileName = MyOhana.Model_Texture(LstTextures.SelectedIndex).Name & ".png"
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
        If Texture_Thread IsNot Nothing Then
            If Texture_Thread.IsAlive Then
                Texture_Thread.Abort()
                BtnTextureExportAllFF.Text = "Export all from folder"
                ProgressTextures.Text = Nothing
                ProgressTextures.Percentage = 0
                ProgressTextures.Refresh()

                BtnTextureOpen.Enabled = True
                BtnTextureSave.Enabled = True
                BtnTextureInsert.Enabled = True
                BtnTextureInsertAll.Enabled = True

                Exit Sub
            End If
        End If

        Dim InputDlg As New FolderBrowserDialog
        If InputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim OutputDlg As New FolderBrowserDialog
            If OutputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                Texture_Thread = New Thread(Sub() Texture_Exporter(InputDlg.SelectedPath, OutputDlg.SelectedPath))
                Texture_Thread.Start()

                BtnTextureExportAllFF.Text = "Cancel"
                BtnTextureOpen.Enabled = False
                BtnTextureSave.Enabled = False
                BtnTextureInsert.Enabled = False
                BtnTextureInsertAll.Enabled = False
            End If
        End If
    End Sub
    Private Sub Texture_Exporter(InFolder As String, OutFolder As String)
        Dim Exporter As New Ohana
        Dim Input_Files() As FileInfo = New DirectoryInfo(InFolder).GetFiles()
        Dim Total_Index, Index As Integer
        For Each File As FileInfo In Input_Files
            Try
                Exporter.Load_Textures(File.FullName, False)
                If Exporter.Model_Texture.Count > 0 Then
                    Dim Output_Folder As String = Path.Combine(OutFolder, Path.GetFileNameWithoutExtension(File.Name))
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

        Set_Enabled(BtnTextureOpen, True)
        Set_Enabled(BtnTextureSave, True)
        Set_Enabled(BtnTextureInsert, True)
        Set_Enabled(BtnTextureInsertAll, True)
    End Sub
    Private Sub BtnTextureMode_Click(sender As Object, e As EventArgs) Handles BtnTextureMode.Click
        Select Case Texture_Mode
            Case TextureMode.Original
                Texture_Mode = TextureMode.FlipY
                BtnTextureMode.Text = "Flip-Y"
                My.Settings.TextureFlipMirror = 1
            Case TextureMode.FlipY
                Texture_Mode = TextureMode.Mirror
                BtnTextureMode.Text = "Mirror-X"
                My.Settings.TextureFlipMirror = 2
            Case TextureMode.Mirror
                Texture_Mode = TextureMode.FlipY_Mirror
                BtnTextureMode.Text = "Flip/Mirror"
                My.Settings.TextureFlipMirror = 3
            Case TextureMode.FlipY_Mirror
                Texture_Mode = TextureMode.Original
                BtnTextureMode.Text = "Original"
                My.Settings.TextureFlipMirror = 0
        End Select
        My.Settings.Save()
    End Sub
    Private Sub BtnTextureInsert_Click(sender As Object, e As EventArgs) Handles BtnTextureInsert.Click
        If Texture_Thread IsNot Nothing Then
            If Texture_Thread.IsAlive Then
                Texture_Thread.Abort()
                BtnTextureInsert.Text = "Import"
                ProgressTextures.Text = Nothing
                ProgressTextures.Percentage = 0
                ProgressTextures.Refresh()

                BtnTextureOpen.Enabled = True
                BtnTextureSave.Enabled = True
                BtnTextureExportAllFF.Enabled = True
                BtnTextureInsertAll.Enabled = True

                Exit Sub
            End If
        End If

        If (MyOhana.Current_Texture <> Nothing Or MyOhana.BCH_Have_Textures) And LstTextures.SelectedIndex > -1 Then
            Dim OpenDlg As New OpenFileDialog
            OpenDlg.Title = "Select the Texture to insert"
            OpenDlg.Filter = "PNG|*.png"
            If OpenDlg.ShowDialog = DialogResult.OK Then
                If File.Exists(OpenDlg.FileName) Then
                    Dim Trd As New Thread(Sub() Insert_Texture(OpenDlg.FileName, LstTextures.SelectedIndex))
                    Trd.Start()

                    BtnTextureInsert.Text = "Cancel"
                    BtnTextureOpen.Enabled = False
                    BtnTextureSave.Enabled = False
                    BtnTextureExportAllFF.Enabled = False
                    BtnTextureInsertAll.Enabled = False
                End If
            End If
        End If
    End Sub
    Private Sub Insert_Texture(File_Name As String, Index As Integer)
        Update_Progress(ProgressTextures, 0, "Inserting data...")

        Texture_Thread = New Thread(Sub() MyOhana.Insert_Texture(File_Name, Index))
        Texture_Thread.Start()

        Dim Old_Percentage As Single
        While Texture_Thread.IsAlive
            If MyOhana.Texture_Insertion_Percentage <> Old_Percentage Then
                Update_Progress(ProgressTextures, MyOhana.Texture_Insertion_Percentage, "Inserting data...")
                Old_Percentage = MyOhana.Texture_Insertion_Percentage
            End If
        End While

        Update_Progress(ProgressTextures, 0, Nothing)
        Update_Button_Text(BtnTextureInsert, "Import")

        If LstTextures.SelectedIndex = Index Then Set_Image(ImgTexture, MyOhana.Model_Texture(Index).Image)
        Set_Enabled(BtnTextureOpen, True)
        Set_Enabled(BtnTextureSave, True)
        Set_Enabled(BtnTextureExportAllFF, True)
        Set_Enabled(BtnTextureInsertAll, True)
    End Sub
    Private Sub BtnTextureInsertAll_Click(sender As Object, e As EventArgs) Handles BtnTextureInsertAll.Click
        If Texture_Thread IsNot Nothing Then
            If Texture_Thread.IsAlive Then
                Texture_Thread.Abort()
                BtnTextureInsertAll.Text = "Import all"
                ProgressTextures.Text = Nothing
                ProgressTextures.Percentage = 0
                ProgressTextures.Refresh()

                BtnTextureOpen.Enabled = True
                BtnTextureSave.Enabled = True
                BtnTextureExportAllFF.Enabled = True
                BtnTextureInsert.Enabled = True

                Exit Sub
            End If
        End If

        If MyOhana.Current_Texture <> Nothing Or MyOhana.BCH_Have_Textures Then
            Dim FolderDlg As New FolderBrowserDialog
            If FolderDlg.ShowDialog = DialogResult.OK Then
                If Directory.Exists(FolderDlg.SelectedPath) Then
                    Dim Trd As New Thread(Sub() Insert_Textures_From_Folder(FolderDlg.SelectedPath))
                    Trd.Start()

                    BtnTextureInsertAll.Text = "Cancel"
                    BtnTextureOpen.Enabled = False
                    BtnTextureSave.Enabled = False
                    BtnTextureExportAllFF.Enabled = False
                    BtnTextureInsert.Enabled = False
                End If
            End If
        End If
    End Sub
    Private Sub Insert_Textures_From_Folder(Folder As String)
        Dim Not_Found As Boolean
        Dim Not_Found_Files As String = Nothing
        Dim Index As Integer
        For Each Texture As Ohana.OhanaTexture In MyOhana.Model_Texture
            Dim File_Name As String = Path.Combine(Folder, Texture.Name & ".png")
            If File.Exists(File_Name) Then
                Update_Progress(ProgressTextures, Convert.ToSingle((Index / MyOhana.Model_Texture.Count) * 100), "Inserting " & Texture.Name & "...")
                MyOhana.Insert_Texture(File_Name, Index, False)
                If LstTextures.SelectedIndex = Index Then Set_Image(ImgTexture, MyOhana.Model_Texture(Index).Image)
            Else
                Not_Found = True
                If Index < 15 Then Not_Found_Files &= Texture.Name & ".png" & Environment.NewLine
            End If
            Index += 1
        Next
        If MyOhana.Model_Texture.Count > 15 Then Not_Found_Files &= "[Truncated]"

        Update_Progress(ProgressTextures, 0, Nothing)
        Update_Button_Text(BtnTextureInsertAll, "Import all")

        Set_Enabled(BtnTextureOpen, True)
        Set_Enabled(BtnTextureSave, True)
        Set_Enabled(BtnTextureExportAllFF, True)
        Set_Enabled(BtnTextureInsert, True)

        If Not_Found Then MessageBox.Show("The following files couldn't be found:" & Environment.NewLine & Not_Found_Files, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub
    Private Sub BtnTextureSave_Click(sender As Object, e As EventArgs) Handles BtnTextureSave.Click
        If MyOhana.Current_Texture <> Nothing Then
            File.Delete(MyOhana.Current_Texture)
            File.Copy(MyOhana.Temp_Texture_File, MyOhana.Current_Texture)
        ElseIf MyOhana.BCH_Have_Textures Then
            File.Delete(MyOhana.Current_Model)
            File.Copy(MyOhana.Temp_Model_File, MyOhana.Current_Model)
        End If
    End Sub

    Private Function Mirror_Image(Img As Bitmap) As Bitmap
        Dim ImgData As BitmapData = Img.LockBits(New Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
        Dim Data((ImgData.Height * ImgData.Stride) - 1) As Byte
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

#Region "Text"
    Private Sub BtnTextOpen_Click(sender As Object, e As EventArgs) Handles BtnTextOpen.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open Text file"
        If OpenDlg.ShowDialog = DialogResult.OK Then
            If File.Exists(OpenDlg.FileName) Then
                Open_Text(OpenDlg.FileName)
            End If
        End If
    End Sub
    Private Sub Open_Text(File_Name As String)
        Current_Opened_Text = File_Name
        Try
            MyMinko.Extract_Strings(File_Name)
            Update_Texts()
            Enable_Text_Buttons()
        Catch
            Disable_Text_Buttons()
            MessageBox.Show("Sorry, something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Enable_Text_Buttons()
        BtnTextExport.Enabled = True
        BtnTextImport.Enabled = True
        BtnTextSave.Enabled = True
    End Sub
    Private Sub Disable_Text_Buttons()
        BtnTextExport.Enabled = False
        BtnTextImport.Enabled = False
        BtnTextSave.Enabled = False
    End Sub
    Private Sub BtnTextExport_Click(sender As Object, e As EventArgs) Handles BtnTextExport.Click
        Dim SaveDlg As New SaveFileDialog
        SaveDlg.Title = "Save texts"
        SaveDlg.Filter = "XML|*.xml"
        If SaveDlg.ShowDialog = DialogResult.OK Then
            Dim Out As New StringBuilder
            Out.AppendLine("<!--OhanaXY Pokémon Text Rip :P-->")
            Out.AppendLine("<textfile>")
            For Each Line As String In MyMinko.Strings
                Out.AppendLine("    <text>")
                Dim Temp() As String = Line.Split(Convert.ToChar(&HA))
                For Each Temp_Line As String In Temp
                    Out.AppendLine("        " & Temp_Line)
                Next
                Out.AppendLine("    </text>")
            Next
            Out.AppendLine("</textfile>")
            File.WriteAllText(SaveDlg.FileName, Out.ToString)
        End If
    End Sub
    Private Sub BtnTextImport_Click(sender As Object, e As EventArgs) Handles BtnTextImport.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open texts"
        OpenDlg.Filter = "XML|*.xml"
        If OpenDlg.ShowDialog = DialogResult.OK Then
            Dim TextData As MatchCollection = Regex.Matches(File.ReadAllText(OpenDlg.FileName), "<text>(.+?)</text>", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
            Dim Strings(TextData.Count - 1) As String
            Dim Index As Integer
            For Each Text As Match In TextData
                Dim Value As String = Text.Groups(1).Value.Trim

                Dim Temp() As String = Value.Split(Environment.NewLine)
                Strings(Index) = Nothing
                Dim i As Integer = 1
                For Each Temp_Line As String In Temp
                    Strings(Index) &= Temp_Line.Trim
                    If i < Temp.Count Then Strings(Index) &= Convert.ToChar(&HA)
                    i += 1
                Next
                Index += 1
            Next

            Current_Text_Temp = Path.GetTempFileName
            MyMinko.Insert_Strings(Strings, Current_Text_Temp)

            MyMinko.Extract_Strings(Current_Text_Temp)
            Update_Texts()
        End If
    End Sub
    Private Sub BtnTextSave_Click(sender As Object, e As EventArgs) Handles BtnTextSave.Click
        If Current_Text_Temp <> Nothing And Current_Opened_Text <> Nothing Then
            File.Delete(Current_Opened_Text)
            File.Copy(Current_Text_Temp, Current_Opened_Text)
        End If
    End Sub

    Private Sub Update_Texts()
        LstStrings.Clear()
        For Each Line As String In MyMinko.Strings
            Dim Temp() As String = Line.Split(Convert.ToChar(&HA))
            If Temp.Count > 1 Then Temp(0) &= " [...]"
            LstStrings.AddItem(Temp(0))
        Next
        LstStrings.Refresh()
    End Sub
#End Region

#Region "GARC"
    Private Sub BtnOpenGARC_Click(sender As Object, e As EventArgs) Handles BtnGARCOpen.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open container"
        OpenDlg.Filter = "GARC/Container file|*.*"
        If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Open_GARC(OpenDlg.FileName)
        End If
    End Sub
    Private Sub Open_GARC(File_Name As String)
        If MyNako.Load(File_Name) Then
            Update_GARC_List()
            Enable_GARC_Buttons()
        Else
            Disable_GARC_Buttons()
            LstFiles.Clear()
            MessageBox.Show("This is not a container from Pokémon!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub Enable_GARC_Buttons()
        BtnGARCExtract.Enabled = True
        BtnGARCExtractAll.Enabled = True
        BtnGARCInsert.Enabled = True
        BtnGARCSave.Enabled = True
    End Sub
    Private Sub Disable_GARC_Buttons()
        BtnGARCExtract.Enabled = False
        BtnGARCExtractAll.Enabled = False
        BtnGARCInsert.Enabled = False
        BtnGARCSave.Enabled = False
    End Sub
    Private Sub Update_GARC_List()
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
                Item.Text(0).Text = .Name
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
        Next
        LstFiles.Refresh()
    End Sub
    Private Sub BtnGARCExtract_Click(sender As Object, e As EventArgs) Handles BtnGARCExtract.Click
        If MyNako.Files IsNot Nothing Then
            If LstFiles.SelectedIndex > -1 Then
                Dim SaveDlg As New SaveFileDialog
                SaveDlg.Title = "Extract file"
                SaveDlg.FileName = MyNako.Files(LstFiles.SelectedIndex).Name
                If SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim InFile As New FileStream(MyNako.Current_File, FileMode.Open)
                    MyNako.Extract(InFile, SaveDlg.FileName, LstFiles.SelectedIndex)
                    InFile.Close()
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

                    BtnGARCOpen.Enabled = True
                    BtnGARCInsert.Enabled = True
                    BtnGARCExtract.Enabled = True
                    BtnGARCSave.Enabled = True

                    Exit Sub
                End If
            End If

            Dim OutputDlg As New FolderBrowserDialog
            If OutputDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                GARC_Thread = New Thread(Sub() GARC_ExtractAll(OutputDlg.SelectedPath))
                GARC_Thread.Start()

                BtnGARCExtractAll.Text = "Cancel"
                BtnGARCOpen.Enabled = False
                BtnGARCInsert.Enabled = False
                BtnGARCExtract.Enabled = False
                BtnGARCSave.Enabled = False
            End If
        End If
    End Sub
    Private Sub GARC_ExtractAll(OutFolder As String)
        Dim InFile As New FileStream(MyNako.Current_File, FileMode.Open)
        For Index As Integer = 0 To MyNako.Files.Length - 1
            MyNako.Extract(InFile, Path.Combine(OutFolder, MyNako.Files(Index).Name), Index)
            If MyNako.Files.Length > 1 Then Update_Progress(ProgressGARC, Convert.ToSingle((Index / (MyNako.Files.Length - 1)) * 100), "Extracting " & MyNako.Files(Index).Name & "...")
        Next
        InFile.Close()

        Update_Progress(ProgressGARC, 0, Nothing)
        Update_Button_Text(BtnGARCExtractAll, "Extract all")

        Set_Enabled(BtnGARCOpen, True)
        Set_Enabled(BtnGARCInsert, True)
        Set_Enabled(BtnGARCExtract, True)
        Set_Enabled(BtnGARCSave, True)
    End Sub
    Private Sub BtnGARCInsert_Click(sender As Object, e As EventArgs) Handles BtnGARCInsert.Click
        If LstFiles.SelectedIndex > -1 Then
            Dim OpenDlg As New OpenFileDialog
            If OpenDlg.ShowDialog = DialogResult.OK Then
                If File.Exists(OpenDlg.FileName) Then
                    Dim Item As New Nako.Inserted_File
                    Item.Index = LstFiles.SelectedIndex
                    Item.File_Name = OpenDlg.FileName
                    MyNako.Inserted_Files.Add(Item)

                    Dim Temp As New FileStream(OpenDlg.FileName, FileMode.Open)
                    Dim Magic As String = Encoding.ASCII.GetString(BitConverter.GetBytes(Read32(Temp, 0)))

                    Dim Header As MyListview.ListItem
                    ReDim Header.Text(2)
                    Header.Text(0).Text = "file_" & LstFiles.SelectedIndex & MyNako.Guess_Format(Magic)
                    Header.Text(1).Left = 400
                    Header.Text(1).Text = "???"
                    Header.Text(2).Left = 500
                    Header.Text(2).Text = Format_Size(Convert.ToInt32(Temp.Length))
                    LstFiles.ChangeItem(LstFiles.SelectedIndex, Header)

                    Temp.Close()
                End If
            End If
        End If
    End Sub
    Private Sub BtnGARCSave_Click(sender As Object, e As EventArgs) Handles BtnGARCSave.Click
        If GARC_Thread IsNot Nothing Then
            If GARC_Thread.IsAlive Then
                GARC_Thread.Abort()
                BtnGARCSave.Text = "Save"
                ProgressGARC.Text = Nothing
                ProgressGARC.Percentage = 0
                ProgressGARC.Refresh()

                BtnGARCOpen.Enabled = True
                BtnGARCInsert.Enabled = True
                BtnGARCExtract.Enabled = True
                BtnGARCExtractAll.Enabled = True

                Exit Sub
            End If
        End If

        If MyNako.Files.Count > 0 Then
            Dim Trd As New Thread(AddressOf GARC_Save)
            Trd.Start()

            BtnGARCSave.Text = "Cancel"
            BtnGARCOpen.Enabled = False
            BtnGARCInsert.Enabled = False
            BtnGARCExtract.Enabled = False
            BtnGARCExtractAll.Enabled = False
        End If
    End Sub
    Private Sub GARC_Save()
        Update_Progress(ProgressGARC, 0, "Please wait, rebuilding GARC...")
        MyNako.Compression_Percentage = 0

        GARC_Thread = New Thread(AddressOf MyNako.Insert)
        GARC_Thread.Start()

        Dim Old_Percentage As Single
        While GARC_Thread.IsAlive
            If MyNako.Compression_Percentage <> Old_Percentage Then
                Update_Progress(ProgressGARC, MyNako.Compression_Percentage, "Compressing data...")
                Old_Percentage = MyNako.Compression_Percentage
            End If
        End While
        Update_Progress(ProgressGARC, 0, Nothing)
        Update_Button_Text(BtnGARCSave, "Save")

        Set_Enabled(BtnGARCOpen, True)
        Set_Enabled(BtnGARCInsert, True)
        Set_Enabled(BtnGARCExtract, True)
        Set_Enabled(BtnGARCExtractAll, True)
    End Sub
    Private Sub BtnGARCCompression_Click(sender As Object, e As EventArgs) Handles BtnGARCCompression.Click
        MyNako.Fast_Compression = Not MyNako.Fast_Compression
        If MyNako.Fast_Compression Then
            BtnGARCCompression.Text = "Fast compression"
        Else
            BtnGARCCompression.Text = "Optimal compression"
        End If

        My.Settings.FastCompression = MyNako.Fast_Compression
        My.Settings.Save()
    End Sub
#End Region

#Region "ROM"
    Private Sub BtnROMOpen_Click(sender As Object, e As EventArgs) Handles BtnROMOpen.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open ROM"
        OpenDlg.Filter = "3DS Roms|*.3ds;*.3dz"
        If OpenDlg.ShowDialog = DialogResult.OK Then
            If File.Exists(OpenDlg.FileName) Then
                Current_ROM = OpenDlg.FileName
                Dim ROM As New FileStream(OpenDlg.FileName, FileMode.Open)
                Parse_Header(ROM)
                ROM.Close()
                If Current_XORPad <> Nothing Then BtnROMDecrypt.Enabled = True
            End If
        End If
    End Sub
    Private Sub BtnROMOpenXorPad_Click(sender As Object, e As EventArgs) Handles BtnROMOpenXorPad.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open XORPad"
        OpenDlg.Filter = "XORPad|*.xorpad"
        If OpenDlg.ShowDialog = DialogResult.OK Then
            If File.Exists(OpenDlg.FileName) Then
                Current_XORPad = OpenDlg.FileName
                If Current_ROM <> Nothing Then BtnROMDecrypt.Enabled = True
            End If
        End If
    End Sub
    Private Sub BtnROMDecrypt_Click(sender As Object, e As EventArgs) Handles BtnROMDecrypt.Click
        If Current_ROM IsNot Nothing And Current_XORPad IsNot Nothing Then
            Dim FolderDlg As New FolderBrowserDialog
            If FolderDlg.ShowDialog = DialogResult.OK Then
                Dim Trd As New Thread(Sub() Decrypt_Data(Current_ROM, Current_XORPad, FolderDlg.SelectedPath & "\out\", Convert.ToInt32(NCCH_Container(0).RomFS_Offset), Convert.ToInt32(NCCH_Container(0).RomFS_Size)))
                Trd.Start()
            End If
        End If
    End Sub

    Private Sub Parse_Header(InData As FileStream)
        LstROMLog.Clear()

        Dim Magic As String = Get_Data(InData, &H100, 4, False)
        If Magic <> "NCSD" Then
            Add_Log("[!] NCSD container not found!", Color.Red)
            Exit Sub
        End If
        Add_Log("Parsing NCSD header...", Color.White)

        Dim Offset As Integer = &H120
        For Container As Integer = 0 To 7
            With NCCH_Container(Container)
                .Offset = Read32(InData, Offset) * &H200
                .Length = Read32(InData, Offset + 4) * &H200

                If .Length > 0 Then
                    Dim Base_Offset As Integer = .Offset + &H100

                    If Get_Data(InData, Base_Offset, 4, False) <> "NCCH" Then
                        Add_Log("[!] Invalid NCCH header! The ROM is corrupted!", Color.Red)
                        Exit Sub
                    End If
                    Add_Log("NCCH #" & Container & " (" & Get_Data(InData, Base_Offset + &H50, &H10, False) & ")", Color.Cyan)

                    Add_Log("-- Signature (256 bytes in Hexadecimal/SHA-256):", Color.Gainsboro)
                    For i As Integer = 0 To 7
                        Add_Log(Get_Data(InData, .Offset + i * 32, 32), Color.Gainsboro)
                    Next

                    Add_Log("-- Content Size: " & Format_Size(Read32(InData, Base_Offset + &H4) * &H200), Color.White)
                    InData.Seek(Base_Offset + &H8, SeekOrigin.Begin)
                    ReDim .Partition_ID(7)
                    InData.Read(.Partition_ID, 0, 8)
                    Add_Log("-- Partition ID: " & "0x" & Get_Data(InData, Base_Offset + &H8, 8), Color.White)
                    Add_Log("-- Maker Code: " & "0x" & Get_Data(InData, Base_Offset + &H10, 2), Color.White)
                    Add_Log("-- Version: " & "0x" & Get_Data(InData, Base_Offset + &H12, 2), Color.White)
                    Add_Log("-- Program ID: " & "0x" & Get_Data(InData, Base_Offset + &H18, 8), Color.White)
                    .Product_Code = Get_Data(InData, Base_Offset + &H50, &H10, False)
                    Add_Log("-- ExHeader Hash (32 bytes in Hexadecimal/SHA-256):", Color.Gainsboro)
                    Add_Log(Get_Data(InData, Base_Offset + &H60, &H20), Color.Gainsboro)
                    .ExHeader_Size = Read32(InData, Base_Offset + &H80) * &H200
                    Add_Log("-- ExHeader Size: " & Format_Size(Convert.ToInt32(.ExHeader_Size)), Color.White)

                    Dim Content_Type As String = Nothing
                    InData.Seek(Base_Offset + &H88 + &H5, SeekOrigin.Begin)
                    Dim Temp As Byte = InData.ReadByte
                    If (Temp And &H1) > &H1 Then Content_Type &= "Data"
                    If (Temp And &H2) = &H2 Then If Content_Type <> Nothing Then Content_Type &= "/Executable" Else Content_Type &= "Executable"
                    If (Temp And &H4) = &H4 Then If Content_Type <> Nothing Then Content_Type &= "/System Update" Else Content_Type &= "System Update"
                    If (Temp And &H8) = &H8 Then If Content_Type <> Nothing Then Content_Type &= "/Manual" Else Content_Type &= "Manual"
                    If (Temp And &H10) = &H10 Then If Content_Type <> Nothing Then Content_Type &= "/Trial" Else Content_Type &= "Trial"
                    Add_Log("-- Flags: " & "0x" & Get_Data(InData, Base_Offset + &H88, 8) & If(Content_Type <> Nothing, " (" & Content_Type & ")", Nothing), Color.White)

                    Dim Plain_Region_Offset As Long = Read32(InData, Base_Offset + &H90) * &H200
                    If Plain_Region_Offset > 0 Then Plain_Region_Offset += .Offset
                    Add_Log("-- Plain Region Offset: " & "0x" & Hex(Plain_Region_Offset).PadLeft(8, "0"c), Color.White)
                    Add_Log("-- Plain Region Size: " & Format_Size(Read32(InData, Base_Offset + &H94) * &H200), Color.White)
                    Dim Logo_Region_Offset As Long = Read32(InData, Base_Offset + &H98) * &H200
                    If Logo_Region_Offset > 0 Then Logo_Region_Offset += .Offset
                    Add_Log("-- Logo Region Offset: " & "0x" & Hex(Logo_Region_Offset).PadLeft(8, "0"c), Color.White)
                    Add_Log("-- Logo Region Size: " & Format_Size(Read32(InData, Base_Offset + &H9C) * &H200), Color.White)

                    .ExeFS_Offset = Read32(InData, Base_Offset + &HA0) * &H200
                    If .ExeFS_Offset > 0 Then .ExeFS_Offset += .Offset
                    .ExeFS_Size = Read32(InData, Base_Offset + &HA4) * &H200
                    .RomFS_Offset = Read32(InData, Base_Offset + &HB0) * &H200
                    If .RomFS_Offset > 0 Then .RomFS_Offset += .Offset
                    .RomFS_Size = Read32(InData, Base_Offset + &HB4) * &H200

                    Add_Log("-- Executable File System Offset: " & "0x" & Hex(.ExeFS_Offset).PadLeft(8, "0"c), Color.White)
                    Add_Log("-- Executable File System Size: " & Format_Size(Convert.ToInt32(.ExeFS_Size)), Color.White)
                    Add_Log("-- Executable File System Hash Region Size: " & Format_Size(Read32(InData, Base_Offset + &HA8) * &H200), Color.White)
                    Add_Log("-- ROM File System Offset: " & "0x" & Hex(.RomFS_Offset).PadLeft(8, "0"c), Color.White)
                    Add_Log("-- ROM File System Size: " & Format_Size(Convert.ToInt32(.RomFS_Size)), Color.White)
                    Add_Log("-- ROM File System Hash Region Size: " & Format_Size(Read32(InData, Base_Offset + &HB8) * &H200), Color.White)

                    Add_Log("-- Executable File System Hash (32 bytes in Hexadecimal/SHA-256):", Color.Gainsboro)
                    Add_Log(Get_Data(InData, Base_Offset + &HC0, &H20), Color.Gainsboro)
                    Add_Log("-- ROM File System Hash (32 bytes in Hexadecimal/SHA-256):", Color.Gainsboro)
                    Add_Log(Get_Data(InData, Base_Offset + &HE0, &H20), Color.Gainsboro)
                End If
            End With

            Offset += 8
        Next

        Add_Log("Done! The ROM is ready to be decrypted!", Color.Green, True)
    End Sub
    Private Sub Add_Log(Text As String, Color As Color, Optional Refresh As Boolean = False)
        Dim Item As MyListview.ListItem
        ReDim Item.Text(0)
        Item.Text(0).Text = Text
        Item.Text(0).ForeColor = Color
        Add_List_Item(LstROMLog, Item, Refresh)
    End Sub
    Public Function Get_Data(InData As FileStream, Start_Offset As Integer, Count As Integer, Optional HexFmt As Boolean = True) As String
        Dim Data(Count - 1) As Byte
        InData.Position = Start_Offset
        InData.Read(Data, 0, Count)
        Dim Out As String = Nothing
        For i As Integer = 0 To Count - 1
            If HexFmt Then
                Out &= Hex(Data(i)).PadLeft(2, "0"c)
            Else
                If Data(i) > 0 Then Out &= Chr(Data(i))
            End If
        Next
        Return Out
    End Function

    Private Sub Decrypt_Data(In_File As String, In_XOR As String, Out_Path As String, InOffset As Integer, InSize As Integer)
        Add_Log("Decrypting Rom File System (it may take some time)...", Color.White, True)

        Dim InFile As New FileStream(In_File, FileMode.Open)
        Dim InXOR As New FileStream(In_XOR, FileMode.Open)
        Dim Output_File As String = Path.Combine(Path.GetDirectoryName(In_File), Path.GetFileNameWithoutExtension(In_File) & ".dec")
        Dim OutFile As New FileStream(Output_File, FileMode.Create)
        InFile.Seek(InOffset, SeekOrigin.Begin)
        For Offset As Integer = 0 To InSize - 1 Step 16384
            Dim BuffLen As Integer = Convert.ToInt32(If(InSize - Offset >= 16384, 16384, InSize - Offset))
            Dim Buffer(BuffLen - 1) As Byte
            Dim XorBuff(BuffLen - 1) As Byte
            InFile.Read(Buffer, 0, BuffLen)
            InXOR.Read(XorBuff, 0, BuffLen)
            For i As Integer = 0 To Buffer.Length - 1
                Buffer(i) = Buffer(i) Xor XorBuff(i)
            Next
            OutFile.Write(Buffer, 0, BuffLen)
        Next
        OutFile.Close()
        InFile.Close()
        InXOR.Close()

        Extract_RomFS(Output_File, Out_Path)
    End Sub

    Private Sub Extract_RomFS(InFile As String, OutFolder As String)
        Dim RomFS As New FileStream(InFile, FileMode.Open)
        Dim Reader As New BinaryReader(RomFS)
        Dim Info_Offset As Integer = &H1000
        RomFS.Seek(&H1000, SeekOrigin.Begin)
        Dim Header_Size As Integer = Reader.ReadInt32
        Dim Sections(3) As Info_Header
        For Section As Integer = 0 To 3
            Sections(Section).Offset = Reader.ReadInt32
            Sections(Section).Length = Reader.ReadInt32
        Next
        Dim Data_Offset As Integer = &H1000 + Reader.ReadInt32

        Dim Directory_Info_Offset As Integer = &H1000 + Sections(1).Offset
        Dim Directory_Info_Length As Integer = Sections(1).Length
        Dim File_Info_Offset As Integer = &H1000 + Sections(3).Offset
        Dim File_Info_Length As Integer = Sections(3).Length

        Dim Directories As New List(Of Rom_Directory)
        Parse_Directory(RomFS, Directory_Info_Offset, Directory_Info_Offset, File_Info_Offset, Data_Offset, Nothing, OutFolder)
        RomFS.Close()
    End Sub
    Private Sub Parse_Directory(RomFS As FileStream, Offset As Integer, Directory_Info_Offset As Integer, File_Info_Offset As Integer, Data_Offset As Integer, Path As String, Out_Path As String)
        Dim Dir As Rom_Directory

        With Dir
            .Parent_Offset = Read32(RomFS, Offset)
            .Sibling_Offset = Read32(RomFS, Offset + 4)
            .Children_Offset = Read32(RomFS, Offset + 8)
            .File_Offset = Read32(RomFS, Offset + 12)
            .Unknow = Read32(RomFS, Offset + 16)
            .Name = Nothing
            Dim Temp As Integer = Offset + 24
            Dim Name_Length As Integer = Read32(RomFS, Offset + 20)
            For Name_Offset As Integer = Temp To Temp + Name_Length - 1 Step 2
                .Name &= ChrW(Read16(RomFS, Name_Offset))
            Next
            Dim Current_Path As String = Path & If(Path <> Nothing, "\", Nothing) & .Name

            If .File_Offset <> &HFFFFFFFF Then
                Parse_File(RomFS, File_Info_Offset + .File_Offset, File_Info_Offset, Data_Offset, Current_Path, Out_Path)
            End If
            If .Children_Offset <> &HFFFFFFFF Then
                Parse_Directory(RomFS, Directory_Info_Offset + .Children_Offset, Directory_Info_Offset, File_Info_Offset, Data_Offset, Current_Path, Out_Path)
            End If
            If .Sibling_Offset <> &HFFFFFFFF Then
                Parse_Directory(RomFS, Directory_Info_Offset + .Sibling_Offset, Directory_Info_Offset, File_Info_Offset, Data_Offset, Path, Out_Path)
            End If
        End With
    End Sub
    Private Sub Parse_File(RomFS As FileStream, Offset As Integer, File_Info_Offset As Integer, Data_Offset As Integer, Path As String, Out_Path As String)
        Dim File As Rom_File

        With File
            .Parent_Offset = Read32(RomFS, Offset)
            .Sibling_Offset = Read32(RomFS, Offset + 4)
            .Data_Offset = Read64(RomFS, Offset + 8)
            .Data_Length = Read64(RomFS, Offset + 16)
            .Unknow = Read32(RomFS, Offset + 24)
            .Name = Nothing
            Dim Temp As Integer = Offset + 32
            Dim Name_Length As Integer = Read32(RomFS, Offset + 28)
            For Name_Offset As Integer = Temp To Temp + Name_Length - 1 Step 2
                .Name &= ChrW(Read16(RomFS, Name_Offset))
            Next
            Dim File_Name As String = Path & "\" & .Name
            Add_Log("Extracting " & Path & "\" & .Name & "...", Color.White, True)

            If Not Directory.Exists(Out_Path & "\" & Path) Then Directory.CreateDirectory(Out_Path & "\" & Path)
            Dim Out_File As New FileStream(Out_Path & "\" & File_Name, FileMode.Create)
            Dim File_Offset As Integer = Convert.ToInt32(Data_Offset + .Data_Offset)
            RomFS.Seek(File_Offset, SeekOrigin.Begin)
            For Write_Offset As Integer = File_Offset To Convert.ToInt32(File_Offset + .Data_Length - 1) Step 16
                Dim BuffLen As Integer = Convert.ToInt32(If(.Data_Length - (Write_Offset - File_Offset) >= 16, 16, .Data_Length - (Write_Offset - File_Offset)))
                Dim Buffer(BuffLen - 1) As Byte
                RomFS.Read(Buffer, 0, BuffLen)
                Out_File.Write(Buffer, 0, BuffLen)
            Next
            Out_File.Close()

            If .Sibling_Offset <> &HFFFFFFFF Then
                Parse_File(RomFS, File_Info_Offset + .Sibling_Offset, File_Info_Offset, Data_Offset, Path, Out_Path)
            End If
        End With
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
            Update_Progress(ProgressSearch, Convert.ToSingle((Index / Input_Files.Count) * 100), "Searching on file " & CurrFile.Name & "...")
            Index += 1

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
                        Add_List_Item(LstMatches, Item, True)
                        Found = True
                        Exit For
                    End If
                Next
                If Found Then Exit For
            Next
        Next

        Update_Progress(ProgressSearch, 0, Nothing)
        Update_Button_Text(BtnSearch, "Search")
    End Sub
#End Region

#End Region

    Private Sub BtnModelMapEditor_Click(sender As Object, e As EventArgs) Handles BtnModelMapEditor.Click
        If MyOhana.Magic.Substring(0, 2) = "GR" Then
            FrmMapProp.Show()
            FrmMapProp.makeMapIMG(MapProps())
        End If
    End Sub

    Private Function MapProps() As Byte()
        Dim br As New BinaryReader(System.IO.File.OpenRead(MyOhana.Current_Model))
        Dim buff As Byte() = br.ReadBytes(&H10)
        br.BaseStream.Position = &H80
        buff = br.ReadBytes(Read32(buff, 8) - Read32(buff, 4))
        br.Close()
        Return buff
    End Function

    Public Sub saveMapProps(ByVal w As Short, ByVal h As Short, ByVal mapVals As UInteger())
        Using dataStream As FileStream = New FileStream(MyOhana.Current_Model, FileMode.Open)
            Using bw As New BinaryWriter(dataStream)
                Try
                    bw.BaseStream.Position = &H80
                    bw.Write(w)
                    bw.Write(h)
                    For i As Integer = 0 To mapVals.Length - 1
                        bw.Write(mapVals(i))
                    Next
                    bw.Close()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                End Try
            End Using
            MessageBox.Show("Saved map!")
        End Using
    End Sub

End Class
