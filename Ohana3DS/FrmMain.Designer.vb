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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.BtnClose = New System.Windows.Forms.Label()
        Me.BtnMinimize = New System.Windows.Forms.Label()
        Me.PanelModel = New System.Windows.Forms.Panel()
        Me.Screen = New System.Windows.Forms.PictureBox()
        Me.PanelTextures = New System.Windows.Forms.Panel()
        Me.PanelGARC = New System.Windows.Forms.Panel()
        Me.PanelSearch = New System.Windows.Forms.Panel()
        Me.PanelSplash = New System.Windows.Forms.Panel()
        Me.GrpOptions = New Ohana3DS.MyGroupbox()
        Me.BtnModelScale = New System.Windows.Forms.Button()
        Me.ProgressModels = New Ohana3DS.MyProgressbar()
        Me.BtnModelBCHVer = New System.Windows.Forms.Button()
        Me.BtnModelExportAllFF = New System.Windows.Forms.Button()
        Me.BtnModelExport = New System.Windows.Forms.Button()
        Me.BtnModelOpen = New System.Windows.Forms.Button()
        Me.GrpInfo = New Ohana3DS.MyGroupbox()
        Me.BtnModelTexturesMore = New System.Windows.Forms.Button()
        Me.LblInfoTextures = New System.Windows.Forms.Label()
        Me.LblInfoBones = New System.Windows.Forms.Label()
        Me.LblInfoTriangles = New System.Windows.Forms.Label()
        Me.LblInfoVertices = New System.Windows.Forms.Label()
        Me.LblInfoTexturesDummy = New System.Windows.Forms.Label()
        Me.LblInfoBonesDummy = New System.Windows.Forms.Label()
        Me.LblInfoTrianglesDummy = New System.Windows.Forms.Label()
        Me.LblInfoVerticesDummy = New System.Windows.Forms.Label()
        Me.GrpTexturePreview = New Ohana3DS.MyGroupbox()
        Me.ImgTexture_Container = New System.Windows.Forms.Panel()
        Me.ImgTexture = New System.Windows.Forms.PictureBox()
        Me.GrpTextures = New Ohana3DS.MyGroupbox()
        Me.LstTextures_Container = New System.Windows.Forms.Panel()
        Me.LstTextures = New Ohana3DS.MyListview()
        Me.GrpTexOptions = New Ohana3DS.MyGroupbox()
        Me.BtnTextureMode = New System.Windows.Forms.Button()
        Me.BtnTextureExportAllFF = New System.Windows.Forms.Button()
        Me.ProgressTextures = New Ohana3DS.MyProgressbar()
        Me.BtnTextureBCHVer = New System.Windows.Forms.Button()
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
        Me.GrpGARCOptions = New Ohana3DS.MyGroupbox()
        Me.ProgressGARC = New Ohana3DS.MyProgressbar()
        Me.BtnGARCExtractAll = New System.Windows.Forms.Button()
        Me.BtnGARCExtract = New System.Windows.Forms.Button()
        Me.BtnOpenGARC = New System.Windows.Forms.Button()
        Me.GrpFiles = New Ohana3DS.MyGroupbox()
        Me.LstFiles_Container = New System.Windows.Forms.Panel()
        Me.LstFiles = New Ohana3DS.MyListview()
        Me.GrpMatches = New Ohana3DS.MyGroupbox()
        Me.LstMatches_Container = New System.Windows.Forms.Panel()
        Me.LstMatches = New Ohana3DS.MyListview()
        Me.GrpSearchOptions = New Ohana3DS.MyGroupbox()
        Me.TxtSearch = New System.Windows.Forms.TextBox()
        Me.ProgressSearch = New Ohana3DS.MyProgressbar()
        Me.BtnSearch = New System.Windows.Forms.Button()
        Me.Title = New Ohana3DS.MyWindowTitle()
        Me.SideBar = New Ohana3DS.MyListview()
        Me.PanelModel.SuspendLayout()
        CType(Me.Screen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelTextures.SuspendLayout()
        Me.PanelGARC.SuspendLayout()
        Me.PanelSearch.SuspendLayout()
        Me.GrpOptions.SuspendLayout()
        Me.GrpInfo.SuspendLayout()
        Me.GrpTexturePreview.SuspendLayout()
        Me.ImgTexture_Container.SuspendLayout()
        CType(Me.ImgTexture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpTextures.SuspendLayout()
        Me.LstTextures_Container.SuspendLayout()
        Me.GrpTexOptions.SuspendLayout()
        Me.GrpTexInfo.SuspendLayout()
        Me.GrpGARCOptions.SuspendLayout()
        Me.GrpFiles.SuspendLayout()
        Me.LstFiles_Container.SuspendLayout()
        Me.GrpMatches.SuspendLayout()
        Me.LstMatches_Container.SuspendLayout()
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
        Me.BtnClose.TabIndex = 14
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
        'PanelModel
        '
        Me.PanelModel.BackColor = System.Drawing.Color.Transparent
        Me.PanelModel.Controls.Add(Me.GrpOptions)
        Me.PanelModel.Controls.Add(Me.GrpInfo)
        Me.PanelModel.Controls.Add(Me.Screen)
        Me.PanelModel.Location = New System.Drawing.Point(146, 32)
        Me.PanelModel.Name = "PanelModel"
        Me.PanelModel.Size = New System.Drawing.Size(654, 568)
        Me.PanelModel.TabIndex = 0
        Me.PanelModel.Visible = False
        '
        'Screen
        '
        Me.Screen.BackColor = System.Drawing.Color.Black
        Me.Screen.Location = New System.Drawing.Point(7, 0)
        Me.Screen.Name = "Screen"
        Me.Screen.Size = New System.Drawing.Size(640, 480)
        Me.Screen.TabIndex = 18
        Me.Screen.TabStop = False
        '
        'PanelTextures
        '
        Me.PanelTextures.BackColor = System.Drawing.Color.Transparent
        Me.PanelTextures.Controls.Add(Me.GrpTexturePreview)
        Me.PanelTextures.Controls.Add(Me.GrpTextures)
        Me.PanelTextures.Controls.Add(Me.GrpTexOptions)
        Me.PanelTextures.Controls.Add(Me.GrpTexInfo)
        Me.PanelTextures.Location = New System.Drawing.Point(146, 32)
        Me.PanelTextures.Name = "PanelTextures"
        Me.PanelTextures.Size = New System.Drawing.Size(654, 568)
        Me.PanelTextures.TabIndex = 0
        Me.PanelTextures.Visible = False
        '
        'PanelGARC
        '
        Me.PanelGARC.BackColor = System.Drawing.Color.Transparent
        Me.PanelGARC.Controls.Add(Me.GrpGARCOptions)
        Me.PanelGARC.Controls.Add(Me.GrpFiles)
        Me.PanelGARC.Location = New System.Drawing.Point(146, 32)
        Me.PanelGARC.Name = "PanelGARC"
        Me.PanelGARC.Size = New System.Drawing.Size(654, 568)
        Me.PanelGARC.TabIndex = 0
        Me.PanelGARC.Visible = False
        '
        'PanelSearch
        '
        Me.PanelSearch.BackColor = System.Drawing.Color.Transparent
        Me.PanelSearch.Controls.Add(Me.GrpMatches)
        Me.PanelSearch.Controls.Add(Me.GrpSearchOptions)
        Me.PanelSearch.Location = New System.Drawing.Point(146, 32)
        Me.PanelSearch.Name = "PanelSearch"
        Me.PanelSearch.Size = New System.Drawing.Size(654, 568)
        Me.PanelSearch.TabIndex = 0
        Me.PanelSearch.Visible = False
        '
        'PanelSplash
        '
        Me.PanelSplash.BackColor = System.Drawing.Color.Transparent
        Me.PanelSplash.BackgroundImage = CType(resources.GetObject("PanelSplash.BackgroundImage"), System.Drawing.Image)
        Me.PanelSplash.Location = New System.Drawing.Point(146, 32)
        Me.PanelSplash.Name = "PanelSplash"
        Me.PanelSplash.Size = New System.Drawing.Size(654, 568)
        Me.PanelSplash.TabIndex = 0
        '
        'GrpOptions
        '
        Me.GrpOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpOptions.Controls.Add(Me.BtnModelScale)
        Me.GrpOptions.Controls.Add(Me.ProgressModels)
        Me.GrpOptions.Controls.Add(Me.BtnModelBCHVer)
        Me.GrpOptions.Controls.Add(Me.BtnModelExportAllFF)
        Me.GrpOptions.Controls.Add(Me.BtnModelExport)
        Me.GrpOptions.Controls.Add(Me.BtnModelOpen)
        Me.GrpOptions.ForeColor = System.Drawing.Color.White
        Me.GrpOptions.Location = New System.Drawing.Point(213, 486)
        Me.GrpOptions.Name = "GrpOptions"
        Me.GrpOptions.Size = New System.Drawing.Size(434, 82)
        Me.GrpOptions.TabIndex = 20
        Me.GrpOptions.TabStop = False
        Me.GrpOptions.Text = "Options"
        '
        'BtnModelScale
        '
        Me.BtnModelScale.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelScale.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelScale.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelScale.Location = New System.Drawing.Point(284, 18)
        Me.BtnModelScale.Name = "BtnModelScale"
        Me.BtnModelScale.Size = New System.Drawing.Size(72, 24)
        Me.BtnModelScale.TabIndex = 4
        Me.BtnModelScale.Text = "Scale 1:32"
        Me.BtnModelScale.UseVisualStyleBackColor = True
        '
        'ProgressModels
        '
        Me.ProgressModels.Location = New System.Drawing.Point(156, 48)
        Me.ProgressModels.Name = "ProgressModels"
        Me.ProgressModels.Percentage = 0.0!
        Me.ProgressModels.Size = New System.Drawing.Size(278, 24)
        Me.ProgressModels.TabIndex = 0
        '
        'BtnModelBCHVer
        '
        Me.BtnModelBCHVer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelBCHVer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelBCHVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelBCHVer.Location = New System.Drawing.Point(362, 18)
        Me.BtnModelBCHVer.Name = "BtnModelBCHVer"
        Me.BtnModelBCHVer.Size = New System.Drawing.Size(72, 24)
        Me.BtnModelBCHVer.TabIndex = 5
        Me.BtnModelBCHVer.Text = "X/Y"
        Me.BtnModelBCHVer.UseVisualStyleBackColor = True
        '
        'BtnModelExportAllFF
        '
        Me.BtnModelExportAllFF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelExportAllFF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelExportAllFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelExportAllFF.Location = New System.Drawing.Point(0, 48)
        Me.BtnModelExportAllFF.Name = "BtnModelExportAllFF"
        Me.BtnModelExportAllFF.Size = New System.Drawing.Size(150, 24)
        Me.BtnModelExportAllFF.TabIndex = 3
        Me.BtnModelExportAllFF.Text = "Export all from folder"
        Me.BtnModelExportAllFF.UseVisualStyleBackColor = True
        '
        'BtnModelExport
        '
        Me.BtnModelExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelExport.Location = New System.Drawing.Point(78, 18)
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
        Me.GrpInfo.Location = New System.Drawing.Point(7, 486)
        Me.GrpInfo.Name = "GrpInfo"
        Me.GrpInfo.Size = New System.Drawing.Size(200, 82)
        Me.GrpInfo.TabIndex = 19
        Me.GrpInfo.TabStop = False
        Me.GrpInfo.Text = "Info"
        '
        'BtnModelTexturesMore
        '
        Me.BtnModelTexturesMore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnModelTexturesMore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnModelTexturesMore.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnModelTexturesMore.Location = New System.Drawing.Point(112, 48)
        Me.BtnModelTexturesMore.Name = "BtnModelTexturesMore"
        Me.BtnModelTexturesMore.Size = New System.Drawing.Size(88, 24)
        Me.BtnModelTexturesMore.TabIndex = 6
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
        'GrpTexturePreview
        '
        Me.GrpTexturePreview.Controls.Add(Me.ImgTexture_Container)
        Me.GrpTexturePreview.ForeColor = System.Drawing.Color.White
        Me.GrpTexturePreview.Location = New System.Drawing.Point(213, 3)
        Me.GrpTexturePreview.Name = "GrpTexturePreview"
        Me.GrpTexturePreview.Size = New System.Drawing.Size(434, 478)
        Me.GrpTexturePreview.TabIndex = 24
        Me.GrpTexturePreview.TabStop = False
        Me.GrpTexturePreview.Text = "View"
        '
        'ImgTexture_Container
        '
        Me.ImgTexture_Container.AutoScroll = True
        Me.ImgTexture_Container.Controls.Add(Me.ImgTexture)
        Me.ImgTexture_Container.Location = New System.Drawing.Point(0, 18)
        Me.ImgTexture_Container.Name = "ImgTexture_Container"
        Me.ImgTexture_Container.Size = New System.Drawing.Size(434, 460)
        Me.ImgTexture_Container.TabIndex = 0
        '
        'ImgTexture
        '
        Me.ImgTexture.Location = New System.Drawing.Point(0, 0)
        Me.ImgTexture.Name = "ImgTexture"
        Me.ImgTexture.Size = New System.Drawing.Size(128, 128)
        Me.ImgTexture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.ImgTexture.TabIndex = 1
        Me.ImgTexture.TabStop = False
        '
        'GrpTextures
        '
        Me.GrpTextures.Controls.Add(Me.LstTextures_Container)
        Me.GrpTextures.ForeColor = System.Drawing.Color.White
        Me.GrpTextures.Location = New System.Drawing.Point(7, 3)
        Me.GrpTextures.Name = "GrpTextures"
        Me.GrpTextures.Size = New System.Drawing.Size(200, 478)
        Me.GrpTextures.TabIndex = 23
        Me.GrpTextures.TabStop = False
        Me.GrpTextures.Text = "Textures"
        '
        'LstTextures_Container
        '
        Me.LstTextures_Container.AutoScroll = True
        Me.LstTextures_Container.Controls.Add(Me.LstTextures)
        Me.LstTextures_Container.Location = New System.Drawing.Point(0, 18)
        Me.LstTextures_Container.Name = "LstTextures_Container"
        Me.LstTextures_Container.Size = New System.Drawing.Size(200, 460)
        Me.LstTextures_Container.TabIndex = 0
        '
        'LstTextures
        '
        Me.LstTextures.Location = New System.Drawing.Point(0, 0)
        Me.LstTextures.MinHeight = 460
        Me.LstTextures.Name = "LstTextures"
        Me.LstTextures.Size = New System.Drawing.Size(183, 460)
        Me.LstTextures.TabIndex = 0
        Me.LstTextures.TileHeight = 16
        '
        'GrpTexOptions
        '
        Me.GrpTexOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureMode)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureExportAllFF)
        Me.GrpTexOptions.Controls.Add(Me.ProgressTextures)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureBCHVer)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureExportAll)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureExport)
        Me.GrpTexOptions.Controls.Add(Me.BtnTextureOpen)
        Me.GrpTexOptions.ForeColor = System.Drawing.Color.White
        Me.GrpTexOptions.Location = New System.Drawing.Point(213, 486)
        Me.GrpTexOptions.Name = "GrpTexOptions"
        Me.GrpTexOptions.Size = New System.Drawing.Size(434, 81)
        Me.GrpTexOptions.TabIndex = 22
        Me.GrpTexOptions.TabStop = False
        Me.GrpTexOptions.Text = "Options"
        '
        'BtnTextureMode
        '
        Me.BtnTextureMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureMode.Location = New System.Drawing.Point(284, 18)
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
        Me.BtnTextureExportAllFF.Location = New System.Drawing.Point(0, 48)
        Me.BtnTextureExportAllFF.Name = "BtnTextureExportAllFF"
        Me.BtnTextureExportAllFF.Size = New System.Drawing.Size(150, 24)
        Me.BtnTextureExportAllFF.TabIndex = 4
        Me.BtnTextureExportAllFF.Text = "Export all from folder"
        Me.BtnTextureExportAllFF.UseVisualStyleBackColor = True
        '
        'ProgressTextures
        '
        Me.ProgressTextures.Location = New System.Drawing.Point(156, 48)
        Me.ProgressTextures.Name = "ProgressTextures"
        Me.ProgressTextures.Percentage = 0.0!
        Me.ProgressTextures.Size = New System.Drawing.Size(278, 24)
        Me.ProgressTextures.TabIndex = 0
        '
        'BtnTextureBCHVer
        '
        Me.BtnTextureBCHVer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureBCHVer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureBCHVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureBCHVer.Location = New System.Drawing.Point(362, 18)
        Me.BtnTextureBCHVer.Name = "BtnTextureBCHVer"
        Me.BtnTextureBCHVer.Size = New System.Drawing.Size(72, 24)
        Me.BtnTextureBCHVer.TabIndex = 6
        Me.BtnTextureBCHVer.Text = "X/Y"
        Me.BtnTextureBCHVer.UseVisualStyleBackColor = True
        '
        'BtnTextureExportAll
        '
        Me.BtnTextureExportAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnTextureExportAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnTextureExportAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTextureExportAll.Location = New System.Drawing.Point(156, 18)
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
        Me.BtnTextureExport.Location = New System.Drawing.Point(78, 18)
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
        Me.GrpTexInfo.Location = New System.Drawing.Point(7, 486)
        Me.GrpTexInfo.Name = "GrpTexInfo"
        Me.GrpTexInfo.Size = New System.Drawing.Size(200, 81)
        Me.GrpTexInfo.TabIndex = 21
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
        'GrpGARCOptions
        '
        Me.GrpGARCOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpGARCOptions.Controls.Add(Me.ProgressGARC)
        Me.GrpGARCOptions.Controls.Add(Me.BtnGARCExtractAll)
        Me.GrpGARCOptions.Controls.Add(Me.BtnGARCExtract)
        Me.GrpGARCOptions.Controls.Add(Me.BtnOpenGARC)
        Me.GrpGARCOptions.ForeColor = System.Drawing.Color.White
        Me.GrpGARCOptions.Location = New System.Drawing.Point(7, 486)
        Me.GrpGARCOptions.Name = "GrpGARCOptions"
        Me.GrpGARCOptions.Size = New System.Drawing.Size(640, 81)
        Me.GrpGARCOptions.TabIndex = 23
        Me.GrpGARCOptions.TabStop = False
        Me.GrpGARCOptions.Text = "Options"
        '
        'ProgressGARC
        '
        Me.ProgressGARC.Location = New System.Drawing.Point(0, 48)
        Me.ProgressGARC.Name = "ProgressGARC"
        Me.ProgressGARC.Percentage = 0.0!
        Me.ProgressGARC.Size = New System.Drawing.Size(228, 24)
        Me.ProgressGARC.TabIndex = 0
        '
        'BtnGARCExtractAll
        '
        Me.BtnGARCExtractAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnGARCExtractAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnGARCExtractAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGARCExtractAll.Location = New System.Drawing.Point(156, 18)
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
        Me.BtnGARCExtract.Location = New System.Drawing.Point(78, 18)
        Me.BtnGARCExtract.Name = "BtnGARCExtract"
        Me.BtnGARCExtract.Size = New System.Drawing.Size(72, 24)
        Me.BtnGARCExtract.TabIndex = 2
        Me.BtnGARCExtract.Text = "Extract"
        Me.BtnGARCExtract.UseVisualStyleBackColor = True
        '
        'BtnOpenGARC
        '
        Me.BtnOpenGARC.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnOpenGARC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnOpenGARC.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnOpenGARC.Location = New System.Drawing.Point(0, 18)
        Me.BtnOpenGARC.Name = "BtnOpenGARC"
        Me.BtnOpenGARC.Size = New System.Drawing.Size(72, 24)
        Me.BtnOpenGARC.TabIndex = 1
        Me.BtnOpenGARC.Text = "Open"
        Me.BtnOpenGARC.UseVisualStyleBackColor = True
        '
        'GrpFiles
        '
        Me.GrpFiles.Controls.Add(Me.LstFiles_Container)
        Me.GrpFiles.ForeColor = System.Drawing.Color.White
        Me.GrpFiles.Location = New System.Drawing.Point(7, 3)
        Me.GrpFiles.Name = "GrpFiles"
        Me.GrpFiles.Size = New System.Drawing.Size(640, 478)
        Me.GrpFiles.TabIndex = 1
        Me.GrpFiles.TabStop = False
        Me.GrpFiles.Text = "Files"
        '
        'LstFiles_Container
        '
        Me.LstFiles_Container.AutoScroll = True
        Me.LstFiles_Container.Controls.Add(Me.LstFiles)
        Me.LstFiles_Container.Location = New System.Drawing.Point(0, 18)
        Me.LstFiles_Container.Name = "LstFiles_Container"
        Me.LstFiles_Container.Size = New System.Drawing.Size(640, 460)
        Me.LstFiles_Container.TabIndex = 0
        '
        'LstFiles
        '
        Me.LstFiles.Location = New System.Drawing.Point(0, 0)
        Me.LstFiles.MinHeight = 250
        Me.LstFiles.Name = "LstFiles"
        Me.LstFiles.Size = New System.Drawing.Size(623, 460)
        Me.LstFiles.TabIndex = 0
        Me.LstFiles.Text = "MyListview1"
        Me.LstFiles.TileHeight = 16
        '
        'GrpMatches
        '
        Me.GrpMatches.Controls.Add(Me.LstMatches_Container)
        Me.GrpMatches.ForeColor = System.Drawing.Color.White
        Me.GrpMatches.Location = New System.Drawing.Point(7, 3)
        Me.GrpMatches.Name = "GrpMatches"
        Me.GrpMatches.Size = New System.Drawing.Size(640, 478)
        Me.GrpMatches.TabIndex = 26
        Me.GrpMatches.TabStop = False
        Me.GrpMatches.Text = "Matches"
        '
        'LstMatches_Container
        '
        Me.LstMatches_Container.AutoScroll = True
        Me.LstMatches_Container.Controls.Add(Me.LstMatches)
        Me.LstMatches_Container.Location = New System.Drawing.Point(0, 18)
        Me.LstMatches_Container.Name = "LstMatches_Container"
        Me.LstMatches_Container.Size = New System.Drawing.Size(640, 460)
        Me.LstMatches_Container.TabIndex = 0
        '
        'LstMatches
        '
        Me.LstMatches.Location = New System.Drawing.Point(0, 0)
        Me.LstMatches.MinHeight = 250
        Me.LstMatches.Name = "LstMatches"
        Me.LstMatches.Size = New System.Drawing.Size(623, 460)
        Me.LstMatches.TabIndex = 2
        Me.LstMatches.Text = "MyListview1"
        Me.LstMatches.TileHeight = 16
        '
        'GrpSearchOptions
        '
        Me.GrpSearchOptions.BackColor = System.Drawing.Color.Transparent
        Me.GrpSearchOptions.Controls.Add(Me.TxtSearch)
        Me.GrpSearchOptions.Controls.Add(Me.ProgressSearch)
        Me.GrpSearchOptions.Controls.Add(Me.BtnSearch)
        Me.GrpSearchOptions.ForeColor = System.Drawing.Color.White
        Me.GrpSearchOptions.Location = New System.Drawing.Point(7, 486)
        Me.GrpSearchOptions.Name = "GrpSearchOptions"
        Me.GrpSearchOptions.Size = New System.Drawing.Size(640, 81)
        Me.GrpSearchOptions.TabIndex = 27
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
        Me.TxtSearch.TabIndex = 0
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
        Me.BtnSearch.TabIndex = 1
        Me.BtnSearch.Text = "Search"
        Me.BtnSearch.UseVisualStyleBackColor = True
        '
        'Title
        '
        Me.Title.AutoSize = True
        Me.Title.BackColor = System.Drawing.Color.Transparent
        Me.Title.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.ForeColor = System.Drawing.Color.White
        Me.Title.Location = New System.Drawing.Point(327, 4)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(146, 25)
        Me.Title.TabIndex = 18
        Me.Title.Text = "OhanaXY Alpha"
        '
        'SideBar
        '
        Me.SideBar.BackColor = System.Drawing.Color.Transparent
        Me.SideBar.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SideBar.ForeColor = System.Drawing.Color.White
        Me.SideBar.Location = New System.Drawing.Point(0, 32)
        Me.SideBar.MinHeight = 568
        Me.SideBar.Name = "SideBar"
        Me.SideBar.Size = New System.Drawing.Size(146, 568)
        Me.SideBar.TabIndex = 0
        Me.SideBar.TileHeight = 32
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.PanelSplash)
        Me.Controls.Add(Me.PanelModel)
        Me.Controls.Add(Me.PanelTextures)
        Me.Controls.Add(Me.PanelGARC)
        Me.Controls.Add(Me.PanelSearch)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.BtnMinimize)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.SideBar)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.Name = "FrmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OhanaXY by gdkchan"
        Me.PanelModel.ResumeLayout(False)
        CType(Me.Screen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelTextures.ResumeLayout(False)
        Me.PanelGARC.ResumeLayout(False)
        Me.PanelSearch.ResumeLayout(False)
        Me.GrpOptions.ResumeLayout(False)
        Me.GrpInfo.ResumeLayout(False)
        Me.GrpInfo.PerformLayout()
        Me.GrpTexturePreview.ResumeLayout(False)
        Me.ImgTexture_Container.ResumeLayout(False)
        Me.ImgTexture_Container.PerformLayout()
        CType(Me.ImgTexture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpTextures.ResumeLayout(False)
        Me.LstTextures_Container.ResumeLayout(False)
        Me.GrpTexOptions.ResumeLayout(False)
        Me.GrpTexInfo.ResumeLayout(False)
        Me.GrpTexInfo.PerformLayout()
        Me.GrpGARCOptions.ResumeLayout(False)
        Me.GrpFiles.ResumeLayout(False)
        Me.LstFiles_Container.ResumeLayout(False)
        Me.GrpMatches.ResumeLayout(False)
        Me.LstMatches_Container.ResumeLayout(False)
        Me.GrpSearchOptions.ResumeLayout(False)
        Me.GrpSearchOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SideBar As Ohana3DS.MyListview
    Friend WithEvents BtnClose As System.Windows.Forms.Label
    Friend WithEvents BtnMinimize As System.Windows.Forms.Label
    Friend WithEvents Title As Ohana3DS.MyWindowTitle
    Friend WithEvents PanelModel As System.Windows.Forms.Panel
    Friend WithEvents GrpOptions As Ohana3DS.MyGroupbox
    Friend WithEvents BtnModelExportAllFF As System.Windows.Forms.Button
    Friend WithEvents BtnModelExport As System.Windows.Forms.Button
    Friend WithEvents BtnModelOpen As System.Windows.Forms.Button
    Friend WithEvents GrpInfo As Ohana3DS.MyGroupbox
    Friend WithEvents LblInfoTextures As System.Windows.Forms.Label
    Friend WithEvents LblInfoBones As System.Windows.Forms.Label
    Friend WithEvents LblInfoTriangles As System.Windows.Forms.Label
    Friend WithEvents LblInfoVertices As System.Windows.Forms.Label
    Friend WithEvents LblInfoTexturesDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoBonesDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoTrianglesDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoVerticesDummy As System.Windows.Forms.Label
    Friend WithEvents Screen As System.Windows.Forms.PictureBox
    Friend WithEvents PanelTextures As System.Windows.Forms.Panel
    Friend WithEvents GrpTexOptions As Ohana3DS.MyGroupbox
    Friend WithEvents BtnTextureExportAll As System.Windows.Forms.Button
    Friend WithEvents BtnTextureExport As System.Windows.Forms.Button
    Friend WithEvents BtnTextureOpen As System.Windows.Forms.Button
    Friend WithEvents GrpTexInfo As Ohana3DS.MyGroupbox
    Friend WithEvents LblInfoTextureIndex As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureIndexDummy As System.Windows.Forms.Label
    Friend WithEvents GrpTextures As Ohana3DS.MyGroupbox
    Friend WithEvents GrpTexturePreview As Ohana3DS.MyGroupbox
    Friend WithEvents LstTextures_Container As System.Windows.Forms.Panel
    Friend WithEvents LstTextures As Ohana3DS.MyListview
    Friend WithEvents LblInfoTextureCDDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureFormatDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureResolutionDummy As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureCD As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureFormat As System.Windows.Forms.Label
    Friend WithEvents LblInfoTextureResolution As System.Windows.Forms.Label
    Friend WithEvents BtnTextureBCHVer As System.Windows.Forms.Button
    Friend WithEvents ProgressTextures As Ohana3DS.MyProgressbar
    Friend WithEvents ImgTexture_Container As System.Windows.Forms.Panel
    Friend WithEvents ImgTexture As System.Windows.Forms.PictureBox
    Friend WithEvents BtnTextureExportAllFF As System.Windows.Forms.Button
    Friend WithEvents BtnTextureMode As System.Windows.Forms.Button
    Friend WithEvents ProgressModels As Ohana3DS.MyProgressbar
    Friend WithEvents BtnModelBCHVer As System.Windows.Forms.Button
    Friend WithEvents BtnModelTexturesMore As System.Windows.Forms.Button
    Friend WithEvents PanelGARC As System.Windows.Forms.Panel
    Friend WithEvents GrpFiles As Ohana3DS.MyGroupbox
    Friend WithEvents LstFiles As Ohana3DS.MyListview
    Friend WithEvents LstFiles_Container As System.Windows.Forms.Panel
    Friend WithEvents GrpGARCOptions As Ohana3DS.MyGroupbox
    Friend WithEvents ProgressGARC As Ohana3DS.MyProgressbar
    Friend WithEvents BtnGARCExtractAll As System.Windows.Forms.Button
    Friend WithEvents BtnGARCExtract As System.Windows.Forms.Button
    Friend WithEvents BtnOpenGARC As System.Windows.Forms.Button
    Friend WithEvents PanelSearch As System.Windows.Forms.Panel
    Friend WithEvents GrpSearchOptions As Ohana3DS.MyGroupbox
    Friend WithEvents TxtSearch As System.Windows.Forms.TextBox
    Friend WithEvents ProgressSearch As Ohana3DS.MyProgressbar
    Friend WithEvents BtnSearch As System.Windows.Forms.Button
    Friend WithEvents GrpMatches As Ohana3DS.MyGroupbox
    Friend WithEvents LstMatches_Container As System.Windows.Forms.Panel
    Friend WithEvents LstMatches As Ohana3DS.MyListview
    Friend WithEvents BtnModelScale As System.Windows.Forms.Button
    Friend WithEvents PanelSplash As System.Windows.Forms.Panel

End Class
