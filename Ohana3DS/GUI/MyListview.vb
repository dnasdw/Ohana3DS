Imports System.ComponentModel
Imports System.Drawing.Design

Public Class MyListview
    Inherits Control

    Private Selected As Color = Color.FromArgb(15, 82, 186)

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

    Private Total_Size As Integer
    Private Min_Height As Integer = 250
    Private Total_Items As Integer

    Dim Mouse_Position As Point
    Private First_Click As Boolean
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
    Public Property MinHeight() As Integer
        Get
            Return Min_Height
        End Get
        Set(Value As Integer)
            Min_Height = Value
        End Set
    End Property
    Public ReadOnly Property SelectedIndex() As Integer
        Get
            Return Selected_Index
        End Get
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
    Public Sub Clear()
        LstItems.Clear()
        Me.Refresh()
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If LstItems.Count = 0 Then Exit Sub
        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.ClipRectangle)

        If LstItems IsNot Nothing Then
            Dim Start_Y As Integer
            Dim Index As Integer

            For Each Item As ListItem In LstItems
                If Item.Text(0).Text = "-" Then
                    e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(0, Start_Y + 1), New Point(e.ClipRectangle.Width, Start_Y + 1))
                    Start_Y += 3
                Else
                    Dim Item_Rect As New Rectangle(0, Start_Y, Me.Width, TileHeight)
                    Dim Mouse_Rect As New Rectangle(Mouse_Position, New Size(1, 1))
                    If Item_Rect.IntersectsWith(Mouse_Rect) And First_Click And Not Item.Header Then 'Selecionado
                        e.Graphics.FillRectangle(New SolidBrush(Selected), New Rectangle(0, Start_Y, Me.Width, TileHeight))
                        Selected_Index = Index
                    End If

                    Dim Temp As Integer = 0
                    For Each SubData As ListText In Item.Text
                        Dim TxtHeight As Integer = Convert.ToInt32(e.Graphics.MeasureString(SubData.Text, Me.Font).Height)
                        If SubData.ForeColor = Nothing Then SubData.ForeColor = Me.ForeColor
                        e.Graphics.DrawString(SubData.Text, Me.Font, New SolidBrush(SubData.ForeColor), New Point(SubData.Left, (Start_Y + (TileHeight \ 2) - (TxtHeight \ 2))))

                        If Item.Header Then
                            Dim ItemW As Integer
                            If Temp = Item.Text.Count - 1 Then ItemW = Me.Width - SubData.Left Else ItemW = Item.Text(Temp + 1).Left - SubData.Left
                            Dim X As Integer = SubData.Left + ItemW - 1
                            Dim Y As Integer = Start_Y + TileHeight - 1
                            e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(SubData.Left, Y), New Point(X, Y))
                            e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(X, Start_Y), New Point(X, Y))
                        End If

                        If SubData.Vertical_Line Then
                            e.Graphics.DrawLine(New Pen(Me.ForeColor), New Point(SubData.Left - 1, Start_Y), New Point(SubData.Left - 1, Me.Height))
                        End If

                        Temp += 1
                    Next
                    Start_Y += TileHeight

                    If Not Item.Header Then Index += 1
                End If
            Next

            Total_Size = Start_Y
            If Me.Height <> Total_Size Then
                If Total_Size > Min_Height Then Me.Height = Total_Size Else Me.Height = Min_Height
            End If
            Total_Items = Index
        End If

        MyBase.OnPaint(e)
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        First_Click = True
        Mouse_Position = e.Location
        Me.Refresh()

        MyBase.OnMouseDown(e)
    End Sub
End Class
