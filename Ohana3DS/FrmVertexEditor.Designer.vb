<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmVertexEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmVertexEditor))
        Me.LstObjects = New Ohana3DS.MyListview()
        Me.LstFaces = New Ohana3DS.MyListview()
        Me.BtnImportObj = New System.Windows.Forms.Button()
        Me.BtnExportObj = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.Title = New Ohana3DS.MyWindowTitle()
        Me.BtnMinimize = New System.Windows.Forms.Label()
        Me.BtnClose = New System.Windows.Forms.Label()
        Me.BtnExportFace = New System.Windows.Forms.Button()
        Me.LblTexID = New System.Windows.Forms.Label()
        Me.TxtTexID = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'LstObjects
        '
        Me.LstObjects.BackColor = System.Drawing.Color.Transparent
        Me.LstObjects.ForeColor = System.Drawing.Color.White
        Me.LstObjects.Location = New System.Drawing.Point(12, 32)
        Me.LstObjects.Name = "LstObjects"
        Me.LstObjects.SelectedIndex = -1
        Me.LstObjects.Size = New System.Drawing.Size(250, 406)
        Me.LstObjects.TabIndex = 0
        Me.LstObjects.Text = "MyListview1"
        Me.LstObjects.TileHeight = 16
        '
        'LstFaces
        '
        Me.LstFaces.BackColor = System.Drawing.Color.Transparent
        Me.LstFaces.ForeColor = System.Drawing.Color.White
        Me.LstFaces.Location = New System.Drawing.Point(268, 32)
        Me.LstFaces.Name = "LstFaces"
        Me.LstFaces.SelectedIndex = -1
        Me.LstFaces.Size = New System.Drawing.Size(360, 406)
        Me.LstFaces.TabIndex = 1
        Me.LstFaces.Text = "MyListview2"
        Me.LstFaces.TileHeight = 16
        '
        'BtnImportObj
        '
        Me.BtnImportObj.BackColor = System.Drawing.Color.Transparent
        Me.BtnImportObj.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnImportObj.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnImportObj.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnImportObj.ForeColor = System.Drawing.Color.White
        Me.BtnImportObj.Location = New System.Drawing.Point(200, 444)
        Me.BtnImportObj.Name = "BtnImportObj"
        Me.BtnImportObj.Size = New System.Drawing.Size(88, 24)
        Me.BtnImportObj.TabIndex = 7
        Me.BtnImportObj.Text = "Import .obj..."
        Me.BtnImportObj.UseVisualStyleBackColor = False
        '
        'BtnExportObj
        '
        Me.BtnExportObj.BackColor = System.Drawing.Color.Transparent
        Me.BtnExportObj.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnExportObj.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnExportObj.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnExportObj.ForeColor = System.Drawing.Color.White
        Me.BtnExportObj.Location = New System.Drawing.Point(12, 444)
        Me.BtnExportObj.Name = "BtnExportObj"
        Me.BtnExportObj.Size = New System.Drawing.Size(88, 24)
        Me.BtnExportObj.TabIndex = 8
        Me.BtnExportObj.Text = "Export .obj..."
        Me.BtnExportObj.UseVisualStyleBackColor = False
        '
        'BtnClear
        '
        Me.BtnClear.BackColor = System.Drawing.Color.Transparent
        Me.BtnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnClear.ForeColor = System.Drawing.Color.White
        Me.BtnClear.Location = New System.Drawing.Point(540, 444)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(88, 24)
        Me.BtnClear.TabIndex = 9
        Me.BtnClear.Text = "Clear face"
        Me.BtnClear.UseVisualStyleBackColor = False
        '
        'Title
        '
        Me.Title.AutoSize = True
        Me.Title.BackColor = System.Drawing.Color.Transparent
        Me.Title.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.ForeColor = System.Drawing.Color.White
        Me.Title.Location = New System.Drawing.Point(238, 0)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(165, 25)
        Me.Title.TabIndex = 24
        Me.Title.Text = "OhanaXY - Vertex"
        '
        'BtnMinimize
        '
        Me.BtnMinimize.BackColor = System.Drawing.Color.Transparent
        Me.BtnMinimize.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMinimize.ForeColor = System.Drawing.Color.White
        Me.BtnMinimize.Location = New System.Drawing.Point(564, 4)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(32, 24)
        Me.BtnMinimize.TabIndex = 23
        Me.BtnMinimize.Text = "_"
        Me.BtnMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.Color.Transparent
        Me.BtnClose.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.Color.White
        Me.BtnClose.Location = New System.Drawing.Point(596, 4)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(32, 24)
        Me.BtnClose.TabIndex = 22
        Me.BtnClose.Text = "X"
        Me.BtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnExportFace
        '
        Me.BtnExportFace.BackColor = System.Drawing.Color.Transparent
        Me.BtnExportFace.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnExportFace.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.BtnExportFace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnExportFace.ForeColor = System.Drawing.Color.White
        Me.BtnExportFace.Location = New System.Drawing.Point(106, 444)
        Me.BtnExportFace.Name = "BtnExportFace"
        Me.BtnExportFace.Size = New System.Drawing.Size(88, 24)
        Me.BtnExportFace.TabIndex = 25
        Me.BtnExportFace.Text = "Export face..."
        Me.BtnExportFace.UseVisualStyleBackColor = False
        '
        'LblTexID
        '
        Me.LblTexID.AutoSize = True
        Me.LblTexID.BackColor = System.Drawing.Color.Transparent
        Me.LblTexID.ForeColor = System.Drawing.Color.White
        Me.LblTexID.Location = New System.Drawing.Point(428, 450)
        Me.LblTexID.Name = "LblTexID"
        Me.LblTexID.Size = New System.Drawing.Size(60, 13)
        Me.LblTexID.TabIndex = 26
        Me.LblTexID.Text = "Texture ID:"
        '
        'TxtTexID
        '
        Me.TxtTexID.BackColor = System.Drawing.Color.Black
        Me.TxtTexID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtTexID.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.TxtTexID.ForeColor = System.Drawing.Color.White
        Me.TxtTexID.Location = New System.Drawing.Point(494, 444)
        Me.TxtTexID.Name = "TxtTexID"
        Me.TxtTexID.Size = New System.Drawing.Size(40, 24)
        Me.TxtTexID.TabIndex = 27
        '
        'FrmVertexEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(640, 480)
        Me.Controls.Add(Me.TxtTexID)
        Me.Controls.Add(Me.LblTexID)
        Me.Controls.Add(Me.BtnExportFace)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.BtnMinimize)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnClear)
        Me.Controls.Add(Me.BtnExportObj)
        Me.Controls.Add(Me.BtnImportObj)
        Me.Controls.Add(Me.LstFaces)
        Me.Controls.Add(Me.LstObjects)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmVertexEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmVertexEditor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LstObjects As Ohana3DS.MyListview
    Friend WithEvents LstFaces As Ohana3DS.MyListview
    Friend WithEvents BtnImportObj As System.Windows.Forms.Button
    Friend WithEvents BtnExportObj As System.Windows.Forms.Button
    Friend WithEvents BtnClear As System.Windows.Forms.Button
    Friend WithEvents Title As Ohana3DS.MyWindowTitle
    Friend WithEvents BtnMinimize As System.Windows.Forms.Label
    Friend WithEvents BtnClose As System.Windows.Forms.Label
    Friend WithEvents BtnExportFace As System.Windows.Forms.Button
    Friend WithEvents LblTexID As System.Windows.Forms.Label
    Friend WithEvents TxtTexID As System.Windows.Forms.TextBox
End Class
