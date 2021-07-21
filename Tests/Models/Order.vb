Imports ControlLibrary.FilterBuilder2.Model.Attributes
<TableAlias("Ordem de Compra")>
Public Class Order
    <HideColumnInFilter()>
    Public Property ID As Long
    <ColumnAlias("Cliente")> <HideColumnInFilter({"IsCustomer", "IsProvider", "IsShipper"})>
    Public Property Customer As Person
    <ColumnAlias("Fornecedor")> <HideColumnInFilter({"IsCustomer", "IsProvider", "IsShipper"})>
    Public Property Provider As Person
    <ColumnAlias("Transportadora")> <HideColumnInFilter({"IsCustomer", "IsProvider", "IsShipper"})>
    Public Property Shipper As Person
    <ColumnAlias("Produtos")>
    Public Property Item As Item
    <ColumnAlias("Serviços")>
    Public Property Service As Item
    <ColumnAlias("Está Ativo?")>
    Public Property IsActive As Boolean
    Public Property CreationDate As Date

    Public Property Address As Address
End Class
