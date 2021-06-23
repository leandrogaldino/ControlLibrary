Imports System.ComponentModel
Imports ControlLibrary.FilterBuilder.Model

<DisplayName("Pessoa")>
Public Class Person
    Public Property ID As Long
    <DisplayName("Nome")>
    Public Property Name As String
    <DisplayName("Aniversário")>
    Public Property Birth As Date
    <DisplayName("Compressor")>
    Public Property Compressor As Compressor
    Public Property Altura As Decimal
    Public Property IsCustomer As Boolean
End Class
