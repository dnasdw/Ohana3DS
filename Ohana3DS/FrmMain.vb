Imports System.IO
Public Class FrmMain
    Dim Rot_InitX, Rot_InitY, Rot_FinalX, Rot_FinalY As Integer
    Dim Mov_InitX, Mov_InitY, Mov_FinalX, Mov_FinalY As Integer
    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MsgBox(2 ^ (7 - 6))
        'Nako.Extract("D:\Downloads\Decryptor\Decryptor\workdir\decrypted_RomFS\a\0\0\7", "D:\TESTE\")
        'File.WriteAllBytes("d:\decomp.bin", Nako.LZSS_Decompress(File.ReadAllBytes("D:\TESTE\file_10.bin")))
        'Nako.Extract("D:\Downloads\Decryptor\Decryptor\workdir\decrypted_RomFS\a\0\4\1", "D:\MAPASPOKE\")
        'Nako.Extract("D:\Downloads\Decryptor\Decryptor\workdir\decrypted_RomFS\a\0\1\3", "D:\DUMP\")
        'Nako.Extract("D:\Downloads\Decryptor\Decryptor\workdir\decrypted_RomFS\a\1\6\5", "D:\DUMP2\")
        'Nako.Extract("D:\Downloads\Decryptor\Decryptor\workdir\decrypted_RomFS\a\1\4\0", "D:\Team Flare\")
        'Nako.Extract("D:\Downloads\Decryptor\Decryptor\workdir\decrypted_RomFS\a\0\8\9", "D:\Player\")
        Ohana.Initialize(Screen.Handle)
        'Ohana.Load_Textures("D:\DUMP\file_3.bin")
        'Ohana.Load_Model("D:\MAPASPOKE\file_20.bin")
        'Ohana.Load_Textures(Application.StartupPath & "\texture.bin")
        'Ohana.Load_Textures("D:\DUMP\file_7.bin")
        'Ohana.Load_Model("D:\MAPASPOKE\file_83.bin")
        'Ohana.Load_Textures("D:\Team Flare\file_4.bin")
        Ohana.Load_Model("D:\Player\file_525.bch")
        'Ohana.Load_Model(Application.StartupPath & "\model.bin")
        'Ohana.Load_Textures("D:\dec_7798.pt")
        'Ohana.Load_Model("D:\dec_7796.pc")
        'Ohana.Load_Textures("D:\TESTE\file_7838_texture.pt")
        'Ohana.Load_Model("D:\TESTE\file_7836_model.pc")
        Show()
        Ohana.Render()
    End Sub
    Private Sub PicMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseDown
        If e.Button = MouseButtons.Left Then
            Rot_InitX = MousePosition.X
            Rot_InitY = MousePosition.Y
        ElseIf e.Button = MouseButtons.Right Then
            Mov_InitX = MousePosition.X
            Mov_InitY = MousePosition.Y
        End If
    End Sub
    Private Sub PicMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseUp
        If e.Button = MouseButtons.Left Then
            Rot_FinalX += (Rot_InitX - MousePosition.X)
            Rot_FinalY += (Rot_InitY - MousePosition.Y)
        ElseIf e.Button = MouseButtons.Right Then
            Mov_FinalX += (Mov_InitX - MousePosition.X)
            Mov_FinalY += (Mov_InitY - MousePosition.Y)
        End If
    End Sub
    Private Sub PicMouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseMove
        If e.Button = MouseButtons.Left Then
            Ohana.RotX = (Rot_InitX - MousePosition.X) + Rot_FinalX
            Ohana.RotY = (Rot_InitY - MousePosition.Y) + Rot_FinalY
        ElseIf e.Button = MouseButtons.Right Then
            Ohana.MoveX = (Mov_InitX - MousePosition.X) + Mov_FinalX
            Ohana.MoveY = (Mov_InitY - MousePosition.Y) + Mov_FinalY
        End If
    End Sub
    Private Sub PicZoom(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If e.Delta > 0 Then
            Ohana.Zoom += 1.5F
        Else
            Ohana.Zoom -= 1.5F
        End If
    End Sub
End Class
