<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: O procedimento a seguir é exigido pelo Windows Form Designer
    'Ele pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.BtnClose = New System.Windows.Forms.Label()
        Me.BtnMinimize = New System.Windows.Forms.Label()
        Me.Splash = New System.Windows.Forms.PictureBox()
        Me.ModelNameTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainTabs = New Ohana3DS.MyTabcontrol()
        Me.ModelPage = New System.Windows.Forms.TabPage()
        Me.GrpOptions = New Ohana3DS.MyGroupbox()
        Me.BtnModelMapEditor = New System.Windows.Forms.Button()
        Me.BtnModelSave = New System.Windows.Forms.Button()
        Me.BtnModelVertexEditor = New System.Windows.Forms.Button()
        Me.BtnModelScale = New System.Windows.Forms.Button()
        Me.ProgressModels = New Ohana3DS.MyProgressbar()
        Me.BtnModelExportAllFF = New System.Windows.Forms.Button()
        Me.BtnModelExport = New System.Windows.Forms.Button()
        Me.BtnModelOpen = New System.Windows.Forms.Button()
        Me.GrpInfo = New Ohana3DS.MyGroupbox()
        Me.LblModelName = New System.Windows.Forms.Label()
        Me.BtnModelTexturesMore = New System.Windows.Forms.Button()
        Me.LblInfoTextures = New System.Windows.Forms.Label()
        Me.LblInfoBones = New System.Windows.Forms.Label()
        Me.LblInfoTriangles = New System.Windows.Forms.Label()
        Me.LblInfoVertices = New System.Windows.Forms.Label()
        Me.LblInfoTexturesDummy = New System.Windows.Forms.Label()
        Me.LblInfoBonesDummy = New System.Windows.Forms.Label()
        Me.LblInfoTrianglesDummy = New System.Windows.Forms.Label()
        Me.LblInfoVerticesDummy = New System.Windows.Forms.Label()
        Me.Screen = New System.Windows.Forms.PictureBox()
        Me.TexturePage = New System.Windows.Forms.TabPage()
        Me.GrpTexOptions = New Ohana3DS.MyGroupbox()
        Me.BtnTextureInsertAll = New System.Windows.Forms.Button()
        Me.BtnTextureSave = New System.Windows.Forms.Button()
        Me.BtnTextureInsert = New System.Windows.Forms.Button()
        Me.BtnTextureMode = New System.Windows.Forms.Button()
        Me.BtnTextureExportAllFF = New System.Windows.Forms.Button()
        Me.ProgressTextures = New Ohana3DS.MyProgressbar()
        Me.BtnTextureExportAll = New System.Windows.Forms.Button()
        Me.BtnTextureExport = New System.Windows.Forms.Button()
        Me.BtnTextureOpen = New System.Windows.Forms.Button()
        Me.GrpTexInfo = New Ohana3DS.MyGroupbox()
        Me.LblInfoTextureCD = New System.Windows.Forms.Label()
        Me.LblInfoTextureFormat = New System.Windows.Forms.Label()
        Me.LblInfoTextureResolution = New System.Windows.Forms.Label()
        Me.LblInfoTextureCDDummy = New System.Windows.Forms.Label()
        Me.LblInfoTextureFormatDummy = New System.Windows.Forms.Label()
        Me.LblInfoTextureResolutionDummy = New System.Windows.Forms.Label()
        Me.LblInfoTextureIndex = New System.Windows.Forms.Label()
        Me.LblInfoTextureIndexDummy = New System.Windows.Forms.Label()
        Me.GrpTexturePreview = New Ohana3DS.MyGroupbox()
        Me.ImgTexture = New Ohana3DS.MyPicturebox()
        Me.GrpTextures = New Ohana3DS.MyGroupbox()
        Me.LstTextures = New Ohana3DS.MyListview()
        Me.TextPage = New System.Windows.Forms.TabPage()
        Me.GrpTextOptions = New Ohana3DS.MyGroupbox()
        Me.BtnTextSave = New System.Windows.Forms.Button()
        Me.BtnTextImport = New System.Windows.Forms.Button()
        Me.BtnTextExport = New System.Windows.Forms.Button()
        Me.BtnTextOpen = New System.Windows.Forms.Button()
        Me.GrpTextStrings = New Ohana3DS.MyGroupbox()
        Me.LstStrings = New Ohana3DS.MyListview()
        Me.GARCPage = New System.Windows.Forms.TabPage()
        Me.GrpGARCOptions = New Ohana3DS.MyGroupbox()
        Me.BtnGARCCompression = New System.Windows.Forms.Button()
        Me.BtnGARCSave = New System.Windows.Forms.Button()
        Me.BtnGARCInsert = New System.Windows.Forms.Button()
        Me.ProgressGARC = New Ohana3DS.MyProgressbar()
        Me.BtnGARCExtractAll = New System.Windows.Forms.Button()
        Me.BtnGARCExtract = New System.Windows.Forms.Button()
        Me.BtnGARCOpen = New System.Windows.Forms.Button()
        Me.GrpFiles = New Ohana3DS.MyGroupbox()
        Me.LstFiles = New Ohana3DS.MyListview()
        Me.ROMPage = New System.Windows.Forms.TabPage()
        Me.GrpROMLog = New Ohana3DS.MyGroupbox()
        Me.LstROMLog = New Ohana3DS.MyListview()
        Me.GrpROMOptions = New Ohana3DS.MyGroupbox()
        Me.BtnROMDecrypt = New System.Windows.Forms.Button()
        Me.BtnROMOpenXorPad = New System.Windows.Forms.Button()
        Me.BtnROMOpen = New System.Windows.Forms.Button()
        Me.SearchPage = New System.Windows.Forms.TabPage()
        Me.GrpMatches = New Ohana3DS.MyGroupbox()
        Me.LstMatches = New Ohana3DS.MyListview()
        Me.GrpSearchOptions = New Ohana3DS.MyGroupbox()
        Me.TxtSearch = New System.Windows.Forms.TextBox()
        Me.ProgressSearch = New Ohana3DS.MyProgressbar()
        Me.BtnSearch = New System.Windows.Forms.Button()
        Me.Title = New Ohana3DS.MyWindowTitle()
        Me.colorBG = New System.Windows.Forms.ColorDialog()
        CType(Me.Splash, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainTabs.SuspendLayout()
        Me.ModelPage.SuspendLayout()
        Me.GrpOptions.SuspendLayout()
        Me.GrpInfo.SuspendLayout()
        CType(Me.Screen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TexturePage.SuspendLayout()
        Me.GrpTexOptions.SuspendLayout()
        Me.GrpTexInfo.SuspendLayout()
        Me.GrpTexturePreview.SuspendLayout()
        CType(Me.ImgTexture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpTextures.SuspendLayout()
        Me.TextPage.SuspendLayout()
        Me.GrpTextOptions.SuspendLayout()
        Me.GrpTextStrings.SuspendLayout()
        Me.GARCPage.SuspendLayout()
        Me.GrpGARCOptions.SuspendLayout()
        Me.GrpFiles.SuspendLayout()
        Me.ROMPage.SuspendLayout()
        Me.GrpROMLog.SuspendLayout()
        Me.GrpROMOptions.SuspendLayout()
        Me.SearchPage.SuspendLayout()
        Me.GrpMatches.SuspendLayout()
        Me.GrpSearchOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.Color.Transparent
        Me.BtnClose.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.Color.White
        Me.BtnClose.Location = New System.Drawing.Point(756, 4)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(32, 24)
        Me.BtnClose.TabIndex = 0
        Me.BtnClose.Text = "X"
        Me.BtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnMinimize
        '
        Me.BtnMinimize.BackColor = System.Drawing.Color.Transparent
        Me.BtnMinimize.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMinimize.ForeColor = System.Drawing.Color.White
        Me.BtnMinimize.Location = New System.Drawing.Point(724, 4)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(32, 24)
        Me.BtnMinimize.TabIndex = 15
        Me.BtnMinimize.Text = "_"
        Me.BtnMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Splash
        '
        Me.Splash.Image = CType(resources.GetObject("Splash.Image"), System.Drawing.Image)
        Me.Splash.Location = New System.Drawing.Point(0, 32)
        Me.Splash.Name = "Splash"
        Me.Splash.Size = New System.Drawing.Size(800, 568)
        Me.Splash.TabIndex = 20
        Me.Splash.TabStop = False
        '
        'MainTabs
        '
        Me.MainTabs.Alignment = System.Windows.Forms.TabAlignment.Left
        Me.MainTabs.BackgroundImage = CType(resources.GetObject("MainTabs.BackgroundImage"), System.Drawing.Image)
        Me.MainTabs.Controls.Add(Me.ModelPage)
        Me.MainTabs.Controls.Add(Me.TexturePage)
        Me.MainTabs.Controls.Add(Me.TextPage)
        Me.MainTabs.Controls.Add(Me.GARCPage)
        Me.MainTabs.Controls.Add(Me.ROMPage)
        Me.MainTabs.Controls.Add(Me.SearchPage)
        Me.MainTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.MainTabs.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainTabs.ItemSize = New System.Drawing.Size(56, 24)
        Me.MainTabs.Location = New System.Drawing.Point(0, 32)
        Me.MainTabs.Multiline = True
        Me.MainTabs.Name = "MainTabs"
        Me.MainTabs.SelectedIndex = 0
        Me.MainTabs.Size = New System.Drawing.Size(800, 568)
        Me.MainTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.MainTabs.TabIndex = 19
        Me.MainTabs.Visible = False
        '
        'ModelPage
        '
        Me.ModelPage.BackColor = System.Drawing.Color.Transparent
        Me.ModelPage.Controls.Add(Me.GrpOptions)
        Me.ModelPage.Controls.Add(Me.GrpInfo)
        Me.ModelPage.Controls.Add(Me.Screen)
        Me.ModelPage.ForeColor = System.Drawing.Color.White
        Me.ModelPage.Location = New System.Drawing.Point(28, 4)
        Me.ModelPage.Name = "ModelPage"
        Me.ModelPage.Padding = New System.Windows.Forms.Padding(3)
        Me.ModelPage.Size = New System.Drawing.Size(768, 560)
        Me.ModelPage.TabIndex = 0
        Me.ModelPage.Text = "Model"
        '
        'GrpOptions
        '
        Me.GrpOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpOptions.Controls.Add(Me.BtnModelMapEditor)
        Me.GrpOptions.Controls.Add(Me.BtnModelSave)
        Me.GrpOptions.Controls.Add(Me.BtnModelVertexEditor)
        Me.GrpOptions.Controls.Add(Me.BtnModelScale)
        Me.GrpOptions.Controls.Add(Me.ProgressModels)
        Me.GrpOptions.Controls.Add(Me.BtnModelExportAllFF)
        Me.GrpOptions.Controls.Add(Me.BtnModelExport)
        Me.GrpOptions.Controls.Add(Me.BtnModelOpen)
        Me.GrpOptions.ForeColor = System.Drawing.Color.White
        Me.GrpOptions.Location = New System.Drawing.Point(206, 480)
        Me.GrpOptions.Name = "GrpOptions"
        Me.GrpOptions.Size = New System.Drawing.Size(562, 80)
        Me.GrpOptions.TabIndex = 23
        Me.GrpOptions.TabStop = False
        Me.GrpOptions.Text = "Options"
        '
        'BtnModelMapEditor
        '
        Me.BtnModelMapEditor.Enabled = False
        Me.BtnModelMapEditor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelMapEditor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelMapEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelMapEditor.Location = New System.Drawing.Point(162, 48)
        Me.BtnModelMapEditor.Name = "BtnModelMapEditor"
        Me.BtnModelMapEditor.Size = New System.Drawing.Size(150, 24)
        Me.BtnModelMapEditor.TabIndex = 8
        Me.BtnModelMapEditor.Text = "Edit map data..."
        Me.BtnModelMapEditor.UseVisualStyleBackColor = True
        '
        'BtnModelSave
        '
        Me.BtnModelSave.Enabled = False
        Me.BtnModelSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelSave.Location = New System.Drawing.Point(0, 48)
        Me.BtnModelSave.Name = "BtnModelSave"
        Me.BtnModelSave.Size = New System.Drawing.Size(72, 24)
        Me.BtnModelSave.TabIndex = 6
        Me.BtnModelSave.Text = "Save"
        Me.BtnModelSave.UseVisualStyleBackColor = True
        '
        'BtnModelVertexEditor
        '
        Me.BtnModelVertexEditor.Enabled = False
        Me.BtnModelVertexEditor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelVertexEditor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelVertexEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelVertexEditor.Location = New System.Drawing.Point(84, 48)
        Me.BtnModelVertexEditor.Name = "BtnModelVertexEditor"
        Me.BtnModelVertexEditor.Size = New System.Drawing.Size(72, 24)
        Me.BtnModelVertexEditor.TabIndex = 7
        Me.BtnModelVertexEditor.Text = "Edit..."
        Me.BtnModelVertexEditor.UseVisualStyleBackColor = True
        '
        'BtnModelScale
        '
        Me.BtnModelScale.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelScale.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelScale.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelScale.Location = New System.Drawing.Point(412, 18)
        Me.BtnModelScale.Name = "BtnModelScale"
        Me.BtnModelScale.Size = New System.Drawing.Size(150, 24)
        Me.BtnModelScale.TabIndex = 5
        Me.BtnModelScale.Text = "Model scale: 1:32"
        Me.BtnModelScale.UseVisualStyleBackColor = True
        '
        'ProgressModels
        '
        Me.ProgressModels.Location = New System.Drawing.Point(412, 48)
        Me.ProgressModels.Name = "ProgressModels"
        Me.ProgressModels.Percentage = 0.0!
        Me.ProgressModels.Size = New System.Drawing.Size(150, 24)
        Me.ProgressModels.TabIndex = 0
        '
        'BtnModelExportAllFF
        '
        Me.BtnModelExportAllFF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelExportAllFF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelExportAllFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelExportAllFF.Location = New System.Drawing.Point(162, 18)
        Me.BtnModelExportAllFF.Name = "BtnModelExportAllFF"
        Me.BtnModelExportAllFF.Size = New System.Drawing.Size(150, 24)
        Me.BtnModelExportAllFF.TabIndex = 3
        Me.BtnModelExportAllFF.Text = "Export all from folder"
        Me.BtnModelExportAllFF.UseVisualStyleBackColor = True
        '
        'BtnModelExport
        '
        Me.BtnModelExport.Enabled = False
        Me.BtnModelExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelExport.Location = New System.Drawing.Point(84, 18)
        Me.BtnModelExport.Name = "BtnModelExport"
        Me.BtnModelExport.Size = New System.Drawing.Size(72, 24)
        Me.BtnModelExport.TabIndex = 2
        Me.BtnModelExport.Text = "Export"
        Me.BtnModelExport.UseVisualStyleBackColor = True
        '
        'BtnModelOpen
        '
        Me.BtnModelOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelOpen.Location = New System.Drawing.Point(0, 18)
        Me.BtnModelOpen.Name = "BtnModelOpen"
        Me.BtnModelOpen.Size = New System.Drawing.Size(72, 24)
        Me.BtnModelOpen.TabIndex = 1
        Me.BtnModelOpen.Text = "Open"
        Me.BtnModelOpen.UseVisualStyleBackColor = True
        '
        'GrpInfo
        '
        Me.GrpInfo.BackColor = System.Drawing.Color.Transparent
        Me.GrpInfo.Controls.Add(Me.LblModelName)
        Me.GrpInfo.Controls.Add(Me.BtnModelTexturesMore)
        Me.GrpInfo.Controls.Add(Me.LblInfoTextures)
        Me.GrpInfo.Controls.Add(Me.LblInfoBones)
        Me.GrpInfo.Controls.Add(Me.LblInfoTriangles)
        Me.GrpInfo.Controls.Add(Me.LblInfoVertices)
        Me.GrpInfo.Controls.Add(Me.LblInfoTexturesDummy)
        Me.GrpInfo.Controls.Add(Me.LblInfoBonesDummy)
        Me.GrpInfo.Controls.Add(Me.LblInfoTrianglesDummy)
        Me.GrpInfo.Controls.Add(Me.LblInfoVerticesDummy)
        Me.GrpInfo.ForeColor = System.Drawing.Color.White
        Me.GrpInfo.Location = New System.Drawing.Point(0, 480)
        Me.GrpInfo.Name = "GrpInfo"
        Me.GrpInfo.Size = New System.Drawing.Size(200, 80)
        Me.GrpInfo.TabIndex = 22
        Me.GrpInfo.TabStop = False
        Me.GrpInfo.Text = "Info"
        '
        'LblModelName
        '
        Me.LblModelName.Location = New System.Drawing.Point(112, 18)
        Me.LblModelName.Name = "LblModelName"
        Me.LblModelName.Size = New System.Drawing.Size(88, 16)
        Me.LblModelName.TabIndex = 24
        '
        'BtnModelTexturesMore
        '
        Me.BtnModelTexturesMore.Enabled = False
        Me.BtnModelTexturesMore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelTexturesMore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelTexturesMore.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelTexturesMore.Location = New System.Drawing.Point(112, 48)
        Me.BtnModelTexturesMore.Name = "BtnModelTexturesMore"
        Me.BtnModelTexturesMore.Size = New System.Drawing.Size(88, 24)
        Me.BtnModelTexturesMore.TabIndex = 9
        Me.BtnModelTexturesMore.Text = "Texture info..."
        Me.BtnModelTexturesMore.UseVisualStyleBackColor = True
        '
        'LblInfoTextures
        '
        Me.LblInfoTextures.AutoSize = True
        Me.LblInfoTextures.Font = New System.Drawing.Font("Segoe UI Light", 8.25!)
        Me.LblInfoTextures.Location = New System.Drawing.Point(59, 57)
        Me.LblInfoTextures.Name = "LblInfoTextures"
        Me.LblInfoTextures.Size = New System.Drawing.Size(13, 13)
        Me.LblInfoTextures.TabIndex = 7
        Me.LblInfoTextures.Text = "0"
        '
        'LblInfoBones
        '
        Me.LblInfoBones.AutoSize = True
        Me.LblInfoBones.Font = New System.Drawing.Font("Segoe UI Light", 8.25!)
        Me.LblInfoBones.Location = New System.Drawing.Point(59, 44)
        Me.LblInfoBones.Name = "LblInfoBones"
        Me.LblInfoBones.Size = New System.Drawing.Size(13, 13)
        Me.LblInfoBones.TabIndex = 6
        Me.LblInfoBones.Text = "0"
        '
        'LblInfoTriangles
        '
        Me.LblInfoTriangles.AutoSize = True
        Me.LblInfoTriangles.Font = New System.Drawing.Font("Segoe UI Light", 8.25!)
        Me.LblInfoTriangles.Location = New System.Drawing.Point(59, 31)
        Me.LblInfoTriangles.Name = "LblInfoTriangles"
        Me.LblInfoTriangles.Size = New System.Drawing.Size(13, 13)
        Me.LblInfoTriangles.TabIndex = 5
        Me.LblInfoTriangles.Text = "0"
        '
        'LblInfoVertices
        '
        Me.LblInfoVertices.AutoSize = True
        Me.LblInfoVertices.Font = New System.Drawing.Font("Segoe UI Light", 8.25!)
        Me.LblInfoVertices.Location = New System.Drawing.Point(59, 18)
        Me.LblInfoVertices.Name = "LblInfoVertices"
        Me.LblInfoVertices.Size = New System.Drawing.Size(13, 13)
        Me.LblInfoVertices.TabIndex = 4
        Me.LblInfoVertices.Text = "0"
        '
        'LblInfoTexturesDummy
        '
        Me.LblInfoTexturesDummy.AutoSize = True
        Me.LblInfoTexturesDummy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInfoTexturesDummy.Location = New System.Drawing.Point(0, 57)
        Me.LblInfoTexturesDummy.Name = "LblInfoTexturesDummy"
        Me.LblInfoTexturesDummy.Size = New System.Drawing.Size(53, 13)
        Me.LblInfoTexturesDummy.TabIndex = 3
        Me.LblInfoTexturesDummy.Text = "Textures:"
        '
        'LblInfoBonesDummy
        '
        Me.LblInfoBonesDummy.AutoSize = True
        Me.LblInfoBonesDummy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInfoBonesDummy.Location = New System.Drawing.Point(0, 44)
        Me.LblInfoBonesDummy.Name = "LblInfoBonesDummy"
        Me.LblInfoBonesDummy.Size = New System.Drawing.Size(41, 13)
        Me.LblInfoBonesDummy.TabIndex = 2
        Me.LblInfoBonesDummy.Text = "Bones:"
        '
        'LblInfoTrianglesDummy
        '
        Me.LblInfoTrianglesDummy.AutoSize = True
        Me.LblInfoTrianglesDummy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInfoTrianglesDummy.Location = New System.Drawing.Point(0, 31)
        Me.LblInfoTrianglesDummy.Name = "LblInfoTrianglesDummy"
        Me.LblInfoTrianglesDummy.Size = New System.Drawing.Size(56, 13)
        Me.LblInfoTrianglesDummy.TabIndex = 1
        Me.LblInfoTrianglesDummy.Text = "Triangles:"
        '
        'LblInfoVerticesDummy
        '
        Me.LblInfoVerticesDummy.AutoSize = True
        Me.LblInfoVerticesDummy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInfoVerticesDummy.Location = New System.Drawing.Point(0, 18)
        Me.LblInfoVerticesDummy.Name = "LblInfoVerticesDummy"
        Me.LblInfoVerticesDummy.Size = New System.Drawing.Size(50, 13)
        Me.LblInfoVerticesDummy.TabIndex = 0
        Me.LblInfoVerticesDummy.Text = "Vertices:"
        '
        'Screen
        '
        Me.Screen.BackColor = System.Drawing.Color.Black
        Me.Screen.Location = New System.Drawing.Point(0, 0)
        Me.Screen.Name = "Screen"
        Me.Screen.Size = New System.Drawing.Size(768, 480)
        Me.Screen.TabIndex = 21
        Me.Screen.TabStop = False
        '
        'TexturePage
        '
        Me.TexturePage.BackColor = System.Drawing.Color.Transparent
        Me.TexturePage.Controls.Add(Me.GrpTexOptions)
        Me.TexturePage.Controls.Add(Me.GrpTexInfo)
        Me.TexturePage.Controls.Add(Me.GrpTexturePreview)
        Me.TexturePage.Controls.Add(Me.GrpTextures)
        Me.TexturePage.ForeColor = System.Drawing.Color.White
        Me.TexturePage.Location = New System.Drawing.Point(28, 4)
        Me.TexturePage.Name = "TexturePage"
        Me.TexturePage.Padding = New System.Windows.Forms.Padding(3)
        Me.TexturePage.Size = New System.Drawing.Size(768, 560)
        Me.TexturePage.TabIndex = 1
        Me.TexturePage.Text = "Textures"
        '
        'GrpTexOptions
        '
        Me.GrpTexOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureInsertAll)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureSave)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureInsert)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureMode)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureExportAllFF)
        Me.GrpTexOptions.Controls.Add(Me.ProgressTextures)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureExportAll)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureExport)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureOpen)
        Me.GrpTexOptions.ForeColor = System.Drawing.Color.White
        Me.GrpTexOptions.Location = New System.Drawing.Point(206, 480)
        Me.GrpTexOptions.Name = "GrpTexOptions"
        Me.GrpTexOptions.Size = New System.Drawing.Size(562, 80)
        Me.GrpTexOptions.TabIndex = 26
        Me.GrpTexOptions.TabStop = False
        Me.GrpTexOptions.Text = "Options"
        '
        'BtnTextureInsertAll
        '
        Me.BtnTextureInsertAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureInsertAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureInsertAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureInsertAll.Location = New System.Drawing.Point(162, 48)
        Me.BtnTextureInsertAll.Name = "BtnTextureInsertAll"
        Me.BtnTextureInsertAll.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextureInsertAll.TabIndex = 8
        Me.BtnTextureInsertAll.Text = "Import all"
        Me.BtnTextureInsertAll.UseVisualStyleBackColor = True
        '
        'BtnTextureSave
        '
        Me.BtnTextureSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureSave.Location = New System.Drawing.Point(0, 48)
        Me.BtnTextureSave.Name = "BtnTextureSave"
        Me.BtnTextureSave.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextureSave.TabIndex = 6
        Me.BtnTextureSave.Text = "Save"
        Me.BtnTextureSave.UseVisualStyleBackColor = True
        '
        'BtnTextureInsert
        '
        Me.BtnTextureInsert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureInsert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureInsert.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureInsert.Location = New System.Drawing.Point(84, 48)
        Me.BtnTextureInsert.Name = "BtnTextureInsert"
        Me.BtnTextureInsert.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextureInsert.TabIndex = 7
        Me.BtnTextureInsert.Text = "Import"
        Me.BtnTextureInsert.UseVisualStyleBackColor = True
        '
        'BtnTextureMode
        '
        Me.BtnTextureMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureMode.Location = New System.Drawing.Point(490, 18)
        Me.BtnTextureMode.Name = "BtnTextureMode"
        Me.BtnTextureMode.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextureMode.TabIndex = 5
        Me.BtnTextureMode.Text = "Original"
        Me.BtnTextureMode.UseVisualStyleBackColor = True
        '
        'BtnTextureExportAllFF
        '
        Me.BtnTextureExportAllFF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureExportAllFF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureExportAllFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureExportAllFF.Location = New System.Drawing.Point(240, 18)
        Me.BtnTextureExportAllFF.Name = "BtnTextureExportAllFF"
        Me.BtnTextureExportAllFF.Size = New System.Drawing.Size(150, 24)
        Me.BtnTextureExportAllFF.TabIndex = 4
        Me.BtnTextureExportAllFF.Text = "Export all from folder"
        Me.BtnTextureExportAllFF.UseVisualStyleBackColor = True
        '
        'ProgressTextures
        '
        Me.ProgressTextures.Location = New System.Drawing.Point(240, 48)
        Me.ProgressTextures.Name = "ProgressTextures"
        Me.ProgressTextures.Percentage = 0.0!
        Me.ProgressTextures.Size = New System.Drawing.Size(150, 24)
        Me.ProgressTextures.TabIndex = 0
        '
        'BtnTextureExportAll
        '
        Me.BtnTextureExportAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureExportAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureExportAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureExportAll.Location = New System.Drawing.Point(162, 18)
        Me.BtnTextureExportAll.Name = "BtnTextureExportAll"
        Me.BtnTextureExportAll.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextureExportAll.TabIndex = 3
        Me.BtnTextureExportAll.Text = "Export all"
        Me.BtnTextureExportAll.UseVisualStyleBackColor = True
        '
        'BtnTextureExport
        '
        Me.BtnTextureExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureExport.Location = New System.Drawing.Point(84, 18)
        Me.BtnTextureExport.Name = "BtnTextureExport"
        Me.BtnTextureExport.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextureExport.TabIndex = 2
        Me.BtnTextureExport.Text = "Export"
        Me.BtnTextureExport.UseVisualStyleBackColor = True
        '
        'BtnTextureOpen
        '
        Me.BtnTextureOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureOpen.Location = New System.Drawing.Point(0, 18)
        Me.BtnTextureOpen.Name = "BtnTextureOpen"
        Me.BtnTextureOpen.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextureOpen.TabIndex = 1
        Me.BtnTextureOpen.Text = "Open"
        Me.BtnTextureOpen.UseVisualStyleBackColor = True
        '
        'GrpTexInfo
        '
        Me.GrpTexInfo.BackColor = System.Drawing.Color.Transparent
        Me.GrpTexInfo.Controls.Add(Me.LblInfoTextureCD)
        Me.GrpTexInfo.Controls.Add(Me.LblInfoTextureFormat)
        Me.GrpTexInfo.Controls.Add(Me.LblInfoTextureResolution)
        Me.GrpTexInfo.Controls.Add(Me.LblInfoTextureCDDummy)
        Me.GrpTexInfo.Controls.Add(Me.LblInfoTextureFormatDummy)
        Me.GrpTexInfo.Controls.Add(Me.LblInfoTextureResolutionDummy)
        Me.GrpTexInfo.Controls.Add(Me.LblInfoTextureIndex)
        Me.GrpTexInfo.Controls.Add(Me.LblInfoTextureIndexDummy)
        Me.GrpTexInfo.ForeColor = System.Drawing.Color.White
        Me.GrpTexInfo.Location = New System.Drawing.Point(0, 480)
        Me.GrpTexInfo.Name = "GrpTexInfo"
        Me.GrpTexInfo.Size = New System.Drawing.Size(200, 80)
        Me.GrpTexInfo.TabIndex = 25
        Me.GrpTexInfo.TabStop = False
        Me.GrpTexInfo.Text = "Info"
        '
        'LblInfoTextureCD
        '
        Me.LblInfoTextureCD.AutoSize = True
        Me.LblInfoTextureCD.Font = New System.Drawing.Font("Segoe UI Light", 8.25!)
        Me.LblInfoTextureCD.Location = New System.Drawing.Point(77, 57)
        Me.LblInfoTextureCD.Name = "LblInfoTextureCD"
        Me.LblInfoTextureCD.Size = New System.Drawing.Size(19, 13)
        Me.LblInfoTextureCD.TabIndex = 10
        Me.LblInfoTextureCD.Text = "---"
        '
        'LblInfoTextureFormat
        '
        Me.LblInfoTextureFormat.AutoSize = True
        Me.LblInfoTextureFormat.Font = New System.Drawing.Font("Segoe UI Light", 8.25!)
        Me.LblInfoTextureFormat.Location = New System.Drawing.Point(77, 44)
        Me.LblInfoTextureFormat.Name = "LblInfoTextureFormat"
        Me.LblInfoTextureFormat.Size = New System.Drawing.Size(19, 13)
        Me.LblInfoTextureFormat.TabIndex = 9
        Me.LblInfoTextureFormat.Text = "---"
        '
        'LblInfoTextureResolution
        '
        Me.LblInfoTextureResolution.AutoSize = True
        Me.LblInfoTextureResolution.Font = New System.Drawing.Font("Segoe UI Light", 8.25!)
        Me.LblInfoTextureResolution.Location = New System.Drawing.Point(77, 31)
        Me.LblInfoTextureResolution.Name = "LblInfoTextureResolution"
        Me.LblInfoTextureResolution.Size = New System.Drawing.Size(24, 13)
        Me.LblInfoTextureResolution.TabIndex = 8
        Me.LblInfoTextureResolution.Text = "0x0"
        '
        'LblInfoTextureCDDummy
        '
        Me.LblInfoTextureCDDummy.AutoSize = True
        Me.LblInfoTextureCDDummy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInfoTextureCDDummy.Location = New System.Drawing.Point(0, 57)
        Me.LblInfoTextureCDDummy.Name = "LblInfoTextureCDDummy"
        Me.LblInfoTextureCDDummy.Size = New System.Drawing.Size(71, 13)
        Me.LblInfoTextureCDDummy.TabIndex = 7
        Me.LblInfoTextureCDDummy.Text = "Color depth:"
        '
        'LblInfoTextureFormatDummy
        '
        Me.LblInfoTextureFormatDummy.AutoSize = True
        Me.LblInfoTextureFormatDummy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInfoTextureFormatDummy.Location = New System.Drawing.Point(0, 44)
        Me.LblInfoTextureFormatDummy.Name = "LblInfoTextureFormatDummy"
        Me.LblInfoTextureFormatDummy.Size = New System.Drawing.Size(47, 13)
        Me.LblInfoTextureFormatDummy.TabIndex = 6
        Me.LblInfoTextureFormatDummy.Text = "Format:"
        '
        'LblInfoTextureResolutionDummy
        '
        Me.LblInfoTextureResolutionDummy.AutoSize = True
        Me.LblInfoTextureResolutionDummy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInfoTextureResolutionDummy.Location = New System.Drawing.Point(0, 31)
        Me.LblInfoTextureResolutionDummy.Name = "LblInfoTextureResolutionDummy"
        Me.LblInfoTextureResolutionDummy.Size = New System.Drawing.Size(30, 13)
        Me.LblInfoTextureResolutionDummy.TabIndex = 5
        Me.LblInfoTextureResolutionDummy.Text = "Size:"
        '
        'LblInfoTextureIndex
        '
        Me.LblInfoTextureIndex.AutoSize = True
        Me.LblInfoTextureIndex.Font = New System.Drawing.Font("Segoe UI Light", 8.25!)
        Me.LblInfoTextureIndex.Location = New System.Drawing.Point(77, 18)
        Me.LblInfoTextureIndex.Name = "LblInfoTextureIndex"
        Me.LblInfoTextureIndex.Size = New System.Drawing.Size(23, 13)
        Me.LblInfoTextureIndex.TabIndex = 4
        Me.LblInfoTextureIndex.Text = "0/0"
        '
        'LblInfoTextureIndexDummy
        '
        Me.LblInfoTextureIndexDummy.AutoSize = True
        Me.LblInfoTextureIndexDummy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInfoTextureIndexDummy.Location = New System.Drawing.Point(0, 18)
        Me.LblInfoTextureIndexDummy.Name = "LblInfoTextureIndexDummy"
        Me.LblInfoTextureIndexDummy.Size = New System.Drawing.Size(37, 13)
        Me.LblInfoTextureIndexDummy.TabIndex = 0
        Me.LblInfoTextureIndexDummy.Text = "Num.:"
        '
        'GrpTexturePreview
        '
        Me.GrpTexturePreview.Controls.Add(Me.ImgTexture)
        Me.GrpTexturePreview.ForeColor = System.Drawing.Color.White
        Me.GrpTexturePreview.Location = New System.Drawing.Point(206, 0)
        Me.GrpTexturePreview.Name = "GrpTexturePreview"
        Me.GrpTexturePreview.Size = New System.Drawing.Size(562, 480)
        Me.GrpTexturePreview.TabIndex = 28
        Me.GrpTexturePreview.TabStop = False
        Me.GrpTexturePreview.Text = "View"
        '
        'ImgTexture
        '
        Me.ImgTexture.Image = Nothing
        Me.ImgTexture.Location = New System.Drawing.Point(0, 18)
        Me.ImgTexture.Name = "ImgTexture"
        Me.ImgTexture.Size = New System.Drawing.Size(562, 462)
        Me.ImgTexture.TabIndex = 0
        Me.ImgTexture.TabStop = False
        '
        'GrpTextures
        '
        Me.GrpTextures.Controls.Add(Me.LstTextures)
        Me.GrpTextures.ForeColor = System.Drawing.Color.White
        Me.GrpTextures.Location = New System.Drawing.Point(0, 0)
        Me.GrpTextures.Name = "GrpTextures"
        Me.GrpTextures.Size = New System.Drawing.Size(200, 480)
        Me.GrpTextures.TabIndex = 27
        Me.GrpTextures.TabStop = False
        Me.GrpTextures.Text = "Textures"
        '
        'LstTextures
        '
        Me.LstTextures.Location = New System.Drawing.Point(0, 18)
        Me.LstTextures.Name = "LstTextures"
        Me.LstTextures.SelectedIndex = -1
        Me.LstTextures.Size = New System.Drawing.Size(200, 460)
        Me.LstTextures.TabIndex = 9
        Me.LstTextures.TileHeight = 16
        '
        'TextPage
        '
        Me.TextPage.BackColor = System.Drawing.Color.Transparent
        Me.TextPage.Controls.Add(Me.GrpTextOptions)
        Me.TextPage.Controls.Add(Me.GrpTextStrings)
        Me.TextPage.ForeColor = System.Drawing.Color.White
        Me.TextPage.Location = New System.Drawing.Point(28, 4)
        Me.TextPage.Name = "TextPage"
        Me.TextPage.Size = New System.Drawing.Size(768, 560)
        Me.TextPage.TabIndex = 2
        Me.TextPage.Text = "Text"
        '
        'GrpTextOptions
        '
        Me.GrpTextOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpTextOptions.Controls.Add(Me.BtnTextSave)
        Me.GrpTextOptions.Controls.Add(Me.BtnTextImport)
        Me.GrpTextOptions.Controls.Add(Me.BtnTextExport)
        Me.GrpTextOptions.Controls.Add(Me.BtnTextOpen)
        Me.GrpTextOptions.ForeColor = System.Drawing.Color.White
        Me.GrpTextOptions.Location = New System.Drawing.Point(0, 480)
        Me.GrpTextOptions.Name = "GrpTextOptions"
        Me.GrpTextOptions.Size = New System.Drawing.Size(768, 80)
        Me.GrpTextOptions.TabIndex = 27
        Me.GrpTextOptions.TabStop = False
        Me.GrpTextOptions.Text = "Options"
        '
        'BtnTextSave
        '
        Me.BtnTextSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextSave.Location = New System.Drawing.Point(0, 48)
        Me.BtnTextSave.Name = "BtnTextSave"
        Me.BtnTextSave.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextSave.TabIndex = 3
        Me.BtnTextSave.Text = "Save"
        Me.BtnTextSave.UseVisualStyleBackColor = True
        '
        'BtnTextImport
        '
        Me.BtnTextImport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextImport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextImport.Location = New System.Drawing.Point(84, 48)
        Me.BtnTextImport.Name = "BtnTextImport"
        Me.BtnTextImport.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextImport.TabIndex = 4
        Me.BtnTextImport.Text = "Import"
        Me.BtnTextImport.UseVisualStyleBackColor = True
        '
        'BtnTextExport
        '
        Me.BtnTextExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextExport.Location = New System.Drawing.Point(84, 18)
        Me.BtnTextExport.Name = "BtnTextExport"
        Me.BtnTextExport.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextExport.TabIndex = 2
        Me.BtnTextExport.Text = "Export"
        Me.BtnTextExport.UseVisualStyleBackColor = True
        '
        'BtnTextOpen
        '
        Me.BtnTextOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextOpen.Location = New System.Drawing.Point(0, 18)
        Me.BtnTextOpen.Name = "BtnTextOpen"
        Me.BtnTextOpen.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextOpen.TabIndex = 1
        Me.BtnTextOpen.Text = "Open"
        Me.BtnTextOpen.UseVisualStyleBackColor = True
        '
        'GrpTextStrings
        '
        Me.GrpTextStrings.Controls.Add(Me.LstStrings)
        Me.GrpTextStrings.ForeColor = System.Drawing.Color.White
        Me.GrpTextStrings.Location = New System.Drawing.Point(0, 0)
        Me.GrpTextStrings.Name = "GrpTextStrings"
        Me.GrpTextStrings.Size = New System.Drawing.Size(768, 480)
        Me.GrpTextStrings.TabIndex = 26
        Me.GrpTextStrings.TabStop = False
        Me.GrpTextStrings.Text = "Texts (Preview only!)"
        '
        'LstStrings
        '
        Me.LstStrings.Location = New System.Drawing.Point(0, 18)
        Me.LstStrings.Name = "LstStrings"
        Me.LstStrings.SelectedIndex = -1
        Me.LstStrings.Size = New System.Drawing.Size(768, 460)
        Me.LstStrings.TabIndex = 5
        Me.LstStrings.Text = "MyListview1"
        Me.LstStrings.TileHeight = 16
        '
        'GARCPage
        '
        Me.GARCPage.BackColor = System.Drawing.Color.Transparent
        Me.GARCPage.Controls.Add(Me.GrpGARCOptions)
        Me.GARCPage.Controls.Add(Me.GrpFiles)
        Me.GARCPage.ForeColor = System.Drawing.Color.White
        Me.GARCPage.Location = New System.Drawing.Point(28, 4)
        Me.GARCPage.Name = "GARCPage"
        Me.GARCPage.Size = New System.Drawing.Size(768, 560)
        Me.GARCPage.TabIndex = 3
        Me.GARCPage.Text = "Container"
        '
        'GrpGARCOptions
        '
        Me.GrpGARCOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpGARCOptions.Controls.Add(Me.BtnGARCCompression)
        Me.GrpGARCOptions.Controls.Add(Me.BtnGARCSave)
        Me.GrpGARCOptions.Controls.Add(Me.BtnGARCInsert)
        Me.GrpGARCOptions.Controls.Add(Me.ProgressGARC)
        Me.GrpGARCOptions.Controls.Add(Me.BtnGARCExtractAll)
        Me.GrpGARCOptions.Controls.Add(Me.BtnGARCExtract)
        Me.GrpGARCOptions.Controls.Add(Me.BtnGARCOpen)
        Me.GrpGARCOptions.ForeColor = System.Drawing.Color.White
        Me.GrpGARCOptions.Location = New System.Drawing.Point(0, 480)
        Me.GrpGARCOptions.Name = "GrpGARCOptions"
        Me.GrpGARCOptions.Size = New System.Drawing.Size(768, 80)
        Me.GrpGARCOptions.TabIndex = 25
        Me.GrpGARCOptions.TabStop = False
        Me.GrpGARCOptions.Text = "Options"
        '
        'BtnGARCCompression
        '
        Me.BtnGARCCompression.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnGARCCompression.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnGARCCompression.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGARCCompression.Location = New System.Drawing.Point(615, 18)
        Me.BtnGARCCompression.Name = "BtnGARCCompression"
        Me.BtnGARCCompression.Size = New System.Drawing.Size(150, 24)
        Me.BtnGARCCompression.TabIndex = 4
        Me.BtnGARCCompression.Text = "Optimal compression"
        Me.BtnGARCCompression.UseVisualStyleBackColor = True
        '
        'BtnGARCSave
        '
        Me.BtnGARCSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnGARCSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnGARCSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGARCSave.Location = New System.Drawing.Point(0, 48)
        Me.BtnGARCSave.Name = "BtnGARCSave"
        Me.BtnGARCSave.Size = New System.Drawing.Size(72, 24)
        Me.BtnGARCSave.TabIndex = 5
        Me.BtnGARCSave.Text = "Save"
        Me.BtnGARCSave.UseVisualStyleBackColor = True
        '
        'BtnGARCInsert
        '
        Me.BtnGARCInsert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnGARCInsert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnGARCInsert.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGARCInsert.Location = New System.Drawing.Point(84, 48)
        Me.BtnGARCInsert.Name = "BtnGARCInsert"
        Me.BtnGARCInsert.Size = New System.Drawing.Size(72, 24)
        Me.BtnGARCInsert.TabIndex = 6
        Me.BtnGARCInsert.Text = "Insert"
        Me.BtnGARCInsert.UseVisualStyleBackColor = True
        '
        'ProgressGARC
        '
        Me.ProgressGARC.Location = New System.Drawing.Point(615, 48)
        Me.ProgressGARC.Name = "ProgressGARC"
        Me.ProgressGARC.Percentage = 0.0!
        Me.ProgressGARC.Size = New System.Drawing.Size(150, 24)
        Me.ProgressGARC.TabIndex = 0
        '
        'BtnGARCExtractAll
        '
        Me.BtnGARCExtractAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnGARCExtractAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnGARCExtractAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGARCExtractAll.Location = New System.Drawing.Point(162, 18)
        Me.BtnGARCExtractAll.Name = "BtnGARCExtractAll"
        Me.BtnGARCExtractAll.Size = New System.Drawing.Size(72, 24)
        Me.BtnGARCExtractAll.TabIndex = 3
        Me.BtnGARCExtractAll.Text = "Extract all"
        Me.BtnGARCExtractAll.UseVisualStyleBackColor = True
        '
        'BtnGARCExtract
        '
        Me.BtnGARCExtract.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnGARCExtract.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnGARCExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGARCExtract.Location = New System.Drawing.Point(84, 18)
        Me.BtnGARCExtract.Name = "BtnGARCExtract"
        Me.BtnGARCExtract.Size = New System.Drawing.Size(72, 24)
        Me.BtnGARCExtract.TabIndex = 2
        Me.BtnGARCExtract.Text = "Extract"
        Me.BtnGARCExtract.UseVisualStyleBackColor = True
        '
        'BtnGARCOpen
        '
        Me.BtnGARCOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnGARCOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnGARCOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGARCOpen.Location = New System.Drawing.Point(0, 18)
        Me.BtnGARCOpen.Name = "BtnGARCOpen"
        Me.BtnGARCOpen.Size = New System.Drawing.Size(72, 24)
        Me.BtnGARCOpen.TabIndex = 1
        Me.BtnGARCOpen.Text = "Open"
        Me.BtnGARCOpen.UseVisualStyleBackColor = True
        '
        'GrpFiles
        '
        Me.GrpFiles.Controls.Add(Me.LstFiles)
        Me.GrpFiles.ForeColor = System.Drawing.Color.White
        Me.GrpFiles.Location = New System.Drawing.Point(0, 0)
        Me.GrpFiles.Name = "GrpFiles"
        Me.GrpFiles.Size = New System.Drawing.Size(768, 480)
        Me.GrpFiles.TabIndex = 24
        Me.GrpFiles.TabStop = False
        Me.GrpFiles.Text = "Files"
        '
        'LstFiles
        '
        Me.LstFiles.Location = New System.Drawing.Point(0, 18)
        Me.LstFiles.Name = "LstFiles"
        Me.LstFiles.SelectedIndex = -1
        Me.LstFiles.Size = New System.Drawing.Size(768, 460)
        Me.LstFiles.TabIndex = 7
        Me.LstFiles.TileHeight = 16
        '
        'ROMPage
        '
        Me.ROMPage.BackColor = System.Drawing.Color.Transparent
        Me.ROMPage.Controls.Add(Me.GrpROMLog)
        Me.ROMPage.Controls.Add(Me.GrpROMOptions)
        Me.ROMPage.ForeColor = System.Drawing.Color.White
        Me.ROMPage.Location = New System.Drawing.Point(28, 4)
        Me.ROMPage.Name = "ROMPage"
        Me.ROMPage.Size = New System.Drawing.Size(768, 560)
        Me.ROMPage.TabIndex = 5
        Me.ROMPage.Text = "ROM"
        '
        'GrpROMLog
        '
        Me.GrpROMLog.Controls.Add(Me.LstROMLog)
        Me.GrpROMLog.ForeColor = System.Drawing.Color.White
        Me.GrpROMLog.Location = New System.Drawing.Point(0, 0)
        Me.GrpROMLog.Name = "GrpROMLog"
        Me.GrpROMLog.Size = New System.Drawing.Size(768, 480)
        Me.GrpROMLog.TabIndex = 30
        Me.GrpROMLog.TabStop = False
        Me.GrpROMLog.Text = "Log"
        '
        'LstROMLog
        '
        Me.LstROMLog.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstROMLog.Location = New System.Drawing.Point(0, 18)
        Me.LstROMLog.Name = "LstROMLog"
        Me.LstROMLog.SelectedIndex = -1
        Me.LstROMLog.Size = New System.Drawing.Size(768, 460)
        Me.LstROMLog.TabIndex = 3
        Me.LstROMLog.TileHeight = 16
        '
        'GrpROMOptions
        '
        Me.GrpROMOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpROMOptions.Controls.Add(Me.BtnROMDecrypt)
        Me.GrpROMOptions.Controls.Add(Me.BtnROMOpenXorPad)
        Me.GrpROMOptions.Controls.Add(Me.BtnROMOpen)
        Me.GrpROMOptions.ForeColor = System.Drawing.Color.White
        Me.GrpROMOptions.Location = New System.Drawing.Point(0, 480)
        Me.GrpROMOptions.Name = "GrpROMOptions"
        Me.GrpROMOptions.Size = New System.Drawing.Size(768, 80)
        Me.GrpROMOptions.TabIndex = 31
        Me.GrpROMOptions.TabStop = False
        Me.GrpROMOptions.Text = "Options"
        '
        'BtnROMDecrypt
        '
        Me.BtnROMDecrypt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnROMDecrypt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnROMDecrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnROMDecrypt.Location = New System.Drawing.Point(156, 18)
        Me.BtnROMDecrypt.Name = "BtnROMDecrypt"
        Me.BtnROMDecrypt.Size = New System.Drawing.Size(72, 24)
        Me.BtnROMDecrypt.TabIndex = 4
        Me.BtnROMDecrypt.Text = "Decrypt"
        Me.BtnROMDecrypt.UseVisualStyleBackColor = True
        '
        'BtnROMOpenXorPad
        '
        Me.BtnROMOpenXorPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnROMOpenXorPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnROMOpenXorPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnROMOpenXorPad.Location = New System.Drawing.Point(78, 18)
        Me.BtnROMOpenXorPad.Name = "BtnROMOpenXorPad"
        Me.BtnROMOpenXorPad.Size = New System.Drawing.Size(72, 24)
        Me.BtnROMOpenXorPad.TabIndex = 3
        Me.BtnROMOpenXorPad.Text = "Open XOR"
        Me.BtnROMOpenXorPad.UseVisualStyleBackColor = True
        '
        'BtnROMOpen
        '
        Me.BtnROMOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnROMOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnROMOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnROMOpen.Location = New System.Drawing.Point(0, 18)
        Me.BtnROMOpen.Name = "BtnROMOpen"
        Me.BtnROMOpen.Size = New System.Drawing.Size(72, 24)
        Me.BtnROMOpen.TabIndex = 2
        Me.BtnROMOpen.Text = "Open"
        Me.BtnROMOpen.UseVisualStyleBackColor = True
        '
        'SearchPage
        '
        Me.SearchPage.BackColor = System.Drawing.Color.Transparent
        Me.SearchPage.Controls.Add(Me.GrpMatches)
        Me.SearchPage.Controls.Add(Me.GrpSearchOptions)
        Me.SearchPage.ForeColor = System.Drawing.Color.White
        Me.SearchPage.Location = New System.Drawing.Point(28, 4)
        Me.SearchPage.Name = "SearchPage"
        Me.SearchPage.Size = New System.Drawing.Size(768, 560)
        Me.SearchPage.TabIndex = 4
        Me.SearchPage.Text = "Search"
        '
        'GrpMatches
        '
        Me.GrpMatches.Controls.Add(Me.LstMatches)
        Me.GrpMatches.ForeColor = System.Drawing.Color.White
        Me.GrpMatches.Location = New System.Drawing.Point(0, 0)
        Me.GrpMatches.Name = "GrpMatches"
        Me.GrpMatches.Size = New System.Drawing.Size(768, 480)
        Me.GrpMatches.TabIndex = 28
        Me.GrpMatches.TabStop = False
        Me.GrpMatches.Text = "Matches"
        '
        'LstMatches
        '
        Me.LstMatches.Location = New System.Drawing.Point(0, 18)
        Me.LstMatches.Name = "LstMatches"
        Me.LstMatches.SelectedIndex = -1
        Me.LstMatches.Size = New System.Drawing.Size(768, 460)
        Me.LstMatches.TabIndex = 3
        Me.LstMatches.TileHeight = 16
        '
        'GrpSearchOptions
        '
        Me.GrpSearchOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpSearchOptions.Controls.Add(Me.TxtSearch)
        Me.GrpSearchOptions.Controls.Add(Me.ProgressSearch)
        Me.GrpSearchOptions.Controls.Add(Me.BtnSearch)
        Me.GrpSearchOptions.ForeColor = System.Drawing.Color.White
        Me.GrpSearchOptions.Location = New System.Drawing.Point(0, 480)
        Me.GrpSearchOptions.Name = "GrpSearchOptions"
        Me.GrpSearchOptions.Size = New System.Drawing.Size(768, 80)
        Me.GrpSearchOptions.TabIndex = 29
        Me.GrpSearchOptions.TabStop = False
        Me.GrpSearchOptions.Text = "Options"
        '
        'TxtSearch
        '
        Me.TxtSearch.BackColor = System.Drawing.Color.Black
        Me.TxtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtSearch.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtSearch.ForeColor = System.Drawing.Color.White
        Me.TxtSearch.Location = New System.Drawing.Point(0, 18)
        Me.TxtSearch.Name = "TxtSearch"
        Me.TxtSearch.Size = New System.Drawing.Size(150, 24)
        Me.TxtSearch.TabIndex = 1
        '
        'ProgressSearch
        '
        Me.ProgressSearch.Location = New System.Drawing.Point(0, 48)
        Me.ProgressSearch.Name = "ProgressSearch"
        Me.ProgressSearch.Percentage = 0.0!
        Me.ProgressSearch.Size = New System.Drawing.Size(228, 24)
        Me.ProgressSearch.TabIndex = 0
        '
        'BtnSearch
        '
        Me.BtnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSearch.Location = New System.Drawing.Point(156, 18)
        Me.BtnSearch.Name = "BtnSearch"
        Me.BtnSearch.Size = New System.Drawing.Size(72, 24)
        Me.BtnSearch.TabIndex = 2
        Me.BtnSearch.Text = "Search"
        Me.BtnSearch.UseVisualStyleBackColor = True
        '
        'Title
        '
        Me.Title.AutoSize = True
        Me.Title.BackColor = System.Drawing.Color.Transparent
        Me.Title.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.ForeColor = System.Drawing.Color.White
        Me.Title.Location = New System.Drawing.Point(349, 4)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(103, 25)
        Me.Title.TabIndex = 18
        Me.Title.Text = "Ohana3DS"
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.MainTabs)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.BtnMinimize)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.Splash)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FrmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ohana3DS"
        CType(Me.Splash, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainTabs.ResumeLayout(False)
        Me.ModelPage.ResumeLayout(False)
        Me.GrpOptions.ResumeLayout(False)
        Me.GrpInfo.ResumeLayout(False)
        Me.GrpInfo.PerformLayout()
        CType(Me.Screen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TexturePage.ResumeLayout(False)
        Me.GrpTexOptions.ResumeLayout(False)
        Me.GrpTexInfo.ResumeLayout(False)
        Me.GrpTexInfo.PerformLayout()
        Me.GrpTexturePreview.ResumeLayout(False)
        CType(Me.ImgTexture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpTextures.ResumeLayout(False)
        Me.TextPage.ResumeLayout(False)
        Me.GrpTextOptions.ResumeLayout(False)
        Me.GrpTextStrings.ResumeLayout(False)
        Me.GARCPage.ResumeLayout(False)
        Me.GrpGARCOptions.ResumeLayout(False)
        Me.GrpFiles.ResumeLayout(False)
        Me.ROMPage.ResumeLayout(False)
        Me.GrpROMLog.ResumeLayout(False)
        Me.GrpROMOptions.ResumeLayout(False)
        Me.SearchPage.ResumeLayout(False)
        Me.GrpMatches.ResumeLayout(False)
        Me.GrpSearchOptions.ResumeLayout(False)
        Me.GrpSearchOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnClose As System.Windows.Forms.Label
    Friend WithEvents BtnMinimize As System.Windows.Forms.Label
    Friend WithEvents Title As Ohana3DS.MyWindowTitle
    Friend WithEvents MainTabs As Ohana3DS.MyTabcontrol
    Friend WithEvents ModelPage As System.Windows.Forms.TabPage
    Friend WithEvents TexturePage As System.Windows.Forms.TabPage
    Friend WithEvents TextPage As System.Windows.Forms.TabPage
    Friend WithEvents GARCPage As System.Windows.Forms.TabPage
    Friend WithEvents SearchPage As System.Windows.Forms.TabPage
    Friend WithEvents ROMPage As System.Windows.Forms.TabPage
    Friend WithEvents GrpOptions As Ohana3DS.MyGroupbox
    Friend WithEvents BtnModelScale As System.Windows.Forms.Button
    Friend WithEvents ProgressModels As Ohana3DS.MyProgressbar
    Friend WithEvents BtnModelExportAllFF As System.Windows.Forms.Button
    Friend WithEvents BtnModelExport As System.Windows.Forms.Button
    Friend WithEvents BtnModelOpen As System.Windows.Forms.Button
    Friend WithEvents GrpInfo As Ohana3DS.MyGroupbox
    Friend WithEvents BtnModelTexturesMore As System.Windows.Forms.Button
    Friend WithEvents LblInfoTextures As System.Windows.Forms.Label
    Friend WithEvents LblInfoBones As System.Windows.Forms.Label
    Friend WithEvents LblInfoTriangles As System.Windows.Forms.Label
    Friend WithEvents LblInfoVertices As System.Windows.Forms.Label
    Friend WithEvents LblInfoTexturesDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoBonesDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoTrianglesDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoVerticesDummy As System.Windows.Forms.Label
    Friend WithEvents Screen As System.Windows.Forms.PictureBox
    Friend WithEvents GrpTexOptions As Ohana3DS.MyGroupbox
    Friend WithEvents BtnTextureMode As System.Windows.Forms.Button
    Friend WithEvents BtnTextureExportAllFF As System.Windows.Forms.Button
    Friend WithEvents ProgressTextures As Ohana3DS.MyProgressbar
    Friend WithEvents BtnTextureExportAll As System.Windows.Forms.Button
    Friend WithEvents BtnTextureExport As System.Windows.Forms.Button
    Friend WithEvents BtnTextureOpen As System.Windows.Forms.Button
    Friend WithEvents GrpTexInfo As Ohana3DS.MyGroupbox
    Friend WithEvents LblInfoTextureCD As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureFormat As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureResolution As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureCDDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureFormatDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureResolutionDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureIndex As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureIndexDummy As System.Windows.Forms.Label
    Friend WithEvents GrpTexturePreview As Ohana3DS.MyGroupbox
    Friend WithEvents GrpTextures As Ohana3DS.MyGroupbox
    Friend WithEvents GrpGARCOptions As Ohana3DS.MyGroupbox
    Friend WithEvents ProgressGARC As Ohana3DS.MyProgressbar
    Friend WithEvents BtnGARCExtractAll As System.Windows.Forms.Button
    Friend WithEvents BtnGARCExtract As System.Windows.Forms.Button
    Friend WithEvents BtnGARCOpen As System.Windows.Forms.Button
    Friend WithEvents GrpFiles As Ohana3DS.MyGroupbox
    Friend WithEvents GrpMatches As Ohana3DS.MyGroupbox
    Friend WithEvents GrpSearchOptions As Ohana3DS.MyGroupbox
    Friend WithEvents TxtSearch As System.Windows.Forms.TextBox
    Friend WithEvents ProgressSearch As Ohana3DS.MyProgressbar
    Friend WithEvents BtnSearch As System.Windows.Forms.Button
    Friend WithEvents Splash As System.Windows.Forms.PictureBox
    Friend WithEvents BtnTextureInsert As System.Windows.Forms.Button
    Friend WithEvents BtnTextureSave As System.Windows.Forms.Button
    Friend WithEvents GrpTextOptions As Ohana3DS.MyGroupbox
    Friend WithEvents BtnTextExport As System.Windows.Forms.Button
    Friend WithEvents BtnTextOpen As System.Windows.Forms.Button
    Friend WithEvents GrpTextStrings As Ohana3DS.MyGroupbox
    Friend WithEvents BtnTextImport As System.Windows.Forms.Button
    Friend WithEvents BtnTextSave As System.Windows.Forms.Button
    Friend WithEvents LstTextures As Ohana3DS.MyListview
    Friend WithEvents LstFiles As Ohana3DS.MyListview
    Friend WithEvents LstMatches As Ohana3DS.MyListview
    Friend WithEvents BtnGARCInsert As System.Windows.Forms.Button
    Friend WithEvents BtnGARCSave As System.Windows.Forms.Button
    Friend WithEvents BtnGARCCompression As System.Windows.Forms.Button
    Friend WithEvents BtnModelVertexEditor As System.Windows.Forms.Button
    Friend WithEvents BtnModelSave As System.Windows.Forms.Button
    Friend WithEvents BtnModelMapEditor As System.Windows.Forms.Button
    Friend WithEvents LstStrings As Ohana3DS.MyListview
    Friend WithEvents ImgTexture As Ohana3DS.MyPicturebox
    Friend WithEvents BtnTextureInsertAll As System.Windows.Forms.Button
    Friend WithEvents LblModelName As System.Windows.Forms.Label
    Friend WithEvents ModelNameTip As System.Windows.Forms.ToolTip
    Friend WithEvents GrpROMLog As Ohana3DS.MyGroupbox
    Friend WithEvents LstROMLog As Ohana3DS.MyListview
    Friend WithEvents GrpROMOptions As Ohana3DS.MyGroupbox
    Friend WithEvents BtnROMDecrypt As System.Windows.Forms.Button
    Friend WithEvents BtnROMOpenXorPad As System.Windows.Forms.Button
    Friend WithEvents BtnROMOpen As System.Windows.Forms.Button
    Friend WithEvents colorBG As System.Windows.Forms.ColorDialog

End Class
