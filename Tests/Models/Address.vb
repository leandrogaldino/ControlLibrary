Imports ControlLibrary.FilterBuilder2.Model.Attributes
<TableAlias("Endereço2")>
Public Class Address
    Public Property ID As Long
    <ColumnAlias("Rua")>
    Public Property Street As String
    <ColumnAlias("Estado")>
    Public Property State As String
End Class
