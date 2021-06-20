Imports ControlLibrary
Public Class QueriedBoxTests
    Private Filter As FilterBuilder
    Private Person As New Person
    Private Sub QueriedBoxTests_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Where As FilterBuilder.Model.WhereClause
        Filter = New FilterBuilder(Person)
        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.Altura")
        Where.ComparsionOperator.Display = "Entre"
        Where.ComparsionOperator.Value = "BETWEEN"
        Where.LogicalOperator.Display = "e"
        Where.LogicalOperator.Value = "AND"
        Where.Parameter.Value = "[Data de Nascimento Inicial]"
        Where.Parameter2.Value = "[Data de Nascimento Final]"

        Filter.Wheres.Add(Where)

        Dim r = Filter.GetResult
        If r IsNot Nothing Then
            MsgBox(r.CommandText)
        End If

        'Application.Exit()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub
End Class