Public Class MyPicturebox
    Inherits PictureBox

    Private Ash_Gray As Color = Color.FromArgb(178, 190, 181)

    Private Scroll_X, Scroll_Y As Integer
    Private Scroll_Bar_X, Scroll_Bar_Y As Integer, Scroll_Bar_Size As Integer = 64

    Private Mouse_Position As Point
    Private Scroll_Mouse_X, Scroll_Mouse_Y As Integer
    Private Mouse_Drag_H, Mouse_Drag_V As Boolean
    Private Bar_Fore_Color_H, Bar_Fore_Color_V As Color

    Private Mouse_Inside, Show_Scrollbars As Boolean

    Private Img As Image

    Public Overloads Property Image As Image
        Get
            Return Img
        End Get
        Set(Value As Image)
            Img = Value
            Scroll_X = 0
            Scroll_Y = 0
            Scroll_Bar_X = 0
            Scroll_Bar_Y = 0
            Show_Scrollbars = False
            Me.Refresh()
        End Set
    End Property

    Protected Overrides Sub OnPaint(pe As PaintEventArgs)
        If Img IsNot Nothing Then
            pe.Graphics.DrawImage(Img, New Point(If(Img.Width > Me.Width, Scroll_X * -1, 0), If(Img.Height > Me.Height, Scroll_Y * -1, 0)))

            'Barra de rolagem
            If Show_Scrollbars Then
                If Img.Width > Me.Width Then 'Horizontal
                    pe.Graphics.FillRectangle(New SolidBrush(Bar_Fore_Color_H), New Rectangle(Scroll_Bar_X + 1, Me.Height - 10, Scroll_Bar_Size - 2, 9))
                    Draw_Rounded_Rectangle(pe.Graphics, New Rectangle(Scroll_Bar_X, Me.Height - 11, Scroll_Bar_Size - 1, 10), 4, Bar_Fore_Color_H)
                End If
                If Img.Height > Me.Height Then 'Vertical
                    pe.Graphics.FillRectangle(New SolidBrush(Bar_Fore_Color_V), New Rectangle(Me.Width - 10, Scroll_Bar_Y + 1, 9, Scroll_Bar_Size - 2))
                    Draw_Rounded_Rectangle(pe.Graphics, New Rectangle(Me.Width - 11, Scroll_Bar_Y, 10, Scroll_Bar_Size - 1), 4, Bar_Fore_Color_V)
                End If
            End If
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim Scroll_Rect_H As New Rectangle(Scroll_Bar_X, Me.Height - 10, Scroll_Bar_Size, 10)
            Dim Scroll_Rect_V As New Rectangle(Me.Width - 10, Scroll_Bar_Y, 10, Scroll_Bar_Size)
            Dim Mouse_Rect As New Rectangle(e.X, e.Y, 1, 1)
            If Scroll_Rect_H.IntersectsWith(Mouse_Rect) Then
                Scroll_Mouse_X = e.X - Scroll_Bar_X
                Mouse_Drag_H = True
            ElseIf Scroll_Rect_V.IntersectsWith(Mouse_Rect) Then
                Scroll_Mouse_Y = e.Y - Scroll_Bar_Y
                Mouse_Drag_V = True
            End If
        End If

        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        Me.Focus()
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Mouse_Drag_H = False
            Mouse_Drag_V = False
            If Not Mouse_Inside Then
                Show_Scrollbars = False
                Me.Refresh()
            End If
        End If

        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        Dim Scroll_Rect_H As New Rectangle(Scroll_Bar_X, Me.Height - 10, Scroll_Bar_Size, 10)
        Dim Scroll_Rect_V As New Rectangle(Me.Width - 10, Scroll_Bar_Y, 10, Scroll_Bar_Size)
        Dim Mouse_Rect As New Rectangle(e.X, e.Y, 1, 1)
        If Scroll_Rect_H.IntersectsWith(Mouse_Rect) Then
            If Bar_Fore_Color_H <> Ash_Gray Then
                Bar_Fore_Color_H = Ash_Gray
                Me.Refresh()
            End If
        ElseIf Not Mouse_Drag_H Then
            If Bar_Fore_Color_H <> Color.White Then
                Bar_Fore_Color_H = Color.White
                Me.Refresh()
            End If
        End If
        If Scroll_Rect_V.IntersectsWith(Mouse_Rect) Then
            If Bar_Fore_Color_V <> Ash_Gray Then
                Bar_Fore_Color_V = Ash_Gray
                Me.Refresh()
            End If
        ElseIf Not Mouse_Drag_V Then
            If Bar_Fore_Color_V <> Color.White Then
                Bar_Fore_Color_V = Color.White
                Me.Refresh()
            End If
        End If

        If e.Button = Windows.Forms.MouseButtons.Left Then
            If Mouse_Drag_H Then
                Dim X As Integer = e.X - Scroll_Mouse_X
                If X < 0 Then
                    X = 0
                ElseIf X > Me.Width - Scroll_Bar_Size Then
                    X = Me.Width - Scroll_Bar_Size
                End If
                Scroll_Bar_X = X

                Scroll_X = Convert.ToInt32((X / (Me.Width - Scroll_Bar_Size)) * (Img.Width - Me.Width))
                Me.Refresh()
            ElseIf Mouse_Drag_V Then
                Dim Y As Integer = e.Y - Scroll_Mouse_Y
                If Y < 0 Then
                    Y = 0
                ElseIf Y > Me.Height - Scroll_Bar_Size Then
                    Y = Me.Height - Scroll_Bar_Size
                End If
                Scroll_Bar_Y = Y

                Scroll_Y = Convert.ToInt32((Y / (Me.Height - Scroll_Bar_Size)) * (Img.Height - Me.Height))
                Me.Refresh()
            End If
        End If

        MyBase.OnMouseMove(e)
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        Mouse_Inside = True
        Show_Scrollbars = True
        Me.Refresh()

        MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        If Not Mouse_Drag_H Then Bar_Fore_Color_H = Color.White
        If Not Mouse_Drag_V Then Bar_Fore_Color_V = Color.White
        Mouse_Inside = False
        If Not Mouse_Drag_H And Not Mouse_Drag_V Then Show_Scrollbars = False
        Me.Refresh()

        MyBase.OnMouseLeave(e)
    End Sub

    Private Sub Draw_Rounded_Rectangle(ByVal Gfx As Graphics, Rect As Rectangle, Width As Integer, ByVal Color As Color)
        Dim Pen As New Pen(Color)

        Dim Arc_Rect As New RectangleF(Rect.Location, New SizeF(Width, Width))

        'Top Left Arc
        Gfx.DrawArc(Pen, Arc_Rect, 180, 90)
        Gfx.DrawLine(Pen, Rect.X + CInt(Width / 2), Rect.Y, Rect.X + Rect.Width - CInt(Width / 2), Rect.Y)

        'Top Right Arc
        Arc_Rect.X = Rect.Right - Width
        Gfx.DrawArc(Pen, Arc_Rect, 270, 90)
        Gfx.DrawLine(Pen, Rect.X + Rect.Width, Rect.Y + CInt(Width / 2), Rect.X + Rect.Width, Rect.Y + Rect.Height - CInt(Width / 2))

        'Bottom Right Arc
        Arc_Rect.Y = Rect.Bottom - Width
        Gfx.DrawArc(Pen, Arc_Rect, 0, 90)
        Gfx.DrawLine(Pen, Rect.X + CInt(Width / 2), Rect.Y + Rect.Height, Rect.X + Rect.Width - CInt(Width / 2), Rect.Y + Rect.Height)

        'Bottom Left Arc
        Arc_Rect.X = Rect.Left
        Gfx.DrawArc(Pen, Arc_Rect, 90, 90)
        Gfx.DrawLine(Pen, Rect.X, Rect.Y + CInt(Width / 2), Rect.X, Rect.Y + Rect.Height - CInt(Width / 2))
    End Sub
End Class
