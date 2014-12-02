Public Class MyProgressbar
    Inherits Control

    Private ProgressVal As Single
    Public Sub New()
        Me.DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
            ControlStyles.DoubleBuffer Or _
            ControlStyles.ResizeRedraw Or _
            ControlStyles.UserPaint Or _
            ControlStyles.OptimizedDoubleBuffer Or _
            ControlStyles.SupportsTransparentBackColor, True)
    End Sub
    Public Property Percentage() As Single
        Get
            Return ProgressVal
        End Get
        Set(Value As Single)
            ProgressVal = Value
            Me.Refresh()
        End Set
    End Property
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.ClipRectangle)
        e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(15, 82, 186)), New Rectangle(0, 0, Convert.ToInt32((ProgressVal / 100) * Me.Width), Me.Height))

        Dim TextW As Integer = Convert.ToInt32(e.Graphics.MeasureString(Me.Text, Me.Font).Width)
        Dim TextH As Integer = Convert.ToInt32(e.Graphics.MeasureString(Me.Text, Me.Font).Height)
        e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), New Point((Me.Width \ 2) - (TextW \ 2), (Me.Height \ 2) - (TextH \ 2)))

        MyBase.OnPaint(e)
    End Sub
End Class
