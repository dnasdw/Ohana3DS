Public Class MyGroupbox
    Inherits GroupBox
    Public Sub New()
        Me.DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim Text_Size As Size = TextRenderer.MeasureText(Me.Text, Me.Font)

        Dim Border_Rectangle As Rectangle = e.ClipRectangle
        Border_Rectangle.Y = (Border_Rectangle.Y + (Text_Size.Height \ 2))
        Border_Rectangle.Width -= 1
        Border_Rectangle.Height = (Border_Rectangle.Height - (Text_Size.Height \ 2))

        e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(0, 8), New Point(6, 8))
        e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(Text_Size.Width, 8), New Point(Border_Rectangle.Width, 8))

        Dim Text_Rectangle As New Rectangle(6, 0, Text_Size.Width, Text_Size.Height)
        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), Text_Rectangle)
        e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), Text_Rectangle)
    End Sub
End Class
