Imports System.ComponentModel

Public Class Item

    Public Property ID As Long
    <DisplayName("Nome")>
    Public Property Name As String
    Public Property NCM As String
    <DisplayName("Unidade")>
    Public Property Unity As String
    <DisplayName("Valor")>
    Public Property Value As Decimal
    <DisplayName("Data de Criação")>
    Public Property CreationDate As Date
    <DisplayName("Está Ativo?")>
    Public Property Active As Boolean
    <DisplayName("Avaliação")>
    Public Property Evaluation As ItemEvaluation
End Class

<DisplayName("Avaliação do Item")>
Public Class ItemEvaluation
    Public Property ID As Long
    <DisplayName("Nota")>
    Public Property Rate As Integer
End Class
