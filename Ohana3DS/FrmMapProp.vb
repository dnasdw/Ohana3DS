Imports System.Runtime.InteropServices

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

    Dim mapVals(100) As UInteger
    Dim width, height As UShort

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
                width = br.ReadUInt16()
                height = br.ReadUInt16()
                Dim img As New Bitmap(width * 8, height * 8)
                Dim c As New Color()
                For i As Integer = 0 To width * height - 1
                    Dim col2 As UInteger = br.ReadUInt32()
                    proplist.Add(col2)
                    If col2 = &H1000021 Then
                        c = Color.Black
                    Else
                        col2 = LCG(col2, 4)
                        c = Color.FromArgb(&HFF, &HFF - CByte(col2 And &HFF), &HFF - CByte((col2 >> 8) And &HFF), &HFF - CByte(col2 >> 24 And &HFF))
                    End If
                    For x As Integer = 0 To 7
                        For y As Integer = 0 To 7
                            img.SetPixel((x + (i * 8) Mod (img.Width)), y + ((i \ width) * 8), c)
                        Next
                    Next
                Next
                br.Close()
                mapPicBox.Image = img
                mapVals = proplist.ToArray
            End Using
        End Using
    End Sub
    Public Function LCG(seed As Long, ctr As Integer) As UInteger
        For i As Integer = 0 To ctr - 1
            seed *= &H41C64E6D
            seed += &H6073
        Next
        Return seed
    End Function

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

    Private Sub mapPicBox_Click(sender As Object, e As EventArgs) Handles mapPicBox.Click
        Dim mouseEventArgs = TryCast(e, MouseEventArgs)
        If mouseEventArgs IsNot Nothing Then
            Dim mapProps As String() = My.Resources.MapProperties.Split(New Char() {Environment.NewLine, ","}, StringSplitOptions.None)
            Dim X As Integer = Math.Floor(mouseEventArgs.X / 8)
            Dim Y As Integer = Math.Floor(mouseEventArgs.Y / 8)
            mapCoords.Text = "X= " & Convert.ToString(X) & " Y= " & Convert.ToString(Y)
            For i As UInteger = 0 To mapProps.Length - 1 Step 2
                Dim p1 As UInteger = UInteger.Parse(mapProps(i))
                Dim p2 As String = mapProps(i + 1)
                Clipboard.SetText(mapVals(((Y * 40) + X)))
                If p1 = mapVals(((Y * 40) + X)) Then
                    mapPropCom.Text = p2
                End If
            Next
        End If
    End Sub

    Private Sub mapPropSave_Click(sender As Object, e As EventArgs) Handles mapPropSave.Click
        MessageBox.Show("Not finished.")
    End Sub

End Class