Imports ControlLibrary

Public Class QueriedBoxTests
    Private f As FilterBuilder
    Private Sub QueriedBoxTests_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim p As New Person
        f = New FilterBuilder(p)
        f.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MsgBox(f.GetSelectCommand)
        f.ExecuteQuery()
    End Sub
End Class