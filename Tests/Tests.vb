Imports ControlLibrary
Imports System.Xml
Public Class Tests
    Private Filter As FilterBuilder
    Private Person As New Person
    Private Sub QueriedBoxTests_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'criar xml
        'Dim Node As XmlNode
        Dim doc As New XmlDocument
        Dim Where As FilterBuilder.Model.WhereClause

        'Node = doc.CreateNode("NODE_TEXT", "name", "namespaceuri")
        'doc.CreateElement("name")
        'doc.Save("C:\Users\faturamento\Desktop")
        Filter = New FilterBuilder(New Person)
        'MsgBox(Filter.MainTable.Columns(0).Name)

        'Filter.Save("t.xml")

        'Dim writer As New XmlTextWriter("teste.xml", Nothing)
        'writer.Formatting = Formatting.Indented
        'writer.WriteStartElement("Filter")
        'writer.WriteElementString("Object", "Tests.Person")
        'writer.WriteElementString("FilterName", "nome do filtro")
        'writer.WriteElementString("FreeWhere", "clausula where completa")
        'writer.WriteStartElement("Wheres")
        'writer.WriteElementString("Column", "nome do objeto + nome da coluna")
        'writer.WriteElementString("Operator", "nome do operador")
        'writer.WriteElementString("Operato2", "nome do operador2")
        'writer.WriteElementString("Value", "o primeiro valor")
        'writer.WriteElementString("Value2", "o segundo valor")
        'writer.WriteEndElement()
        'writer.WriteEndElement()
        'writer.Close()


        'Carregar o objeto com um xml
        'doc = New Xml.XmlDocument
        'doc.Load("teste.xml")
        'Where = New FilterBuilder.Model.WhereClause
        'Dim n =doc.SelectNodes("Filter/Object").Item(0).InnerText
        'Filter = New FilterBuilder(Activator.CreateInstance(Type.GetType(n)))
        'Filter.FilterName = doc.SelectNodes("Filter/FilterName").Item(0).InnerText
        'Filter.FreeWhereClause = doc.SelectNodes("Filter/FreeWhere").Item(0).InnerText


        'For Each Element As Xml.XmlElement In doc.SelectNodes("Filter/Wheres/Where")

        '    Where = New FilterBuilder.Model.WhereClause
        '    Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = Element("Column").InnerText)

        '    Where.ComparsionOperator.Value = Element("Operator").InnerText

        '    Where.LogicalOperator.Value = Element("Operator2").InnerText
        '    Where.Parameter.Value = Element("Value1").InnerText
        '    Where.Parameter2.Value = Element("Value2").InnerText



        '    Filter.Wheres.Add(Where)


        'Next



        'Filter.ShowDialog()
        'Dim r = Filter.GetResult

        'Dim c As New SQLite.SQLiteCommand
        'c.CommandText = r.CommandText
        'r.Parameters.ForEach(Sub(x) c.Parameters.AddWithValue(x.Name, x.Value))

        'Utility.DebugQuery(c)

        'Application.Exit()

        'If Filter.ShowDialog = FilterBuilder.FilterDialogResult.Execute Then

        'End If



        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.IsCustomer")
        Where.ComparsionOperator.Display = "Igual"
        Where.ComparsionOperator.Value = "="
        Where.LogicalOperator.Display = "e"
        Where.LogicalOperator.Value = "AND"
        Where.Parameter.Value = "[Cliente]"
        Filter.Wheres.Add(Where)

        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.Name")
        Where.ComparsionOperator.Display = "Contem"
        Where.ComparsionOperator.Value = "LIKE"
        Where.Parameter.Value = "[Digite um nome]"
        Where.LogicalOperator.Display = "e"
        Where.LogicalOperator.Value = "AND"
        Filter.Wheres.Add(Where)

        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.ID")
        Where.ComparsionOperator.Display = "Entre"
        Where.ComparsionOperator.Value = "BETWEEN"
        Where.Parameter.Value = "[ID Inicial]"
        Where.Parameter2.Value = "[ID Final]"
        Filter.Wheres.Add(Where)

        Where = New FilterBuilder.Model.WhereClause
        Where.Column = Filter.MainTable.Columns.Find(Function(x) x.Name = "Person.Birth")
        Where.ComparsionOperator.Display = "Entre"
        Where.ComparsionOperator.Value = "BETWEEN"
        Where.Parameter.Value = "[Data Inicial]"
        Where.Parameter2.Value = "[Data Final]"
        Filter.Wheres.Add(Where)


        Filter.FreeWhereClause = "Person.ID = 10"

        Filter.FilterName = "Filtro Teste"

        Filter.Save(Now.ToString("ddMMyyyyhhmmssfff") & ".xml")

        'Dim r = Filter.GetResult
        'If r IsNot Nothing Then
        'MsgBox(r.CommandText)
        'End If


        If Filter.ShowDialog = FilterBuilder.FilterDialogResult.Save Then
            Filter.Save()
        End If

        'Application.Exit()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MsgBox(Tests2.ShowDialog().ToString)
    End Sub
End Class

