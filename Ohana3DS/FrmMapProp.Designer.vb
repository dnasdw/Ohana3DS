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
        Me.mapPicBox = New System.Windows.Forms.PictureBox()
        Me.mapPropSave = New System.Windows.Forms.Button()
        Me.LblMapProp = New System.Windows.Forms.Label()
        Me.mapCoords = New System.Windows.Forms.Label()
        Me.Title = New Ohana3DS.MyWindowTitle()
        Me.mapPropCom = New System.Windows.Forms.ComboBox()
        Me.mapPropSet = New System.Windows.Forms.Button()
        CType(Me.mapPicBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.Color.Transparent
        Me.BtnClose.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.Color.White
        Me.BtnClose.Location = New System.Drawing.Point(300, 4)
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
        Me.BtnMinimize.Location = New System.Drawing.Point(268, 4)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(32, 24)
        Me.BtnMinimize.TabIndex = 21
        Me.BtnMinimize.Text = "_"
        Me.BtnMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'mapPicBox
        '
        Me.mapPicBox.BackColor = System.Drawing.Color.Transparent
        Me.mapPicBox.Location = New System.Drawing.Point(12, 57)
        Me.mapPicBox.Name = "mapPicBox"
        Me.mapPicBox.Size = New System.Drawing.Size(320, 320)
        Me.mapPicBox.TabIndex = 23
        Me.mapPicBox.TabStop = False
        '
        'mapPropSave
        '
        Me.mapPropSave.BackColor = System.Drawing.Color.Transparent
        Me.mapPropSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.mapPropSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.mapPropSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mapPropSave.ForeColor = System.Drawing.Color.White
        Me.mapPropSave.Location = New System.Drawing.Point(276, 385)
        Me.mapPropSave.Name = "mapPropSave"
        Me.mapPropSave.Size = New System.Drawing.Size(56, 23)
        Me.mapPropSave.TabIndex = 25
        Me.mapPropSave.Text = "Save"
        Me.mapPropSave.UseVisualStyleBackColor = False
        '
        'LblMapProp
        '
        Me.LblMapProp.AutoSize = True
        Me.LblMapProp.BackColor = System.Drawing.Color.Transparent
        Me.LblMapProp.ForeColor = System.Drawing.Color.White
        Me.LblMapProp.Location = New System.Drawing.Point(9, 390)
        Me.LblMapProp.Name = "LblMapProp"
        Me.LblMapProp.Size = New System.Drawing.Size(57, 13)
        Me.LblMapProp.TabIndex = 26
        Me.LblMapProp.Text = "Properties:"
        '
        'mapCoords
        '
        Me.mapCoords.AutoSize = True
        Me.mapCoords.BackColor = System.Drawing.Color.Transparent
        Me.mapCoords.ForeColor = System.Drawing.Color.White
        Me.mapCoords.Location = New System.Drawing.Point(12, 41)
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
        Me.Title.Location = New System.Drawing.Point(14, 4)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(240, 25)
        Me.Title.TabIndex = 22
        Me.Title.Text = "OhanaXY - Map Properties"
        '
        'mapPropCom
        '
        Me.mapPropCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mapPropCom.FormattingEnabled = True
        Me.mapPropCom.Location = New System.Drawing.Point(72, 387)
        Me.mapPropCom.Name = "mapPropCom"
        Me.mapPropCom.Size = New System.Drawing.Size(132, 21)
        Me.mapPropCom.TabIndex = 28
        '
        'mapPropSet
        '
        Me.mapPropSet.BackColor = System.Drawing.Color.Transparent
        Me.mapPropSet.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.mapPropSet.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.mapPropSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mapPropSet.ForeColor = System.Drawing.Color.White
        Me.mapPropSet.Location = New System.Drawing.Point(210, 385)
        Me.mapPropSet.Name = "mapPropSet"
        Me.mapPropSet.Size = New System.Drawing.Size(56, 23)
        Me.mapPropSet.TabIndex = 29
        Me.mapPropSet.Text = "Set"
        Me.mapPropSet.UseVisualStyleBackColor = False
        '
        'FrmMapProp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(344, 419)
        Me.Controls.Add(Me.mapPropSet)
        Me.Controls.Add(Me.mapPropCom)
        Me.Controls.Add(Me.mapCoords)
        Me.Controls.Add(Me.LblMapProp)
        Me.Controls.Add(Me.mapPropSave)
        Me.Controls.Add(Me.mapPicBox)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.BtnMinimize)
        Me.Controls.Add(Me.BtnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmMapProp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmMapProp"
        CType(Me.mapPicBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnClose As System.Windows.Forms.Label
    Friend WithEvents BtnMinimize As System.Windows.Forms.Label
    Friend WithEvents Title As Ohana3DS.MyWindowTitle
    Friend WithEvents mapPicBox As System.Windows.Forms.PictureBox
    Friend WithEvents mapPropSave As System.Windows.Forms.Button
    Friend WithEvents LblMapProp As System.Windows.Forms.Label
    Friend WithEvents mapCoords As System.Windows.Forms.Label
    Friend WithEvents mapPropCom As System.Windows.Forms.ComboBox
    Friend WithEvents mapPropSet As System.Windows.Forms.Button
End Class
