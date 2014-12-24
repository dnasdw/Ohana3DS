Imports System.IO
Imports System.Text
Imports System.Globalization
Imports System.Runtime.InteropServices
Public Class FrmVertexEditor
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

    Private Sub FrmVertexEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LstObjects.Clear()

        Dim ObjIndex As Integer
        For Each Obj As Ohana.VertexList In MyOhana.Model_Object
            With Obj
                LstObjects.AddItem("obj_" & ObjIndex & "_" & MyOhana.Model_Texture_Index(.Texture_ID))

            End With
            ObjIndex += 1
        Next

        LstObjects.Refresh()
    End Sub
    Private Sub FrmMapProp_FormClosing(sender As Object, e As EventArgs) Handles MyBase.FormClosing
        MyOhana.Edit_Mode = False
    End Sub

    Private Sub LstObjects_SelectedIndexChanged(Index As Integer) Handles LstObjects.SelectedIndexChanged
        LstFaces.Clear()
        MyOhana.Selected_Object = Index
        MyOhana.Selected_Face = -1
        For Face As Integer = 0 To MyOhana.Model_Object(Index).Per_Face_Index.Count - 1
            LstFaces.AddItem("face_" & Face)
        Next
        TxtTexID.Text = MyOhana.Model_Object(Index).Texture_ID.ToString()
        LstFaces.Refresh()
        MyOhana.Edit_Mode = True
    End Sub
    Private Sub LstFaces_SelectedIndexChanged(Index As Integer) Handles LstFaces.SelectedIndexChanged
        MyOhana.Selected_Face = Index
    End Sub

    Private Sub BtnImportObj_Click(sender As Object, e As EventArgs) Handles BtnImportObj.Click
        If LstObjects.SelectedIndex > -1 Then
            Dim OpenDlg As New OpenFileDialog
            OpenDlg.Filter = "Wavefront OBJ|*.obj"
            If OpenDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                If File.Exists(OpenDlg.FileName) Then
                    MyOhana.Insert_OBJ(OpenDlg.FileName)
                End If
            End If
        Else
            MsgBox("You must select an object first!", vbExclamation)
        End If
    End Sub
    Private Sub BtnExportObj_Click(sender As Object, e As EventArgs) Handles BtnExportObj.Click
        If LstObjects.SelectedIndex > -1 Then
            Dim SaveDlg As New SaveFileDialog
            SaveDlg.Filter = "Wavefront OBJ|*.obj"
            If SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim Out As New StringBuilder
                Dim Info As New NumberFormatInfo
                Info.NumberDecimalSeparator = "."
                Info.NumberDecimalDigits = 6

                Out.AppendLine("mtllib " & Path.GetFileName(SaveDlg.FileName) & ".mtl")

                With MyOhana.Model_Object(MyOhana.Selected_Object)
                    For i As Integer = 0 To .Vertice.Length - 1
                        Out.AppendLine("v " & (.Vertice(i).X * MyOhana.Load_Scale).ToString("N", Info) & " " & (.Vertice(i).Y * MyOhana.Load_Scale).ToString("N", Info) & " " & (.Vertice(i).Z * MyOhana.Load_Scale).ToString("N", Info))
                        Out.AppendLine("vn " & (.Vertice(i).NX * MyOhana.Load_Scale).ToString("N", Info) & " " & (.Vertice(i).NY * MyOhana.Load_Scale).ToString("N", Info) & " " & (.Vertice(i).NZ * MyOhana.Load_Scale).ToString("N", Info))
                        Out.AppendLine("vt " & .Vertice(i).U.ToString("N", Info) & " " & .Vertice(i).V.ToString("N", Info))
                    Next

                    Out.AppendLine("usemtl " & MyOhana.Model_Texture_Index(.Texture_ID))

                    For i As Integer = 0 To .Index.Length - 1 Step 3
                        Dim a As String = (.Index(i) + 1).ToString()
                        Dim b As String = (.Index(i + 1) + 1).ToString()
                        Dim c As String = (.Index(i + 2) + 1).ToString()

                        Out.AppendLine("f " & a & "/" & a & "/" & a & " " & b & "/" & b & "/" & b & " " & c & "/" & c & "/" & c)
                    Next

                    File.WriteAllText(SaveDlg.FileName, Out.ToString)

                    Dim OutMtl As New StringBuilder
                    OutMtl.AppendLine("newmtl " & MyOhana.Model_Texture_Index(.Texture_ID))
                    OutMtl.AppendLine("illum 2")
                    OutMtl.AppendLine("Kd 0.800000 0.800000 0.800000")
                    OutMtl.AppendLine("Ka 0.200000 0.200000 0.200000")
                    OutMtl.AppendLine("Ks 0.000000 0.000000 0.000000")
                    OutMtl.AppendLine("Ke 0.000000 0.000000 0.000000")
                    OutMtl.AppendLine("Ns 0.000000")
                    OutMtl.AppendLine("map_Kd " & MyOhana.Model_Texture_Index(.Texture_ID) & ".png")

                    File.WriteAllText(SaveDlg.FileName & ".mtl", OutMtl.ToString)
                End With
            End If
        Else
            MessageBox.Show("You must select an object first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Private Sub BtnExportFace_Click(sender As Object, e As EventArgs) Handles BtnExportFace.Click
        If LstFaces.SelectedIndex > -1 Then
            Dim SaveDlg As New SaveFileDialog
            SaveDlg.Filter = "Wavefront OBJ|*.obj"
            If SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim Out As New StringBuilder
                Dim Info As New NumberFormatInfo
                Info.NumberDecimalSeparator = "."
                Info.NumberDecimalDigits = 6

                Out.AppendLine("mtllib " & Path.GetFileName(SaveDlg.FileName) & ".mtl")

                With MyOhana.Model_Object(MyOhana.Selected_Object)
                    Dim Temp As List(Of Integer) = .Per_Face_Index(LstFaces.SelectedIndex).ToList
                    Dim Vertex_Remap(.Vertice.Length - 1) As Integer
                    Dim Count As Integer
                    For i As Integer = 0 To .Vertice.Length - 1
                        If Temp.IndexOf(i) > -1 Then 'Verifica se o vértice é de fato usado na face
                            Out.AppendLine("v " & (.Vertice(i).X * MyOhana.Load_Scale).ToString("N", Info) & " " & (.Vertice(i).Y * MyOhana.Load_Scale).ToString("N", Info) & " " & (.Vertice(i).Z * MyOhana.Load_Scale).ToString("N", Info))
                            Out.AppendLine("vn " & (.Vertice(i).NX * MyOhana.Load_Scale).ToString("N", Info) & " " & (.Vertice(i).NY * MyOhana.Load_Scale).ToString("N", Info) & " " & (.Vertice(i).NZ * MyOhana.Load_Scale).ToString("N", Info))
                            Out.AppendLine("vt " & .Vertice(i).U.ToString("N", Info) & " " & .Vertice(i).V.ToString("N", Info))
                            Vertex_Remap(i) = Count
                            Count += 1
                        End If
                    Next

                    Out.AppendLine("usemtl " & MyOhana.Model_Texture_Index(.Texture_ID))

                    For i As Integer = 0 To .Per_Face_Index(LstFaces.SelectedIndex).Length - 1 Step 3
                        Dim a As String = (Vertex_Remap(.Per_Face_Index(LstFaces.SelectedIndex)(i)) + 1).ToString()
                        Dim b As String = (Vertex_Remap(.Per_Face_Index(LstFaces.SelectedIndex)(i + 1)) + 1).ToString()
                        Dim c As String = (Vertex_Remap(.Per_Face_Index(LstFaces.SelectedIndex)(i + 2)) + 1).ToString()

                        Out.AppendLine("f " & a & "/" & a & "/" & a & " " & b & "/" & b & "/" & b & " " & c & "/" & c & "/" & c)
                    Next

                    File.WriteAllText(SaveDlg.FileName, Out.ToString)

                    Dim OutMtl As New StringBuilder
                    OutMtl.AppendLine("newmtl " & MyOhana.Model_Texture_Index(.Texture_ID))
                    OutMtl.AppendLine("illum 2")
                    OutMtl.AppendLine("Kd 0.800000 0.800000 0.800000")
                    OutMtl.AppendLine("Ka 0.200000 0.200000 0.200000")
                    OutMtl.AppendLine("Ks 0.000000 0.000000 0.000000")
                    OutMtl.AppendLine("Ke 0.000000 0.000000 0.000000")
                    OutMtl.AppendLine("Ns 0.000000")
                    OutMtl.AppendLine("map_Kd " & MyOhana.Model_Texture_Index(.Texture_ID) & ".png")

                    File.WriteAllText(SaveDlg.FileName & ".mtl", OutMtl.ToString)
                End With
            End If
        Else
            MessageBox.Show("You must select an face first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        If MyOhana.Selected_Face > -1 Then
            Dim Data() As Byte = File.ReadAllBytes(MyOhana.Temp_Model_File)
            With MyOhana.Model_Object(MyOhana.Selected_Object)
                Dim Current_Face_Offset As Integer = .Per_Face_Entry(MyOhana.Selected_Face).Offset
                Dim Face_Length As Integer = .Per_Face_Entry(MyOhana.Selected_Face).Length

                For i As Integer = Current_Face_Offset To (Current_Face_Offset + Face_Length)
                    Data(i) = 0
                Next

                Dim j As Integer
                For k As Integer = 0 To MyOhana.Selected_Face - 1
                    j += .Per_Face_Entry(k).Length \ .Per_Face_Entry(k).Format
                Next
                For g As Integer = 0 To (Face_Length \ .Per_Face_Entry(MyOhana.Selected_Face).Format) - 1
                    .Index(j) = 0
                    .Per_Face_Index(MyOhana.Selected_Face)(g) = 0
                    j += 1
                Next
            End With
            File.WriteAllBytes(MyOhana.Temp_Model_File, Data)
        Else
            MessageBox.Show("You must select an face first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub TxtTexID_TextChanged(sender As Object, e As EventArgs) Handles TxtTexID.TextChanged
        If LstObjects.SelectedIndex > -1 Then
            Dim TexID As Integer = Int32.Parse(TxtTexID.Text)
            If TexID < MyOhana.Model_Texture_Index.Length Then
                With MyOhana.Model_Object(MyOhana.Selected_Object)
                    .Texture_ID = TexID
                    Dim Data() As Byte = File.ReadAllBytes(MyOhana.Temp_Model_File)
                    Data(.Texture_ID_Offset) = Convert.ToByte(TexID And &HFF)
                    File.WriteAllBytes(MyOhana.Temp_Model_File, Data)
                End With
            End If
        End If
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
    Private Sub Button_MouseLeave(sender As Object, e As EventArgs) Handles BtnClose.MouseLeave, BtnMinimize.MouseLeave
        Dim Lbl As Label = CType(sender, Label)
        Lbl.BackColor = Color.Transparent
        Lbl.ForeColor = Color.White
    End Sub
#End Region

End Class