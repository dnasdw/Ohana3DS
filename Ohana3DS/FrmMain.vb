﻿Imports System.IO
Imports System.Threading
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions

Public Class FrmMain
    'Classes de Compressão/Extração
    Dim MyMinko As New Minko
    Dim MyNako As New Nako

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
    Dim Texture_Export_Thread As Thread
    Dim GARC_Thread, GARC_Insertion_Thread As Thread
    Dim Search_Thread As Thread

    Dim Old_Index As Integer = -1
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
        Me.AllowDrop = True
        MyFileDrop = New FileDrop(AddressOf File_Dropped)
        MyOhana.Initialize(Screen)

        MyOhana.Scale = My.Settings.ModelScale
        BtnModelScale.Text = "Scale 1:" & My.Settings.ModelScale
        MyOhana.Model_Mirror_X = My.Settings.ModelMirror
        If My.Settings.ModelMirror Then BtnModelMirror.Text = "Mirror-X" Else BtnModelMirror.Text = "Normal"
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

        Show()
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

    Private Sub File_Dropped(File_Name As String)
        Dim Temp As New FileStream(File_Name, FileMode.Open)

        Dim Magic_2_Bytes As String = ReadMagic(Temp, 0, 2)
        Dim Magic_3_Bytes As String = ReadMagic(Temp, 0, 3)
        Dim Magic_4_Bytes As String = ReadMagic(Temp, 0, 4)
        Dim CLIM_Magic As String = ReadMagic(Temp, Convert.ToInt32(Temp.Length - 40), 4)
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

    Private Sub BtnModelMapEditor_Click(sender As Object, e As EventArgs) Handles BtnModelMapEditor.Click
        If MyOhana.Magic.Substring(0, 2) = "GR" Then
            FrmMapProp.Show()
            MyOhana.makeMapIMG(MapProps())
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

#Region "GUI"

