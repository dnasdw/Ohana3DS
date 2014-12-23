Imports System.Runtime.InteropServices
Imports System.IO
Public Class FrmMapProp
    <DllImport("dwmapi")> _
    Public Shared Function DwmExtendFrameIntoClientArea(ByVal hWnd As IntPtr, ByRef pMarInset As Margins) As Integer
    End Function
    <DllImport("dwmapi")> _
    Public Shared Function DwmSetWindowAttribute(ByVal hWnd As IntPtr, ByVal Attr As Integer, ByRef AttrValue As Integer, ByVal AttrSize As Integer) As Integer
    End Function
    Public Structure Margins
        Dim TopHeight As Integer
        Dim BottomHeight As Integer
        Dim LeftWidth As Integer
        Dim RightWidth As Integer
    End Structure

    Dim mapVals As UInteger()
    Dim mapProps As String()

    Dim mouseX As Integer = 0
    Dim mouseY As Integer = 0

    Dim mapWidth As Short = 0
    Dim mapHeight As Short = 0

    Dim mode As Boolean = False

    Private Sub FrmMapProp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MyOhana.Map_Properties_Mode = True
        mapProps = My.Resources.MapProperties.Split(New Char() {Environment.NewLine, ","}, StringSplitOptions.None)
        mapPropCom.DataSource = MyOhana.getProps()
    End Sub
    Private Sub FrmMapProp_FormClosing(sender As Object, e As EventArgs) Handles MyBase.FormClosing
        MyOhana.Map_Properties_Mode = False
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams 'Cria sombra (sem Aero)
        Get
            Dim Create_Params As CreateParams = MyBase.CreateParams
            Create_Params.ClassStyle = Create_Params.ClassStyle Or &H20000
            Return Create_Params
        End Get
    End Property

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg <> &HA3 Then MyBase.WndProc(m)
        Select Case m.Msg
            Case &H84
                Dim Mouse_Position As Point = PointToClient(Cursor.Position)
                If Mouse_Position.Y < 32 Then If m.Result = New IntPtr(1) Then m.Result = New IntPtr(2)
            Case &H85 'Cria sombra (com Aero)
                Dim val = 2
                DwmSetWindowAttribute(Handle, 2, val, 4)
                Dim Margins As New Margins()
                With Margins
                    .TopHeight = 1
                    .BottomHeight = 1
                    .LeftWidth = 1
                    .RightWidth = 1
                End With
                DwmExtendFrameIntoClientArea(Handle, Margins)
        End Select
    End Sub

    Public Sub makeMapIMG(byteArray As Byte())
        Dim proplist As New List(Of UInteger)
        Using dataStream As IO.Stream = New IO.MemoryStream(byteArray)
            Using br As New IO.BinaryReader(dataStream)
                Try
                    mapWidth = br.ReadUInt16()
                    mapHeight = br.ReadUInt16()
                    For i As Integer = 0 To mapWidth * mapHeight - 1
                        proplist.Add(br.ReadUInt32())
                    Next
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                End Try
                br.Close()
                mapVals = proplist.ToArray
                updateImg()
            End Using
        End Using
    End Sub

    Private Sub updateImg()
        Dim img As New Bitmap(mapWidth * 8, mapHeight * 8)
        Dim c As New Color()
        Dim i As Integer = 0
        Dim col As UInteger = 0
        For v As Integer = 0 To mapVals.Length - 1
            col = mapVals(v)
            If col = &H1000021 Then
                c = Color.Black
            Else
                col = LCG(col, 4)
                c = Color.FromArgb(&HFF, &HFF - CByte(col And &HFF), &HFF - CByte((col >> 8) And &HFF), &HFF - CByte(col >> 24 And &HFF))
            End If
            For x As Integer = 0 To 7
                For y As Integer = 0 To 7
                    img.SetPixel((x + (i * 8) Mod (img.Width)), y + ((i \ mapWidth) * 8), If(x = 7 Or y = 7, Color.Black, c))
                Next
            Next
            i = i + 1
        Next
        mapPicBox.Image = img
    End Sub

    Public Function LCG(seed As Long, ctr As Integer) As UInteger
        For i As Integer = 0 To ctr - 1
            seed *= &H41C64E6D
            seed += &H6073
        Next
        Return seed
    End Function

    Private Sub mapPicBox_Click(sender As Object, e As EventArgs) Handles mapPicBox.Click
        Dim mouseEventArgs = TryCast(e, MouseEventArgs)
        If mouseEventArgs IsNot Nothing Then
            mouseX = Math.Floor(mouseEventArgs.X / 8)
            mouseY = Math.Floor(mouseEventArgs.Y / 8)
            mapCoords.Text = "X= " & Convert.ToString(mouseX) & " Y= " & Convert.ToString(mouseY)
            If mode = True Then 'Edit mode
                mapVals((mouseY * 40) + mouseX) = UInteger.Parse(mapProps(Array.FindIndex(mapProps, Function(s) s = mapPropCom.Text) - 1))
                updateImg()
            Else                'View mode
                For i As UInteger = 0 To mapProps.Length - 1 Step 2
                    Dim p1 As UInteger = UInteger.Parse(mapProps(i))
                    Dim p2 As String = mapProps(i + 1)
                    If p1 = mapVals(((mouseY * 40) + mouseX)) Then
                        mapPropCom.Text = p2
                    End If
                Next
            End If
        End If
    End Sub

    Public Function getMapVals() As UInteger()
        Return mapVals
    End Function

    Private Sub mapPropSet_Click(sender As Object, e As EventArgs) Handles mapPropSet.Click
        If mode = True Then
            mode = False
            mapPropSet.Text = "Edit"
        Else
            mode = True
            mapPropSet.Text = "View"
        End If
    End Sub

    Private Sub mapPropSave_Click(sender As Object, e As EventArgs) Handles mapPropSave.Click
        FrmMain.saveMapProps(mapWidth, mapHeight, mapVals)
    End Sub

#Region "GUI"
    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub
    Private Sub BtnMinimize_Click(sender As Object, e As EventArgs) Handles BtnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub Button_MouseEnter(sender As Object, e As EventArgs) Handles BtnMinimize.MouseEnter
        Dim Lbl As Label = CType(sender, Label)
        Lbl.BackColor = Color.FromArgb(15, 82, 186)
        Lbl.ForeColor = Color.White
    End Sub
    Private Sub BtnClose_MouseEnter(sender As Object, e As EventArgs) Handles BtnClose.MouseEnter
        Dim Lbl As Label = CType(sender, Label)
        Lbl.BackColor = Color.Crimson
        Lbl.ForeColor = Color.WhiteSmoke
    End Sub
    Private Sub Button_MouseLeave(sender As Object, e As EventArgs) Handles BtnMinimize.MouseLeave, BtnClose.MouseLeave
        Dim Lbl As Label = CType(sender, Label)
        Lbl.BackColor = Color.Transparent
        Lbl.ForeColor = Color.White
    End Sub
#End Region

End Class