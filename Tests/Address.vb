Imports ControlLibrary.FilterBuilder.Model
Imports System.ComponentModel

<DisplayName("Endereço")>
Public Class Address
    Public Property ID As Long
    <DisplayName("Rua")>
    Public Property Street As String
    Public Property City As String
    Public Property State As String

End Class
