Imports System.ComponentModel
Imports System.Drawing.Design

Public Class MyListview
    Inherits Control

    Private Selected As Color = Color.FromArgb(15, 82, 186)
    Private Ash_Gray As Color = Color.FromArgb(178, 190, 181)

    Public Structure ListText
        Dim Text As String
        Dim Left As Integer
        Dim ForeColor As Color
        Dim Vertical_Line As Boolean
    End Structure
    Public Structure ListItem
        Dim Text() As ListText
        Dim Header As Boolean
    End Structure
    Private LstItems As New List(Of ListItem)
    Private Tile_Height As Integer = 32
    Private Selected_Index As Integer = -1
    Private Selected_Index_Total As Integer = -1

    Private Scroll_Y As Integer
    Private Scroll_Bar_Y As Integer, Scroll_Bar_Height As Integer = 64

    Private Mouse_Position As Point
    Private Scroll_Mouse_Y As Integer
    Private Mouse_Drag As Boolean
    Private Clicked As Boolean

    Private Bar_Fore_Color As Color
    Public Sub New()
        Me.DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
            ControlStyles.DoubleBuffer Or _
            ControlStyles.ResizeRedraw Or _
            ControlStyles.UserPaint Or _
            ControlStyles.OptimizedDoubleBuffer Or _
            ControlStyles.SupportsTransparentBackColor, True)
    End Sub
    Public Property TileHeight() As Integer
        Get
            Return Tile_Height
        End Get
        Set(Value As Integer)
            Tile_Height = Value
        End Set
    End Property
    Public Property SelectedIndex() As Integer
        Get
            Return Selected_Index
        End Get
        Set(Value As Integer)
            Selected_Index = Value
        End Set
    End Property
    Public Sub AddItem(Text As String, Optional Left As Integer = 0, Optional Header As Boolean = False)
        Dim Item As ListItem
        ReDim Item.Text(0)
        Item.Text(0).Text = Text
        Item.Text(0).Left = Left
        Item.Header = Header
        LstItems.Add(Item)
    End Sub
    Public Sub AddItem(Item As ListItem)
        LstItems.Add(Item)
    End Sub
    Public Sub ChangeItem(Index As Integer, Item As ListItem)
        Dim Temp() As ListItem = LstItems.ToArray()
        Dim j As Integer
        For i As Integer = 0 To Temp.Length - 1
            If Not Temp(i).Header Then
                If j = Index Then
                    Temp(i) = Item
                    Exit For
                End If
                j += 1
            End If
        Next
        LstItems.Clear()
        LstItems.AddRange(Temp)
        Me.Refresh()
    End Sub
    Public Sub Clear()
        LstItems.Clear()
        Selected_Index = -1
        Scroll_Y = 0
        Scroll_Bar_Y = 0
        Me.Refresh()
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If LstItems.Count = 0 Then Exit Sub
        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.ClipRectangle)

        If LstItems IsNot Nothing Then
            Dim Total_Size As Integer = LstItems.Count * Tile_Height
            Dim Start_Y As Integer
            If Total_Size > Me.Height Then Start_Y = Scroll_Y * -1
            Dim Total_Index, Index As Integer

            For Each Item As ListItem In LstItems
                'Item selecionado (e detecção de click no Item)
                If Start_Y >= -Tile_Height Then
                    If Start_Y > Me.Height Then Exit For
                    If Not Item.Header Then
                        If Clicked Then
                            Dim Item_Rect As New Rectangle(0, Start_Y, Me.Width, TileHeight)
                            Dim Mouse_Rect As New Rectangle(Mouse_Position, New Size(1, 1))
                            If Item_Rect.IntersectsWith(Mouse_Rect) Then 'Selecionado
                                Clicked = False
                                e.Graphics.FillRectangle(New SolidBrush(Selected), New Rectangle(0, Start_Y, Me.Width, TileHeight))
                                Selected_Index = Index
                                Selected_Index_Total = Total_Index
                                RaiseEvent SelectedIndexChanged(Index)
                            End If
                        Else
                            If Index = Selected_Index Then e.Graphics.FillRectangle(New SolidBrush(Selected), New Rectangle(0, Start_Y, Me.Width, TileHeight))
                        End If
                    End If

                    'Textos e afins
                    Dim Temp As Integer = 0
                    For Each SubData As ListText In Item.Text
                        Dim TxtHeight As Integer = Convert.ToInt32(e.Graphics.MeasureString(SubData.Text, Me.Font).Height)
                        If SubData.ForeColor = Nothing Then SubData.ForeColor = Me.ForeColor
                        If SubData.Text <> Nothing Then
                            e.Graphics.DrawString(SubData.Text, Me.Font, New SolidBrush(SubData.ForeColor), New Point(SubData.Left, (Start_Y + (Tile_Height \ 2) - (TxtHeight \ 2))))
                        End If

                        If Item.Header Then
                            Dim ItemW As Integer
                            If Temp = Item.Text.Count - 1 Then ItemW = Me.Width - SubData.Left Else ItemW = Item.Text(Temp + 1).Left - SubData.Left
                            Dim X As Integer = SubData.Left + ItemW - 1
                            Dim Y As Integer = Start_Y + Tile_Height - 1
                            e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(SubData.Left, Y), New Point(X, Y))
                            e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(X, Start_Y), New Point(X, Y))
                        End If

                        If SubData.Vertical_Line Then
                            e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(SubData.Left - 1, Start_Y), New Point(SubData.Left - 1, Me.Height))
                        End If

                        Temp += 1
                    Next
                End If

                Start_Y += Tile_Height
                If Not Item.Header Then Index += 1
                Total_Index += 1
            Next

            'Barra de rolagem
            If Total_Size > Me.Height Then
                e.Graphics.FillRectangle(New SolidBrush(Bar_Fore_Color), New Rectangle(Me.Width - 10, Scroll_Bar_Y + 1, 9, Scroll_Bar_Height - 2))
                Draw_Rounded_Rectangle(e.Graphics, New Rectangle(Me.Width - 11, Scroll_Bar_Y, 10, Scroll_Bar_Height - 1), 4, Bar_Fore_Color)
            End If
        End If

        MyBase.OnPaint(e)
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        Me.Focus()

        MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim Scroll_Rect As New Rectangle(Me.Width - 10, Scroll_Bar_Y, 10, Scroll_Bar_Height)
            If Scroll_Rect.IntersectsWith(New Rectangle(e.X, e.Y, 1, 1)) Then
                Scroll_Mouse_Y = e.Y - Scroll_Bar_Y
                Mouse_Drag = True
            Else
                Clicked = True
                Mouse_Position = e.Location
                Me.Refresh()
            End If
        End If

        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        Me.Focus()
        If e.Button = Windows.Forms.MouseButtons.Left Then Mouse_Drag = False

        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        Dim Scroll_Rect As New Rectangle(Me.Width - 10, Scroll_Bar_Y, 10, Scroll_Bar_Height)
        If Scroll_Rect.IntersectsWith(New Rectangle(e.X, e.Y, 1, 1)) Then
            If Bar_Fore_Color <> Ash_Gray Then
                Bar_Fore_Color = Ash_Gray
                Me.Refresh()
            End If
        ElseIf Not Mouse_Drag Then
            If Bar_Fore_Color <> Color.White Then
                Bar_Fore_Color = Color.White
                Me.Refresh()
            End If
        End If

        If e.Button = Windows.Forms.MouseButtons.Left And Mouse_Drag Then
            Dim Y As Integer = e.Y - Scroll_Mouse_Y
            If Y < 0 Then
                Y = 0
            ElseIf Y > Me.Height - Scroll_Bar_Height Then
                Y = Me.Height - Scroll_Bar_Height
            End If
            Scroll_Bar_Y = Y

            Dim Total_Size As Integer = LstItems.Count * Tile_Height
            Scroll_Y = Convert.ToInt32((Y / (Me.Height - Scroll_Bar_Height)) * (Total_Size - Me.Height))
            Me.Refresh()
        End If

        MyBase.OnMouseMove(e)
    End Sub
    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        Dim Total_Size As Integer = LstItems.Count * Tile_Height
        If e.Delta <> 0 And Total_Size > Me.Height Then
            Dim Y As Integer

            If e.Delta > 0 Then
                Y = Scroll_Bar_Y - 16
            ElseIf e.Delta < 0 Then
                Y = Scroll_Bar_Y + 16
            End If

            If Y < 0 Then
                Y = 0
            ElseIf Y > Me.Height - Scroll_Bar_Height Then
                Y = Me.Height - Scroll_Bar_Height
            End If
            Scroll_Bar_Y = Y

            Scroll_Y = Convert.ToInt32((Y / (Me.Height - Scroll_Bar_Height)) * (Total_Size - Me.Height))
            Me.Refresh()
        End If

        MyBase.OnMouseWheel(e)
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        If Not Mouse_Drag Then
            Bar_Fore_Color = Color.White
            Me.Refresh()
        End If

        MyBase.OnMouseLeave(e)
    End Sub
    Protected Overrides Function IsInputKey(keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.Up, Keys.Down, Keys.Left, Keys.Right
                Return True
        End Select
        Return MyBase.IsInputKey(keyData)
    End Function
    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Dim Total_Size As Integer = LstItems.Count * Tile_Height
        Select Case e.KeyCode
            Case Keys.Up
                If Selected_Index > 0 Then
                    Selected_Index -= 1
                    Selected_Index_Total -= 1
                    RaiseEvent SelectedIndexChanged(Selected_Index)
                End If

                While (Selected_Index_Total * Tile_Height) - Scroll_Y < 0
                    Dim Y As Integer = Scroll_Bar_Y - 1
                    If Y < 0 Then Y = 0
                    Scroll_Bar_Y = Y
                    Scroll_Y = Convert.ToInt32((Y / (Me.Height - Scroll_Bar_Height)) * (Total_Size - Me.Height))
                    If Y = 0 Then Exit While
                End While
            Case Keys.Down
                If Selected_Index < Get_Headerless_Length() - 1 Then
                    Selected_Index += 1
                    Selected_Index_Total += 1
                    RaiseEvent SelectedIndexChanged(Selected_Index)
                End If

                While (Selected_Index_Total * Tile_Height) - Scroll_Y > (Me.Height - Tile_Height)
                    Dim Y As Integer = Scroll_Bar_Y + 1
                    If Y > Me.Height - Scroll_Bar_Height Then Y = Me.Height - Scroll_Bar_Height
                    Scroll_Bar_Y = Y
                    Scroll_Y = Convert.ToInt32((Y / (Me.Height - Scroll_Bar_Height)) * (Total_Size - Me.Height))
                    If Y = Me.Height - Scroll_Bar_Height Then Exit While
                End While
        End Select

        Me.Refresh()

        MyBase.OnKeyDown(e)
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

    Private Function Get_Headerless_Length() As Integer
        Dim j As Integer
        For i As Integer = 0 To LstItems.Count - 1
            If Not LstItems(i).Header Then j += 1
        Next
        Return j
    End Function

    Public Event SelectedIndexChanged(New_Index As Integer)
End Class
