Imports System.ComponentModel
Public Class MyTabcontrol
    Inherits TabControl
    Public Sub New()
        InitializeComponent()

        Me.DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
            ControlStyles.DoubleBuffer Or _
            ControlStyles.ResizeRedraw Or _
            ControlStyles.UserPaint Or _
            ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: O procedimento a seguir é exigido pelo Windows Form Designer
    'Ele pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

    Private BgImage As Image
    <Browsable(True)>
    Public Overrides Property BackgroundImage() As Image
        Get
            Return BgImage
        End Get
        Set(ByVal Value As Image)
            BgImage = Value
            Me.Refresh()
        End Set
    End Property
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        If BgImage IsNot Nothing Then e.Graphics.FillRectangle(New TextureBrush(BgImage), Me.ClientRectangle)

        If TabCount <= 0 Then Return

        For Index As Integer = 0 To TabCount - 1
            Dim Current_Tab As TabPage = Me.TabPages(Index)
            Dim Tab_Rect As Rectangle = Me.GetTabRect(Index)
            e.Graphics.FillRectangle(New SolidBrush(Current_Tab.BackColor), Tab_Rect)
            If Index = SelectedIndex Then e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(15, 82, 186)), Tab_Rect)

            'Abast com rotação de texto
            If Alignment = TabAlignment.Left Or Alignment = TabAlignment.Right Then
                Dim Rotation_Angle As Single = 90
                If Alignment = TabAlignment.Left Then Rotation_Angle = 270
                Dim Point As New PointF(Tab_Rect.Left + (Tab_Rect.Width \ 2), Tab_Rect.Top + (Tab_Rect.Height \ 2))
                e.Graphics.TranslateTransform(Point.X, Point.Y)
                e.Graphics.RotateTransform(Rotation_Angle)
                Tab_Rect = New Rectangle(-(Tab_Rect.Height \ 2), -(Tab_Rect.Width \ 2), Tab_Rect.Height, Tab_Rect.Width)
            End If

            'Desenha nome da aba
            Dim Format As New StringFormat
            Format.Alignment = StringAlignment.Center
            Format.LineAlignment = StringAlignment.Center
            e.Graphics.DrawString(Current_Tab.Text, Me.Font, New SolidBrush(Current_Tab.ForeColor), RectangleF.op_Implicit(Tab_Rect), Format)
            e.Graphics.ResetTransform()
        Next
    End Sub
End Class