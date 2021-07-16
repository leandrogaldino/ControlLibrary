<ControlLibrary.FilterBuilder.Model.Hide("Compressor")>
<ControlLibrary.FilterBuilder.Model.Hide("Address.Street")>
Public Class Person
    Public Property ID As Long
    Public Property Compressor As Compressor
    Public Property Lista As New List(Of Address)
    Public Property p As New Person

    precisa tratar como e feitas As colecoes customizadas derivadas de collection e usar o attributo acima.
End Class