#Region "General"
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
        OpenDlg.Title = "Open Pokémon BCH model"
        OpenDlg.Filter = "BCH Model|*.*"
        If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            If File.Exists(OpenDlg.FileName) Then Open_Model(OpenDlg.FileName)
        End If
    End Sub
    Private Function Open_Model(File_Name As String, Optional Show_Warning As Boolean = True) As Boolean
        Dim Response As Boolean

        LblModelName.Text = Nothing
        LblInfoVertices.Text = "0"
        LblInfoTriangles.Text = "0"
        LblInfoBones.Text = "0"
        LblInfoTextures.Text = "0"

        Try
            Current_Model = File_Name
            Response = MyOhana.Load_Model(File_Name)
            If Response Then
                LblModelName.Text = Path.GetFileName(File_Name)

                If MyOhana.BCH_Have_Textures Then
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
            Else
                If Show_Warning Then MessageBox.Show("This file is not a model file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch
            Response = False
            MyOhana.Model_Object = Nothing
            If Show_Warning Then MessageBox.Show("Sorry, something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Screen.Refresh()
        If Response Then MyOhana.Render()

        Return Response
    End Function
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
        Exporter.Model_Mirror_X = MyOhana.Model_Mirror_X
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
            BtnModelScale.Text = "Scale 1:32"
        ElseIf MyOhana.Scale = 32 Then
            MyOhana.Scale = 64
            BtnModelScale.Text = "Scale 1:64"
        ElseIf MyOhana.Scale = 64 Then
            MyOhana.Scale = 1
            BtnModelScale.Text = "Scale 1:1"
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
    Private Sub BtnModelMirror_Click(sender As Object, e As EventArgs) Handles BtnModelMirror.Click
        MyOhana.Model_Mirror_X = Not MyOhana.Model_Mirror_X
        If MyOhana.Model_Mirror_X Then
            BtnModelMirror.Text = "Mirror-X"
        Else
            BtnModelMirror.Text = "Normal"
        End If

        My.Settings.ModelMirror = MyOhana.Model_Mirror_X
        My.Settings.Save()
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
        If e.Button = MouseButtons.Left Then
            Rot_InitX = MousePosition.X
            Rot_InitY = MousePosition.Y
        ElseIf e.Button = MouseButtons.Right Then
            Mov_InitX = MousePosition.X
            Mov_InitY = MousePosition.Y
        End If
    End Sub
    Private Sub Screen_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseUp
        If e.Button = MouseButtons.Left Then
            Rot_FinalX += (Rot_InitX - MousePosition.X)
            Rot_FinalY += (Rot_InitY - MousePosition.Y)
        ElseIf e.Button = MouseButtons.Right Then
            Mov_FinalX += (Mov_InitX - MousePosition.X)
            Mov_FinalY += (Mov_InitY - MousePosition.Y)
        End If
    End Sub
    Private Sub Screen_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseMove
        If Not Screen.Focused Then Screen.Select()
        If e.Button = MouseButtons.Left Then
            MyOhana.Rotation.X = (Rot_InitX - MousePosition.X) + Rot_FinalX
            MyOhana.Rotation.Y = (Rot_InitY - MousePosition.Y) + Rot_FinalY
        ElseIf e.Button = MouseButtons.Right Then
            MyOhana.Translation.X = (Mov_InitX - MousePosition.X) + Mov_FinalX
            MyOhana.Translation.Y = (Mov_InitY - MousePosition.Y) + Mov_FinalY
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
    Private Sub Open_Texture(File_Name As String)
        Try
            LstTextures.Clear()
            ImgTexture.Image = Nothing

            MyOhana.Load_Textures(File_Name)
            For Each Texture As Ohana.OhanaTexture In MyOhana.Model_Texture
                LstTextures.AddItem(Texture.Name)
            Next
            LstTextures.Refresh()
        Catch
            MessageBox.Show("Sorry, something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub LstTextures_SelectedIndexChanged(Index As Integer) Handles LstTextures.SelectedIndexChanged
        If Index > -1 Then
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
        End If
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
                Exporter.Load_Textures(File.FullName, False)
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
        If (MyOhana.Current_Texture <> Nothing Or MyOhana.BCH_Have_Textures) And LstTextures.SelectedIndex > -1 Then
            Dim OpenDlg As New OpenFileDialog
            OpenDlg.Title = "Select the Texture to insert"
            OpenDlg.Filter = "PNG|*.png"
            If OpenDlg.ShowDialog = DialogResult.OK Then
                If File.Exists(OpenDlg.FileName) Then
                    MyOhana.Insert_Texture(OpenDlg.FileName, LstTextures.SelectedIndex)
                End If
            End If
        End If
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

#Region "Text"
    Private Sub BtnTextOpen_Click(sender As Object, e As EventArgs) Handles BtnTextOpen.Click
        Dim OpenDlg As New OpenFileDialog
        OpenDlg.Title = "Open Text file"
        If OpenDlg.ShowDialog = DialogResult.OK Then
            If File.Exists(OpenDlg.FileName) Then
                Current_Opened_Text = OpenDlg.FileName
                MyMinko.Extract_Strings(OpenDlg.FileName)
                Update_Texts()
            End If
        End If
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
        OpenDlg.Title = "Open GARC container"
        OpenDlg.Filter = "GARC file|*.*"
        If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Open_GARC(OpenDlg.FileName)
        End If
    End Sub
    Private Sub Open_GARC(File_Name As String)
        MyNako.Load(File_Name)
        Update_GARC_List()
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
                SaveDlg.FileName = MyNako.Files(LstFiles.SelectedIndex).Name
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
            MyNako.Extract(Path.Combine(OutFolder, MyNako.Files(Index).Name), Index)
            If MyNako.Files.Length > 1 Then Update_Progress(ProgressGARC, Convert.ToSingle((Index / (MyNako.Files.Length - 1)) * 100), "Extracting " & MyNako.Files(Index).Name & "...")
        Next

        Update_Progress(ProgressGARC, 0, Nothing)
        Update_Button_Text(BtnGARCExtractAll, "Extract all")
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
        If MyNako.Files.Count > 0 Then
            If GARC_Insertion_Thread IsNot Nothing Then
                If GARC_Insertion_Thread.IsAlive Then
                    GARC_Insertion_Thread.Abort()
                    BtnGARCSave.Text = "Save"
                    ProgressGARCInsertion.Text = Nothing
                    ProgressGARCInsertion.Percentage = 0
                    ProgressGARCInsertion.Refresh()

                    Exit Sub
                End If
            End If

            Dim Trd As New Thread(AddressOf GARC_Save)
            Trd.Start()

            BtnGARCSave.Text = "Cancel"
        End If
    End Sub
    Private Sub GARC_Save()
        Update_Progress(ProgressGARCInsertion, 0, "Please wait, rebuilding GARC...")
        MyNako.Compression_Percentage = 0

        GARC_Insertion_Thread = New Thread(AddressOf MyNako.Insert)
        GARC_Insertion_Thread.Start()

        Dim Old_Percentage As Single
        While GARC_Insertion_Thread.IsAlive
            If MyNako.Compression_Percentage <> Old_Percentage Then
                Update_Progress(ProgressGARCInsertion, MyNako.Compression_Percentage, "Compressing data...")
                Old_Percentage = MyNako.Compression_Percentage
            End If
        End While
        Update_Progress(ProgressGARCInsertion, 0, Nothing)
        Update_Button_Text(BtnGARCSave, "Save")
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
