Public Class MySliderLabel
    Inherits Control

    Private WithEvents Timer As New Timer

    Private Scroll_X As Integer
    Private Scrolling_Needed As Boolean

    Public Sub New()
        Me.DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
            ControlStyles.DoubleBuffer Or _
            ControlStyles.ResizeRedraw Or _
            ControlStyles.UserPaint Or _
            ControlStyles.OptimizedDoubleBuffer Or _
            ControlStyles.SupportsTransparentBackColor, True)

        Timer.Interval = 40
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.FillRectangle(Brushes.Transparent, e.ClipRectangle)
        Dim Text_Width As Integer = Convert.ToInt32(e.Graphics.MeasureString(Me.Text, Me.Font).Width)
        If Text_Width > Me.Width Then Scrolling_Needed = True

        If Scrolling_Needed Then
            If Scroll_X > Text_Width Then Scroll_X = 0
            e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), New Point(Scroll_X * -1, 0))
            e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), New Point(Text_Width + (Scroll_X * -1), 0))
        Else
            e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), Point.Empty)
        End If

        MyBase.OnPaint(e)
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        If Scrolling_Needed Then Timer.Enabled = True

        MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        Timer.Enabled = False
        Scroll_X = 0
        Me.Refresh()

        MyBase.OnMouseLeave(e)
    End Sub
    Protected Overrides Sub OnTextChanged(e As EventArgs)
        Scrolling_Needed = False
        Me.Refresh()

        MyBase.OnTextChanged(e)
    End Sub

    Private Sub Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer.Tick
        Scroll_X += 1
        Me.Refresh()
    End Sub
End Class
