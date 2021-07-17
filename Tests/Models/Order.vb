Imports System.ComponentModel
Imports ControlLibrary.FilterBuilder.Model
<TableAlias("Ordem")>
<FieldAlias("Order, ID, Código")> <FieldAlias("Person, Name, Nome")> <FieldAlias("Address, Street, Rua")> <FieldAlias("Address, State, Estado")>
<HideField("Person, ID")> <HideField("Address, ID")>
Public Class Order

    Public Property ID As Long
    <DisplayName("ABC")>
    Public Property Customer As Person
    Public Property Items As List(Of OrderItem)
End Class
