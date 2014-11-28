Public Class MyWindowTitle
    Inherits Label
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        Select Case m.Msg
            Case &H84 : m.Result = New IntPtr(-1)
        End Select
    End Sub
End Class
