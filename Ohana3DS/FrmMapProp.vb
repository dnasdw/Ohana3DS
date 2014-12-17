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