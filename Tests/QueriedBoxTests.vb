Imports ControlLibrary
Public Class QueriedBoxTests
    Private Filter As FilterBuilder
    Private Person As New Person
    Private Sub QueriedBoxTests_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Where As FilterBuilder.Model.WhereClause
        Filter = New FilterBuilder(Person)
        Filter.FilterName = "Filtro Personalizado"

        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.ID")
        Where.ComparsionOperator.Display = "Entre"
        Where.ComparsionOperator.Value = "BETWEEN"
        Where.LogicalOperator.Display = "e"
        Where.LogicalOperator.Value = "AND"
        Where.Parameter.Value = "[Valor Inicial]"
        Where.Parameter2.Value = "[Valor Final]"
        Filter.Wheres.Add(Where)

        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.Name")
        Where.ComparsionOperator.Display = "Contem"
        Where.ComparsionOperator.Value = "LIKE"
        Where.LogicalOperator.Display = "e"
        Where.LogicalOperator.Value = "AND"
        Where.Parameter.Value = "Leandro"

        Filter.Wheres.Add(Where)
        Dim r = Filter.GetResult
        If r IsNot Nothing Then
            MsgBox(r.CommandText)
        End If

        'Application.Exit()
    End Sub

End Class