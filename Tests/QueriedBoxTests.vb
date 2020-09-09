Imports ControlLibrary
Public Class QueriedBoxTests
    Private Sub QueriedBoxTexts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        QueriedBox.DefaultDbProvider = "System.Data.SQLite"

        QueriedBox.DefaultConnectionString = "Data Source=C:\Users\usuario\Documents\GitHub\Evaluation\Evaluation\bin\Debug\Data\data.db;Version=3;UTF8Encoding=True;foreign keys=true;"
    End Sub

    Private Sub QueriedBox2_HyperlinkClicked(sender As Object, e As EventArgs) Handles QueriedBox2.HyperlinkClicked
        MsgBox(QueriedBox2.FreezedPrimaryKey)
    End Sub

    Private Sub QueriedBox1_HyperlinkClicked(sender As Object, e As EventArgs) Handles QueriedBox1.HyperlinkClicked
        MsgBox(QueriedBox1.FreezedPrimaryKey)
    End Sub
End Class