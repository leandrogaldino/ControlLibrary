Imports ControlLibrary

Public Class Tests
    Private Filter As FilterBuilder
    Private Person As New Person
    Private Where As FilterBuilder.Model.WhereClause
    Private Sub QueriedBoxTests_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        Filter = New FilterBuilder(GetType(Order))



        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.RelatedTables(0).Columns(0)
        'Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.ID")
        Where.ComparsionOperator.Display = "Igual"
        Where.ComparsionOperator.Value = "="
        Where.LogicalOperator.Display = "e"
        Where.LogicalOperator.Value = "AND"
        Where.Parameter.Value = "[ID]"
        Filter.Wheres.Add(Where)
        Dim r = Filter.GetResult
        Debug.Print(r.CommandText)
        Filter.ShowDialog()



        'Where = New FilterBuilder.Model.WhereClause
        'Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.IsCustomer")
        'Where.ComparsionOperator.Display = "Igual"
        'Where.ComparsionOperator.Value = "="
        'Where.LogicalOperator.Display = "e"
        'Where.LogicalOperator.Value = "AND"
        'Where.Parameter.Value = "[Cliente]"
        'Filter.Wheres.Add(Where)
        'Where = New FilterBuilder.Model.WhereClause
        'Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.Name")
        'Where.ComparsionOperator.Display = "Contem"
        'Where.ComparsionOperator.Value = "LIKE"
        'Where.Parameter.Value = "[Digite um nome]"
        'Where.LogicalOperator.Display = "e"
        'Where.LogicalOperator.Value = "AND"
        'Filter.Wheres.Add(Where)
        'Where = New FilterBuilder.Model.WhereClause
        'Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.ID")
        'Where.ComparsionOperator.Display = "Entre"
        'Where.ComparsionOperator.Value = "BETWEEN"
        'Where.Parameter.Value = "[ID Inicial]"
        'Where.Parameter2.Value = "[ID Final]"
        'Filter.Wheres.Add(Where)
        'Where = New FilterBuilder.Model.WhereClause
        'Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.Birth")
        'Where.ComparsionOperator.Display = "Entre"
        'Where.ComparsionOperator.Value = "BETWEEN"
        'Where.Parameter.Value = "[Data Inicial]"
        'Where.Parameter2.Value = "[Data Final]"
        'Filter.Wheres.Add(Where)
        'Filter.FreeWhereClause = "Person.ID = 10"
        'Filter.FilterName = "Filtro Teste"
        'Dim nome As String = Now.ToString("ddMMyyyyhhmmssfff") & ".xml"
        'Filter.Save(nome)
        'Filter = New FilterBuilder(nome)
        'Dim r = Filter.GetResult
        'MsgBox(r.CommandText)
    End Sub


End Class

