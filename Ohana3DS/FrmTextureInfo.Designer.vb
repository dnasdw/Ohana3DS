<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTextureInfo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmTextureInfo))
        Me.LstBonesInfo_Container = New System.Windows.Forms.Panel()
        Me.LstModelTextures = New Ohana3DS.MyListview()
        Me.Title = New Ohana3DS.MyWindowTitle()
        Me.BtnMinimize = New System.Windows.Forms.Label()
        Me.BtnClose = New System.Windows.Forms.Label()
        Me.LstBonesInfo_Container.SuspendLayout()
        Me.SuspendLayout()
        '
        'LstBonesInfo_Container
        '
        Me.LstBonesInfo_Container.AutoScroll = True
        Me.LstBonesInfo_Container.BackColor = System.Drawing.Color.Transparent
        Me.LstBonesInfo_Container.Controls.Add(Me.LstModelTextures)
        Me.LstBonesInfo_Container.Location = New System.Drawing.Point(0, 32)
        Me.LstBonesInfo_Container.Name = "LstBonesInfo_Container"
        Me.LstBonesInfo_Container.Size = New System.Drawing.Size(640, 448)
        Me.LstBonesInfo_Container.TabIndex = 1
        '
        'LstModelTextures
        '
        Me.LstModelTextures.BackColor = System.Drawing.Color.Transparent
        Me.LstModelTextures.ForeColor = System.Drawing.Color.White
        Me.LstModelTextures.Location = New System.Drawing.Point(0, 0)
        Me.LstModelTextures.MinHeight = 448
        Me.LstModelTextures.Name = "LstModelTextures"
        Me.LstModelTextures.Size = New System.Drawing.Size(623, 448)
        Me.LstModelTextures.TabIndex = 1
        Me.LstModelTextures.TileHeight = 16
        '
        'Title
        '
        Me.Title.AutoSize = True
        Me.Title.BackColor = System.Drawing.Color.Transparent
        Me.Title.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.ForeColor = System.Drawing.Color.White
        Me.Title.Location = New System.Drawing.Point(229, 0)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(182, 25)
        Me.Title.TabIndex = 21
        Me.Title.Text = "OhanaXY - Textures"
        '
        'BtnMinimize
        '
        Me.BtnMinimize.BackColor = System.Drawing.Color.Transparent
        Me.BtnMinimize.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMinimize.ForeColor = System.Drawing.Color.White
        Me.BtnMinimize.Location = New System.Drawing.Point(564, 4)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(32, 24)
        Me.BtnMinimize.TabIndex = 20
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
        Me.BtnClose.TabIndex = 19
        Me.BtnClose.Text = "X"
        Me.BtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FrmTextureInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(640, 480)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.BtnMinimize)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.LstBonesInfo_Container)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.Name = "FrmTextureInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmBonesInfo"
        Me.LstBonesInfo_Container.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LstBonesInfo_Container As System.Windows.Forms.Panel
    Friend WithEvents LstModelTextures As Ohana3DS.MyListview
    Friend WithEvents Title As Ohana3DS.MyWindowTitle
    Friend WithEvents BtnMinimize As System.Windows.Forms.Label
    Friend WithEvents BtnClose As System.Windows.Forms.Label
End Class
