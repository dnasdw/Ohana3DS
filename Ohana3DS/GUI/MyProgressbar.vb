Imports System.Drawing.Drawing2D

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
        Dim Bg As New LinearGradientBrush(New Point(0, 0), New Point(0, Me.Height - 1), Color.Transparent, Color.FromArgb(15, 82, 186))
        e.Graphics.FillRectangle(Bg, New Rectangle(0, 0, Convert.ToInt32((ProgressVal / 100) * (Me.Width - 1)), Me.Height - 1))

        Dim Text As String = Me.Text
        Dim Ellipsis As Boolean = e.Graphics.MeasureString(Text, Me.Font).Width > Me.Width
        If Ellipsis Then
            While e.Graphics.MeasureString(Text, Me.Font).Width > Me.Width
                Text = Text.Substring(0, Text.Length - 1)
            End While
            If Text.Length > 0 Then Text = Text.Substring(0, Text.Length - 1)
            Text &= "..."
        End If
        Dim TextW As Integer = Convert.ToInt32(e.Graphics.MeasureString(Text, Me.Font).Width)
        Dim TextH As Integer = Convert.ToInt32(e.Graphics.MeasureString(Text, Me.Font).Height)
        e.Graphics.DrawString(Text, Me.Font, New SolidBrush(Me.ForeColor), New Point((Me.Width \ 2) - (TextW \ 2), (Me.Height \ 2) - (TextH \ 2)))

        Dim Border As New LinearGradientBrush(New Point(0, 0), New Point(0, Me.Height - 1), Color.Transparent, Me.ForeColor)
        e.Graphics.DrawRectangle(New Pen(Border), New Rectangle(0, 0, Me.Width - 1, Me.Height - 1))

        MyBase.OnPaint(e)
    End Sub
End Class
