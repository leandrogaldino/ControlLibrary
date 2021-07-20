<ControlLibrary.FilterBuilder2.Model.Attributes.TableAlias("Ordem de Compra")>
Public Class Order
    Public Property ID As Long
    <ControlLibrary.FilterBuilder2.Model.Attributes.ColumnAlias("Cliente")>
    <ControlLibrary.FilterBuilder2.Model.Attributes.ShowColumn("Name", "Nome")>
    Public Property Customer As Person
    <ControlLibrary.FilterBuilder2.Model.Attributes.ColumnAlias("Fornecedor")>
    Public Property Provider As Person
    <ControlLibrary.FilterBuilder2.Model.Attributes.ColumnAlias("Produtos")>
    Public Property Item As Item
    <ControlLibrary.FilterBuilder2.Model.Attributes.ColumnAlias("Serviços")>
    Public Property Service As Item
    <ControlLibrary.FilterBuilder2.Model.Attributes.ColumnAlias("Está Ativo?")>
    Public Property IsActive As Boolean
    Public Property CreationDate As Date
End Class
