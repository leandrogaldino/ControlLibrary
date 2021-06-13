Imports System.ComponentModel
Imports ControlLibrary.FilterBuilder.Model

<DisplayName("Pessoa")>
Public Class Person
    Public Property ID As Long
    <DisplayName("Nome")>
    Public Property Name As String
    <DisplayName("Aniversário")>
    Public Property Birth As Date
    <DisplayName("Compressoru")>
    Public Property Compressor As Compressor
End Class
