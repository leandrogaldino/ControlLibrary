Public Class Person
    <ControlLibrary.FilterBuilder2.Model.Attributes.ColumnAlias("ID da Pessoa")>
    Public Property PersonID As Long
    Public Property PersonName As String
    Public Property Address As Address
    Public Property IsCustomer As Boolean
    Public Property IsProvider As Boolean
    Public Property IsShipper As Boolean
End Class
