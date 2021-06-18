Imports ControlLibrary
Public Class QueriedBoxTests
    Private Filter As FilterBuilder
    Private Person As New Person
    Private Sub QueriedBoxTests_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Where As FilterBuilder.Model.WhereClause
        Filter = New FilterBuilder(Person)
        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.MainTable.Columns(0)
        Where.ComparsionOperator.Display = "Igual"
        Where.ComparsionOperator.Value = "="
        Where.LogicalOperator.Display = "e"
        Where.LogicalOperator.Value = "AND"
        Where.Value = "[Digite o ID]"

        Filter.Wheres.Add(Where)


        Utility.DebugQuery(Filter.GetCommandOBSOLETE(New SQLite.SQLiteConnection("")))

    End Sub
End Class