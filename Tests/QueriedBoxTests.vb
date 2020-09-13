Imports ControlLibrary
Public Class QueriedBoxTests
    Private Sub QueriedBoxTexts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        QueriedBox.DefaultDbProvider = "System.Data.SQLite"

        QueriedBox.DefaultConnectionString = "Data Source=C:\Users\leand\source\repos\GitHub\Evaluation\Evaluation\bin\Debug\Data\data.db;Version=3;UTF8Encoding=True;foreign keys=true;"


        QueriedBox2.Freeze(3, True)
    End Sub

    Private Sub QueriedBox2_FreezedPrimaryKeyChanged(sender As Object, e As EventArgs) Handles QueriedBox2.FreezedPrimaryKeyChanged
        Debug.Print(QueriedBox2.FreezedPrimaryKey)
    End Sub
End Class