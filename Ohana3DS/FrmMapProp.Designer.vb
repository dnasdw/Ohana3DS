<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMapProp
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMapProp))
        Me.BtnClose = New System.Windows.Forms.Label()
        Me.BtnMinimize = New System.Windows.Forms.Label()
        Me.mapCoords = New System.Windows.Forms.Label()
        Me.Title = New Ohana3DS.MyWindowTitle()
        Me.MapTabs = New Ohana3DS.MyTabcontrol()
        Me.TabProperties = New System.Windows.Forms.TabPage()
        Me.mapPropSet = New System.Windows.Forms.Button()
        Me.mapPropCom = New System.Windows.Forms.ComboBox()
        Me.LblMapProp = New System.Windows.Forms.Label()
        Me.mapPropSave = New System.Windows.Forms.Button()
        Me.mapPicBox = New System.Windows.Forms.PictureBox()
        Me.TabObjects = New System.Windows.Forms.TabPage()
        Me.GrpTranslation = New Ohana3DS.MyGroupbox()
        Me.TxtTZ = New System.Windows.Forms.TextBox()
        Me.LblTZDummy = New System.Windows.Forms.Label()
        Me.TxtTY = New System.Windows.Forms.TextBox()
        Me.TxtTX = New System.Windows.Forms.TextBox()
        Me.LblTYDummy = New System.Windows.Forms.Label()
        Me.LblTXDummy = New System.Windows.Forms.Label()
        Me.GrpRotation = New Ohana3DS.MyGroupbox()
        Me.TxtRZ = New System.Windows.Forms.TextBox()
        Me.LblRZDummy = New System.Windows.Forms.Label()
        Me.TxtRY = New System.Windows.Forms.TextBox()
        Me.TxtRX = New System.Windows.Forms.TextBox()
        Me.LblRYDummy = New System.Windows.Forms.Label()
        Me.LblRXDummy = New System.Windows.Forms.Label()
        Me.GrpScale = New Ohana3DS.MyGroupbox()
        Me.TxtSZ = New System.Windows.Forms.TextBox()
        Me.LblSZ = New System.Windows.Forms.Label()
        Me.TxtSY = New System.Windows.Forms.TextBox()
        Me.TxtSX = New System.Windows.Forms.TextBox()
        Me.LblSY = New System.Windows.Forms.Label()
        Me.LblSXDummy = New System.Windows.Forms.Label()
        Me.LstObjects = New Ohana3DS.MyListview()
        Me.MapTabs.SuspendLayout()
        Me.TabProperties.SuspendLayout()
        CType(Me.mapPicBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabObjects.SuspendLayout()
        Me.GrpTranslation.SuspendLayout()
        Me.GrpRotation.SuspendLayout()
        Me.GrpScale.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.Color.Transparent
        Me.BtnClose.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.Color.White
        Me.BtnClose.Location = New System.Drawing.Point(320, 4)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(32, 24)
        Me.BtnClose.TabIndex = 20
        Me.BtnClose.Text = "X"
        Me.BtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnMinimize
        '
        Me.BtnMinimize.BackColor = System.Drawing.Color.Transparent
        Me.BtnMinimize.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMinimize.ForeColor = System.Drawing.Color.White
        Me.BtnMinimize.Location = New System.Drawing.Point(288, 4)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(32, 24)
        Me.BtnMinimize.TabIndex = 21
        Me.BtnMinimize.Text = "_"
        Me.BtnMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'mapCoords
        '
        Me.mapCoords.AutoSize = True
        Me.mapCoords.BackColor = System.Drawing.Color.Transparent
        Me.mapCoords.ForeColor = System.Drawing.Color.White
        Me.mapCoords.Location = New System.Drawing.Point(9, 41)
        Me.mapCoords.Name = "mapCoords"
        Me.mapCoords.Size = New System.Drawing.Size(0, 13)
        Me.mapCoords.TabIndex = 27
        '
        'Title
        '
        Me.Title.AutoSize = True
        Me.Title.BackColor = System.Drawing.Color.Transparent
        Me.Title.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.ForeColor = System.Drawing.Color.White
        Me.Title.Location = New System.Drawing.Point(110, 4)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(143, 25)
        Me.Title.TabIndex = 22
        Me.Title.Text = "Map Properties"
        '
        'MapTabs
        '
        Me.MapTabs.BackgroundImage = CType(resources.GetObject("MapTabs.BackgroundImage"), System.Drawing.Image)
        Me.MapTabs.Controls.Add(Me.TabProperties)
        Me.MapTabs.Controls.Add(Me.TabObjects)
        Me.MapTabs.Location = New System.Drawing.Point(12, 32)
        Me.MapTabs.Name = "MapTabs"
        Me.MapTabs.SelectedIndex = 0
        Me.MapTabs.Size = New System.Drawing.Size(340, 398)
        Me.MapTabs.TabIndex = 30
        '
        'TabProperties
        '
        Me.TabProperties.BackColor = System.Drawing.Color.Transparent
        Me.TabProperties.Controls.Add(Me.mapPropSet)
        Me.TabProperties.Controls.Add(Me.mapPropCom)
        Me.TabProperties.Controls.Add(Me.LblMapProp)
        Me.TabProperties.Controls.Add(Me.mapPropSave)
        Me.TabProperties.Controls.Add(Me.mapPicBox)
        Me.TabProperties.ForeColor = System.Drawing.Color.White
        Me.TabProperties.Location = New System.Drawing.Point(4, 28)
        Me.TabProperties.Name = "TabProperties"
        Me.TabProperties.Padding = New System.Windows.Forms.Padding(3)
        Me.TabProperties.Size = New System.Drawing.Size(332, 366)
        Me.TabProperties.TabIndex = 0
        Me.TabProperties.Text = "Properties"
        '
        'mapPropSet
        '
        Me.mapPropSet.BackColor = System.Drawing.Color.Transparent
        Me.mapPropSet.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.mapPropSet.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.mapPropSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mapPropSet.ForeColor = System.Drawing.Color.White
        Me.mapPropSet.Location = New System.Drawing.Point(204, 334)
        Me.mapPropSet.Name = "mapPropSet"
        Me.mapPropSet.Size = New System.Drawing.Size(56, 23)
        Me.mapPropSet.TabIndex = 34
        Me.mapPropSet.Text = "Edit"
        Me.mapPropSet.UseVisualStyleBackColor = False
        '
        'mapPropCom
        '
        Me.mapPropCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mapPropCom.FormattingEnabled = True
        Me.mapPropCom.Location = New System.Drawing.Point(66, 336)
        Me.mapPropCom.Name = "mapPropCom"
        Me.mapPropCom.Size = New System.Drawing.Size(132, 21)
        Me.mapPropCom.TabIndex = 33
        '
        'LblMapProp
        '
        Me.LblMapProp.AutoSize = True
        Me.LblMapProp.BackColor = System.Drawing.Color.Transparent
        Me.LblMapProp.ForeColor = System.Drawing.Color.White
        Me.LblMapProp.Location = New System.Drawing.Point(3, 339)
        Me.LblMapProp.Name = "LblMapProp"
        Me.LblMapProp.Size = New System.Drawing.Size(57, 13)
        Me.LblMapProp.TabIndex = 32
        Me.LblMapProp.Text = "Properties:"
        '
        'mapPropSave
        '
        Me.mapPropSave.BackColor = System.Drawing.Color.Transparent
        Me.mapPropSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.mapPropSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.mapPropSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mapPropSave.ForeColor = System.Drawing.Color.White
        Me.mapPropSave.Location = New System.Drawing.Point(270, 334)
        Me.mapPropSave.Name = "mapPropSave"
        Me.mapPropSave.Size = New System.Drawing.Size(56, 23)
        Me.mapPropSave.TabIndex = 31
        Me.mapPropSave.Text = "Save"
        Me.mapPropSave.UseVisualStyleBackColor = False
        '
        'mapPicBox
        '
        Me.mapPicBox.BackColor = System.Drawing.Color.Transparent
        Me.mapPicBox.Location = New System.Drawing.Point(6, 6)
        Me.mapPicBox.Name = "mapPicBox"
        Me.mapPicBox.Size = New System.Drawing.Size(320, 320)
        Me.mapPicBox.TabIndex = 30
        Me.mapPicBox.TabStop = False
        '
        'TabObjects
        '
        Me.TabObjects.BackColor = System.Drawing.Color.Transparent
        Me.TabObjects.Controls.Add(Me.GrpTranslation)
        Me.TabObjects.Controls.Add(Me.GrpRotation)
        Me.TabObjects.Controls.Add(Me.GrpScale)
        Me.TabObjects.Controls.Add(Me.LstObjects)
        Me.TabObjects.ForeColor = System.Drawing.Color.White
        Me.TabObjects.Location = New System.Drawing.Point(4, 28)
        Me.TabObjects.Name = "TabObjects"
        Me.TabObjects.Padding = New System.Windows.Forms.Padding(3)
        Me.TabObjects.Size = New System.Drawing.Size(332, 366)
        Me.TabObjects.TabIndex = 1
        Me.TabObjects.Text = "Objects"
        '
        'GrpTranslation
        '
        Me.GrpTranslation.Controls.Add(Me.TxtTZ)
        Me.GrpTranslation.Controls.Add(Me.LblTZDummy)
        Me.GrpTranslation.Controls.Add(Me.TxtTY)
        Me.GrpTranslation.Controls.Add(Me.TxtTX)
        Me.GrpTranslation.Controls.Add(Me.LblTYDummy)
        Me.GrpTranslation.Controls.Add(Me.LblTXDummy)
        Me.GrpTranslation.Location = New System.Drawing.Point(209, 232)
        Me.GrpTranslation.Name = "GrpTranslation"
        Me.GrpTranslation.Size = New System.Drawing.Size(120, 110)
        Me.GrpTranslation.TabIndex = 6
        Me.GrpTranslation.TabStop = False
        Me.GrpTranslation.Text = "Translation"
        '
        'TxtTZ
        '
        Me.TxtTZ.BackColor = System.Drawing.Color.Black
        Me.TxtTZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtTZ.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtTZ.ForeColor = System.Drawing.Color.White
        Me.TxtTZ.Location = New System.Drawing.Point(28, 79)
        Me.TxtTZ.Name = "TxtTZ"
        Me.TxtTZ.Size = New System.Drawing.Size(89, 24)
        Me.TxtTZ.TabIndex = 5
        '
        'LblTZDummy
        '
        Me.LblTZDummy.AutoSize = True
        Me.LblTZDummy.Location = New System.Drawing.Point(-3, 84)
        Me.LblTZDummy.Name = "LblTZDummy"
        Me.LblTZDummy.Size = New System.Drawing.Size(17, 13)
        Me.LblTZDummy.TabIndex = 4
        Me.LblTZDummy.Text = "Z:"
        '
        'TxtTY
        '
        Me.TxtTY.BackColor = System.Drawing.Color.Black
        Me.TxtTY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtTY.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtTY.ForeColor = System.Drawing.Color.White
        Me.TxtTY.Location = New System.Drawing.Point(28, 49)
        Me.TxtTY.Name = "TxtTY"
        Me.TxtTY.Size = New System.Drawing.Size(89, 24)
        Me.TxtTY.TabIndex = 3
        '
        'TxtTX
        '
        Me.TxtTX.BackColor = System.Drawing.Color.Black
        Me.TxtTX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtTX.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtTX.ForeColor = System.Drawing.Color.White
        Me.TxtTX.Location = New System.Drawing.Point(28, 19)
        Me.TxtTX.Name = "TxtTX"
        Me.TxtTX.Size = New System.Drawing.Size(89, 24)
        Me.TxtTX.TabIndex = 2
        '
        'LblTYDummy
        '
        Me.LblTYDummy.AutoSize = True
        Me.LblTYDummy.Location = New System.Drawing.Point(-3, 54)
        Me.LblTYDummy.Name = "LblTYDummy"
        Me.LblTYDummy.Size = New System.Drawing.Size(17, 13)
        Me.LblTYDummy.TabIndex = 1
        Me.LblTYDummy.Text = "Y:"
        '
        'LblTXDummy
        '
        Me.LblTXDummy.AutoSize = True
        Me.LblTXDummy.Location = New System.Drawing.Point(-3, 24)
        Me.LblTXDummy.Name = "LblTXDummy"
        Me.LblTXDummy.Size = New System.Drawing.Size(17, 13)
        Me.LblTXDummy.TabIndex = 0
        Me.LblTXDummy.Text = "X:"
        '
        'GrpRotation
        '
        Me.GrpRotation.Controls.Add(Me.TxtRZ)
        Me.GrpRotation.Controls.Add(Me.LblRZDummy)
        Me.GrpRotation.Controls.Add(Me.TxtRY)
        Me.GrpRotation.Controls.Add(Me.TxtRX)
        Me.GrpRotation.Controls.Add(Me.LblRYDummy)
        Me.GrpRotation.Controls.Add(Me.LblRXDummy)
        Me.GrpRotation.Location = New System.Drawing.Point(206, 116)
        Me.GrpRotation.Name = "GrpRotation"
        Me.GrpRotation.Size = New System.Drawing.Size(120, 110)
        Me.GrpRotation.TabIndex = 6
        Me.GrpRotation.TabStop = False
        Me.GrpRotation.Text = "Rotation"
        '
        'TxtRZ
        '
        Me.TxtRZ.BackColor = System.Drawing.Color.Black
        Me.TxtRZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtRZ.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtRZ.ForeColor = System.Drawing.Color.White
        Me.TxtRZ.Location = New System.Drawing.Point(31, 78)
        Me.TxtRZ.Name = "TxtRZ"
        Me.TxtRZ.Size = New System.Drawing.Size(89, 24)
        Me.TxtRZ.TabIndex = 5
        '
        'LblRZDummy
        '
        Me.LblRZDummy.AutoSize = True
        Me.LblRZDummy.Location = New System.Drawing.Point(0, 83)
        Me.LblRZDummy.Name = "LblRZDummy"
        Me.LblRZDummy.Size = New System.Drawing.Size(17, 13)
        Me.LblRZDummy.TabIndex = 4
        Me.LblRZDummy.Text = "Z:"
        '
        'TxtRY
        '
        Me.TxtRY.BackColor = System.Drawing.Color.Black
        Me.TxtRY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtRY.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtRY.ForeColor = System.Drawing.Color.White
        Me.TxtRY.Location = New System.Drawing.Point(31, 49)
        Me.TxtRY.Name = "TxtRY"
        Me.TxtRY.Size = New System.Drawing.Size(89, 24)
        Me.TxtRY.TabIndex = 3
        '
        'TxtRX
        '
        Me.TxtRX.BackColor = System.Drawing.Color.Black
        Me.TxtRX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtRX.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtRX.ForeColor = System.Drawing.Color.White
        Me.TxtRX.Location = New System.Drawing.Point(31, 19)
        Me.TxtRX.Name = "TxtRX"
        Me.TxtRX.Size = New System.Drawing.Size(89, 24)
        Me.TxtRX.TabIndex = 2
        '
        'LblRYDummy
        '
        Me.LblRYDummy.AutoSize = True
        Me.LblRYDummy.Location = New System.Drawing.Point(0, 54)
        Me.LblRYDummy.Name = "LblRYDummy"
        Me.LblRYDummy.Size = New System.Drawing.Size(17, 13)
        Me.LblRYDummy.TabIndex = 1
        Me.LblRYDummy.Text = "Y:"
        '
        'LblRXDummy
        '
        Me.LblRXDummy.AutoSize = True
        Me.LblRXDummy.Location = New System.Drawing.Point(0, 24)
        Me.LblRXDummy.Name = "LblRXDummy"
        Me.LblRXDummy.Size = New System.Drawing.Size(17, 13)
        Me.LblRXDummy.TabIndex = 0
        Me.LblRXDummy.Text = "X:"
        '
        'GrpScale
        '
        Me.GrpScale.Controls.Add(Me.TxtSZ)
        Me.GrpScale.Controls.Add(Me.LblSZ)
        Me.GrpScale.Controls.Add(Me.TxtSY)
        Me.GrpScale.Controls.Add(Me.TxtSX)
        Me.GrpScale.Controls.Add(Me.LblSY)
        Me.GrpScale.Controls.Add(Me.LblSXDummy)
        Me.GrpScale.Location = New System.Drawing.Point(206, 0)
        Me.GrpScale.Name = "GrpScale"
        Me.GrpScale.Size = New System.Drawing.Size(120, 110)
        Me.GrpScale.TabIndex = 1
        Me.GrpScale.TabStop = False
        Me.GrpScale.Text = "Scale"
        '
        'TxtSZ
        '
        Me.TxtSZ.BackColor = System.Drawing.Color.Black
        Me.TxtSZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtSZ.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtSZ.ForeColor = System.Drawing.Color.White
        Me.TxtSZ.Location = New System.Drawing.Point(31, 79)
        Me.TxtSZ.Name = "TxtSZ"
        Me.TxtSZ.Size = New System.Drawing.Size(89, 24)
        Me.TxtSZ.TabIndex = 5
        '
        'LblSZ
        '
        Me.LblSZ.AutoSize = True
        Me.LblSZ.Location = New System.Drawing.Point(0, 84)
        Me.LblSZ.Name = "LblSZ"
        Me.LblSZ.Size = New System.Drawing.Size(17, 13)
        Me.LblSZ.TabIndex = 4
        Me.LblSZ.Text = "Z:"
        '
        'TxtSY
        '
        Me.TxtSY.BackColor = System.Drawing.Color.Black
        Me.TxtSY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtSY.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtSY.ForeColor = System.Drawing.Color.White
        Me.TxtSY.Location = New System.Drawing.Point(31, 49)
        Me.TxtSY.Name = "TxtSY"
        Me.TxtSY.Size = New System.Drawing.Size(89, 24)
        Me.TxtSY.TabIndex = 3
        '
        'TxtSX
        '
        Me.TxtSX.BackColor = System.Drawing.Color.Black
        Me.TxtSX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtSX.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtSX.ForeColor = System.Drawing.Color.White
        Me.TxtSX.Location = New System.Drawing.Point(31, 19)
        Me.TxtSX.Name = "TxtSX"
        Me.TxtSX.Size = New System.Drawing.Size(89, 24)
        Me.TxtSX.TabIndex = 2
        '
        'LblSY
        '
        Me.LblSY.AutoSize = True
        Me.LblSY.Location = New System.Drawing.Point(0, 54)
        Me.LblSY.Name = "LblSY"
        Me.LblSY.Size = New System.Drawing.Size(17, 13)
        Me.LblSY.TabIndex = 1
        Me.LblSY.Text = "Y:"
        '
        'LblSXDummy
        '
        Me.LblSXDummy.AutoSize = True
        Me.LblSXDummy.Location = New System.Drawing.Point(0, 24)
        Me.LblSXDummy.Name = "LblSXDummy"
        Me.LblSXDummy.Size = New System.Drawing.Size(17, 13)
        Me.LblSXDummy.TabIndex = 0
        Me.LblSXDummy.Text = "X:"
        '
        'LstObjects
        '
        Me.LstObjects.Location = New System.Drawing.Point(6, 6)
        Me.LstObjects.Name = "LstObjects"
        Me.LstObjects.SelectedIndex = -1
        Me.LstObjects.Size = New System.Drawing.Size(194, 354)
        Me.LstObjects.TabIndex = 0
        Me.LstObjects.TileHeight = 32
        '
        'FrmMapProp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(364, 442)
        Me.Controls.Add(Me.MapTabs)
        Me.Controls.Add(Me.mapCoords)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.BtnMinimize)
        Me.Controls.Add(Me.BtnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmMapProp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Map Properties"
        Me.TopMost = True
        Me.MapTabs.ResumeLayout(False)
        Me.TabProperties.ResumeLayout(False)
        Me.TabProperties.PerformLayout()
        CType(Me.mapPicBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabObjects.ResumeLayout(False)
        Me.GrpTranslation.ResumeLayout(False)
        Me.GrpTranslation.PerformLayout()
        Me.GrpRotation.ResumeLayout(False)
        Me.GrpRotation.PerformLayout()
        Me.GrpScale.ResumeLayout(False)
        Me.GrpScale.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnClose As System.Windows.Forms.Label
    Friend WithEvents BtnMinimize As System.Windows.Forms.Label
    Friend WithEvents Title As Ohana3DS.MyWindowTitle
    Friend WithEvents mapCoords As System.Windows.Forms.Label
    Friend WithEvents MapTabs As Ohana3DS.MyTabcontrol
    Friend WithEvents TabProperties As System.Windows.Forms.TabPage
    Friend WithEvents mapPropSet As System.Windows.Forms.Button
    Friend WithEvents mapPropCom As System.Windows.Forms.ComboBox
    Friend WithEvents LblMapProp As System.Windows.Forms.Label
    Friend WithEvents mapPropSave As System.Windows.Forms.Button
    Friend WithEvents mapPicBox As System.Windows.Forms.PictureBox
    Friend WithEvents TabObjects As System.Windows.Forms.TabPage
    Friend WithEvents GrpScale As Ohana3DS.MyGroupbox
    Friend WithEvents LblSXDummy As System.Windows.Forms.Label
    Friend WithEvents LstObjects As Ohana3DS.MyListview
    Friend WithEvents TxtSX As System.Windows.Forms.TextBox
    Friend WithEvents TxtSZ As System.Windows.Forms.TextBox
    Friend WithEvents LblSZ As System.Windows.Forms.Label
    Friend WithEvents TxtSY As System.Windows.Forms.TextBox
    Friend WithEvents LblSY As System.Windows.Forms.Label
    Friend WithEvents GrpTranslation As Ohana3DS.MyGroupbox
    Friend WithEvents TxtTZ As System.Windows.Forms.TextBox
    Friend WithEvents LblTZDummy As System.Windows.Forms.Label
    Friend WithEvents TxtTY As System.Windows.Forms.TextBox
    Friend WithEvents TxtTX As System.Windows.Forms.TextBox
    Friend WithEvents LblTXDummy As System.Windows.Forms.Label
    Friend WithEvents GrpRotation As Ohana3DS.MyGroupbox
    Friend WithEvents TxtRZ As System.Windows.Forms.TextBox
    Friend WithEvents LblRZDummy As System.Windows.Forms.Label
    Friend WithEvents TxtRY As System.Windows.Forms.TextBox
    Friend WithEvents TxtRX As System.Windows.Forms.TextBox
    Friend WithEvents LblRYDummy As System.Windows.Forms.Label
    Friend WithEvents LblRXDummy As System.Windows.Forms.Label
    Friend WithEvents LblTYDummy As System.Windows.Forms.Label
End Class
